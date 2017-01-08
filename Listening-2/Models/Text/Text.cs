using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Models.Text
{
    public class Text
    {
        [BsonId]
        public ObjectId TextId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string[][] WordsInParagraphs { get; set; }
        public string AudioName { get; set; }
        public int SymbolsCount { get; set; }
    }
}
