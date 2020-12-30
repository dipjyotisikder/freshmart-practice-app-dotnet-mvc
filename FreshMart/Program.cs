using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FreshMart
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();

        static string environment = "";

        public static void Main(string[] args)
        {
            environment = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Env").Value;
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>

            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((config, xonf) =>
            {
                xonf.AddJsonFile("appsettings.json");
                xonf.AddJsonFile($"appsettings.{environment}.json");
            }).UseStartup<Startup>();
    }
}
