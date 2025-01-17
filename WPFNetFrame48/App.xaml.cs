using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using WPFNetFrame48.Contracts.Services;
using WPFNetFrame48.Contracts.Views;
using WPFNetFrame48.Models;
using WPFNetFrame48.Services;
using WPFNetFrame48.ViewModels;
using WPFNetFrame48.Views;

namespace WPFNetFrame48
{
    public partial class App : Application
    {
        private IHost _host;
        private static Mutex _mutex = null;
        private const string MutexName = "WPFNetFrame48_SingleInstance";

        public T GetService<T>()
            where T : class
            => _host.Services.GetService(typeof(T)) as T;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX5fdHRcQ2JZWEd3W0o=");
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            // Controleer eerst op bestaande instantie
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Er draait al een instantie van de applicatie.", "Waarschuwing",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                Current.Shutdown();
                return;
            }

            // Vervolg met normale startup
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            _host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureAppConfiguration(c => { c.SetBasePath(appLocation); })
                .ConfigureServices(ConfigureServices)
                .Build();

            await _host.StartAsync();

            string pageArgument = e.Args.Length > 0 ? e.Args[0].ToLower() : "main";

            var navigationService = _host.Services.GetRequiredService<INavigationService>();
            switch (pageArgument)
            {
                case "settings":
                    navigationService.NavigateTo(typeof(SettingsViewModel).FullName);
                    break;
                case "treeview":
                    navigationService.NavigateTo(typeof(TreeViewViewModel).FullName);
                    break;
                default:
                    navigationService.NavigateTo(typeof(MainViewModel).FullName);
                    break;
            }
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Services
            services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
            services.AddSingleton<ISystemService, SystemService>();
            services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Views and ViewModels
            services.AddTransient<IShellWindow, ShellWindow>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<TreeViewViewModel>();
            services.AddTransient<TreeViewPage>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            _host = null;

            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
        }
    }
}