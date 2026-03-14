using Inventory.Domain.Items;
using Joseco.DDD.Core.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Test.Domain.Items
{
    public class ItemTest
    {
        [Fact]
        [Trait("Category", "Unit")]

        public void ItemCreation_IsValid()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var itemName = "Test Item";
            // Act
            var item = new Item(guid, itemName);
            // Assert

            Assert.Equal(guid, item.Id);
            Assert.Equal(itemName, item.Name);
            Assert.Equal(0, item.Stock.Value);
            Assert.Equal(0, item.Reserved.Value);
            Assert.Equal(0, item.Available.Value);
            Assert.Equal(0, item.UnitaryCost.Value);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData(" ")]
        [Trait("Category", "Unit")]

        public void ItemCreation_ThrowsException(string itemName)
        {
            // Arrange
            var guid = Guid.NewGuid();
            // Assert
            Assert.Throws<DomainException>(() => new Item(guid, itemName));
        }

        [Fact]
        [Trait("Category", "Unit")]

        public void Item_AddStock_QuantityZero()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var itemName = "Test Item";
            var item = new Item(guid, itemName);
            var quantityToAdd = 0;
            var unitaryCost = 10m;
            var costStrategy = new AverageCostStrategy();
            var expectedError = ItemErrors.NonNegativeStock();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                item.AddStock(quantityToAdd, unitaryCost, costStrategy)
            );
            try
            {
                item.AddStock(quantityToAdd, unitaryCost, costStrategy);
            }
            catch (DomainException de)
            {
                Assert.Equal(expectedError.Code, de.Error.Code);
                Assert.Equal(expectedError.StructuredMessage, de.Error.StructuredMessage);
                Assert.Equal(expectedError.Type, de.Error.Type);
            }
        }

        [Fact]
        [Trait("Category", "Unit")]

        public void Item_AddStock_CostStrategyNull()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var itemName = "Test Item";
            var item = new Item(guid, itemName);
            var quantityToAdd = 10;
            var unitaryCost = 10m;
            var expectedError = ItemErrors.CostStrategyNotProvided();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                item.AddStock(quantityToAdd, unitaryCost, null)
            );
            try
            {
                item.AddStock(quantityToAdd, unitaryCost, null);
            }
            catch (DomainException de)
            {
                Assert.Equal(expectedError.Code, de.Error.Code);
                Assert.Equal(expectedError.StructuredMessage, de.Error.StructuredMessage);
                Assert.Equal(expectedError.Type, de.Error.Type);
            }
        }
        [Fact]
        [Trait("Category", "Unit")]

        public void Item_AddStock_IsValid()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var itemName = "Test Item";
            var item = new Item(guid, itemName);
            var quantityToAdd = 10;
            var unitaryCost = 10m;
            var costStrategy = new AverageCostStrategy();
            // Act
            item.AddStock(quantityToAdd, unitaryCost, costStrategy);
            // Assert
            Assert.Equal(10, item.Stock.Value);
            Assert.Equal(0, item.Reserved.Value);
            Assert.Equal(10, item.Available.Value);
            Assert.Equal(10m, item.UnitaryCost.Value);
        }
        [Fact]
        [Trait("Category", "Unit")]

        public void Item_AddStock_MultipleTimes()
        {
            var guid = Guid.NewGuid();
            var itemName = "Test Item";
            var item = new Item(guid, itemName);
            var costStrategy = new AverageCostStrategy();
            var quantitiesToAdd = new List<int> { 10, 20, 30 };
            var unitaryCosts = new List<decimal> { 10m, 20m, 30m };


            for (int i = 0; i < quantitiesToAdd.Count; i++)
            {
                item.AddStock(quantitiesToAdd[i], unitaryCosts[i], costStrategy);
            }

            Assert.Equal(60, item.Stock.Value);
            Assert.Equal(0, item.Reserved.Value);
            Assert.Equal(60, item.Available.Value);
            Assert.Equal(23.34m, item.UnitaryCost.Value);

        }
        [Fact]
        [Trait("Category", "Unit")]

        public void Item_AddStock_OtherCostStrategy()
        {
            var guid = Guid.NewGuid();
            var itemName = "Test Item";
            var item = new Item(guid, itemName);
            var costStrategy = new Mock<ICostStrategy>();
            var currentStock = 0;
            var currentUnitaryCost = 0m;

            var quantityToAdd = 10;
            var unitaryCost = 10m;

            var expectedCost = 15m;
            costStrategy.Setup(cs =>
                cs.CalculateNewCost(currentStock, currentUnitaryCost, quantityToAdd, unitaryCost))
            .Returns(expectedCost);

            item.AddStock(quantityToAdd, unitaryCost, costStrategy.Object);

            Assert.Equal(quantityToAdd, item.Stock.Value);
            Assert.Equal(0, item.Reserved.Value);
            Assert.Equal(quantityToAdd, item.Available.Value);
            Assert.Equal(expectedCost, item.UnitaryCost.Value);

        }
    }
}
