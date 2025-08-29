using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokyoGarden.Model
{
    public class Alergeny
    {
        [Key] public int id { get; set; }
        public string? nazwa_alergenu { get; set; }
        public string? opis_alergenu { get; set; }
        public virtual ICollection<Pozycje_Menu> pozycje_menu { get; set; } = new List<Pozycje_Menu>();
    }
}
