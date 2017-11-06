using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace Sciendo.Mixx.DataAccess.Domain
{
    public class MixxPlaylist
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        public IEnumerable<MixxPlaylistTrack> PlaylistTracks { get; set; }

        public MixxPlaylist(string name)
        {
            Name = name;
        }

        public static MixxPlaylist Load(string name, bool deep, IDbConnection connection, IDbTransaction transaction)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            var result = connection.QuerySingle<MixxPlaylist>("SELECT Id, Name FROM Playlists WHERE Name=@name",name,transaction);
            if (result.Id != 0 && deep)
                result.PlaylistTracks =
                    connection.Query<MixxPlaylistTrack>(@"SELECT pt.track_id, pt.position, tl.location
  FROM PlaylistTracks pt join library l on pt.track_id=l.id
  join track_locations tl on l.location=tl.id
  where playlist_id=@Id
  order by position asc", result.Id, transaction);
            return result;
        }

        internal bool Save(IDbConnection connection, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            Id = connection.Execute("INSERT INTO Playlists VALUES(@Name)", Name, transaction);
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
            var result = connection.Execute("DELETE FROM PlaylistTracks WHERE Playlist_Id=@Id", Id, transaction);
            if (result <= 0)
            {
                return false;
            }
            result = connection.Execute("DELETE From Playlists where id=@Id", Id, transaction);
            if (result <= 0)
                return false;
            return true;
        }
    }
}
