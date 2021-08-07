using AirbnbApp.Messanging;
using AirbnbApp.Services;
using AirbnbApp.Validations;
using AirbnbApp.Views;
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
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AirbnbApp.ViewModels
{
    class ProfileInfoVM : ViewModelBase, IDataErrorInfo
    {
        private string firstname;
        private string lastname;
        private DateTime birthDate;
        private string email;
        private RelayCommand changeCommand;
        private RelayCommand addImageCommand;
        private RelayCommand saveCommand;
        private RelayCommand cancelCommand;
        private Messenger messenger;
        private Account account;
        private IObjectSender objectSender;
        private IConvertImage convertImage;
        private BitmapImage ımage;
        private string ımagePath;
        private string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent("as").ToString()).ToString()).ToString();


        public Account Account
        {
            get => account;
            set
            {
                account = value;
                Firstname = value.FirstName;
                Lastname = value.LastName;
                BirthDate = value.BirthDate;
                Email = value.Email;
                if (value.Image != null)
                {
                    Image = convertImage.ConvertBitmapImage(value.Image.Photos);
                    Image.Freeze();
                }
                else
                {
                    Image = new BitmapImage(new Uri(directory + @"\Images\avatar_1.png"));
                    Image.Freeze();
                }
            }
        }

        public string Firstname { get => firstname; set => Set(ref firstname, value); }
        public string Lastname { get => lastname; set => Set(ref lastname, value); }
        public DateTime BirthDate { get => birthDate; set => Set(ref birthDate, value); }
        public string Email { get => email; set => Set(ref email, value); }

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

        public ProfileInfoVM()
        {
            messenger = App.Container.GetInstance<Messenger>();
            objectSender = App.Container.GetInstance<IObjectSender>();
            convertImage = App.Container.GetInstance<IConvertImage>();

            messenger.Register<AccountMessage>(this, x =>
            {
                if (x.Account != null)
                    Account = x.Account;
            });

        }

        public RelayCommand ChangeCommand => changeCommand ?? (changeCommand = new RelayCommand(() =>
        {
            ChangePassWindow changePassWindow = new ChangePassWindow(account);
            changePassWindow.ShowDialog();
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

        public RelayCommand SaveCommand => saveCommand ?? (saveCommand = new RelayCommand(() =>
        {
            if (account.Image == null) account.Image = new Image();
            account.Image.Photos = convertImage.ConvertToByte(Image);
            account.FirstName = Firstname;
            account.LastName = Lastname;
            account.BirthDate = BirthDate;
            objectSender.SendObjectPorstURi(account, ProcessTypes.UpdateAccount);
            messenger.Send(new AccountMessage() { Account = account });
        }));

        public RelayCommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(() =>
        {
            messenger.Send(new ViewChange() { ViewModel = App.Container.GetInstance<HomeVM>() });
        }));

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                var validator = new ProfileInfoValidation();
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
