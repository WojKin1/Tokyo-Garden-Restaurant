using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Alergeny
    {
        [Key]
        public int id { get; set; }

        [MaxLength(25)]
        public string nazwa_alergenu { get; set; }

        [MaxLength(100)]
        public string opis_alergenu { get; set; }

        public virtual ICollection<AlergenPozycjaMenu> PozycjeMenu { get; set; }
    }
}
