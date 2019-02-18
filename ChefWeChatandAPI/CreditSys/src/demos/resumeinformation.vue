<template>
  <div class="resumeinformation">
  	
  	<div class="inforDiv">  		
	  	<p class="infor_text">完善此板块信息，您将获得15积分的奖励！</p>
	  	<cell title="头像"  is-link ><img class="userphoto" :src="memberphoto" @click='imgUpload'></cell>
		  <popup-picker title="性别" :data="sexlist" v-model="sex" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="民族" :data="nationlist" v-model="nation" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <datetime     title="生日" v-model="timeText" :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align ='right'></datetime>
		  <popup-picker title="性格" :data="characterlist" v-model="character" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <x-input title="联系方式" placeholder="请输入" v-model="telephone"></x-input>
		  <popup-picker title="家庭区域" :data="addresslist" v-model="homeAddress" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <x-input title="家庭地址" placeholder="请输入" v-model="homeaddress"></x-input>
		  <cell  title="业余爱好"  @click.native="select" is-link  v-model="hobby"></cell>
		  <popup-picker title="婚姻状况" :data="maritallist" v-model="marital" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="普通话水平" :data="speaklist" v-model="speak" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="在职情况" :data="isjoblist" v-model="isjob" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  
		  <div v-transfer-dom>
	      <popup v-model="show1">
	        <popup-header
	        left-text="取消"
	        right-text="确定"
	        :show-bottom-border="false"
	        @on-click-left="show1 = false"
	        @on-click-right="show1 = false" ></popup-header>
	        <group gutter="0">
	          <checklist  label-position="left" :options="hobbylist"  @on-change="change" ></checklist>
	        </group>
	      </popup>
      </div>
		  
  	</div>
  	<x-button @click.native="saveInfo">保存</x-button>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
import { Cell,XInput,PopupPicker,Datetime,XButton,PopupHeader, Popup, TransferDom, Group, Checklist ,cookie,Toast } from 'vux'
export default {
	directives: {
    TransferDom
  },
  components: {
    Cell,
    XInput,
    PopupPicker,
    Datetime,
    XButton,
    PopupHeader,
    Popup,
    Group,
    Checklist,
    cookie,
    Toast
  },
  data(){
  	return{
  		btnNum:0,
  		isSave:'',//信息是否保存过  0未保存过 1保存过
  		url:'',
  		userData:{},
  		memberInfo:{},
  		show1:false,
  		timestamp:'',
  		nonceStr:'',
  		signature:'',
  		latitude:'',//纬度
  		longitude:'',//经度
  		userId:'',
  		memberphoto:'../../static/credit/perimg.png',
  		userphoto:'',
  		username:'',
  		telephone:'',
  		homeaddress:'',
  		sexlist: [['男', '女']],
  		sex:[],
  		nationlist:[['汉族','壮族','回族','满族','维吾尔族','苗族','彝族','土家族','藏族','蒙古族','侗族','布依族','瑶族','白族','朝鲜族','哈尼族','黎族','哈萨克族','傣族','畲族','傈僳族',
  		'东乡族','仡佬族','拉祜族','佤族','水族','纳西族','羌族','土族','仫佬族','锡伯族','柯尔克孜族','景颇族','达斡尔族','撒拉族','布朗族','毛南族','塔吉克族','普米族','阿昌族','怒族','鄂温克族',
  		'京族','基诺族','德昂族','保安族','俄罗斯族','裕固族','乌孜别克族','门巴族','鄂伦春族','独龙族','赫哲族','高山族','珞巴族','塔塔尔族']],
  		nation:[],
  		timeText:'',
  		characterlist:[['内向','外向']],
  		character:[],
  		addresslist:[],
  		homeAddress:[],
  		hobbylist:['登山','音乐','阅读','电影','摄影','书法','旅游','聚会','棋牌','绘画','网络游戏','健身','钓鱼','足球','羽毛球','茶道'],
  		hobby:'',
  		maritallist:[['已婚','未婚']],
  		marital:[],
  		speaklist:[['一般','良好','熟练']],
  		speak:[],
  		isjoblist:[['在职','待业']],
  		isjob:[],
  		num1: 0,
	    num2: 0,
	    num3: 0,
	    provinceList: [],
			cityList: [],
			areaList: [],
			ProvinceId :0,
		  ProvinceName:'',
		  CityId :0,
		  CityName:'',
		  AreaId :0,
		  AreaName :''  
  	}
  },
  methods:{
  	updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':15,
				'MemberId':this.userId,
				'IntegralSource':'简历完善',
				'Remark':'基本信息'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
//			console.log(response);
  		})
  	},
  	select(){
  		this.show1=true;
  	},
  	change (val) {
//    console.log('change', val);
      var hobby=val.join();
      this.hobby=hobby;
    },
    getRegion(){ //获取省市区数据
				var _this = this;
				this.$http({
					method: 'get',
					url: apiUrl.getRegion
				}).then(function(response) {
					var data = JSON.parse(response.data);
					//省
					var provinceList = data.Province;
					_this.num1 = provinceList[provinceList.length - 1].ProvinceId;
					for(let i in provinceList) {
						_this.addresslist.push({
							name: provinceList[i].ProvinceName.toString(),
							value: provinceList[i].ProvinceId.toString(),
							parent: '0'
						})
					}
					_this.provinceList = provinceList;

					//市
					var cityList = data.City;
					_this.num2 = cityList[cityList.length - 1].CityId;
					for(let i in cityList) {
						var idx = cityList[i].CityId + _this.num1;
						_this.addresslist.push({
							name: cityList[i].CityName.toString(),
							value: idx.toString(),
							parent: cityList[i].ProvinceId.toString(),
						})
					}
					_this.cityList = cityList;

					//区
					var areaList = data.Area;
					for(let i in areaList) {
						var idx = areaList[i].AreaId + _this.num2 + _this.num1;
						var idx2 = areaList[i].CityId + _this.num1;
						_this.addresslist.push({
							name: areaList[i].AreaName.toString(),
							value: idx.toString(),
							parent: idx2.toString(),
							areaLevel: areaList[i].AreaLevel
						})
					}
					_this.areaList = areaList;
	  			_this.getMemberProfile();//获取基本信息
				})
			},
      switchAddress() {
      	// 已获取的后台省市区数据
      	var provinceList = this.provinceList;
      	var cityList = this.cityList;
      	var areaList = this.areaList;
      	// 将前台省市区联动数据转换回后台省市区数据
      	// 转换省市区id
      	var provinceId = Number(this.homeAddress[0]) - 0;
      	var cityId = Number(this.homeAddress[1]) - this.num1;
      	var areaId = Number(this.homeAddress[2]) - this.num2 - this.num1;
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

		    this.ProvinceId = provinceId;
		    this.ProvinceName = provinceName;
		    this.CityId = cityId;
		    this.CityName = cityName;
		    this.AreaId = areaId;
		    this.AreaName = areaName;
     	  this.submitApi();//提交基本信息
      },
      setConfig(){//配置config
        var _this=this;
        var localIds ='';
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
					        'chooseImage',//选择图片
					        'uploadImage',//上传图片
					        'getLocation' //获取地理位置接口
					      ]
					  })
		  			 
		  		  wx.ready(function(){	 
			  			wx.getLocation({
									type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
									success: function (res) {
										_this.latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
									  _this.longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
									},
									fail:function(res){
	           				//获取失败
									}
						  })
		  			});
		  			
		  		})		  		
	  	  })
      },
      getLocation(latitude,longitude){//根据经纬度获取省市区名称
	    	var _this=this;
	      $.ajax({
	        url: 'http://apis.map.qq.com/ws/geocoder/v1/?location='+latitude+','+longitude+'&key=4MFBZ-LJMC3-NKG3O-YM3LT-OKQRV-PMBJA&output=jsonp&get_poi=1',
	        type: 'get',
	        dataType: 'jsonp',
	       	contentType:'application/json; charset=utf-8',
	        data: {},
	        success: function (data) {
            var province = data.result.address_component.province.slice(0,length-1);//省
            var city = data.result.address_component.city.slice(0,length-1);//市
            var district = data.result.address_component.district.slice(0,length-1);//区
//          var street = data.result.address_component.street_number;//具体地址
            
            for(let i in _this.provinceList) {
		      		if(_this.provinceList[i].ProvinceName.indexOf(province) != -1){
		      			var ProvinceId = _this.provinceList[i].ProvinceId.toString();
		      		}
		      	}
		      	for(let i in _this.cityList) {
							if(_this.cityList[i].CityName.indexOf(city) != -1) {
		      			var CityId = (_this.cityList[i].CityId + 34).toString();
							}
		      	}
		      	for(let i in _this.areaList) {
							if(_this.areaList[i].AreaName.indexOf(district) != -1) {
		      			var AreaId = (_this.areaList[i].AreaId + 34 + 358).toString();
							} 
		      	}
		      	_this.homeAddress = [ProvinceId,CityId,AreaId];//当前省市区
//          _this.homeaddress = street;//当前家庭地址
	        }
	      })
	    },
      imgUpload(){//点击头像 选择上传图片
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
	//								    _this.memberphoto=localIds[0];
					    wx.uploadImage({
							  localId:localIds[0] , // 需要上传的图片的本地ID，由chooseImage接口获得
							  isShowProgressTips: 1, // 默认为1，显示进度提示
							  success: function (res) {
							    var serverId = res.serverId; // 返回图片的服务器端ID
					        _this.$http({
					        	method:'GET',
					        	url:apiUrl.saveWeChatImage+'?serverId='+serverId
					        }).then(function(res){        
//					        	_this.userphoto=apiUrl.getUrl+res.data;//测试
					        	_this.userphoto="https://s3-010-shinho-ecshool-prd-bjs.s3.cn-north-1.amazonaws.com.cn"+res.data;//正式
					        	_this.memberphoto=_this.userphoto;
					        })
							  }
					    })
					  },
					  fail:function(res){
//					  	alert(JSON.stringify(res));
					  }
			 })						 
     },
     saveInfo(){//更新基本信息
     	  var _this=this;
     	  if (this.userphoto==''||this.sex==''||this.nation==''||this.timeText==''||this.character==''){
     	 	  this.$vux.toast.text('内容不能为空', 'middle');
     	  }else if(this.telephone==''){
     	 	  this.$vux.toast.text('内容不能为空', 'middle');
     	  }else if(this.homeAddress==''||this.homeaddress==''||this.hobby==''||this.marital==''||this.speak==''||this.isjob==''){
     	 	  this.$vux.toast.text('内容不能为空', 'middle');
     	  }else{//提交信息
     	  	this.btnNum++;
     	  	if(this.btnNum==1){
     	 	    this.switchAddress();//获取省市区参数
     	  	}
     	  } 	 
     },
     submitApi(){//提交基本信息
     			var _this=this;
     	    var sex = '', isjob = '', ismarry = '';
     	 	  if(this.sex[0]=='女'){//0
     	 	  	sex=0;
     	 	  }else{
     	 	  	sex=1;
     	 	  }
     	 	  if(this.isjob[0]=='待业'){//0
     	 	  	isjob=0;
     	 	  }else{
     	 	  	isjob=1;
     	 	  }
     	 	  if(this.marital[0]=='未婚'){//0
     	 	  	ismarry=0;
     	 	  }else{
     	 	  	ismarry=1;
     	 	  }
	     	 	var params={
	     			"MemberId":this.userId,
						"ImgUrl":this.userphoto,
						"MemberTelePhone":this.telephone,
						"Sex":sex,
						"Nation":this.nation[0],
						"Birthday":this.timeText,
						"Nature":this.character[0],
						"ProvinceId":this.ProvinceId,
						"ProvinceName":this.ProvinceName,
						"CityId":this.CityId,
						"CityName":this.CityName,
						"AreaId":this.AreaId,
						"AreaName":this.AreaName,
						"HomeAddress":this.homeaddress,
						"Hobbys":this.hobby,
						"IsMarry":ismarry,
						"ChineseLv":this.speak[0],
						"IsJob":isjob
	     	  }
	     	  this.$http({
	     	 	  method:'POST',
	     	 	  url:apiUrl.updateMemberInfo,
	     	 	  data:params
	     	  }).then(function(response){
            if(response.data=="Exc Success"){
		     	  	if(_this.isSave==0 && _this.btnNum==1){//没有保存过  进行保存
		     	  		_this.$vux.toast.text('保存成功,您已增加15积分','middle');
		     	  		_this.updateMemberIntegral();//更改积分
		     	  	}else{
		     	  		_this.$vux.toast.text('保存成功','middle');
		     	  	}
				      setTimeout(function(){
				      	_this.$router.push('/component/resume');
				      },500)
			      }else{
		     	  	_this.$vux.toast.text('保存失败','middle');
		     	  	_this.btnNum=0;
			      }
	   	    })
     },
     getMemberProfile(){//获取简历信息 
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
				if(data.length!=0){
					_this.memberInfo=data[0];
//				  console.log(_this.memberInfo);
	        if(_this.memberInfo.ImgUrl!=null&&_this.memberInfo.ImgUrl!=""){//头像
	        	_this.memberphoto=_this.memberInfo.ImgUrl;
	        	_this.userphoto=_this.memberInfo.ImgUrl;
	        }
	        _this.username=_this.memberInfo.MemberName;//名字
					if(_this.memberInfo.Sex==0){//性别
						_this.sex=['女'];
					}else if(_this.memberInfo.Sex==1){
						_this.sex=['男'];
					}else{
						_this.sex=[];
					}
					
					if(_this.memberInfo.Nation==null||_this.memberInfo.Nation==""){//民族
						_this.nation=[];
						_this.isSave=0;//没有保存过
					}else{
						_this.nation=[_this.memberInfo.Nation];
	        	_this.isSave=1;//保存过
					}
					
					if(_this.memberInfo.Birthday==null){//生日
						_this.timeText="";
					}else{
						_this.timeText=_this.memberInfo.Birthday.substring(0,10);
					}
					_this.memberInfo.Nature==null||_this.memberInfo.Nature=="" ? _this.character=[] : _this.character=[_this.memberInfo.Nature];//性格
					_this.telephone=_this.memberInfo.MemberTelePhone;//联系方式
					
					//家庭区域
					if(_this.memberInfo.HomeProvinceId == null){//未填写省市区
						if(_this.latitude==''){//没有成功获取到经纬度
							_this.homeAddress=[0,0,0];
            }else{
	            _this.getLocation(_this.latitude,_this.longitude);//自动定位省市区 
            }
					}else{
						var HomeProvinceId=(_this.memberInfo.HomeProvinceId+0).toString();
						var HomeCityId=(_this.memberInfo.HomeCityId+34).toString();
						var HomeAreaId=(_this.memberInfo.HomeAreaId+34+358).toString();
						_this.homeAddress=[HomeProvinceId,HomeCityId,HomeAreaId];//家庭区域
					}
					_this.homeaddress=_this.memberInfo.HomeAddress;//家庭地址
					_this.hobby=_this.memberInfo.Hobbys;//业余爱好
					if(_this.memberInfo.IsMarry==0){//婚姻状况
						_this.marital=['未婚'];
					}else if(_this.memberInfo.IsMarry==1){
						_this.marital=['已婚'];
					}else{
						_this.marital=[];
					}
					_this.memberInfo.ChineseLv==null||_this.memberInfo.ChineseLv=="" ? _this.speak=[] : _this.speak=[_this.memberInfo.ChineseLv];//普通话水平
					if(_this.memberInfo.IsJob==0){//在职情况
						_this.isjob=['待业'];
					}else if(_this.memberInfo.IsJob==1){
						_this.isjob=['在职'];
					}else{
						_this.isjob=[];
					}
				}else{
					_this.isSave=0;//没有保存过
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
  	$('#vux_view_box_body').css('background','#fff');
  	
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
  },
  created(){
		this.setConfig();
  	this.getRegion();//获取省市区
  }
}
</script>

<style scoped>
.inforDiv{margin-left: 15px;overflow-x:hidden;}
.infor_text{color: #E83428;font-size: 12px;margin-top: 15px;}
.userphoto{width: 48px;height: 48px;border-radius: 50%;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumeinformation .weui-cell:before{border-top: 0px !important;}
.resumeinformation .vux-cell-box:before{border-top: 0px !important;}
.resumeinformation .weui-cell{padding: 10px 18px 10px 0 !important;border-bottom: 1px solid #c8c7cc;}
.resumeinformation .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
.resumeinformation .vux-label {font-size: 15px;color: #3e3e3e;}
.resumeinformation .weui-label {font-size: 15px;color: #3e3e3e;}
.resumeinformation .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;margin-bottom:30px}
.resumeinformation .weui-btn:after {border: none;}
.vux-popup-picker-header-menu-right { color: #e83428 !important; }
.dp-header .dp-item.dp-right { color: #e83428 !important; }
.resumeinformation .weui-input {text-align: right;font-size: 12px;color: #a3a3a3;}
.resumeinformation .weui-icon-clear:before{display:none;}
.v-transfer-dom .weui-cells__title + .weui-cells{height: 14rem;overflow-y:auto;}
.vux-popup-header-right{color: #E83428 !important;}
.weui-cells_checkbox .weui-check:checked + .vux-checklist-icon-checked:before{color: #E83428 !important;}
.resumeinformation .vux-datetime {color: #3e3e3e;font-size: 15px;}
.resumeinformation .weui-label{font-size: 14px !important;color: #3E3E3E !important;}
.resumeinformation .weui-input,.resumeinformation .vux-popup-picker-value,.resumeinformation .weui-cell__ft{font-size: 14px;color: #3E3E3E;}

</style>