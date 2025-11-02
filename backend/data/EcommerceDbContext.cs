using Microsoft.EntityFrameworkCore;
using Backend.features.Models;

namespace Backend.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }
        
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetalleOrdenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("roles");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.HasMany(r => r.Usuarios).WithOne(u => u.Rol).HasPrincipalKey(r => r.Id);
            });
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RolId).HasColumnName("rol_id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
                entity.Property(e => e.Estado).HasColumnName("estado");
                
                entity.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categorias");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");

            });
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("productos");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Marca).HasColumnName("marca");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.Stock).HasColumnName("stock");
                entity.Property(e => e.Imagen).HasColumnName("imagen");
                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);

            });
            modelBuilder.Entity<Orden>(entity =>
            {
                entity.ToTable("ordenes");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Total).HasColumnName("total");
                
                entity.HasMany(o => o.Detalles).WithOne(d => d.Orden).HasForeignKey(o => o.OrdenId);
                entity.HasOne(o => o.Usuario).WithMany(u=> u.Ordenes).HasForeignKey(o => o.UsuarioId);
            });

            modelBuilder.Entity<DetalleOrden>(entity =>
            {
                entity.ToTable("detalle_ordenes");
                entity.HasKey(dxo => new { dxo.OrdenId, dxo.ProductoId });

                entity.Property(e => e.OrdenId).HasColumnName("orden_id");
                entity.Property(e => e.ProductoId).HasColumnName("producto_id");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.Subtotal).HasColumnName("subtotal");
                
                entity.HasOne(e => e.Orden).WithMany(dxo => dxo.Detalles).HasForeignKey(dxo => dxo.OrdenId);
                entity.HasOne(e => e.Producto).WithMany(dxo => dxo.DetallesOrdenes).HasForeignKey(dxo => dxo.ProductoId);

            });
            
        }

    }
}