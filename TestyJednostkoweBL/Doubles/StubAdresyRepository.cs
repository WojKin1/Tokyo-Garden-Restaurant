using IDAL;
using Model;
using System.Collections.Generic;

namespace BL.Tests.Doubles
{
    public class StubAdresyRepository : IAdresyRepository
    {
        public IEnumerable<Adresy> GetAdresy() => new List<Adresy>
        {
            new Adresy { miasto = "Warszawa" },
            new Adresy { miasto = "Kraków" }
        };
        public Adresy GetAdresyByID(int adresId) => null;
        public void InsertAdres(Adresy adres) { }
        public void DeleteAdres(int adresId) { }
        public void UpdateAdres(Adresy adres) { }
        public void Save() { }
        public void Dispose() { }
    }
}
