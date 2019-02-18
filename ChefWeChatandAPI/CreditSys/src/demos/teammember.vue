<template>
  <div class="teammember">
    <!--用户部分-->
    <div class="person_top">
    	 <div class="edit" @click="jump(editUrl)"><img src="../../static/credit/team-bj.png"></div>
    	 <div class="personphoto"><div class="myphoto"><img :src="wxImg"/></div></div>   	 
    	 <p class="myName">{{salemanInfo.Name}}<img src="../../static/credit/team-ewm.png" @click="jump(codeUrl)"></p>
    	 <grid :rows="3">
		      <grid-item></grid-item>
		       <grid-item>
		        <p class="grid-center grid-center1">工号</p>
		        <p class="grid-center">{{salemanInfo.WorkNum}}</p>
		      </grid-item>
		       <grid-item></grid-item>
      </grid>
    </div>
    
    <!--订单-->
    <div class="person_icon">
    	<grid :rows="3">
	      <grid-item  :label="item.text" v-for="(item,index) in list" :key="index" class="gridpad" @on-item-click="jump(item.url)">
	        <img slot="icon" :src="item.img">
	      </grid-item>
      </grid>
    </div>
    
    <!--审核中-->
    <div class="sucDiv" v-show='shFlag'>
    	<img src="../../static/credit/success.png"/>
      <p>您正在等待</p>
      <p>欣和酒店业务代表身份审核中</p>
    </div>
    
    <!--禁用该队员提示-->
    <div class="mask" v-show='enableFlag'>
    	<p style="text-align: center;font-size: 18px;color: #fff;margin-top: 200px;">您的账户已被禁用</p>
    </div>
    
    <div class="masks" v-show="maskFlag">
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
import { Grid, GridItem,Scroller,Group,Cell, CellBox,Card,cookie} from 'vux'
export default {
	components: {
    Grid, 
    GridItem,
    Scroller,
    Group,
    Cell,
    CellBox,
    Card,
    cookie
  },
  data(){
  	return{
  		maskFlag:true,
  		enableFlag:false,//禁用页面
  		shFlag:false,//审核中页面
  		salemanInfo:{},//会员信息
  		wxImg:'',//微信头像
  		userData:'',
  		userId:'',
  		editUrl: '/component/teammemberedit',
  		codeUrl:'/component/teamcode',
  		list:[{
  			img:'../../static/credit/team1.png',
  			text:'认证码',
  			url:'/component/registercode'
  		},{
  			img:'../../static/credit/team2.png',
  			text:'绑定厨师',
  			url:'/component/associatechef'
  		},{
  			img:'../../static/credit/team3.png',
  			text:'推广日历',
  			url:'/component/extensioncalendarmonth'
  		},{
  			img:'../../static/credit/team4.png',
  			text:'常见问题',
  			url:'/component/feedback'
  		},{
  			img:'../../static/credit/team5.png',
  			text:'业绩贡献',
  			url:'/component/contribution'
  		},{
  			img:'../../static/credit/per7.png',
  			text:'活动足迹',
  			url:'/component/footmark'
  		}]
  	}
  },
  methods:{
  	joinClick(){//点击参与
  		window.location.href="https://mp.weixin.qq.com/s/py4eXQuX0xGvoNwEfeXt8w"
  	},
  	closeClick(){//点击关闭
  		this.maskFlag = false;
  		$('.teammember').css({
			  'overflow':'scroll'
		  })
  	},
  	getSaleManInfo(){//获取队员信息
  		var _this=this;
  		var params={
  		  "SalemanId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSaleManInfo,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.salemanInfo=JSON.parse(data.Data);
  			
  			if(_this.salemanInfo.RegistState==0){//审核中
					_this.shFlag=true;
				}
  			if(_this.salemanInfo.IsEnable==1){//禁用该队员
  			  _this.enableFlag=true;
  			}
  			
  		})
  	},
  	jump(link){
  		this.$router.push(link);
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
  	this.userId=parseInt(this.userData.UserId);
	  this.wxImg=this.userData.Headimgurl;//微信头像  	
		this.getSaleManInfo();//获取会员信息
  	
  	//判断是否显示指引蒙版
    var storage=window.localStorage;
    var billteam=storage.getItem("billteam");//获取storage
    if(billteam==null){//没有记录
    	//第一次显示指引层
    	this.maskFlag = true;
    	storage.setItem("billteam",'yes');//设置
    	$('.teammember').css({
			  'overflow':'hidden'
		  })
    }else{
    	//不显示指引层
    	this.maskFlag = false;
    }
  	
  }
}
</script>
<style scoped>
	.teammember{background: #fbfbfb;height: 100%;overflow: scroll;}
	.person_top{width: 100%;height: 200px; position: relative; background: url('../../static/credit/teamback.png') no-repeat;background-size: 100% 100%;}
	.person_top .edit { width: 30px; height: 30px; position: absolute; z-index: 1000; right: 5px; top: 20px;}
	.edit img{width: 23px;}
	.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;}
	.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 5px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
	.myphoto img{ width: 100%;height: 100%;border-radius: 50%; }
	.myName{text-align: center;}
	.grid-center{text-align: center;font-size: 12px;color: #3E3E3E;}
	.grid-center1{color: #E83428;}
    
  .person_icon{width: 100%;background: #fff;margin-top: 7px;border-top: 1px solid #f1f1f1;}
  .gridpad{border-bottom: 1px solid #f1f1f1;}
  .myName img{width: 14px;margin-left: 10px;}
  
  .sucDiv{width: 100%;height: 100%;position: fixed;top: 0px;background: #FFFFFF;text-align: center;}    
  .sucDiv img{ width: 160px;margin-top: 90px;}
  .sucDiv p:nth-child(2){font-size: 17px;color: #E83428;margin-top: 15px;}
  .sucDiv p:nth-child(3){font-size: 13px;color: #707070;margin-top: 10px;}
  .sucDiv button{width: 90%;height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 30%;}
  
  .mask{width: 100%;height: 100%;position: fixed;top: 0;background: rgba(0,0,0,0.5);z-index: 10000;}
  .masks{width: 100%;height: 100%;position: fixed;top: 0;background: rgba(0,0,0,0.7);z-index: 1000;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.teammember .weui-grid{padding: 13px 13px !important;}
	.teammember .weui-grids:before{border-top: 0 !important;}
	.teammember .weui-grid:before{border-right: 0 !important;}
	.teammember .weui-grid:after{border-bottom: 0 !important;}
	.teammember .weui-grid__icon{width: 45px !important;height: 45px !important;}
	.teammember .weui-grid__label{font-size: 12px !important;color: #3E3E3E !important;}
	.teammember .weui-cell{height: 35px !important;padding: 10px 20px;}
	.teammember .weui-cell:before {border-top: 1px solid #f1f1f1 !important;margin-left: 7px;margin-right: 7px;}
	.teammember .vux-label {font-size: 12px; color: #3e3e3e;margin-left: 27px;}
	.teammember .weui-cell__ft{color: #E83428 !important;font-size: 12px;}
  .teammember .weui-cell__ft:after {height: 7px !important; width: 7px !important; border-width: 1px 1px 0 0 !important;border-color: #E83428 !important;}
  .teammember .vux-cell-box{color: #e83428;font-size: 13px;} 
  .teammember .vux-cell-box img{width: 15px;margin-right: 12px;position: relative;top: 2px;} 
</style>