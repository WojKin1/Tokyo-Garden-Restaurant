using Xunit;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Custom
{
    public class CustomFakeTest
    {
        private class FakePozycjaMenuRepository : IPozycjaMenuRepository
        {
            private readonly List<PozycjaMenu> _pozycjeMenu = new List<PozycjaMenu>();

            public IEnumerable<PozycjaMenu> GetPozycjeMenu() => _pozycjeMenu;

            public PozycjaMenu GetPozycjaMenuByID(int id) => _pozycjeMenu.FirstOrDefault(p => p.id == id);

            public void InsertPozycjaMenu(PozycjaMenu pozycja)
            {
                if (_pozycjeMenu.Any())
                    pozycja.id = _pozycjeMenu.Max(p => p.id) + 1;
                else
                    pozycja.id = 1;

                _pozycjeMenu.Add(pozycja);
            }

            public void DeletePozycjaMenu(int id)
            {
                var pozycja = GetPozycjaMenuByID(id);
                if (pozycja != null)
                    _pozycjeMenu.Remove(pozycja);
            }

            public void UpdatePozycjaMenu(PozycjaMenu pozycja)
            {
                var index = _pozycjeMenu.FindIndex(p => p.id == pozycja.id);
                if (index != -1)
                    _pozycjeMenu[index] = pozycja;
            }

            public void Save() {}

            public void Dispose() {}
        }

        [Fact]
        public void FakeTest_ObslugaPozycjiMenu_PoliczPozycje()
        {
            var fakePozycjaMenuRepo = new FakePozycjaMenuRepository();

            fakePozycjaMenuRepo.InsertPozycjaMenu(new PozycjaMenu { nazwa_pozycji = "Sushi", cena = 35.99 });
            fakePozycjaMenuRepo.InsertPozycjaMenu(new PozycjaMenu { nazwa_pozycji = "Ramen", cena = 29.99 });
            fakePozycjaMenuRepo.InsertPozycjaMenu(new PozycjaMenu { nazwa_pozycji = "Tempura", cena = 25.50 });

            var obslugaPozycjiMenu = new ObslugaPozycjiMenu(fakePozycjaMenuRepo);

            var result = obslugaPozycjiMenu.PoliczPozycje();

            Assert.Equal(3, result);

            fakePozycjaMenuRepo.InsertPozycjaMenu(new PozycjaMenu { nazwa_pozycji = "Gyoza", cena = 19.99 });

            result = obslugaPozycjiMenu.PoliczPozycje();
            Assert.Equal(4, result);
        }
    }
}