using listening.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
// using Microsoft.Data.Sqlite;

namespace listening.Repositories
{
    public class ResultRepository : IRepository<Result, int>
    {
        private string _connectionString;

        public ResultRepository(IConfigurationRoot configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public void Delete(int itemId)
        {
            // throw new NotImplementedException();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    "delete from Results where Id=@Id", new { Id = itemId });
            }
        }

        public IQueryable<Result> GetAll()
        {
            throw new NotImplementedException();
        }

        public Result GetById(int id)
        {
            // throw new NotImplementedException();
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Query<Result>(
                    "select * from Results where Id=@Id", new { Id = id });
                return result.First();
            }
        }

        public void Insert(Result item)
        {
            // throw new NotImplementedException();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute($@"INSERT INTO Results (UserId, TextId,
                           ResultsEncodedString, Started, Finished) 
                       VALUES ('{item.UserId}','{item.TextId}','{item.ResultsEncodedString}',
                           '{item.Started}','{item.Finished}')");
                connection.Execute($@"INSERT INTO Results (UserId, TextId,
                            ResultsEncodedString, Started, Finished) 
                        VALUES (@UserId, @TextId, @ResultsEncodedString, @Started, @Finished)",
                        new{ item.UserId,item.TextId, ResultsEncodedString="x010101" ,
                            item.Started,item.Finished });
            }
        }

        public void Update(Result item)
        {
            // throw new NotImplementedException();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    @"update Results 
                        set UserId=@UserId, TextId=@TextId, 
                            ResultsEncodedString=@ResultsEncodedString,
                            Started=@Started, Finished=@Finished
                        where Id=@Id",
                    item);
            }
        }
    }
}
