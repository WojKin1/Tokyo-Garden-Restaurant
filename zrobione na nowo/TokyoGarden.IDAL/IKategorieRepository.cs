using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IDAL
{
    public interface IKategorieRepository : IRepository<Kategorie>
    {
        Task<Kategorie?> GetByNameAsync(string categoryName);
    }
}
