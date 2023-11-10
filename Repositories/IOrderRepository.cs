using BusinessObjects.Models;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetAsync(Guid id);
        Task<List<Order>> GetAllByCustomerAsync(Guid cusId);
        Task AddAsync(Order model);
        Task DeliveringAsync(Order model);
        Task CompleteAsync(Order model);
        Task CancelAsync(Order model);
    }
}
