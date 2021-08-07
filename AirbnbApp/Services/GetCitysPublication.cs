using AirbnbApp.Messanging;
using GalaSoft.MvvmLight.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Services
{
    public class GetCitysPublication
    {
        public static List<Publication> GetPublications(float Latitude, float Longitude, List<Publication> Collection)
        {

            SearchCityService search = new SearchCityService();
            CityInformation CityName = search.GetWeatherByCoords(Latitude,Longitude);
            List<Publication> Collectiona = new List<Publication>();
            foreach (var item in Collection.Where(x => x.Home.City.Name == CityName.name))
            {
                Collectiona.Add(item);
            }
            var mesenger = App.Container.GetInstance<Messenger>();
            mesenger.Send<HomeListChanged>(new HomeListChanged() { Publications = Collectiona });
            return Collectiona;
        }
    }
}
