using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Compartido.DTOs;
using System.Reflection;
namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [HttpGet]
        public IActionResult CambiarPassword()
        {
            return View(new CambioPasswordDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CambiarPassword(CambioPasswordDTO dto)
        {
            var token = HttpContext.Session.GetString("Token");
            var id = HttpContext.Session.GetInt32("idUsuario");

            if (string.IsNullOrEmpty(token) || id == null)
            {
                // Usuario no autenticado o sesión expiró
                return RedirectToAction("Login", "Home");
            }

            dto.Id = id.Value;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("usuarios/cambiarPassword", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "Contraseña cambiada correctamente.";
                return View(new CambioPasswordDTO()); // Limpio el formulario
            }
            else
            {
                ViewBag.Mensaje = "Password actual incorrecta.";
                return View(dto);
            }
        }

    }
}
