using App1.Database;
using App1.Interfaces;
using App1.Models;
using App1.Services;
using App1.Views;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    public partial class App : Application
    {
        public const string DBFILENAME = "taggame.db";

        public App()
        {
            InitializeComponent();

            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                // Создаем бд, если она отсутствует
                db.Database.EnsureCreated();
                if (db.Users.Count() == 0)
                {
                    db.Users.Add(new User { Username = "Tom", Password = "123", });
                    db.Users.Add(new User { Username = "Alice", Password = "123", });
                    db.SaveChanges();
                }
            }

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
