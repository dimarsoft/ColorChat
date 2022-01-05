using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ColorChat.SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appName = Assembly.GetAssembly(typeof(Program))?.GetName().Name;
            //var dataTag = $"{DateTime.Now:yyyy-MM-dd_HH_mm_ss}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(string.Format($"{appName}.log"))
                .CreateLogger();            
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
