using System;
using System.IO;
using Lucky9.Application.Common.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace api.Startup
{
    public static class AppGlobal
    {
        public static IConfiguration Configuration { get; set; }

        public static AppSettings Settings
        {
            get
            {

                return AppGlobal.Configuration.Get<AppSettings>();

            }
        }
        public static IWebHostEnvironment Environment { get; set; }


    }


}

 
