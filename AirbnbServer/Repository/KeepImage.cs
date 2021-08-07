using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AirbnbServer.Repository
{
    public class KeepImage
    {
        public int Id { get; set; }
        [Column(TypeName = "Image")]
        public byte[] Photos { get; set; }
        public virtual HomeFeature homeFeature { get; set; }
    }
}
