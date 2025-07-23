using Microsoft.EntityFrameworkCore;
using Servicio.Lafise.Model;
using Servicio.Lafise.DTOs;


namespace Servicio.Lafise.Data
{
    public class GestionBdContext : DbContext
    {
        public GestionBdContext(DbContextOptions<GestionBdContext> options) : base(options)
        {

        }

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Cuentas> Cuentas { get; set; }

        public DbSet<CuentaClienteDto> CuentaClienteDto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<CuentaClienteDto>()
                .HasNoKey()
                .ToView(null); 
        }


    }
}
