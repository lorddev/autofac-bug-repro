using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutofacBugRepro.Models;

namespace AutofacBugRepro.Services
{
    public interface IProductCategoryServices
    {
        Task<IEnumerable<InventoryProductCategory>> GetInventoryProductCategories();
        Task<IEnumerable<InventoryProductSummary>> GetInventoryProducts(int categoryId);
    }
}