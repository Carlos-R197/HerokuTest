using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Catalog.Api.Repositories;
using Catalog.Api.Entities;
using Catalog.Api.Dtos;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository repository;

        public ItemsController(IItemRepository repo)
        {
            repository = repo;
        }

        // Get: /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        // Get: /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetAsync(id);
            if (item is null)
                return NotFound();

            return Ok(item.AsDto());
        }

        // Post: /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto dto)
        {
            var item = new Item {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateAsync(item);
            
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        // Put: /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto dto)
        {
            var existingItem = await repository.GetAsync(id);
            if (existingItem is null)
                return NotFound();
            
            var updatedItem = existingItem with {
                Name = dto.Name,
                Price = dto.Price
            };
            await repository.UpdateAsync(updatedItem);

            return NoContent();
        }

        // Delete: /items/{id}
        [HttpDelete]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetAsync(id);
            if (existingItem is null)
                return NotFound();
            
            await repository.DeleteAsync(id);
            return NoContent();
        }
    }
}