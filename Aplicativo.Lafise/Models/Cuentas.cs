using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplicativo.Lafise.Models
{
    public class Cuentas
    {
        [Key]
        public long IdCuentas { get; set; }

        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public decimal? SaldoDisponible { get; set; }
        public string? Estado { get; set; }
        public DateTime FechaApertura { get; set; }
        public long IdClientes { get; set; }

        [ForeignKey("IdClientes")]
        public Clientes? Clientes { get; set; }
    }
}
