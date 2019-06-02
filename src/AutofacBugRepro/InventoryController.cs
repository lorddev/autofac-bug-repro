using System.Collections.Generic;
using System.Threading.Tasks;
using AutofacBugRepro.Models;
using AutofacBugRepro.Services;

namespace AutofacBugRepro
{
    public class InventoryController
    {
        private readonly IProductCategoryServices _productCategoryServices;
        private readonly IInventoryServices _inventoryServices;

        public InventoryController(IProductCategoryServices productCategoryServices, IInventoryServices inventoryServices)
        {
            _productCategoryServices = productCategoryServices;
            _inventoryServices = inventoryServices;
        }

        public async Task<IEnumerable<InventoryProductCategory>> GetProductCategories()
        {
            return await _productCategoryServices.GetInventoryProductCategories();
        }

        public async Task<IEnumerable<InventoryProductSummary>> GetProductsByCategory(int id)
        {
            return await _productCategoryServices.GetInventoryProducts(id);
        }

        public async Task<IEnumerable<InventoryProductSummary>> QueryInventoryBySkuList(List<string> skuList)
        {
            return await _inventoryServices.QueryInventoryBySkuList(skuList);
        }

        // Put
        public async Task<IEnumerable<InventoryProductSummary>> UpdateAvailableProductsQuantity(
            List<InvProductOperation> inventoryProducts)
        {
            return await _inventoryServices.UpdateAvailableProductsQuantity(inventoryProducts);
        }

        // Put
        public async Task<IEnumerable<InventoryProductSummary>> SubtractItems(List<InvProductOperation> inventoryProducts)
        {
            return await _inventoryServices.SubtractInventory(inventoryProducts);
        }
    }
}