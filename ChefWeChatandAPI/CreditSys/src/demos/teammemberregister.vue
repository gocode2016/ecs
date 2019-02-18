<template>
  <div class='teammemberregister'>
    <div class="wdmicon">
    	  <img src='../../static/credit/logo.png' />
    </div>  
    <div class="inputDiv">
    	  <group gutter='0.5rem'>
          	<x-input title="手机号" v-model="registerParams.TelePhone"  keyboard="number" is-type="china-mobile" ></x-input>
      	</group>
    	  <group gutter='0.5rem'>      	    
    	  	  <x-input title="短信验证码" class="weui-vcode" v-model="identcode" ></x-input>  
    	  	  <span class='identcode' @click="getIdent" v-if="isShow">{{indentText}}</span>
    	  	  <span class='countNum' v-else>{{countDown}}秒</span>
     		</group>
     		<group gutter='0.5rem'>
    	  	  <popup-picker title="岗位" :data="positionData"  v-model="registerParams.Position"  cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
    	 	</group>
     		<group gutter='0.5rem'>
    	  	  <x-input title="工号" v-model="registerParams.WorkNum"></x-input>
    	 	</group>
    	 	<group gutter='0.5rem'>
    	  	  <x-input title="姓名" v-model="registerParams.MemberName"></x-input> 
    	 	</group>
    	 	<!--<group gutter='0.5rem'>-->
    	  	  <!--<popup-picker title="大区" :data="region" :columns="2" v-model="registerParams.regionArr" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>-->
    	 	<!--</group>-->
    	 	<group gutter='0.5rem'>
    	  	  <popup-picker title="工作地点" :data="address" :columns="3" v-model="registerParams.addressArr" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
    	 	</group>
    	 	<group gutter='0.5rem'>
    	  	  <x-input title="身份证号码" v-model="registerParams.CardId"></x-input> 
    	 	</group>

    	 	<button @click="submit">提交</button>
    </div>
    
    
     <!--提交成功页面-->
    <div class="sucDiv" v-show='sucFlag'>
      <img src="../../static/credit/success.png" />
      <p>恭喜你！提交成功</p>
      <p>48小时内将告知审核结果</p>
    </div>
    <!--审核中-->
    <div class="sucDiv" v-show='shFlag'>
    	<img src="../../static/credit/success.png" />
      <p>您正在等待</p>
      <p>欣和酒店业务代表身份审核中</p>
    </div>
  </div>
</template>

<script>
  import apiUrl from '../apiUrls.js';
//import region from '../region.js';
	import { Cell, Group, XInput, PopupPicker, Alert, AjaxPlugin, XButton,Loading,Toast } from 'vux'
	var flag=0;
	var btnNum=0;
	export default {
	  components: {
	    Cell,
	    Group,
	    XInput,
	    PopupPicker,
	    Alert,
	    AjaxPlugin,
      XButton,
      Loading,
      Toast
	  },
	  data () {
	    return {
	    	userId:'',
	    	sucFlag:false, //显示注册成功页面
	    	shFlag:false,//审核中页面
	    	indentText:'获取验证码',
	    	obj:'',
	    	isShow:true,
	      num1: 0,
	      num2: 0,
	      num3: 0,
	      countDown:60,
        provinceList: [],
        cityList: [],
        areaList: [],
	      url1: apiUrl.checkMemberTelephone,         // 手机号API
	      url2: apiUrl.sendRegistSMS,                // 验证码API
	      url3: apiUrl.addRegistSaleMan,             // 提交API
	      identcode:'',//短信验证码
	      identcode1:'',//返回短信验证码
  	 	  registerParams:{
//        "regionArr": [],
          "Position":[],//岗位
          "addressArr": [],
          "ProvinceId": 0,
          "ProvinceName": '',   //省
          "CityId": 0,
          "CityName": '',       //市
          "AreaId": 0,
          "AreaName": '',       //区
          "MemberName": '',			//成员名
			    "TelePhone": '',
			    "CardId": '' ,        //身份证号
			    'WorkNum': '',         //工号
			    "OpenId":'',
			    "Nickname":'', 
			    "HeadImgUrl":''
			  },
//			  region:[],    //大区数据   
	      address: [],
  		  positionData: [['STL', 'RI','WSI','CSI','DSR']],//岗位数据
	    }
	  },
	  methods: {
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
	  			var Data=JSON.parse(data.Data);
//	  			console.log(Data);
	  			if(Data.RegistState==1){//1注册成功
	  				_this.$router.push('/component/teammember');
	  			}else if(Data.RegistState==0){//审核中
	  				_this.shFlag=true;
	  			}
	  		})
	  	},
      getAddressList() {
      	var _this = this;
      	// 省
			  this.$http({
    	 	  method:'GET',
    	 	  url:apiUrl.getAllProvince,
    	 	  data:{}
		 	  }).then(function(respose){
					var obj = respose.data;
					var provinceList = JSON.parse(obj);
					_this.num1 = provinceList[provinceList.length - 1].ProvinceId;
					var provinceArr = new Array();
					for(let i in provinceList) {						
						_this.address.push({
							name: provinceList[i].ProvinceName.toString(),
		        	value: provinceList[i].ProvinceId.toString(),
		        	parent: '0'
						})
					}
          _this.provinceList = provinceList;
					// console.log(_this.address);
		 	  })

		 	  // 市
			  this.$http({
    	 	  method:'GET',
    	 	  url:apiUrl.getCityList,
    	 	  data:{}
		 	  }).then(function(respose){
					var obj = respose.data;
					var cityList = JSON.parse(obj);
					_this.num2 = cityList[cityList.length -1].CityId;
					var cityArr = new Array();
					for(let i in cityList) {
					  var idx = cityList[i].CityId + _this.num1;		
						_this.address.push({
							name: cityList[i].CityName.toString(),
		        	value: idx.toString(),
		        	parent: cityList[i].ProvinceId.toString(),
						})
					}
          _this.cityList = cityList;
					//console.log(_this.address);
		 	  })

		 	  // 区
			  this.$http({
    	 	  method:'GET',
    	 	  url:apiUrl.getAreaList,
    	 	  data:{}
		 	  }).then(function(respose){
					var obj = respose.data;
					var areaList = JSON.parse(obj);
					console.log(_this.num1, _this.num2);
					var areaArr = new Array();
					for(let i in areaList) {
					  var idx = areaList[i].AreaId + _this.num2 + _this.num1;	
					  var idx2 = areaList[i].CityId + _this.num1;					
						_this.address.push({
							name: areaList[i].AreaName.toString(),
		        	value: idx.toString(),
		        	parent: idx2.toString()
						})
					}
          _this.areaList = areaList;
          //console.log(_this.address);					
		 	  })
      },
      switchAddress() {
        // 已获取的后台省市区数据
        var provinceList = this.provinceList;
        var cityList = this.cityList;
        var areaList = this.areaList;
        // 将前台省市区联动数据转换回后台省市区数据
        // 转换省市区id
        var provinceId = this.registerParams.addressArr[0] - 0;
        var cityId = this.registerParams.addressArr[1] - this.num1;
        var areaId = this.registerParams.addressArr[2] - this.num2 - this.num1;
        // 获取名称
        var provinceName = '', cityName = '', areaName = '';
        for(let i in provinceList) {
          if(provinceList[i].ProvinceId == provinceId){
            provinceName = provinceList[i].ProvinceName;
          }
        }
        for(let i in cityList) {
          if(cityList[i].ProvinceId == provinceId && cityList[i].CityId == cityId) {
            cityName = cityList[i].CityName;
          }
        }
        for(let i in areaList) {
          if(areaList[i].CityId == cityId && areaList[i].AreaId == areaId ) {
            areaName = areaList[i].AreaName;
          }         
        }

        this.registerParams.ProvinceId = provinceId;
        this.registerParams.ProvinceName = provinceName;
        this.registerParams.CityId = cityId;
        this.registerParams.CityName = cityName;
        this.registerParams.AreaId = areaId;
        this.registerParams.AreaName = areaName;
        this.submitApi();//提交信息接口
      },
      //验证手机号
      getTele(){
      	  var _this=this;
	        var box=/^1[3|4|5|7|8]\d{9}$/;
		 	    if(!box.test(this.registerParams.TelePhone)){
		 	    	this.showPlugin ('请输入正确的手机号');
		 	    	flag=1;
		 	    }else{
	 	    	  this.$http({
	    	 	    method:'POST',
	    	 	    url:this.url1,
	    	 	    data:{'TelePhone':this.registerParams.TelePhone}
	 	    	  }).then(function(respose){
	    	 	    if(respose.data!=0){	//不可以注册
	    	 	    	_this.$vux.loading.hide();//loading图标消失
	    	 	    	btnNum=0;
	    	 	   	  _this.showPlugin ('该手机号已注册');
	    	 	   	  flag=1;
	    	 	    }else{
            	  _this.switchAddress();//获取省市区
	    	 	    }
	 	    	  })
		 	    }         
      },
      //获取验证码
      getIdent(){
      	var _this=this;
	        var box=/^1[3|4|5|7|8]\d{9}$/;
		 	    if(!box.test(this.registerParams.TelePhone)){
		 	    	this.showPlugin ('请输入正确的手机号');
		 	    	flag=1;
		 	    }else{
	 	    	  this.$http({
	    	 	    method:'POST',
	    	 	    url:this.url1,
	    	 	    data:{'TelePhone':this.registerParams.TelePhone}
	 	    	  }).then(function(respose){
	    	 	    if(respose.data!=0){
	    	 	   	  //不可以注册
	    	 	   	  _this.showPlugin ('该手机号已注册');
	    	 	   	  flag=1;
	    	 	    }else{
	    	 	    	//可以注册
	    	 	    	//60s倒计时
					    	_this.isShow=false;
					    	var count=setInterval(function(){
					    		_this.countDown--;
					    		if(_this.countDown==-1){
					    			clearInterval(count);		  	    	
						  	    _this.countDown=60;
						  	    _this.isShow=true;
						  	    _this.indentText='重新获取';
						      }
					    	},1000)
					    	_this.$http({
					  	 	  method:'POST',
					  	 	  url:_this.url2,
					  	 	  data:{'Telephone':_this.registerParams.TelePhone}
							 	  }).then(function(respose){
					    	    _this.identcode1=respose.data;                 
							 	})
	    	 	    }
	 	    	  })
		 	    }       
      },
      //设置cookie
			setCookie(cname,cvalue,exdays){
			  var d = new Date();
			  d.setTime(d.getTime()+(exdays*24*60*60*1000));
			  var expires = "expires="+d.toGMTString();
			  document.cookie = cname + "=" + cvalue + "; " + expires;
			},
      // 获取cookie
	    getCookie(cname){	
			  var name = cname + "=";
			  var ca = document.cookie.split(';');
			  for(var i=0; i<ca.length; i++) 
			  {
			    var c = ca[i].trim();
			    if (c.indexOf(name)==0) return c.substring(name.length,c.length);
			  }
			  return "";
	    },
	    // 将cookie的value转为对象
	    parse(num){
				var a = num.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
			},
      //提交
      submit(){
      	  var idCard=/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;//身份证正则
      	  if(this.registerParams.TelePhone==''){         	         
          	this.showPlugin ('手机号不能为空');
          }else if(this.identcode==''){
          	this.showPlugin ('验证码不能为空');
          }else if(this.identcode!=this.identcode1){
          	this.showPlugin ('验证码有误');
          }else if(this.registerParams.Position==''){
          	this.showPlugin ('岗位不能为空');
          }else if(this.registerParams.Position!='DSR'&&this.registerParams.WorkNum==''){
          	this.showPlugin ('工号不能为空');
          }else if(this.registerParams.MemberName==''){
          	this.showPlugin ('姓名不能为空');
          }else if(this.registerParams.addressArr==''){
          	this.showPlugin ('工作地点不能为空');
          }else if(this.registerParams.CardId==''){
          	this.showPlugin ('身份证号码不能为空');
          }else if(!idCard.test(this.registerParams.CardId)){
	      		this.showPlugin ('身份证号码不正确');
		  	 	  flag=3;
      	  }else{
      	  	btnNum++;
            if(btnNum==1){
            	this.showLoading () ;//显示loading
            	this.getTele();//验证手机号
            }
          }
      },
      submitApi(){//队员注册接口
        var _this = this;
//    	var region = this.registerParams.regionArr[0].split("|");
      	var params = {
          "ProvinceId": this.registerParams.ProvinceId,
          "ProvinceName": this.registerParams.ProvinceName,    //省
          "CityId": this.registerParams.CityId,
          "CityName": this.registerParams.CityName,            //市
          "AreaId": this.registerParams.AreaId,
          "AreaName": this.registerParams.AreaName,            //区
//        "RegionId": region[0],                               //大区ID
//        "RegionName": region[1],                             //大区名称
          "Position":this.registerParams.Position[0],             //岗位
          "WorkNum": this.registerParams.WorkNum,              //工号
          "Name": this.registerParams.MemberName,              //姓名
          "Telephone": this.registerParams.TelePhone,          //电话
          "CardId": this.registerParams.CardId,                //身份证号
          "HeadImgUrl":this.registerParams.HeadImgUrl,
  			  "Nickname":this.registerParams.Nickname,
  			  "OpenId":this.registerParams.OpenId
        }
      	this.$http({
 	    	 	  method:'POST',
 	    	 	  url:this.url3,
 	    	 	  data: params
		 	  }).then(function(respose){
		 	  	 _this.$vux.loading.hide();//loading图标消失
           var data = JSON.parse(respose.data);
	         if(typeof(data)!="number"){//需要返回队员Id
	         	 setTimeout(function(){
		     	 	   _this.showPlugin('提交信息失败，请重新提交');
		     	 	   btnNum=0;
		     	   },1) 
	         }else{//提交成功 需要审核 	             	 
	         	 var cookievalue=_this.getCookie('WeiXinUser');
	           var cookievalue=cookievalue.replace('UserId=0','UserId='+ data);
	           var cookievalue=cookievalue.replace('UserType=0','UserType=1');
	           _this.setCookie('WeiXinUser',cookievalue);
	         	 setTimeout(function(){             	 	 
	         	 	 _this.sucFlag=true;
	         	 },1)        	 
	         }
		 	  })
      },
      //提醒样式
      showPlugin (text) {
      	var _this=this;
	      this.$vux.alert.show({
	        content: this.$t(text),
	        onHide (){
	        	if(flag==1){
	        			_this.registerParams.TelePhone='';
	        			flag=0;
	        	}
	          if(flag==3){
	        			_this.registerParams.sfznum='';
	        			flag=0;
	        	}
          }
	      })	      
      },
      showLoading () {//loading 图标
	      this.$vux.loading.show({
	        text: 'Loading'
	      })
	    },
	    onChange (val) {
	      const _this = this
	      if (val) {
	        this.$vux.toast.show({
	          text: 'Hello World',
	          onShow () {
	            console.log('Plugin: I\'m showing')
	          },
	          onHide () {
	            console.log('Plugin: I\'m hiding')
	            _this.show9 = false
	          }
	        })
	      } else {
	        this.$vux.toast.hide()
	      }
		  }
	  },
	  mounted: function(){
	  	  // 获取省市区及改写数据
	  	  // 切记给后台传数据的时候，
	  	  // 省值不变，
	  	  // 市值减去市列表的最后一条数据的id值，再传
	  	  // 区值减去区列表的最后一条数据的id值再减去市列表的最后一条数据的id值，再传
	  	  this.getAddressList();
        
        //获取openId   
        this.obj=this.parse(this.getCookie("WeiXinUser"));
        this.registerParams.OpenId=this.obj.Openid;
        this.registerParams.Nickname=decodeURI(this.obj.Nickname);
        this.registerParams.HeadImgUrl=this.obj.Headimgurl;
        if(this.obj.UserId>0&&this.obj.UserType==1){
        	this.userId=this.obj.UserId;
        	this.getSaleManInfo();//获取队员信息
        }else if(this.obj.UserId>0&&this.obj.UserType==2){//会员
//      	//   	不能注册队员
          this.$vux.toast.text('您已经是会员', 'middle');
          var _this=this;
          setTimeout(function(){
          	_this.$router.push('/component/personal');
          },1)
        }
        
        // 设置大区数据
//      this.region = region.regionArr;

	  	  // 设置 teammemberregister 高度
	  	  $('.teammemberregister').height(window.screen.height);
	  	  var height=window.screen.height;
	  	  $('.teammemberregister .weui-cell,.teammemberregister button').css({
	  	  	'height':height*0.07
	  	  })
	  	  $('.identcode,.countNum').css({
	  	  	'top': height*0.07*0.35+'px'
	  	  })
	  	  
        //设置弹框字段
        $('.weui-dialog__btn_primary').html('确定');
	  }
	}
</script>

<style scoped>
  .wdmicon{text-align: center;margin-top: 1rem;} 
  .wdmicon img{width: 30%;}
  .inputDiv{width: 90%;margin-left: 5%;}
  .identcode,.countNum{position: absolute;top: 13px;right:10px;font-size: 13px;}
  .identcode{color: #e83428;}
  .countNum{color: #b1b1b1;}
  .teammemberregister button {
  	width: 100%;
  	margin-top: 20px;
  	background: #e83428; border: none; color: #fff;
  	font-size: 16px;
  	border-radius: .5rem;
  }
  /*提交成功页面*/
  .sucDiv{width: 100%;height: 100%;position: fixed;top: 0px;background: #FFFFFF;text-align: center;}    
  .sucDiv img{ width: 160px;margin-top: 90px;}
  .sucDiv p:nth-child(2){font-size: 17px;color: #E83428;margin-top: 15px;}
  .sucDiv p:nth-child(3){font-size: 13px;color: #707070;margin-top: 10px;}
  .sucDiv button{width: 90%;height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 30%;}
</style>
<style>
  .teammemberregister{ background: url('../../static/credit/register_bg.png') no-repeat left bottom; background-size: 100% 100%; overflow: hidden; }
  .teammemberregister .weui-label { width: 4.5rem !important; }
  .teammemberregister .weui-cells { border: 1px solid #dddddd; border-radius: 0.5rem; }
  .vux-popup-picker-header-menu-right { color: #e83428 !important; } 
  .weui-dialog__btn_primary{color:#e83428 !important;}
  .weui-cells:before{border-top:0 !important;}
  .weui-cells:after{border-bottom:0 !important;}
  .vux-cell-box:before{border-top:0 !important;}
  .vux-cell-box:after{border-bottom:0 !important;}
  .teammemberregister .weui-cell{font-size: 13px;color: #3e3e3e;}
  .teammemberregister .weui-cell__ft:after{display: none !important;}
  .teammemberregister .weui-vcode .weui-icon-clear:before{display: none !important;} 	
  .teammemberregister .weui-cell{padding: 0 15px!important;}
</style>

