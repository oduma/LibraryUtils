using System.Data;
using System.Linq;
using Dapper;

namespace Sciendo.Mixx.DataAccess.Domain
{
    public class MixxxPlaylistTrack
    {
        public string FileName { get; private set; }
        public int Position { get; private set; }
        public int TrackId { get; set; }

        public MixxxPlaylistTrack()
        {
            
        }
        public MixxxPlaylistTrack(string fileName, int position)
        {
            FileName = fileName;
            Position = position;
        }

        public bool Save(int playlistId, IDbConnection connection, IDbTransaction transaction)
        {
            var parameters= new DynamicParameters();
            parameters.Add("@PlaylistId",playlistId,DbType.Int32,ParameterDirection.Input);
            parameters.Add("@TrackId", TrackId,DbType.Int32,ParameterDirection.Input);
            parameters.Add("@PositionId",Position,DbType.Int32,ParameterDirection.Input);
            var playlistTrackId = connection.Query<int>(@"INSERT INTO PlaylistTracks (
                               playlist_id,
                               track_id,
                               position)
                           VALUES (
                               @PlaylistId,
                               @TrackId,
                               @PositionId); select last_insert_rowid()", parameters, transaction).First();
            if (playlistTrackId <= 0)
                return false;
            return true;
        }
    }
}
