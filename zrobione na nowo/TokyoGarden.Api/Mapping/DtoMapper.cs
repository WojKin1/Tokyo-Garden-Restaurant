using System.Linq;
using TokyoGarden.Api.DTOs;
using TokyoGarden.Model;

namespace TokyoGarden.Api.Mapping
{
    public static class DtoMapper
    {
        public static UzytkownikDTO ToDto(this Uzytkownicy u) =>
            u == null ? null! : new UzytkownikDTO
            {
                Id = u.id,
                NazwaUzytkownika = u.nazwa_uzytkownika,
                Telefon = u.telefon,
                TypUzytkownika = u.typ_uzytkownika
            };

        public static AdresDTO ToDto(this Adresy a) =>
            a == null ? null! : new AdresDTO
            {
                Id = a.id,
                Miasto = a.miasto,
                Ulica = a.ulica,
                NrDomu = a.nr_domu,
                NrMieszkania = a.nr_mieszkania
            };

        public static KategoriaDTO ToDto(this Kategorie k) =>
            k == null ? null! : new KategoriaDTO
            {
                Id = k.id,
                NazwaKategorii = k.nazwa_kategorii
            };

        public static AlergenDTO ToDto(this Alergeny a) =>
            a == null ? null! : new AlergenDTO
            {
                Id = a.id,
                NazwaAlergenu = a.nazwa_alergenu
            };

        public static PozycjaMenuDTO ToDto(this Pozycje_Menu p) =>
            p == null ? null! : new PozycjaMenuDTO
            {
                Id = p.id,
                Nazwa = p.nazwa_pozycji,
                Cena = p.cena,
                Opis = p.opis,
                Kategoria = p.kategoria_menu?.ToDto(),
                Alergeny = p.alergeny?.Select(x => x.ToDto()).ToList() ?? new()
            };

        public static PozycjaZamowieniaDTO ToDto(this Pozycje_Zamowienia pz) =>
            pz == null ? null! : new PozycjaZamowieniaDTO
            {
                Id = pz.id,
                Ilosc = pz.ilosc,
                Cena = pz.cena,
                PozycjaMenu = pz.pozycja_menu?.ToDto()
            };

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
