using System;
using System.Collections.Generic;
using System.Linq;
using UHack.Core;
using UHack.Core.Domain.Applications;
using UHack.Core.Domain.Configuration;
using UHack.Services.Applications;
using UHack.Services.Configuration;

namespace UHack.Web.Framework
{
    /// <summary>
    /// Application context for web application
    /// </summary>
    public partial class WebApplicationContext : IApplicationContext
    {
        private readonly IApplicationService _applicationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        private Application _cachedApplication;

        public WebApplicationContext(IApplicationService applicationService, ISettingService settingService, IWebHelper webHelper)
        {
            this._applicationService = applicationService;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets or sets the current application
        /// </summary>
        public virtual Application CurrentApplication
        {
            get
            {
                if (_cachedApplication != null)
                    return _cachedApplication;

                //ty to determine the current store by HTTP_HOST
                var host = _webHelper.ServerVariables("HTTP_HOST");
                var allApplications = _applicationService.GetAllApplications();
                var application = allApplications.FirstOrDefault(s => s.ContainsHostValue(host));

                if (application == null)
                {
                    //load the first found application
                    application = allApplications.FirstOrDefault();
                }

                if (application == null)
                {
                    //add defaut application for first installation
                    application = new Application();
                    application.Name = "Cash Club";
                    string hostUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
                    application.Url = hostUrl;
                    application.SslEnabled = false;
                    application.SecureUrl = "";
                    application.Hosts = "";
                    _applicationService.Insert(application);

                    #region Add default Settings

                    var setting = new Setting();
                    setting.Name = "seosettings.reservedurlrecordslugs";
                    setting.Value = "login,register,logout,page-not-found";
                    setting.ApplicationId = 1;
                    _settingService.InsertSetting(setting);

                    setting = new Setting();
                    setting.Name = "seosettings.allowunicodecharsinurls";
                    setting.Value = "lTrue";
                    setting.ApplicationId = 1;
                    _settingService.InsertSetting(setting);

                    #endregion
                }

                if (application == null)
                    throw new Exception("No application could be loaded");

                _cachedApplication = application;
                return _cachedApplication;
            }
        }
    }
}
