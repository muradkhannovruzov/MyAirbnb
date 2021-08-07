using AirbnbApp.Messanging;
using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AirbnbApp.ViewModels
{
    class SignInVM : ViewModelBase
    {
        private string username;
        private string password;
        private Visibility lableVisibilty;
        private Messenger messenger;
        private RelayCommand<object> loginCommand;
        private string lableText;
        private Visibility loadingVisibility;
        private string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent("as").ToString()).ToString()).ToString();
        private Visibility passwordVisibility = Visibility.Visible;
        private Visibility textBoxVisibility =Visibility.Hidden;
        private RelayCommand<object> showCommand;
        private RelayCommand registerCommand;
        private IObjectSender objectSender;

        public string Username
        {
            get => username;
            set
            {
                Set(ref username, value);
                LableVisibilty = Visibility.Hidden;
            }
        }
        public string Password
        {
            get => password;
            set
            {
                Set(ref password, value);
                LableVisibilty = Visibility.Hidden;
            }
        }
        public string LableText { get => lableText; set => Set(ref lableText, value); }
        public Visibility LableVisibilty { get => lableVisibilty; set => Set(ref lableVisibilty, value); }
        public Visibility PasswordVisibility
        {
            get => passwordVisibility;
            set
            {
                Set(ref passwordVisibility, value);
                if (value == Visibility.Visible) TextBoxVisibility = Visibility.Collapsed;
                else TextBoxVisibility = Visibility.Visible;
            }
        }
        public Visibility TextBoxVisibility { get => textBoxVisibility; set => Set(ref textBoxVisibility, value); }
        public Visibility LoadingVisibility { get => loadingVisibility; set => Set(ref loadingVisibility, value); }
        public string GifPath => directory + @"\Images\circle3.gif";
        public SignInVM(IObjectSender objectSender)
        {
            LableVisibilty = Visibility.Collapsed;
            LoadingVisibility = Visibility.Collapsed;
            PasswordVisibility = Visibility.Visible;

            messenger = App.Container.GetInstance<Messenger>();
            this.objectSender = objectSender;
        }

        public RelayCommand<object> ShowCommand => showCommand ?? (showCommand = new RelayCommand<object>((x) =>
        {
            var passwordBox = x as PasswordBox;

            if (PasswordVisibility == Visibility.Visible)
            {
                PasswordVisibility = Visibility.Collapsed;
                Password = passwordBox.Password;
            }
            else
            {
                PasswordVisibility = Visibility.Visible;
                passwordBox.Password = Password;

            }
        }));
        public RelayCommand<object> LoginCommand => loginCommand ?? (loginCommand = new RelayCommand<object>((x) =>
        {
            var passwordBox = x as PasswordBox;
            if (PasswordVisibility == Visibility.Visible) Password = passwordBox.Password;
            if (string.IsNullOrWhiteSpace(Username))
            {
                LableText = "Enter Email";
                LableVisibilty = Visibility.Visible;
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                LableText = "Enter password";
                LableVisibilty = Visibility.Visible;
            }
            else
            {
                Task.Run(async () =>
                {
                    Account account=null;
                    LoadingVisibility = Visibility.Visible;
                    EmailPass emailPass = new EmailPass()
                    {
                        Email = Username,
                        Pass = Password

                    };

                    var str =await objectSender.SendObjectPorstURi(emailPass, ProcessTypes.FindAccount);
                    
                    if(!string.IsNullOrEmpty(str)) account = JsonConvert.DeserializeObject<Account>(str);
                    if (account != null)
                    {
                        var view = App.Container.GetInstance<HomeVM>();
                        LableVisibilty = Visibility.Collapsed;
                        messenger.Send(new AccountMessage() { Account = account });
                        messenger.Send(new ViewChange() { ViewModel = view });
                        Password = string.Empty;
                    }
                    else
                    {
                        LableText = "Account not found";
                        LableVisibilty = Visibility.Visible;
                    }
                    LoadingVisibility = Visibility.Collapsed;
                });
            }
        }));

        public RelayCommand RegisterCommand => registerCommand ?? (registerCommand = new RelayCommand(() =>
        {
            messenger.Send(new ViewChange() { ViewModel = App.Container.GetInstance<RegisterVM>() });
        }));
    }
}
