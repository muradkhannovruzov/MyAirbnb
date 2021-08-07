using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Messaging
    {
        public int Id { get; set; }
        public int Account1ID { get; set; }
        public int Account2ID { get; set; }
        public string Account1Name { get; set; }
        public string Account2Name { get; set; }
        public virtual List<Messages> Messages { get; set; } = new List<Messages>();
    }
}
