using System.Threading.Tasks;

namespace TokyoGarden.IDAL
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
