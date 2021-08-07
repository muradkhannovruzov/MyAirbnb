using AirbnbApp.Messanging;
using AirbnbApp.Services;
using AirbnbApp.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AirbnbApp.ViewModels
{
    public class MainVM : ViewModelBase
    {
        public Account account = null;
        private RelayCommand homeCommand;
        private ViewModelBase currentViewModel;
        private RelayCommand signCommand;
        private RelayCommand settingCommand;
        private Messenger messenger;
        private RelayCommand addPublicationCommand;
        private int selectedIndex = 0;
        private string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent("as").ToString()).ToString()).ToString();
        private GridLength columnWidth;
        public bool isCheckedHamgurber;
        private RelayCommand goProfile;
        private RelayCommand mouseEnterCommand;
        private RelayCommand mouseLeaveCommand;
        private Visibility avatarVisibility;
        private IConvertImage convertImage;
        private string signIcon;
        private string signText;
        private RelayCommand goMessages;
        public Visibility accountIn = Visibility.Collapsed;
        private BitmapImage avatar;

        public Visibility AccountIn { get => accountIn; set => Set(ref accountIn, value); }
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                Set(ref selectedIndex, value);

                if (selectedIndex == 0)
                {
                    CurrentViewModel = App.Container.GetInstance<HomeVM>();
                    IsCheckedHamgurber = false;
                }
                if (selectedIndex == 1)
                {
                    if (account != null)
                    {
                        CurrentViewModel = App.Container.GetInstance<AddPublicationVM>();
                        IsCheckedHamgurber = false;
                    }
                    else MessageBox.Show("You didn't login");

                }
                if (selectedIndex == 2)
                {
                    if (account == null)
                    {
                        CurrentViewModel = App.Container.GetInstance<SignInVM>();
                        IsCheckedHamgurber = false;
                    }
                    else
                    {
                        if (SignText == "Sign out") messenger.Send(new AccountMessage() { Account = null });
                    }
                }
                if (selectedIndex == 3)
                {
                    if (account != null)
                    {
                        CurrentViewModel = App.Container.GetInstance<ProfileInfoVM>();
                        IsCheckedHamgurber = false;
                    }
                    else MessageBox.Show("You didn't login");
                }
            }
        }

        public BitmapImage Avatar
        {
            get => avatar; set
            {
                Set(ref avatar, value);
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel; set
            {
                Set(ref currentViewModel, value);
            }
        }

        public MainVM()
        {
            AvatarVisibility = Visibility.Collapsed;
            SignIcon = "SignIn";
            SignText = "Sign In";
            CurrentViewModel = App.Container.GetInstance<HomeVM>();
            ColumnWidth = new GridLength(40);
            messenger = App.Container.GetInstance<Messenger>();
            convertImage = App.Container.GetInstance<IConvertImage>();
            messenger.Register<ViewChange>(this, x =>
            {

                CurrentViewModel = x.ViewModel;

            });
            messenger.Register<AccountMessage>(this, x =>
            {
                account = x.Account;
                if (x.Account != null)
                {
                    AccountIn = Visibility.Visible;
                    SignIcon = "SignOut";
                    SignText = "Sign out";
                }
                else
                {
                    AccountIn = Visibility.Collapsed;
                    SignIcon = "SignIn";
                    SignText = "Sign In";
                }


                if (account != null)
                {
                    if (account.Image != null)
                    {
                        Avatar = convertImage.ConvertBitmapImage(account.Image.Photos);
                        Avatar.Freeze();
                    }
                    else
                    {
                        Avatar = new BitmapImage(new Uri(directory + @"\Images\avatar_1.png"));
                        Avatar.Freeze();
                    }
                }                
                else
                {
                    Avatar = new BitmapImage(new Uri(directory + @"\Images\avatar_1.png"));
                    Avatar.Freeze();
                }

            });
        }
        public bool IsCheckedHamgurber
        {
            get => isCheckedHamgurber;
            set
            {
                isCheckedHamgurber = value;

                if (isCheckedHamgurber)
                {
                    ColumnWidth = new GridLength(200);
                    AvatarVisibility = Visibility.Visible;
                }
                else
                {
                    ColumnWidth = new GridLength(40);
                    AvatarVisibility = Visibility.Collapsed;
                }
            }
        }
        public Visibility AvatarVisibility { get => avatarVisibility; set => Set(ref avatarVisibility, value); }

        public string SignIcon { get => signIcon; set => Set(ref signIcon, value); }
        public string SignText { get => signText; set => Set(ref signText, value); }

        public RelayCommand MouseEnterCommand => mouseEnterCommand ?? (mouseEnterCommand = new RelayCommand(() =>
        {
            IsCheckedHamgurber = true;
        }));

        public RelayCommand MouseLeaveCommand => mouseLeaveCommand ?? (mouseLeaveCommand = new RelayCommand(() =>
        {
            IsCheckedHamgurber = false;
        }));
        public GridLength ColumnWidth
        {
            get => columnWidth;
            set => Set(ref columnWidth, value);
        }
        public RelayCommand HomeCommand => homeCommand ?? (homeCommand = new RelayCommand(() =>
        {
            CurrentViewModel = App.Container.GetInstance<HomeVM>();
        }));

        public RelayCommand SignCommand => signCommand ?? (signCommand = new RelayCommand(() =>
        {
            if (account == null)
            {
                CurrentViewModel = App.Container.GetInstance<SignInVM>();
            }
            else
            {
                if (SignText == "Sign out") messenger.Send(new AccountMessage() { Account = null });
            }
        }));

        public RelayCommand SettingCommand => settingCommand ?? (settingCommand = new RelayCommand(() =>
        {

        }));
        public RelayCommand AddPublicationCommand => addPublicationCommand ?? (addPublicationCommand = new RelayCommand(() =>
        {
            CurrentViewModel = App.Container.GetInstance<AddPublicationVM>();
        }));
        public RelayCommand GoProfile => goProfile ?? (goProfile = new RelayCommand(() =>
        {
            if (account != null)
            {
                CurrentViewModel = App.Container.GetInstance<ProfileVM>();
            }
            else
            {

            }
        }));
        public RelayCommand GoMessages => goMessages ?? (goMessages = new RelayCommand(() =>
          {
              if (account != null)
              {
                  var a = new MessagesUC(account);
                  a.Show();
              }
              else
              {

              }
          }));
    }
}
