using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Sciendo.Mixx.DataAccess.Configuration;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;

namespace Sciendo.Mixx.DataAccess
{
    public class DataHandler:IDataHandler
    {
        private readonly IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>> _mapper;
        private SQLiteConnection _connection;

        public DataHandler(IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>> mapper)
        {
            MixxxConfigurationSection mixxxConfig = ConfigurationManager.GetSection("mixxx") as MixxxConfigurationSection;

            if(mixxxConfig==null)
                throw new ConfigurationErrorsException("Empty or missing mixxx section.");
            _connection = new SQLiteConnection {ConnectionString = $"Data Source={mixxxConfig.MixxxDatabaseFile};version=3;" };
            _connection.Open();

            _mapper = mapper;
        }

        public void Dispose()
        {
            if (_connection != null)
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
        }

        public bool Create(string name, IEnumerable<PlaylistItem> playlistItems)
        {

            var mixxPlaylist = new MixxxPlaylist(name) {PlaylistTracks = _mapper.Transform(playlistItems)};
            using (var transaction = _connection.BeginTransaction())
            {
                var result= mixxPlaylist.Save(_connection, transaction);
                if (result)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public IEnumerable<PlaylistItem> Get(string name)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                var mixxPlaylist = MixxxPlaylist.Load(name, true, _connection, transaction);

                if (mixxPlaylist == null || mixxPlaylist.Id == 0)
                    return null;
                return _mapper.Transform(mixxPlaylist.PlaylistTracks);
            }
        }

        public bool Delete(string name)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                var existingMixxPlaylist = MixxxPlaylist.Load(name, true, _connection, transaction);
                if (existingMixxPlaylist ==null || existingMixxPlaylist.Id <= 0)
                {
                    transaction.Rollback();
                    return false;
                }
                if (existingMixxPlaylist.Delete(_connection,transaction))
                {
                    transaction.Commit();
                    return true;
                }
                transaction.Rollback();
                return false;
            }
        }
    }
}
