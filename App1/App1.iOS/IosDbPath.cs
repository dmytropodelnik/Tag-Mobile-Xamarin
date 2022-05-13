using System;
using Xamarin.Forms;
using System.IO;
using App1.iOS;
using App1.Interfaces;

[assembly: Dependency(typeof(IosDbPath))]
namespace App1.iOS
{
    public class IosDbPath : IPath
    {
        public string GetDatabasePath(string sqliteFilename)
        {
            // определяем путь к бд
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", sqliteFilename);
        }
    }
}