using System;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using UHack.Core.Data;
using UHack.Core.Data.Domain;
using Xamarin.Forms;

namespace UHack.Core.Services
{
    public class UserService
    {

        SQLiteAsyncConnection _db;
        public UserService()
        {
            _db = DependencyService.Get<IAppRuntimeSettings>().CreateSqLiteConnection();
            _db.CreateTableAsync<User>().Wait();
        }

        public Task<User> GetById(int id)
        {
            return _db.Table<User>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> Insert(User user)
        {
            return _db.InsertAsync(user);
        }

        public async Task<int> Update(User user)
        {
            var result  = await _db.UpdateAsync(user);
            return result;
        }

    }
}
