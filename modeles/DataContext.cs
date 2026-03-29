using Microsoft.EntityFrameworkCore;
using PassKeep.modeles;



namespace PassKeep.modeles
{
    class DataContext : DbContext
    {

        ///  <param name="optionsBuilder">Options builder for configuring the database context.</param>

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source = bin/Debug/net10.0/DBPasskeep.db");
            }
        }
        public DbSet<PKUser> User { get; set; }
        public DbSet<DbDictionnaire> Dictionnaire { get; set; }
        public DbSet<ProfilConnexion> ProfilConnexion { get; set; }



    }
}

