using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Models.TextViewModels
{
    public class CorrectWordLocatorsDto
    {
        public string Word { get; set; }
        public WordLocatorDto[] Locators { get; set; }
    }
}
