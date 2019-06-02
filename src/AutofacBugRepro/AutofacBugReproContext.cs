using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using AutofacBugRepro.Models;

namespace AutofacBugRepro
{
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class AutofacBugReproContext : BaseDbContext, IDbContext
    {
        static AutofacBugReproContext()
        {
            Database.SetInitializer<AutofacBugReproContext>(null);
        }

        public AutofacBugReproContext()
            : base("TestContext")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public ConnectionState ConnectionState => Database.Connection.State;

        public virtual DbSet<ReadyMadeProduct> ReadyMadeProducts { get; set; }
       
        public void Open()
        {
            Database.Connection.Open();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public class DbInitializer : DropCreateDatabaseIfModelChanges<AutofacBugReproContext>
    {
        protected override void Seed(AutofacBugReproContext context)
        {
            var products = new List<ReadyMadeProduct>
            {
                new ReadyMadeProduct
                {
                    Brand = "MyBrand", 
                    Msrp = 9.99M,
                    UnitPrice = 8.99M,
                    Picture = "product.png",
                    ProductCategoryId = 1,
                    ProductName = "Test Product 1",
                    Sku = "TP0001",
                    UnitInStock = 100
                },
                new ReadyMadeProduct
                {
                    Brand = "MyBrand",
                    Msrp = 9.99M,
                    UnitPrice = 8.99M,
                    Picture = "product.png",
                    ProductCategoryId = 1,
                    ProductName = "Test Product 2",
                    Sku = "TP0002",
                    UnitInStock = 100
                },
                new ReadyMadeProduct
                {
                    Brand = "MyBrand",
                    Msrp = 9.99M,
                    UnitPrice = 8.99M,
                    Picture = "product.png",
                    ProductCategoryId = 1,
                    ProductName = "Test Product 3",
                    Sku = "TP0003",
                    UnitInStock = 100
                },
                new ReadyMadeProduct
                {
                    Brand = "MyBrand",
                    Msrp = 9.99M,
                    UnitPrice = 8.99M,
                    Picture = "product.png",
                    ProductCategoryId = 1,
                    ProductName = "Test Product 4",
                    Sku = "TP0004",
                    UnitInStock = 100
                }

            };

            products.ForEach(p => context.ReadyMadeProducts.Add(p));

            context.SaveChanges();
        }
    }
}
