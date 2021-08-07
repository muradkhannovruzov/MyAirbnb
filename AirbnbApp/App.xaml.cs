using AirbnbApp.Services;
using AirbnbApp.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirbnbApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Container { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {

            Container = new Container();

            Container.Register<HomeVM>();
            Container.Register<SignInVM>();
            Container.RegisterSingleton<Messenger>();
            Container.RegisterSingleton<RegisterVM>();
            Container.RegisterSingleton<ProfileVM>();
            Container.RegisterSingleton<AddPublicationVM>();
            Container.RegisterSingleton<ProfileInfoVM>();

            Container.Register<IObjectSender, ObjectSender>();
            Container.RegisterSingleton<INotificationService, NotificationService>();
            Container.Register<IServerConnection, ServerConnection>();
            Container.Register<IConvertImage, ConvertImage>();


            base.OnStartup(e);
        }
    }
}
