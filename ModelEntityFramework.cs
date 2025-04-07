public class Adresy
{
    public int id { get; set; }
    public string miasto { get; set; }
    public string nr_domu { get; set; }
    public string nr_mieszkania { get; set; }
    public string ulica { get; set; }
}

public class Uzytkownicy
{
    public int id { get; set; }
    public string nazwa_uzytkownika { get; set; }
    public string haslo { get; set; }
    public string telefon { get; set; }
    public string typ_uzytkownika { get; set; }
    public virtual ICollection<Zamowienia> zamowienia { get; set; }
}

public class Zamowienia
{
    public int id { get; set; }
    public DateTime data_zamowienia { get; set; }
    public string dodatkowe_informacje { get; set; }
    public string status_zamowienia { get; set; }
    public decimal laczna_cena { get; set; }
    public string metoda_platnosci { get; set; }
    public string opcje_zamowienia { get; set; }
    public virtual ICollection<Pozycje_Zamowienia> pozycje_zamowienia { get; set; }
    public virtual Uzytkownicy uzytkownik { get; set; }
}

public class Pozycje_Zamowienia
{
    public int id { get; set; }
    public decimal cena { get; set; }
    public int ilosc { get; set; }
    public virtual Zamowienia zamowienie { get; set; }
    public virtual Pozycje_Menu pozycja_menu { get; set; }
}

public class Pozycje_Menu
{
    public int id { get; set; }
    public decimal cena { get; set; }
    public string nazwa_pozycji { get; set; }
    public string opis { get; set; }
    public string skladniki { get; set; }
    public byte[] image_data { get; set; }
    public virtual ICollection<Alergeny> alergeny { get; set; }
    public virtual Kategorie_Menu kategoria_menu { get; set; }
}

public class Kategorie_Menu
{
    public int id { get; set; }
    public string nazwa_kategorii { get; set; }
    public virtual ICollection<Pozycje_Menu> pozycje_menu { get; set; }
}

public class Alergeny
{
    public int id { get; set; }
    public string nazwa_alergenu { get; set; }
    public string opis_alergenu { get; set; }
    public virtual ICollection<Pozycje_Menu> pozycje_menu { get; set; }
}
