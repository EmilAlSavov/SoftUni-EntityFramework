namespace Trucks.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
    using Trucks.Data.Models;

    public class TrucksContext : DbContext
    {
        public TrucksContext()
        { 
        }

        public DbSet<Truck> Trucks { get; set; }

        public DbSet<Despatcher> Despatchers { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientTruck> ClientsTrucks { get; set; }

        public TrucksContext(DbContextOptions options)
            : base(options) 
        { 
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientTruck>(ct => ct.HasKey(ct => new
            {
                ct.TruckId,
                ct.ClientId
            }));
        }
    }
}
