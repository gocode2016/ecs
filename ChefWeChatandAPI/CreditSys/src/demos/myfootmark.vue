<template>
  <div class="myfootmark">
	  
	  <div class="footbox-top">
	  	<div class="searchDiv">
	    	<input type="text" v-model="keyword" placeholder="搜索关键字"/>
	    	<img src="../../static/credit/search.png" @click="searchClick"/>
	    </div>
	  </div>
    <div class="listDiv">
	    	<div class="listBox" v-for="(item,index) in footList" :key="index">
	    		<div>
	    			<p class="listBox-content">{{item.PageShort}}</p>
	    			<p class="listBox-time">{{item.CreateTime}}</p>
	    		</div>
	    		<div>
	    			<p>{{item.PageDetail}}</p>
	    		</div>
	    	</div>
	  </div>
  
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
			userId:'',
			userType:'',
			openId:'',
			footList:[],
			keyword:'',//关键字
      pageIndex:0
		}
	},
	methods:{
		getFootPirntMember(){//获取会员足迹
			var _this=this;
			var params={
  		  "OpenId":this.openId,
  		  "PageIndex":this.pageIndex,
  		  "UserType":this.userType,
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
		searchClick(){//点击搜索按钮
			if(this.keyword!=''){
				this.$router.push({path:'/component/footresult',query:{keyword:this.keyword}});
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
	  this.userId=parseInt(this.userData.UserId);
	  this.userType=parseInt(this.userData.UserType);
	  this.openId=this.userData.Openid;
	  
		this.getFootPirntMember();//获取会员足迹
	}
}
</script>

<style scoped>
.footbox-top{width: 100%;border-bottom: 3px solid #F8F8F8;background: #fff;}
.searchDiv{width: 75%;height: 30px;background: #f8f8f8;border-radius: 5px;margin-left: 3%;margin-top: 12px;margin-bottom: 12px;}
.searchDiv *{display: inline-block;vertical-align: middle;}
.searchDiv input{width: 85%;border: none;background:#f8f8f8;height: 30px;outline: none;margin-left:5%;}
.searchDiv img{width: 6%;}
.listDiv{width: 100%;}
.listBox{display: flex;height: 60px;border-bottom: 1px solid #F8F8F8;padding: 10px 6%;box-sizing: border-box;}
.listBox div:nth-child(1){width: 45%;}
.listBox-content{font-size: 14px;color: #3e3e3e;}
.listBox-time{font-size: 12px;color: #888888;}
.listBox div:nth-child(2){width: 55%;text-align: right;font-size: 14px;color: #ea3427;}
.sljz{display: none;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>