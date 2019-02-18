<template>
	<div class="resumehotel">

		<div class="hotelDiv">
			<p class="infor_text">完善此板块信息，您将获得20积分的奖励！</p>
			<x-input title="真实姓名" placeholder="请输入" v-model="username"></x-input>
			<popup-picker title="工作岗位" :data="offerlist" :columns="2" v-model="offer" @on-change="offerShow" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
			<!--酒店会员-->
			<div v-show="offerId==1">
				<popup-picker title="酒店区域" :data="addresslist" v-model="hotelarea" @on-change="getCity" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
				<x-input title="酒店名称" v-model="hotelName"></x-input>
				<x-input title="酒店地址" placeholder="请输入" v-model="hoteladdress"></x-input>
				<popup-picker title="主营类型" :data="hotelselllist" v-model="hotelSell" value-text-align='right'></popup-picker>
				<popup-picker title="酒店人均消费" :data="hotelSpendList" v-model="hotelSpend" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
				<popup-picker title="现在使用欣和产品" :data="IfUseList" v-model="IfUse" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
				<popup-picker title="调味品采购权" :data="productSourceList" v-model="productSource" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
			</div>
			<!--美食爱好者-->
			<div v-show="offerId==2">
				<popup-picker title="是否有孩子" :data="IfUseList" v-model="HaveChild" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
				<popup-picker title="您的家庭收入" :data="HomeIncomeList" v-model="HomeIncome" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
				<popup-picker title="您的烹饪频率" :data="CookieNumList" v-model="CookieNum" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
			</div>
			<!--调味品供货商-->
			<div v-show="offerId==3">
				<x-input title="所在批发市场名称" v-model="MarketsaleName"></x-input>
				<popup-picker title="门店区域" :data="addresslist" v-model="hotelarea" @on-change="getCity" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
				<x-input title="门店名称" v-model="hotelName"></x-input>
				<x-input title="门店地址" placeholder="请输入" v-model="hoteladdress"></x-input>
				<popup-picker title="门店经营范围" :data="MarketScopeList" v-model="MarketScope" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
			</div>
  	        <x-button @click.native="saveClick">保存</x-button>
		</div>

	</div>
</template>

<script>
	import { Cell, XInput, PopupPicker, Datetime, XButton, PopupHeader, Popup, TransferDom, Group, Checklist, cookie, Toast, WechatPlugin } from 'vux'
	import offer from '../offer.js'
	import apiUrl from '../apiUrls.js'
	import wx from 'weixin-js-sdk'
	export default {
		components: {
			Cell,
			XInput,
			PopupPicker,
			Datetime,
			XButton,
			PopupHeader,
			Popup,
			TransferDom,
			Group,
			Checklist,
			cookie,
			Toast,
			WechatPlugin
		},
		data() {
			return {
				timestamp:'',
  				nonceStr:'',
  				signature:'',
  				url:'',
  				latitude:'',//纬度
  				longitude:'',//经度
				btnNum:0,
				isUpdate:0,//0可以修改 1不能修改 酒店信息
				isSave:'',//酒店信息是否保存过
				userData:{},
				userId:'',
				source:'',//来源 商城试用商品详情页
				skudata:{},
				addressId:'',
				productType:'',
				count:'',
				selectArr:[],
				prize:'',
				orderId:'',
				skuId:'',
				skuName:'',
				productId:'',
				total:'',
				num1: 0,
			    num2: 0,
			    num3: 0,
			    provinceList: [],
				cityList: [],
				areaList: [],
				offerId:1,//默认显示 酒店会员信息
				memberInfo:{},//获取信息集合
				username: '', //姓名
				offerlist: [],
				offer: [], //岗位
				addresslist: [],
				hotelarea: [], //酒店区域 门店区域
				hoteladdress: '', //酒店地址 门店地址
				hotelName:'',//门店名称
				hotelSell:[],//主营类型
				hotelSpendList:[],
				hotelSpend: [], //酒店人均消费
				IfUse: [], //是否使用产品
				HaveChild: [], //是否有孩子
				HomeIncome:[],//家庭收入
				CookieNum:[],//烹饪频率
				MarketsaleName:'',//所在批发市场名称
				MarketScope:[],//门店经营范围
				MarketScopeList: [
					[{
						name: '菜市场摊点',
						value: '菜市场摊点'
					}, {
						name: '果蔬店',
						value: '果蔬店'
					}, {
						name: '粮油店',
						value: '粮油店'
					}, {
						name: '农村大集摊点',
						value: '农村大集摊点'
					}, {
						name: '肉食店',
						value: '肉食店'
					}, {
						name: '调味料店',
						value: '调味料店'
					}]
				],
				HomeIncomeList: [
					[{
						name: '小于5万',
						value: '小于5万'
					}, {
						name: '5万-10万',
						value: '5万-10万'
					}, {
						name: '10万-20万',
						value: '10万-20万'
					}, {
						name: '20万以上',
						value: '20万以上'
					}]
				],
				CookieNumList: [
					[{
						name: '每天1次及以上',
						value: '每天1次及以上'
					}, {
						name: '每周1-6次',
						value: '每周1-6次'
					}, {
						name: '每月1-3次',
						value: '每月1-3次'
					}, {
						name: '从不',
						value: '从不'
					}]
				],
				IfUseList: [//是否使用产品
				    [{
						name: '是',
						value: '1'
					}, {
						name: '否',
						value: '0'
					}]
				], 
				productSource:[],//调味品采购权
				productSourceList: [
					[{
						name: '决策人',
						value: '决策人'
					}, {
						name: '执行人',
						value: '执行人'
					}, {
						name: '不参与',
						value: '不参与'
					}]
				],
				hotelselllist: [//主营类型数据
					[{
						name: '家常菜馆',
						value: '家常菜馆'
					}, {
						name: '商务宴会',
						value: '商务宴会'
					}, {
						name: '婚宴',
						value: '婚宴'
					}, {
						name: '火锅',
						value: '火锅'
					}, {
						name: '烧烤',
						value: '烧烤'
					}, {
						name: '外卖',
						value: '外卖'
					}, {
						name: '食堂',
						value: '食堂'
					}, {
						name: '私房菜',
						value: '私房菜'
					}, {
						name: '大排档',
						value: '大排档'
					}, {
						name: '航空铁路配餐',
						value: '航空铁路配餐'
					}, {
						name: '会所',
						value: '会所'
					}, {
						name: '其他',
						value: '其他'
					}]
				],
				hotelSpend1: [
					[{
						name: '300元及以上',
						value: '300元及以上'
					}, {
						name: '150-299元',
						value: '150-299元'
					}, {
						name: '80-149元',
						value: '80-149元'
					}, {
						name: '50-79元',
						value: '50-79元'
					}, {
						name: '<50元',
						value: '<50元'
					}]
				],
				hotelSpend2: [
					[{
						name: '300元及以上',
						value: '300元及以上'
					}, {
						name: '120-299元',
						value: '120-299元'
					}, {
						name: '60-119元',
						value: '60-119元'
					}, {
						name: '40-59元',
						value: '40-59元'
					}, {
						name: '<40元',
						value: '<40元'
					}]
				],
				hotelSpend3: [
					[{
						name: '300元及以上',
						value: '300元及以上'
					}, {
						name: '80-299元',
						value: '80-299元'
					}, {
						name: '40-79元',
						value: '40-79元'
					}, {
						name: '20-39元',
						value: '20-39元'
					}, {
						name: '<20元',
						value: '<20元'
					}]
				],
				hotelSpend4: [
					[{
						name: '300元及以上',
						value: '300元及以上'
					}, {
						name: '60-299元',
						value: '60-299元'
					}, {
						name: '40-59元',
						value: '40-59元'
					}, {
						name: '20-39元',
						value: '20-39元'
					}, {
						name: '<20元',
						value: '<20元'
					}]
				],
			}
		},
		methods: {
			getRegion() { //获取省市区数据
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
					_this.getMemberProfile();//获取酒店信息
					
				})
			},
			switchAddress() {
				// 已获取的后台省市区数据
				var provinceList = this.provinceList;
				var cityList = this.cityList;
				var areaList = this.areaList;
				// 将前台省市区联动数据转换回后台省市区数据
				// 转换省市区id
				var provinceId = Number(this.hotelarea[0]) - 0;
				var cityId = Number(this.hotelarea[1]) - this.num1;
				var areaId = Number(this.hotelarea[2]) - this.num2 - this.num1;
				// 获取名称
				var provinceName = '',
					cityName = '',
					areaName = '';
				for(let i in provinceList) {
					if(provinceList[i].ProvinceId == provinceId) {
						provinceName = provinceList[i].ProvinceName;
					}
				}
				for(let i in cityList) {
					if(cityList[i].ProvinceId == provinceId && cityList[i].CityId == cityId) {
						cityName = cityList[i].CityName;
					}
				}
				for(let i in areaList) {
					if(areaList[i].CityId == cityId && areaList[i].AreaId == areaId) {
						areaName = areaList[i].AreaName;
					}
				}
				this.ProvinceId = provinceId;
				this.ProvinceName = provinceName;
				this.CityId = cityId;
				this.CityName = cityName;
				this.AreaId = areaId;
				this.AreaName = areaName;
				
				//提交酒店信息
				this.btnNum++;
				if(this.btnNum==1){
					this.updateHotelInfo();//提交酒店信息
				}
				
			},
			getCity(value) {//验证城市接口获取人均消费
//					console.log(this.num2,this.num1);//358 34
				//区id
				var areaId = Number(this.hotelarea[2]) - this.num2 - this.num1;
				for(let i in this.areaList) {
					var arealist = this.areaList[i];
					if(areaId == arealist.AreaId) {
						var areaLevel = arealist.AreaLevel;
					}
				}
				// 调取验证城市接口，获取人均消费数组
//				console.log(areaLevel);
//				console.log(this.areaList);
				if(areaLevel == '一级') {
					this.hotelSpendList = this.hotelSpend1;
				} else if(areaLevel == '二级') {
					this.hotelSpendList = this.hotelSpend2;
				} else if(areaLevel == '三级') {
					this.hotelSpendList = this.hotelSpend3;
				} else if(areaLevel == '四级') {
					this.hotelSpendList = this.hotelSpend4;
				}
			},
			offerShow(value) {// 判定工作和岗位
				if(this.offer[1] == '美食爱好者') {
					this.offerId = 2;
				} else if(this.offer[1] == '调味品供货商') {
					this.offerId = 3;
				} else {
					this.offerId = 1;
					this.getCity();//获取人均消费
				}
			},
			saveClick(){//点击保存按钮
				if(this.offerId == 1 && this.isUpdate == 0) {
					if(this.username == ''||this.offer == ''||this.hotelarea == ''||this.hotelName==''||this.hoteladdress == ''||this.hotelSell == ''||this.hotelSpend== ''||this.IfUse == ''||this.productSource == ''){
     	 	  			this.$vux.toast.text('内容不能为空', 'middle');
					}else{
          				this.switchAddress();
          			}
				}else if(this.offerId == 2 && this.isUpdate == 0) { //美食爱好者
					if(this.username == ''||this.offer == ''||this.HaveChild == ''||this.HomeIncome == ''||this.CookieNum == ''){
     	 	  			this.$vux.toast.text('内容不能为空', 'middle');
					}else{
          				this.switchAddress();
          			}
				}else if(this.offerId == 3 && this.isUpdate == 0) { //调味品供货商
					if(this.username == ''||this.offer == ''||this.MarketsaleName==''||this.hotelarea == ''||this.hotelName==''||this.hoteladdress == ''||this.MarketScope==''){
     	 	  			this.$vux.toast.text('内容不能为空', 'middle');
					}else{
          				this.switchAddress();
          			}
				}else{
     	 	  		this.$vux.toast.text('认证信息审核中', 'middle');
				}
			},
			updateHotelInfo(){//提交酒店信息
				var _this=this;
				var IsUseShinho=Number(this.IfUse[0]);
				var IsAnyChild=Number(this.HaveChild[0]);
				
				if(this.offerId==1){//酒店会员
					this.MarketsaleName="";
					this.MarketScope[0]="";
					IsAnyChild="";
					this.HomeIncome[0]="";
					this.CookieNum[0]="";
				}else if(this.offerId==2){//美食爱好者
					this.ProvinceId=0;
					this.ProvinceName="";
					this.CityId=0;
					this.CityName="";
					this.AreaId=0;
					this.AreaName="";
					this.hotelName="";
					this.hoteladdress="";
					this.MarketsaleName="";
					this.MarketScope[0]="";
					this.hotelSell[0]="";
					this.hotelSpend[0]="";
					IsUseShinho="";
					this.productSource[0]="";
				}else if(this.offerId==3){//调味品供货商
					IsAnyChild="";
					this.HomeIncome[0]="";
					this.CookieNum[0]="";
					this.hotelSell[0]="";
					this.hotelSpend[0]="";
					IsUseShinho="";
					this.productSource[0]="";
				}
				
				var params={
					"MemberId":this.userId,
					"MemberName":this.username,
					"PositionType":this.offer[0],
					"Position":this.offer[1],
					"ProvinceId":this.ProvinceId,
					"ProvinceName":this.ProvinceName,
					"CityId":this.CityId,
					"CityName":this.CityName,
					"AreaId":this.AreaId,
					"AreaName":this.AreaName,
					"HotelName":this.hotelName,
					"HotelAddress":this.hoteladdress,
					"WholesaleName": this.MarketsaleName, //批发市场
					"ShopOperateSize":this.MarketScope[0], //门店经营范围
					"IsAnyChild" :IsAnyChild,
					"HomeIncome" :this.HomeIncome[0],//家庭收入
					"CookRate":this.CookieNum[0],   //烹饪频率
					"OperationType":this.hotelSell[0],   //主营类型
					"PerConsumption": this.hotelSpend[0], //人均消费
					"IsUseShinho":IsUseShinho,
					"Purchaser":this.productSource[0] //调味品采购权
		  		}
//				console.log(params);
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.updateHotelInfo,
		  			data:params
		  		}).then(function(response){
                    if(response.data=="Exc Success"){
                    	if(_this.isSave==0 && _this.btnNum==1){//没有保存过  进行保存
			     	  		_this.$vux.toast.text('保存成功,您已增加20积分','middle');
			     	  		_this.updateMemberIntegral();//更改积分
			     	  	}else if(_this.isSave==1){
			     	  		_this.$vux.toast.text('保存成功','middle');
			     	  		_this.goWhere();
			     	  	}
                    }else{
			     	  	_this.$vux.toast.text('保存失败','middle');
			     	  	_this.btnNum=0;
                    }
		  		})
			},
			goWhere(){//提交成功后 页面跳转
				var _this = this;
				if(_this.source == 'shopdetail'&&_this.productType==1){//从商城跳转过来 跳转到实物下单页
	      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,source:_this.source,addressId:_this.addressId}});
	      		}else if(_this.source == 'shopdetail'&&_this.productType==2){//从商城跳转过来 跳转到虚拟下单页
	      			_this.$router.push({path:'/component/inventedorder',query:{skudata:_this.skudata,count:_this.count}});
	      		}else if(_this.source == 'shopcar'){//从购物车跳转过来 实物订单
	   				_this.$router.push({path:'/component/shoporder',query:{selectArr:_this.selectArr,total:_this.total,addressId:0,source:'shopcar'}});
	      		}else if(_this.source == 'integraldraw'){//从积分抽奖跳转过来
	   				//订单接口
//	   				alert('integraldraw');
					_this.addOrder();//新增订单
	      		}else{
	      			_this.$router.push('/component/resume');
	      		}
			},
			updateMemberIntegral(){//更改积分
		  		var _this=this;
		  		var params={
		  		    'Operation':'plus',
					'Integral':20,
					'MemberId':this.userId,
					'IntegralSource':'简历完善',
					'Remark':'酒店信息'
		  		}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.updateMemberIntegral,
		  			data:params
		  		}).then(function(response){
		//			console.log(response);
		           _this.goWhere();
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
					_this.memberInfo=data[0];
					
					if((data[0].PerConsumption==null||data[0].PerConsumption=="")&&(data[0].HomeIncome==null||data[0].HomeIncome=="")&&(data[0].WholesaleName==null||data[0].WholesaleName=='')){
						_this.isSave=0;//没有保存过
					}else{
						_this.isSave=1;//保存过
					}
					
					_this.username=_this.memberInfo.MemberName;
					_this.offer=[_this.memberInfo.PositionType,_this.memberInfo.Position];//工作岗位
					
					if(_this.memberInfo.Position=='美食爱好者'){//2
						_this.offerId=2;
						if(_this.memberInfo.IsAnyChild==1){//有孩子
							_this.HaveChild=['1'];
						}else if(_this.memberInfo.IsAnyChild==0){//没有
							_this.HaveChild=['0'];
						}else{
							_this.HaveChild=[];
						}
					    _this.memberInfo.HomeIncome==null||_this.memberInfo.HomeIncome=="" ? _this.HomeIncome=[] :_this.HomeIncome=[_this.memberInfo.HomeIncome];//家庭收入
						_this.memberInfo.CookRate==null||_this.memberInfo.CookRate=="" ? _this.CookieNum=[] : _this.CookieNum=[_this.memberInfo.CookRate];//烹饪频率
					}else if(_this.memberInfo.Position=='调味品供货商'){//3
						_this.offerId=3;
						_this.MarketsaleName=_this.memberInfo.WholesaleName;//所在批发市场名称
						//门店区域
						if(_this.memberInfo.ProvinceId==null){//未填写省市区
                            if(_this.latitude==''){//没有成功获取到经纬度
								_this.hotelarea=[0,0,0];
                            }else{
		                    	_this.getLocation(_this.latitude,_this.longitude);//自动定位省市区 
                            }
						}else{
							var ProvinceId=(_this.memberInfo.ProvinceId+0).toString();
							var CityId=(_this.memberInfo.CityId+34).toString();
							var AreaId=(_this.memberInfo.AreaId+34+358).toString();
							_this.hotelarea=[ProvinceId,CityId,AreaId];
						}
						_this.hotelName=_this.memberInfo.HotelName;//门店名称
						_this.hoteladdress=_this.memberInfo.HotelAddress;//门店地址
						_this.memberInfo.ShopOperateSize==null||_this.memberInfo.ShopOperateSize=="" ? _this.MarketScope=[] : _this.MarketScope=[_this.memberInfo.ShopOperateSize];//门店经营范围
					}else{//1
						_this.offerId=1;
						//酒店区域
						if(_this.memberInfo.ProvinceId==null){//未填写省市区
							if(_this.latitude==''){//没有成功获取到经纬度
								_this.hotelarea=[0,0,0];
                            }else{
		                    	_this.getLocation(_this.latitude,_this.longitude);//自动定位省市区 
                            }
						}else{
							var ProvinceId=(_this.memberInfo.ProvinceId+0).toString();
							var CityId=(_this.memberInfo.CityId+34).toString();
							var AreaId=(_this.memberInfo.AreaId+34+358).toString();
							_this.hotelarea=[ProvinceId,CityId,AreaId];
						}
						_this.getCity();//获取酒店人均消费
						_this.hotelName=_this.memberInfo.HotelName;//酒店名称
						_this.hoteladdress=_this.memberInfo.HotelAddress;//酒店地址
						_this.memberInfo.OperationType==null||_this.memberInfo.OperationType=="" ? _this.hotelSell=[] : _this.hotelSell=[_this.memberInfo.OperationType];//主营类型
						_this.memberInfo.PerConsumption==null||_this.memberInfo.PerConsumption=="" ? _this.hotelSpend=[] : _this.hotelSpend=[_this.memberInfo.PerConsumption];//酒店人均消费
						_this.memberInfo.IsUseShinho==null||_this.memberInfo.IsUseShinho=="" ? _this.IfUse=[] : _this.IfUse=[_this.memberInfo.IsUseShinho.toString()];//是否使用欣和
						_this.memberInfo.Purchaser==null||_this.memberInfo.Purchaser=="" ? _this.productSource=[] : _this.productSource=[_this.memberInfo.Purchaser];//调味品采购权
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
//		  			console.log(data);
		  			if(data!=null){
		  			  if(data.AuthState==0){//信息审核中 不能修改酒店信息
		  			    _this.isUpdate=1;// 不能修改酒店信息
		  			  }
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
						    });
			  			}) 
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
				      	_this.hotelarea = [ProvinceId,CityId,AreaId];//当前定位的省市区
						_this.getCity();//获取人均消费
			        }
			    })
	        },
	        addOrder(){//新增订单
			    var _this=this;
				var OrderDetaile=[];//提交的商品
	  		    var item={
					"OrderDetaileId": 0,
				    "OrderId": 0,
			        "SkuId": this.skuId,
			        "SkuName": this.skuName,
			        "ProductId":this.productId,
			        "Count": 1
				}
	  		    OrderDetaile.push(item);
				var params={
				    "MemberId": this.userId,
				    "OrderAddress": "",
				    "OrderName":this.username,
				    "OrderTelephone":"",
				    "OrderState": "未提交",
				    "OrderPrice": 0,
				    "OrderType":2,
				    "InventedType":1,
				    "OrderFrom":"积分抽奖",
				    "OrderRemark":"积分抽奖",
				    "OrderDetaile":OrderDetaile
				}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.addOrder,
		  			data:params
		  		}).then(function(response){
			        _this.orderId=JSON.parse(response.data);
//			        alert(_this.orderId);
			        setTimeout(function(){
				    	_this.$router.push({path:'/component/receiveprice',query:{prize:_this.prize,orderId:_this.orderId,source:'integraldraw'}});
			        },100)
		  		})
		    }
		},
		mounted() {
			//配置config
			var uri=window.location.href.split('#')[0];
		    this.url=encodeURIComponent(uri);
			this.setConfig();
			
			this.offerlist = offer.offerArr;//获取岗位
            
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
			this.getMemberAuth();//获取认证信息状态
			
			this.skudata = this.$route.query.skudata;
			this.addressId = this.$route.query.addressId;
			this.source = this.$route.query.source;
			this.productType = this.$route.query.productType;
			this.count = this.$route.query.count;
			this.selectArr = this.$route.query.selectArr;
			this.total = this.$route.query.total;
			this.prize = this.$route.query.prize;
			this.skuId = this.$route.query.skuId;
			this.skuName = this.$route.query.skuName;
			this.productId = this.$route.query.productId;
			
		},
		created(){
            this.getRegion();//获取省市区
		}
	}
</script>

<style scoped>
	.hotelDiv {margin-left: 15px;overflow-x: hidden;}
	.infor_text {color: #E83428;font-size: 12px;margin-top: 15px;}
</style>
<style>
	#vux_view_box_body {background: #fff;}
	.resumehotel .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;margin-bottom: 30px;}
	.resumehotel .weui-btn:after {border: none;}
	.resumehotel .weui-cell:before{border-top: 0px !important;}
	.resumehotel .vux-cell-box:before{border-top: 0px !important;}
	.resumehotel .weui-cell{padding: 10px 18px 10px 0 !important;border-bottom: 1px solid #c8c7cc;}
	.resumehotel .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
	.resumehotel .vux-label {font-size: 15px;color: #3e3e3e;}
	.resumehotel .weui-label {font-size: 15px;color: #3e3e3e;}
	.vux-popup-picker-header-menu-right { color: #e83428 !important; }
	.dp-header .dp-item.dp-right { color: #e83428 !important; }
	.resumehotel .weui-input {text-align: right;font-size: 12px;color: #a3a3a3;}
	.resumehotel .weui-icon-clear:before{display:none;}
	.v-transfer-dom .weui-cells__title + .weui-cells{height: 14rem;overflow-y:auto;}
	.vux-popup-header-right{color: #E83428 !important;}
	.weui-cells_checkbox .weui-check:checked + .vux-checklist-icon-checked:before{color: #E83428 !important;}
	.resumehotel .vux-datetime {color: #3e3e3e;font-size: 15px;}
	.resumehotel .weui-label{font-size: 14px !important;color: #3E3E3E !important;}
	.resumehotel .weui-input,.resumeinformation .vux-popup-picker-value,.resumeinformation .weui-cell__ft{font-size: 14px;color: #3E3E3E;}
</style>

