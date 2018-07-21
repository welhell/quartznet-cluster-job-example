using System.Threading.Tasks;
using QuartzCluster.Entities;

namespace QuartzCluster.Repositories
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
    }
}