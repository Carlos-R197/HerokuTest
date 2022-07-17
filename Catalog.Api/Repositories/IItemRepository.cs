using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Guid id);
    }

}