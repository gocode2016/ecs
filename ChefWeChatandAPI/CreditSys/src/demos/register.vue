<template>
  <div class='register'>
    <div class="wdmicon">
    	  <img src='../../static/credit/logo.png'/>
    </div>  
    <div class="inputDiv">
    	  <group gutter='0.5rem'>
          	<x-input title="手机号" v-model="registerParams.TelePhone" @on-blur="getTele" keyboard="number" is-type="china-mobile" ></x-input>
      	</group>
    	  <group gutter='0.5rem'>      	    
    	  	  <x-input title="短信验证码" class="weui-vcode" v-model="identcode" @on-blur="testIdent"></x-input>  
    	  	  <span class='identcode' @click="getIdent" v-if="isShow">{{indentText}}</span>
    	  	  <span class='countNum' v-else>{{countDown}}秒</span>
     		</group>
     		<group gutter='0.5rem'>
    	  	  <x-input title="注册码" placeholder='非必填项,填写后可参与扫码积分' v-model="registerParams.RegistCode" @on-blur="testRegist"></x-input>
    	 	</group>
    	 	<group gutter='0.5rem'>
    	  	  <x-input title="姓名" v-model="registerParams.MemberName"></x-input> 
    	 	</group>
    	 	<group gutter='0.5rem'>
    	  	  <popup-picker title="岗位" :data="offer" :columns="2" v-model="registerParams.offerArr" show-name cancel-text='取消' confirm-text='确定' value-text-align	='left'></popup-picker>
    	 	</group>
    	 	<group gutter='0.5rem'>
    	  	  <popup-picker title="餐厅地址" :data="address" :columns="3" v-model="registerParams.addressArr" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
    	 	</group>
    	 	<group gutter='0.5rem'>
    	  	  <x-input title="餐厅名称" v-model="registerParams.HotelName"></x-input> 
    	 	</group>

    	 	<button @click="submit">提交</button>  
    </div>
    
    <!--注册成功提示页面-->
    <div class="sucDiv" v-show='sucFlag'>
      <img src="../../static/credit/success.png" />
      <p>恭喜你！注册成功</p>
      <p>即将跳转的个人中心</p>
    </div>
    
  </div>
</template>

<script>
	import apiUrl from '../apiUrls.js';
	import offer from '../offer.js';
	import { Cell, Group, XInput, PopupPicker, Alert, XButton,Loading,Toast } from 'vux'
	var flag=0;
	var btnNum=0;
	export default {		
	  components: {
	    Cell,
	    Group,
	    XInput,
	    PopupPicker,
	    Alert,
	    XButton,
	    Loading,
	    Toast
	  },
	  data () {
	    return {
	    	signsuccess:'',//签到成功页跳转到注册页标识
	    	activityId:'',//线下签到活动ID
	    	sucFlag:false, //显示注册成功页面
	    	indentText:'获取验证码',
	    	isShow:true,
	    	obj:'',
	      num1: 0,
	      num2: 0,
	      num3: 0,
	      countDown:60,
				provinceList: [],
				cityList: [],
				areaList: [],
	      url1: apiUrl.checkMemberTelephone,		// 手机号API
	      url2: apiUrl.sendRegistSMS,						// 验证码API
	      url3: apiUrl.checkRegistCode, 				// 注册码API
	      url4: apiUrl.createMember, 						// 提交API
	      identcode:'',													// 短信验证码
	      identcode1:'',												// 返回短信验证码
  	 	  registerParams:{
  	 	  	"offerArr": [],
  	 	  	"addressArr": [],
			    "ProvinceId": 0,
			    "ProvinceName": '',						//省
			    "CityId": 0,
			    "CityName": '',								//市
			    "AreaId": 0,
			    "AreaName": '',								//区
			    "MemberName": '',			        //成员名
			    "TelePhone": '',
			    "RegistCode":'',							//注册码
			    "Position": '',								//职位
			    "PositionType": '',			      //职位类别
			    "HotelName": '' ,       				//酒店名称
			    "OpenId":'',
			    "Nickname":'', 
			    "HeadImgUrl":''
			  },
        offer: [],
	      address: [],
	      RegistCode:0,
	      MemberRecId:0//推荐人id
	    }
	  },
	  methods: {
      getAddressList() {
      	var _this = this;
      	// 省
			  this.$http({
    	 	  method: 'GET',
    	 	  url: apiUrl.getAllProvince
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
    	 	  method: 'GET',
    	 	  url: apiUrl.getCityList
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
    	 	  method: 'GET',
    	 	  url: apiUrl.getAreaList
		 	  }).then(function(respose){
					var obj = respose.data;
					var areaList = JSON.parse(obj);
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
		    this.submitApi();//提交注册信息
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
	    	 	    if(respose.data!=0){
	    	 	   	  //不可以注册
	    	 	   	  _this.showPlugin ('该手机号已注册');
	    	 	   	  flag=1;
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
	 	    	  }).then(function(response){
//	 	    	  	console.log(response.data);
	    	 	    if(response.data!=0){
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
      //检测验证码
      testIdent(){
      	 if(this.identcode!=this.identcode1){
      	 	  this.showPlugin ('验证码不正确');
      	 	  flag=2;
      	 }
      },
      //验证注册码	
      testRegist(){
      	var _this=this;
      	if(this.registerParams.RegistCode!=""){
	      	this.$http({
	    	 	    method:'POST',
	    	 	    url:this.url3,
	    	 	    data:{'RegistCode':this.registerParams.RegistCode}
			 	  }).then(function(respose){
	            if(respose.data!=1){
	            	_this.showPlugin ('注册码错误');
	            	flag=3;
	            }	            	 	          
			 	  })
      	}      	
      },
			//设置cookie
			setCookie(cname,cvalue,exdays){
			  var d = new Date();
			  d.setTime(d.getTime()+(exdays*24*60*60*1000));
			  var expires = "expires="+d.toUTCString();
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
      	if(this.registerParams.RegistCode==''){//注册码为空传-1 
        	this.RegistCode=-1;
        }else{
        	this.RegistCode=Number(this.registerParams.RegistCode);
        }
         
      	if(this.registerParams.TelePhone==''){
        	this.showPlugin ('手机号不能为空');
        }else if(this.identcode==''){
        	this.showPlugin ('验证码不能为空');
        }else if(this.registerParams.MemberName==''){
        	this.showPlugin ('姓名不能为空');
        }else if(this.registerParams.offerArr==''){
        	this.showPlugin ('岗位不能为空');
        }else if(this.registerParams.addressArr==''){
        	this.showPlugin ('餐厅地址不能为空');
        }else if(this.registerParams.HotelName==''){
        	this.showPlugin ('餐厅名称不能为空');
        }else if(this.registerParams.OpenId==''||this.registerParams.OpenId==undefined||this.registerParams.OpenId==null){
        	this.showPlugin ('出现错误，请重新提交');
        }else{
	         	btnNum++;
	        	if(btnNum==1){
	        		this.showLoading () ;//显示loading
	        	  this.switchAddress();
	        	}
        }  	
              
      },
      submitApi(){
      	var _this=this; 
      	var Remark="";
      	var params = {
    				"ProvinceId": this.registerParams.ProvinceId,
				    "ProvinceName": this.registerParams.ProvinceName,		//省
				    "CityId": this.registerParams.CityId,
				    "CityName": this.registerParams.CityName,						//市
				    "AreaId": this.registerParams.AreaId,
				    "AreaName": this.registerParams.AreaName,						//区
				    "MemberName": this.registerParams.MemberName ,			//成员名
				    "TelePhone": this.registerParams.TelePhone,        //手机号
				    "RegistCode": this.RegistCode ,			//注册码
				    "Position": this.registerParams.offerArr[1],			  //职位
				    "PositionType": this.registerParams.offerArr[0],	  //职位类别
				    "HotelName": this.registerParams.HotelName,         //酒店名称
				    "HeadImgUrl":this.registerParams.HeadImgUrl,
	  			  "Nickname":this.registerParams.Nickname,
	  			  "OpenId":this.registerParams.OpenId,
	  			  "Remark":Remark,
	  			  "MemberRecId":this.MemberRecId
        }
      	this.$http({
    	 	  method: 'POST',
    	 	  url: this.url4,
    	 	  data: params
		 	  }).then(function(response){
           var data = JSON.parse(response.data);
           if(typeof(data)!="number"||data==-1){//需要返回会员Id     
           	 _this.$vux.loading.hide();//loading图标消失
	         	 setTimeout(function(){
	         	 	 _this.showPlugin('提交信息失败,请重新提交');
	         	 	 btnNum=0;
	         	 },1)         	 
	         }else{//注册成功 跳转到个人中心             	 
	         	   var cookievalue=_this.getCookie('WeiXinUser');
		           var cookievalue=cookievalue.replace('UserId=0','UserId='+ data);
		           var cookievalue=cookievalue.replace('UserType=0','UserType=2');
		           _this.setCookie('WeiXinUser',cookievalue);
		           if(_this.MemberRecId!=0){//推荐人ID 推荐同行注册
		           	 _this.getRecQualifications();
		           }else if(_this.signsuccess==0){//从签到页进入注册页
		           	 _this.addActivity(data);
		           }else{
	               	_this.$vux.loading.hide();//loading图标消失
			           	setTimeout(function(){             	 	 
			         	 	  _this.sucFlag=true;
			         	  },1)  
			         	  setTimeout(function(){
			         	 	  _this.$router.push('/component/personal');
			         	  },1000) 
		           }
	         }
		 	  })
      },
      getRecQualifications(){//获取推荐名额
				  var _this=this;
		  		this.$http({
		  			method:'get',
		  			url:apiUrl.getRecQualifications+'?MemberId='+this.MemberRecId
		  		}).then(function(response){
		  			if(response.data<=20){
		  				//增加积分
		  				_this.updateMemberIntegral();
		  			}else{
		          _this.$vux.loading.hide();//loading图标消失
		  				setTimeout(function(){
			          _this.sucFlag=true;
			        },1)
		          setTimeout(function(){
			         	_this.$router.push('/component/personal');
			        },1000) 
		  			}
		  		})
			},
			addActivity(userId){
				var _this=this;
				var params={
					"ActivityId":this.activityId,
					"UserId":userId,
					"OpenId":this.registerParams.OpenId,
					"SignDate":""
				}
				this.$http({
					method:'post',
					url:apiUrl.signIn,
					data:params
				}).then(function(res){
					var data=JSON.parse(res.data);
					var count=parseInt(data.Count);
					var activityName=data.text;
	        _this.$vux.loading.hide();//loading图标消失
	        _this.sucFlag=true;
	        setTimeout(function(){
	        	_this.$router.push({path:'/component/signsuccess',query:{count:count,activityName:activityName}});		
			    },1000) 
				})
			},
			updateMemberIntegral(){//更改积分
				var _this=this;
	  		var params={
	  		  'Operation':'plus',
					'Integral':20,
					'MemberId':this.MemberRecId,
					'IntegralSource':'推荐奖励',
					'Remark':'推荐同行注册'
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.updateMemberIntegral,
	  			data:params
	  		}).then(function(response){
		        _this.$vux.loading.hide();//loading图标消失
		        setTimeout(function(){
		          _this.sucFlag=true;
		        },1)
	          setTimeout(function(){
		         	_this.$router.push('/component/personal');
		        },1000) 
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
	          if(flag==2){
	        			_this.identcode='';
	        			flag=0;
	        	}
	          if(flag==3){
	        			_this.registerParams.RegistCode='';
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
	  	  this.getAddressList();
//      会员 o11Z-jjgramCWa2NUJY5y8HVpst4
//      队员 o11Z-jl5QeM5T5kZ4e5MCxweE1_E
//      this.setCookie("WeiXinUser","UserId=0&UserType=0&Openid=o11Z-jjgramCWa2NUJY5y8HVpst4&Nickname=%e6%8c%91%e5%b1%b1%e6%b3%89%e7%9a%84%e5%b0%8f%e6%9c%a8%e6%a1%b6&Sex=1&Province=%e5%b1%b1%e4%b8%9c&City=%e7%83%9f%e5%8f%b0&Country=%e4%b8%ad%e5%9b%bd&Headimgurl=http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKKlicnRBVQwmzJfa6GPu5dHP4zuLCnMYeUcSjBYFTA16nGNGoc5oJzncRK0hNvQFUCIaXibqicPhPvQ/0",100);
        //获取openId   
        this.obj=this.parse(this.getCookie("WeiXinUser"));
        this.registerParams.OpenId=this.obj.Openid;
        this.registerParams.Nickname=decodeURI(this.obj.Nickname);
        this.registerParams.HeadImgUrl=this.obj.Headimgurl;
        if(this.obj.UserId>0&&this.obj.UserType==2){
        	this.$router.push('/component/personal');
        }else if(this.obj.UserId>0&&this.obj.UserType==1){//已经是队员
          //   	不能注册会员
        	this.$router.push('/component/teammember');
        }

        //推荐ID
        var locationUrl=window.location.href;
        if(locationUrl.indexOf('RecommendId')<0){//没有推荐id
        	this.MemberRecId=0;
        }else{
        	var RecommendArr=locationUrl.split('?')[1];
        	this.MemberRecId=parseInt(RecommendArr.split('=')[1]);
        }
        
        this.signsuccess=this.$route.query.signsuccess;
        this.activityId=this.$route.query.activityId;
        
	  	  // 设置岗位数据
	  	  this.offer = offer.offerArr;
	  	  // 设置 register 高度
	  	  $('.register').height(window.screen.height);
	  	  var height=window.screen.height;
	  	  $('.register .weui-cell,.register button').css({
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
  .identcode,.countNum{position: absolute;right:10px;font-size: 13px;display:inline-block ;}
  .identcode{color: #e83428;}
  .countNum{color: #b1b1b1;}
  .register button{
  	width: 100%;
  	margin-top: 20px;
  	background: #e83428; border: none; color: #fff;
  	font-size: 16px;
  	border-radius: .5rem;  	
  }
  /*会员注册成功页面*/
  .sucDiv{width: 100%;height: 100%;position: fixed;top: 0px;background: #FFFFFF;text-align: center;}    
  .sucDiv img{ width: 160px;margin-top: 90px;}
  .sucDiv p:nth-child(2){font-size: 17px;color: #E83428;margin-top: 15px;}
  .sucDiv p:nth-child(3){font-size: 13px;color: #707070;margin-top: 10px;}
</style>

<style>
  .register{ background: url('../../static/credit/register_bg.png') no-repeat left bottom; background-size: 100% 100%; overflow: hidden; }
  .register .weui-label { width: 4.5rem !important;}
  .register .weui-cells { border: 1px solid #dddddd; border-radius: 0.5rem; }
  .vux-popup-picker-header-menu-right { color: #e83428 !important; } 
  .weui-dialog__btn_primary{color:#e83428 !important;}
  .weui-cells:before{border-top:0 !important;}
  .weui-cells:after{border-bottom:0 !important;}
  .vux-cell-box:before{border-top:0 !important;}
  .vux-cell-box:after{border-bottom:0 !important;}
  .register .weui-cell{font-size: 13px;color: #3e3e3e;}
  .register .weui-cell__ft:after{display: none !important;}
  .register .weui-vcode .weui-icon-clear:before{display: none !important;} 
  .register .weui-cell{padding: 0 15px!important;}
</style>
