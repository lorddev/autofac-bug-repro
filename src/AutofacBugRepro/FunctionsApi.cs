using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace AutofacBugRepro
{
    public abstract class FunctionsApi
    {
        static FunctionsApi()
        {
            Database.SetInitializer(new DbInitializer());
        }

        // ReSharper disable once EmptyConstructor
        protected FunctionsApi() { }

        protected static HttpResponseMessage JsonResponse(dynamic result, HttpStatusCode statusCode)
        {
            var json = JsonConvert.SerializeObject(result);
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}