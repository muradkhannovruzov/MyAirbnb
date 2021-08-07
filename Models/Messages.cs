using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string Account1Message { get; set; }
        public string Account2Message { get; set; }
        public bool Account1Readed { get; set; }
        public bool Account2Readed { get; set; }
        public string Time { get; set; }
        public string Account1Background { get; set; } = "Pink";
        public string Account2Background { get; set; } = "Green";
    }
}
