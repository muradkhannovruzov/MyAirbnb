using AirbnbApp.Messanging;
using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using Models;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AirbnbApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class HomeViewVM : ViewModelBase
    {
        private DateTime beginTime = DateTime.Now;
        private DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);
        private RelayCommand makeRezervation;
        public Location CurrLoc { get; set; } = new Location();
        public Publication Publication { get; set; }
        public ImageSource SelectedImage { get; set; }
        public Messenger MyMessenger { get; }

        private IObjectSender ObjectSender;

        public DateTime BeginTime
        {
            get => beginTime; set
            {
                if (value >= BeginTime && value < EndTime)
                {
                    beginTime = value;
                }

            }
        }
        public DateTime EndTime
        {
            get => endTime; set
            {
                if (value >= EndTime && value > BeginTime)
                {
                    endTime = value;
                }

            }
        }
        public Account LogInAccount { get; set; }
        public HomeViewVM(Publication publication, Account account)
        {           
            MyMessenger = App.Container.GetInstance<Messenger>();
            ObjectSender = new ObjectSender(new ServerConnection());
            Publication = publication;
            LogInAccount = account;
            CurrLoc.Latitude = double.Parse(Publication?.Home?.lan);
            CurrLoc.Longitude = double.Parse(Publication?.Home?.lon);
        }
        public RelayCommand MakeRezervation => makeRezervation ?? (makeRezervation = new RelayCommand(() =>
        {
            if (LogInAccount != null)
            {
                if (Publication.AccountId != LogInAccount.Id)
                {
                    Task.Run(async () =>
                    {
                        Reservation rev = new Reservation();
                        rev.BeginTime = BeginTime;
                        rev.EndTime = EndTime;
                        rev.PublicationId = Publication.Id;
                        rev.AccountRezvId = LogInAccount.Id;
                        var str = await ObjectSender.SendObjectPorstURi(rev, ProcessTypes.MakeRezerv);

                        string a = JsonConvert.DeserializeObject<string>(str);

                        MessageBox.Show(a);
                    });
                }
                else
                {
                    MessageBox.Show("You can't rezerv your own");
                }
            }
            else
            {
                MessageBox.Show("Please Log In!!!");
            }
        }));
    }
}
