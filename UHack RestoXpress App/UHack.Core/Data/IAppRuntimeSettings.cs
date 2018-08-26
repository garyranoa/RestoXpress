using System;
using SQLite;


//https://kalcik.net/2015/09/10/xamarin-and-autofac-how-dependencyservice-and-dependency-injection-are-working-together/

namespace UHack.Core.Data
{
    public interface IAppRuntimeSettings
    {
        SQLiteAsyncConnection CreateSqLiteConnection();
        string DatabaseFilename { get; }
    }
}
