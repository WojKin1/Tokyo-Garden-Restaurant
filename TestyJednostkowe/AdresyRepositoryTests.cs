using Microsoft.EntityFrameworkCore;
using Xunit;
using Model;
using DAL;
using IDAL;

public class AdresyRepositoryTests
{
    private DbContextOptions<DbTokyoGarden> GetInMemoryDbOptions()
    {
        return new DbContextOptionsBuilder<DbTokyoGarden>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;
    }

    [Fact]
    public void TestGetAdresy()
    {
        var options = GetInMemoryDbOptions();
        var context = new DbTokyoGarden(options);

        context.Adres.Add(new Adresy { miasto = "Warszawa" });
        context.Adres.Add(new Adresy { miasto = "Kraków" });
        context.SaveChanges();

        var repository = new AdresyRepository(context);

        var adresy = repository.GetAdresy();

        Assert.Equal(2, adresy.Count());
    }

    [Fact]
    public void TestAddAdres()
    {
        var options = GetInMemoryDbOptions();
        var context = new DbTokyoGarden(options);

        var repository = new AdresyRepository(context);
        var adres = new Adresy { miasto = "Wroc³aw" };

        repository.InsertAdres(adres);
        repository.Save();

        var savedAdres = context.Adres.Find(adres.id);
        Assert.NotNull(savedAdres);
        Assert.Equal("Wroc³aw", savedAdres.miasto);
    }
}
