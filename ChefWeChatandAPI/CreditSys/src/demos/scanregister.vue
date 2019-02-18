<template>
  <div class="scanregister">
    
    <div class="inputDiv">
    		<x-input title="联系方式" v-model="registerParams.Phone"  keyboard="number"  ></x-input>
    		<div style="position: relative;">
    			<x-input title="短信验证码" class="weui-vcode" v-model="identcode" style="margin-top: 16px;" ></x-input>  
	  			<span class='identcode' v-if="isShow" @click="getIdent">{{indentText}}</span>
	  			<span class='countNum' v-else>{{countDown}}秒</span>	
    		</div>
    		
    		<div class="box">
    			<div>
    				<button @click="selectType(1)" :class="{selectStyle:identity==1}">酒店从业者</button>
    		    	<span class="select" :class="{selectIcon:identity==1}"></span>
    			</div>
    			<div>
    				<button @click="selectType(2)" :class="{selectStyle:identity==2}">调味品供货商</button>
    		    	<span class="select" :class="{selectIcon:identity==2}"></span>
    			</div>
    			<div>
    				<button @click="selectType(3)" :class="{selectStyle:identity==3}">美食爱好者</button>
    		    <span class="select" :class="{selectIcon:identity==3}"></span>
    			</div>
    		</div>
    		
    </div>
    <button class="getbtn" @click="registerClick">领&nbsp;&nbsp;&nbsp;取</button>
    
    
  </div>
</template>

<script>
import { XInput,Toast,Loading,cookie } from 'vux' 
import apiUrl from '../apiUrls.js'
var btnNum = 0;
export default{
	components:{
		XInput,
		Toast,
		Loading,
		cookie
	},
	data(){
		return{
			isType:'',//提现or存入零钱
			money:'',//领取金额
			userData:{},
			registerParams:{
	        "Phone": '',        //手机
	        "OpenId":'',
	        "NickName":'', 
	        "HeadImgUrl":'' 
			},
	    identcode:'' ,// 输入的短信验证码
	    identcode1:'',// 返回的短信验证码
	    isShow:true,
			indentText:'获取验证码',
			countDown:60,
			identity:0//注册人身份 1酒店从业者 2调味品供货商  3美食爱好者
		}
	},
	methods:{
		selectType(val){//选择身份
			if(this.identity == 0 || val != this.identity){
				this.identity = val;
			}else if(val == this.identity){
				this.identity = 0;
			}
		},
		//获取验证码
    getIdent(){     	 
        var _this=this;
        var box=/^1[3|4|5|7|8]\d{9}$/;
	 	    if(!box.test(this.registerParams.Phone)){
            this.$vux.toast.text('请输入正确的手机号', 'middle');
            this.registerParams.Phone="";//手机号置空
	 	    }else{
 	    	  this.$http({
    	 	    method:'POST',
    	 	    url:apiUrl.checkMemberTelephone,
    	 	    data:{'TelePhone':this.registerParams.Phone}
 	    	  }).then(function(response){
    	 	    if(response.data!=0){
    	 	   	  //不可以注册
              _this.$vux.toast.text('该手机号已注册', 'middle');
              _this.registerParams.Phone="";//手机号置空
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
    registerClick(){//点击注册
    	if(this.registerParams.Phone == ""){
        this.$vux.toast.text('手机号不能为空', 'middle');
    	}else if(this.identcode == ""){
        this.$vux.toast.text('验证码不能为空', 'middle');
    	}else if(this.identcode != this.identcode1){
        this.$vux.toast.text('验证码输入有误', 'middle');
    	}else if(this.identity == 0){
        this.$vux.toast.text('请选择职业类型', 'middle');
    	}else{
        this.getTele();//验证手机号
    	}
    },
	  getTele(){//验证手机号
  	    var _this=this;
        var box=/^1[3|4|5|7|8]\d{9}$/;
	 	    if(!box.test(this.registerParams.Phone)){
          this.$vux.toast.text('请输入正确的手机号', 'middle');
	 	    }else{
 	    	  this.$http({
    	 	    method:'POST',
    	 	    url:apiUrl.checkMemberTelephone,
    	 	    data:{'TelePhone':this.registerParams.Phone}
 	    	  }).then(function(respose){
    	 	    if(respose.data!=0){
    	 	   	  //不可以注册
              _this.$vux.toast.text('该手机号已注册', 'middle');
    	 	    }else{
    	 	    	btnNum++;
			        if(btnNum == 1){
		            _this.showLoading();//显示loading
		          	_this.submitApi();//提交注册信息
			        }
    	 	    }
 	    	  })
	 	    }         
	  },
    submitApi(){//提交注册接口
    	var _this = this;
		  var identity = "";
			if(this.identity == 1){
				identity = "酒店从业者";
			}else if(this.identity == 2){
				identity = "调味品供货商";
			}else if(this.identity == 3){
				identity = "美食爱好者";
			}
			
			var params={
	  		  "OpenId": this.registerParams.OpenId,
					"Phone": this.registerParams.Phone,
					"JobType": identity,
					"Nickname":this.registerParams.NickName ,
					"HeadImgUrl":this.registerParams.HeadImgUrl,
					"Remark": "2L生抽扫码活动"
	  	}
  		this.$http({
  			method:'post',
				url:apiUrl.redPackCreate,
  			data:params
  		}).then(function(response){
  			var data = JSON.parse(response.data);
  			if(data.result_status == 'succ'){
  				//注册成功
  				var userId = JSON.parse(data.data);//会员id
  				//更新cookie
  				_this.userData.UserId=userId;
		      _this.userData.UserType=2;
		      var cookieValue = [];
					for(var i in _this.userData){
						cookieValue.push(i + "=" + _this.userData[i]);
					}
					cookieValue=cookieValue.join("&");
          _this.setCookie('WeiXinUser',cookieValue);
  				//跳转到领取页
  				if(_this.isType == 1){//提现
  					_this.getMoney();//调提现接口
  				}else if(_this.isType == 0){//存入零钱
            _this.$vux.loading.hide();//loading图标消失
  					_this.$router.push({path:'/component/scanreceive',query:{money:_this.money,isType:_this.isType}});
  				}
  			}else if(data.result_status == 'fail'){//失败
          _this.$vux.loading.hide();//loading图标消失
  				btnNum = 0;
          _this.$vux.toast.text('提交信息失败,请重新提交', 'middle');
  			}
  		})
    },
    getMoney(){//提现接口
			var _this = this;
			var params={
  			"OpenId": this.registerParams.OpenId
  		}
  		this.$http({
  			method:'post',
				url:apiUrl.redPackTiXian,
  			data:params
  		}).then(function(response){
  			console.log(JSON.parse(response.data));
        _this.$vux.loading.hide();//loading图标消失
  			var data = JSON.parse(response.data);
  			if(data.result_status == 'succ'){//提现成功
					_this.$router.push({path:'/component/scanreceive',query:{money:_this.money,isType:_this.isType}});
  			}else if(data.result_status == 'fail'){//提现失败 提示
//				_this.$vux.toast.text(data.message, 'middle');
//				btnNum = 0;
					_this.$router.push({path:'/component/scanreceive',query:{isType:2,message:data.message}});
  			}
  		})
		},
    setCookie(cname,cvalue,exdays){//设置cookie
			  var d = new Date();
			  d.setTime(d.getTime()+(exdays*24*60*60*1000));
			  var expires = "expires="+d.toUTCString();
			  document.cookie = cname + "=" + cvalue + "; " + expires;
		},
    onChange (val) {//提示框
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
	  },
	  showLoading () {//loading 图标
      this.$vux.loading.show({
        text: 'Loading'
      })
    }
	},
	mounted(){
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
//	    this.userId=parseInt(this.userData.UserId);
//	    this.userType=parseInt(this.userData.UserType);
	    this.registerParams.OpenId=this.userData.Openid;
	    this.registerParams.NickName=decodeURI(this.userData.Nickname);
      this.registerParams.HeadImgUrl=this.userData.Headimgurl;
      
      this.isType = this.$route.query.isType;//存入钱包0 提现1
      this.money = this.$route.query.money;//存入钱包0 提现1
	}
}
</script>

<style scoped>
.scanregister{background: url('../../static/credit/scan_back.png') no-repeat center;background-size:100% 100%;width: 100%;height: 100%;overflow: hidden;}
.inputDiv{width: 90%;margin-left: 5%;margin-top: 50%;}
.identcode,.countNum{position: absolute;right:10px;font-size: 14px;display:inline-block ;top:11px;color: #ff8a00;}
.inputDiv button{width: 80%;height:36px;background: #fff;border-radius: 5px;border: none;color: #ff8a00;outline: none;font-size: 15px;letter-spacing: 1px;}
.box{
		display: flex;
		justify-content: space-between;
		margin-top: 8%;
	}
	
	.box div{
		width: 33%;
		position: relative;
	}
	
	.box button{
		background: none;
		border: 1px solid #fff;
		color: #fff;
		text-align: center;
		font-size: 13px;
		line-height: 30px;
		border-radius: 5px;
		outline: none;
		width: 90%;
	}
	
  .selectStyle{
		border: 1px solid #f69c3b !important;
    color:#f69c3b !important;
  }
    	
	.select{
		display: inline-block;
		width: 18px;
		height:18px;
		background: #fff;
		border-radius: 50%;
		position: absolute;
		top: -7px;
		right: 3%;
	}
	
    .select::before{
   	  content: '';
	    position: absolute;
	    width: 5px;
	    height: 8px;
	    color: #ddd;
	    border-bottom: 1.5px solid;
	    border-right: 1.5px solid;
	    margin-left: 6px;
	    margin-top: 3px;
	    transform-origin: center;
	    transform:rotate(45deg);
	    -webkit-transform:rotate(45deg);
	}
	
	.selectIcon{
		background:#f69c3b;
	}
	
	.selectIcon::before{
	  color: #fff;
	}
.getbtn{width:45%;height:42px;margin-top: 15%;margin-left: 27%;background:#ff8a00;color: #fff;border-radius: 5px;border: none;outline: none;font-size: 16px;}
@media screen and (max-height:500px){
	.inputDiv{margin-top: 37%;}
}
</style>
<style>
  .scanregister .weui-label { width: 5.5rem !important;text-align: left;font-size: 14px;}
  .scanregister .weui-cells { border: 1px solid #dddddd; border-radius: 5px; margin-top:20px !important;position:relative;}
  .weui-cells:before{border-top:0 !important;}
  .weui-cells:after{border-bottom:0 !important;}
  .vux-cell-box:before{border-top:0 !important;}
  .vux-cell-box:after{border-bottom:0 !important;}
  .scanregister .weui-cell{font-size: 13px;color: #3e3e3e;height:46px;padding: 0 10px!important;border-radius: 5px;}
  .scanregister .weui-cell__ft:after{display: none !important;}
  .scanregister .weui-vcode .weui-icon-clear:before{display: none !important;} 
  .vux-x-input,.weui-cell{background: #fff;}
  .scanregister .weui-input{font-size: 14px;}
</style>