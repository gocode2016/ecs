using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using IntegralMallAPI.Models;
using System.Linq;
using System.Web;

namespace IntegralMallAPI
{
    public class IntegralMallContext:DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<Category> Category { get; set; }
        public DbSet<Norms> Norms { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SKU> SKU { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<MemberOrder> Order { get; set; }
        public DbSet<OrderDetaile> OrderDetaile { get; set; }
        //public DbSet<SaleActivity> SaleActivity { get; set; }
        public DbSet<SaleActivity> SaleActivity { get; set; }

        public IntegralMallContext()
            : base(CONNECTION_STRING)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();//防止黑幕交易 要不然每次都要访问 EdmMetadata这个表
        }
    }
}