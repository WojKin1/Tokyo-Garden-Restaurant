using System.Threading.Tasks;
using Moq;
using TokyoGarden.BL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.BL.Tests
{
    public class UzytkownikServiceMoqTests
    {
        [Fact]
        public async Task Add_Dodaje_Uzytkownika()
        {
            var repo = new Mock<IUzytkownikRepository>();
            var svc = new UzytkownikService(repo.Object);

            var u = new Uzytkownicy { nazwa_uzytkownika = "test", haslo = "Haslo123!" };
            await svc.AddAsync(u);

            repo.Verify(r => r.AddAsync(It.IsAny<Uzytkownicy>()), Times.Once);
        }

        [Fact]
        public async Task Update_Aktualizuje_Uzytkownika()
        {
            var repo = new Mock<IUzytkownikRepository>();
            var svc = new UzytkownikService(repo.Object);

            var u = new Uzytkownicy { id = 1, nazwa_uzytkownika = "nowy" };
            await svc.UpdateAsync(u);

            repo.Verify(r => r.UpdateAsync(It.IsAny<Uzytkownicy>()), Times.Once);
        }
    }

    public class PozycjeZamowieniaServiceMoqTests
    {
        [Fact]
        public async Task Update_Zmienia_Ilosc()
        {
            var repo = new Mock<IPozycjeZamowieniaRepository>();
            var svc = new PozycjeZamowieniaService(repo.Object);

            var item = new Pozycje_Zamowienia { id = 1, ilosc = 1, cena = 10 };
            await svc.UpdateAsync(item);

            repo.Verify(r => r.UpdateAsync(It.IsAny<Pozycje_Zamowienia>()), Times.Once);
        }
    }
}
