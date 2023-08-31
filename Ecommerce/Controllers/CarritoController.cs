using Ecommerce.Models;
using Ecommerce.Servicios;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Ecommerce.Controllers
{
    public class CarritoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CarritoController(IHttpClientFactory httpClientFactory, SignInManager<IdentityUser> signInManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(Constantes.UrlBase);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = _signInManager.UserManager.GetUserAsync(User).Result;
            var idUsuario = user.Id;


            var response = await _httpClient.GetAsync($"api/Carrito/ProductosSeleccionados?IdUsuario={idUsuario}"); // Cambia la ruta según tus endpoints
            if (response.IsSuccessStatusCode)
            {
                var pSeleccionados = await response.Content.ReadFromJsonAsync<List<CarritoUsuario>>();
                
                return View(pSeleccionados); // Puedes pasar la lista de productos a tu vista
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> EliminarProductoDelCarrito(int id)
        {
            

            var response = await _httpClient.DeleteAsync($"api/Carrito/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Carrito"); // Redirect to some other page after successful deletion.
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                // Handle other error cases as needed.
                return View("Error");
            }
        }
    }
}
