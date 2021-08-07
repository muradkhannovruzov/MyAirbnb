using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models
{
    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual Image Image { get; set; }
        [NotMapped]
        public BitmapImage BitmapImage { get; set; }
        public virtual List<Publication> MyPublications { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
        public virtual List<Messaging> Messages { get; set; } = new List<Messaging>();
    }
}
