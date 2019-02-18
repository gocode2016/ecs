<template>
  <div class="chefhomepage">
    <!--用户部分-->
    <div class="person_top">
    	 <div class="personphoto"><div class="myphoto"><img :src="userImg"/></div></div>   	 
    	 <p class="myName">{{chefInfo.MemberName}}</p>
    	 <grid :rows="4">
		      <grid-item>
		        <p class="grid-center grid-center1">ID</p>
		        <p class="grid-center">{{chefInfo.MemberId}}</p>
		      </grid-item>
		      <grid-item>
		        <p class="grid-center grid-center1">等级</p>
		        <p class="grid-center" v-if='showFlag'>会员已认证</p>
		        <p class="grid-center wrz" v-else>会员未认证</p>
		      </grid-item>
		      <grid-item>
		        <p class="grid-center grid-center1">当前积分</p>
		        <p class="grid-center">{{chefInfo.LeaveIntegral}}</p>
		      </grid-item>
          <grid-item>
            <p class="grid-center grid-center1">总积分</p>
            <p class="grid-center">{{chefInfo.TotalIntegral}}</p>
          </grid-item>
      </grid>
    </div>
    
    <!--订单-->
    <div class="person_icon">
    	<grid :rows="4">
	      <grid-item  :label="item.text" v-for="(item,index) in list" :key="index" class="gridpad" @on-item-click="jump(item.url)">
	        <img slot="icon" :src="item.img">
	      </grid-item>
      </grid>
    </div>

    <!--厨师信息-->
    <x-input title="姓名" text-align="right" v-model="MemberName"></x-input> 
    <x-input title="联系方式" text-align="right" v-model="Telephone" keyboard="number" is-type="china-mobile" ></x-input>
    <div v-show="offerId==1">
	    <popup-picker title="酒店区域" :data="addresslist" v-model="hotelarea" @on-change="getCity" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
			<x-input title="酒店名称" v-model="hotelName" text-align="right"></x-input>
			<x-input title="酒店地址" placeholder="请输入" v-model="hoteladdress" text-align="right"></x-input>
    </div>
    <x-input title="客户编码" text-align="right" v-model="MemberCode"></x-input> 
    <popup-picker title="工作岗位" :data="offer" :columns="2" v-model="offerArr" show-name @on-change="offerShow" cancel-text='取消' confirm-text='确定' value-text-align ='right'></popup-picker>
    <!--酒店会员-->
    <div v-show="offerId==1">
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
			<x-input title="所在批发市场名称" v-model="MarketsaleName" text-align="right"></x-input>
			<popup-picker title="门店区域" :data="addresslist" v-model="hotelarea" @on-change="getCity" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
			<x-input title="门店名称" v-model="hotelName" text-align="right"></x-input>
			<x-input title="门店地址" placeholder="请输入" v-model="hoteladdress" text-align="right"></x-input>
			<popup-picker title="门店经营范围" :data="MarketScopeList" v-model="MarketScope" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
		</div>
    <x-input title="备注" text-align="right" v-model="Remark"></x-input>
    <x-button  @click.native="save" style="margin-top: 25px;">保存</x-button>
    
    <!--认证信息-->
    <div class="mask" v-show="maskFlag">
    	<div class="rzBox">
    		<p style="text-align: center;">
    			<img src="../../static/credit/yrz_icon.png" style="width:24%;margin-top: 10px;"/>
    			<img src="../../static/credit/prizex.png" style="width: 24px;position:absolute;right:11%;margin-top: 10px;" @click="closeClick"/>
    		</p>
    		<div v-if="rzFlag"><!--实名认证-->
			    <div class="chefbox">
			    	<p><span>认证方式:实名认证</span></p>
			    	<p><span>通过时间:</span>{{chefInfo.AuthTime}}</p>
			    	<p><span>真实姓名:</span>{{chefInfo.MemberName}}</p>
			    	<p><span>工作岗位:</span>{{chefInfo.PositionType}}{{chefInfo.Position}}</p>
			    	<p><span>酒店区域:</span>{{chefInfo.ProvinceName}}{{chefInfo.CityName}}{{chefInfo.AreaName}}</p>
			    	<p><span>酒店名称:</span>{{chefInfo.HotelName}}</p>
			    	<p><span>酒店地址:</span>{{chefInfo.HotelAddress}}</p>
			    	<p><span>主营类型:</span>{{chefInfo.OperationType}}</p>
			    	<p><span>酒店人均消费:</span>{{chefInfo.PerConsumption}}</p>
			    	<p><span>现在使用欣和产品:</span>{{chefInfo.IsUseShinho}}</p>
			    	<p><span>调味品采购权:</span>{{chefInfo.Purchaser}}</p>
			    </div>
			    <p class="photo_text">照片认证：</p>
			    <div class="photorz">
			    	<div><img :src="chefInfo.AuthImg1"/></div>
			    	<span>+</span>
			    	<div><img :src="chefInfo.AuthImg2"/></div>
			    </div>
		    </div>
		    <div v-else><!--认证码认证-->
		    	<div class="chefbox">
			    	<p><span>认证方式:</span>认证码认证</p>
			    	<p><span>通过时间:</span>{{registCodeInfo.RegistDate}}</p>
			    	<p><span>提供认证码的业务代表:</span>{{registCodeInfo.Name}}</p>
			    	<p><span>业务代表联系方式:</span>{{registCodeInfo.Telephone}}</p>
			    </div>
		    </div>
		    <button class="btnDiv" @click="closeClick">确定</button>
    	</div>
    </div>
    
  </div>
</template>
<script>
import apiUrl from '../apiUrls.js'
import region from '../region.js'
import offer from '../offer.js'
import { Grid, GridItem, Group, PopupPicker, Cell, CellBox, XInput, XButton, Card, cookie ,Toast,Loading} from 'vux'
export default {
	components: {
    Grid, 
    GridItem,
    Group,
    PopupPicker,
    Cell,
    CellBox,
    XInput,
    XButton,
    Card,
    cookie,
    Toast,
    Loading
  },
  data(){
  	return{
  		maskFlag:false,
  		rzFlag:true,//认证信息默认显示 实名认证
  		registCodeInfo:'',//注册码信息
  		num1: 0,
	    num2: 0,
	    num3: 0,
	    provinceList: [],
			cityList: [],
			areaList: [],
			AreaId:0,
      AreaName:'',
      CityId:0,
      CityName:'',
      ProvinceId:0,
      ProvinceName:'',
			addresslist: [],
			hotelarea: [], //酒店区域 门店区域
			hoteladdress: '', //酒店地址 门店地址
			hotelName:'',//门店名称
  		offerId:1,//默认
  		userData:{},
  		userImg:'',//头像
  		userId:'',
  		chefInfo:{},//厨师信息
  		showFlag:true,
  		list:[{
  			img:'../../static/credit/chef_icon1.png',
  			text:'认证信息',
			  url:''
  		},{
  			img:'../../static/credit/team5.png',
  			text:'业绩贡献',
			  url:'/component/chefcontribution'
  		},{
  			img:'../../static/credit/per2.png',
  			text:'积分明细',
  			url:'/component/teammemberintegraldetail'
  		},{
  			img:'../../static/credit/per7.png',
  			text:'活动足迹',
  			url:'/component/cheffootmark'
  		}],
      MemberName:'',
      Telephone:'',
      offerArr: [],
      MemberCode:'',//客户编码
      MemberCodeTime:'',//客户编码时间
      Remark:'',
      offer: [],//岗位数据
      isMemberCode:0,//是否有客户编码
      currentTime:'',//记录客户编码保存的当前时间
      openId:'',
      IfUse:[],//是否使用欣和产品
      hotelSpendList:[],//酒店人均消费数据
			hotelSpend: [], //酒店人均消费
			hotelSell:[],//主营类型
			productSource:[],//调味品采购权
			HaveChild:[],//是否有孩子
			HomeIncome:[],//家庭收入
			CookieNum:[],//烹饪频率
			MarketsaleName:'',//所在批发市场名称
			MarketScope:[],//门店经营范围
			MarketScopeList: [//门店经营范围数据
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
			IfUseList: [//是否使用产品
		    [{
					name: '是',
					value: '1'
				},{
					name: '否',
					value: '0'
				}]
			],
			HomeIncomeList: [//家庭收入数据
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
  	}
  },
  methods:{
  	offerShow(value) {// 判定工作和岗位
				if(this.offerArr[1] == '美食爱好者') {
					this.offerId = 2;
				} else if(this.offerArr[1] == '调味品供货商') {
					this.offerId = 3;
				} else {
					this.offerId = 1;
				}
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
					_this.chefInfo=data[0];
					console.log(data);
					_this.MemberName=_this.chefInfo.MemberName;//姓名
					_this.Telephone=_this.chefInfo.MemberTelePhone;//电话
					_this.offerArr=[_this.chefInfo.PositionType,_this.chefInfo.Position];//工作岗位
					_this.MemberCode=_this.chefInfo.MemberCode;//客户编码
					_this.Remark=_this.chefInfo.Remark//备注
					
					
					if(_this.chefInfo.MemberCode!=""&&_this.chefInfo.MemberCode!=null){//有客户编码
						_this.isMemberCode=1;
					}
          
          if(_this.chefInfo.AuthImg1==null&&_this.chefInfo.AuthImg2==null){
          	_this.rzFlag=false;//显示认证码认证
          	$('.rzBox').css({
          		'height':'auto',
          		'margin-top':'50%'
          	})
          }
          
          if(_this.chefInfo.Position=='美食爱好者'){//2
         	  _this.offerId=2;
						if(_this.chefInfo.IsAnyChild==1){//有孩子
							_this.HaveChild=['1'];
						}else if(_this.chefInfo.IsAnyChild==0){//没有
							_this.HaveChild=['0'];
						}else{
							_this.HaveChild=[];
						}
						_this.chefInfo.HomeIncome==null||_this.chefInfo.HomeIncome=="" ? _this.HomeIncome=[] :_this.HomeIncome=[_this.chefInfo.HomeIncome];//家庭收入
						_this.chefInfo.CookRate==null||_this.chefInfo.CookRate=="" ? _this.CookieNum=[] : _this.CookieNum=[_this.chefInfo.CookRate];//烹饪频率
          }else if(_this.chefInfo.Position=='调味品供货商'){//3
         	  _this.offerId=3;
						_this.MarketsaleName=_this.chefInfo.WholesaleName;//所在批发市场名称
						//门店区域
						if(_this.chefInfo.ProvinceId!=null&&_this.chefInfo.ProvinceId!=''){
							var ProvinceId=(_this.chefInfo.ProvinceId+0).toString();
							var CityId=(_this.chefInfo.CityId+34).toString();
							var AreaId=(_this.chefInfo.AreaId+34+358).toString();
							_this.hotelarea=[ProvinceId,CityId,AreaId];
						}else{
							_this.hotelarea=[0,0,0];
						}
						
						_this.hotelName=_this.chefInfo.HotelName;//门店名称
						_this.hoteladdress=_this.chefInfo.HotelAddress;//门店地址
						_this.chefInfo.ShopOperateSize==null||_this.chefInfo.ShopOperateSize=="" ? _this.MarketScope=[] : _this.MarketScope=[_this.chefInfo.ShopOperateSize];//门店经营范围
          }else{//1
         	  _this.offerId=1;
						//酒店区域
						if(_this.chefInfo.ProvinceId!=null&&_this.chefInfo.ProvinceId!=''){
							var ProvinceId=(_this.chefInfo.ProvinceId+0).toString();
							var CityId=(_this.chefInfo.CityId+34).toString();
							var AreaId=(_this.chefInfo.AreaId+34+358).toString();
							_this.hotelarea=[ProvinceId,CityId,AreaId];
						}else{
							_this.hotelarea=[0,0,0];
						}
						_this.getCity();//获取酒店人均消费
						_this.hotelName=_this.chefInfo.HotelName;//酒店名称
						_this.hoteladdress=_this.chefInfo.HotelAddress;//酒店地址
						_this.chefInfo.OperationType==null||_this.chefInfo.OperationType=="" ? _this.hotelSell=[] : _this.hotelSell=[_this.chefInfo.OperationType];//主营类型
						_this.chefInfo.PerConsumption==null||_this.chefInfo.PerConsumption=="" ? _this.hotelSpend=[] : _this.hotelSpend=[_this.chefInfo.PerConsumption];//酒店人均消费
						_this.chefInfo.IsUseShinho==null||_this.chefInfo.IsUseShinho=="" ? _this.IfUse=[] : _this.IfUse=[_this.chefInfo.IsUseShinho.toString()];//是否使用欣和
						_this.chefInfo.Purchaser==null||_this.chefInfo.Purchaser=="" ? _this.productSource=[] : _this.productSource=[_this.chefInfo.Purchaser];//调味品采购权
          }
          
          if(data[0].IsUseShinho==null||data[0].IsUseShinho==''){//是否使用欣和
						data[0].IsUseShinho='';
					}else if(data[0].IsUseShinho==0){
						data[0].IsUseShinho='否';
					}else if(data[0].IsUseShinho==1){
						data[0].IsUseShinho='是';
					}
          
				})
	  },
	  getMemberRegistCode(){//获取认证码认证信息
	  	var _this=this;
	  	var params={
  		  'MemberId':this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getMemberRegistCode,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			if(data.length!=0){
  				_this.registCodeInfo=data[0];//注册码认证信息
  			}
  		})
	  },
  	save(){//保存  
  		var _this=this;
			if(this.MemberName==''||this.Telephone==''||this.offerArr==''){
				this.$vux.toast.text('填写完整才能保存哦', 'middle');
			}else{
				  if(this.isMemberCode==0&&this.MemberCode!=""){//之前没有 现在有客户编码 
				  	this.getTime();//获取当前时间
				  }else if(this.isMemberCode==1){//之前有客户编码
				  	this.MemberCodeTime=this.chefInfo.MemberCodeTime;
				  }else if(this.isMemberCode==0&&this.MemberCode==""){//之前和现在都没有客户编码
				  	this.MemberCodeTime='';
				  }
				  this.showLoading();
				  this.switchAddress();
		  }
  	},
  	updateBindMemberInfo(){//更改厨师主页的信息
	    var _this=this;
			var IsUseShinho = Number(this.IfUse[0]); //是否使用欣和产品
			var IsAnyChild = Number(this.HaveChild[0]); //是否有孩子

      if(this.hotelarea[0]=='0'){//省市区为空时传值
					this.AreaId=0;
					this.AreaName='';
					this.CityId=0;
					this.CityName='';
					this.ProvinceId=0;
					this.ProvinceName='';
			}
			if(this.offerId == 1) { //酒店会员身份 以下字段置空
				//美食爱好者
				IsAnyChild = ""; //是否有孩子
				this.HomeIncome[0] = ""; //家庭收入
				this.CookieNum[0] = ""; //烹饪频率
				//调味品供货商
				this.MarketsaleName = ""; //批发市场名称
				this.MarketScope[0] = ""; //门店经营范围
			} else if(this.offerId == 2) { //美食爱好者
				//酒店会员 
				this.hoteladdress = ""; //酒店地址 门店地址 家庭地址
				this.hotelSell[0] = ""; //酒店主营
				this.hotelSpend[0] = ""; //酒店人均消费
				IsUseShinho = ""; //是否使用欣和产品
				this.productSource[0] = ""; //调味品采购权
				//调味品供货商
				this.MarketsaleName = ""; //批发市场名称
				this.MarketScope[0] = ""; //门店经营范围
			} else if(this.offerId == 3) { //调味品供货商
				//酒店会员 
				this.hotelSell[0] = ""; //酒店主营
				this.hotelSpend[0] = ""; //酒店人均消费
				IsUseShinho = ""; //是否使用欣和产品
				this.productSource[0] = ""; //调味品采购权
				//美食爱好者
				IsAnyChild = ""; //是否有孩子
				this.HomeIncome[0] = ""; //家庭收入
				this.CookieNum[0] = ""; //烹饪频率
			}
  		
  		var params={
  			'MemberId':this.userId,//会员id
  		  'MemberTelePhone':this.Telephone,//电话
				'MemberName': this.MemberName,
				'PositionType':this.offerArr[0] ,
				'Position':this.offerArr[1] ,
				'MemberCode':this.MemberCode,//客户编码
				'MemberCodeTime':this.MemberCodeTime,//客户编码时间
				'Remark':this.Remark,//备注
				'AreaId':this.AreaId,
				'AreaName':this.AreaName,
				'CityId':this.CityId,
				'CityName':this.CityName,
				'ProvinceId':this.ProvinceId,
				"ProvinceName":this.ProvinceName,
				'HotelName':this.hotelName, //酒店名称 门店名称
				'HotelAddress':this.hoteladdress, //酒店地址 门店地址 家庭地址
				//酒店会员 
				'OperationType':this.hotelSell[0], //酒店主营
				'PerConsumption':this.hotelSpend[0] , //酒店人均消费
				'IsUseShinho': IsUseShinho, //是否使用欣和产品
				'Purchaser': this.productSource[0],//调味品采购权
				//美食爱好者
				'IsAnyChild': IsAnyChild, //是否有孩子
				'HomeIncome': this.HomeIncome[0], //家庭收入
				'CookRate': this.CookieNum[0], //烹饪频率
				//调味品供货商
				'WholesaleName':this.MarketsaleName , //批发市场名称
				'ShopOperateSize': this.MarketScope[0], //门店经营范围
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateBindMemberInfo,
  			data:params
  		}).then(function(response){
  			_this.$vux.loading.hide();//loading图标消失
  			if(response.data=='Exc Success'){
				  _this.$vux.toast.text('保存成功', 'middle');
  			}else{
				  _this.$vux.toast.text('保存失败', 'middle');
  			}
  		})
  	},
  	getTime(){//获取当前时间
	  		var myDate = new Date();
				var year=myDate.getFullYear();    //获取完整的年份(4位,1970-????)
				var month=myDate.getMonth()+1;       //获取当前月份(0-11,0代表1月)
				var date=myDate.getDate();        //获取当前日(1-31)
				var hour=myDate.getHours();       //获取当前小时数(0-23)
        var minute=myDate.getMinutes();     //获取当前分钟数(0-59)
        var second=myDate.getSeconds();
        
        var month=time(month);
        var date=time(date);
        var hour=time(hour);
        var minute=time(minute);
        var second=time(second);
        function time(a){
        	if(a<10){
        		a='0'+a;
        	}
        	return a;
        }
        this.currentTime=year+'-'+month+'-'+date+'T'+hour+':'+minute+':'+second;
        this.MemberCodeTime=this.currentTime;
	  },
  	jump(link){
			if(link=='/component/teammemberintegraldetail'){
				this.$router.push({path:link,query:{userId:this.userId}});
			}else if(link=='/component/cheffootmark'){
				this.$router.push({path:link,query:{memberId:this.userId,openId:this.openId}});
			}else if(link=='/component/chefcontribution'){
				this.$router.push({path:link,query:{userId:this.userId}});
			}else if(link==''){
				//显示弹框
				this.maskFlag=true;
			}
  	},
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
      var provinceId = this.hotelarea[0] - 0;
      var cityId = this.hotelarea[1] - this.num1;
      var areaId = this.hotelarea[2] - this.num2 - this.num1;
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
      this.updateBindMemberInfo();//更新厨师信息
    },
    getCity(value) {//验证城市接口获取人均消费
				//	console.log(this.num2,this.num1);//358 34
				//区id
				var areaId = Number(this.hotelarea[2]) - this.num2 - this.num1;
				for(let i in this.areaList) {
					var arealist = this.areaList[i];
					if(areaId == arealist.AreaId) {
						var areaLevel = arealist.AreaLevel;
					}
				}
				// 调取验证城市接口，获取人均消费数组
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
		closeClick(){//关闭认证信息
			this.maskFlag=false;
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
	  this.userId=this.$route.query.memberId;
    this.getRegion();//获取省市区
    this.getMemberRegistCode();//获取认证码信息
    this.offer = offer.offerArr;// 设置岗位数据
  }
}
</script>
<style scoped>
	.person_top{width: 100%;height: 200px;background: url('../../static/credit/perback.png') no-repeat;background-size: 100% 100%;}
	.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;}
	.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 3px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
	.myphoto img{width:100%;}
	.myName{text-align: center;}
	.grid-center{text-align: center;font-size: 12px;color: #3E3E3E;}
	.grid-center1{color: #E83428;}
	.wrz:after{content: " "; display: inline-block; height: 6px; width: 6px;border-width: 1.5px 1.5px 0 0;border-color: #E83428;border-style: solid;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);}
    
  .person_icon{width: 100%;background: #fff;margin-top: 7px;border-top: 3px solid #fbfbfb; border-bottom: 3px solid #f1f1f1;}
  .gridpad{border-bottom: 1px solid #f1f1f1;}
  
 	.inte{width: 100%;border-top: 1px solid #f1f1f1;background: #fff;margin-top: 7px;}
 	/*.associatechef button { width: 50%; height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 30%;}*/
 	
  .mask{width: 100%;height:100%;background: rgba(0,0,0,0.5);position: absolute;top: 0;z-index: 1000;}
  .rzBox{background: #fff;width: 80%;margin-left: 10%;margin-top: 50px;border-radius: 5px;margin-bottom: 50px;height: 80%; overflow: scroll;}
 	.chefbox,.photo_text{padding-left: 10px;font-size: 13px;color: #3E3E3E;}    
	.chefbox p{line-height: 40px;border-bottom: 1px solid #f4f4f4;}
	.chefbox p span{margin-right: 10px;}
	.photo_text{line-height: 40px;}
	.photorz{display: flex;padding: 0 5%;}
	.photorz div{width: 35%;height: 90px;}
	.photorz div img{width:100%;height: 100%;}
	.photorz span{font-size: 30px;color: #E83428;margin: 0 12%;line-height: 90px;}
	.rzBox .btnDiv{width: 30%;height: 30px;background: #E83428;color: #fff;border-radius: 5px;border: none;margin: 20px 35% 20px 35%;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.chefhomepage .weui-grid{padding: 13px 13px !important;}
	.chefhomepage .weui-grids:before{border-top: 0 !important;}
	.chefhomepage .weui-grid:before{border-right: 0 !important;}
	.chefhomepage .weui-grid:after{border-bottom: 0 !important;}
	.chefhomepage .weui-grid__icon{width: 45px !important;height: 45px !important;}
	.chefhomepage .weui-grid__label{font-size: 12px !important;color: #3E3E3E !important;}
	.chefhomepage .weui-cell{height: 35px !important;padding: 10px 10px;}
	.chefhomepage .vux-label {font-size: 12px; color: #3e3e3e;margin-left: 27px;}
	.chefhomepage .weui-cell__ft{color: #E83428 !important;font-size: 12px;}
  .chefhomepage .weui-cell__ft:after {height: 7px !important; width: 7px !important; border-width: 1px 1px 0 0 !important; }
  .chefhomepage .vux-cell-box img{width: 15px;margin-right: 12px;position: relative;top: 2px;} 
  .chefhomepage button.weui-btn, input.weui-btn {width: 30%;height: 40px;background: #e83428;font-size: 14px;color: white;margin-bottom: 20px;}
  .chefhomepage .weui-grids:after{border-left: 0px;}
  .chefhomepage .weui-cells:before{border-top: 0px;}
  .chefhomepage .vux-cell-box:before{left: 0px;}
  .chefhomepage .weui-cells:after{border-bottom: 1.1px solid #D9D9D9;}
  .chefhomepage .weui-label{font-size: 14px !important;color: #3E3E3E !important;}
  .chefhomepage .weui-input,.chefhomepage .vux-popup-picker-value{font-size: 14px;color: #878787;}
</style>