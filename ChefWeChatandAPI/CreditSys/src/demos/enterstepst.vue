<template>
  <div class="enterstepst">
    <div style="width:91%;margin: 0 auto;">
    	  <br/>
	      <step v-model="step1" background-color='#fbf9fe'>
	        <step-item></step-item>
	        <step-item></step-item>
	        <step-item></step-item>
	      </step>	      
	      <div class="steptext">
	      	<span>基本信息</span><span>创新菜</span><span>本地菜</span>	             
	      </div>
	     <!--基本信息-->
      	<group>
          <x-input title="赛区选择" @on-focus="alertZone" v-model="areacity1"></x-input>            
	      </group>
	      <group gutter='0.4rem'>
	      	<popup-picker title="报名途径" :data="road"  v-model="roadText" cancel-text='取消' confirm-text='确定' value-text-align	='left' @on-hide='recom'></popup-picker>
	      </group>
	      <group gutter='0.4rem' v-show='flag'>
	        <x-input title="推荐人" v-model="refer"></x-input>            
	      </group>
	      <group gutter='0.4rem'>      	
	      	<datetime title="出生日期" v-model="timeText"  :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align	='left'></datetime>
	      </group>	      
	      <div class="photoshow">
	      	<div><span>+个人照片</span></div>
	      	<div><span>+酒店照片</span></div>
	      </div>
	      <div class="btnDiv">
	      	<button @click="save">保存</button><button @click="nextStep">下一步</button>	        
	      </div>       
    </div>
    <!--赛区选择-->
    <div class="mask" v-show='flag1'>
    	<div class='zone'>
    		<input class="search" placeholder="输入所选择的赛区"/>
    		<div class="searchDiv">    			
    			<div class="cityDiv" v-for="(item,index) in areaList" :key="index">
    				<span>{{item.ProvinceName}}</span>
    				<checker v-model="areacity" default-item-class="demo5-item" selected-item-class="demo5-item-selected">
              <checker-item v-for="(i,index) in item.AreaName" :key="index" :value="i.AreaName" @click.native="MatchRegionId(i.MatchRegionId)">{{i.AreaName}}</checker-item>
            </checker>
    			</div>    		  
    		</div>
    		<div class="areaBtn"><button @click="cancel">取消</button><button @click="confirm">确定</button></div>    		
    	</div>
    </div>    
    <!--信息不完整提示-->
    <div class="mask1" v-show='flag2'>
    	<div class="menu">
    	   <p>填写完整才可以进入到下一步喔！点击【保存】按钮可以随时保存进度</p>
    	   <button @click="menuBtn">知道了</button>
    	</div>
    </div>
    <!--保存成功页面-->
    <div class="saveDiv" v-show='saveFlag'>
    	<img src="../../static/credit/success.png" />
    	<p>保存成功</p>
    	<p>再次进入点击报名可继续填写</p>
    	<button @click="closeSave">关闭</button>
    </div>
  </div>
</template>

<script>
import {Step, StepItem,Group,XInput,PopupPicker, Checker, CheckerItem,Datetime} from 'vux'
import apiUrl from "../apiUrls.js"

export default {
  components: {
    Step, 
    StepItem, 
    Group,
    XInput,    
    PopupPicker,
    Checker,
    CheckerItem,
    Datetime
  },
  data() {
    return{
    	step1: 0,
    	roadText:[],   	//报名途径
    	road:[['自我推荐','行业大师推荐','酒店推荐','欣和业务推荐']],
    	timeText:'',//出生日期
    	flag:false,//显示推荐人
    	flag1:false,//显示赛区
    	flag2:false,//显示信息不完整提示
    	saveFlag:false,//显示保存成功页面
    	areaList:[],
    	areacity:'',
    	areacity1:'',//赛区
    	matchId:'',//赛区id
    	refer:'',//推荐人
    	MemberId:1,//传参
      ChefActivityId:1,//传参
      ChefPicUrl:'',//个人照片
      ChefHotelPicUrl:''//酒店照片
//  	url:'http://192.168.8.35:8003/api/Chef/GetChefByMBIdCAId'//获取厨师基本信息
    }
  },
  methods: {
  	getArea(){//获取赛区API
  		var _this=this;
  		this.$http({
  			method:"GET",
  			url:apiUrl.getMatch
  		}).then(function(response){
  			_this.areaList=JSON.parse(response.data);
  		})
  	},
  	addChef(){//新增厨师基本信息API
  		var params={
  			"MemberId":this.MemberId,
  			"ChefActivityId":this.ChefActivityId,
  			"MatchRegionId":this.matchId,
  			"ApplyType":this.roadText[0],
  			"ReferrerName":this.refer,
  			"Birthday":this.timeText,
  			"ChefPicUrl":this.ChefPicUrl,
  			"ChefHotelPicUrl":this.ChefHotelPicUrl
  		}
  		this.$http({
  			method:"POST",
  			url:apiUrl.addChef,
  			data:params
  		}).then(function(response){
  			console.log(response.data);
  		})
  	},
  	getChef(){//获取厨师基本信息API
  		var _this=this;
  		this.$http({
  			method:"GET",
  			url:apiUrl.getChefMess+'?mbId='+1+'&caId='+1
  		}).then(function(response){
//			console.log(JSON.parse(response.data)[0]);
  			var res=JSON.parse(response.data)[0];
  			_this.areacity1=res.AreaName;//赛区
  			_this.areacity=res.AreaName;
  			_this.roadText.push(res.ApplyType);//报名途径
  			_this.timeText=res.Birthday;//出生日期
  			_this.matchId=res.MatchRegionId;//赛区id  			
  			_this.MemberId=res.MemberId;//参数
  			_this.ChefActivityId=res.ChefActivityId;//参数
  			_this.ChefPicUrl=res.ChefPicUrl;//个人照片
  			_this.ChefHotelPicUrl=res.ChefHotelPicUrl;//酒店照片
  			if(_this.roadText[0]!='自我推荐'&&_this.roadText[0]!=''){
  				_this.flag=true;
  				_this.refer=res.ReferrerName;//推荐人
  			} 			
  		})
  	},
  	MatchRegionId(id){//获取赛区id
  		this.matchId=id;
  	},
  	alertZone(){
  		this.flag1=true;
  	},   
    recom(){
    	if(this.roadText!='自我推荐'&&this.roadText!=''){
    		this.flag=true;
    	}else{
    		this.flag=false;
    	}
    },
    nextStep () {//下一步
    	if(this.areacity1==''||this.roadText==''||(this.flag==true&&this.refer=='')||this.timeText==''){//内容为空弹框
    		this.flag2=true;
    	}else{
    		this.addChef();
    		this.$router.push({path:'/component/enterstepnd'});
    	}      
    },
    menuBtn(){
    	this.flag2=false;
    },
    cancel(){
    	this.flag1=false;
    },
    confirm(){
    	this.flag1=false;
    	this.areacity1=this.areacity;
    },
    save(){//点击保存
    	this.addChef();//调用新增信息接口
    	this.saveFlag=true;
    },
    closeSave(){//关闭保存页面
    	this.saveFlag=false;
    }
  },
  mounted(){
    this.getArea();//获取赛区
//  this.getChef();//获取厨师信息
  }
}
</script>
<style scoped>	
	.enterstepst{
		background: #FFFFFF;
	}
	.steptext span{
		display: inline-block;
		text-align: center;
		width: 33.3%;
		font-size: 13px;
		color:#3E3E3E;
	}
	.photoshow {
		margin-top: 40px;
		display: flex;
		text-align: center;
	}
	.photoshow div{
		width: 44%;
		height: 120px;
		border: 1px solid #959595;
		line-height: 180px;
	}
	.photoshow span{
		font-size: 13px;
		color: #888888;
	}
	.photoshow div:nth-child(1){
		margin-right: 10%;
		background: url('../../static/credit/per.png') no-repeat center 15px;
		background-size: 60px;
	}
	.photoshow div:nth-child(2){
		background: url('../../static/credit/hotel01.png') no-repeat center 15px;
		background-size: 60px;
	}
	.btnDiv{
		margin-top: 100px;
		padding-left: 2%;
	}
	.btnDiv button{
		width: 43%;
		height: 40px;
		border: 1px solid #E83428;
		border-radius: 5px;
		background:#FFFFFF;
		color: #E83428;
		outline: none;
	}
	.btnDiv button:nth-child(2){
		margin-left: 10%;
	}
	.mask,.mask1{
		width: 100%;
		height: 100%;
		background: rgba(0,0,0,0.5);
		position: fixed;
		top: 46px;
	}
	.zone{
		width: 100%;
		height: 410px;
		background: #FFFFFF;
	}
	.search{
		width: 270px;
		height:30px;
		background: #f9f9f9 url(../../static/credit/search.png) no-repeat 230px center;
		border: none;
    border-radius: 5px;
    margin-left: 10%;
    margin-top: 20px;
    text-indent: 1em;
    background-size: 20px;
    outline: none;
	}
	.searchDiv{
		width: 100%;
		height: 300px;
		overflow-y:auto;
		border-top: 1px solid #c8c8c8;
		margin-top: 10px;
	}
	.cityDiv{
		width: 80%;
		margin-left: 10%;
		margin-top: 20px;
	}
	.demo5-item {
	  width: 50px;
	  height: 26px;
	  line-height: 26px;
	  text-align: center;
	  color: #3E3E3E;
	  font-size: 13px;
	  margin-left:8%;
	}
	.demo5-item-selected {
		color: #E83428;
	  border-bottom:1px solid #E83428;
	}
	.areaBtn button{
		width: 50%;
		height: 50px;
		font-size: 13px;	
	}
	.areaBtn button:nth-child(1){
	  background: #fff;	
	  color: #E83428;
	  border: 1px solid #E83428;
	  box-sizing: border-box;
	}
	.areaBtn button:nth-child(2){
	  background: #E83428;	
	  color: white;
	  border: none;
	}
	.menu{
		width:60%;
		height: 130px;
		background: #FFFFFF;
		border-radius: 10px;
		margin: 150px auto;
		padding-top: 20px;
	}
	.menu p{
		font-size: 13px;
		color: #3E3E3E;
		width: 80%;
		margin-left: 10%;
	}
	.menu button{
		width: 50%;
		height: 33px;
		background: #fff;
		color: #E83428;
		border: 1px solid #E83428;
		border-radius: 20px;	
		margin-left: 25%;
		margin-top: 18px;
		outline: none;
	}
	.saveDiv{
		width: 100%;
		height: 100%;
		position: fixed;
		top: 46px;
		background: #FFFFFF;
		text-align: center;
	}
	.saveDiv img{
		width: 160px;
		margin-top: 90px;
	}
	.saveDiv p:nth-child(2){
	  font-size: 17px;	
	  color: #E83428;
	  margin-top: 15px;
	}
	.saveDiv p:nth-child(3){
	  font-size: 13px;	
	  color: #707070;
	  margin-top: 10px;
	}
	.saveDiv button{
		width: 90%;
		height: 40px;
		color: white;
		background: #E83428;
		border: none;
		border-radius: 5px;
		margin-top: 20%;
	}
</style>
<style>
  .enterstepst .vux-step{width: 76%;margin-left: 12%;}
  .enterstepst .weui-cell{font-size: 13px;color: #3e3e3e;}
  .enterstepst .weui-cells:before{border-top: none !important;}
  .enterstepst .weui-cells:after{border-bottom: none !important;}
  .enterstepst .weui-cells { border: 1px solid #dddddd; border-radius: 0.5rem; }
  .enterstepst .weui-label { width: 4.5rem !important; }
  .vux-popup-picker-header-menu-right { color: #e83428 !important; }
  .enterstepst .vux-cell-box:before{border-top: none !important;}
  .enterstepst .vux-datetime p{width: 4.5rem;}
  .enterstepst .weui-cell__ft{color: #3E3E3E !important;}
  .dp-right {color: #e83428 !important;}
  #vux_view_box_body{background: #FFFFFF !important;}
  
  .vux-step-item-head-process .vux-step-item-head-inner{border:none !important;background: #E83428 !important;}
  .vux-step-item-head-wait .vux-step-item-head-inner{border:none !important;background: #bcbcbc !important;}
  .vux-step-item-tail-finish{background:#e83428 !important;}
  .vux-step-item-head-finish .vux-step-item-head-inner{border: none !important;background: #E83428 !important;}
  .weui-icon-success-no-circle{color:white !important;}
</style>