using RestDbExample.Database;
using RestDbExample.Services;
using RestDbExample.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RestDbExample
{
    public class App : Application
    {
        static DatabaseManager database;
        public static string RestUrl = "https://api.producthunt.com{0}";
        public static string AccessToken = "b423dc09e5642f0a4c00979c031f3ac49646aedf098764f5f506a0f936262d0c";
        public static ConnectionManager ConnectionManager { get; private set; }

        public App()
        {
            ConnectionManager = new ConnectionManager(new ServiceBroker());
            MainPage = new NavigationPage(new MainPage());
        }

        public static DatabaseManager Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseManager();
                }
                return database;
            }
        }

        public static bool HasInternetConnection
        {
            get
            {
                return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
