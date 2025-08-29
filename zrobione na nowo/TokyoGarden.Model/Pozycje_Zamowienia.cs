using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokyoGarden.Model
{
    public class Pozycje_Zamowienia
    {
        [Key] public int id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal cena { get; set; }

        public int ilosc { get; set; }

        public virtual Zamowienia? zamowienie { get; set; }
        public virtual Pozycje_Menu? pozycja_menu { get; set; }
    }
}
