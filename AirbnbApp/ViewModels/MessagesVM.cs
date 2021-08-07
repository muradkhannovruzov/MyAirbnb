using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using AirbnbApp.Views;

namespace AirbnbApp.ViewModels
{
    class MessagesVM : ViewModelBase
    {
        private RelayCommand findAccount;
        private string email;
        private string writingTo = "";
        private ObservableCollection<Messages> messages = new ObservableCollection<Messages>();
        private RelayCommand writeMessage;
        private string message;
        private Account account2;
        private Messaging selectedMessage = new Models.Messaging();
        private int messageIndex;
        private RelayCommand<object> cancelCommand;
        private DispatcherTimer newMessage;

        public int MessageIndex
        {
            get => messageIndex; set
            {
                Set(ref messageIndex, value);
                if (value > -1)
                {
                    SelectedMessage = Messaging[value];
                }
            }
        }
        public Messaging SelectedMessage
        {
            get => selectedMessage; set
            {

                Set(ref selectedMessage, value);
                messages.Clear();
                foreach (var item in SelectedMessage.Messages)
                {
                    messages.Add(item);
                }
                WritingTo = SelectedMessage.Account2Name;
            }
        }
        public ObservableCollection<Messaging> Messaging { get; set; } = new ObservableCollection<Messaging>();
        public Account Account2
        {

            get => account2;

            set
            {
                Set(ref account2, value);
                var Message = Account.Messages.FirstOrDefault(x => x.Account2ID == Account2.Id);
                if (Message == null)
                {
                    var message = new Messaging();
                    message.Account1ID = Account.Id;
                    message.Account2ID = Account2.Id;
                    message.Account1Name = Account.Email;
                    message.Account2Name = Account2.Email;
                    Account.Messages.Add(message);
                    Account.Messages[Account.Messages.Count - 1].Messages = new List<Messages>();
                    WritingTo = value.Email;
                    Messaging.Add(message);
                    MessagesListRefresh.Refresh.Invoke();
                    Task.Run(async () =>
                    {
                        await ObjSender.SendObjectPorstURi(message, ProcessTypes.NewMessaging);
                    });
                }
                else
                {

                }
            }
        }
        public string Message
        {
            get => message;

            set
            {
                Set(ref message, value);
            }
        }
        public ObservableCollection<Messages> Messages
        {
            get => messages; set
            {
                Set(ref messages, value);
            }
        }
        public ObjectSender ObjSender { get; set; }
        public string WritingTo
        {
            get => writingTo; set
            {
                Set(ref writingTo, value);
                WriteMessage.RaiseCanExecuteChanged();
            }
        }
        public string Email
        {
            get => email; set
            {
                Set(ref email, value);
                FindAccount.RaiseCanExecuteChanged();
            }
        }
        public Account Account { get; set; }
        public MessagesVM(Account a)
        {
            ObjSender = new ObjectSender(new ServerConnection());
            Account = a;
            if (Account.Messages.Count > 0)
            {
                foreach (var item in Account.Messages)
                {
                    Messaging.Add(item);
                }
                foreach (var item in Messaging.FirstOrDefault().Messages)
                {
                    Messages.Add(item);
                }
                SelectedMessage = Messaging[0];
            }
            newMessage = new System.Windows.Threading.DispatcherTimer();
            newMessage.Tick += new EventHandler(NewMessage);
            newMessage.Interval = new TimeSpan(0, 0, 2);
            newMessage.Start();
        }

        private async void NewMessage(object sender, EventArgs e)
        {
            var str = await ObjSender.SendObjectPorstURi(Account.Email, ProcessTypes.FindEmail);
            var account = JsonConvert.DeserializeObject<Account>(str);
            int i = Messaging.IndexOf(SelectedMessage);
            Messaging = new ObservableCollection<Messaging>();

            foreach (var item in account.Messages)
            {
                Messaging.Add(item);
            }
            MessageIndex = i;
        }

        public RelayCommand WriteMessage => writeMessage ?? (writeMessage = new RelayCommand(() =>
        {
            if (WritingTo != null && !string.IsNullOrWhiteSpace(Message))
            {
                Messages mesage = new Messages();
                mesage.Account1Message = Message;
                mesage.Account2Message = "";
                mesage.Account1Readed = true;
                mesage.Account2Readed = false;
                mesage.Account1Background = "Green";
                mesage.Account2Background = "White";
                mesage.Time = DateTime.Now.ToShortDateString();
                Messages.Add(mesage);
                if (SelectedMessage.Messages == null)
                {
                    SelectedMessage.Messages = new List<Messages>();
                    if (Account.Messages[MessageIndex].Messages == null)
                    {
                        Account.Messages[MessageIndex].Messages = new List<Messages>();
                    }
                }
                SelectedMessage.Messages.Add(mesage);
                Task.Run(async () =>
                {
                    await ObjSender.SendObjectPorstURi(SelectedMessage, ProcessTypes.NewMessage);
                });
                Message = "";
            }
        }));

        public RelayCommand FindAccount => findAccount ?? (findAccount = new RelayCommand(async () =>
        {
            if (Email != Account.Email && Email != Account2?.Email)
            {
                var str = await ObjSender.SendObjectPorstURi(Email, ProcessTypes.FindEmail);
                var account = JsonConvert.DeserializeObject<Account>(str);
                if (account != null)
                {
                    Account2 = account;
                }
                else
                {
                    MessageBox.Show("Account doesn't find!!!");
                }

            }

        }, () => !string.IsNullOrWhiteSpace(Email)));

        public RelayCommand<object> CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand<object>((x) =>
        {
            newMessage.Stop();
            var mes = x as MessagesUC;
            mes.Close();
            
        }));
    }
}
