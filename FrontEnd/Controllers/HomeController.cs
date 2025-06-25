using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
         public async Task<IActionResult> Login(string email, string password)
         {
             var loginDTO = new LoginDTO
             {
                 Email = email,
                 Password = password
             };

             var response = await _httpClient.PostAsJsonAsync("Auth/Login", loginDTO);

             if (response.IsSuccessStatusCode)
             {
                 var result = await response.Content.ReadFromJsonAsync<LoginResultDTO>();

                 // Guardar token y datos en sesión
                 HttpContext.Session.SetString("Token", result.Token);
                 HttpContext.Session.SetInt32("idUsuario", result.Id);
                HttpContext.Session.SetString("Rol", result.Rol);

                return RedirectToAction("Index");
             }

             ViewBag.Message = "Credenciales incorrectas";
             return View();
         }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            ViewBag.Rol = HttpContext.Session.GetString("RolLogueado");
            return View();
        }
    }
}
