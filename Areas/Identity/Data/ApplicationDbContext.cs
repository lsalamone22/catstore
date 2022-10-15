using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using catstore.Models;

namespace catstore.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contacto> DataContactos { get; set; }

     public DbSet<Producto> DataProductos { get; set; }

     public DbSet<PRO> DataPRO { get; set; }

     
    public DbSet<Pago> DataPago { get; set; }

    public DbSet<Pedido> DataPedido { get; set; }

    public DbSet<DetallePedido> DataDetallePedido { get; set; }  

}
