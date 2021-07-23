using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PhoneApp.Services;

namespace PhoneApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private APIClient _apiClient = new APIClient();
        public LoadingPage()
        {
            
        }

        public async void OnStart()
        {
            
        }
    }
}