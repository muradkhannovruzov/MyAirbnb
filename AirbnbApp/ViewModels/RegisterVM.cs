using AirbnbApp.Messanging;
using AirbnbApp.Services;
using AirbnbApp.Validations;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AirbnbApp.ViewModels
{
    public class RegisterVM : ViewModelBase, IDataErrorInfo
    {
        private Visibility passwordVisibility;
        private Visibility textBoxVisibility;
        private RelayCommand<object> showCommand;
        private RelayCommand<object> regCommand;
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private DateTime birthDate = DateTime.Now;
        private Messenger messenger;
        private RelayCommand backCommand;
        private INotificationService notificationService;
        private string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent("as").ToString()).ToString()).ToString();
        private string ımagePath;
        private BitmapImage ımage;
        private RelayCommand addImageCommand;
        private IConvertImage convertImage;



        public RegisterVM(INotificationService notificationService)
        {
            PasswordVisibility = Visibility.Visible;
            messenger = App.Container.GetInstance<Messenger>();
            this.notificationService = notificationService;
            this.convertImage = App.Container.GetInstance<IConvertImage>();
        }

        public BitmapImage Image { get => ımage; set => Set(ref ımage, value); }

        public string ImagePath
        {
            get => ımagePath;
            set
            {
                ımagePath = value;
                Image = new BitmapImage(new Uri(value));
            }
        }

        public string FirstName
        {
            get => firstName; set
            {
                Set(ref firstName, value);
                RegCommand.RaiseCanExecuteChanged();
            }
        }
        public string LastName
        {
            get => lastName; set
            {
                Set(ref lastName, value);
                RegCommand.RaiseCanExecuteChanged();
            }
        }
        public string Email
        {
            get => email; set
            {
                Set(ref email, value);
                RegCommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get => password; set
            {
                Set(ref password, value);
                RegCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime BirthDate
        {
            get => birthDate; set
            {
                Set(ref birthDate, value);
                RegCommand.RaiseCanExecuteChanged();
            }
        }

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

        public RelayCommand<object> RegCommand => regCommand ?? (regCommand = new RelayCommand<object>((x) =>
        {
            var passwordBox = x as PasswordBox;
            if (PasswordVisibility == Visibility.Visible) Password = passwordBox.Password;
            var account = new Account()
            {
                BirthDate = BirthDate,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password
            };
            if (account.Image == null) account.Image = new Models.Image();
            account.Image.Photos = convertImage.ConvertToByte(Image);
            var ok = notificationService.SendCode(Email);
            if (ok) messenger.Send(new ViewChange() { ViewModel = new NotificationVM(account, App.Container.GetInstance<INotificationService>()) });

        }, (x) =>
        {
            return (!string.IsNullOrWhiteSpace((x as PasswordBox).Password) && !string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(Email));
        }));

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
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() =>
        {
            messenger.Send(new ViewChange() { ViewModel = App.Container.GetInstance<SignInVM>() });
        }));

        public RelayCommand AddImageCommand => addImageCommand ?? (addImageCommand = new RelayCommand(() =>
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a image";
            fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.jfif|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                ImagePath = fileDialog.FileName;
            }
        }));

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                var validator = new RegisterValidation();
                var result = validator.Validate(this);
                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains(columnName));
                    if (error != null)
                        return error.ErrorMessage;
                }
                return string.Empty;
            }
        }
    }
}
