using AirbnbApp.Messanging;
using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AirbnbApp.Views;
using System.Windows.Media;
using Newtonsoft.Json;

namespace AirbnbApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class HomeVM : ViewModelBase
    {
        private string latitude;
        private string longitude;
        private Publication theSelectedItem;
        private IObjectSender objectSender;
        private RelayCommand selectCommand;
        private Account account;
        private List<Publication> myList;
        private string city = "";
        private string shearch = "Show All";

        public string Address { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AccountId { get; set; }
        public string Shearch { get => shearch; set => shearch = value; }
        public Account Account
        {
            get => account; set
            {

                account = value;

            }
        }
        public List<Publication> MyList
        {
            get => myList; set
            {
                myList = value;
            }
        }
        public RelayCommand GuestMinusButton { get; set; }
        public RelayCommand GuestPilusButton { get; set; }
        public RelayCommand SearchButton { get; set; }
        public Reservation reservation { get; set; }
        public Location CurrLoc { get; set; } = new Location();
        public string City
        {
            get => city; set
            {
                Set(ref city, value);
                if (!string.IsNullOrWhiteSpace(City))
                {
                    Shearch = "Shearch";
                }
                else
                {
                    Shearch = "Show All";
                }
            }
        }
        public Publication TheSelectedItem
        {
            get => theSelectedItem;
            set
            {
                theSelectedItem = value;
            }
        }
        public ObservableCollection<ImageSource> ConvertedImage { get; set; }
        public string Latitude
        {
            get => latitude;
            set
            {
                latitude = value;
                CurrLoc.Latitude = double.Parse(latitude);
                MyList = GetCitysPublication.GetPublications((float)CurrLoc.Latitude, (float)CurrLoc.Longitude, MyList);

            }
        }
        public string Longitude
        {
            get => longitude;
            set
            {
                longitude = value;
                CurrLoc.Longitude = double.Parse(Longitude);
                MyList = GetCitysPublication.GetPublications((float)CurrLoc.Latitude, (float)CurrLoc.Longitude, MyList);
            }
        }
        public Messenger MyMessenger { get; set; }
        public void ConvertByteToImageHelper()
        {
            ConvertedImage = new ObservableCollection<ImageSource>();
            foreach (var item in MyList)
            {
                if (item.Home.ImageSource == null) item.Home.ImageSource = new List<ImageSource>();
                foreach (var item2 in item.Home.Images)
                {
                    ImageSource source = ConvertByteToBitmapImage.ConvertByteArrayToBitMapImage(item2.Photos);
                    item.Home.ImageSource.Add(source);
                }
            }
        }
        public HomeVM(IObjectSender objectSender)
        {
            SearchButton = new RelayCommand(() => Search());
            MyList = new List<Publication>();
            reservation = new Reservation();
            ConvertedImage = new ObservableCollection<ImageSource>();
            MyMessenger = App.Container.GetInstance<Messenger>();
            this.objectSender = objectSender;

            MyMessenger.Register<LocationMessage>(this, message =>
            {
                Latitude = message.Latitude.ToString();
                Longitude = message.Longitude.ToString();
            });
            MyMessenger.Register<AccountMessage>(this, x =>
            {
                Account = x.Account;
            });

        }
        public void Search()
        {
            var t = Task.Run(async () =>
             {
                 var str = await objectSender.SendObjectPorstURi(null, ProcessTypes.GetAllPublications);
                 MyList = JsonConvert.DeserializeObject<List<Publication>>(str);
                 ConvertByteToImageHelper();
                 if (!string.IsNullOrWhiteSpace(City))
                 {
                     MyList = MyList.Where(x => x.Home.City.Name.ToLower() == City.Trim().ToLower()).ToList();
                 }
             });
            t.Wait();
            Shearch = "Shearch";
            var mesenger = App.Container.GetInstance<Messenger>();
            mesenger.Send<HomeListChanged>(new HomeListChanged() { Publications = MyList });
            RefreshPublications.refresh.Invoke();

        }

        public RelayCommand SelectCommand => selectCommand ?? (selectCommand = new RelayCommand(() =>
        {
            if (Account!=null&&Account.Id != TheSelectedItem.AccountId)
            {
                int publicationId = TheSelectedItem.Id;
                var str = JsonConvert.SerializeObject(publicationId);
                objectSender.SendObjectPorstURi(str, ProcessTypes.ReviewIncrease);
            }
            var HomeView = new HomeView(TheSelectedItem, Account);
            HomeView.Show();
        }));
    }
}
