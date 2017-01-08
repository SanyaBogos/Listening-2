using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace listening.Services
{
    public class ResultService
    {
        private const string digitPattern = @"^\d+$";

        public ResultService()
        {

        }

        public void BuildResultString(string[][] wordsCounts, int symbolsCount)
        {
            var resultEncodedString = new bool[symbolsCount * 2];
            int currentPosition = 0;
            int symbolsCountInWord;

            foreach (var paragraph in wordsCounts)
                foreach (var item in paragraph)
                    if (int.TryParse(item, out symbolsCountInWord))
                        currentPosition += symbolsCountInWord * 2;
                    else
                        for (int i = 0; i < item.Length * 2; i += 2, currentPosition += 2)
                            resultEncodedString[currentPosition + 1] = true;


        }
    }
}
