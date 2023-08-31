using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFGenericHost.Services;
using WPFGenericHost.ViewModels;
using WPFGenericHost.Views;

namespace WPFGenericHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    //对于桌面应用，AddScoped似乎和AddSingleton区别不大，建议用AddSingleton。
                    //AddSingleton用于长期驻留内存的服务或窗口；AddTransient用于临时的窗口和服务。
                    //需要注意的是，如果窗口有关闭后再打开的情况，建议使用AddTransient。使用AddSingleton关闭再打开会导致报错：
                    //“关闭窗口后，无法设置可见性，也无法调用 Show、ShowDialogor 或 WindowInteropHelper.EnsureHandle。”
                    //此时建议重写该窗口的OnClosing()方法：
                    /*
                     protected override void OnClosing(CancelEventArgs e)
                        {
                            e.Cancel = true;  // cancels the window close    
                            this.Hide();      // Programmatically hides the window
                        }
                     */

                    services.Configure<Settings>(context.Configuration);
                    services.AddSingleton<ITextService, TextService>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<Window1ViewModel>();
                    services.AddTransient<Window1>();

                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddNLog();
                })
                .Build();

            Services = _host.Services;

        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
