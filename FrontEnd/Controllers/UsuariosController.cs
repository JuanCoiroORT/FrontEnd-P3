using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5209/";
        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {

            var token = HttpContext.Session.GetString("Token");
            var id = HttpContext.Session.GetInt32("idUsuario");



            if (string.IsNullOrEmpty(token) || id == null)
            {
                // Redirigir a Login si no hay token o id
                return RedirectToAction("Login", "Home");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"usuarios/{id}");

            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = $"Error al obtener usuario: {response.StatusCode} - {body} {id} {token}";
                return View("Edit", new UsuarioDTO());
            }

            var usuarioDTO = JsonSerializer.Deserialize<UsuarioDTO>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(usuarioDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioDTO dto)
        {
            var token = HttpContext.Session.GetString("Token");
            var id = HttpContext.Session.GetInt32("idUsuario");

            if (string.IsNullOrEmpty(token) || id == null)
            {
                // Usuario no autenticado o sesión expiró
                return RedirectToAction("Login", "Home");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Trear el usuario actual
            var getResponse = await _httpClient.GetAsync($"usuarios/{id}");
            if (!getResponse.IsSuccessStatusCode)
            {
                ViewBag.Message = "No se pudo obtener el usuario.";
                return View(dto);
            }



            var json = await getResponse.Content.ReadAsStringAsync();
            var usuarioActual = JsonSerializer.Deserialize<UsuarioDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            // Rellenar los campos obligatorios
            dto.Id = usuarioActual.Id;
            dto.Nombre = usuarioActual.Nombre;
            dto.Apellido = usuarioActual.Apellido;
            dto.Email = usuarioActual.Email;
            dto.CI = usuarioActual.CI;
            dto.Rol = usuarioActual.Rol;

            var putContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var putResponse = await _httpClient.PutAsync($"{_baseUrl}usuarios/{id}", putContent);

            if (putResponse.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "Contraseña cambiada correctamente.";
            }
            else
            {
                var error = await putResponse.Content.ReadAsStringAsync();
                ViewBag.Mensaje = $"Error: {error}";
            }

            return View(dto);
        }

    }
}
