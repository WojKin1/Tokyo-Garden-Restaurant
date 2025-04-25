using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class AlergenPozycjaMenu
    {
        public int id_pozycja_menu { get; set; }
        public virtual PozycjaMenu PozycjaMenu { get; set; }

        public int id_alergen { get; set; }
        public virtual Alergeny Alergen { get; set; }
    }
}
