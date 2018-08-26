using System;
using System.IO;
using SQLite;

namespace CashClubApp.Core.Data
{
    public class CashClubAppConnection 
    {

        string _sqliteDBFilename = "pfcashclub.db3";
        PlatformType _platformType;

        public CashClubAppConnection(PlatformType platformType = PlatformType.IOS)
        {
            _platformType = platformType;
        }

        public virtual SQLiteAsyncConnection GetConnection()   
        {
            string documentsPath = string.Empty;
            string path = string.Empty;

            if (_platformType == PlatformType.Android)
            {
                documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                path = Path.Combine(documentsPath, _sqliteDBFilename);
            }
            else if (_platformType == PlatformType.IOS)
            {
                documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string libraryPath = Path.Combine(documentsPath, "..", "Library");
                path = Path.Combine(libraryPath, _sqliteDBFilename);
            }
            if (_platformType == PlatformType.WindowsPhone)
            {
                documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                path = Path.Combine(documentsPath, _sqliteDBFilename);
            }

            return new SQLiteAsyncConnection(path);

        }
    }
}