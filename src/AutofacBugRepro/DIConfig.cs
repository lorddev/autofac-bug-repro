using Autofac;
using AutofacBugRepro.Services;
using AzureFunctions.Autofac.Configuration;

namespace AutofacBugRepro
{
    public class DIConfig
    {
        public DIConfig()
        {
            DependencyInjection.Initialize(builder =>
            {
                // DAL
                builder.RegisterType<AutofacBugReproContext>().As<IDbContext>().InstancePerLifetimeScope();
                builder.RegisterType<AutofacBugReproContext>().InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

                // Services
                builder.RegisterType<ProductCategoryServices>().As<IProductCategoryServices>().InstancePerLifetimeScope();
                builder.RegisterType<InventoryServices>().As<IInventoryServices>().InstancePerLifetimeScope();
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>();

                // Controllers
                builder.RegisterType<InventoryController>().InstancePerLifetimeScope();
            });
        }
    }
}