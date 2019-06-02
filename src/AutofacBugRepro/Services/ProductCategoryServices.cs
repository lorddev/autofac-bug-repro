using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutofacBugRepro.Models;

namespace AutofacBugRepro.Services
{
    public class ProductCategoryServices : IProductCategoryServices
    {
        private readonly ICacheManager _cacheManager;
        
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<ReadyMadeProduct> _repository;

        public ProductCategoryServices(IRepository<ReadyMadeProduct> productRepository,
            ICacheManager cacheManager,
            IRepository<ProductCategory> productCategoryRepository
        )
        {
            _repository = productRepository;
            _cacheManager = cacheManager;
            _productCategoryRepository = productCategoryRepository;
        }
     
        public async Task<IEnumerable<InventoryProductCategory>> GetInventoryProductCategories()
        {
            const string inventoryCategoriesCacheKey = "inv-cat-cache-key";

            if (_cacheManager.IsSet(inventoryCategoriesCacheKey))
            {
                var cachedValue = _cacheManager.Get<IEnumerable<InventoryProductCategory>>(inventoryCategoriesCacheKey);
                if (cachedValue != null) return cachedValue;
            }

            try
            {
                var productCategories = await _productCategoryRepository
                    .TableNoTracking
                    .Include(p => p.Parent)
                    .Include(p => p.Children)
                    .ToListAsync();

                var parents = productCategories.Where(x => x.ParentId == null).ToList();

                var hierarchicalCategories =
                    AddToHierarchyItems(parents, new Collection<InventoryProductCategory>(), 1);
                _cacheManager.Set(inventoryCategoriesCacheKey, hierarchicalCategories, 30);

                return hierarchicalCategories;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IEnumerable<InventoryProductSummary>> GetInventoryProducts(int categoryId)
        {
            var productCategories = GetAllCategories();
            var productCategory = productCategories.FirstOrDefault(cp => cp.Id == categoryId);

            if (productCategory == null) return null;

            var categories = new List<int> {productCategory.Id};
            if (productCategory.HasChildren) categories.AddRange(productCategory.Children.Select(c => c.Id));

            return await _repository.TableNoTracking
                .Where(p => categories.Contains(p.ProductCategoryId))
                .OrderBy(p => p.Sku)
                .Select(p => new InventoryProductSummary
                {
                    Htm = p.Url,
                    ProductName = p.ProductName,
                    Sku = p.Sku,
                    Price = p.UnitPrice,
                    UnitInStock = p.UnitInStock
                }).ToListAsync();
        }

        private IQueryable<ProductCategory> GetAllCategories()
        {
            const string key = "productCategory.all-productCategories";

            return _cacheManager.Get(key, 300, () =>
            {
                var allEntities = _productCategoryRepository
                    .TableNoTracking
                    .Include(p => p.Parent)
                    .Include(p => p.Children)
                    .ToList();

                return allEntities.AsQueryable();
            });
        }

        private static IEnumerable<InventoryProductCategory> AddToHierarchyItems(
            IEnumerable<ProductCategory> categories,
            ICollection<InventoryProductCategory> items, int padding)
        {
            foreach (var category in categories.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name))
            {
                items.Add(category.ParentId != null
                    ? new InventoryProductCategory
                        {Id = category.Id, Name = $"{new string('─', padding)} {category.Name}"}
                    : new InventoryProductCategory {Id = category.Id, Name = category.Name});

                if (category.HasChildren) AddToHierarchyItems(category.Children.ToList(), items, padding * 2);
            }

            return items;
        }
    }
}