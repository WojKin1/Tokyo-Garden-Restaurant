using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TokyoGarden.BL;
using TokyoGarden.DAL;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

var builder = WebApplication.CreateBuilder(args);

// DbContext (czyta "DefaultConnection" z appsettings.json)
builder.Services.AddDbContext<DbTokyoGarden>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

// CORS – pozwala na połączenia z Angulara
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")      
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
    );
});

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUzytkownikRepository, UzytkownikRepository>();
builder.Services.AddScoped<IPozycjeMenuRepository, PozycjeMenuRepository>();
builder.Services.AddScoped<IKategorieRepository, KategorieRepository>();
builder.Services.AddScoped<IZamowieniaRepository, ZamowieniaRepository>();
builder.Services.AddScoped<IPozycjeZamowieniaRepository, PozycjeZamowieniaRepository>();
builder.Services.AddScoped<IAlergenyRepository, AlergenyRepository>();
builder.Services.AddScoped<IAdresyRepository, AdresyRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IUzytkownikService, UzytkownikService>();
builder.Services.AddScoped<IPozycjeMenuService, PozycjeMenuService>();
builder.Services.AddScoped<IKategorieService, KategorieService>();
builder.Services.AddScoped<IZamowieniaService, ZamowieniaService>();
builder.Services.AddScoped<IPozycjeZamowieniaService, PozycjeZamowieniaService>();
builder.Services.AddScoped<IAlergenyService, AlergenyService>();
builder.Services.AddScoped<IAdresyService, AdresyService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<DbTokyoGarden>();

    db.Database.Migrate(); // lub EnsureCreated();

    // SEED: Kategorie i Pozycje Menu
    if (!db.KategorieMenu.Any())
    {
        var sushi = new Kategorie { nazwa_kategorii = "Sushi" };
        var ramen = new Kategorie { nazwa_kategorii = "Ramen" };
        var napoje = new Kategorie { nazwa_kategorii = "Napoje" };

        db.KategorieMenu.AddRange(sushi, ramen, napoje);
        db.SaveChanges();

        db.PozycjeMenu.AddRange(
            new Pozycje_Menu { nazwa_pozycji = "California Roll", opis = "8 szt. łosoś+awokado", cena = 28m, skladniki = "ryż, łosoś, awokado", kategoria_menu = sushi },
            new Pozycje_Menu { nazwa_pozycji = "Ramen Miso", opis = "Bulion miso", cena = 35m, skladniki = "makaron, bulion miso, wieprzowina", kategoria_menu = ramen },
            new Pozycje_Menu { nazwa_pozycji = "Zielona herbata", opis = "Japońska herbata", cena = 10m, skladniki = "herbata", kategoria_menu = napoje }
        );

        db.SaveChanges();
    }

    // SEED: Użytkownicy
    if (!db.Uzytkownicy.Any())
    {
        var admin = new Uzytkownicy
        {
            nazwa_uzytkownika = "admin",
            haslo = "admin123",       // 🔒 w produkcji koniecznie hashować!
            telefon = "111222333",
            typ_uzytkownika = "Admin"
        };

        var user = new Uzytkownicy
        {
            nazwa_uzytkownika = "janek",
            haslo = "user123",        // 🔒 jw.
            telefon = "444555666",
            typ_uzytkownika = "Uzytkownik"
        };

        db.Uzytkownicy.AddRange(admin, user);
        db.SaveChanges();
    }
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Włączamy CORS **przed** MapControllers
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
