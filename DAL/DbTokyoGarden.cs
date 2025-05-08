using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class DbTokyoGarden : DbContext
    {
        // Konstruktor dla testów (InMemory DB)
        public DbTokyoGarden(DbContextOptions<DbTokyoGarden> options) : base(options)
        {
        }

        // Domyślny konstruktor – do użycia w normalnym uruchamianiu aplikacji
        public DbTokyoGarden()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TokyoGarden;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Klucz złożony dla relacji wiele-do-wielu (AlergenPozycjaMenu)
            modelBuilder.Entity<AlergenPozycjaMenu>()
                .HasKey(x => new { x.id_pozycja_menu, x.id_alergen });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Adresy> Adres { get; set; }
        public DbSet<Uzytkownik> uzytkownik { get; set; }
        public DbSet<Alergeny> alergenies { get; set; }
        public DbSet<AlergenPozycjaMenu> AlergenPozycjaMenu { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<PozycjaMenu> PozycjeMenu { get; set; }
        public DbSet<PozycjaZamowienia> PozycjeZamowienia { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
    }
}
