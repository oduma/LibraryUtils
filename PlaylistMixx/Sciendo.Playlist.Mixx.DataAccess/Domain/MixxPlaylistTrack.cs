using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Sciendo.Mixx.DataAccess.Domain
{
    public class MixxPlaylistTrack
    {
        public string FileName { get; private set; }
        public int Position { get; private set; }
        public int TrackId { get; set; }

        public MixxPlaylistTrack(string fileName, int position)
        {
            FileName = fileName;
            Position = position;
        }

        public bool Save(int playlistId, IDbConnection connection, IDbTransaction transaction)
        {
            var playlistTrackId = connection.Execute(@"INSERT INTO PlaylistTracks (
                               playlist_id,
                               track_id,
                               position)
                           VALUES (
                               @PlaylistId,
                               @TrackId,
                               @PositionId);", new[] {playlistId,TrackId,Position}, transaction);
            if (playlistTrackId <= 0)
                return false;
            return true;
        }
    }
}
