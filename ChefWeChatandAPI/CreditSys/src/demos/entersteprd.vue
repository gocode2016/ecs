<template>
  <div class="entersteprd">
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
  	</div>
    <div class="upload">十&nbsp;菜品照片</div>
    <input type="text" placeholder="请填写菜品名字" class="dishname" v-model="dishName"/>
    <!--菜品故事-->
    <div class="alike show">
		 	 <img src="../../static/credit/book1.png" />
		 	 <span>菜品故事</span>
	  </div>
	  <group>
      <x-textarea  placeholder="说说这道菜背后的故事吧~"></x-textarea>
    </group>
    <!--备料展示-->
     <div class="alike show">
     	 <img src="../../static/credit/show.png" />
     	 <span>备料展示</span>
     </div>    
     <!--主料-->
     <p class="zliao">主料</p>
     <ul class="main">
     	 <li>
     	 	 <input class="maintext" type="text" placeholder="主料：如牛肉"><input class="weight" type="text"><span class="ke">g&nbsp;</span><img src="../../static/credit/jian.png" class="rem"/>    	 	     	 	
     	 </li>    	
     </ul> 
     <p class="add"><img src="../../static/credit/jia.png" @click="add1"/></p>
     <!--辅料-->
     <p class="zliao">辅料</p>
     <ul class="access">
     	 <li>
     	 	 <input class="maintext" type="text" placeholder="食材：如百合"><input class="weight" type="text"><span class="ke">g&nbsp;</span><img src="../../static/credit/jian.png" class="rem"/>     	 	     	 	
     	 </li>     	 
     </ul>
     <p class="add"><img src="../../static/credit/jia.png" @click="add2"/></p>
      <!--调料-->
     <p class="zliao">调料</p>
     <p class="seasonText">(以下调味品中至少选择一样作为调料)</p>     
      <checker v-model="demo6" type="checkbox" default-item-class="demo5-item" selected-item-class="demo5-item-selected">
        <checker-item v-for="(item,index) in list" :key="index" :value="index" @on-item-click="onItemClick(item.text,index)"><img v-bind:src="item.url"><p>{{item.text}}</p></checker-item>        
      </checker>
     <ul class="season"></ul>
     <p class="seasonText">(还需要其他调料吗？请写在下面)</p>
     <ul class="othersea">
       <li>
     	 	 <input class="maintext" type="text" placeholder="味达美冰糖老抽"><input class="weight" type="text"><span>g&nbsp;</span><img src="../../static/credit/jian.png" class="rem"/>    	 	     	 	
     	 </li>
     </ul>
     <p class="add"><img src="../../static/credit/jia.png" @click="add3"/></p>
     <!--烹饪步骤-->
     <div class="alike show">
	     	 <img src="../../static/credit/cook.png" />
	     	 <span>烹饪步骤</span>
	   </div> 
	   <ul class='cookStep'>
	   	 <li><span>1、</span><textarea placeholder="添加步骤说明"></textarea><img src="../../static/credit/jian.png" class="rem"/></li>
	   </ul>
	   <p class="add"><img src="../../static/credit/jia.png" @click="add4"/></p>
	   <!--按钮-->
	   <div class="btnDiv"><button @click="save">保存</button><button @click="nextStep">提交</button></div>
	    <!--信息不完整提示-->
    <div class="mask1" v-show='flag2'>
    	<div class="menu">
    	   <p>填写完整才可以提交喔！点击【保存】按钮可以随时保存进度</p>
    	   <button @click="menuBtn">知道了</button>
    	</div>
    </div>
    <!--提交成功页面-->
    <div class="sucDiv" v-show='flag3'>
    	<img src="../../static/credit/success.png" />
    	<p>恭喜你！提交成功</p>
    	<p>24小时内将告知审核结果</p>
    	<button @click="closeBtn">关闭</button>
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
import {Step, StepItem, XTextarea, Group,Checker, CheckerItem} from 'vux'

var flag=[0,0,0,0,0,0];//未被选中时状态
export default {
  components: {
  	Step, 
  	StepItem,
    XTextarea, 
    Group,
    Checker, 
    CheckerItem
  },
  data () {
    return{
    	step1: 2,
    	demo6: [],
    	flag2:false,
    	flag3:false,
    	saveFlag:false,
    	dishName:'',//菜名
    	list:[{    	
    		url:'../../static/credit/jy.png',
    		text:'味达美冰糖老抽'
    	},{    	
    		url:'../../static/credit/jy.png',
    		text:'味达美冰糖老抽'
    	},{
    		url:'../../static/credit/jy.png',
    		text:'味达美冰糖老抽'
    	},{ 
    		url:'../../static/credit/jy.png',
    		text:'味达美冰糖老抽'
    	},{
    		url:'../../static/credit/jy.png',
    		text:'味达美冰糖老抽'
    	},{
    		url:'../../static/credit/jy.png',
    		text:'味达美冰糖老抽'
    	}]
    }
  },
  methods:{
    add1(){
    		var li=$('<li data-v-b401235a><input  data-v-b401235a class="maintext" type="text"><input  data-v-b401235a class="weight" type="text"><span  data-v-b401235a class="ke">g&nbsp;</span><img  data-v-b401235a src="../../static/credit/jian.png" class="rem"/></li>');  
    	  $('.main').append(li);	
    },
    add2(){
    		var li=$('<li  data-v-b401235a><input  data-v-b401235a class="maintext" type="text"><input  data-v-b401235a class="weight" type="text"><span  data-v-b401235a class="ke">g&nbsp;</span><img  data-v-b401235a src="../../static/credit/jian.png" class="rem"/></li>');  
    	  $('.access').append(li);	
    },
    onItemClick(text,value){    
    	if(flag[value]==0){
      	var li=$('<li data-v-b401235a class='+value+'><input data-v-b401235a class="maintext" type="text" value='+text+'  readonly="readonly"><input data-v-b401235a class="weight" type="text"><span data-v-b401235a>g&nbsp;</span><img data-v-b401235a src="../../static/credit/jian.png" class="rem"/></li>');
        $('.season').append(li);
        flag[value]=1;
      }else{
      	$('.'+value+'').remove();
      	flag[value]=0;
      }
    },
    add3(){
    		var li=$('<li  data-v-b401235a><input  data-v-b401235a class="maintext" type="text"><input  data-v-b401235a class="weight" type="text"><span  data-v-b401235a>g&nbsp;</span><img  data-v-b401235a src="../../static/credit/jian.png" class="rem"/></li>');  
    	  $('.othersea').append(li);	
    },
    add4(){
    	var length=$('.cookStep li').length+1;
    	var li=$('<li  data-v-b401235a><span >'+length+'、</span><textarea  data-v-b401235a placeholder="添加步骤说明"></textarea><img  data-v-b401235a src="../../static/credit/jian.png" class="rem"/></li>')
    	$('.cookStep').append(li);	
    },
    nextStep(){   	
    	if(this.dishName==''){//弹出提示框
    		this.flag2=true;
    	}else{//进入提交成功页面；
    		this.step1++;
    		this.flag3=true;
    	}
    },
    menuBtn(){
    	this.flag2=false;
    },
    closeBtn(){
//  	this.flag3=false;
      this.$router.push({path:'/'})
    },
    save(){//点击保存
    	this.saveFlag=true;
    },
    closeSave(){//关闭保存页面
    	this.saveFlag=false;
    }
 },
 mounted(){
 	 $('.main').on('click','.rem',function(){
 	 	 $(this).parent().remove();
 	 })
	 $('.access').on('click','.rem',function(){
	 	 $(this).parent().remove();
	 })
	 $('.season').on('click','.rem',function(){ 	
	 	 var sz=$(this).parent()[0].className;
	 	 flag[sz]=0;
	 	 $(this).parent().remove();
	 	 $('.vux-checker-item').eq(sz).removeClass('demo5-item-selected');
	 })
	 $('.othersea').on('click','.rem',function(){
 	 	 $(this).parent().remove();
 	 })
	 $('.cookStep').on('click','.rem',function(){
 	 	 $(this).parent().remove();
 	 })
 }
}
</script>
<style scoped>
	.steptext span{
		display: inline-block;
		text-align: center;
		width: 33.3%;
		font-size: 13px;
		color:#3E3E3E;
	}
	.upload{
		width: 100%;
		height: 225px;
		background: #fbfbfb;
		text-align:center;
		color: #8e8e8e;
		line-height: 225px;
		margin-top: 25px;
	}
	.dishname{
		width: 90%;
		height: 60px;
		margin-left: 5%;
		font-size: 16px;
		text-align: center;
		outline: none;
		border: none;
		border-bottom: 1px solid #dddddd;
	}
	.alike *{
		display: inline-block;
		vertical-align: middle;
	}
	.alike img{
		width: 20px;
	}
	.alike span{
		color: #e83426;
		font-size: 15px;
		font-weight: bold;
	}
	.show{
		margin-top: 20px;
		padding-left: 10px;
		padding-right: 10px;
	}
	.zliao{
		width: 100%;
		height: 30px;
		background: #f9f9f9;
		color: #E83428;
		line-height: 30px;
		margin-top: 15px;
		text-indent: 10px;
	}
	.main li,.access li,.season li,.othersea li{
		width: 100%;
		height: 45px;
		box-sizing: border-box;
		border-bottom: 1px solid #e2e2e2;
		list-style: none;
		line-height: 45px;
		position: relative;
	}
	.main li *,.access li *,.season li *,.othersea li *{
		float: left;
	}
	.maintext,.weight{
		height: 100%;
		line-height: 44px;
		outline: none;
		border: none;
	}
	.maintext{
		width: 65%;				
		text-indent: 10px;
	}
	.weight{
		width: 20%;
		text-align: right;		
	}
	.ke{
		display: inline-block;
		height: 100%;
		border-right: 1px solid #e2e2e2;
	}
	.rem{
		width: 20px;
		position: absolute;
		right: 10px;
		top: 15px;
	}
	.add{
		text-align: right;
		padding-right: 10px;
		margin-top: 5px;
	}
	.add img{
		width: 20px;
	}
	.seasonText{
		font-size: 12px;
		color: #E83428;
		padding-left: 10px;
		margin-top: 15px;
	}
	.demo5-item {
	  width: 22.6%;
	  height: 105px;
	  background: #f9f9f9;
	  margin-left:2%;
	  margin-top: 12px;
	  text-align: center;
	}
	.demo5-item img{
		width:80%;
		height:100%;
	}
	.demo5-item p{
		font-size: 12px;
		color:#3E3E3E;
	}
	.demo5-item-selected {
	  background: #b4b4b4 url(../../static/credit/select01.png) no-repeat right bottom;
	  background-size:20px;
	}
	.season{
		margin-top: 15px;
	}
	.cookStep li{
		height: 45px;
		line-height: 40px;
		margin-left: 15px;
		list-style: none;
		position: relative;
	}
	textarea{
		width: 80%;
		position: absolute;
		top: 12px;
		font-size: 13px;
		color: #3E3E3E;
		outline: none;
		border: none;
		resize : none;
	}
	.btnDiv{
		margin-top: 40px;
		padding-left: 5%;
		padding-right: 5%;
	}
	.btnDiv button{
		width: 45%;
		height: 40px;
		border: 1px solid #E83428;
		border-radius: 5px;
		background:#FFFFFF;
		color: #E83428;
	}
	.btnDiv button:nth-child(2){
		margin-left: 10%;
	}
	.mask1{
		width: 100%;
		height: 100%;
		background: rgba(0,0,0,0.5);
		position: fixed;
		top: 46px;
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
	.sucDiv{
		width: 100%;
		height: 100%;
		position: fixed;
		top: 46px;
		background: #FFFFFF;
		text-align: center;
	}
	.sucDiv img{
		width: 160px;
		margin-top: 90px;
	}
	.sucDiv p:nth-child(2){
	  font-size: 17px;	
	  color: #E83428;
	  margin-top: 15px;
	}
	.sucDiv p:nth-child(3){
	  font-size: 13px;	
	  color: #707070;
	  margin-top: 10px;
	}
	.sucDiv button{
		width: 90%;
		height: 40px;
		color: white;
		background: #E83428;
		border: none;
		border-radius: 5px;
		margin-top: 20%;
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
  #vux_view_box_body{background: #FFFFFF !important;}
  .entersteprd .weui-textarea{font-size: 13px !important;text-indent: 1em;}
  .entersteprd .vux-no-group-title:before{border-top: none !important;}
  .entersteprd .vux-no-group-title:after{border-bottom: none !important;}
  .entersteprd .weui-cells{margin-top: 0 !important;}
  
  .entersteprd .vux-step{width: 76%;margin-left: 12%;}
  .vux-step-item-head-process .vux-step-item-head-inner{border:none !important;background: #E83428 !important;}
  .vux-step-item-head-wait .vux-step-item-head-inner{border:none !important;background: #bcbcbc !important;}
  .vux-step-item-tail-finish{background:#e83428 !important;}
  .vux-step-item-head-finish .vux-step-item-head-inner{border: none !important;background: #E83428 !important;}
  .weui-icon-success-no-circle{color:white !important;}
</style>