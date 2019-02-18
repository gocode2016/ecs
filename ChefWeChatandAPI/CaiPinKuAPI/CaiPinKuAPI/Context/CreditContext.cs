using CaiPinKuAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CaiPinKuAPI
{
    public class CreditContext : DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<CpkFirstCategory> CpkFirstCategory { get; set; }
        public DbSet<CpkSecondCategory> CpkSecondCategory { get; set; }
        public DbSet<CpkCaiPinBasicInfo> CpkCaiPinBasicInfo { get; set; }
        public DbSet<CpkCaiPinMaterial> CpkCaiPinMaterial { get; set; }
        public DbSet<CpkZhuanTi> CpkZhuanTi { get; set; }
        public DbSet<CpkZhuanTiContent> CpkZhuanTiContent { get; set; }
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