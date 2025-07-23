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
    public class ClientesController : ControllerBase
    {
        private readonly GestionBdContext _context;

        public ClientesController(GestionBdContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("ListarClientes")]
        public async Task<ActionResult<IEnumerable<Clientes>>> ListaClientes()
        {
            var Clientess = await _context.Clientes.ToListAsync();

            return Ok(Clientess);
        }

 
        [HttpPost]
        [Route("CrearClientesCuentas")]
        public async Task<IActionResult> CrearCUentaCliente([FromBody] ClienteCuentaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nombreParam = new SqlParameter("@Nombre", dto.Nombre ?? (object)DBNull.Value);
            var identificacionParam = new SqlParameter("@Identificacion", dto.Identificacion ?? (object)DBNull.Value);
            var saldoDisponibleParam = new SqlParameter("@SaldoDisponible",
                        dto.SaldoDisponible ?? (object)DBNull.Value);
            

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_InsertaClientesCuentas @Nombre, @Identificacion,  @SaldoDisponible",
                    nombreParam, identificacionParam, saldoDisponibleParam
                );

                return Ok(new { message = "Clientes creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno en el server al crear el Clientes");
            }
        }

        [HttpGet("ObtenerClientes/{id}")]
        public async Task<IActionResult> ObternerPorIDCLientes(long id)
        {
            var Clientes = await _context.Clientes.FindAsync(id);

            if (Clientes == null)
            {
                return NotFound();
            }

            return Ok(Clientes);
        }

        [HttpPut]
        [Route("ActualizarClientes")]
        public async Task<IActionResult> ActualizaClientes([FromBody] Clientes Clientes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var IDclienteParam = new SqlParameter("@IdClientes", Clientes.IdClientes);
            var nombreParam = new SqlParameter("@Nombre", Clientes.Nombre ?? (object)DBNull.Value);
            var identificacionParam = new SqlParameter("@Identificacion", Clientes.Identificacion ?? (object)DBNull.Value);



            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_ActualizaClientes @IdClientes, @Nombre, @Identificacion",
                    IDclienteParam, nombreParam , identificacionParam
                );

                return Ok(new { message = "Clientes actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al actualizar el Clientes");
            }
        }

        [HttpDelete]
        [Route("EliminarClientesCuentas")]
        public async Task<IActionResult> EliminarClientesCuentas (long id)
        {
            
            var cliente = await _context.Clientes
                .Include(c => c.Cuentas)
                .FirstOrDefaultAsync(c => c.IdClientes == id);

            if (cliente == null)
                return NotFound();

           
            if (cliente.Cuentas != null && cliente.Cuentas.Any())
            {
                _context.Cuentas.RemoveRange(cliente.Cuentas);
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente y sus cuentas eliminados correctamente" });
        }


    }
}