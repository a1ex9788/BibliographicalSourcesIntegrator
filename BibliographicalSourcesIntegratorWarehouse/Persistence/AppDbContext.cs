using BibliographicalSourcesIntegratorContracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace BibliographicalSourcesIntegratorWarehouse.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Publication> Publications { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<CongressComunication> CongressComunications { get; set; }

        public DbSet<Exemplar> Exemplars { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Person> People { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies()
                .UseSqlite("Data Source=BibliographicalSourcesIntegratorWarehouseDB.db");
        }
    }
}