using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AirbnbApp.Services
{
    public interface IConvertImage
    {
        byte[] ConvertToByte(BitmapImage bitmapImage);
        BitmapImage ConvertBitmapImage(byte[] bytes);
    }
}
