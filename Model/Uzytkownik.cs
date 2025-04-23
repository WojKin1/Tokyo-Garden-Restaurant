using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Uzytkownik
    {
        [Key]
        public int id { get; set; }
        [MaxLength(25)]
        public string nazwa_uzytkownika { get; set; }
        [MaxLength(15)]
        public string haslo { get; set; }
        [MaxLength(12)]
        public string telefon { get; set; }
        [MaxLength(50)]
        public int typ_uzytkownika { get; set; }

    }
}
