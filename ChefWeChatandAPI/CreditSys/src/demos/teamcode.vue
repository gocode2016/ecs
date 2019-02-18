<template>
  <div class="teamcode">
    
    <img :src="codeImg" />
    <ul>
    	<li><span>姓名</span><span>{{salemanInfo.Name}}</span></li>
    	<li><span>联系方式</span><span>{{salemanInfo.Telephone}}</span></li>
    	<li><span>大区</span><span>{{salemanInfo.RegionName}}</span></li>
    	<li><span>工作地点</span><span>{{salemanInfo.ProvinceName}}{{salemanInfo.CityName}}{{salemanInfo.AreaName}}</span></li>
    </ul>
    
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
				salemanInfo:{},
				codeImg:''
			}
		},
		methods:{
			getSaleManInfo(){//获取会员信息
		  		var _this=this;
		  		var params={
		  		  "SalemanId":this.userId
		  		}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.getSaleManInfo,
		  			data:params
		  		}).then(function(response){
		  			var data = JSON.parse(response.data);
		  			_this.salemanInfo = JSON.parse(data.Data);
		  		})
		  },
		  showCode(){//获取二维码
		  	var _this=this;
		  	var params="?salemanId="+this.userId
	  		this.$http({
	  			method:'get',
	  			url:apiUrl.createSubscribeQRcode + params,
	  		}).then(function(response){
	  			 _this.codeImg="https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket="+response.data;
	  		})
		  }
		},
		mounted(){
			var width=$(window).width();//浏览器宽度
			$('.teamcode img').css({
				'margin-top':width*0.1,
				'height':width*0.6//二维码高度
			});
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
			  this.getSaleManInfo();//队员信息
			  this.showCode();//二维码
		}
	}
</script>

<style scoped>
.teamcode img{width: 60%;margin-left: 20%;}
.teamcode ul li{list-style:none;height: 54px;line-height:54px;border-bottom: 1px solid #f1f1f1;font-size: 12px;padding:0 10px;}
.teamcode ul li span:nth-child(1){color: #3E3E3E;float: left;}
.teamcode ul li span:nth-child(2){color: #888888;float: right;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
</style>