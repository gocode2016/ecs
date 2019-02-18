<template>
  <div class="authenname">
    
    <div class="authennameDiv">
    	  <div class="authen_box">
    			<div><img :src="userphoto1"  @click="imgUpload(1)"/></div>
    			<div><img :src="userphoto2"  @click="imgUpload(2)"/></div>
    	  </div>
    		<p class="text_style">以下二种组合任选一种上传</p>
    		<p class="text_style text_styles">一、</p>
    		<p class="authen_style">
    		  <img src="../../static/credit/authenname1.png" />
    		</p>
    		<p class="text_style text_styles">二、</p>
    		<p class="authen_style">
    		  <img src="../../static/credit/authenname2.png" />
    		</p>
    </div>
    <button @click="submitClick" v-show="btnFlag">提交认证</button>
    
  </div>
</template>

<script>
	import { Toast,cookie } from 'vux'
	import apiUrl from '../apiUrls.js'
	import wx from 'weixin-js-sdk'
	export default{
		components:{
			Toast,
			cookie
		},
		data(){
			return{
				btnNum:0,
				userData:{},
				userId:'',
				timestamp:'',
	  		nonceStr:'',
	  		signature:'',
	  		url:'',
	  		userphoto1:'../../static/credit/addphoto.png',
	  		userphoto2:'../../static/credit/addphoto.png',
	  		btnFlag:true,//按钮显示
	  		state:'',//当前状态 0新增信息 1修改信息
			}
		},
		methods:{
			submitClick(){//点击提交
				if(this.userphoto1=='../../static/credit/addphoto.png'||this.userphoto2=='../../static/credit/addphoto.png'){
					this.$vux.toast.text('上传图片后才能提交哦', 'middle');
				}else{
					this.btnNum++;
					if(this.btnNum==1){
						this.getMemberProfile();//获取简历信息 
					}
				}
			},
			realAuthAdd(val){//提交信息
				var _this=this;
				var params={
  		    'MemberId':this.userId,
					'AuthImg1':this.userphoto1,
					'AuthImg2':this.userphoto2,
					'AuthState':val,
					'Remark':''
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.realAuthAdd,
	  			data:params
	  		}).then(function(response){
	  			if(response.data=='Exc Success'){//成功 0
	  				if(val==-1){//认证失败
	  					_this.$router.push({path:'/component/authenexamine',query:{isSuccess:1}})
	  				}else if(val==0){//提交信息成功
	  					_this.$router.push({path:'/component/authenexamine',query:{isSuccess:0}})
	  				}
	  			}else{
	  				_this.$router.push({path:'/component/authenexamine',query:{isSuccess:1}})
	  			}
	  		})
			},
			getMemberAuth(){//获取信息
				var _this=this;
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getMemberAuth+'?memberId='+this.userId
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			if(data==null){//没有提交过信息
	  				_this.btnFlag=true;
	  				_this.state=0;//需要新增信息
	  			}else{
	  				if(data.AuthState==0||data.AuthState==1){//0 审核中 1 审核通过 
		  				//隐藏按钮 展示信息
		  				_this.userphoto1=data.AuthImg1;
		  				_this.userphoto2=data.AuthImg2;
		  				_this.btnFlag=false;
		  			}else if(data.AuthState==-1){//-1审核不通过
		  				_this.userphoto1=data.AuthImg1;
		  				_this.userphoto2=data.AuthImg2;
		  				_this.btnFlag=true;
		  				_this.state=1;//需要修改信息
		  			}
	  			}
	  		})
			},
			realAuthUpdate(val){//修改信息
				var _this=this;
				var params={
  		    'MemberId':this.userId,
					'AuthImg1':this.userphoto1,
					'AuthImg2':this.userphoto2,
					'AuthState':val,
					'Remark':''
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.realAuthUpdate,
	  			data:params
	  		}).then(function(response){
	  			if(response.data=='Exc Success'){//成功 0
	  				if(val==-1){//认证失败
	  					_this.$router.push({path:'/component/authenexamine',query:{isSuccess:1}})
	  				}else if(val==0){//提交信息成功
	  					_this.$router.push({path:'/component/authenexamine',query:{isSuccess:0}})
	  				}
	  			}else{
	  				_this.$router.push({path:'/component/authenexamine',query:{isSuccess:1}})
	  			}
	  		})
			},
			getMemberProfile(){//获取简历信息 岗位美食爱好者和供货商 认证失败
				var _this=this;
				var params={
				  'MemberId':this.userId
				}
				this.$http({
					method:'POST',
					url:apiUrl.getMemberProfile,
					data:params
				}).then(function(response){	
					var data=JSON.parse(response.data);
					if(data[0].Position == '美食爱好者' || data[0].Position == '调味品供货商'){
						//认证失败
						if(_this.state==0){//新增 
							_this.realAuthAdd(-1);//提交信息
						}else if(_this.state==1){//修改
							_this.realAuthUpdate(-1);//修改信息
						}
					}else{
						if(_this.state==0){//新增 
							_this.realAuthAdd(0);//提交信息
						}else if(_this.state==1){//修改
							_this.realAuthUpdate(0);//修改信息
						}
					}
				})
	  	},
			setConfig(){//配置config
        var _this=this;
		  	this.$http({
		  		method:'GET',
		  		url:apiUrl.getTimestampAndNonceStr
		  	}).then(function(response){
		  		var data=response.data;
		  		var dataArr=data.split('|');
		  		_this.timestamp=parseInt(dataArr[0]);//时间戳
		  		_this.nonceStr=dataArr[1];//随机字符串
		  		
		      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
		  		_this.$http({
		  			method:'GET',
		  			url:apiUrl.getJsapiTicketSignature+params
		  		}).then(function(response){
		  			 _this.signature=response.data;
		  			 wx.config({
					      debug: false,
					      appId: apiUrl.appId,
					      timestamp: _this.timestamp,
					      nonceStr: _this.nonceStr,
					      signature: _this.signature,
					      jsApiList: [
					        'chooseImage',
					        'uploadImage'
					      ]
					   })
		  		})		  		
	  	  })
      },
      imgUpload(val){//上传头像
     	  var localIds='';
     	  var _this=this;
     	  wx.chooseImage({
					  count: 1, // 默认9
					  sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
					  sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
					  success: function (res) {
					    localIds = res.localIds;				    
					    // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
					    // localId 可以用于微信手机版图片显示（目前PC版不可用）
					    wx.uploadImage({
							  localId:localIds[0] , // 需要上传的图片的本地ID，由chooseImage接口获得
							  isShowProgressTips: 1, // 默认为1，显示进度提示
							  success: function (res) {
							    var serverId = res.serverId; // 返回图片的服务器端ID
					        _this.$http({
					        	method:'GET',
					        	url:apiUrl.saveWeChatImage+'?serverId='+serverId
					        }).then(function(res){        
//					        	  var userphoto="https://s3-010-shinho-ecshool-uat-bjs.s3.cn-north-1.amazonaws.com.cn"+res.data;//测试
					        	var userphoto="https://s3-010-shinho-ecshool-prd-bjs.s3.cn-north-1.amazonaws.com.cn"+res.data;//正式
					        	if(val==1){//左图
					        		_this.userphoto1=userphoto;
					        	}else if(val==2){//右图
					        		_this.userphoto2=userphoto;
					        	}
					        })
							  }
					    })
					  },
					  fail:function(res){
//					  	alert(JSON.stringify(res));
					  }
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
	  	this.userId=Number(this.userData.UserId);
	  	
	  	this.getMemberAuth();//获取认证信息
	  	
	  	var uri=window.location.href.split('#')[0];
		  this.url=encodeURIComponent(uri);
	  	this.setConfig();//配置config
		}
	}
</script>

<style scoped>
.authenname{background: #fff;border: 1px solid #fff;box-sizing: border-box;}
.authennameDiv{width: 90%;margin-left: 5%;background: #f8f8f8;border-radius: 5px;padding: 20px 0;margin-top: 20px;}
.authen_box {display: flex;}
.authen_box div{width: 40%;height: 100px;margin: 0 4.5%;}
.authen_box div img{width: 100%;height: 100%;}
.text_style{font-size: 13px;color: #3E3E3E;margin-left: 4.5%;margin-top: 30px;}
.text_styles{margin: 10px 4.5% !important;}
.authen_style{text-align: center;}
.authen_style img{width: 90%;}
button{width: 90%;height: 40px;background: #E83428;border-radius: 5px;color: #fff;margin-left: 5%;border: none;margin-top: 50px;margin-bottom: 30px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>