using AirbnbApp.Messanging;
using AirbnbApp.Services;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace AirbnbApp.Views
{
    /// <summary>
    /// Interaction logic for HomeUC.xaml
    /// </summary>
    public partial class HomeUC : UserControl
    {

        private Messenger messenger;
        public HomeUC()
        {
            InitializeComponent();
            RefreshPublications.refresh += Refresh;
            messenger = App.Container.GetInstance<Messenger>();
            messenger.Register<HomeListChanged>(this,(x) =>
            {
                Map.Children.Clear();
                foreach (var item in x.Publications)
                {
                    var Pin = new Microsoft.Maps.MapControl.WPF.Pushpin();
                    Pin.Location = new Microsoft.Maps.MapControl.WPF.Location(double.Parse(item.Home.lan),double.Parse(item.Home.lon));
                    Pin.Content = item.Home.Address;
                    Map.Children.Add(Pin);
                }                
            });
        }
        public void Refresh()
        {
            PublicationList.Items.Refresh();
        }

    }
}
