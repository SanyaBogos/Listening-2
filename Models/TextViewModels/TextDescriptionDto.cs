using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Models.TextViewModels
{
    public class TextDescriptionDto
    {
        public string TextId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string AudioName { get; set; }
    }
}
