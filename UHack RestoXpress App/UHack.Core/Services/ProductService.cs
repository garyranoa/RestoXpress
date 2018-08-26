using System;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using UHack.Core.Data;
using UHack.Core.Data.Domain;
using Xamarin.Forms;
using System.Collections.Generic;

namespace UHack.Core.Services
{
    public class ProductService
    {

        SQLiteAsyncConnection _db;
        public ProductService()
        {
            _db = DependencyService.Get<IAppRuntimeSettings>().CreateSqLiteConnection();
            _db.CreateTableAsync<Product>().Wait();
        }

        public Task<List<Product>> GetAll()
        {
            return _db.Table<Product>().ToListAsync();
        }



        public Task<Product> GetById(int id)
        {
            return _db.Table<Product>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }


        public Task<int> Insert(Product product)
        {
            return _db.InsertAsync(product);
        }

        public Task<int> Update(Product product)
        {
            return _db.UpdateAsync(product);
        }

    }
}
