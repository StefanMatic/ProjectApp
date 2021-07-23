using PhoneApp.Services;
using PhoneApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace PhoneApp
{
    public partial class App : Application
    {
        private APIClient _apiClient => DependencyService.Get<APIClient>();
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<APIClient>();
            MainPage = new LoadingPage();
        }

        protected override async void OnStart()
        {
            Trace.WriteLine("Here");
            bool _valid = await _apiClient.Validate();
            Trace.WriteLine(_valid);
            if (!_valid)
            {
                App.Current.MainPage = new LoginPage();

            }
            else
            {
                App.Current.MainPage = new AppShell();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
