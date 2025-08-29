using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokyoGarden.Model
{
    public class Uzytkownicy
    {
        [Key] public int id { get; set; }
        public string? nazwa_uzytkownika { get; set; }
        public string? haslo { get; set; }
        public string? telefon { get; set; }
        public string? typ_uzytkownika { get; set; }

        public virtual ICollection<Zamowienia> zamowienia { get; set; } = new List<Zamowienia>();
    }
}
