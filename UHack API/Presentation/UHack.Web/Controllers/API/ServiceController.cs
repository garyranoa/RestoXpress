using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Xml.Linq;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Text;
using UHack.Core.Domain.Users;
using UHack.Services.Authentication;
using UHack.Services.Common;
using UHack.Services.Security;
using System.Configuration;
using System.Runtime.Serialization.Json;
using UHack.Core;
using UHack.Web.Extensions;
using Newtonsoft.Json;

namespace UHack.Web.Controllers.API
{
    [RoutePrefix("api/Service")]
    public class ServiceController : ApiController
    {

        private readonly ICommonService _service;
        private readonly IEncryptionService _encryptionService;

        public ServiceController(ICommonService service,
                                IEncryptionService encryptionService)
        {
            this._service = service;
            this._encryptionService = encryptionService;
        }


        [HttpPost]
        [ActionName("register")]
        public async Task<IHttpActionResult> Register(User model)
        {

            string saltKey = _encryptionService.CreateSaltKey(5);
            string passwordSalt = saltKey;
            string password = _encryptionService.CreatePasswordHash(model.Password, saltKey);
            try
            {

                var user = new User()
                {
                    Username = model.Username,
                    Email = "",
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Phone = model.Phone,
                    ActiveInBusiness =true,
                    RoleId = model.RoleId,
                    Password = password,
                    PasswordFormat = PasswordFormat.Hashed,
                    PasswordSalt = passwordSalt,
                    LastLoginDate = DateTime.Now,
                    Active = true,
                    Deleted = false,
                    CreatedOn = DateTime.Now
                };
                _service.InsertUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [ActionName("user-info")]
        public async Task<IHttpActionResult> UserInfo(LoginInfo model)
        {
            try
            {
    
                var user = _service.GetUserByUsername(model.Username);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }


        }

        [HttpPost]
        [ActionName("save-product")]
        public async Task<IHttpActionResult> SaveProduct(Product model)
        {

            try
            {
                var product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserId = model.UserId,
                    CategoryId = 1,
                    CostPrice = model.CostPrice,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Location = model.Location,
                    ImageUrl = model.ImageUrl,
                    IsAvailable = true,
                    Active = true,
                    CreatedOn = DateTime.Now
                };
                _service.InsertProduct(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [ActionName("save-sales")]
        public async Task<IHttpActionResult> SaveSales(Sales model)
        {

            try
            {
                var sales = new Sales()
                {
                    InvoiceId = model.InvoiceId,
                    UserId = model.UserId,
                    ProductId = model.ProductId,
                    IsShipped = false,
                    Total = model.Total,
                    Markup = model.Markup,
                    CreatedOn = DateTime.Now
                };
                _service.InsertSales(sales);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [ActionName("redirect")]
        public IHttpActionResult UnionRedirect()
        {

            IList<Customer> customers = new List<Customer>();
            customers.Add(new Customer() { Name = "Nice customer", Address = "USA", Telephone = "123345456" });
            customers.Add(new Customer() { Name = "Good customer", Address = "UK", Telephone = "9878757654" });
            customers.Add(new Customer() { Name = "Awesome customer", Address = "France", Telephone = "34546456" });
            return Ok<IList<Customer>>(customers);
        }

        [HttpGet]
        [ActionName("test")]
        public IHttpActionResult Test()
        {
  
            IList<Customer> customers = new List<Customer>();
            customers.Add(new Customer() { Name = "Nice customer", Address = "USA", Telephone = "123345456" });
            customers.Add(new Customer() { Name = "Good customer", Address = "UK", Telephone = "9878757654" });
            customers.Add(new Customer() { Name = "Awesome customer", Address = "France", Telephone = "34546456" });
            return Ok<IList<Customer>>(customers);
        }


        public class Customer
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Telephone { get; set; }
        }

        public class LoginInfo
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

    }

    


}
