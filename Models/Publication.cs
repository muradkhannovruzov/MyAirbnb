using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Publication
    {
        private double rating;

        public int Id { get; set; }
        public virtual Home Home { get; set; }
        public int AccountId { get; set; }
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual List<Reservation> Reservations { get; set; }
        public int Review { get; set; } = 0;
        public double Rating
        {
            get
            {
                if (Comments.Count == 0) return -1;
                return Comments.Average(x => x.Vote);
            }
        }
        public string AccountMail { get; set; }
        public string Number { get; set; }

    }
}
