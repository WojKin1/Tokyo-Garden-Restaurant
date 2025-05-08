using IDAL;
using Model;
using System.Collections.Generic;

namespace BL.Tests.Doubles
{
    public class MockAdresyRepository : IAdresyRepository
    {
        public int GetAdresyCallCount { get; private set; } = 0;

        public IEnumerable<Adresy> GetAdresy()
        {
            GetAdresyCallCount++;
            return new List<Adresy>();
        }

        public Adresy GetAdresyByID(int adresId) => null;
        public void InsertAdres(Adresy adres) { }
        public void DeleteAdres(int adresId) { }
        public void UpdateAdres(Adresy adres) { }
        public void Save() { }
        public void Dispose() { }
    }
}
