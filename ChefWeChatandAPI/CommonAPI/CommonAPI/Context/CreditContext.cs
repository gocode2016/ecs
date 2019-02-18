using CommonAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CommonAPI
{
    public class CreditContext : DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<Address> Address { get; set; }
        public DbSet<PrizeInfo> PrizeInfo { get; set; }
        public DbSet<PrizeInfoWinner> PrizeInfoWinner { get; set; }
        public DbSet<PrizeOpenIdHomeTasteDraw> PrizeOpenIdHomeTasteDraw { get; set; }
        public DbSet<ECWXTranspondDetails> ECWXTranspondDetails { get; set; }
        public DbSet<HomeTastePacket> HomeTastePacket { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<LuckDrawLog> LuckDrawLog { get; set; }
        public DbSet<DrawPrize> DrawPrize { get; set; }
        public DbSet<MemberPrize> MemberPrize { get; set; }

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