using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Models
{
    public class Home
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual City City { get; set; }
        public string Address { get; set; }
        public string SelectedImage { get; set; }
        public int SelectedImageIndex { get; set; } = 0;
        public string TheQuestion { get; set; }
        public HomeType HomeType { get; set; }
        public TypeOfPlace PlaceType { get; set; }
        public virtual List<Amenity> Amenities { get; set; } = new List<Amenity>();
        public virtual List<Image> Images { get; set; }
        [NotMapped]
        public List<ImageSource> ImageSource { get; set; }
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }
        public int InfantCount { get; set; }
        public int BedrommsCount { get; set; }
        public int BathrommsCount { get; set; }
        public int Price { get; set; }
        public string lon { get; set; }
        public string lan { get; set; }
    }
}
