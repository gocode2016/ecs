<template>
  <div class="resumepreview">
	<!--头像-->
	<div class="personphoto"><div class="myphoto"><img :src="userImg"/></div></div> 
	<!--基本信息-->
	<div class="title">基本信息</div>
	<div class="resumeDiv">
		<p class="resumePre"><span class="resume_text">姓名:</span><span class="resume_content">{{memberInfo.MemberName}}</span></p>
		<p class="resumePre"><span class="resume_text">性别:</span><span class="resume_content">{{memberInfo.Sex}}</span></p>
		<p class="resumePre"><span class="resume_text">民族:</span><span class="resume_content">{{memberInfo.Nation}}</span></p>
		<p class="resumePre"><span class="resume_text">生日:</span><span class="resume_content">{{memberInfo.Birthday}}</span></p>
		<p class="resumePre"><span class="resume_text">性格:</span><span class="resume_content">{{memberInfo.Nature}}</span></p>
		<p class="resumePre"><span class="resume_text">联系方式:</span><span class="resume_content">{{memberInfo.MemberTelePhone}}</span></p>
		<p class="resumePre"><span class="resume_text">家庭区域:</span><span class="resume_content">{{memberInfo.HomeProvinceName}} {{memberInfo.HomeCityName}} {{memberInfo.HomeAreaName}}</span></p>
		<p class="resumePre"><span class="resume_text">家庭地址:</span><span class="resume_content">{{memberInfo.HomeAddress}}</span></p>
		<p class="resumePre"><span class="resume_text">业余爱好:</span><span class="resume_content">{{memberInfo.Hobbys}}</span></p>
		<p class="resumePre"><span class="resume_text">婚姻状况:</span><span class="resume_content">{{memberInfo.IsMarry}}</span></p>
		<p class="resumePre"><span class="resume_text">普通话水平:</span><span class="resume_content">{{memberInfo.ChineseLv}}</span></p>
		<p class="resumePre"><span class="resume_text">在职情况:</span><span class="resume_content">{{memberInfo.IsJob}}</span></p>
	</div>
	<!--酒店信息-->
	<div class="title titles">酒店信息</div>
	<div class="resumeDiv">
		<p class="resumePre"><span class="resume_text">工作岗位:</span><span class="resume_content">{{memberInfo.PositionType}} {{memberInfo.Position}}</span></p>
		<p class="resumePre"><span class="resume_text">酒店区域:</span><span class="resume_content">{{memberInfo.ProvinceName}} {{memberInfo.CityName}} {{memberInfo.AreaName}}</span></p>
		<p class="resumePre"><span class="resume_text">酒店名称:</span><span class="resume_content">{{memberInfo.HotelName}}</span></p>
		<p class="resumePre"><span class="resume_text">酒店地址:</span><span class="resume_content">{{memberInfo.HotelAddress}}</span></p>
		<p class="resumePre"><span class="resume_text">主营类型:</span><span class="resume_content">{{memberInfo.OperationType}}</span></p>
		<p class="resumePre"><span class="resume_text">酒店人均消费:</span><span class="resume_content">{{memberInfo.PerConsumption}}</span></p>
		<p class="resumePre"><span class="resume_text">使用欣和产品:</span><span class="resume_content">{{memberInfo.IsUseShinho}}</span></p>
		<p class="resumePre"><span class="resume_text">调味品采购权:</span><span class="resume_content">{{memberInfo.Purchaser}}</span></p>
	</div>
	<!--从厨理念-->
	<div class="title titles">从厨理念</div>
	<div class="resumeDiv">
		<p class="resumePre"><span class="resume_text">从事行业原因:</span><span class="resume_content">{{memberInfo.JobReason}}</span></p>
		<p class="resumePre"><span class="resume_text">食材选择要求:</span><span class="resume_content">{{memberInfo.FoodDemand}}</span></p>
		<p class="resumePre"><span class="resume_text">是否乐于分享:</span><span class="resume_content">{{memberInfo.IsShare}}</span></p>
	</div>
	<!--职业技能-->
	<div class="title titles">职业技能</div>
	<div class="resumeDiv">
		<p class="resumePre"><span class="resume_text">学历:</span><span class="resume_content">{{memberInfo.Educations}}</span></p>
		<p class="resumePre"><span class="resume_text">专业:</span><span class="resume_content">{{memberInfo.Major}}</span></p>
		<p class="resumePre"><span class="resume_text">毕业院校:</span><span class="resume_content">{{memberInfo.School}}</span></p>
		<p class="resumePre"><span class="resume_text">师承:</span><span class="resume_content">{{memberInfo.Teacher}}</span></p>
		<p class="resumePre"><span class="resume_text">擅长菜系:</span><span class="resume_content">{{memberInfo.ExpertInCuisine}}</span></p>
		<p class="resumePre"><span class="resume_text">相关工作爱好:</span><span class="resume_content">{{memberInfo.Speciality}}</span></p>
		<p class="resumePre"><span class="resume_text">个人代表菜品:</span><span class="resume_content">{{memberInfo.Represent}}</span></p>
		<p class="resumePre"><span class="resume_text">菜品成功原因:</span><span class="resume_content">{{memberInfo.SuccessReasons}}</span></p>
		<p class="resumePre"><span class="resume_text">未来职业发展:</span><span class="resume_content">{{memberInfo.Development}}</span></p>
		<p class="resumePre"><span class="resume_text">希望考察城市:</span><span class="resume_content">{{memberInfo.WishCity}}</span></p>
		<p class="resumePre"><span class="resume_text">酒店管理决策权:</span><span class="resume_content">{{memberInfo.ChoicePower}}</span></p>
		<p class="resumePre"><span class="resume_text">自由支配时间:</span><span class="resume_content">{{memberInfo.IsCtrlTime}}</span></p>
	</div>
	<!--荣誉资格-->
	<div class="title titles">荣誉资格</div>
	<div class="resumeDiv">
		<p class="resumePre"><span class="resume_text">烹饪赛事:</span><span class="resume_content">{{memberInfo.MatchName}}</span></p>
		<p class="resumePre"><span class="resume_text">职业资格:</span><span class="resume_content">{{memberInfo.Qualifications}}</span></p>
		<p class="resumePre"><span class="resume_text">营养师等级:</span><span class="resume_content">{{memberInfo.DietitianLv}}</span></p>
		<p class="resumePre"><span class="resume_text">电视媒体曝光:</span><span class="resume_content">{{memberInfo.ExposureCount}}</span></p>
		<p class="resumePre"><span class="resume_text">荣誉职称:</span><span class="resume_content">{{memberInfo.Honor}}</span></p>
	</div>
	<!--从业经历-->
	<div class="title titles">从业经历</div>
	<div class="resumeDiv">
		<p class="resumePre"><span class="resume_text">入行时间:</span><span class="resume_content">{{memberInfo.JoinDate}}</span></p>
		<p class="resumePre"><span class="resume_text">厨龄:</span><span class="resume_content">{{kitchenOld}}</span></p>
		<p class="resumePre"><span class="resume_text">多城市从业经验:</span><span class="resume_content">{{memberInfo.IsManyExp}}</span></p>
		<div class="timeBox">
			<div class="timeline_text">
				<p v-for="(item, index) in dataList" :key="index">{{item.BeginTime}}-{{item.EndTime}}</p>
			</div>
			<div class="timeline-demo">
				<timeline>
				  <timeline-item v-for="(item, index) in dataList" :key="index">
					<h4 :class="[item === 0 ? 'recent' : '']">{{item.HotelName}}</h4>
					<p :class="[item === 0 ? 'recent' : '']">{{item.ProvinceName}}{{item.CityName}}{{item.AreaName}}</p>
					<p :class="[item === 0 ? 'recent' : '']">{{item.Job}}</p>
				  </timeline-item>
				</timeline>
			</div>
		</div>
	</div>
	<!--自我介绍-->
	<div class="title titles">自我介绍</div>
	<div class="resumeDiv">
		<div class="textarea">{{memberInfo.Introduction}}</div>
		<p class="prompt_title">温馨提示:</p>
		<p class="prompt_text">请妥善保管好您的个人信息，切勿随意泄露给他人，求职面试时可直接微信转发给面试官。</p>
	</div>
	
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
import { Timeline, TimelineItem,cookie } from 'vux'
export default{
  components:{
  	Timeline, 
  	TimelineItem,
  	cookie
  },
  data(){
  	return{
  		userImg:'../../static/credit/perimg.png',
      userData:'',
      userId:'',
      memberInfo:{},//简历信息
      kitchenOld:'',//厨龄
      dataList:[],//从业经历
      timestamp:'',
      nonceStr:'',
      signature:'',
      url:'',
      openId:''
  	}
  },
  methods:{
		getMemberProfile(){//获取简历信息
			var _this=this;
			var params={
			  'MemberId':this.userId
			}
			this.$http({
				method:'POST',
				url:apiUrl.getMemberProfile,
				data:params
			}).then(function(response){	
				var data=JSON.parse(response.data);
//				console.log(data);
				if(data.length!=0){
					_this.memberInfo=data[0];
					if(_this.memberInfo.ImgUrl!=''&&_this.memberInfo.ImgUrl!=null){
					  _this.userImg=_this.memberInfo.ImgUrl;
				  }
					
					if(_this.memberInfo.Sex==0){//性别
						_this.memberInfo.Sex='女';
					}else if(_this.memberInfo.Sex==1){
						_this.memberInfo.Sex='男';
					}else{
						_this.memberInfo.Sex='';
					}
					
					if(_this.memberInfo.Birthday==null||_this.memberInfo.Birthday==''){//生日
						_this.memberInfo.Birthday='';
					}else{
						_this.memberInfo.Birthday=_this.memberInfo.Birthday.substring(0,10);
					}
					
					if(_this.memberInfo.IsMarry==0){//婚姻状况
						 _this.memberInfo.IsMarry='未婚';
					}else if(_this.memberInfo.IsMarry==1){
						 _this.memberInfo.IsMarry='已婚';
					}else{
						 _this.memberInfo.IsMarry='';
					}
					
					if(_this.memberInfo.IsJob==0){//在职情况
						_this.memberInfo.IsJob='待业';
					}else if(_this.memberInfo.IsJob==1){
						_this.memberInfo.IsJob='在职';
					}else{
						_this.memberInfo.IsJob='';
					}
					
					if(_this.memberInfo.IsUseShinho==0){//是否使用欣和
						_this.memberInfo.IsUseShinho='否';
					}else if(_this.memberInfo.IsUseShinho==1){
						_this.memberInfo.IsUseShinho='是';
					}else{
						_this.memberInfo.IsUseShinho='';
					}
					
					if(_this.memberInfo.IsShare==0){//是否乐于分享
						_this.memberInfo.IsShare='否';
					}else if(_this.memberInfo.IsShare==1){
						_this.memberInfo.IsShare='是';
					}else{
						_this.memberInfo.IsShare='';
					}
					
					_this.memberInfo.JoinDate=_this.memberInfo.JoinDate.substring(0,10);//入行时间
					var jiondate=_this.memberInfo.JoinDate.split('-');//入行时间
	        var nowdate = new Date();//现在时间
	        _this.kitchenOld=nowdate.getFullYear()-jiondate[0];//厨龄
	        
					if(_this.memberInfo.JoinDate=='1900-01-01'){
						_this.memberInfo.JoinDate='';//入行时间
						_this.kitchenOld='';//厨龄
					}
					
					if(_this.memberInfo.IsManyExp==0){//多城市从业经验
						_this.memberInfo.IsManyExp='否';
					}else if(_this.memberInfo.IsManyExp==1){
						_this.memberInfo.IsManyExp='是';
					}else{
						_this.memberInfo.IsManyExp='';
					}
					
				}
			})
		},
		getMemberResumeList(){//获取从业经历列表
  		var _this=this;
	    var params={
		    'MemberId':this.userId
			}
			this.$http({
				method:'post',
				url:apiUrl.getMemberResumeList,
				data:params
			}).then(function(response){
				var data=JSON.parse(response.data);
				if(data.length!=0){
					_this.dataList=data;
					for(let i=0;i<_this.dataList.length;i++){
						var datalist=_this.dataList[i];
						var BeginTime=datalist.BeginTime.substring(0,4)				
						var EndTime=datalist.EndTime.substring(0,4);
						datalist.BeginTime=BeginTime;
						datalist.EndTime=EndTime;
					}
				}	
			})
    },
    share(){
    	  var _this=this;
        var localIds ='';
		  	this.$http({
		  		method:'GET',
		  		url:apiUrl.getTimestampAndNonceStr
		  	}).then(function(response){
		  		var data=response.data;
		  		var dataArr=data.split('|');
		  		_this.timestamp=parseInt(dataArr[0]);//时间戳
		  		_this.nonceStr=dataArr[1];//随机字符串
		  		
		      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
		  		_this.$http({
		  			method:'GET',
		  			url:apiUrl.getJsapiTicketSignature+params
		  		}).then(function(response){
		  			  _this.signature=response.data;
		  			  
		  			  wx.config({
					      debug:false,
					      appId: apiUrl.appId,
					      timestamp: _this.timestamp,
					      nonceStr: _this.nonceStr,
					      signature: _this.signature,
					      jsApiList: [
					        'onMenuShareTimeline',//分享到朋友圈
					        'onMenuShareAppMessage'//分享给朋友
					      ]
					    })
		  			  
		  			  wx.ready(function(){
		    				wx.onMenuShareTimeline({
				            title:_this.memberInfo.MemberName+'的个人简历',
				            link:apiUrl.getUrl+'/#/component/resumepreview?userId='+_this.userId,
				            imgUrl:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTBBdOsxBWfgSXneaRSyIMkHF6pnyFT0pENGpPBNzIL28icib6oxfXjVJw/0?wx_fmt=jpeg',
				            success: function () { 
				              // 用户确认分享后执行的回调函数
//				              alert('分享到朋友圈成功');
				              _this.addECWXTranspondDetails(2);
				            },
				            cancel: function () { 
				              // 用户取消分享后执行的回调函数
//				              alert('你没有分享到朋友圈');
				            },
				            fail:function(res){
				            	alert(res.errMsg);
				            }
		            });
		            wx.onMenuShareAppMessage({
										title: _this.memberInfo.MemberName+'的个人简历', // 分享标题
										desc: _this.memberInfo.MemberName+'的个人简历', // 分享描述
										link: apiUrl.getUrl+'/#/component/resumepreview?userId='+_this.userId, // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
										imgUrl:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTBBdOsxBWfgSXneaRSyIMkHF6pnyFT0pENGpPBNzIL28icib6oxfXjVJw/0?wx_fmt=jpeg',// 分享图标
										success: function () {
										// 用户确认分享后执行的回调函数
//										  alert('分享给朋友成功');
										  _this.addECWXTranspondDetails(1);
										},
										cancel: function () {
										// 用户取消分享后执行的回调函数
//										  alert('你没有分享给朋友');
										},
										fail:function(res){
//				            	alert(res.errMsg);
				            }
								});
    			    })
		  		})
		  	})
    },
    addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
    	var params={
  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/resumepreview",
  		  "Parameter":"",
  		  "OpenId":this.openId,
  		  "TranspondType":type
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addECWXTranspondDetails,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
  		})
    }
  },
  mounted(){
  	$('#vux_view_box_body').css('background','#fff');
  	this.userData=cookie.get('WeiXinUser',function(val){
  		  var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
  	}) 
	  this.openId=this.userData.Openid;
	  
	  var str=window.location.href;
	  if(str.indexOf('?')<0){//没有userId
	  	this.userId=parseInt(this.userData.UserId);
	  }else{
	  	str=str.split('#')[1];
	  	str=str.split('?')[1];
	  	this.userId=str.split('=')[1];
	  }
	  
    this.getMemberProfile();//简历信息
    this.getMemberResumeList();//从业经历列表
    var uri=window.location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
		this.share();
  }	
}
</script>

<style scoped>
.resumepreview{letter-spacing: 1px;}
.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;margin-bottom: 15px;}
.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 5px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
.myphoto img{width:100%;height: 100%;border-radius: 50%;}
.title{width:70px;height:20px;line-height:20px;background: #E83428;color: white;font-size: 15px;text-align: center;margin-left: 8px;}
.titles{margin-top: 30px;}
.resumeDiv{border-top: 1px solid #E83428;padding-left: 10px;padding-right: 10px;font-size: 14px;}
.resumePre{margin-top: 20px;}	
.resume_text{color: #979797;display: inline-block;width: 120px;}
.resume_content{color: #3e3e3e;}
.timeline-demo .recent {color: rgb(4, 190, 2)}
.timeline-demo p {color: #828081;font-size: 0.8rem;}
.timeline-demo 	h4 {color: #000000;font-weight: normal;}
.timeBox{display: flex;margin-top: 20px;}
.timeline_text{width: 120px;}
.timeline_text p{margin-bottom: 60px;}
.textarea{width: 96%;height: 105px;border: 1px solid #afafaf;margin-top: 18px;color: #3e3e3e;font-size: 13px;text-indent: 1em;padding-left: 5px;padding-right: 5px;}
.prompt_title{color: #E83428;font-size: 13px;margin-top: 15px;}
.prompt_text{color: #7f7f7f;font-size: 12px;margin-bottom: 20px;}
</style>
<style>
#vux_view_box_body{background: #fff;}	
.resumepreview .vux-timeline{padding: 0 !important;}
.resumepreview .vux-timeline-item-tail{background: #e4e5e9 !important;}
.resumepreview .vux-timeline-item-checked{background:#f79896;border-radius: 50%;}
/*.resumepreview .weui-icon-success-no-circle:before{display: none !important;}*/
.resumepreview .vux-timeline-item-color{background: #e4e5e9 !important;}
.resumepreview .vux-timeline-item-content{padding: 0 0 1.5rem 4rem !important;}
</style>