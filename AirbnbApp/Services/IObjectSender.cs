using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Services
{
    public interface IObjectSender
    {
        IServerConnection ServerConnection { get; set; }
        Task<string> SendObjectSendURI(object obj, ProcessTypes processTypes);

        Task<string> SendObjectPorstURi(object obj, ProcessTypes processTypes);
    }
}
