using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TokyoGarden.BL;
using TokyoGarden.DAL;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<DbTokyoGarden>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TokyoGarden.Api")
    )
);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

// CORS z credkami (cookie). Upewnij się, że tu jest domena frontu z Render.
var allowedOrigins = new[]
{
    "https://tokyo-garden-restaurant-1.onrender.com", // FRONT (Static Site)
    "http://localhost:4200"                            // dev
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials() // <<< niezbędne dla cookie
    );
});

// === SESSION (jeśli logowanie trzymasz w Session lub chcesz cookie cross-site) ===
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "tg.session";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;            // cross-site
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // https only
    options.IdleTimeout = TimeSpan.FromHours(12);
});

// DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUzytkownikRepository, UzytkownikRepository>();
builder.Services.AddScoped<IPozycjeMenuRepository, PozycjeMenuRepository>();
builder.Services.AddScoped<IKategorieRepository, KategorieRepository>();
builder.Services.AddScoped<IZamowieniaRepository, ZamowieniaRepository>();
builder.Services.AddScoped<IPozycjeZamowieniaRepository, PozycjeZamowieniaRepository>();
builder.Services.AddScoped<IAlergenyRepository, AlergenyRepository>();
builder.Services.AddScoped<IAdresyRepository, AdresyRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

// Migracje + seed (jak miałeś)
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<DbTokyoGarden>();
    db.Database.Migrate();

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

// Swagger (zostawiamy zawsze)
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

// Statyki (wwwroot)
app.UseDefaultFiles();
app.UseStaticFiles();

// Polityka cookies – wymusza None+Secure gdy trzeba
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});

// CORS z credkami – PRZED session/auth
app.UseCors("AllowFrontend");

// Session – PRZED auth, aby kontrolery miały dostęp do sesji
app.UseSession();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// API
app.MapControllers();

app.Run();
