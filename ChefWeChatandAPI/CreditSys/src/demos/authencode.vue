<template>
  <div class="authencode">
    
    <!--输入认证码-->
    <div class="authencode-wrap">
    	<span>认证码</span>
    	<input type="text" placeholder="请输入认证码" v-model="codeNum"/>
    	<button @click="testClick">验证</button>
    	<p class="prompt" v-show="promptFlag">
    		<img src="../../static/credit/authen_th.png" style="width: 13px;"/>
    	  <span>已超出次数限制，明天再来吧</span>
    	</p>
    </div>
    
    <!--队员信息-->
    <div class="teamInfo">
    	<!--定位成功且获取到队员信息-->
    	<div class="teamInfo_box" v-for="item in  dataList" v-show="showFlag1">
    		<div><img src="../../static/credit/authen_per.png"></div>
    		<div>
    			<p>{{item.ProvinceName}} {{item.CityName}} {{item.AreaName}}</p>
    			<p>{{item.Name}} {{item.Telephone}}</p>
    		</div>
    	</div>
    	<!--定位失败或没有获取到队员信息-->
    	<div v-show="showFlag2">
    		<p v-if="proFlag" style="margin-top: 10px;line-height: 40px;text-align: center;">正在获取信息，请稍等<img src="../../static/credit/loading.gif" style="width: 20px;position: relative;top: 5px;margin-left: 5px;"></p>
    		<p v-else style="margin-top: 10px;padding-left: 20px;">您当前区域没有业务代表哦，切换实名认证吧！</p>
    	</div>
    </div>
    <!--认证说明-->
    <div class="explain">
    	<p>认证说明</p>
    	<p>1、根据以上信息联系当区酒店业务员进行拜访认证；</p>
    	<p>2、经酒店业务员认证属实填写认证码验证即可</p>
    </div>
   
    
    <!--成功 失败提示-->
    <div class="mask" v-show="maskFlag">
    	<div v-if="sucFlag">
    		<p class="mask-ts">提示</p>
    		<p class="mask-suc">认证成功</p>
    		<button @click="jump">立即查看</button>
    	</div>
    	<div v-else>
    		<img src="../../static/credit/prizex.png" @click="closeClick"/>
    		<p class="mask-ts">提示</p>
    		<p class="mask-fail">认证失败</p>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { cookie,Toast,Loading } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'

export default{
	components:{
		cookie,
		Toast
	},
	data(){
		return{
			userData:{},
			userId:'',
			timestamp:'',
	  	nonceStr:'',
	  	signature:'',
	  	url:'',
			codeNum:'',//认证码
			testnum:'',//验证次数
			promptFlag:false,//提示验证次数超出
			maskFlag:false,//蒙版
			sucFlag:false,//成功或失败提示弹框
			dataList:[],
			showFlag1:false,
			showFlag2:true,//默认显示自动定位失败
			proFlag:true
		}
	},
	methods:{
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
					        'getLocation' //获取地理位置接口
					      ]
					    })
		  			  
		  			  wx.ready(function(){
                  wx.getLocation({
											type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
											success: function (res) {
												var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
												var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
												var speed = res.speed; // 速度，以米/每秒计
												var accuracy = res.accuracy; // 位置精度
//												alert('latitude:'+latitude);
//												alert('longitude:'+longitude);
												_this.getLocation(latitude,longitude);//根据经纬度获取省市区名称
											},
											fail:function(res){
                        _this.proFlag=false;//获取失败
											}
								  });
							});
//							wx.error(function (res) {
//									alert("调用微信jsapi返回的状态:"+res.errMsg);
//							});
		  			  
		  			  
		  		})		  		
	  	  })
    },
    getLocation(latitude,longitude){//根据经纬度获取省市区名称
    	var _this=this;
      $.ajax({
        url: 'http://apis.map.qq.com/ws/geocoder/v1/?location='+latitude+','+longitude+'&key=4MFBZ-LJMC3-NKG3O-YM3LT-OKQRV-PMBJA&output=jsonp',
        type: 'get',
        dataType: 'jsonp',
        processData: false, 
        data: {},
        success: function (data) {
//        alert(data.result.ad_info.province+','+data.result.ad_info.city+','+data.result.ad_info.district);
          var province=data.result.ad_info.province.slice(0,length-1);//省份  去掉'省'
          var city=data.result.ad_info.city.slice(0,length-1);//市  去掉'市'
          var district=data.result.ad_info.district;//区或县
          _this.findCity(province,city,district);//获取省市区id 
        }
      })
    },
    findCity(province,city,district){//获取省市区id 
    	var _this=this;
    	var params={
  		  'Province':province,
				'City':city,
				'Area':district
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.findCity,
  			data:params
  		}).then(function(response){
  			if(response.data!=null){
  				var data=JSON.parse(response.data);
				  _this.findSalemanByArea(data.Area[0].AreaId);//根据区id 获取该区域队员
  			}else{
  				_this.proFlag=false;//获取失败
  			}
  		})
    },
    findSalemanByArea(areaId){//根据区id 获取该区域队员
    	var _this=this;
  		this.$http({
  			method:'post',
  			url:apiUrl.findSalemanByArea+'?areaId='+areaId
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			if(data.length!=0){
  				//展示到队员列表
  				_this.dataList=data;
  				_this.showFlag1=true;//展示队员信息
					_this.showFlag2=false;
  			}else{
          _this.proFlag=false;//获取失败
  			}
  		})
    },
    bindRegistCode(){//认证码验证 返回-1验证失败 Exc success验证成功
    	var _this=this;
    	var params={
  		  'RegistCode':this.codeNum,
		  	'MemberId':this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.bindRegistCode,
  			data:params
  		}).then(function(response){
  			_this.$vux.loading.hide();//loading图标消失
				if(response.data=='Exc Success'){//成功
					_this.maskFlag=true;
					_this.sucFlag=true;
				}else{//失败
					_this.maskFlag=true;
					_this.sucFlag=false;
				}
				_this.testnum++;
				cookie.set('testNum', _this.testnum, {
				  expires: 1
				})
  		})
    },
		jump(){
			this.$router.push('/component/personal');
		},
		closeClick(){
			this.maskFlag=false;
		},
		testClick(){//点击验证testNum
			if(this.codeNum!=''){
				var str=/^[0-9]{6,}$/;//正则表达式 至少6位的数字
				this.showLoading () ;
				this.testnum=cookie.get('testNum',function(s){//验证次数
					return parseInt(s);
				})
				
				if(cookie.get('testNum')==undefined){//没有验证过
					this.testnum=0;
		 	    if(!str.test(this.codeNum)){//验证码错误
		 	    	this.$vux.loading.hide();//loading图标消失
		 	    	this.maskFlag=true;
					  this.sucFlag=false;
						this.testnum++;
						cookie.set('testNum', this.testnum, {
						  expires: 1
						})
		 	    }else{//验证成功 调接口
		 	    	this.bindRegistCode();
		 	    }
				}else if(this.testnum<3){//验证次数<3
          if(!str.test(this.codeNum)){//验证码错误
		 	    	this.$vux.loading.hide();//loading图标消失
		 	    	this.maskFlag=true;
					  this.sucFlag=false;
						this.testnum++;
						cookie.set('testNum', this.testnum, {
						  expires: 1
						})
		 	    }else{//验证成功 调接口
		 	    	this.bindRegistCode();
		 	    }					
				}else if(this.testnum>=3){//验证次数>=3
					this.$vux.loading.hide();//loading图标消失
					this.promptFlag=true;
				}
			}else{
				this.$vux.toast.text('认证码不能为空', 'middle');
			}
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
	  },
	  showLoading () {//loading 图标
      this.$vux.loading.show({
        text: 'Loading'
      })
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
	  this.userId=this.userData.UserId;
		var uri=window.location.href.split('#')[0];
	  this.url=encodeURIComponent(uri);
  	this.setConfig();//配置config
	}
}
</script>

<style scoped>
.authencode{height:100%;background: #fff;}
.authencode-wrap{padding: 6% 6% 3% 6%;}
.authencode-wrap span{color: #3e3e3e;font-size: 14px;}
.authencode-wrap input{background: #f4f4f4;border: none; width: 55%;height: 35px;border-radius: 5px; margin-left: 3%;margin-right:3%;text-indent: 1em;outline: none;}
.prompt{margin-top: 5px;margin-left: 16%;}
.prompt *{display:inline-block;vertical-align: middle;}
.prompt span{font-size: 12px;color: #E83428;}
.teamInfo{border-top: 3px solid #f4f4f4;border-bottom: 3px solid #f4f4f4;padding-bottom:10px;color: #757575;font-size: 13px;}
.teamInfo_box{display: flex;margin-top: 20px;margin-left: 20px;}
.teamInfo_box div:nth-child(1){width: 40px;margin-right: 20px;}
.teamInfo_box div:nth-child(1) img{width: 100%;}
.explain{margin-top: 20px;margin-left: 20px;}
.explain p:nth-child(1){font-size: 14px;color: #e83428;margin-bottom: 5px;}
.explain p:nth-child(2),p:nth-child(3){font-size: 13px;color: #3E3E3E;}
.authencode-wrap button{width:20%;height:35px;background: #ea2f28;color: #fff;font-size: 14px;border-radius: 5px;border: none;outline: none;position: relative;top: 1px;}
.mask{width: 100%;height: 100%;background: rgba(0,0,0,0.5);position: fixed;top: 0;}   
.mask div{width: 80%;margin-left: 10%;margin-top: 30%;background: #fff;border-radius: 5px;text-align: center;font-size: 14px;overflow: hidden;padding: 10px 0;}
.mask-ts{color: #333333;}
.mask-suc{color: #a6a6a6;margin-top: 15px;padding-bottom: 15px;border-bottom: 1px solid #f4f4f4;}
.mask-fail{color: #a6a6a6;margin-top: 15px;padding-bottom: 20px;}
.mask button{width: 40%;background: #e73229;color: #fff;border: none;height: 30px;border-radius: 5px;margin-top: 15px;}
.mask img{width: 25px;position: relative;left: 40%;}     
</style>
<style>

</style>