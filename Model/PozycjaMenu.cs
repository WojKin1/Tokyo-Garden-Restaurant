using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class PozycjaMenu
    {
        [Key]
        public int id { get; set; }

        public double cena { get; set; }

        [MaxLength(25)]
        public string nazwa_pozycji { get; set; }

        [MaxLength(25)]
        public string opis { get; set; }

        [MaxLength(25)]
        public string skladniki { get; set; }

        // Relacja
        public int KategoriaId { get; set; }
        public virtual Kategoria Kategoria { get; set; }

        public virtual ICollection<AlergenPozycjaMenu> Alergeny { get; set; }
    }
}
