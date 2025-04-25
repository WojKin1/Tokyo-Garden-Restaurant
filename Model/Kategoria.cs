using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Kategoria
    {
        [Key]
        public int id { get; set; }

        [MaxLength(25)]
        public string nazwa_kategorii { get; set; }

        public virtual ICollection<PozycjaMenu> Pozycje { get; set; }
    }
}
