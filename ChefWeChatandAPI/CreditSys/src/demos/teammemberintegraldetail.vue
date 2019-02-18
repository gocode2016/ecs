<template>
  <div class="teammemberintegraldetail">

    <div class="intetop">
	    <div class="inte_header">
	    	<countup :start="doStart" :start-val="startval" :end-val="endval" :duration="4"  class="demo"></countup>
	    </div>
		  <tab :line-width=2 active-color='#e83428' v-model="index" custom-bar-width="60px">
		    <tab-item class="vux-center" :selected="demo === item" v-for="(item, index) in list" @click="demo = item" :key="index" @click.native="clickPage(index)">{{item}}</tab-item>
		  </tab>
    </div>
    <div class="intetopback"></div>
    
	  <div v-show="allFlag">
	      <div class="tab-swiper vux-center" v-for="(item,index) in allList" :key="index">
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.IntegralSource}}</p>
	      	 	  	<p class="swiper_date">{{item.CreatDate}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.Remark}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	    <p class="swiper_text " :id='item.color' >{{item.IntegralNum}}</p>
	      	 	  </div>
	      </div>
	  </div>	  
	  <div v-show="incomeFlag">
	      <div class="tab-swiper vux-center" v-for="(item,index) in incomeList" :key="index">
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.IntegralSource}}</p>
	      	 	  	<p class="swiper_date">{{item.CreatDate}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.Remark}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	    <p class="swiper_text " :id='item.color' >{{item.IntegralNum}}</p>
	      	 	  </div>
	      </div>
	  </div>
	  <div v-show="payFlag">
	      <div class="tab-swiper vux-center" v-for="(item,index) in payList" :key="index">
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.IntegralSource}}</p>
	      	 	  	<p class="swiper_date">{{item.CreatDate}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.Remark}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	    <p class="swiper_text " :id='item.color' >{{item.IntegralNum}}</p>
	      	 	  </div>
	      </div>
	  </div>  
    
  </div>
</template>

<script>
import { Countup,Tab, TabItem, Swiper, SwiperItem,cookie } from 'vux'
import apiUrl from '../apiUrls.js'
export default{
	components:{
		Countup,
		Tab, 
		TabItem,
		Swiper,
		SwiperItem,
		cookie
	},
	data(){
		return{
			userData:{},
			userId:'',
			allList:[],//全部列表
			incomeList:[],//收入列表
			payList:[],//支出列表
			allFlag:true,
			incomeFlag:false,
			payFlag:false,
			list: ['全部', '收入', '支出'],
			index: 0,
			demo: '全部',
			doStart:true,
			startval:0,
			endval:0,
		}
	},
	methods:{
		getMemberIntegralList(){//获取积分列表
			var _this=this;
			var params="?memberId="+this.userId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getMemberIntegralList+params,
  		}).then(function(response){
//			  console.log(JSON.parse(response.data));
  			_this.allList=JSON.parse(response.data);
  			for(var i=0;i<_this.allList.length;i++){
  				var alllist=_this.allList[i];
  				if(i==0){
  					if(alllist.IntegralType==1){//收入
						_this.startval=_this.endval-alllist.IntegralNum;
//						console.log(_this.startval);
  					}else if(alllist.IntegralType==2){//支出
						_this.startval=_this.endval+alllist.IntegralNum;
//						console.log(_this.startval);
  					}
  				}
  				alllist.CreatDate=alllist.CreatDate.substring(0,10);//时间
  				if(alllist.IntegralType==1){//收入列表
  					alllist.IntegralNum='+'+alllist.IntegralNum;
  					alllist.color='green';
  					_this.incomeList.push(alllist);
  				}else if(alllist.IntegralType==2){//支出列表
  					alllist.IntegralNum='-'+alllist.IntegralNum;
  					alllist.color='red';
  					_this.payList.push(alllist);
  				}
  			}
  		})
		},
		getMemberById(){//个人中心首页信息 获取当前积分
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
//			  console.log(data);
			  _this.endval=data[0].LeaveIntegral;
			  _this.getMemberIntegralList();
  		})
  	},
		clickPage(index){
			if(index==0){
				this.allFlag=true;//全部列表
				this.incomeFlag=false;
				this.payFlag=false;
			}else if(index==1){
				this.incomeFlag=true;
				this.allFlag=false;//全部列表
				this.payFlag=false;
			}else if(index==2){
				this.payFlag=true;
				this.incomeFlag=false;
				this.allFlag=false;//全部列表
			}
		}
	},
	mounted(){
	  this.userId=this.$route.query.userId;
    this.getMemberById();//获取当前积分
	}
}
</script>

<style scoped>
.intetop{position: fixed;top: 0px;width: 100%;}
.intetopback{width: 100%;height: 154px;}
.inte_header{width: 100%;height: 110px;text-align:center;line-height:110px;background: url('../../static/credit/inteImg.png') no-repeat;background-size: 100% 100%;}
.demo{font-size: 30px;color: #E83428;}
.tab-swiper{width: 100%;height: 76px;border-bottom:1px solid #ebebeb;font-size: 13px;display: flex;}
.tab-swiper div:nth-child(1){width: 25%;padding-left: 8%;}
.tab-swiper div:nth-child(2){width: 39%;margin-left: 4%;margin-right: 4%;}
.tab-swiper div:nth-child(3){width: 10%;}
.swiper_text{margin-top: 15px;}
.swiper_date{margin-top: 5px;color: #787878;font-size: 12px;}
#green{color:#39a326;}
#red{color: #e83428;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.teammemberintegraldetail .vux-tab-item{font-size: 13px !important;color: #000000 !important;}
.teammemberintegraldetail .vux-tab-selected{color: #E83428 !important;}
</style>
