using AirbnbApp.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Maps.MapControl.WPF;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AirbnbApp.ViewModels
{
    class MakeCommentVM : ViewModelBase
    {
        public ImageSource SelectedImage { get; set; }
        public bool EnableAccount { get; set; } = true;
        private IObjectSender objectSender;
        public MakeCommentVM(Publication publication, Account account)
        {
            objectSender = new ObjectSender(new ServerConnection());
            //Account = account;
            Publication = publication;
            LogInAccount = account;
            CurrLoc.Latitude = double.Parse(Publication.Home.lan);
            CurrLoc.Longitude = double.Parse(Publication.Home.lon);
            foreach (var item in publication.Comments)
            {
                if (item.AccountId == account.Id)
                {
                    Comment = item.Text;
                    Rating = item.Vote;
                    EnableAccount = false;
                    break;
                }
            }
        }
        public string Comment
        {
            get => comment; set
            {
                Set(ref comment, value);
                MakeComment.RaiseCanExecuteChanged();
            }
        }
        public Location CurrLoc { get; private set; } = new Location();
        public Publication Publication { get; }
        public Account LogInAccount;
        private RelayCommand makeComment;
        private string comment = "";
        private int rating = -1;

        public int Rating
        {
            get => rating; set
            {
                Set(ref rating, value);
                MakeComment.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand MakeComment => makeComment ?? (makeComment = new RelayCommand(() =>
        {
            EnableAccount = false;
            Task.Run(() =>
            {
                var Comment = new Comment();
                Comment.AccountName = LogInAccount.FirstName;
                Comment.AccountId = LogInAccount.Id;
                Comment.PublicationID = Publication.Id;
                Comment.Text = comment;
                Comment.Vote = rating;
                objectSender.SendObjectPorstURi(Comment,ProcessTypes.MakeComment);
            });

        }, () => Comment.Length > 1 && Rating>0));

        public Account Account { get; private set; }
    }
}
