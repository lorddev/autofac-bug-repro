using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutofacBugRepro.Models;

namespace AutofacBugRepro.Services
{
    public class InventoryServices : IInventoryServices
    {
        private readonly IDbContext _context;

        public InventoryServices(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryProductSummary>> QueryInventoryBySkuList(List<string> skuList)
        {
            if (skuList == null || !skuList.Any())
            {
                return Enumerable.Empty<InventoryProductSummary>();
            }

            return await (from p in _context.Set<ReadyMadeProduct>()
                where skuList.Contains(p.Sku)
                orderby p.Sku
                select new InventoryProductSummary
                {
                    Htm = p.Url,
                    ProductName = p.ProductName,
                    Sku = p.Sku,
                    Price = p.UnitPrice,
                    UnitInStock = p.UnitInStock
                }).ToListAsync();
        }

        public async Task<IEnumerable<InventoryProductSummary>> UpdateAvailableProductsQuantity(
            List<InvProductOperation> inventoryProducts)
        {
            if (inventoryProducts == null || !inventoryProducts.Any())
            {
                return Enumerable.Empty<InventoryProductSummary>();
            }

            var skuList = new List<string>();
            foreach (var item in inventoryProducts)
            {
                var sku = await SetInventory(item.Sku, i => item.Qty);
                skuList.Add(sku);
            }

            return await QueryInventoryBySkuList(skuList);
        }

        public async Task<IEnumerable<InventoryProductSummary>> SubtractInventory(List<InvProductOperation> inventoryProducts)
        {
            if (inventoryProducts == null || !inventoryProducts.Any())
            {
                return Enumerable.Empty<InventoryProductSummary>();
            }

            var skuList = new List<string>();
            foreach (var item in inventoryProducts)
            {
                var sku = await SetInventory(item.Sku, i => i - item.Qty);
                skuList.Add(sku);
            }

            return await QueryInventoryBySkuList(skuList);
        }

        public async Task<string> SetInventory(string sku, Func<int, int> qtyFunction)
        {
            var product = _context.ReadyMadeProducts.FirstOrDefault(p => p.Sku == sku);
            if (product == null)
            {
                //_log.Info($"Unable to find product with sku {sku}");
                return sku;
            }

            product.UnitInStock = qtyFunction.Invoke(product.UnitInStock);
            if (product.UnitInStock < 0)
            {
                product.UnitInStock = 0;
            }

            await _context.SaveChangesAsync();
            
            return sku;
        }
    }
}