using AirbnbServer.Repository;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();

            foreach (var item in Enum.GetNames(typeof(ProcessTypes)))
            {
                listener.Prefixes.Add("http://localhost:8888/" + item + '/');
            }


            listener.Start();
            Console.WriteLine("Witing for Connection...");


            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Client is connected.");

                HttpListenerRequest req = context.Request;
                HttpListenerResponse res = context.Response;
                Console.WriteLine($"User host name : {req.UserHostName}");
                var processType = req.Url.LocalPath;
                processType.Remove(0);
                processType.Remove(processType.Length - 1);
                Console.WriteLine($"Process type: {processType}");
                List<byte> test = new List<byte>();
                using (var stream = req.InputStream)
                {
                    int size = 512;
                    while (true)
                    {
                        byte[] temp = new byte[size];
                        var x = stream.Read(temp, 0, size);
                        test.AddRange(temp);
                        if (x < size) break;
                    }
                }
                Console.WriteLine($"Packet size: {test.Count} bytes");

                object obj = null;

                if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.AddAccount)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var Account = JsonConvert.DeserializeObject<Account>(str);
                    obj = AddAccount(Account);
                }
                else if(req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.FindAccount)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var emailPass = JsonConvert.DeserializeObject<EmailPass>(str);
                    obj = FindAccount(emailPass);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.GetPublicationWithId)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var PublicationId = JsonConvert.DeserializeObject<int>(str);
                    obj = GetPublicationWithId(PublicationId);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.GetSpecificAccounts)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var PublicationId = JsonConvert.DeserializeObject<List<int>>(str);
                    obj = GetSpecificAccounts(PublicationId);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.FindEmail)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var Email = JsonConvert.DeserializeObject<string>(str);
                    obj = FindEmail(Email);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.MakeComment)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var comment = JsonConvert.DeserializeObject<Comment>(str);
                    AddCommnet(comment);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.ReviewIncrease)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var PublicationId = JsonConvert.DeserializeObject<int>(str);
                    ReviewIncrease(PublicationId);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.MakeRezerv)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var Rezv = JsonConvert.DeserializeObject<Reservation>(str);
                    obj = GetPublication(Rezv);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.GetAllPublications)}/")
                {
                    obj = GetAllPublications();
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.DeletePublication)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    int id = JsonConvert.DeserializeObject<int>(str);
                    DeletePublication(id);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.AddPublication)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var publication = JsonConvert.DeserializeObject<Publication>(str);
                    AddPublication(publication);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.GetCountry)}/")
                {
                    obj = GetCountries();
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.GetAmenities)}/")
                {
                    obj = GetAmenities();
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.UpdateAccount)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var account = JsonConvert.DeserializeObject<Account>(str);
                    UpdateAccount(account);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.NewMessaging)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var Messaging = JsonConvert.DeserializeObject<Messaging>(str);
                    NewMessaging(Messaging);
                }
                else if (req.Url.LocalPath == $"/{Enum.GetName(typeof(ProcessTypes), ProcessTypes.NewMessage)}/")
                {
                    var str = Encoding.UTF8.GetString(test.ToArray());
                    var Message = JsonConvert.DeserializeObject<Messaging>(str);
                    NewMessage(Message);
                }
                byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

                res.ContentLength64 = buffer.Length;

                using (var output = res.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private static void NewMessage(Messaging message)
        {
            var lastmessage = message.Messages[message.Messages.Count-1];
            var db = new AirbnbDB();
            var acc = db.Accounts.FirstOrDefault(x => x.Id == message.Account1ID);

            var messaging = acc.Messages.FirstOrDefault(x=>x.Id == message.Id);
            if (messaging == null) messaging = new Messaging();
            if (messaging.Messages==null)
            {
                messaging.Messages = new List<Messages>();
            }
            messaging.Messages.Add(lastmessage);

            var acc2 = db.Accounts.FirstOrDefault(x => x.Id == message.Account2ID);

            var acc2mesages = acc2.Messages;
            var acc2mess = acc2mesages.FirstOrDefault(x=>x.Account2ID == acc.Id && x.Account1ID == acc2.Id);
            if (acc2mess.Messages==null)
            {
                acc2mess.Messages = new List<Messages>();
            }
            acc2mess.Messages.Add(new Messages() { Account1Background = "white", Account1Message=lastmessage.Account2Message, Account2Background = "pink",
                                                     Account2Message=lastmessage.Account1Message, Time = lastmessage.Time});
            acc2.Messages = acc2mesages;
            db.SaveChanges();
        }

        private static void NewMessaging(Messaging Message)
        {
            var db = new AirbnbDB();
            var acc = db.Accounts.FirstOrDefault(x => x.Id == Message.Account1ID);

            acc.Messages.Add(Message);

            var messenging = acc.Messages[acc.Messages.Count-1];
            var acc2 = db.Accounts.FirstOrDefault(x => x.Id == messenging.Account2ID);

            acc2.Messages.Add(new Messaging() { Account1ID = acc2.Id, Account1Name = acc2.Email, Account2ID=acc.Id, Account2Name= acc.Email, Messages = new List<Messages>() });
            db.SaveChanges();

        }

        private static Account FindEmail(string email)
        {
            var db = new AirbnbDB();
            var str = db.Accounts.FirstOrDefault(x=>x.Email == email);
            if (str == null)
            {
                return str;
            }
            else
            {
                return str;
            }
        }

        private static List<Account> GetSpecificAccounts(List<int> publicationId)
        {
            var db = new AirbnbDB();
            List<Account> accounts = new List<Account>();
            foreach (var item in publicationId)
            {
                accounts.Add(db.Accounts.FirstOrDefault(x=>x.Id==item));
            }
            return accounts;
        }

        private static void ReviewIncrease(int publicationId)
        {
            Console.WriteLine("ReviewIncrease "+ publicationId);
            var db = new AirbnbDB();
            var pub = db.Publications.FirstOrDefault(x=>x.Id == publicationId);
            ++pub.Review;
            db.SaveChanges();
        }

        private static void DeletePublication(int id)
        {
            Console.WriteLine("delete publication");
            var db = new AirbnbDB();
            var t = db.Publications.FirstOrDefault(x=>x.Id == id);
            db.Publications.Remove(t);
            db.SaveChanges();
        }

        private static Publication GetPublicationWithId(int publicationId)
        {
            var db = new AirbnbDB();
            return db.Publications.FirstOrDefault(x=>x.Id == publicationId);
        }

        public static Account FindAccount(EmailPass emailPass)
        {
            var db = new AirbnbDB();
            return db.Accounts.FirstOrDefault(x => x.Email == emailPass.Email && x.Password == emailPass.Pass);
        }

        public static void AddCommnet(Comment comment)
        {
            var db = new AirbnbDB();
            var Account = db.Accounts.FirstOrDefault(x=>x.Id==comment.AccountId);
            var publication = db.Publications.FirstOrDefault(x=>x.Id==comment.PublicationID);
            comment.AccountId = Account.Id;
            publication.Comments.Add(comment);
            db.SaveChanges();
        }

        public static string GetPublication(Reservation Rez)
        {
            string message = "Your reservation can't aprovved!!!";
            var db = new AirbnbDB();
            bool BeginTimeBool = true;
            bool EndTimeBool = true;
            bool TimeAviable = true;
            var Publication = db.Publications.FirstOrDefault(x=>x.Id == Rez.PublicationId);
            DateTime BeginTime = Rez.BeginTime, EndTime = Rez.EndTime;
            foreach (var item in Publication.Reservations)
            {
                if (BeginTime >= item.BeginTime && item.EndTime >= BeginTime)
                {
                    BeginTimeBool = false;
                    break;
                }
                if (EndTime >= item.BeginTime && item.EndTime >= EndTime)
                {
                    EndTimeBool = false;
                    break;
                }
            }
            if (BeginTime >= EndTime) TimeAviable = false;

            if (BeginTimeBool && EndTimeBool && TimeAviable)
            {
                AirbnbDB airbnbDB = new AirbnbDB();
                var publication = airbnbDB.Publications.FirstOrDefault(x => x.Id == Publication.Id);
                var account = airbnbDB.Accounts.FirstOrDefault(x=>x.Id == Rez.AccountRezvId);
                Rez.AccountRezvId = account.Id;
                publication.Reservations.Add(Rez);
                account.Reservations.Add(Rez);
                airbnbDB.SaveChanges();
                Console.WriteLine(Rez.AccountRezvId);
                message = "Your reservation aprovved!!!!";
            }
            return message;
        }

        public static List<Publication> GetAllPublications()
        {
            var db = new AirbnbDB();
            return db.Publications.ToList();
        }

        public static string AddAccount(Account account)
        {
            string Exist = "";
            var db = new AirbnbDB();
            if (db.Accounts.FirstOrDefault(x=>x.Email == account.Email) == null)
            {
                Exist = "Account Added";
                db.Accounts.Add(account);
                db.SaveChanges();
            }
            else
            {
                Exist = "This Email Has Already Been Created!!!!!";
            }
            return Exist;
        }

        public static void AddPublication(Publication publication)
        {
            var db = new AirbnbDB();
            Console.WriteLine(publication.Home.Amenities.Count().ToString());
            var city = db.Cities.FirstOrDefault(x => x.Id == publication.Home.City.Id);
            publication.Home.City = city;
            db.Publications.Add(publication);
            db.SaveChanges();
        }

        public static List<Country> GetCountries()
        {
            var db = new AirbnbDB();
            return db.Countries.ToList();
        }

        public static List<Amenity> GetAmenities()
        {
            var db = new AirbnbDB();
            return db.Amenities.ToList();
        }

        public static void UpdateAccount(Account account)
        {
            var db = new AirbnbDB();
            var acc = db.Accounts.FirstOrDefault(x => x.Id == account.Id);
            acc.FirstName = account.FirstName;
            acc.LastName = account.LastName;
            acc.Password = account.Password;
            acc.Image = account.Image;
            acc.BirthDate = account.BirthDate;
            acc.Password = account.Password;
            db.SaveChanges();
        }
    }
}

