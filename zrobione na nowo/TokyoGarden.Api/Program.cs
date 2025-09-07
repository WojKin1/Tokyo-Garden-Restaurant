using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TokyoGarden.BL;
using TokyoGarden.DAL;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using BCrypt.Net;

// Tworzy obiekt konfiguracji aplikacji webowej ASP.NET Core
var builder = WebApplication.CreateBuilder(args);

// Rejestruje kontekst bazy danych z użyciem PostgreSQL i połączenia z konfiguracji
builder.Services.AddDbContext<DbTokyoGarden>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TokyoGarden.Api") // <-- migracje w projekcie API
    )
);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Konfiguruje opcje serializacji JSON dla kontrolerów API
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

// Definiuje politykę CORS – pozwala na dostęp z dowolnego frontendu (na zaliczenie)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
    );
});

// Rejestruje repozytoria danych w kontenerze DI jako zależności
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUzytkownikRepository, UzytkownikRepository>();
builder.Services.AddScoped<IPozycjeMenuRepository, PozycjeMenuRepository>();
builder.Services.AddScoped<IKategorieRepository, KategorieRepository>();
builder.Services.AddScoped<IZamowieniaRepository, ZamowieniaRepository>();
builder.Services.AddScoped<IPozycjeZamowieniaRepository, PozycjeZamowieniaRepository>();
builder.Services.AddScoped<IAlergenyRepository, AlergenyRepository>();
builder.Services.AddScoped<IAdresyRepository, AdresyRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Rejestruje serwisy logiki biznesowej w kontenerze DI
builder.Services.AddScoped<IUzytkownikService, UzytkownikService>();
builder.Services.AddScoped<IPozycjeMenuService, PozycjeMenuService>();
builder.Services.AddScoped<IKategorieService, KategorieService>();
builder.Services.AddScoped<IZamowieniaService, ZamowieniaService>();
builder.Services.AddScoped<IPozycjeZamowieniaService, PozycjeZamowieniaService>();
builder.Services.AddScoped<IAlergenyService, AlergenyService>();
builder.Services.AddScoped<IAdresyService, AdresyService>();

// Dodaje obsługę Swaggera do dokumentacji API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tworzy instancję aplikacji webowej z wcześniej skonfigurowanymi usługami
var app = builder.Build();

// Tworzy zakres usług i inicjalizuje bazę danych oraz dane startowe
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<DbTokyoGarden>();

    // Wykonuje migrację bazy danych lub tworzy ją jeśli nie istnieje
    db.Database.Migrate(); // lub EnsureCreated();

    // Dodaje domyślne kategorie menu jeśli baza jest pusta
    if (!db.KategorieMenu.Any())
    {
        var sushi = new Kategorie { nazwa_kategorii = "Sushi" };
        var ramen = new Kategorie { nazwa_kategorii = "Ramen" };
        var napoje = new Kategorie { nazwa_kategorii = "Napoje" };

        db.KategorieMenu.AddRange(sushi, ramen, napoje);
        db.SaveChanges();

        // Dodaje przykładowe pozycje menu przypisane do kategorii
        db.PozycjeMenu.AddRange(
            new Pozycje_Menu { nazwa_pozycji = "California Roll", opis = "8 szt. łosoś+awokado", cena = 28m, skladniki = "ryż, łosoś, awokado", kategoria_menu = sushi },
            new Pozycje_Menu { nazwa_pozycji = "Ramen Miso", opis = "Bulion miso", cena = 35m, skladniki = "makaron, bulion miso, wieprzowina", kategoria_menu = ramen },
            new Pozycje_Menu { nazwa_pozycji = "Zielona herbata", opis = "Japońska herbata", cena = 10m, skladniki = "herbata", kategoria_menu = napoje }
        );

        db.SaveChanges();
    }

    // Dodaje użytkowników z domyślnymi danymi i zaszyfrowanym hasłem
    if (!db.Uzytkownicy.Any())
    {
        var admin = new Uzytkownicy
        {
            nazwa_uzytkownika = "admin",
            haslo = BCrypt.Net.BCrypt.HashPassword("admin123"),
            telefon = "111222333",
            typ_uzytkownika = "Admin"
        };
        var user = new Uzytkownicy
        {
            nazwa_uzytkownika = "janek",
            haslo = BCrypt.Net.BCrypt.HashPassword("user123"),
            telefon = "444555666",
            typ_uzytkownika = "Uzytkownik"
        };
        db.Uzytkownicy.AddRange(admin, user);
        db.SaveChanges();
    }
}

// Włącza Swaggera (OPCJA B – zawsze, także w Production)  // <<< DODANE
app.UseSwagger();                                         // <<< DODANE
app.UseSwaggerUI();                                       // <<< DODANE

// Przekierowuje wszystkie żądania HTTP na HTTPS
app.UseHttpsRedirection();

// Serwowanie plików statycznych z katalogu wwwroot (HTML/CSS/JS)  // <<< DODANE
app.UseDefaultFiles();   // szuka index.html pod "/"                // <<< DODANE
app.UseStaticFiles();                                            // <<< DODANE

// Włącza politykę CORS przed uruchomieniem kontrolerów
app.UseCors("AllowFrontend");

// Włącza obsługę autentykacji i autoryzacji w aplikacji
app.UseAuthentication();
app.UseAuthorization();

// Mapuje kontrolery API na odpowiednie endpointy HTTP
app.MapControllers();

// Uruchamia aplikację webową
app.Run();
