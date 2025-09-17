using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TokyoGarden.DAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.DAL.Tests
{
    public class PozycjeMenuRepositoryQueryTests
    {
        private static DbTokyoGarden MakeDb()
            => new(new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"mem-{System.Guid.NewGuid()}").Options);

        [Fact]
        public async Task GetByAllergenAsync_Filters_Correctly()
        {
            using var db = MakeDb();
            var repo = new PozycjeMenuRepository(db);

            var al1 = new Alergeny{ id = 1, nazwa_alergenu = "Gluten"};
            var al2 = new Alergeny{ id = 2, nazwa_alergenu = "Soja"};

            var e1 = new Pozycje_Menu { nazwa_pozycji = "Soba", alergeny = new List<Alergeny>{ al1 } };
            var e2 = new Pozycje_Menu { nazwa_pozycji = "Tofu", alergeny = new List<Alergeny>{ al2 } };

            await repo.AddAsync(e1); await repo.AddAsync(e2);
            await db.SaveChangesAsync();

            var res = await repo.GetByAllergenAsync(2);
            Assert.Single(res);
            Assert.Equal("Tofu", res.First().nazwa_pozycji);
        }
    }
}
