﻿using Microsoft.AspNetCore.Mvc;
using FrontEnd.Models;


namespace MVC.Controllers
{
    public class EnviosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5209/";
        public EnviosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Vista para buscar por numTracking
        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }


        //Accion que llama al endpoint
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
    }
}
