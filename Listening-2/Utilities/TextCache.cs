using listening.Models;
using listening.Models.TextViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Utilities
{
    public class TextCache : TextDto, IIdenticable<string>
    {
        public string[][] WordsInParagraphs { get; set; }
        public string[][] CountsInParagraphs { get; set; }

        // wrapped to make possibility of generic cache 
        // (probably renaming (TextId->Id) would be better idea, but we should be ensured, 
        // that all front-end endpoints works correct after change)
        public string Id
        {
            get { return TextId; }
            set { TextId = value; }
        }
    }
}
