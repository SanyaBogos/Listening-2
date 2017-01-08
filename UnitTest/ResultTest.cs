using listening.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ListeningTest
{
    public class ResultTest
    {
        private ResultService sut;

        public ResultTest()
        {
            sut = new ResultService();
        }

        public class IndexOfData : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {
                    new string[][] {
                        new string[] { "1", "2", "3" },
                        new string[] { "1", "!?", "3" },
                        new string[] { "1", ".", "3" },
                        new string[] { "1", "...", "3" },
                        new string[] { "4", "5", "6" }
                    }
                },
            };

            public IEnumerator<object[]> GetEnumerator()
            { return _data.GetEnumerator(); }

            IEnumerator IEnumerable.GetEnumerator()
            { return GetEnumerator(); }
        }

        [Theory, ClassData(typeof(IndexOfData))]
        public void ShouldBuildResultString(string[][] wordsCounts)
        {
            var counts = Count(wordsCounts);
            sut.BuildResultString(wordsCounts, counts);

            Assert.Equal(true, false);
        }

        private int Count(string[][] wordsCounts)
        {
            int count = 0, x;

            foreach (var paragrapth in wordsCounts)
                foreach (var word in paragrapth)
                    if (int.TryParse(word, out x))
                        count += x;
                    else
                        count += word.Length;

            return count;
        }
    }
}
