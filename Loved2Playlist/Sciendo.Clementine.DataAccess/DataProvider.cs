using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sciendo.Clementine.DataAccess.DataTypes;

namespace Sciendo.Clementine.DataAccess
{
    public class DataProvider:IDataProvider
    {
        private IDbConnection _connection;
        private IEnumerable<ClementineTrack> _tracks;
        public DataProvider(string connectionString)
        {
            _connection=new SQLiteConnection();
            _connection.ConnectionString = connectionString;
            _connection.Open();
        }

        public void Dispose()
        {
            if(_connection!=null)
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
        }

        public void Load()
        {
            _tracks = _connection.Query<ClementineTrack>(@"SELECT title,
       artist,
       albumartist,
       filename
  FROM songs;");
            LastRefresh=DateTime.Now;
        }

        public DateTime LastRefresh { get; private set; }
        public ClementineTrack[] AllTracks => _tracks.ToArray();
    }
}
