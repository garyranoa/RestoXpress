﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(UHack.Web.Startup))]
namespace UHack.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
   
        }
        
    }

   
}