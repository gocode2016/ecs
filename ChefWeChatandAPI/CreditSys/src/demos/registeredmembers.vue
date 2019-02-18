<template>
  <div class='registeredmembers'>
    <div class="wdmicon">
    	<p>专业厨师服务平台</p>
    </div>  
    <div class="inputDiv">
    	  <group gutter='0.5rem'>
          	<x-input title="联系方式" v-model="registerParams.Phone"  keyboard="number" is-type="china-mobile" ></x-input>
      	</group>
    	  <group gutter='0.5rem'>      	    
    	  	  <x-input title="短信验证码" class="weui-vcode" v-model="identcode" @on-blur="testIdent"></x-input>  
    	  	  <span class='identcode' @click="getIdent" v-if="isShow">{{indentText}}</span>
    	  	  <span class='countNum' v-else>{{countDown}}秒</span>
     	  </group>
     	  <group gutter='0.5rem'>
          	<x-input title="认证码" v-model="registerParams.RegistCode"  keyboard="number" placeholder="选填" style="color: #BEBEBE;"></x-input>
      	</group>
    	 	<button @click="submit" class="submit">确认</button>  
    </div>
    
    <!--注册成功提示页面-->
    <div class="sucDiv" v-show='sucFlag'>
      <img src="../../static/credit/success.png" />
      <p>恭喜你！注册成功</p>
      <p>即将跳转到个人中心</p>
    </div>
  </div>
</template>

<script>
	import apiUrl from '../apiUrls.js';
	import { Cell, Group, XInput, PopupPicker, Alert, XButton,Loading,Toast,cookie } from 'vux'
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
	    Toast,
	    cookie
	  },
	  data () {
	    return {
	    	source:'',//注册来源
	    	remark:'',//产品扫码注册传参
	    	skudata:{},
	    	addressId:'',
        userData:{},
        userId:'',
        userType:'',
	    	sucFlag:false, //显示注册成功页面
	    	indentText:'获取验证码',
	    	isShow:true,
	      countDown:60,
	      identcode:'',							// 短信验证码
	      identcode1:'',							// 返回短信验证码
  	 	  registerParams:{
	        "Phone": '',                    //手机
	        "OpenId":'',
	        "Nickname":'', 
	        "HeadImgUrl":'' ,
	        "RegistCode":''
			  }
	    }
	  },
	  methods: {
      //验证手机号
      getTele(){
      	  var _this=this;
	        var box=/^1[3|4|5|7|8]\d{9}$/;
		 	    if(!box.test(this.registerParams.Phone)){
		 	    	this.showPlugin ('请输入正确的手机号');
		 	    	flag=1;
		 	    }else{
	 	    	  this.$http({
	    	 	    method:'POST',
	    	 	    url:apiUrl.checkMemberTelephone,
	    	 	    data:{'TelePhone':this.registerParams.Phone}
	 	    	  }).then(function(respose){
	    	 	    if(respose.data!=0){
	    	 	   	  //不可以注册
	    	 	   	  flag=1;
	    	 	   	  _this.showPlugin ('该手机号已注册');
	    	 	    }else{
	    	 	    	btnNum++;
				        if(btnNum==1){
			            _this.showLoading();//显示loading
			          	_this.submitApi();//提交注册信息
				        }
	    	 	    }
	 	    	  })
		 	    }         
      },
      //获取验证码
      getIdent(){     	 
          var _this=this;
	        var box=/^1[3|4|5|7|8]\d{9}$/;
		 	    if(!box.test(this.registerParams.Phone)){
		 	    	flag=1;
		 	    	this.showPlugin ('请输入正确的手机号');
		 	    }else{
	 	    	  this.$http({
	    	 	    method:'POST',
	    	 	    url:apiUrl.checkMemberTelephone,
	    	 	    data:{'TelePhone':this.registerParams.Phone}
	 	    	  }).then(function(response){
	    	 	    if(response.data!=0){
	    	 	   	  //不可以注册
	    	 	   	  flag=1;
	    	 	   	  _this.showPlugin ('该手机号已注册');
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
					  	 	  url:apiUrl.sendRegistSMS,
					  	 	  data:{'Telephone':_this.registerParams.Phone}
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
      	 	  flag=2;
      	 	  this.showPlugin ('验证码不正确');
      	 }
      },
      testRegist(){//验证认证码	
      	var _this=this;
      	this.$http({
    	 	    method:'POST',
    	 	    url:apiUrl.checkRegistCode,
    	 	    data:{'RegistCode':this.registerParams.RegistCode}
		 	  }).then(function(respose){
            if(respose.data!=1){
            	_this.showPlugin ('认证码错误');
            	flag=3;
            }else{
        			_this.getTele();//验证手机号
            }
		 	  })
      },
      //提交
      submit(){  
      	if(this.registerParams.Phone==''){
        	this.showPlugin ('手机号不能为空');
        }else if(this.identcode==''||(this.identcode!=this.identcode1)){
        	this.showPlugin ('验证码不正确');
        }else if(this.registerParams.RegistCode != ''){
        	this.testRegist();//验证认证码	
        }else{
        	this.getTele();//验证手机号
        }  	 
      },
      submitApi(){
      	var _this=this; 
      	if(this.source=='thanksdraw'){//通过感恩节抽奖注册
      		var remark="感恩节抽奖";
      	}else if(this.source=='cookday'){//通过厨师节活动注册
      		var remark="厨师节活动";
      	}else if(this.source=='shopdetail'){//通过申领试用注册
      		var remark="申领试用";
      	}else if(this.remark != undefined){//通过产品扫码注册
      		var remark = this.remark;
      	}else{
      		var remark="";
      	}
      	var params = {
				  "Phone": this.registerParams.Phone,  //手机号
  			  "OpenId":this.registerParams.OpenId,
  			  "Nickname":this.registerParams.Nickname,
  			  "HeadImgUrl":this.registerParams.HeadImgUrl,
  			  "Remark":remark,//备注
  			  "RegistCode":this.registerParams.RegistCode//认证码
        }
      	this.$http({
    	 	  method: 'POST',
    	 	  url: apiUrl.createVisitor,
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
		          _this.userData.UserId=data;
				      _this.userData.UserType=2;
				      var cookieValue = [];
							for(var i in _this.userData){
								cookieValue.push(i + "=" + _this.userData[i]);
							}
							cookieValue=cookieValue.join("&");
		          _this.setCookie('WeiXinUser',cookieValue);
		         
             	_this.$vux.loading.hide();//loading图标消失
	           	setTimeout(function(){             	 	 
	         	 	  _this.sucFlag=true;
	         	  },1) 
	         	  
	         	  if(_this.source=='thanksdraw'){//通过感恩节抽奖进入注册页
	         	  	setTimeout(function(){
		         	 	  _this.$router.push('/component/thanksdraw');
		         	  },1000)
	         	  }else if(_this.source=='cookday'){//通过厨师节活动进入注册页
	         	  	setTimeout(function(){
		         	 	  _this.$router.push('/component/cookday');
		         	  },1000)
	         	  }else if(_this.source=='shopdetail'){//商城 申领试用活动
	         	  	setTimeout(function(){
		         	 	  _this.$router.push({path:'/component/resumehotel',query:{skudata:_this.skudata,productType:1,source:_this.source,addressId:_this.addressId}});
		         	  },1000)
	         	  }else{
	         	  	setTimeout(function(){
		         	 	  _this.$router.push('/component/resumehotel');
		         	  },1000)
	         	  }
	         	   
	          }
		 	  })
      },
      setCookie(cname,cvalue,exdays){//设置cookie
			  var d = new Date();
			  d.setTime(d.getTime()+(exdays*24*60*60*1000));
			  var expires = "expires="+d.toUTCString();
			  document.cookie = cname + "=" + cvalue + "; " + expires;
			},
      //提醒样式
      showPlugin (text) {
      	var _this=this;
	      this.$vux.alert.show({
	        content: this.$t(text),
	        onHide (){
	        	if(flag==1){
	        			_this.registerParams.Phone='';
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
	  	
      //获取用户信息
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
	    this.registerParams.OpenId=this.userData.Openid;
	    this.registerParams.Nickname=decodeURI(this.userData.Nickname);
      this.registerParams.HeadImgUrl=this.userData.Headimgurl;
      
			this.skudata=this.$route.query.skudata;//商品信息
			this.source=this.$route.query.source;//注册来源
			this.addressId=this.$route.query.addressId;//地址id
			if(this.$route.query.remark.indexOf('#/')!=-1){//产品扫码注册追踪来源
				this.remark = this.$route.query.remark.slice(0,-2)
			}else{
				this.remark = this.$route.query.remark
			}
			
      //判断是否注册会员
      if(this.userId>0&&this.userType==2){
       	this.$router.push('/component/personal');
      }else if(this.userId>0&&this.userType==1){//已经是队员  不能注册会员
       	this.$router.push('/component/teammember');
      }

      //设置弹框字段
      $('.weui-dialog__btn_primary').html('确定');
      
	  }
	}
</script>
<style scoped>
  .wdmicon{height:20%;background: url('../../static/credit/register_icon.png') no-repeat center;background-size: 100% 100%;overflow: hidden;}
  .wdmicon p{text-align: center;color: #e73229;font-size: 22px;margin-top: 10%;letter-spacing: 1px;}
  .inputDiv{width: 90%;margin-left: 5%;}
  .identcode,.countNum{position: absolute;right:10px;font-size: 13px;display:inline-block ;top:13px}
  .identcode{color: #e83428;}
  .countNum{color: #b1b1b1;}
  .registeredmembers button{width: 100%;margin-top: 70px;background: #e83428; border: none; color: #fff;font-size: 16px;border-radius: 5px;height: 46px;}
    
  /*会员注册成功页面*/
  .sucDiv{width: 100%;height: 100%;position: fixed;top: 0px;background: #FFFFFF;text-align: center;}    
  .sucDiv img{ width: 160px;margin-top: 90px;}
  .sucDiv p:nth-child(2){font-size: 17px;color: #E83428;margin-top: 15px;}
  .sucDiv p:nth-child(3){font-size: 13px;color: #707070;margin-top: 10px;}
</style>

<style>
  .registeredmembers{ background: url('../../static/credit/register_bgnew.png') no-repeat left bottom; background-size: 100% 100%; overflow: hidden; height:100%;width:100%}
  .registeredmembers .weui-label { width: 4.5rem !important;}
  .registeredmembers .weui-cells { border: 1px solid #dddddd; border-radius: 5px; margin-top:20px !important;position:relative;}
  .vux-popup-picker-header-menu-right { color: #e83428 !important; } 
  .weui-dialog__btn_primary{color:#e83428 !important;}
  .weui-cells:before{border-top:0 !important;}
  .weui-cells:after{border-bottom:0 !important;}
  .vux-cell-box:before{border-top:0 !important;}
  .vux-cell-box:after{border-bottom:0 !important;}
  .registeredmembers .weui-cell{font-size: 13px;color: #3e3e3e;height:46px}
  .registeredmembers .weui-cell__ft:after{display: none !important;}
  .registeredmembers .weui-vcode .weui-icon-clear:before{display: none !important;} 
  .registeredmembers .weui-cell{padding: 0 15px!important;}
  .registeredmembers .weui-input::-webkit-input-placeholder {/*placeholder颜色*/
    color: #bebebe;
  }
</style>
