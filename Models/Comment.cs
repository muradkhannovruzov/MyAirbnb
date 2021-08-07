using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public int Vote { get; set; }
        public int PublicationID { get; set; }
        public string Text { get; set; }
    }
}
