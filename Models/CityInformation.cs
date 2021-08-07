using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CityInformation
    {
            public Coord coord { get; set; }
            public string _base { get; set; }
            public int visibility { get; set; }
            public int dt { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
    }
}
