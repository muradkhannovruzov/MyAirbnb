using AirbnbApp.Messanging;
using AirbnbApp.Services;
using AirbnbApp.Validations;
using AirbnbApp.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace AirbnbApp.ViewModels
{
    class ChangePassVM : ViewModelBase, IDataErrorInfo
    {
        private string oldPass;
        private string newPass;
        private string confirmPass;
        private RelayCommand<object> cancelCommand;
        private RelayCommand<object> changeCommand;
        private Account account;
        private IObjectSender objectSender;
        private string lableContent;
        private Visibility lableVisibility;
        private Messenger messenger;

        public string LableContent { get => lableContent; set => Set(ref lableContent, value); }
        public Visibility LableVisibility { get => lableVisibility; set => Set(ref lableVisibility, value); }

        public string OldPass { get => oldPass; set => oldPass = value; }
        public string NewPass { get => newPass; set => newPass = value; }
        public string ConfirmPass { get => confirmPass; set => Set(ref confirmPass, value); }
        public ChangePassVM(Account account)
        {
            this.account = account;
            this.objectSender = App.Container.GetInstance<IObjectSender>();
            LableVisibility = Visibility.Hidden;
            messenger = App.Container.GetInstance<Messenger>();
        }

        public RelayCommand<object> CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand<object>((x) =>
        {
            var window = x as ChangePassWindow;
            window.Close();
        }));

        public RelayCommand<object> ChangeCommand => changeCommand ?? (changeCommand = new RelayCommand<object>((x) =>
        {
            var window = x as ChangePassWindow;

            OldPass = window.cuPass.Password;
            NewPass = window.nPass.Password;
            ConfirmPass = window.coPass.Password;
            if(OldPass == account.Password)
            {
                if(NewPass == ConfirmPass)
                {
                    if(NewPass.Length >=3)
                    {
                        account.Password = NewPass;
                        objectSender.SendObjectPorstURi(account, ProcessTypes.UpdateAccount);
                        messenger.Send(new AccountMessage()
                        {
                            Account = account
                        });
                        lableVisibility = Visibility.Hidden;
                        window.Close();
                    }
                    else
                    {
                        LableContent = "The password cannot be less the 3 characters";
                        lableVisibility = Visibility.Visible;
                    }
                }
                else
                {
                    LableContent = "New password and confirm password is not same";
                    lableVisibility = Visibility.Visible;
                }
            }
            else
            {
                LableContent = "Current password is incorrect";
                LableVisibility = Visibility.Visible;
            }
            
        }));

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                var validator = new ChangePassValidation();
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
