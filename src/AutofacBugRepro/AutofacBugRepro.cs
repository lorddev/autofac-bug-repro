using System.Data;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using AutofacBugRepro.Models;

namespace AutofacBugRepro
{
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class AutofacBugRepro : BaseDbContext, IDbContext
    {
        static AutofacBugRepro()
        {
            Database.SetInitializer<AutofacBugRepro>(null);
        }

        public AutofacBugRepro()
            : base("AutofacBugRepro")
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
}
