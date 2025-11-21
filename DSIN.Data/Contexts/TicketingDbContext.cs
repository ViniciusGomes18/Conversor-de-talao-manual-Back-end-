using DSIN.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DSIN.Data.Contexts
{
    public sealed class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
            : base(options) { }

        public DbSet<Agent> Agents { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<TicketBook> TicketBooks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // ===== Agents =====
            modelBuilder.Entity<Agent>(e =>
            {
                e.ToTable("Agents");
                e.HasKey(x => x.Id);

                // Guid -> deixa o provider (Npgsql) mapear para uuid
                e.Property(x => x.Id)
                    .ValueGeneratedNever();

                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.Email).HasMaxLength(180).IsRequired();
                e.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();

                e.HasIndex(x => x.Email).IsUnique();
            });

            // ===== Drivers =====
            modelBuilder.Entity<Driver>(e =>
            {
                e.ToTable("Drivers");
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .ValueGeneratedNever();

                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.CPF).HasMaxLength(11).IsRequired();

                e.HasIndex(x => x.CPF);
            });

            // ===== Vehicles =====
            modelBuilder.Entity<Vehicle>(e =>
            {
                e.ToTable("Vehicles");
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .ValueGeneratedNever();

                e.Property(x => x.Plate).HasMaxLength(8).IsRequired();
                e.Property(x => x.Model).HasMaxLength(100).IsRequired();
                e.Property(x => x.Color).HasMaxLength(50).IsRequired();

                e.HasIndex(x => x.Plate);
            });

            // ===== TicketBooks =====
            modelBuilder.Entity<TicketBook>(e =>
            {
                e.ToTable("TicketBooks");
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .ValueGeneratedNever();

                e.Property(x => x.AgentId)
                    .IsRequired();

                e.Property(x => x.VehicleId)
                    .IsRequired();

                e.Property(x => x.DriverId);

                e.Property(x => x.PlateSnapshot).HasMaxLength(10).IsRequired();
                e.Property(x => x.VehicleModelSnapshot).HasMaxLength(100).IsRequired();
                e.Property(x => x.VehicleColorSnapshot).HasMaxLength(50).IsRequired();
                e.Property(x => x.DriverNameSnapshot).HasMaxLength(150);
                e.Property(x => x.DriverCpfSnapshot).HasMaxLength(11);
                e.Property(x => x.ViolationCode).HasMaxLength(20).IsRequired();
                e.Property(x => x.ViolationDescription).HasMaxLength(500).IsRequired();
                e.Property(x => x.Location).HasMaxLength(200);
                // deixa o provider mapear o tipo de data/hora
                e.Property(x => x.OccurredAt).IsRequired();

                e.HasIndex(x => x.OccurredAt);
                e.HasIndex(x => x.PlateSnapshot);
                e.HasIndex(x => x.AgentId);

                e.HasOne(x => x.Agent)
                    .WithMany()
                    .HasForeignKey(x => x.AgentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Vehicle)
                    .WithMany()
                    .HasForeignKey(x => x.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Driver)
                    .WithMany()
                    .HasForeignKey(x => x.DriverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}