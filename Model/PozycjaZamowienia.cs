using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class PozycjaZamowienia
    {
        [Key]
        public int id { get; set; }

        public double cena { get; set; }

        public int ilosc { get; set; }

        // Relacje
        public int ZamowienieId { get; set; }
        public virtual Zamowienie Zamowienie { get; set; }

        public int PozycjaMenuId { get; set; }
        public virtual PozycjaMenu PozycjaMenu { get; set; }
    }
}
