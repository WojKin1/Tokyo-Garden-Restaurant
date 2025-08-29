using Microsoft.EntityFrameworkCore;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class DbTokyoGarden : DbContext
    {
        public DbTokyoGarden(DbContextOptions<DbTokyoGarden> options) : base(options) { }

        public DbSet<Adresy> Adresy { get; set; }
        public DbSet<Uzytkownicy> Uzytkownicy { get; set; }
        public DbSet<Zamowienia> Zamowienia { get; set; }
        public DbSet<Pozycje_Zamowienia> PozycjeZamowienia { get; set; }
        public DbSet<Pozycje_Menu> PozycjeMenu { get; set; }
        public DbSet<Kategorie> KategorieMenu { get; set; }
        public DbSet<Alergeny> Alergeny { get; set; }
    }
}
