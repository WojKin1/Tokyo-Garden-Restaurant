using BL;
using DAL;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AdresyRepository repo = new AdresyRepository(new DbTokyoGarden());
            ObslugaAdresow oa = new ObslugaAdresow(repo);

            foreach(var adresy in oa.PobierzPosortowaneAdresy())
            {
                Console.WriteLine(adresy.miasto);
            }

            Console.WriteLine($"Liczba adresów: {oa.PoliczAdresy()}");
        }
    }
}
