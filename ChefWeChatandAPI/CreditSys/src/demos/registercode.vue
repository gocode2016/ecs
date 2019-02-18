<template>
  <div class="registercode">
      <tab :line-width=2 active-color='#e83428' v-model="index" custom-bar-width="60px">
        <tab-item class="vux-center" :selected="demo === item" v-for="(item, index) in list" @click="demo = item" :key="index" @click.native='clickPage(index)'>{{item.text}}（{{item.count}}）</tab-item>
      </tab>
      
      <div v-if="showflag">
        <div class="tab-swiper vux-center" v-for="(item,index) in textlist" :key="index">
      	 	  <div>
      	 	  	<p class="swiper_text_id">ID:{{item.RegistCodeId}}</p>
      	 	  </div>
      	 	  <div>
      	 	  	<p class="swiper_text_time">{{item.GenerDate}}</p>
      	 	  </div>
        </div>
        <x-button  @click.native="addRegistCode" style="margin-top: 25px;">生成</x-button>
      </div> 
      
      <div v-else>
      	<div class="tab-swiper tab-swiper1 vux-center" v-for="(item,index) in textlist1" :key="index">
					 	<div>
					 	  <p class="swiper_text">ID:{{item.RegistCodeId}}</p>
					 	</div>
					 	<div>
					 	  <p class="swiper_text">{{item.MemberName}}</p>
					 	</div>
					 	<div>
					 	  <p class="swiper_text">{{item.HotelName}}</p>
					 	  <p class="swiper_date swiper_date_act">{{item.GenerDate}}</p>
					 	</div>
				</div>
      </div>
	        
  </div>
</template>

<script>
import { Tab, TabItem, Swiper, SwiperItem, Search, AjaxPlugin, XButton,cookie } from 'vux'
import apiUrl from '../apiUrls.js'
export default{
	components:{
		Tab, 
		TabItem,
		Swiper,
		SwiperItem,
		Search,
		AjaxPlugin,
		XButton,
		cookie
	},
	data(){
		return{
			showflag:true,
			userData:{},
	    userId:'',
			list:[],
			index: 0,
			demo: '全部',
			doStart:true,
			startval:100,
			endval:200,
			textlist:[],//未使用列表
			textlist1:[],//已使用列表
		}
	},
	methods:{
		clickPage(index){
			if(index==0){
				this.showflag=true;
			}else{
				this.showflag=false;
			}
		},
   	registCodeList(index){//注册码列表
   		var _this=this;
   		var params={
   			"SalemanId":this.userId,
   			"CodeState":index
   		}
  		this.$http({
  			method:'post',
  			url:apiUrl.registCodeList,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
				for(let i in data){
					data[i].GenerDate=data[i].GenerDate.substring(0,19);
					data[i].GenerDate=data[i].GenerDate.replace(/-/g,'/');
					data[i].GenerDate=data[i].GenerDate.replace('T',' ');
				}
         if(index==1){//未注册
         	 _this.textlist=data;
         }else if(index==2){//已注册
         	 _this.textlist1=data;
         }
         _this.list=[{
			   	 text:'未使用',
				   count:_this.textlist.length
			   },{
			   	 text:'已使用',
				   count:_this.textlist1.length
			   }]
  		})
   	},
   	addRegistCode(){//生成注册码
   		var _this=this;
   		var params="?salemanId="+this.userId;
  		this.$http({
  			method:'get',
  			url:apiUrl.addRegistCode+params
  		}).then(function(response){
//			console.log(response.data);
  			_this.registCodeList(1);//获取注册码列表 未注册
		    _this.registCodeList(2);//获取注册码列表 已注册
  		})
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
//		this.userId=10003;//测试
		this.registCodeList(1);//获取注册码列表 未注册
		this.registCodeList(2);//获取注册码列表 已注册
	}
}
</script>

<style scoped>
	
	
.inte_header{width: 100%;height: 110px;text-align:center;line-height:110px;background: url('../../static/credit/inteImg.png') no-repeat;background-size: 100% 100%;}
.demo{font-size: 30px;color: #E83428;}
.tab-swiper{width: 100%;height: 76px;line-height:76px;border-bottom:1px solid #ebebeb;font-size: 13px;display: flex;}
.tab-swiper div:nth-child(1){width: 40%; padding-left: 10%; }
.tab-swiper div:nth-child(2){width: 60%; text-align: right;padding-right: 10%;}
.swiper_date{margin-top: 5px;color: #787878;font-size: 12px;}
.tab-swiper1{line-height:20px;}
.swiper_text{margin-top: 15px;}
.tab-swiper1 div:nth-child(1){width: 25%; padding-left: 5%; }
.tab-swiper1 div:nth-child(2){width: 30%; margin-left: 8%;padding-right: 0; text-align: left; }
.tab-swiper1 div:nth-child(3){width: 45%;text-align: right;padding-right: 5%;}
.swiper_date_act{color:#1c1c1c;}

</style>
<style>
#vux_view_box_body{background: #fff;}
.registercode .vux-tab-item{font-size: 13px !important;color: #000000 !important;}
.registercode .vux-tab-selected{color: #E83428 !important;}
.registercode .weui-search-bar { background-color: #fff !important; }
.registercode .weui-search-bar:before { border-top: 1px  solid #fff; color: #fff; }
.registercode .weui-search-bar:after { border-bottom: 1px solid #fff; color: #fff; } 
.registercode .weui-search-bar__form,
.registercode .weui-search-bar__box,
.registercode .weui-search-bar__label { background: #efefe4 !important; }
.registercode .sort { height: 40px; text-align: center; }
.registercode .sort span { display: inline-block; height: 40px; line-height: 40px; width: 120px; }
.registercode .sort .activity { margin-right: 30px; }
.registercode .sort .integral {  }
.registercode button { width: 50% !important; height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 30%;}
.registercode .vux-slider > .vux-swiper > .vux-swiper-item{background: #fff;}
</style>
