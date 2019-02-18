using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using MarketActivityAPI.Models;

namespace MarketActivityAPI
{
    public class MarketActivityContext:DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<SignActivity> SignActivity { get; set; }
        public DbSet<SignInLog> SignInLog { get; set; }

        public MarketActivityContext()
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