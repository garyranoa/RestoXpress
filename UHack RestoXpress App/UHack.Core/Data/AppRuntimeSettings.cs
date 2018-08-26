using System;
using SQLite;
namespace UHack.Core.Data
{
    public abstract class AppRuntimeSettingsBase : IAppRuntimeSettings
    {
        public abstract SQLiteAsyncConnection CreateSqLiteConnection();
        public string DatabaseFilename { get; } = "pfcashclub.db3";
    }
}
