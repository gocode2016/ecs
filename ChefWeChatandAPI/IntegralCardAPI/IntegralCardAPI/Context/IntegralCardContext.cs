using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using IntegralCardAPI.Models;
using System.Linq;
using System.Web;

namespace IntegralCardAPI
{
    public class IntegralCardContext:DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<IntegralGoods> IntegralGoods { get; set; }
        public DbSet<GoodsIntegral> GoodsIntegral { get; set; }
        public DbSet<IntegralRule> IntegralRule { get; set; }
        public DbSet<IntegralGoodsQrc> IntegralGoodsQrc { get; set; }
        public DbSet<IntegralGoodsQrcPackage> IntegralGoodsQrcPackage { get; set; }
        public DbSet<RegistMember> RegistMember { get; set; }
        public DbSet<MemberIntegralDetail> MemberIntegralDetail { get; set; }
        public DbSet<OpenIdAssociated> OpenIdAssociated { get; set; }
        public DbSet<IntegralRuleChance> IntegralRuleChance { get; set; }

         public IntegralCardContext()
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