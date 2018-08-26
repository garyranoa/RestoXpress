using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
using System.Data.Entity;
using System.Threading.Tasks;
using UHack.Core;
using UHack.Core.Data;
using UHack.Core.Caching;
using UHack.Core.Domain.Users;
using System.Text.RegularExpressions;

namespace UHack.Services.Common
{
    /// <summary>
    /// Service
    /// </summary>
    public partial class CommonService : ICommonService
    {
        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Sales> _salesRepository;
        private readonly IRepository<Product> _productRepository;

        #endregion

        #region Constructor

        public CommonService(IRepository<User> userRepository,
                            IRepository<Sales> salesRepository,
                            IRepository<Product> productRepository)
        {
            this._userRepository = userRepository;
            this._salesRepository = salesRepository;
            this._productRepository = productRepository;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual User GetUserById(int userId)
        {
            if (userId == 0)
                return null;

            return _userRepository.GetById(userId);
        }


        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _userRepository.Table
                        where c.Username == username
                        select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="user"></param>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Insert(user);
        }


        /// <summary>
        /// Update a customer
        /// </summary>
        /// <param name="user"></param>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);
        }

        public virtual void InsertSales(Sales sales)
        {
            if (sales == null)
                throw new ArgumentNullException("sales");

            _salesRepository.Insert(sales);
        }

        public virtual void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            _productRepository.Insert(product);
        }
        #endregion

        public virtual async Task<IList<Product>> GetProducts(int userId)
        {

            var queryPredicate = PredicateBuilder.True<Product>();

            var query = _productRepository.TableNoTracking;

            if (userId > 0)
                query = query.Where(q => q.Active == true && q.UserId == userId);
            else
                query = query.Where(q => q.Active == true);

            query = query.OrderBy(o => o.Name);
            var wholeResults = query.AsExpandable().Where(queryPredicate);
            return await (from b in wholeResults
                          select b).ToListAsync();

        }

    }
}
