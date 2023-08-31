using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using Ecommerce.Servicios;
using Ecommerce.ViewModels;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(Constantes.UrlBase);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Producto"); // Cambia la ruta según tus endpoints
            if (response.IsSuccessStatusCode)
            {
                var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                return View(productos); // Puedes pasar la lista de productos a tu vista
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}