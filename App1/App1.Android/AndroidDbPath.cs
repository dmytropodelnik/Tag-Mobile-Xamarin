using App1.Droid;
using System.IO;
using System;
using Xamarin.Forms;
using App1.Interfaces;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace App1.Droid
{
    public class AndroidDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }
    }
}