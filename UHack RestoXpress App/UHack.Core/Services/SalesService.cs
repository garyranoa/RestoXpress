using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using UHack.Core.Data;
using UHack.Core.Data.Domain;
using Xamarin.Forms;

namespace UHack.Core.Services
{
    public class SalesService
    {

        SQLiteAsyncConnection _db;
        public SalesService()
        {
            _db = DependencyService.Get<IAppRuntimeSettings>().CreateSqLiteConnection();
            _db.CreateTableAsync<Sales>().Wait();
        }

        public Task<Sales> GetById(int id)
        {
            var result = _db.Table<Sales>().Where(w => w.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public Task<List<Sales>> GetAll()
        {
            var results = _db.Table<Sales>().ToListAsync();
            return results;
        }


        public Task<int> Insert(Sales sales)
        {
            return _db.InsertAsync(sales);
        }

        public Task<int> Update(Sales sales)
        {
            return _db.UpdateAsync(sales);
        }

    }
}
