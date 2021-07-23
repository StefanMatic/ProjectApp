using PhoneApp.ViewModels;
using PhoneApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace PhoneApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void LogOut(object sender, EventArgs e)
        {
            SecureStorage.Remove("jwt_token");
            App.Current.MainPage = new LoginPage();
            await Navigation.PopToRootAsync();
        }
    }
}
