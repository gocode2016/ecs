<template>
  <div class="footresult">
    
    <div class="listDiv">
    	<div class="listBox" v-for="(item,index) in footList" :key="index" @click="goChefFoot(item.MemberId,item.OpenId)">
    		<div>
    			<p class="listBox-content">{{item.PageShort}}</p>
    			<p class="listBox-time">{{item.CreateTime}}</p>
    		</div>
    		<div>
    			<p v-show="userType==2" class="listBox-remark">{{item.PageDetail}}</p>
          <p v-show="userType==1" class="listBox-name">{{item.MemberName}}</p>
    			<p v-show="userType==1" class="listBox-remarks">{{item.PageDetail}}</p>
    		</div>
    	</div>
    </div>
    <p class="prompt" v-show="promptFlag">没有搜索到相关内容</p>
    
  </div>
</template>

<script>
import { cookie } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		cookie
	},
	data(){
		return{
			userData:{},
			userType:'',
			openId:'',
			footList:[],
			keyword:'',//关键字
			pageIndex:0,
			promptFlag:false
		}
	},
	methods:{
		getFootPirntMember(){//获取会员足迹
			var _this=this;
			var params={
	  		  "OpenId":this.openId,
	  		  "PageIndex":this.pageIndex,
	  		  "UserType":this.userType,
  		    "Name":this.keyword
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getFootPirntMember,
	  			data:params
	  		}).then(function(response){
				  _this.footList=JSON.parse(response.data);
				  if(_this.footList.length==0){
				  	_this.promptFlag=true;
				  }
	  			for(var i in _this.footList){
	  				var footlist=_this.footList[i];
	  				footlist.CreateTime=footlist.CreateTime.substring(0,10);
	  			}
	  		})
		},
		goChefFoot(memberId,openId){//跳转到厨师足迹
			if(this.userType==1){
				this.$router.push({path:'/component/cheffootmark',query:{memberId:memberId,openId:openId}})
			}
		}
	},
	mounted(){
		this.keyword=this.$route.query.keyword;
		this.userData=cookie.get('WeiXinUser',function(val){
	  		var a = val.split("&");
			  var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
	  }) 
		this.userType=parseInt(this.userData.UserType);
		this.openId=this.userData.Openid;
		this.getFootPirntMember();
	}
}
</script>

<style scoped>
.listDiv{width: 100%}
.listBox{display: flex;/*height: 60px;*/border-bottom: 1px solid #F8F8F8;padding: 10px 6%;box-sizing: border-box;}
.listBox div:nth-child(1){width: 45%;}
.listBox-content{font-size: 14px;color: #3e3e3e;}
.listBox-time{font-size: 12px;color: #888888;}
.listBox div:nth-child(2){width: 55%;text-align: right;}
.listBox-remark{font-size: 14px;color: #ea3427;}
.listBox-name{font-size: 14px;color: #ea3427;}
.listBox-remarks{font-size: 12px;color: #3E3E3E;}
.prompt{text-align: center;margin-top: 10px;color: #3E3E3E;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>