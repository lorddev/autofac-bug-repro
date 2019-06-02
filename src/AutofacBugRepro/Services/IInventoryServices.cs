using System.Collections.Generic;
using System.Threading.Tasks;
using AutofacBugRepro.Models;

namespace AutofacBugRepro.Services
{
    public interface IInventoryServices
    {
        Task<IEnumerable<InventoryProductSummary>> QueryInventoryBySkuList(List<string> skuList);
        Task<IEnumerable<InventoryProductSummary>> UpdateAvailableProductsQuantity(List<InvProductOperation> inventoryProducts);
        Task<IEnumerable<InventoryProductSummary>> SubtractInventory(List<InvProductOperation> inventoryProducts);
    }
}