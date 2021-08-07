using AirbnbApp.Messanging;
using AirbnbApp.Services;
using AirbnbApp.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace AirbnbApp.Views
{
    /// <summary>
    /// Interaction logic for AddPublicationUC.xaml
    /// </summary>
    ///
   
       
    public partial class AddPublicationUC : UserControl
    {

        public AddPublicationUC()
        {
            InitializeComponent();
            ComboBox1.ItemsSource = Enum.GetValues(typeof(HomeType)).Cast<HomeType>();
            ComboBox2.ItemsSource = Enum.GetValues(typeof(TypeOfPlace)).Cast<TypeOfPlace>();
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filename =  Path.GetFileName(files[0]);
                AddPublicationVM.action.Invoke(files[0]);
            }                
        }


        private void Map_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var location = Map.ViewportPointToLocation(e.GetPosition(Map));
            var messenger = App.Container.GetInstance<Messenger>();
            messenger.Send(new LocationMessage() { Latitude = location.Latitude, Longitude = location.Longitude });
            if(DataContext is AddPublicationVM a)
            {
                a.Longitude = location.Longitude.ToString();
                a.Latitude = location.Latitude.ToString();
            }
        }
    }
}
