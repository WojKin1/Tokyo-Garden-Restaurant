using Xunit;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Custom
{
    public class CustomSpyTest
    {
        private class SpyZamowienieRepository : IZamowienieRepository
        {
            private readonly List<Zamowienie> _zamowienia;

            private List<object[]> _getZamowieniaCalls = new List<object[]>();

            public SpyZamowienieRepository(List<Zamowienie> zamowienia)
            {
                _zamowienia = zamowienia;
            }

            public IEnumerable<Zamowienie> GetZamowienia()
            {
                _getZamowieniaCalls.Add(new object[] { });
                return _zamowienia;
            }

            public int GetCallCount() => _getZamowieniaCalls.Count;

            public void DeleteZamowienie(int zamowienieId) => throw new System.NotImplementedException();
            public void Dispose() => throw new System.NotImplementedException();
            public Zamowienie GetZamowienieByID(int zamowienieId) => throw new System.NotImplementedException();
            public void InsertZamowienie(Zamowienie zamowienie) => throw new System.NotImplementedException();
            public void Save() => throw new System.NotImplementedException();
            public void UpdateZamowienie(Zamowienie zamowienie) => throw new System.NotImplementedException();
        }

        [Fact]
        public void SpyTest_ObslugaZamowien_PobierzZamowieniaPosortowane()
        {
            var zamowienia = new List<Zamowienie>
            {
                new Zamowienie { id = 1, data_zamowienia = new System.DateTime(2025, 5, 6), laczna_cena = 120.50 },
                new Zamowienie { id = 2, data_zamowienia = new System.DateTime(2025, 5, 7), laczna_cena = 89.99 },
                new Zamowienie { id = 3, data_zamowienia = new System.DateTime(2025, 5, 5), laczna_cena = 45.25 }
            };

            var spyZamowienieRepo = new SpyZamowienieRepository(zamowienia);

            var obslugaZamowien = new ObslugaZamowien(spyZamowienieRepo);

            var result = obslugaZamowien.PobierzZamowieniaPosortowane().ToList();

            Assert.Equal(1, spyZamowienieRepo.GetCallCount());

            Assert.Equal(3, result.Count);
            Assert.Equal(2, result[0].id);
            Assert.Equal(1, result[1].id);
            Assert.Equal(3, result[2].id);
        }
    }
}