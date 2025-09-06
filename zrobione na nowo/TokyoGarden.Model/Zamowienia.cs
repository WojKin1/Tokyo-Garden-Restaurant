using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokyoGarden.Model
{
    public class Zamowienia
    {
        [Key] public int id { get; set; }
        public DateTime data_zamowienia { get; set; }
        public string? dodatkowe_informacje { get; set; }
        public string? status_zamowienia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal laczna_cena { get; set; }

        public string? metoda_platnosci { get; set; }
        public string? opcje_zamowienia { get; set; }

        public virtual ICollection<Pozycje_Zamowienia> pozycje_zamowienia { get; set; } = new List<Pozycje_Zamowienia>();
        public virtual Uzytkownicy? uzytkownik { get; set; }
    }
}