using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutofacBugRepro.Models;
using AzureFunctions.Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AutofacBugRepro
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public class InventoryFunctions : FunctionsApi
    {
        [FunctionName("GetProductCategories")]
        // /inventory/categories
        public static async Task<HttpResponseMessage> GetProductCategories(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "inventory/categories")]
            HttpRequestMessage req,
            TraceWriter log,
            [Inject] InventoryController controller)
        {
            log.Info($"C# HTTP trigger function ({nameof(GetProductCategories)} received a request.");
            var result = await controller.GetProductCategories();
            return JsonResponse(result, HttpStatusCode.OK);
        }

        [FunctionName("GetProductsByCategory")]
        // /inventory/categories/{id}/products
        public static async Task<HttpResponseMessage> GetProductsByCategory(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "inventory/categories/{id}/products")]
            HttpRequestMessage req,
            TraceWriter log,
            int id,
            [Inject] InventoryController controller)
        {
            log.Info($"C# HTTP trigger function ({nameof(GetProductsByCategory)} received a request.");
            var result = await controller.GetProductsByCategory(id);
            return JsonResponse(result, HttpStatusCode.OK);
        }

        [FunctionName("GetInventoryBySku")]
        // /inventory/skus?sku=ASDF&sku=ASDG&sku=ASDH
        public static async Task<HttpResponseMessage> GetInventoryBySku(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "inventory")]
            HttpRequestMessage req,
            TraceWriter log,
            [Inject] InventoryController controller)
        {
            log.Info($"C# HTTP trigger function ({nameof(GetInventoryBySku)} received a request.");
            var skus = req.GetQueryNameValuePairs();
            var skuList = skus.Where(k => k.Key == "sku").Select(v => v.Value).ToList();
            if (skuList.Any())
            {
                var result = await controller.QueryInventoryBySkuList(skuList);
                return JsonResponse(result, HttpStatusCode.OK);
            }
            else
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Invalid querystring params, expects '?sku=XXXX&sku=XXXY'");
            }
        }

        [FunctionName("UpdateProductsQuantity")]
        // /inventory
        // Post
        public static async Task<HttpResponseMessage> UpdateProductsQuantity(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "inventory")]
            HttpRequestMessage req,
            TraceWriter log,
            [Inject] InventoryController controller)
        {
            log.Info($"C# HTTP trigger function ({nameof(UpdateProductsQuantity)} received a request.");
            var inventoryProducts = await req.Content.ReadAsAsync<List<InvProductOperation>>();
            try
            {
                var result = await controller.UpdateAvailableProductsQuantity(inventoryProducts);
                return JsonResponse(result, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                log.Error($"Error in {nameof(controller.UpdateAvailableProductsQuantity)}", e);
                throw;
            }
        }

        [FunctionName("SubractProductsQuantity")]
        // /inventory
        // Post
        public static async Task<HttpResponseMessage> SubtractProductsQuantity(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "inventory/subtract")]
            HttpRequestMessage req,
            TraceWriter log,
            [Inject] InventoryController controller)
        {
            log.Info($"C# HTTP trigger function ({nameof(SubtractProductsQuantity)} received a request.");
            var inventoryProducts = await req.Content.ReadAsAsync<List<InvProductOperation>>();

            var result = await controller.SubtractItems(inventoryProducts);
            return JsonResponse(result, HttpStatusCode.OK);
        }
    }
}