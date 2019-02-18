<template>
  <div class='signregister'>
  	 <div class="wdmicon">
    	<p>专业厨师服务平台</p>
    </div>  
    <div class="inputDiv">
			<group gutter='0.5rem'>
		      <x-input title="手机号" v-model="registerParams.TelePhone"  keyboard="number" is-type="china-mobile" ></x-input>
		  	</group>
		 	<group gutter='0.5rem'>
		  	  <x-input title="姓名" v-model="registerParams.MemberName"></x-input> 
		 	</group>
		 	<group gutter='0.5rem'>
		  	  <popup-picker title="岗位" :data="offer" :columns="2" v-model="registerParams.offerArr" show-name cancel-text='取消' confirm-text='确定' value-text-align	='left'></popup-picker>
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
	    	signsuccess:'',//签到成功页跳转到注册页标识
	    	activityId:'',//线下签到活动ID
	    	sucFlag:false, //显示注册成功页面
	    	userData:{},
  	 	  registerParams:{
  	 	  	"offerArr": [],//岗位
			    "MemberName": '',//姓名
			    "TelePhone": '',//电话
			    "HotelName": '',//酒店名称
			    "OpenId":'',
			    "NickName":'', 
			    "HeadImgUrl":''
			  },
        offer: []
	    }
	},
	methods: {
        getTele(){//验证手机号
      	  var _this=this;
	        var box=/^1[3|4|5|7|8]\d{9}$/;
		 	    if(!box.test(this.registerParams.TelePhone)){
		 	    	this.showPlugin ('请输入正确的手机号');
		 	    	flag=1;
		 	    }else{
	 	    	  this.$http({
	    	 	    method:'POST',
	    	 	    url:apiUrl.checkMemberTelephone,
	    	 	    data:{'TelePhone':this.registerParams.TelePhone}
	 	    	  }).then(function(respose){
	    	 	    if(respose.data!=0){
	    	 	   	  //不可以注册
	    	 	   	  _this.showPlugin ('该手机号已注册');
	    	 	   	  flag=1;
	    	 	    }else{
	    	 	    	btnNum++;
			        	if(btnNum==1){
			        		_this.showLoading() ;//显示loading
			        	  _this.submitApi();
			        	}
	    	 	    }
	 	    	  })
	 	    	}         
        },
        submit(){//提交 	
	      	if(this.registerParams.TelePhone==''){
	        	this.showPlugin ('手机号不能为空');
	        }else if(this.registerParams.MemberName==''){
	        	this.showPlugin ('姓名不能为空');
	        }else if(this.registerParams.offerArr==''){
	        	this.showPlugin ('岗位不能为空');
	        }else if(this.registerParams.HotelName==''){
	        	this.showPlugin ('餐厅名称不能为空');
	        }else if(this.registerParams.OpenId==''||this.registerParams.OpenId==undefined||this.registerParams.OpenId==null){
	        	this.showPlugin ('出现错误，请重新提交');
	        }else{
	        	this.getTele();//验证手机号
	        }
        },
        submitApi(){
	      	var _this=this; 
	      	var params = {
				    "MemberName": this.registerParams.MemberName ,			//成员名
				    "Phone": this.registerParams.TelePhone,        //手机号
				    "Position": this.registerParams.offerArr[1],			  //职位
				    "PositionType": this.registerParams.offerArr[0],	  //职位类别
				    "HotelName": this.registerParams.HotelName,         //酒店名称
				    "HeadImgUrl":this.registerParams.HeadImgUrl,
		  			"Nickname":this.registerParams.NickName,
  			  	"OpenId":this.registerParams.OpenId,
  			  	"Remark":"签到注册"
        	}
      		this.$http({
					 	method: 'POST',
					 	url: apiUrl.createSignMember,
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
         				//将会员id和类别存入cookie
                  _this.userData.UserId=data;
						      _this.userData.UserType=2;
						      var cookieValue = [];
									for(var i in _this.userData){
										cookieValue.push(i + "=" + _this.userData[i]);
									}
									cookieValue=cookieValue.join("&");
		          		_this.setCookie('WeiXinUser',cookieValue);
                  
			         		if(_this.signsuccess==0){//从签到页进入签到注册页
		           			 _this.addActivity(data);
		              }else{//正常情况下不会进入该判断
		              	_this.$vux.loading.hide();//loading图标消失
				           	setTimeout(function(){             	 	 
				         	 		_this.sucFlag=true;//提示注册成功页面
				         		},1) 
				         		setTimeout(function(){
					         	 	_this.$router.push('/component/personal');
					         	},1000) 
		              }
	         		}
		 			}).catch(function(err){
		 				_this.$vux.loading.hide();//loading图标消失
		 				setTimeout(function(){
	         	 	_this.showPlugin('提交信息失败,请重新提交');
	         	 	btnNum=0;
			      },1)   
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
        showPlugin (text) {//提醒样式
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
        setCookie(cname,cvalue,exdays){//设置cookie
				  var d = new Date();
				  d.setTime(d.getTime()+(exdays*24*60*60*1000));
				  var expires = "expires="+d.toUTCString();
				  document.cookie = cname + "=" + cvalue + "; " + expires;
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
        //获取openId   
        this.userData=cookie.get('WeiXinUser',function(val){
		      var a = val.split("&");
					var obj = {};
					for (var i = 0; i < a.length; i++) {
						var b = a[i].split("=");
						obj[b[0]] = b[1];
					}
					return obj;
				}) 
				this.registerParams.OpenId=this.userData.Openid;
				this.registerParams.NickName=decodeURI(this.userData.Nickname);
        this.registerParams.HeadImgUrl=this.userData.Headimgurl;
        
//      if(this.userData.UserId>0&&this.userData.UserType==2){
//      	this.$router.push('/component/personal');
//      }else if(this.userData.UserId>0&&this.userData.UserType==1){//已经是队员
//        //   	不能注册会员
//      	this.$router.push('/component/teammember');
//      }

        this.signsuccess=this.$route.query.signsuccess;
        this.activityId=this.$route.query.activityId;

	  	  // 设置岗位数据
	  	  this.offer = offer.offerArr;
	  	  // 设置 register 高度
	  	  $('.signregister').height(window.screen.height);
	  	  var height=window.screen.height;
	  	  $('.signregister .weui-cell,.signregister button').css({
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
  .wdmicon{height:20%;background: url('../../static/credit/register_icon.png') no-repeat center;background-size: 100% 100%;overflow: hidden;}
  .wdmicon p{text-align: center;color: #e73229;font-size: 22px;margin-top: 10%;letter-spacing: 1px;}
  .inputDiv{width: 90%;margin-left: 5%;}
  .signregister button{
  	width: 100%;
  	margin-top: 50px;
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
  .signregister{ background: url('../../static/credit/register_bgnew.png') no-repeat left bottom; background-size: 100% 100%; overflow: hidden; height:100%;width:100%}
  .signregister .weui-label { width: 4.5rem !important;}
  .signregister .weui-cells { border: 1px solid #dddddd; border-radius: 0.5rem; }
  .vux-popup-picker-header-menu-right { color: #e83428 !important; } 
  .weui-dialog__btn_primary{color:#e83428 !important;}
  .weui-cells:before{border-top:0 !important;}
  .weui-cells:after{border-bottom:0 !important;}
  .vux-cell-box:before{border-top:0 !important;}
  .vux-cell-box:after{border-bottom:0 !important;}
  .signregister .weui-cell{font-size: 13px;color: #3e3e3e;}
  .signregister .weui-cell__ft:after{display: none !important;}
  .signregister .weui-cell{padding: 0 15px!important;}
</style>
