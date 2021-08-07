using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Image
    {
        public int Id { get; set; }
        [Column(TypeName = "Image")]
        public byte[] Photos { get; set; }

        
    }
}
