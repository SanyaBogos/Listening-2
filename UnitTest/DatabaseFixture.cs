using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ListeningTest
{
    public class DatabaseFixture : IDisposable 
    {
        //private ICollectionFixture<T> _collection;

        public DatabaseFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            Db = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public SqlConnection Db { get; private set; }
    }
}
