using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Services
{
    public interface IServerConnection
    {
        Task<string> PostURI(string uri, HttpContent content);
        Task<string> SendURI(string uri, HttpContent content);
    }
}
