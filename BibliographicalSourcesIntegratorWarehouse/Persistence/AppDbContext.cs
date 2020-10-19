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


        public DbSet<Article> Article { get; set; }

        public DbSet<Book> Book { get; set; }

        public DbSet<CongressComunication> CongressComunication { get; set; }

        public DbSet<Exemplar> Exemplar { get; set; }

        public DbSet<Journal> Journal { get; set; }

        public DbSet<Person> Person { get; set; }
    }
}
