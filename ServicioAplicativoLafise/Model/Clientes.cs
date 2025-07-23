using System.ComponentModel.DataAnnotations;

namespace Servicio.Lafise.Model
{
    public class Clientes
    {
        [Key]
        public long IdClientes { get; set; }

        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public DateTime? FechaApertura { get; set; }
        public string? Nombre { get; set; }
        public string? Identificacion { get; set; }
        public ICollection<Cuentas>? Cuentas { get; set; }
    }
}
