using System.Diagnostics;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7280/";
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Home/Login", loginDTO);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResultDTO>();

                // Guardar token y datos en sesión
                HttpContext.Session.SetString("Token", result.Token);
                HttpContext.Session.SetString("Email", result.Email);
                HttpContext.Session.SetString("Nombre", result.Nombre);
                HttpContext.Session.SetString("RolLogueado", "Cliente"); // si no viene del API

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
