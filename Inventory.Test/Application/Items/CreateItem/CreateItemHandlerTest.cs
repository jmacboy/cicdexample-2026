using Inventory.Application.Items.CreateItem;
using Inventory.Domain.Items;
using Joseco.DDD.Core.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Test.Application.Items.CreateItem
{
    public class CreateItemHandlerTest
    {
        [Fact]
        [Trait("Category", "Unit")]

        public async void CreateItemHandler_Handle_IsValid()
        {
            // Arrange
            var itemRepositoryMock = new Mock<IItemRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var itemHandler = new CreateItemHandler(itemRepositoryMock.Object, unitOfWorkMock.Object);
            var itemId = Guid.NewGuid();
            var itemName = "Test Item";
            var createItemCommand = new CreateItemCommand(itemId, itemName);

            // Act
            var result = await itemHandler.Handle(createItemCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(itemId, result.Value);
            itemRepositoryMock.Verify(x => x.AddAsync(It.Is<Item>(i => i.Id == itemId && i.Name == itemName)), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
