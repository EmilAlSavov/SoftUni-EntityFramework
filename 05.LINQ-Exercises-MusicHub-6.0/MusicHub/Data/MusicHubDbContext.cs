namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Performer> Performers { get; set; }

        public DbSet<Producer> Producers { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<SongPerformer> SongsPerformers { get; set; }

        public DbSet<Writer> Writers { get; set; }
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SongPerformer>
                (
                    e => e.HasKey(e => new { e.SongId, e.PerformerId }
                ));

            //builder.Entity<Album>()
            //    .Property(a => a.Price)
            //    .HasComputedColumnSql("Select SUM(s.Price) FROM Albums AS a\r\nJOIN Songs AS s ON a.Id = s.AlbumId");
        }
    }
}
