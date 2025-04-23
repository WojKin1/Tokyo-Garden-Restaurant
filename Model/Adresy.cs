using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Adresy
    {
        [Key]
        public int id { get; set; }
        [MaxLength(25)]
        public string miasto { get; set; }
        [MaxLength(10)]
        public string nr_domu { get; set; }
        [MaxLength(25)]
        public string nr_mieszkania { get; set; }
        [MaxLength(50)]
        public string ulica { get; set; }
    }
}
