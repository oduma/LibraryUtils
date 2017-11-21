using System;
using NUnit.Framework;

namespace Sciendo.Clementine.DataAccess.Tests.Integration
{
    [TestFixture]
    public class DataProviderTests
    {
        private readonly string _dbPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Db\\clementine.db";

        [Test]
        public void CreateADataProviderOk()
        {
            IDataProvider dataProvider= new DataProvider($"Data Source={_dbPath};version=3");
            Assert.IsNotNull(dataProvider);

        }

        [Test]
        public void LoadDataOk()
        {
            using (IDataProvider dataProvider = new DataProvider($"Data Source={_dbPath};version=3"))
            {
                var beforeLoad = DateTime.Now;
                dataProvider.Load();
                Assert.GreaterOrEqual(dataProvider.LastRefresh,beforeLoad);
                Assert.GreaterOrEqual(dataProvider.AllTracks.Length,1);
            }    
        }
    }
}
