<template>
  <div class="associatechef">
    <div>
      <tab :line-width=2 active-color='#e83428' v-model="index" custom-bar-width="60px" >
        <tab-item class="vux-center" :selected="demo === item" v-for="(item, index) in list" @click="demo = item" :key="index" @on-item-click="tabClick(index)">{{item}}</tab-item>
      </tab>
      <swiper v-model="index" :height="swiperHeight" :show-dots="false" :min-moving-distance = 100000>
      	
        <swiper-item  :key="0">
			    <div class="search">
			    	<input type="text" placeholder="姓名/酒店" v-model="searchValue0"/>
			    	<img src="../../static/credit/search.png" @click="search0"/>
			    </div>
			    <div class="sort">
			    	<span class="activity" @click="actClick">活跃度<img :src="img1" class="topImg"><img :src="img2" class="botImg"></span>
			    	<span class="integral" @click="inteClick">当前积分<img :src="img3" class="topImg"><img :src="img4" class="botImg"></span>
	        </div>
	        <div class="tab-swiper vux-center" v-for="(item,index) in textlist0" :key="index" @click="lookChefDetail(item.MemberId,item.RecommendId)">
	      	 	  <div>
	      	 	  	<p class="swiper_text">ID:{{item.MemberId}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.MemberName}}</p>
	      	 	  	<p class="swiper_date">{{item.MemberTelePhone}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.HotelName}}</p>
	      	 	    <p class="swiper_date swiper_date_act"><span id='red'>{{item.LeaveIntegral}}分 </span>活跃度：{{item.MemberActiveState}}</p>
	      	 	    <button @click.stop="mymemberClick(item.MemberId)">成为我的</button>
	      	 	  </div>
	        </div>
        </swiper-item>
        
        <swiper-item  :key="1">
			    <div class="search">
			    	<input type="text" placeholder="姓名/酒店" v-model="searchValue1"/>
			    	<img src="../../static/credit/search.png" @click="search1"/>
			    </div>
			    <div class="sort">
			    	<span class="activity" @click="actClick2">活跃度<img :src="img5" class="topImg"><img :src="img6" class="botImg"></span>
			    	<span class="integral" @click="inteClick2">当前积分<img :src="img7" class="topImg"><img :src="img8" class="botImg"></span>
	        </div>
	        <div class="tab-swiper vux-center" v-for="(item,index) in textlist1" :key="index"  @click="lookChefDetail(item.MemberId,item.RecommendId)">
	      	 	  <div>
	      	 	  	<p class="swiper_text">ID:{{item.MemberId}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.MemberName}}</p>
	      	 	  	<p class="swiper_date">{{item.MemberTelePhone}}</p>
	      	 	  </div>
	      	 	  <div>
	      	 	  	<p class="swiper_text">{{item.HotelName}}</p>
	      	 	  	<p v-show="item.RecommendId>99">业务代表: {{item.Name}}</p>
	      	 	    <button v-show="item.RecommendId<=99" @click.stop="mymemberClick(item.MemberId)">成为我的</button>
	      	 	  </div>
	        </div>
        </swiper-item>
        
        <swiper-item :key="2">
			    <div class="search" >
			    	<input type="text" placeholder="姓名/酒店" v-model="searchValue2"/>
			    	<img src="../../static/credit/search.png" @click="search2" />
			    </div>
	        <div class="sortsDiv" >
	        	<span :class="{sortStyle:sortnum==0}" @click="sortClick(0)">排序<img :src="myimg1"></span>
	        	<span :class="{sortStyle:sortnum==1}" @click="sortClick(1)">认证<img :src="myimg2"></span>
	        	<span :class="{sortStyle:sortnum==2}" @click="sortClick(2)">客户编码<img :src="myimg3"></span>
	        </div>
	        <div class="mylist">
	        	<div class="selectDiv1 selectDiv" >
	        		<p @click="selectClick(1)"><span>当前积分</span><img src="../../static/credit/dh.png" class="okImg1"></p>
	        		<p @click="selectClick(2)"><span>活跃度</span><img src="../../static/credit/dh.png" class="okImg2"></p>
	        	</div>
	        	<div class="selectDiv2 selectDiv" >
	        		<p @click="selectClick(3)"><span>实名认证</span><img src="../../static/credit/dh.png" class="okImg3"></p>
	        		<p @click="selectClick(4)"><span>认证码认证</span><img src="../../static/credit/dh.png" class="okImg4"></p>
	        	</div>
	        	<div class="selectDiv3 selectDiv" >
	        		<p @click="selectClick(5)"><span>已完善</span><img src="../../static/credit/dh.png" class="okImg5"></p>
	        		<p @click="selectClick(6)"><span>未完善</span><img src="../../static/credit/dh.png" class="okImg6"></p>
	        	</div>
		        <div class="tab-swiper vux-center" v-for="(item,index) in textlist2" :key="index"  @click="lookChefInfo(item.MemberId)">
		      	 	  <div>
		      	 	  	<p class="swiper_text">ID:{{item.MemberId}}</p>
		      	 	  </div>
		      	 	  <div>
		      	 	  	<p class="swiper_text">{{item.MemberName}}</p>
		      	 	  	<p class="swiper_date">{{item.MemberTelePhone}}</p>
		      	 	  </div>
		      	 	  <div>
		      	 	  	<p class="swiper_text">{{item.HotelName}}</p>
		      	 	    <p class="swiper_date swiper_date_act"><span id='red'>{{item.LeaveIntegral}}分 </span>活跃度：{{item.MemberActiveState}}</p>
		      	 	  </div>
		        </div>
	        </div>
        </swiper-item>
        
      </swiper>
    </div>
    
    <!--提示弹框-->
    <div class="mask" v-show="maskFlag">
	    <div class="mask_box">
	      <img src="../../static/credit/prizex.png" @click="closeClick"/>
	      <p class="box_text">是否已确认过该会员隶属于您负责的区域范围？</p>
	      <p><button @click="closeClick">否</button><button @click="confirmClick">是</button></p>
	    </div>
    </div>
    <div class="mask1" v-show="maskFlag1" ></div><!--我的 遮罩层-->
    
    <!--第一次进入该页面 进行指引-->
    <div class="mask" v-show="guideFlag">
    	
    	<div class="box1" v-show="boxFlag1">
    		<img src="../../static/credit/authen_sm.png" style="width: 30%;" />
    		<div class="text_a">
		    	<p>此处包含本区通过平台实名认证的真实厨师，赶紧电话联系，确认关系，点击【成为我的】转化成商机吧！</p>
	    	</div>
	    	<img src="../../static/credit/authen_jt.png" style="width:60%;display: none;position: absolute;top: 188px;right: 3%;" />
    	</div>
    	<div class="box2" v-show="boxFlag2">
    		<img src="../../static/credit/authen_bq.png" style="width: 30%;margin-left: 35%;" />
    		<div>
		    	<p>存在同一区域会由多个餐饮队员同时负责的情况，故此版块你可以查询本区所有会员信息哦！</p>
	    	</div>
    	</div>
    	<div class="box3" v-show="boxFlag3">
    		<img src="../../static/credit/authen_my.png" style="width: 30%;margin-left: 65%;" />
    		<div>
		    	<p>此板块包括和您确认服务关系的会员，快来填写每个会员的酒店客户编码，赚工资啦！</p>
	    	</div>
    	</div>
    	<p style="text-align: center;"><img src="../../static/credit/authen_btn.png" @click="knowBtn" style="width: 40%;margin-top: 30%;"/></p>
    	
    </div>
    
  </div>
</template>

<script>
import { Tab, TabItem, Swiper, SwiperItem, Search,cookie ,Loading} from 'vux'
import apiUrl from '../apiUrls.js'
var actnum=1;
var intenum=0;
var actnum2=1;
var intenum2=0;
var actnum3=1;
var intenum3=0;
export default{
	components:{
		Tab, 
		TabItem,
		Swiper,
		SwiperItem,
		Search,
		cookie,
		Loading
	},
	data(){
		return{
			knownum:1,//点击知道了的次数
			guideFlag:false,//指引提示 遮罩层
			boxFlag1:true,
			boxFlag2:false,
			boxFlag3:false,
			maskFlag1:false,//我的部分 遮罩层
			sortnum:0,//我的列表 默认选中排序
			myimg1:'../../static/credit/botr.png',//我的列表 箭头图片
			myimg2:'../../static/credit/botc.png',
			myimg3:'../../static/credit/botc.png',
			selectText1:'当前积分',//默认 显示排序字段
			selectText2:'活跃度',//默认 显示排序字段
			selectnum:0,//选中次数 0为初始值    1为选中 滑入     2为滑出
			selectnum1:0,//选中次数 0为初始值    1为选中 滑入     2为滑出
			selectnum2:0,//选中次数 0为初始值    1为选中 滑入     2为滑出
			maskFlag:false,//弹框 默认隐藏
			areaId:0,
			swiperHeight:'100000px',
			img1:'',
    	img2:'',
      img3:'',
    	img4:'',
    	img5:'',
    	img6:'',
      img7:'',
    	img8:'',
			userData:{},
			userId:'',//队员id
			memberId:'',//会员id
			list: ['实名认证会员', '本区全部会员', '我的'],
			index: 0,
			demo: '实名认证会员',
			doStart:true,
			startval:100,
			endval:200,
			textlist0:[],
			textlist1:[],
			textlist2:[],
			searchValue0: '' ,// 搜索框值
			searchValue1: '' ,// 搜索框值
			searchValue2: '' , // 搜索框值
		}
	},
	methods:{
			knowBtn(){//点击知道了
				if(this.knownum==1){//进入指引1页面
					this.knownum++;
					//进入指引二
					this.boxFlag2=true;
					this.boxFlag1=false;
					this.boxFlag3=false;
				}else if(this.knownum==2){//进入指引1页面
					this.knownum++;
					//进入指引三
					this.boxFlag3=true;
					this.boxFlag1=false;
					this.boxFlag2=false;
				}else if(this.knownum==3){//进入指引1页面
					//关闭遮罩层
					this.guideFlag=false;
				}
			},
			tabClick(val){//点击 头部切换
				this.maskFlag1=false;
        $(".associatechef").unbind('touchmove');//解除绑定
				if(val==2){
					if(this.selectnum==1||this.selectnum1==1||this.selectnum2==1){//如果状态为下拉状态 需要显示蒙版
						this.maskFlag1=true;
						$('.associatechef').bind("touchmove",function(e){  //无法滑动屏幕
			          e.preventDefault();  
			      });
					}
				}
			},
			lookChefDetail(memberId,recommendId){//点击含有成为我的按钮的部分 可查看厨师详情
				if(recommendId<=99){//大于99 为已认证会员 队员不能查看该会员信息
        	this.$router.push({path:'/component/chefdetail',query:{memberId:memberId}})
        }
			},
		  lookChefInfo(memberId){//查看队员自己名下的会员 厨师主页
		  	this.$router.push({path:'/component/chefhomepage',query:{memberId:memberId}})
		  },
	    search0(){//搜索
	    	if(this.searchValue0!=''){
	    		this.$router.push({path:'/component/teamsearch',query:{index:0,searchValue:this.searchValue0,areaId:this.areaId}})
	    	}
	    },
	    search1(){
	    	if(this.searchValue1!=''){
	    		this.$router.push({path:'/component/teamsearch',query:{index:1,searchValue:this.searchValue1,areaId:this.areaId}})
	    	}
	    },
	    search2(){
	    	if(this.searchValue2!=''){
	    		this.$router.push({path:'/component/teamsearch',query:{index:2,searchValue:this.searchValue2,areaId:this.areaId}})
	    	}
	    },
      getMemberBySaleman(index,type,num){//查询绑定厨师
      	var _this=this;
      	if(type=="ActiveOB"){//活跃度
      		var params={
			      "IndexPage":1,
			      "PageSize":999,
            "SalemanId":this.userId,
            "MemberState":index,
            "MemberName":"",
            "ActiveOB":num
	  		  }
      	}else if(type=="IntegralOB"){
      		var params={
			      "IndexPage":1,
			      "PageSize":999,
            "SalemanId":this.userId,
            "MemberState":index,
            "MemberName":"",
            "IntegralOB":num
	  		  }
      	}
      	
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getMemberBySaleman,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			var count=data.Count;
	  			var dataList=data.data;
				  for(var i in dataList){
	  				var datalist=dataList[i];
	  				if(datalist.MemberActiveState==1){
	  					datalist.MemberActiveState='高';
	  				}else if(datalist.MemberActiveState==2){
	  					datalist.MemberActiveState='低';
	  				}
	  			}
	  		})
      },
      changeImg(){
	  		this.img1='../../static/credit/topc.png';
	    	this.img2='../../static/credit/botc.png';
	      this.img3='../../static/credit/topc.png';
	    	this.img4='../../static/credit/botc.png';
	  	},
	  	changeImg2(){
	  		this.img5='../../static/credit/topc.png';
	    	this.img6='../../static/credit/botc.png';
	      this.img7='../../static/credit/topc.png';
	    	this.img8='../../static/credit/botc.png';
	  	},
	  	actClick(){//实名认证会员  活跃度
	  		if(actnum==1){
	  			actnum=2;
	  			intenum=0;
	  			this.changeImg();
	  		  this.img2='../../static/credit/botr.png';
	  		  var ActiveOB=2;//活跃度 倒序
	  		}else if(actnum==2||actnum==0){
	  			actnum=1;
	  			intenum=0;
	  			this.changeImg();
	  		  this.img1='../../static/credit/topr.png';
	  		  var ActiveOB=1;//活跃度 正序
	  		}
	  		this.getMemberByArea(0,2,ActiveOB,0,0,0);//获取厨师信息 实名认证
	  	},
	  	inteClick(){//实名认证会员 当前积分
	  		if(intenum==1){
	  			intenum=2;
	  			actnum=0;
	  			this.changeImg();
	  		  this.img4='../../static/credit/botr.png';
	  		  var IntegralOB=2;//当前积分 倒序
	  		}else if(intenum==2||intenum==0){
	  			intenum=1;
	  			actnum=0;
	  			this.changeImg();
	  		  this.img3='../../static/credit/topr.png';
	  		  var IntegralOB=1;//当前积分 正序
	  		}
	  		this.getMemberByArea(0,2,0,IntegralOB,0,0);//获取厨师信息 实名认证
	  	},
	  	actClick2(){//本区全部会员 活跃度
	  		if(actnum2==1){
	  			actnum2=2;
	  			intenum2=0;
	  			this.changeImg2();
	  		  this.img6='../../static/credit/botr.png';
	  		  var ActiveOB=2;//活跃度 倒序
	  		}else if(actnum2==2||actnum2==0){
	  			actnum2=1;
	  			intenum2=0;
	  			this.changeImg2();
	  		  this.img5='../../static/credit/topr.png';
	  		  var ActiveOB=1;//活跃度 正序
	  		}
	  		this.getMemberByArea(1,0,ActiveOB,0,0,0);//获取厨师信息 实名认证
	  	},
	  	inteClick2(){//本区全部会员 当前积分
	  		if(intenum2==1){
	  			intenum2=2;
	  			actnum2=0;
	  			this.changeImg2();
	  		  this.img8='../../static/credit/botr.png';
	  		  var IntegralOB=2;//当前积分 倒序
	  		}else if(intenum2==2||intenum2==0){
	  			intenum2=1;
	  			actnum2=0;
	  			this.changeImg2();
	  		  this.img7='../../static/credit/topr.png';
	  		  var IntegralOB=1;//当前积分 正序
	  		}
	  		this.getMemberByArea(1,0,0,IntegralOB,0,0);//获取厨师信息 实名认证
	  	},
	  	getSaleManInfo(){//获取队员信息 区id
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
	  			_this.areaId=data.Area[0].AreaId;
	  			_this.getMemberByArea(0,2,1,0,0,0);//获取厨师信息 实名认证
	  			_this.getMemberByArea(1,0,1,0,0,0);//获取厨师信息 本区全部会员
	  			_this.getMemberByArea(2,0,2,0,0,0);//获取厨师信息 我的
	  			$('.okImg2').show();//默认展示活跃度排序
	  		})
	  	},
	  	getMemberByArea(index,SearchType,ActiveOB,IntegralOB,AuthType,MemberCode){//获取厨师信息
	  		var _this=this;
	  		if(index==0||index==1){
	  			var areaId=this.areaId;
	  			var userId=0;
	  		}else{
	  			var areaId=0;
	  			var userId=this.userId;
	  		}
	  		var params={
		  		'AreaId':areaId,
					'SalemanId':userId,
					'SearchType':SearchType,     //2是第一部分
					'ActiveOB': ActiveOB,    //1正序 2倒序
					'IntegralOB':IntegralOB,       //1 2
					'AuthType':AuthType,        //1实名认证 2注册码认证
					'MemberCode':MemberCode,       // 1已完善  2未完善
					'Name':''        //关键字搜索
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getMemberByArea,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			for(let i in data){
	  				if(data[i].MemberActiveState==1){//活跃度高
	  					data[i].MemberActiveState='高';
	  				}else{
	  					data[i].MemberActiveState='低';
	  				}
	  			}
	  			if(index==0){
	  				_this.textlist0=data;
	  				if(_this.textlist0.length!=0){
	  					$('.box1 img').eq(1).show();
	  				}
	  			}else if(index==1){
	  				_this.textlist1=data;
	  			}else if(index==2){
	  				_this.textlist2=data;
	  			}
	  		})
	  	},
	  	bindMember(){//成为我的 成为该队员名下会员
	  		var _this=this;
	  		var params={
  		    'MemberId':this.memberId.toString(),
					'SalemanId':this.userId.toString()
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.bindMember,
	  			data:params
	  		}).then(function(response){
	  			if(response.data=="Exc Success"){
	  				_this.$vux.loading.hide();//loading图标消失
		  			_this.maskFlag=false;
		  			_this.getMemberByArea(0,2,1,0,0,0);//获取厨师信息 实名认证
		  			_this.getMemberByArea(1,0,1,0,0,0);//获取厨师信息 本区全部会员
		  			_this.getMemberByArea(2,0,2,0,0,0);//获取厨师信息 我的
	  			}
	  		})
	  	},
	  	mymemberClick(memberId){//成为我的
	  		//弹出提示框
	  		this.maskFlag=true;
	  		this.memberId=memberId;
	  	},
	  	closeClick(){//弹框 点击否或x号
	  		this.maskFlag=false;
	  	},
	  	confirmClick(){//点击 是 调成为我的接口
	  		this.showLoading();//loading
	  		this.bindMember();//成为我的 成为该队员名下会员
	  	},
	  	sortClick(index){//点击我的列表  选择-排序认证编码
          $(".selectDiv").hide();//滑出 
 
          if(index==0){//点击排序
         	  if(this.selectnum==0){//滑入
              this.sortnum=index;//选中文字变红色
              this.myimg1='../../static/credit/botr.png';
              this.myimg2='../../static/credit/botc.png';
              this.myimg3='../../static/credit/botc.png';
         	  	this.maskFlag1=true;//遮罩层显示
	         	  $('.selectDiv1').slideDown(200);//滑入
	         	  this.selectnum++;
	         	  this.selectnum1=0;
	         	  this.selectnum2=0;
	         	  $('.associatechef').bind("touchmove",function(e){  //无法滑动屏幕
			          e.preventDefault();  
			        }); 
         	  }else if(this.selectnum==1){ //滑出 
         	  	this.maskFlag1=false;
         	  	this.selectnum=0;
          		$(".selectDiv1").slideUp(200);//滑出 
          		$(".associatechef").unbind('touchmove');//解除绑定
         	  }
          }else if(index==1){//点击认证
         	  if(this.selectnum1==0){//滑入
              this.sortnum=index;//选中文字变红色
              this.myimg1='../../static/credit/botc.png';
              this.myimg2='../../static/credit/botr.png';
              this.myimg3='../../static/credit/botc.png';
         	  	this.maskFlag1=true;//遮罩层显示
	         	  $('.selectDiv2').slideDown(200);//滑入
	         	  this.selectnum1++;
	         	  this.selectnum=0;
	         	  this.selectnum2=0;
	         	  $('.associatechef').bind("touchmove",function(e){  //无法滑动屏幕
			          e.preventDefault();  
			        }); 
         	  }else if(this.selectnum1==1){ //滑出 
         	  	this.maskFlag1=false;
         	  	this.selectnum1=0;
          		$(".selectDiv2").slideUp(200);//滑出 
          		$(".associatechef").unbind('touchmove');//解除绑定
         	  }
          }else if(index==2){//点击认证
         	  if(this.selectnum2==0){//滑入
              this.sortnum=index;//选中文字变红色
              this.myimg1='../../static/credit/botc.png';
              this.myimg2='../../static/credit/botc.png';
              this.myimg3='../../static/credit/botr.png';
         	  	this.maskFlag1=true;//遮罩层显示
	         	  $('.selectDiv3').slideDown(200);//滑入
	         	  this.selectnum2++;
	         	  this.selectnum=0;
	         	  this.selectnum1=0;
	         	  $('.associatechef').bind("touchmove",function(e){  //无法滑动屏幕
			          e.preventDefault();  
			        }); 
         	  }else if(this.selectnum2==1){ //滑出 
         	  	this.maskFlag1=false;
         	  	this.selectnum2=0;
          		$(".selectDiv3").slideUp(200);//滑出 
         			$(".associatechef").unbind('touchmove');//解除绑定
         	  }
          }
	  	},
	  	selectClick(val){//点击排序分类
	  		$('.selectDiv img').hide();//隐藏对号
        $(".associatechef").unbind('touchmove');//解除绑定
	  		if(val==1){
	  			this.selectnum=0;
	  			this.getMemberByArea(2,0,0,2,0,0);//获取厨师信息 我的 当前积分从高到低排序
	  			$('.okImg1').css('display','block');
	  		}else if(val==2){
	  			this.selectnum=0;
	  			this.getMemberByArea(2,0,2,0,0,0);//获取厨师信息 我的 活跃度从高到低排序
	  			$('.okImg2').css('display','block');
	  		}else if(val==3){
	  			this.selectnum1=0;
	  			this.getMemberByArea(2,0,0,0,1,0);//获取厨师信息 我的 实名认证1
	  			$('.okImg3').css('display','block');
	  		}else if(val==4){
	  			this.selectnum1=0;
	  			this.getMemberByArea(2,0,0,0,2,0);//获取厨师信息 我的 注册码认证2
	  			$('.okImg4').css('display','block');
	  		}else if(val==5){
	  			this.selectnum2=0;
	  			this.getMemberByArea(2,0,0,0,0,1);//获取厨师信息 我的 已完善1
	  			$('.okImg5').css('display','block');
	  		}else if(val==6){
	  			this.selectnum2=0;
	  			this.getMemberByArea(2,0,0,0,0,2);//获取厨师信息 我的 未完善2
	  			$('.okImg6').css('display','block');
	  		}
	  		
	  		this.maskFlag1=false;//遮罩层隐藏
	  		$(".selectDiv").slideUp(200);//滑出效果
	  	},
	  	showLoading () {//loading 图标
	      this.$vux.loading.show({
	        text: 'Loading'
	      })
	    }
	},
	mounted(){
//		判断是否显示指引蒙版
    var storage=window.localStorage;
    var guide=storage.getItem("guide");//获取storage
    if(guide==null){//没有记录
    	//第一次显示指引层
    	this.guideFlag=true;
    	storage.setItem("guide",'yes');//设置
    }else{
    	//不显示指引层
    	this.guideFlag=false;
    }
    //从cookie获取用户信息
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
	  this.getSaleManInfo();//获取队员信息 区id
	  
	  this.changeImg();
	  this.img1='../../static/credit/topr.png';
	  this.changeImg2();
	  this.img5='../../static/credit/topr.png';
	  
	  $('.mask1').height(window.screen.height-185);//我的部分 点击排序 遮罩层高度
	}
}
</script>

<style scoped>
.inte_header{width: 100%;height: 110px;text-align:center;line-height:110px;background: url('../../static/credit/inteImg.png') no-repeat;background-size: 100% 100%;}
.demo{font-size: 30px;color: #E83428;}
.tab-swiper{width: 100%;border-bottom:1px solid #ebebeb;font-size: 13px;display: flex;padding-bottom: 10px;}
.tab-swiper button{width: 50%;height:20px;margin-top: 10px;color: #fff;background: #E83428;border-radius: 5px;border: none;font-size: 11px;}
.tab-swiper div:nth-child(1){width: 20%; padding-left: 5%; }
.tab-swiper div:nth-child(2){width: 35%; margin-left: 8%;  }
.tab-swiper div:nth-child(3){width: 45%;text-align: right;padding-right: 5%;}
.swiper_text{margin-top: 15px;}
.swiper_date{margin-top: 5px;color: #787878;font-size: 12px;}
.swiper_date_act{color:#1c1c1c ;}
#red{color: #e83428;margin-right: 5px;}
.sort span{color: #1c1c1c;font-size: 13px;}
.search{width: 90%;height: 30px;background: #f8f8f8;margin-top: 10px;border-radius: 5px;margin-left: 5%;}
.search *{display: inline-block;vertical-align: middle;}
.search img{width: 7%;}
.search input{width: 90%;border: none;height: 100%;background:#f8f8f8;outline: none;text-indent: 10px;}
.topImg{position: relative;top: -6px;left: 5px;}
.botImg{ position: relative;left: -4px;top: 1px;}
.sort img{width: 9px;}
.sortsDiv img{width: 9px;margin-left: 5px;position: relative;top: -1px;}
.sortsDiv{display: flex;justify-content: space-around;border-bottom: 1px solid #f1f1f1;}
.sortsDiv span{display: block;width:20%;text-align: center;font-size: 12px;color: #3E3E3E;height: 40px;line-height: 40px;}
.sortStyle{color: #e83428 !important;}
.selectDiv{position: absolute;font-size: 12px;color: #3E3E3E;background: #fff;width:100%;display: none;}
.selectDiv p{line-height: 30px;}
.selectDiv p span{margin-left: 10%;}
.selectDiv img{width: 16px;float: right;margin-right: 12%;display: none;}
.selectDiv img:nth-child(1){position: relative;top: 10px;}
.selectDiv img:nth-child(2){position: relative;top: 10px;}

/*弹框*/
.mask{width: 100%;height: 100%;background: rgba(0,0,0,0.7);position: absolute;top: 0;z-index: 1000;}
.mask1{width: 100%;height:72%;background: rgba(0,0,0,0.5);position: absolute;top:185px;z-index: 1000;}
.mask .mask_box{width: 80%;background: #fff;border-radius: 5px;margin-top: 20%;margin-left: 10%;border: 1px solid #fff;}
.mask_box img{width: 20px;float: right;margin-right: 10px;margin-top: 1%;}
.mask_box p{padding: 0 10%;font-size: 14px;}
.mask_box button{width: 28%;height:24px;border: none;border-radius: 5px;margin-top: 10%;margin-bottom: 20px;}
.mask_box button:nth-child(1){background: #f4f4f4;color: #3E3E3E;}
.mask_box button:nth-child(2){background: #E83428;color: #fff;margin-left: 43%;}
.box_text{margin-top: 10%;}

/*指引层*/
.box1 div,.box2 div,.box3 div{padding:20px 10%;font-size: 14px;color: #fff;}

</style>
<style>
#vux_view_box_body{background: #fff;}
.associatechef .vux-tab-item{font-size: 13px !important;color: #000000 !important;}
.associatechef .vux-tab-selected{color: #E83428 !important;}
.associatechef .weui-search-bar { background-color: #fff !important; }
.associatechef .weui-search-bar:before { border-top: 1px  solid #fff; color: #fff; }
.associatechef .weui-search-bar:after { border-bottom: 1px solid #fff; color: #fff; } 
.associatechef .weui-search-bar__form,
.associatechef .weui-search-bar__box,
.associatechef .weui-search-bar__label { background:#f8f8f8 !important;border-radius: 5px;text-align: left;}
.associatechef .sort { height: 40px; text-align: center;border-bottom: 1px solid #f1f1f1;box-sizing: border-box;}
.associatechef .sort span { display: inline-block; height: 40px; line-height: 40px; width: 120px; }
.associatechef .sort .activity { margin-right: 30px; }
.associatechef .sort .integral {  }
.associatechef .weui-search-bar__cancel-btn{color: #E83428;}
</style>
