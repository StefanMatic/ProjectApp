using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using System.Net.Http;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly APIClient<Project> _apiClient;
        private static string _route = "Projects";

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _apiClient = new APIClient<Project>(httpClient);
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(Exception ex)
        {
            return View(new ErrorViewModel { ErrorMessage = ex.Message });
        }

        public async Task<IActionResult> DisplayProjects()
        {
            try
            {
                var _response = await _apiClient.HttpGet(_route);
                List<Project> _projects = new List<Project>();
               //_projects = await JsonSerializer.DeserializeAsync<List<Project>>();
                //var _content = await _response.Content.ReadAsStringAsync();
                return View(_projects);
            }
            catch (HttpRequestException ex)
            {
                Error(ex);
                throw ex;
            }
        }
    }
}
