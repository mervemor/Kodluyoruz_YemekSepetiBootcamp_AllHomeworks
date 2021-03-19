using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MiddlewareLog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("LogsFile.txt")
            .CreateLogger();

            CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(conf =>
            {
                conf.ClearProviders();//daha �nce eklenmi� loglar� temizler
                conf.AddDebug(); //debug k�sm�nda da hata g�z�kecek
                conf.AddConsole(); //consoledan �al��t�r�lan bir uygulama ise consola basar 
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).UseSerilog();
        
    }
}
