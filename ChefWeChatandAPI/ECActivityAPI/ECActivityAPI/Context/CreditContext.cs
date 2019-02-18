using ECActivityAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ECActivityAPI
{
    public class CreditContext : DbContext
    {
        private readonly static string CONNECTION_STRING = "CreditSys";

        public DbSet<ChefActivity> ChefActivity { get; set; }
        public DbSet<MatchRegion> MatchRegion { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<TutorComment> TutorComment { get; set; }
        public DbSet<DishTutor> DishTutor { get; set; }
        public DbSet<DishMaterial> DishMaterial { get; set; }
        public DbSet<DishStep> DishStep { get; set; }
        public DbSet<FlavourRec> FlavourRec { get; set; }
        public DbSet<FlavourRecRole> FlavourRecRole { get; set; }
        public DbSet<DishVisitLog> DishVisitLog { get; set; }
        public DbSet<DishComment> DishComment { get; set; }
        public DbSet<DishPrasieLog> DishPrasieLog { get; set; }
        public DbSet<Chef> Chef { get; set; }
        public DbSet<DishChef> DishChef { get; set; }
        public DbSet<DishCollectLog> DishCollectLog { get; set; }
        public DbSet<RegistMember> RegistMember { get; set; }
        public DbSet<OpenIdAssociated> OpenIdAssociated { get; set; }
        public DbSet<DishSelectedLog> DishSelectedLog { get; set; }
        public DbSet<ChefStar> ChefStar { get; set; }
        public DbSet<DishChefStar> DishChefStar { get; set; }
        public DbSet<ChefStarComment> ChefStarComment { get; set; }
        public DbSet<CultivateInterflow> CultivateInterflow { get; set; }
        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<CultivatePraise> CultivatePraise { get; set; }
        public DbSet<CultivateVisit> CultivateVisit { get; set; }
        public DbSet<CultivateComment> CultivateComment { get; set; }
        public DbSet<SigninCreditLog> SigninCreditLog { get; set; }
        public DbSet<SigninCredit> SigninCredit { get; set; }
        public DbSet<ECBrowse> ECBrowse { get; set; }
        public DbSet<ECBrowseDetails> ECBrowseDetails { get; set; }
        public DbSet<ECWXTranspondDetails> ECWXTranspondDetails { get; set; }
        public DbSet<URLName> URLName { get; set; }
        public DbSet<MySelfDishKu> MySelfDishKu { get; set; }
        public DbSet<MySelfFR> MySelfFR { get; set; }
        public DbSet<MySelfFRRole> MySelfFRRole { get; set; }
        public DbSet<ProductFirst> ProductFirst { get; set; }
        public DbSet<ProductSecond> ProductSecond { get; set; }
        /// <summary>
        /// 产品库类
        /// </summary>
        public DbSet<ProductKuInfo> ProductKuInfo { get; set; }
        public DbSet<Specification> Specification { get; set; }
        public DbSet<VirtualClassify> VirtualClassify { get; set; }
        public DbSet<SpecificationConf> SpecificationConf { get; set; }
        public DbSet<SpecificationConfArea> SpecificationConfArea { get; set; }
        public DbSet<SpecificationConfVC> SpecificationConfVC { get; set; }
        public DbSet<ProductIdKuInfoRD> ProductIdKuInfoRD { get; set; }
        public DbSet<ProductIdKuInfoRDMaster> ProductIdKuInfoRDMaster { get; set; }
        public DbSet<DishProduct> DishProduct { get; set; }
        public DbSet<DishProductRF> DishProductRF { get; set; }
        public DbSet<SpecificationVCDate> SpecificationVCDate { get; set; }
        public DbSet<SpecificationVCDatePraiseLog> SpecificationVCDatePraiseLog { get; set; }
        public DbSet<SpecificationVCDateVisitLog> SpecificationVCDateVisitLog { get; set; }
        public DbSet<SpecificationVisitLog> SpecificationVisitLog { get; set; }
        public DbSet<SpecificationPraiseLog> SpecificationPraiseLog { get; set; }
        public DbSet<SpecificationComment> SpecificationComment { get; set; }

        public DbSet<BusinessContribution> BusinessContribution { get; set; }
        public DbSet<RegistCode> RegistCode { get; set; }
        public DbSet<MemberIntegralDetail> MemberIntegralDetail { get; set; }
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