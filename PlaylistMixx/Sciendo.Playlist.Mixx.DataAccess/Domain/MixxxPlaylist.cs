using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace Sciendo.Mixx.DataAccess.Domain
{
    public class MixxxPlaylist
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        public IEnumerable<MixxxPlaylistTrack> PlaylistTracks { get; set; }

        public MixxxPlaylist()
        {
            
        }

        public MixxxPlaylist(string name)
        {
            Name = name;
        }

        public static MixxxPlaylist Load(string name, bool deep, IDbConnection connection, IDbTransaction transaction)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            var result =
                connection.Query<MixxxPlaylist>("SELECT Id, Name FROM Playlists", transaction)
                    .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (result != null && result.Id != 0 && deep)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", result.Id,DbType.Int32,ParameterDirection.Input);
                result.PlaylistTracks =
                    connection.Query<MixxxPlaylistTrack>(@"SELECT pt.track_id, pt.position, tl.location
  FROM PlaylistTracks pt join library l on pt.track_id=l.id
  join track_locations tl on l.location=tl.id
  where playlist_id=@Id
  order by position asc", parameters, transaction);
            }
            return result;
        }

        internal bool Save(IDbConnection connection, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            var parameters= new DynamicParameters();
            parameters.Add("@name",Name,DbType.String,ParameterDirection.Input);
            Id= connection.Query<int>("INSERT INTO Playlists(name) VALUES(@Name);select last_insert_rowid()", parameters, transaction).First();
            if (Id <= 0)
                return false;
            var allMixxTracks = connection.Query<MixxTrack>(@"SELECT library.id as TrackId, track_locations.location as FilePath
  FROM track_locations
  left outer join library on track_locations.id = library.location").ToArray();

            foreach (var playlistTrack in PlaylistTracks)
            {
                var mixxTrack =
                    allMixxTracks.FirstOrDefault(
                            t => string.Equals(t.FilePath.Replace(@"/", @"\"), playlistTrack.FileName.Replace(@"/", @"\")));
                if (mixxTrack == null)
                    return false;
                playlistTrack.TrackId = mixxTrack.TrackId;

                if (!playlistTrack.Save(Id, connection, transaction))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Delete(IDbConnection connection, IDbTransaction transaction)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id",Id,DbType.Int32,ParameterDirection.Input);
            var result = connection.Execute("DELETE FROM PlaylistTracks WHERE Playlist_Id=@Id", parameters, transaction);
            if (result <= 0)
            {
                return false;
            }
            result = connection.Execute("DELETE From Playlists where id=@Id", parameters, transaction);
            if (result <= 0)
                return false;
            return true;
        }
    }
}
