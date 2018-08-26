using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UHack.Core.Domain.Users;

namespace UHack.Services.Common
{
    /// <summary>
    /// Service Interface
    /// </summary>
    public partial interface ICommonService
    {
        User GetUserById(int userId);


        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetUserByUsername(string username);

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="user"></param>
        void InsertUser(User user);


        /// <summary>
        /// Update a customer
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);

        void InsertSales(Sales sales);

        void InsertProduct(Product product);

        Task<IList<Product>> GetProducts(int userId);
    }

}
