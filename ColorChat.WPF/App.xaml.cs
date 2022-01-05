using System.Reflection;
using ColorChat.WPF.Services;
using ColorChat.WPF.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using Serilog;

namespace ColorChat.WPF
{
    public partial class App
    {
        private const string HubUrl = "http://localhost:5000/colorchat";
        protected override void OnStartup(StartupEventArgs e)
        {
            var appName = Assembly.GetAssembly(typeof(App))?.GetName().Name;
            //var dataTag = $"{DateTime.Now:yyyy-MM-dd_HH_mm_ss}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(string.Format($"{appName}.log"))
                .CreateLogger();
            
            Log.Logger.Information("Start app");
            Log.Logger.Debug("{OnStartup}: {HubUrl}", nameof(OnStartup), HubUrl);
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(HubUrl)
                .Build();

            ColorChatViewModel chatViewModel = ColorChatViewModel.CreatedConnectedViewModel(new SignalRChatService(connection));

            MainWindow window = new MainWindow
            {
                DataContext = new MainViewModel(chatViewModel)
            };

            window.Show();
        }
    }
}
