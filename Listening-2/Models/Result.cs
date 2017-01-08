using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Models
{
    public class Result : IIdenticable<int>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TextId { get; set; }
        public bool[] ResultsEncodedString { get; set; }
        public string Started { get; set; }
        public string Finished { get; set; }
    }
}
