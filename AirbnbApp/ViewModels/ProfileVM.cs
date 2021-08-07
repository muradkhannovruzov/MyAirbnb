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
    class ProfileVM : ViewModelBase
    {
        public ObservableCollection<Publication> Publications { get; set; } = new ObservableCollection<Publication>();
        public ObservableCollection<Reservation> Reservations { get; set; } = new ObservableCollection<Reservation>();
        private Account account;
        private RelayCommand<Publication> deletePublication;
        private RelayCommand<Reservation> goRezervation;
        private IObjectSender objectSender;
        private RelayCommand<Publication> goRezerv;

        public Messenger Messenger { get; set; }
        public Account Account { get => account; set => Set(ref account, value); }
        public ProfileVM()
        {
            objectSender = new ObjectSender(new ServerConnection());
            Messenger = App.Container.GetInstance<Messenger>();
            Messenger.Register<AccountMessage>(this, x =>
            {
                if (x.Account != null)
                {
                    Account = x.Account;
                    foreach (var item in Account.Reservations)
                    {
                        Reservations.Add(item);
                    }
                    //Publications = new ObservableCollection<Publication>(); yuxarda elemisen
                    foreach (var item in Account.MyPublications)
                    {
                        Publications.Add(item);
                        if (item.Home.ImageSource == null)
                            item.Home.ImageSource = new List<ImageSource>();

                        for (int i = 0; i < item.Home.Images.Count; i++)
                        {
                            item.Home.ImageSource.Add(ConvertByteToBitmapImage.ConvertByteArrayToBitMapImage(item.Home.Images[i].Photos));
                        }
                    }
                }
                else
                {
                    Account = null;
                    Publications = new ObservableCollection<Publication>();
                    Reservations = new ObservableCollection<Reservation>();
                }


            });
        }
        public RelayCommand<Publication> DeletePublication => deletePublication ?? (deletePublication = new RelayCommand<Publication>((x) =>
        {
            var publicationinUC = Publications.FirstOrDefault(y => y.Id == x.Id);
            Publications.Remove(publicationinUC);
            objectSender.SendObjectPorstURi(JsonConvert.SerializeObject(x.Id), ProcessTypes.DeletePublication);

        }));
        public RelayCommand<Reservation> GoRezervation => goRezervation ?? (goRezervation = new RelayCommand<Reservation>(async (x) =>
          {
              var publication = await Task.Run(async () =>
              {
                  var str = await objectSender.SendObjectPorstURi(x.PublicationId, ProcessTypes.GetPublicationWithId);
                  return JsonConvert.DeserializeObject<Publication>(str);
              });
              Messenger.Send(new ViewChange() { ViewModel = new MakeCommentVM(publication, Account) });
          }));
        public RelayCommand<Publication> GoRezerv => goRezerv ?? (goRezerv = new RelayCommand<Publication>((x)=>
        {
            var RewRezerv = new AirbnbApp.Views.ReviewRezervUC(x);
            RewRezerv.Show();
        }));
    }
}