<template>
  <div class="personal">
    <!--用户部分-->
    <div class="person_top">
    	 <div class="personphoto"><div class="myphoto"><img :src="userImg"/></div></div>   	 
    	 <p class="myName">{{data.MemberName}}</p>
    	 <grid :rows="3">
		      <grid-item>
		        <p class="grid-center grid-center1">ID</p>
		        <p class="grid-center">{{userId}}</p>
		      </grid-item>
		      <grid-item>
		        <p class="grid-center grid-center1">等级</p>
		        <p class="grid-center" v-if='showFlag'>会员已认证</p>
		        <p class="grid-center wrz" v-else @click="jump('/component/authenmethod')">会员未认证</p>
		      </grid-item>
		      <grid-item>
		        <p class="grid-center grid-center1">积分</p>
		        <p class="grid-center">{{data.LeaveIntegral}}</p>
		      </grid-item>
      </grid>
    </div>
    <div class="person_tops"></div>
    
    <!--订单-->
    <div class="person_icon">
    	<grid :rows="3">
	      <grid-item  :label="item.text" v-for="(item,index) in list" :key="index" class="gridpad" @on-item-click="jump(item.url)">
	        <img slot="icon" :src="item.img">
	      </grid-item>
      </grid>
    </div>
    
    <!--米其林图片-->
    <!--<img src="../../static/credit/personal_mql.png" style="width: 100%;">-->
    
    <!--获得更多积分-->
    <div class="inte">
      <cell-box><img src="../../static/credit/jfen.png"/>获得更多积分 </cell-box>	       
      <cell is-link :link='item.url' v-for="(item,index) in inte" :key="index" :title="item.title" :value="item.value"  v-bind:style="{background:'url('+item.img+') no-repeat 20px center',backgroundSize:'15px'}"></cell>      
    </div>
    
    <!--备案-->
    <div style="position: relative;margin-top: 20px;margin-bottom: 80px;" >
    	<span class="record" @click="recordClick">鲁ICP备10013060号</span>
    </div>
    
    <div class="mask" v-show="maskFlag">
    	<div style="width: 70%;margin-top: 16%;text-align: center;margin-left: 15%;">
	      <p style="text-align: right;"><img src="../../static/credit/bill_close.png" style="width: 20px;" @click="closeClick"/></p>
	      <img src="../../static/credit/bill_img.png" style="width: 100%;"/>
	      <img src="../../static/credit/bill_btn.png" style="width: 40%;margin-top: 3%;" @click="joinClick"/>
    	</div>
    </div>
    
  </div>
</template>
<script>
import apiUrl from '../apiUrls.js'
import { Grid, GridItem,Scroller,Group,Cell, CellBox,Card,cookie,Toast} from 'vux'
export default {
	components: {
    Grid, 
    GridItem,
    Scroller,
    Group,
    Cell,
    CellBox,
    Card,
    cookie,
    Toast
  },
  data(){
  	return{
  		maskFlag:true,
  		userImg:'',//头像
  		userData:'',
  		openId:'',
  		userId:'',
  		userType:'',
  		data:'',
  		showFlag:true,
  		IsSiginCredit:'',//当天是否签到
  		reslevel:0,//简历完善度
  		list:[{
  			img:'../../static/credit/per1.png',
  			text:'我的订单',
  			url:'/component/orderlist'
  		},{
  			img:'../../static/credit/per2.png',
  			text:'积分明细',
  			url:'/component/integraldetail'
  		},{
  			img:'../../static/credit/per3.png',
  			text:'求职简历',
  			url:'/component/resume'
  		},{
  			img:'../../static/credit/per4.png',
  			text:'菜品互动',
  			url:'/component/dishshow'
  		},{
  			img:'../../static/credit/per7.png',
  			text:'活动足迹',
  			url:'/component/myfootmark'
  		},{
  			img:'../../static/credit/per6.png',
  			text:'常见问题',
  			url:'/component/feedback'
  		}],
		  inte:[
		  {
				img:'../../static/credit/jf3.png',
				title:'推荐同行注册',
				value:'',
				url:'/component/recommendstart'
			},
//    {/component/recommendstart
//				img:'../../static/credit/jf4.png',
//				title:'指定页面分享',
//				value:'10积分',
//				url:'/component/pageshare'/component/recommendstart
//			},
			{
  			img:'../../static/credit/jf1.png',
  			title:'每日签到',
  			value:'已签到',
  			url:'/component/sign'
  		},{
  			img:'../../static/credit/jf2.png',
  			title:'简历完善',
  			value:'0%',
  			url:'/component/resume'
  		}] 	
  	}
  },
  methods:{
  	joinClick(){//点击参与
  		window.location.href="https://mp.weixin.qq.com/s/py4eXQuX0xGvoNwEfeXt8w"
  	},
  	closeClick(){//点击关闭
  		this.maskFlag = false;
  		$('.personal').css({
			  'overflow':'scroll'
		  })
  	},
  	recordClick(){//点击备案
			window.location.href = 'http://www.miitbeian.gov.cn/state/outPortal/loginPortal.action;jsessionid=yZGh3fHt8OZyxjnAk5z89RzZPqOh3mGi-rolRbIEDyLR4-2KQcQp!-736616813'
		},
  	getMemberById(){//个人中心首页信息
  		var _this=this;
  		var params={
  		  'MemberId':this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getMemberById,
  			data:params
  		}).then(function(response){  	
  			var data=JSON.parse(response.data); 
//			console.log(data);
			  if(data.length!=0){
			  	_this.data=data[0];
			  	if(_this.data.ImgUrl==null||_this.data.ImgUrl==""){
				  	_this.userImg=_this.userData.Headimgurl;//微信头像
				  }else{
				  	_this.userImg=_this.data.ImgUrl;//基本信息头像
				  }
				  
	  			if(_this.data.RecommendId>0){//>0已认证
	  				_this.showFlag=true;
	  			}else{
	  				_this.showFlag=false;
	  			}
	  			
			  }else{
			  	_this.userImg=_this.userData.Headimgurl;//微信头像
			  }
  		})
  	},
  	getRecQualifications() {//获取推荐名额
				var _this = this;
				this.$http({
					method: 'get',
					url: apiUrl.getRecQualifications + '?MemberId=' +  this.userId
				}).then(function(response) {
					var data=Number(response.data);
//					console.log(data);
          if(data<5){
           	_this.inte[0].value='20积分';
          }else if(data<10){
          	_this.inte[0].value='30积分';
          }else if(data<15){
          	_this.inte[0].value='40积分';
          }else if(data<20){
          	_this.inte[0].value='50积分';
          }else{
          	_this.inte[0].value='0积分';
          }
				})
		},
  	getSignin(){//最近7天签到状态
  		var _this=this;
  		var params={
  			"MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSignin,
  			data:params
  		}).then(function(response){
//			  console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data);
  			_this.IsSiginCredit=data.IsSiginCredit;//当天是否签到
			  _this.IsSiginCredit=='f' ? _this.inte[1].value='未签到' : _this.inte[1].value='已签到';
  		})
  	},
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
				console.log(data);
				if(data.length!=0){
					if(data[0].Nature!=null&&data[0].Nature!=""){
					  //基本信息完整
					  _this.reslevel+=20;
					}
					if(data[0].PerConsumption!=null&&data[0].PerConsumption!=""){
						//酒店信息完整
					  _this.reslevel+=20;
					}
					if(data[0].JobReason!=null&&data[0].JobReason!=""){
						//从厨理念完整
						 _this.reslevel+=5;
					}
					if(data[0].Major!=null&&data[0].Major!=""){
						//职业技能完整
						 _this.reslevel+=20;
					}
					if(data[0].MatchName!=null&&data[0].MatchName!=""){
						//荣誉资格完整
						 _this.reslevel+=15;
					}
					if(data[0].Introduction!=null&&data[0].Introduction!=""){
						//自我介绍完整
						_this.reslevel+=5;
					}		
				}
				_this.getMemberResumeList();//获取从业经历列表
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
					//从业经历完整
					_this.reslevel+=15;
				}
				_this.inte[2].value=_this.reslevel+'%';
			})
    },
    updateMemberIntegral(val){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'minus',
				'Integral':val,
				'MemberId':this.userId,
				'IntegralSource':'积分清零',
				'Remark':'积分清零'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
//			_this.$vux.toast.text('成功+1积分', 'middle');
  		})
  	},
//  isCommTranspond(){//是否分享朋友圈
//	    var _this=this;
//  	var params={
//			  "ECBrowseURL":apiUrl.getUrl+"/#/component/pageshare20180419",
//			  "OpenId":this.openId,
//			  "TranspondType":2
//		  }
//			this.$http({
//				method:'post',
//				url:apiUrl.isCommTranspond,
//				data:params
//			}).then(function(response){
//				  if(response.data=='OK'){//没有转发过
//				    _this.inte[0].value='10积分';
//	        }else if(response.data=='No'){
//				    _this.inte[0].value='已完成';
//	        }
//			})
//  },
  	jump(link){
  		this.$router.push(link);
  	},
  	onChange (val) {
      const _this = this
      if (val) {
        this.$vux.toast.show({
          text: 'Hello World',
          onShow () {
            console.log('Plugin: I\'m showing')
          },
          onHide () {
            console.log('Plugin: I\'m hiding')
            _this.show9 = false
          }
        })
      } else {
        this.$vux.toast.hide()
      }
	  }
  },
  mounted(){  	
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
    this.userId=parseInt(this.userData.UserId);
  	this.userType=parseInt(this.userData.UserType);
  	
		if(this.userType==1){//队员
			this.$router.push('/component/teammember');
		}else if(this.userType==0){//未注册
			this.$router.push('/component/registeredmembers');
		}else if(this.userType==2){//会员
			this.getMemberById();  
  	  this.getSignin();//是否签到
  	  this.getMemberProfile();//获取简历信息
	    this.getRecQualifications() ;//推荐同行注册
//	    this.isCommTranspond();//是否分享指定页面

      //判断是否显示指引蒙版
	    var storage=window.localStorage;
	    var bills=storage.getItem("bills");//获取storage
	    if(bills==null){//没有记录
	    	//第一次显示指引层
	    	this.maskFlag = true;
	    	storage.setItem("bills",'yes');//设置
	    	$('.personal').css({
				  'overflow':'hidden'
			  })
	    }else{
	    	//不显示指引层
	    	this.maskFlag = false;
	    }
		}
  }
}
</script>
<style scoped>
	.personal{background: #fbfbfb;height: 100%;overflow: scroll;}
	.person_top{width: 100%;height: 200px;background: url('../../static/credit/perback.png') no-repeat;background-size: 100% 100%;position: fixed;top: 0;z-index: 1000;}
	.person_tops{width: 100%;height: 200px;}
	.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;}
	.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 3px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
	.myphoto img{width:100%;height: 100%;border-radius: 50%;}
	.myName{text-align: center;}
	.grid-center{text-align: center;font-size: 12px;color: #3E3E3E;}
	.grid-center1{color: #E83428;}
    
  .person_icon{width: 100%;background: #fff;margin-top: 7px;border-top: 1px solid #f1f1f1;position: relative;}
  .gridpad{border-bottom: 1px solid #f1f1f1;}
  
  .inte{width: 100%;border-top: 1px solid #f1f1f1;background: #fff;margin-top: 7px;position: relative;}
  .inte .news{position: absolute;left: 43%;top: 70px;width: 30px;}
  @media screen and (max-device-width: 350px) {
	  .inte .news{left: 49%;}
	}
	@media screen and (min-device-width: 400px) {
	  .inte .news{left: 40%;}
	}
	.wrz:after{content: " ";display: inline-block;height: 7px;width: 7px; border-width: 1.5px 1.5px 0 0;border-color: #E83428;border-style: solid;
            position: relative;top: -0.5px;right: -2px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
	/*备案*/
  .record{padding:10px 16px;background: #f6f6f6;color: #818181;font-size: 12px;text-align: center;border-radius: 10px;letter-spacing: 1px;
          position: absolute;left: 50%;transform: translateX(-50%);}
  .mask{width: 100%;height: 100%;position: fixed;top: 0;background: rgba(0,0,0,0.7);z-index: 1000;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.personal .weui-grid{padding: 13px 13px !important;}
	.personal .weui-grids:before{border-top: 0 !important;}
	.personal .weui-grid:before{border-right: 0 !important;}
	.personal .weui-grid:after{border-bottom: 0 !important;}
	.personal .weui-grid__icon{width: 45px !important;height: 45px !important;}
	.personal .weui-grid__label{font-size: 12px !important;color: #3E3E3E !important;}
	.personal .weui-cell{height: 35px !important;padding: 10px 20px;}
	.personal .weui-cell:before {border-top: 1px solid #f1f1f1 !important;margin-left: 7px;margin-right: 7px;}
	.personal .vux-label {font-size: 12px; color: #3e3e3e;margin-left: 27px;}
	.personal .weui-cell__ft{color: #E83428 !important;font-size: 12px;}
  .personal .weui-cell__ft:after {height: 7px !important; width: 7px !important; border-width: 1px 1px 0 0 !important;border-color: #E83428 !important;}
  .personal .vux-cell-box{color: #e83428;font-size: 13px;} 
  .personal .vux-cell-box img{width: 15px;margin-right: 12px;position: relative;top: 2px;} 
</style>