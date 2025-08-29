using System;
using System.Collections.Generic;

namespace TokyoGarden.Api.DTOs
{
    public class ZamowienieDTO
    {
        public int Id { get; set; }
        public DateTime DataZamowienia { get; set; }
        public string? DodatkoweInformacje { get; set; }
        public string? StatusZamowienia { get; set; }
        public decimal LacznaCena { get; set; }
        public string? MetodaPlatnosci { get; set; }
        public string? OpcjeZamowienia { get; set; }

        public UzytkownikDTO? Uzytkownik { get; set; }
        public List<PozycjaZamowieniaDTO> Pozycje { get; set; } = new();
    }
}
