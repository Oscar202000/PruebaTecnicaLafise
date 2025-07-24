using Aplicativo.Lafise.DTOs;
using Aplicativo.Lafise.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Aplicativo.Lafise.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ServicioApi");
        }

        public IActionResult Index()
        {
            return View(); // Vista index.cshtml con HTML + JS
        }

        // GET: Clientes/Obtener
        public async Task<IActionResult> ObtenerClientes()
        {
            try
            {
                var response = await _httpClient.GetAsync("Clientes/ListarClientes");
                if (!response.IsSuccessStatusCode) return BadRequest("Error al obtener clientes");

                var json = await response.Content.ReadAsStringAsync();
                var lista = JsonConvert.DeserializeObject<List<Clientes>>(json);
                return Json(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST: Clientes/Create
        [HttpPost]
        public async Task<IActionResult> CrearCLientesCuentas([FromBody] ClienteCuentaDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Clientes/CrearClientesCuentas", content);
                return response.IsSuccessStatusCode ? Ok() : StatusCode(500, "Error al crear el cliente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: Clientes/ObtenerPorId/5
        public async Task<IActionResult> ObtenerClientesPorId(long id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Clientes/ObtenerClientes/{id}");
                if (!response.IsSuccessStatusCode) return NotFound("Cliente no encontrado");

                var json = await response.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<Clientes>(json);
                return Json(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // PUT: Clientes/Actualizar
        [HttpPut]
        public async Task<IActionResult> ActualizarClientes([FromBody] Clientes cliente)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("Clientes/ActualizarClientes", content);
                return response.IsSuccessStatusCode ? Ok() : StatusCode(500, "Error al actualizar el cliente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // DELETE: Clientes/Eliminar/5
        [HttpDelete]
        public async Task<IActionResult> EliminarClientes(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Clientes/EliminarClientesCuentas?id={id}");
                return response.IsSuccessStatusCode ? Ok() : StatusCode(500, "Error al eliminar el cliente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}