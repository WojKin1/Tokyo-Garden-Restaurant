using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Zamowienie
    {
        [Key]
        public int id { get; set; }

        public DateTime data_zamowienia { get; set; }

        [MaxLength(50)]
        public string dodatkowe_informacje { get; set; }

        [MaxLength(12)]
        public string status_zamowienia { get; set; }

        public double laczna_cena { get; set; }

        [MaxLength(25)]
        public string metoda_platnosci { get; set; }

        [MaxLength(50)]
        public string opcje_zamowienia { get; set; }

        // Relacje
        public int UzytkownikId { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }

        public int AdresId { get; set; }
        public virtual Adresy Adres { get; set; }

        public virtual ICollection<PozycjaZamowienia> PozycjeZamowienia { get; set; }
    }
}
