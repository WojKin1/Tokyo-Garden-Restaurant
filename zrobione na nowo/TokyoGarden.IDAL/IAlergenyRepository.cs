using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IDAL
{
    public interface IAlergenyRepository : IRepository<Alergeny>
    {
        Task<Alergeny?> GetByNameAsync(string allergenName);
    }
}
