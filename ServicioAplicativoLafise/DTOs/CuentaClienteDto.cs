namespace Servicio.Lafise.DTOs
{
    public class CuentaClienteDto
    {

        public long IdCuentas { get; set; }
        public long IdClientes { get; set; }
        public decimal? SaldoDisponible { get; set; }

        public DateTime FechaApertura { get; set; }

        public string? Nombre { get; set; }

        public string? Identificacion { get; set; }
        public string? Estado { get; set; }

    }

}
