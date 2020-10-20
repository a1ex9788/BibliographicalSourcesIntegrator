using BibliographicalSourcesIntegratorWarehouse.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorWarehouse.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Publication> Publications { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<CongressComunication> CongressComunications { get; set; }

        public DbSet<Exemplar> Exemplars { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Person> People { get; set; }
    }
}
