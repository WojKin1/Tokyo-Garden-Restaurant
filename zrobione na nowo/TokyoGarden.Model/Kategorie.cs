using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokyoGarden.Model
{
    public class Kategorie
    {
        [Key] public int id { get; set; }
        public string? nazwa_kategorii { get; set; }
        public virtual ICollection<Pozycje_Menu> pozycje_menu { get; set; } = new List<Pozycje_Menu>();
    }
}
