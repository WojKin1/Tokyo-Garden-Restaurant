using System.Linq;
using TokyoGarden.Api.DTOs;
using TokyoGarden.Model;

namespace TokyoGarden.Api.Mapping
{
    // Klasa pomocnicza do konwersji encji bazodanowych na obiekty DTO
    public static class DtoMapper
    {
        // Konwertuje encję użytkownika na obiekt DTO zawierający dane użytkownika
        public static UzytkownikDTO ToDto(this Uzytkownicy u) =>
            u == null ? null! : new UzytkownikDTO
            {
                Id = u.id,
                NazwaUzytkownika = u.nazwa_uzytkownika,
                Telefon = u.telefon,
                TypUzytkownika = u.typ_uzytkownika
            };

        // Konwertuje encję adresu na obiekt DTO zawierający dane adresowe
        public static AdresDTO ToDto(this Adresy a) =>
            a == null ? null! : new AdresDTO
            {
                Id = a.id,
                Miasto = a.miasto,
                Ulica = a.ulica,
                NrDomu = a.nr_domu,
                NrMieszkania = a.nr_mieszkania
            };

        // Konwertuje encję kategorii na obiekt DTO zawierający nazwę kategorii
        public static KategoriaDTO ToDto(this Kategorie k) =>
            k == null ? null! : new KategoriaDTO
            {
                Id = k.id,
                NazwaKategorii = k.nazwa_kategorii
            };

        // Konwertuje encję alergenu na obiekt DTO zawierający nazwę alergenu
        public static AlergenDTO ToDto(this Alergeny a) =>
            a == null ? null! : new AlergenDTO
            {
                Id = a.id,
                NazwaAlergenu = a.nazwa_alergenu
            };

        // Konwertuje encję pozycji menu na obiekt DTO z kategorią i podstawowymi danymi
        public static PozycjaMenuDTO ToDto(this Pozycje_Menu p) =>
            p == null ? null! : new PozycjaMenuDTO
            {
                Id = p.id,
                Nazwa = p.nazwa_pozycji,
                Cena = p.cena,
                Opis = p.opis,
                Kategoria = p.kategoria_menu?.ToDto(),
                // Alergeny mogą być dodane w przyszłości jeśli będą potrzebne
                //Alergeny = p.alergeny?.Select(x => x.ToDto()).ToList() ?? new()
            };

        // Konwertuje encję pozycji zamówienia na obiekt DTO z powiązaną pozycją menu
        public static PozycjaZamowieniaDTO ToDto(this Pozycje_Zamowienia pz) =>
            pz == null ? null! : new PozycjaZamowieniaDTO
            {
                Id = pz.id,
                Ilosc = pz.ilosc,
                Cena = pz.cena,
                PozycjaMenu = pz.pozycja_menu?.ToDto()
            };

        // Konwertuje encję zamówienia na obiekt DTO z użytkownikiem i listą pozycji
        public static ZamowienieDTO ToDto(this Zamowienia z) =>
            z == null ? null! : new ZamowienieDTO
            {
                Id = z.id,
                DataZamowienia = z.data_zamowienia,
                DodatkoweInformacje = z.dodatkowe_informacje,
                StatusZamowienia = z.status_zamowienia,
                LacznaCena = z.laczna_cena,
                MetodaPlatnosci = z.metoda_platnosci,
                OpcjeZamowienia = z.opcje_zamowienia,
                Uzytkownik = z.uzytkownik?.ToDto(),
                Pozycje = z.pozycje_zamowienia?.Select(p => p.ToDto()).ToList() ?? new()
            };
    }
}
