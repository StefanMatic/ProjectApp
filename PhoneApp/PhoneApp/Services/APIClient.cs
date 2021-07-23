using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using PhoneApp.Models;
using System.Diagnostics;
using Xamarin.Essentials;

namespace PhoneApp.Services
{
    public class APIClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public APIClient()
        {
            _httpClient.BaseAddress = new Uri("https://webapitask-milosa.azurewebsites.net/api/v1/");
        }

        public async Task<Stream> HttpPost<T>(string route, T input)
        {
            
            try
            {
                Stream _stream = null;
                await JsonSerializer.SerializeAsync<T>(_stream, input);
                Trace.WriteLine("Serializing");
                var _content = new StreamContent(_stream);
                using (var _httpResponse = await _httpClient.PostAsync(route,_content))
                {
                    Trace.WriteLine("Client post");
                    _httpResponse.EnsureSuccessStatusCode();
                    var _response = await _httpResponse.Content.ReadAsStreamAsync();
                    return _response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> Authenticate(AuthModel _auth)
        {
            Trace.WriteLine("HttpPost");
            try
            {
                Stream _stream = null;
                var test = JsonSerializer.Serialize<AuthModel>(_auth);
                Trace.WriteLine("Serializing");
                var _content = new StringContent(test, Encoding.UTF8, "application/json");
                using (var _httpResponse = await _httpClient.PostAsync("/api/v1/Auth", _content))
                {
                    Trace.WriteLine("Client post");
                    _httpResponse.EnsureSuccessStatusCode();
                    var test1 = await _httpResponse.Content.ReadAsStringAsync();
                    Trace.WriteLine(test1);
                    User _user = JsonSerializer.Deserialize<User>(test1, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Trace.WriteLine(_user.FirstName);
                    Trace.WriteLine(_user.Token);
                    return _user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Validate()
        {
            try
            {
                Trace.WriteLine("Validating");
                var _token = await SecureStorage.GetAsync("jwt_token");
                if (_token == null)
                {
                    return false;
                }
                else
                {
                    SetToken(_token);
                    using (var _httpResponse = await _httpClient.GetAsync("/api/v1/Auth/Validate"))
                    {
                        Trace.WriteLine("Client post");
                        Trace.WriteLine(_httpResponse.StatusCode);
                        if (_httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            return true;


                        SecureStorage.Remove("jwt_token");
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
