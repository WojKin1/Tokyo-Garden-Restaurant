namespace TokyoGarden.Api.DTOs
{
    public class PozycjaZamowieniaDTO
    {
        public int Id { get; set; }
        public int Ilosc { get; set; }
        public decimal Cena { get; set; }
        public PozycjaMenuDTO? PozycjaMenu { get; set; }
    }
}
