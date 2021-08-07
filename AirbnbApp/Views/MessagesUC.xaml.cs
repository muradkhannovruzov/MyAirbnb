using AirbnbApp.Services;
using AirbnbApp.ViewModels;
using Models;
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
using System.Windows.Shapes;

namespace AirbnbApp.Views
{
    /// <summary>
    /// Interaction logic for MessagesUC.xaml
    /// </summary>
    public partial class MessagesUC : Window
    {
        public MessagesUC(Account a)
        {
            InitializeComponent();
            DataContext = new MessagesVM(a);
            MessagesListRefresh.Refresh += Refresh;
        }

        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        public void Refresh()
        {
            MyList.Items.Refresh();
        }
    }
}
