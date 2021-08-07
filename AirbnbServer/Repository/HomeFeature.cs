using AirbnbServer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Models
{

    public class HomeFeature
    {
        public int Id { get; set;}
        public int Price { get; set;}
        public int Guests { get; set;}
        public int Bedrooms { get; set;}
        public int Bathrooms { get; set;}
        public bool TV { get; set; }
        public bool WIFI { get; set; }
        public bool Hangers { get; set; }
        public bool Shampoo { get; set; }
        public bool Hairdryer { get; set; }
        public bool Essentials { get; set;}
        public bool IronHousehold { get; set; }
        public bool CookingBasics { get; set;}
        public bool Deskworkspace { get; set; }
        public string Address { get; set;}
        public string TheQuestion { get; set;}
        public Home Home { get; set; }
        public HomeType TypeOfHome { get; set;}
        public TypeOfPlace PlaceType { get; set;}
        public virtual List<KeepImage> keepImages { get; set; }
        [NotMapped]
        public List<ImageSource> ImageSource { get; set; }
    }
}
