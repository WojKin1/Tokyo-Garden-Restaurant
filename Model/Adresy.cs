using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Adresy
    {
        [Key]
        public int id { get; set; }

        [MaxLength(25)]
        public string miasto { get; set; }

        public int nr_domu { get; set; }

        public int nr_mieszkania { get; set; }

        [MaxLength(50)]
        public string ulica { get; set; }

        public virtual ICollection<Zamowienie> Zamowienia { get; set; }
    }
}
