using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AirbnbApp.Services
{
    class SearchCityService
    {
        private WebClient web;
        private string URL;
        private const string KEY = "766f6a924fc8abf3c9b5ab76d834d477";

        public SearchCityService()
        {
            web = new WebClient();
            web.Encoding = Encoding.UTF8;
            URL = "http://api.openweathermap.org/data/2.5/weather?lang=az&units=metric&appid=" + KEY;
        }

        public CityInformation GetWeatherByName(string name)
        {
            var json = web.DownloadString(URL + $"&q={name}");
            //JsonConvert.DeserializeObject<CityInformation>(json);
            return JsonConvert.DeserializeObject<CityInformation>(json);
        }

        public CityInformation GetWeatherByCoords(float lat, float lon)
        {
            var json = web.DownloadString(URL + $"&lat={lat}&lon={lon}");
            return JsonConvert.DeserializeObject<CityInformation>(json);
        }
    }
}

//CityInformation