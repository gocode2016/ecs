using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Models
{
    /// <summary>
    /// 会员信息附属表
    /// </summary>
    public class MemberProfile
    {
        [Key]
        public int MemberId { get; set; }
        /// <summary>
        /// 从事行业原因
        /// </summary>
        public string JobReason { get; set; }
        /// <summary>
        /// 食材选择要求
        /// </summary>
        public string FoodDemand { get; set; }
        /// <summary>
        /// 擅长菜系
        /// </summary>
        public string ExpertInCuisine { get; set; }
        /// <summary>
        /// 是否乐于分享
        /// </summary>
        public int? IsShare { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 会员家庭省ID
        /// </summary>
        public int? HomeProvinceId { get; set; }
        /// <summary>
        /// 会员家庭省名称
        /// </summary>
        public string HomeProvinceName { get; set; }
        /// <summary>
        /// 会员家庭市ID
        /// </summary>
        public int? HomeCityId { get; set; }
        /// <summary>
        /// 会员家庭市名称
        /// </summary>
        public string HomeCityName { get; set; }
        /// <summary>
        /// 会员家庭区ID
        /// </summary>
        public int? HomeAreaId { get; set; }
        /// <summary>
        /// 会员家庭区名称
        /// </summary>
        public string HomeAreaName { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string HomeAddress { get; set; }
        /// <summary>
        /// 入行时间
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// 毕业院校
        /// </summary>
        public string School { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 性格
        /// </summary>
        public string Nature { get; set; }
        /// <summary>
        /// 在职
        /// </summary>
        public int? IsJob { get; set; }
        /// <summary>
        /// 业余爱好
        /// </summary>
        public string Hobbys { get; set; }
        /// <summary>
        /// 相关工作特长
        /// </summary>
        public string Speciality { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Educations { get; set; }
        /// <summary>
        /// 决策权限
        /// </summary>
        public string ChoicePower { get; set; }
        /// <summary>
        /// 是否自由支配时间
        /// </summary>
        public string IsCtrlTime { get; set; }
        /// <summary>
        /// 师承
        /// </summary>
        public string Teacher { get; set; }
        /// <summary>
        /// 个人职业发展
        /// </summary>
        public string Development { get; set; }
        /// <summary>
        /// 是否多城市从业经验
        /// </summary>
        public int? IsManyExp { get; set; }
        /// <summary>
        /// 希望考察城市
        /// </summary>
        public string WishCity { get; set; }
        /// <summary>
        /// 个人代表菜
        /// </summary>
        public string Represent { get; set; }
        /// <summary>
        /// 菜品成功原因
        /// </summary>
        public string SuccessReasons { get; set; }
        /// <summary>
        /// 调味品采购权
        /// </summary>
        public string Purchaser { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public int? IsMarry { get; set; }
        /// <summary>
        /// 普通话等级
        /// </summary>
        public string ChineseLv { get; set; }
        /// <summary>
        /// 烹饪比赛名称
        /// </summary>
        public string MatchName { get; set; }
        /// <summary>
        /// 赛事级别
        /// </summary>
        public string MatchLv { get; set; }
        /// <summary>
        /// 赛事级别
        /// </summary>
        public string MatchNum { get; set; }
        /// <summary>
        /// 荣誉职称
        /// </summary>
        public string Honor { get; set; }
        /// <summary>
        /// 职业资格
        /// </summary>
        public string Qualifications { get; set; }
        /// <summary>
        /// 营养师等级
        /// </summary>
        public string DietitianLv { get; set; }
        /// <summary>
        /// 电视媒体曝光
        /// </summary>
        public string ExposureCount { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 主营类型
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// 人均消费
        /// </summary>
        public string PerConsumption { get; set; }
        /// <summary>
        /// 现在使用欣和食品
        /// </summary>
        public int? IsUseShinho { get; set; } 
        /// <summary>
        /// 批发市场
        /// </summary>
        public string WholesaleName { get; set; }
        /// <summary>
        /// 门店经营范围
        /// </summary>
        public string ShopOperateSize { get; set; }
        /// <summary>
        /// 是否有小孩
        /// </summary>
        public int? IsAnyChild { get; set; }
        /// <summary>
        /// 家庭收入
        /// </summary>
        public string HomeIncome { get; set; }
        /// <summary>
        /// 烹饪频率
        /// </summary>
        public string CookRate { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string CardId { get; set; }
        public DateTime? ProfileTime { get; set; }

    }
}