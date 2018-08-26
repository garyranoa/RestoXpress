using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using UHack.Core;
using UHack.Core.Caching;
using UHack.Core.Configuration;
using UHack.Core.Data;
using UHack.Core.Fakes;
using UHack.Core.Infrastructure;
using UHack.Core.Infrastructure.DependencyManagement;
using UHack.Core.Plugins;
using UHack.Core.Domain.Localization;
using UHack.Core.Domain.Security;
using UHack.Core.Domain.Seo;
using UHack.Data;
using UHack.Services.Localization;
using UHack.Services.Seo;
using UHack.Services.Applications;
using UHack.Services.Authentication;
using UHack.Services.Configuration;
using UHack.Services.Security;
using UHack.Services.Common;
using UHack.Services.Events;
using UHack.Services.Logging;
using UHack.Services.Tasks;
using UHack.Web.Framework.Mvc.Routes;
using UHack.Web.Framework.UI;

namespace UHack.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();


            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();


            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                builder.Register<IDbContext>(c => new AppObjectContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
            }
            else
            {
                builder.Register<IDbContext>(c => new AppObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
            }

            //Web Api
            //builder.RegisterType<ApplicationDbContext>().InstancePerHttpRequest();
            //builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>();
            //builder.RegisterType<UserManager<ApplicationUser>>();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("UHack_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("UHack_cache_per_request").InstancePerLifetimeScope();

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            //application context
            builder.RegisterType<WebApplicationContext>().As<IApplicationContext>().InstancePerLifetimeScope();

            //services
            builder.RegisterType<CommonService>().As<ICommonService>().InstancePerLifetimeScope();
  

          
            
            builder.RegisterType<ApplicationService>().As<IApplicationService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<UrlRecordService>().As<IUrlRecordService>().InstancePerLifetimeScope();

            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();


            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();

            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<SettingService>().As<ISettingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("UHack_cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());

            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();

            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();            

        }

        public int Order
        {
            get { return 0; }
        }

        public class SettingsSource : IRegistrationSource
        {
            static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
                "BuildRegistration",
                BindingFlags.Static | BindingFlags.NonPublic);

            public IEnumerable<IComponentRegistration> RegistrationsFor(
                    Service service,
                    Func<Service, IEnumerable<IComponentRegistration>> registrations)
            {
                var ts = service as TypedService;
                if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
                {
                    var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                    yield return (IComponentRegistration)buildMethod.Invoke(null, null);
                }
            }

            static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
            {
                return RegistrationBuilder
                    .ForDelegate((c, p) =>
                    {
                        var currentApplicationId = c.Resolve<IApplicationContext>().CurrentApplication.Id;
                        //uncomment the code below if you want load settings per store only when you have two stores installed.
                        //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                        //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                        //although it's better to connect to your database and execute the following SQL:
                        //DELETE FROM [Setting] WHERE [StoreId] > 0
                        int appId = currentApplicationId;
                        return c.Resolve<ISettingService>().LoadSetting<TSettings>(appId);
                    })
                    .InstancePerLifetimeScope()
                    .CreateRegistration();
            }

            public bool IsAdapterForIndividualComponents { get { return false; } }
        }
    }
}
