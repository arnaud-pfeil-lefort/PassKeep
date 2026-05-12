using Microsoft.EntityFrameworkCore;
using PassKeep.modeles;
using System;
using System.IO;


namespace PassKeep.modeles
{
    class DataContext : DbContext
    {

        ///  <param name="optionsBuilder">Options builder for configuring the database context.</param>

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "DBPasskeep.db"
                );
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }
        public DbSet<PKUser> PKUser { get; set; }
        public DbSet<DbDictionnaire> Dictionnaire { get; set; }
        public DbSet<ProfilConnexion> ProfilConnexion { get; set; }
        public DbSet<TypeProfilConnexion> TypeProfilConnexion { get; set; }


    }
}

