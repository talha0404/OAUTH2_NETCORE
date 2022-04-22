using CRUDAPI.DOMAIN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAPI.EFCORE
{
    public class CrudApiDbContext : DbContext
    {
        public CrudApiDbContext(DbContextOptions<CrudApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////modelBuilder.ApplyConfiguration(new CustomerMap());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Customer> Customers { get; set; }

    }
}
