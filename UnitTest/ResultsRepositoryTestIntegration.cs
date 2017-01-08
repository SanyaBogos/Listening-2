using FluentAssertions;
using listening.Models;
using listening.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using System.IO;

namespace ListeningTest
{
    public class ResultsRepositoryTestIntegration //: IAssemblyFixture<DatabaseFixture> //DatabaseFixture<ResultRepository>
    {
        private ResultRepository _resultRepository;
        private List<Result> _results;

        public ResultsRepositoryTestIntegration()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var appSettingsPath = Path.Combine(currentDir, "../Listening-2/appsettings.json");
            var builder = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            _resultRepository = new ResultRepository(configuration);

            GenerateData();
        }

        [Fact]
        public void CheckCRUD()
        {
            foreach (var result in _results)
                _resultRepository.Insert(result);

            foreach (var result in _results)
                _resultRepository.GetById(result.Id).Should().Be(result);

            foreach (var result in _results)
                _resultRepository.Delete(result.Id);
        }

        private void GenerateData()
        {
            _results = new List<Result>();
            var resultsEncodedString = new bool[] { false, true, false, true };
            for (int i = 0; i < 3; i++)
                _results.Add(new Result
                {
                    Id = int.MaxValue - i - 1,
                    UserId = Guid.NewGuid().ToString(),
                    TextId = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 25),
                    Started = DateTime.Now.AddDays(-13).ToString("yyyy-MM-dd HH:mm:ss"),
                    Finished = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ResultsEncodedString = resultsEncodedString
                });
        }
    }
}
