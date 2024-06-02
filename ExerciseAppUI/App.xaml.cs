using ExerciseAppUI.AppWindows;
using ExerciseAppUI.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Storage;

namespace ExerciseAppUI
{
    public partial class App : Application
    {
        private IHost _host;
        public App()
        {
            this.InitializeComponent();

            // Get theme choice from LocalSettings.
            object value = ApplicationData.Current.LocalSettings.Values["themeSetting"];

            if (value != null)
            {
                // Apply theme choice.
                App.Current.RequestedTheme = (ApplicationTheme)(int)value;
            }
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();

            _host.Start();

            loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            
            loginWindow.Activate();
        }

        internal Window loginWindow { get; set; }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<LoginWindow>();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
