using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Services
{
    public interface INotificationService
    {
        bool SendCode(string email);
        bool CheckCode(string code);
    }
}
