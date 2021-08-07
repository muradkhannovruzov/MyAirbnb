using AirbnbApp.Messanging;
using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirbnbApp.ViewModels
{
    public class NotificationVM : ViewModelBase
    {
        private RelayCommand enterCommand;
        private Messenger messenger;
        private INotificationService notificationService;
        private Visibility lableVis;
        private string code;
        private RelayCommand againCommand;
        private string Email;
        private RelayCommand backCommand;
        private IObjectSender server;
        private Account account;

        public NotificationVM(Account account, INotificationService notificationService)
        {
            Email = account.Email;
            messenger = App.Container.GetInstance<Messenger>();
            this.notificationService = notificationService;
            
            LableVis = Visibility.Collapsed;
            this.account = account;
            this.server = App.Container.GetInstance<IObjectSender>();
        }
        public Visibility LableVis { get => lableVis; set => Set(ref lableVis, value); }

        public string Code
        {
            get => code; set
            {
                Set(ref code, value);
                LableVis = Visibility.Collapsed;
            }
        }
        public RelayCommand EnterCommand => enterCommand ?? (enterCommand = new RelayCommand(() =>
        {
            if (notificationService.CheckCode(Code))
            {
                Task.Run(() =>
                {
                    var str = server.SendObjectPorstURi(account, ProcessTypes.AddAccount);
                    MessageBox.Show(str.Result);
                    messenger.Send(new ViewChange() { ViewModel = App.Container.GetInstance<SignInVM>() });
                });
            }
            else
            {
                LableVis = Visibility.Visible;
            }
        }));

        public RelayCommand AgainCommand => againCommand ?? (againCommand = new RelayCommand(() =>
        {
            notificationService.SendCode(Email);
        }));
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() =>
        {
            messenger.Send(new ViewChange() { ViewModel = App.Container.GetInstance<RegisterVM>() });
        }));
    }
}
