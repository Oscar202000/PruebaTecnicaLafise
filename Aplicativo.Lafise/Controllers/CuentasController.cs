using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aplicativo.Lafise.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Aplicativo.Lafise.Controllers
{
    public class CuentasController : Controller
    {
        private readonly HttpClient _httpClient;

        public CuentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ServicioApi");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCuentas()
        {
            var response = await _httpClient.GetAsync("Cuentas/ListaCuentas");
            if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var cuentas = JsonConvert.DeserializeObject<List<CuentaClienteDto>>(json)
                          ?? new List<CuentaClienteDto>();
            return Json(cuentas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CuentaClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payload = JsonConvert.SerializeObject(dto);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Cuentas/CrearCuentasCuentas", content);
            return response.IsSuccessStatusCode
                ? Ok()
                : StatusCode((int)response.StatusCode);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarCuenta([FromBody] CuentaClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payload = JsonConvert.SerializeObject(dto);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("Cuentas/ActualizarCuentas", content);
            return response.IsSuccessStatusCode
                ? Ok()
                : StatusCode((int)response.StatusCode);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarCuenta(long id)
        {
            var response = await _httpClient.DeleteAsync($"Cuentas/EliminarCuentasCuentas?id={id}");
            return response.IsSuccessStatusCode
                ? Ok()
                : StatusCode((int)response.StatusCode);
        }
    }
}
