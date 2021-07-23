using PhoneApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;
using PhoneApp.Services;
using PhoneApp.Models;
using System.Threading.Tasks;

namespace PhoneApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private AuthModel _authModel;
        private string username;
        private string password;
        private string response;
        public Command LoginCommand { get; }
        private APIClient _apiClient = new APIClient();

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await OnLoginClicked());
        }

        private async Task OnLoginClicked()
        {
            Trace.WriteLine("Login clicked");
            try
            {
                _authModel = new AuthModel
                {
                    UserName = username,
                    Password = password
                };
                var test = await _apiClient.Authenticate(_authModel);
                Trace.WriteLine("Authenticated");
                Trace.WriteLine(test.Token);
                await SecureStorage.SetAsync("jwt_token", test.Token);
                Trace.WriteLine("Secure storage");
                //response = test;
                Trace.WriteLine(test);
                if (await _apiClient.Validate())
                {
                    App.Current.MainPage = new AppShell();

                }
                else
                {
                    Response = "Login failed";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string Response
        {
            get => response;
            set => SetProperty(ref response, value);
        }
    }
}
