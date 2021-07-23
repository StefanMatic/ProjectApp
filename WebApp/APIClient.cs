using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http.Headers;
using WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApp
{
    public class APIClient<T>
    {
        //POST
        //GET
        //PUT
        //DELETE
        private readonly HttpClient _httpClient;
        public APIClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:44362/api/v1/");
        }


        public async Task<Stream> HttpGet(string route)
        {
            try
            {
                    using (var _response = await _httpClient.GetAsync(route, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                    { 
                        _response.EnsureSuccessStatusCode();
                        var _content = await _response.Content.ReadAsStreamAsync();
                        return _content;
                    }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}
