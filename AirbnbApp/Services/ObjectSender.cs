using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Services
{
    public class ObjectSender : IObjectSender
    {
        public IServerConnection ServerConnection { get; set; }

        public ObjectSender(IServerConnection serverConnection)
        {
            ServerConnection = serverConnection;
        }

        public async Task<string> SendObjectSendURI(object obj, ProcessTypes processTypes)
        {
            var payload = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            return await Task.Run(async () =>await ServerConnection.SendURI($"http://localhost:8888/{Enum.GetName(typeof(ProcessTypes),processTypes)}/",
                                                              content));
        }

        public async Task<string> SendObjectPorstURi(object obj, ProcessTypes processTypes)
        {
            var payload = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            return await Task.Run(async() =>await ServerConnection.PostURI($"http://localhost:8888/{Enum.GetName(typeof(ProcessTypes), processTypes)}/",
                                                              content));
        }
    }
}