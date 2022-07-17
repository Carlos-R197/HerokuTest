using Catalog.Api.Repositories;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    private readonly InMemItemRepository repo = new();
    private readonly Random rand = new();

    [Fact]
    public async Task GetItemAsync_WithUnexestingItem_ReturnsNotFound()
    {
        // Arrange
        var controller = new ItemsController(repo);
        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateItemAsync_NewItem_ReturnsCreateAt()
    {
        var item = new CreateItemDto {
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1, 1000)
        };
        var controller = new ItemsController(repo);
        var result = await controller.CreateItemAsync(item);
        Assert.IsType<CreatedAtActionResult>(result.Result);   
    }

    [Fact]
    public async Task UpdateItemAsync_ExistingItem_UpdatedProperly()
    {
        var controller = new ItemsController(repo);
        var items = await controller.GetItemsAsync();
        var item = items.First();
        UpdateItemDto updatedProps = new() {
            Name = "Blue Potion",
            Price = 11
        };
        var result = await controller.UpdateItemAsync(item.Id, updatedProps);
        items = await controller.GetItemsAsync();

        Assert.IsType<NoContentResult>(result as NoContentResult);
        Assert.Equal(items.First().Name, updatedProps.Name);
        Assert.Equal(items.First().Price, updatedProps.Price);
    }

    [Fact]
    public async Task DeleteItemAsync_NewItem_DeletedProperly()
    {
        var controller = new ItemsController(repo);
        var items =  await controller.GetItemsAsync();
        var result =  await controller.DeleteItemAsync(items.First().Id);
        Assert.IsType<NoContentResult>(result as NoContentResult);
    }

    private Item CreateRandomItem()
    {
        return new Item {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1, 1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}