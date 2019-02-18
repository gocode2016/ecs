using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using WxScanAPI.Models;
using System.Linq;
using System.Web;

namespace WxScanAPI
{
    public class WxScanContext:DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";
        public DbSet<RegistMember> RegistMember { get; set; }
        public DbSet<MemberIntegralDetail> MemberIntegralDetail { get; set; }
        public DbSet<OpenIdAssociated> OpenIdAssociated { get; set; }
        public DbSet<RedPackScanRecord> RedPackScanRecord { get; set; }

        public WxScanContext()
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