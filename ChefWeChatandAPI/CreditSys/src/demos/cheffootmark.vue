<template>
  <div class="cheffootmark">
    
    <!--用户部分-->
    <div class="person_top">
    	 <div class="personphoto"><div class="myphoto"><img :src="userImg"/></div></div>   	 
    	 <p class="myName">{{chefInfo.MemberName}}</p>
    	 <grid :rows="4">
		      <grid-item>
		        <p class="grid-center grid-center1">ID</p>
		        <p class="grid-center">{{userId}}</p>
		      </grid-item>
		      <grid-item>
		        <p class="grid-center grid-center1">等级</p>
		        <p class="grid-center" v-if='showFlag'>会员已认证</p>
		        <p class="grid-center wrz" v-else>会员未认证</p>
		      </grid-item>
		      <grid-item>
		        <p class="grid-center grid-center1">当前积分</p>
		        <p class="grid-center">{{chefInfo.LeaveIntegral}}</p>
		      </grid-item>
	        <grid-item>
	          <p class="grid-center grid-center1">总积分</p>
	          <p class="grid-center">{{chefInfo.TotalIntegral}}</p>
	        </grid-item>
	      </grid>
    </div>
    <!--活动足迹-->
    <div class="listDiv">
    	<div class="listBox" v-for="(item,index) in footList" :key="index">
    		<div>
    			<p class="listBox-content">{{item.PageShort}}</p>
    			<p class="listBox-time">{{item.CreateTime}}</p>
    		</div>
    		<div>
    			<p class="listBox-remark">{{item.PageDetail}}</p>
    		</div>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { Grid, GridItem } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Grid, 
		GridItem
	},
	data(){
		return{
			showFlag:true,
			userId:'',//会员userid
			openId:'',//会员openid
			chefInfo:{},//厨师信息
			userImg:'',
			footList:[],
			pageIndex:0
		}
	},
	methods:{
		getFootPirntMember(){//获取会员足迹
			var _this=this;
			var params={
  		  "OpenId":this.openId,
  		  "PageIndex":this.pageIndex,
  		  "UserType":2,
  		  "Name":''
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getFootPirntMember,
  			data:params
  		}).then(function(response){
  			_this.footList=JSON.parse(response.data);
  			for(var i in _this.footList){
  				var footlist=_this.footList[i];
  				footlist.CreateTime=footlist.CreateTime.substring(0,10);
  			}
  		})
		},
		getMemberById(){//获取厨师信息
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
			  	_this.chefInfo=data[0];
	  			if(_this.chefInfo.RecommendId==0){//0未认证  
	  				_this.showFlag=false;
	  			}else if(_this.chefInfo.RecommendId>0){//>0已认证
	  				_this.showFlag=true;
	  			}
	  			_this.chefInfo.ImgUrl==''||_this.chefInfo.ImgUrl==null ? _this.userImg='../../static/credit/perimg.png' : _this.userImg=_this.chefInfo.ImgUrl;
			  }else{
			  	_this.userImg='../../static/credit/perimg.png';
			  }
  		})
  	}
	},
	mounted(){
		this.userId=this.$route.query.memberId;
		this.openId=this.$route.query.openId;
		this.getMemberById();//厨师信息
		this.getFootPirntMember();//厨师活动足迹
	}
}
</script>

<style scoped>
	.person_top{width: 100%;height: 220px;background: url('../../static/credit/perback.png') no-repeat;background-size: 100% 100%;border-bottom: 3px solid #F8F8F8;}
	.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;}
	.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 3px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
	.myphoto img{width:100%;}
	.myName{text-align: center;}
	.grid-center{text-align: center;font-size: 12px;color: #3E3E3E;}
	.grid-center1{color: #E83428;}
	.wrz:after{content: " "; display: inline-block; height: 6px; width: 6px;border-width: 1.5px 1.5px 0 0;border-color: #E83428;border-style: solid;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);}
	.listDiv{width: 100%;}
	.listBox{display: flex;height: 60px;border-bottom: 1px solid #F8F8F8;padding: 10px 6%;box-sizing: border-box;}
	.listBox div:nth-child(1){width: 45%;}
	.listBox-content{font-size: 14px;color: #3e3e3e;}
	.listBox-time{font-size: 12px;color: #888888;}
	.listBox div:nth-child(2){width: 55%;text-align: right;}
	.listBox-remark{font-size: 14px;color: #e93526;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.cheffootmark .weui-grid{padding: 13px 13px !important;}
	.cheffootmark .weui-grids:before{border-top: 0 !important;}
	.cheffootmark .weui-grid:before{border-right: 0 !important;}
	.cheffootmark .weui-grid:after{border-bottom: 0 !important;}
</style>