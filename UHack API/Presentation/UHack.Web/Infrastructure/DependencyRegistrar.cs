using Autofac;
using Autofac.Core;
using UHack.Core.Caching;
using UHack.Core.Infrastructure;
using UHack.Core.Infrastructure.DependencyManagement;
using UHack.Web.Controllers;
using UHack.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using UHack.Web.Models;
using System.Reflection;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;

namespace UHack.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
            builder.RegisterType<HomeController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("UHack_cache_static"));


            builder.RegisterType<KeepAliveController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("UHack_cache_static"));

           

        }

        public int Order
        {
            get { return 2; }
        }
    }
}