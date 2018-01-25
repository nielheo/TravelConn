using GeographyModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeographyService
{
    public class GeographyDbContext : DbContext
    {
        public readonly ILogger<GeographyDbContext> _logger;
        public readonly DbContextOptions _options;
        private bool _migrations;

        public GeographyDbContext()
        {
            _migrations = false;
        }

        public GeographyDbContext(DbContextOptions options, ILogger<GeographyDbContext> logger)
            : base(options)
        {
            _options = options;
            _logger = logger;
            Database.EnsureCreated();
            _migrations = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/relationships
            // http://stackoverflow.com/questions/38520695/multiple-relationships-to-the-same-table-in-ef7core

            #region Country
            // Country
            modelBuilder.Entity<Country>().HasKey(c => c.Code);
            modelBuilder.Entity<Country>().Property(e => e.Code)
                .HasColumnType("varchar(2)")
                .HasMaxLength(2)
                .ValueGeneratedNever();
            modelBuilder.Entity<Country>().Property(e => e.Name)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);
            modelBuilder.Entity<Country>().Property(e => e.Permalink)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);
            #endregion

            #region City
            // City
            modelBuilder.Entity<City>().HasKey(c => c.Code);
            modelBuilder.Entity<City>().Property(e => e.Code)
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .ValueGeneratedNever();
            modelBuilder.Entity<City>().Property(e => e.Name)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);
            modelBuilder.Entity<City>().Property(e => e.Permalink)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);
            modelBuilder.Entity<City>().Property(e => e.CountryCode)
                .HasColumnType("varchar(2)")
                .HasMaxLength(2);

            // City - Country
            modelBuilder.Entity<City>()
                .HasOne(h => h.Country)
                .WithMany(p => p.Cities)
                .HasForeignKey(h => h.CountryCode)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion City
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
    }
}
