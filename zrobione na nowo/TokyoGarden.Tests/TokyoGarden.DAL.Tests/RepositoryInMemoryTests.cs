using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TokyoGarden.DAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.DAL.Tests
{
    public class RepositoryInMemoryTests
    {
        private static DbTokyoGarden MakeDb()
            => new(new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"mem-{System.Guid.NewGuid()}").Options);

        [Fact]
        public async Task Add_And_GetAll()
        {
            using var db = MakeDb();
            var repo = new PozycjeMenuRepository(db);

            await repo.AddAsync(new Pozycje_Menu { nazwa_pozycji = "Ramen", cena = 39m });
            await repo.AddAsync(new Pozycje_Menu { nazwa_pozycji = "Sushi", cena = 49m });
            await db.SaveChangesAsync();

            var all = await repo.GetAllAsync();
            Assert.Equal(2, all.Count());
        }

        [Fact]
        public async Task Update_Changes_Are_Persisted()
        {
            using var db = MakeDb();
            var repo = new PozycjeMenuRepository(db);

            var e = new Pozycje_Menu { nazwa_pozycji = "Temp", cena = 10m };
            await repo.AddAsync(e);
            await db.SaveChangesAsync();

            e.nazwa_pozycji = "Updated";
            await repo.UpdateAsync(e);
            await db.SaveChangesAsync();

            var got = await repo.GetByIdAsync(e.id);
            Assert.Equal("Updated", got?.nazwa_pozycji);
        }

        [Fact]
        public async Task Delete_Removes_Entity()
        {
            using var db = MakeDb();
            var repo = new PozycjeMenuRepository(db);

            var e = new Pozycje_Menu { nazwa_pozycji = "Del", cena = 10m };
            await repo.AddAsync(e);
            await db.SaveChangesAsync();

            await repo.DeleteAsync(e.id);
            await db.SaveChangesAsync();

            var all = await repo.GetAllAsync();
            Assert.DoesNotContain(all, x => x.id == e.id);
        }
    }
}
