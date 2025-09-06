using System;
using System.Collections.Generic;

namespace TokyoGarden.Api.DTOs
{
    public class PozycjaMenuDTO
    {
        public int Id { get; set; }
        public string? Nazwa { get; set; }
        public decimal Cena { get; set; }
        public string? Opis { get; set; }
        public KategoriaDTO? Kategoria { get; set; }
        //public List<AlergenDTO> Alergeny { get; set; } = new();
    }
}
