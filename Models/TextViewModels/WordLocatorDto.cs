using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Models.TextViewModels
{
    public class WordLocatorDto
    {
        public int ParagraphIndex { get; set; }
        public int WordIndex { get; set; }
        public bool IsCapital { get; set; }
    }
}
