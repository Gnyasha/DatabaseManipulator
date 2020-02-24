namespace DatabaseManipulator
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ManipulatorDbContext : DbContext
    {
        public ManipulatorDbContext()
            : base("name=ManipulatorDbContext")
        {
        }

        public virtual DbSet<tblReceivedMessage> tblReceivedMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
