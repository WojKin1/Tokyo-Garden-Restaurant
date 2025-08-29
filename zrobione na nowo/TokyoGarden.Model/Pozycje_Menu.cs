using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokyoGarden.Model
{
    public class Pozycje_Menu
    {
        [Key] public int id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal cena { get; set; }

        public string? nazwa_pozycji { get; set; }
        public string? opis { get; set; }
        public string? skladniki { get; set; }
        public byte[]? image_data { get; set; }

        public virtual ICollection<Alergeny> alergeny { get; set; } = new List<Alergeny>();
        public virtual Kategorie? kategoria_menu { get; set; }
    }
}
