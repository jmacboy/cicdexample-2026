using Inventory.Application.Items.CreateItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.IntTest.Factories
{
    public class ItemFactory
    {
        public static CreateItemCommand createItemCommand(Guid? id = null, string? itemName = null)
        {
            return new CreateItemCommand(
                id ?? Guid.NewGuid(),
                itemName ?? "Test Item"
            );
        }
    }
}