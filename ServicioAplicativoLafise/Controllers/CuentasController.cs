using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Servicio.Lafise.Data;
using Servicio.Lafise.Model;
using Servicio.Lafise.DTOs;

namespace Servicio.Lafise.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly GestionBdContext _context;

        public CuentasController(GestionBdContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("ListaCuentas")]
        public async Task<ActionResult<IEnumerable<CuentaClienteDto>>> ListaCuentas()
        {
            try
            {
                var cuentas = await _context.CuentaClienteDto
                    .FromSqlRaw("EXEC SP_ObtenerCuentas")
                    .ToListAsync();

                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("CrearCuentasCuentas")]
        public async Task<IActionResult> CrearCUentaCliente([FromBody] CuentaClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var idclienteParam = new SqlParameter("@Idcliente", dto.IdClientes );
            var saldoDisponibleParam = new SqlParameter("@SaldoDisponible",
                        dto.SaldoDisponible ?? (object)DBNull.Value);


            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_InsertaCuentas @Idcliente, @SaldoDisponible",
                    idclienteParam, saldoDisponibleParam
                );

                return Ok(new { message = "Cuentas creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno en el server al crear el Cuentas");
            }
        }

        [HttpGet("ObtenerCuentas/{id}")]
        public async Task<IActionResult> ObtenerPorIDCuentas(long id)
        {
            try
            {
                var cuentas = await _context.CuentaClienteDto
                    .FromSqlInterpolated($"EXEC SP_ObtenerCuentasPorId {id}")
                    .ToListAsync();

           
                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Error interno al procesar la petición.");
            }
        }

        [HttpPut]
        [Route("ActualizarCuentas")]
        public async Task<IActionResult> ActualizaCuentas([FromBody] Cuentas Cuentas)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var IDCuentaParam = new SqlParameter("@IdCuentas", Cuentas.IdCuentas);
            var EstadoParam = new SqlParameter("@Estado", Cuentas.Estado ?? (object)DBNull.Value);
            var saldoDisponibleParam = new SqlParameter("@SaldoDsisponible", Cuentas.SaldoDisponible ?? (object)DBNull.Value);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_ActualizaCuentas @IdCuentas, @Estado, @SaldoDsisponible",
                    IDCuentaParam, EstadoParam, saldoDisponibleParam
                );

                return Ok(new { message = "Cuentas actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al actualizar el Cuentas");
            }
        }

        [HttpDelete]
        [Route("EliminarCuentasCuentas")]
        public async Task<IActionResult> EliminarCuentasCuentas(long id)
        {
            var Cuentas = await _context.Cuentas.FindAsync(id);

            if (Cuentas == null)
                return NotFound();

            _context.Cuentas.Remove(Cuentas!);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cuenta eliminado correctamente" });

        }


    }
}