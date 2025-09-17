using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TokyoGarden.DAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.DAL.Tests
{
    public class OtherRepositoriesInMemoryTests
    {
        private static DbTokyoGarden MakeDb()
            => new(new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"mem-{System.Guid.NewGuid()}").Options);

        [Fact]
        public async Task Adresy_CRUD_Works()
        {
            using var db = MakeDb();
            var repo = new AdresyRepository(db);
            var a = new Adresy { ulica = "Test", miasto = "Poznań", nr_domu = "10" };
            await repo.AddAsync(a); await db.SaveChangesAsync();

            a.miasto = "Warszawa"; await repo.UpdateAsync(a); await db.SaveChangesAsync();
            var got = await repo.GetByIdAsync(a.id);
            Assert.Equal("Warszawa", got?.miasto);

            await repo.DeleteAsync(a.id); await db.SaveChangesAsync();
            var again = await repo.GetByIdAsync(a.id);
            Assert.Null(again);
        }
    }
}
