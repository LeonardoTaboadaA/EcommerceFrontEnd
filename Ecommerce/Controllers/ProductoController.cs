using Ecommerce.Models;
using Ecommerce.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Ecommerce.ViewModels;
using Azure;

namespace Ecommerce.Controllers
{
    public class ProductoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProductoController(IHttpClientFactory httpClientFactory, SignInManager<IdentityUser> signInManager )
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(Constantes.UrlBase);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _signInManager = signInManager;   
        }

        public async Task<IActionResult> DetalleProducto(int? id, string showSnackbar = "")
        {

            if (id == null)
            {
                return NotFound();
            }

            ViewBag.ShowSnackbar = showSnackbar;

            var response = await _httpClient.GetAsync($"api/Producto/{id}");

            if (response.IsSuccessStatusCode)
            {
                var producto = await response.Content.ReadFromJsonAsync<Producto>();
                var user = _signInManager.UserManager.GetUserAsync(User).Result;
                var proCarrito = new ProductoCarritoViewModel
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Imagen = producto.Imagen,
                    PrecioLista = producto.PrecioLista,
                    Precio = producto.Precio,
                    DsctoPorcentaje = producto.DsctoPorcentaje,
                    Descuento = producto.Descuento,
                    Stock = producto.Stock,
                    UserId = user.Id,
                    Cantidad = 1,
                    SubTotal = producto.Precio * 1,
                    Activo = true
                };
                
                return View(proCarrito); 
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            else
            {
                return View("Error");
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AñadirAlCarrito([Bind("IdProducto,Nombre,Descripcion,Imagen,PrecioLista,Precio,DsctoPorcentaje,Descuento,Stock,UserId,Cantidad,SubTotal,Activo")] ProductoCarritoViewModel carritoVM)
        {
            var userId = carritoVM.UserId;
            var idProducto = carritoVM.IdProducto;
            var cantSeleccionada = carritoVM.Cantidad;
            var stockActual = carritoVM.Stock;
            var precio = carritoVM.Precio;

            var rptaICExis = await _httpClient.GetAsync($"api/Carrito/Usuario/{userId}/Producto/{idProducto}");

            if (rptaICExis.IsSuccessStatusCode)
            {
                var itemCarExis = await rptaICExis.Content.ReadFromJsonAsync<Carrito>();
                var idCarrito = itemCarExis.IdCarrito;
                var cantExist = itemCarExis.Cantidad;
                var cantTotal = cantExist + cantSeleccionada;
                if (cantTotal <= stockActual)
                {
                    
                    var actCarrito = new Carrito
                    {
                        IdCarrito = idCarrito,
                        UserId = userId,
                        IdProducto = idProducto,
                        Cantidad = cantTotal,
                        SubTotal = cantTotal * precio,
                        Activo = carritoVM.Activo,
                    };

                    var jsonActCarrito = JsonSerializer.Serialize(actCarrito);
                    var contentActCarrito = new StringContent(jsonActCarrito, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"api/Carrito/{idCarrito}", contentActCarrito);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.ShowSnackbar = "AC";
                        return RedirectToAction("DetalleProducto", new { id = carritoVM.IdProducto, showSnackbar = ViewBag.ShowSnackbar });
                    }
                    
                    return View("Error");
                    

                    
                }
                else
                {
                    ViewBag.ShowSnackbar = "NHS";
                    return RedirectToAction("DetalleProducto", new { id = carritoVM.IdProducto, showSnackbar = ViewBag.ShowSnackbar });
                }

                
                
            }
            else if (rptaICExis.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var carrito = new Carrito
                {
                    IdCarrito = 0,
                    UserId = userId,
                    IdProducto = idProducto,
                    Cantidad = cantSeleccionada,
                    SubTotal = carritoVM.Cantidad * precio,
                    Activo = carritoVM.Activo,
                };

                var jsonCarrito = JsonSerializer.Serialize(carrito);
                var content = new StringContent(jsonCarrito, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Carrito", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.ShowSnackbar = "AC";
                    return RedirectToAction("DetalleProducto", new { id = carritoVM.IdProducto, showSnackbar = ViewBag.ShowSnackbar });
                }
                
                return View("Error");
                
            }
            else
            {
                return View("Error");
            }

            

        }

        
    }
}
