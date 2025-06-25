using Microsoft.AspNetCore.Mvc;
using FrontEnd.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http;
using Compartido.DTOs;


namespace MVC.Controllers
{
    public class EnviosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5209/";
        public EnviosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }


        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> BuscarEnvio(string numTracking)
        {
            Console.WriteLine($"Tracking recibido: {numTracking}");
            if (string.IsNullOrWhiteSpace(numTracking))
            {
                ViewBag.Msg = "Debe ingresar un número de tracking válido.";
                return View("Buscar");
            }

            var response = await _httpClient.GetAsync($"{_baseUrl}Envios/{numTracking}");

            if (response.IsSuccessStatusCode)
            {
                var envio = await response.Content.ReadFromJsonAsync<EnvioDTO>();
                return View("Detalle", envio);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewBag.Msg = "No se encontro un envio con ese numero de tracking.";
            }
            else
            {
                ViewBag.Msg = "Ocurrio un error al consultar el envio";
            }

            return View("Buscar");
        }

        [HttpGet]
        public async Task<IActionResult> EnviosCliente()
        {
            var token = HttpContext.Session.GetString("Token");
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");

            if (string.IsNullOrEmpty(token) || idUsuario == null)
            {
                // Redirigir a Login si no hay token o id
                return RedirectToAction("Login", "Home");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"envios/cliente/{idUsuario}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "Error al obtener los envíos.";
                return View(new List<EnvioDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var envios = JsonSerializer.Deserialize<List<EnvioDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(envios);
        }

        [HttpGet]
        public async Task<IActionResult> SeguimientosAsync(int id)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirigir a Login si no hay token
                return RedirectToAction("Login", "Home");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"envios/{id}/seguimientos");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Error al obtener los seguimientos.";
                return View(new List<SeguimientoDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var seguimientos = JsonSerializer.Deserialize<List<SeguimientoDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(seguimientos);
        }

        [HttpGet]
        public IActionResult Filtrar()
        {
            return View(new FiltroEnvioDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Filtrar(FiltroEnvioDTO filtro)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirigir a Login si no hay token
                return RedirectToAction("Login", "Home");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var content = new StringContent(JsonSerializer.Serialize(filtro), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}envios/filtrar", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Msg = "Error al filtrar los envíos.";
                return View("Filtrar", filtro);
            }

            var json = await response.Content.ReadAsStringAsync();
            var envios = JsonSerializer.Deserialize<List<EnvioDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View("ListaFiltrada", envios);
        }
    }
}
