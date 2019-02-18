using CreditUserAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CreditUserAPI
{
    public class CreditContext : DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<RegistMember> Member { get; set; }
        public DbSet<RegistCode> RegistCode { get; set; }
        public DbSet<MemberProfile> MemberProfile { get; set; }
        public DbSet<OpenIdAssociated> OpenIdAssociated { get; set; }
        public DbSet<RegistSaleMan> RegistSaleMan { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<MemberResume> MemberResume { get; set; }
        public DbSet<MemberIntegralDetail> MemberIntegralDetail { get; set; }
        public DbSet<IntegralRule> IntegralRule { get; set; }
        public DbSet<IntegralQrc> IntegralQrc { get; set; }
        public DbSet<MemberRealAuth> MemberRealAuth { get; set; }
        public DbSet<SalemanArea> SalemanArea { get; set; }

        public CreditContext()
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