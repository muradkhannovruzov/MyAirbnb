using AirbnbApp.Model;
using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirbnbApp.ViewModels
{
    class ReviewRezervVM : ViewModelBase
    {
        public List<RezervAccounts> RezervAccounts { get; set; } = new List<RezervAccounts>();
        public Publication publication { get; set; }
        public List<int> AccouontIDS { get; set; } = new List<int>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        private IObjectSender objectSender;
        public ReviewRezervVM(Publication p)
        {
            publication = p;
            foreach (var item in p.Reservations)
            {
                AccouontIDS.Add(item.AccountRezvId);
            }
            objectSender =new ObjectSender(new ServerConnection());
            var t = Task.Run(async()=>
            {
                var str = await objectSender.SendObjectPorstURi(AccouontIDS, ProcessTypes.GetSpecificAccounts);
                Accounts = JsonConvert.DeserializeObject<List<Account>>(str);

                for (int i = 0; i < Accounts.Count; i++)
                {
                    RezervAccounts.Add(new Model.RezervAccounts() { Account=Accounts[i], Reservation=publication.Reservations[i] });
                }

            });
            t.Wait();
        }
    }
}
