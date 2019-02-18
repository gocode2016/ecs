<template>
  <div class="shopaddress">
    
    <div class="boxInfo" v-show="box1">
    	<x-input title="收货人：" v-model='FullName' :show-clear="false"  placeholder-align="left" label-width="5em"></x-input>
    	<x-input title="联系方式："  v-model='Mobile' :show-clear="false"  placeholder-align="left" label-width="5em"></x-input>
    	<popup-picker title="所在地区" :data="address" :columns="3" v-model="addressArr" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
    	<x-input title="详细地址：" v-model='MemberAddress' placeholder="街道、楼牌号"  :show-clear="false"  placeholder-align="left" label-width="5em"></x-input>
    	<x-switch title="设为默认地址" v-model='switchvalue' inline-desc="注：每次下单时会优先使用该地址"></x-switch>
    	<button @click="saveOrUpdate">保存并使用</button>
    </div>
    <div class="boxList" v-show="box2">
    	<div class="wrap" v-for="(item,index) in addressInfo" @click="selectAddress(index,item.AddressId)">
    		<div><img :src='item.img'></div>
    		<div>
    			<p class="name">{{item.FullName}}<span>{{item.Mobile}}</span></p>
    			<p>{{item.ProvinceName}}{{item.CityName}}{{item.AreaName}}{{item.MemberAddress}}</p>
    		</div>
    		<div @click.stop="edit(item.AddressId)"><img src="../../static/credit/bj.png"></div>
    	</div>
    	<button @click="creatAddress">+ 新建地址</button>
    </div>
    
  </div>
</template>

<script>
import { XInput ,PopupPicker,XSwitch,cookie,Toast,Loading } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		XInput,
		PopupPicker,
		XSwitch,
		cookie,
		Toast,
		Loading
	},
	data(){
		return{
//			box1:false,
			box1:false,
			box2:false,
			address: [],
			addressArr: [],
			provinceList: [],
			cityList: [],
			areaList: [],
			num1: 0,
		  num2: 0,
		  num3: 0,
			switchvalue:false,
			addressInfo:[],//地址列表
			updateInfo:{},//单个地址信息
			userId:'',
			userData:{},
			FullName:'',
			Mobile:'',
			MemberAddress :'',
			IsDefault :'',
			ZipCode :'10010',//测试
			ProvinceId:0,
		  ProvinceName:'', 
		  CityId :0,
	    CityName:'',
	    AreaId :0,
	    AreaName :'',
		  addressId:0,
		  selectArr:[],
		  total:'',
		  skudata:'',
		  orderId:''
		}
	},
	methods:{
      getAddressList(){//收货地址列表
	    	var _this=this;
	    	if(this.addressId==0){//全部地址列表
	    	  var params="?memberId="+this.userId;
	    	}else{//单个地址
	    	  var params="?memberId="+this.userId+'&AddressId='+this.addressId;
	    	}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getAddressList+params,
	  		}).then(function(response){
	  			if(_this.addressId==0){
	  			  _this.addressInfo=JSON.parse(response.data);
//	  			  console.log(_this.addressInfo);
	  			  for(let i in _this.addressInfo){
	  			  	if(i==0){
	  			  		_this.addressInfo[i].img='../../static/credit/dh.png';
	  			  	}else{
	  			  		_this.addressInfo[i].img='';
	  			  	}
	  			  }
	  			}else{
		  		  _this.updateInfo=JSON.parse(response.data)[0];
		  		  _this.FullName=_this.updateInfo.FullName;//姓名
		  		  _this.Mobile=_this.updateInfo.Mobile;//电话
		  		  _this.MemberAddress=_this.updateInfo.MemberAddress;//详细地址
		  		  var ProvinceId=(_this.updateInfo.ProvinceId+0).toString();
				    var CityId=(_this.updateInfo.CityId+34).toString();
				    var AreaId=(_this.updateInfo.AreaId+34+358).toString();
		  		  _this.addressArr=[ProvinceId,CityId,AreaId];//区域
		  		  _this.updateInfo.IsDefault==1 ? _this.switchvalue=true : _this.switchvalue=false;//设为默认地址
	  			}
	  		})
      },	
      selectAddress(index,addressId){//选择地址
      	var _this=this;
      	for(let i in this.addressInfo){
      		this.addressInfo[i].img='';
      		this.addressInfo.splice(i,1,this.addressInfo[i]);
      	}
      	this.addressInfo[index].img='../../static/credit/dh.png';
      	setTimeout(function(){
      		if(_this.$route.query.source=='shopcar'){
      			_this.$router.push({path:'/component/shoporder',query:{selectArr:_this.selectArr,total:_this.total,addressId:addressId,source:'shopcar'}});
      		}else if(_this.$route.query.source=='shopdetail'){
      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,addressId:addressId,source:'shopdetail'}});
      		}else if(_this.$route.query.source=='thanksdraw'){
      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,addressId:addressId,source:'thanksdraw',orderId:_this.orderId}});
      		}
      	},500)
      },
      creatAddress(){//新建地址
      	this.box1=true;
      	this.box2=false;
      	$("#vux_view_box_body").css({"background":"#fff"})
      },
      edit(addressId){//编辑地址
      	this.box1=true;
      	this.box2=false;
      	$("#vux_view_box_body").css({"background":"#fff"})
      	this.addressId=addressId;
      	this.getAddressList();//获取单个地址
      },
      saveAddress(){//保存地址  新增地址接口
      	this.showLoading () ;
      	var _this=this;
      	if(this.switchvalue){//默认地址
      	  this.IsDefault=1;//默认地址
      	}else{
      	  this.IsDefault=0;
      	}
      	this.switchAddress();//获取省市区id 名称
      	var params={ 
      	  'MemberId':this.userId,
		      'FullName':this.FullName,
          'Mobile':this.Mobile,
          'ProvinceId':this.ProvinceId,
				  'ProvinceName':this.ProvinceName, 
				  'CityId' :this.CityId ,
				  'CityName':this.CityName,
				  'AreaId' :this.AreaId,
				  'AreaName' :this.AreaName,
          'MemberAddress' :this.MemberAddress,
          'IsDefault':this.IsDefault,
          'ZipCode':this.ZipCode,
				}
				this.$http({
					method:'post',
					url:apiUrl.saveAddress,
					data:params
				}).then(function(response){
//					_this.addressId=parseInt(response.data);
          _this.$vux.loading.hide();//loading图标消失
          if(_this.$route.query.source=='shopcar'){
      			_this.$router.push({path:'/component/shoporder',query:{selectArr:_this.selectArr,total:_this.total,addressId:_this.addressId,source:'shopcar'}});
      		}else if(_this.$route.query.source=='shopdetail'){
      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,addressId:_this.addressId,source:'shopdetail'}});
      		}else if(_this.$route.query.source=='thanksdraw'){
      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,addressId:_this.addressId,source:'thanksdraw',orderId:_this.orderId}});
      		}
				})
      },
      updateAddress(){//更新地址
      	this.showLoading () ;
      	var _this=this;
      	if(this.switchvalue){//默认地址
      	  this.IsDefault=1;//默认地址
      	}else{
      	  this.IsDefault=0;
      	}
      	this.switchAddress();//获取省市区id 名称
      	var params={
      	  'AddressId':this.addressId,
  		    'MemberId':this.userId,
		      'FullName':this.FullName,
          'Mobile':this.Mobile,
          'ProvinceId':this.ProvinceId,
				  'ProvinceName':this.ProvinceName, 
				  'CityId' :this.CityId ,
				  'CityName':this.CityName,
				  'AreaId' :this.AreaId,
				  'AreaName' :this.AreaName,
          'MemberAddress' :this.MemberAddress,
          'IsDefault':this.IsDefault,
          'ZipCode':this.ZipCode,
  			}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.updateAddress,
	  			data:params
	  		}).then(function(response){
//	  			_this.addressId=parseInt(response.data);
          _this.$vux.loading.hide();//loading图标消失
          if(_this.$route.query.source=='shopcar'){
      			_this.$router.push({path:'/component/shoporder',query:{selectArr:_this.selectArr,total:_this.total,addressId:_this.addressId,source:'shopcar'}});
      		}else if(_this.$route.query.source=='shopdetail'){
      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,addressId:_this.addressId,source:'shopdetail'}});
      		}else if(_this.$route.query.source=='thanksdraw'){
      			_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skudata,addressId:_this.addressId,source:'thanksdraw',orderId:_this.orderId}});
      		}
	  		})
      },
      saveOrUpdate(){//保存或更新
      	if(this.FullName==''||this.Mobile==''||this.addressArr==''||this.MemberAddress==''){//判断信息是否完整
      		this.$vux.toast.text('请把信息填写完整哦', 'middle');
      	}else{
      		var box=/^1\d{10}$/;//手机号正则
      		if(!box.test(this.Mobile)){
      		  this.$vux.toast.text('请填写正确的手机号', 'middle');
		    	}else{
		    		if(this.addressId==0){//保存地址
		          this.saveAddress();
		        }else{//更新地址
		          this.updateAddress();
		        }
		    	}
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
	    getAddress() {//获取省市区
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
	//			console.log(_this.address);					
		 	  })
      },
      switchAddress() {
      	// 已获取的后台省市区数据
      	var provinceList = this.provinceList;
      	var cityList = this.cityList;
      	var areaList = this.areaList;
      	// 将前台省市区联动数据转换回后台省市区数据
      	// 转换省市区id
      	var provinceId = this.addressArr[0] - 0;
      	var cityId = this.addressArr[1] - this.num1;
      	var areaId = this.addressArr[2] - this.num2 - this.num1;
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
    this.userId=parseInt(this.userData.UserId);
    
    if(this.$route.query.isAddress==1){//没有地址 新增页面
    	this.box2=false;
    	this.box1=true;
    	$("#vux_view_box_body").css({"background":"#fff"})
    }else if(this.$route.query.isAddress==2){//有地址 显示地址列表页面
    	this.box1=false;
    	this.box2=true;
    	$("#vux_view_box_body").css({"background":"#f8f8f8"})
    }
    if(this.$route.query.source=='shopcar'){
    	this.selectArr=this.$route.query.selectArr;
      this.total=this.$route.query.total;
    }else if(this.$route.query.source=='shopdetail'){
    	this.skudata=this.$route.query.skudata;
    }else if(this.$route.query.source=='thanksdraw'){
    	this.skudata=this.$route.query.skudata;
    	this.orderId=this.$route.query.orderId;
    }
    
		this.getAddress();//获取省市区
		this.getAddressList();//获取收货地址
	}
}
</script>

<style scoped>
	.boxInfo{width:100%;height:100%;background: #fff;}
  .boxInfo button{width: 50%;height: 40px;background: #E83428; color: #fff;border: none;margin-left: 25%;position: absolute;bottom: 20px;}
  .boxList{width:100%;height:100%;background: #ccc;}
  .wrap{width: 100%;padding: 20px 0px;background: #fff;font-size: 14px;color: #3E3E3E;height: 45px;display: flex;flex-wrap: wrap;}
  .wrap div:nth-child(1),.wrap div:nth-child(3){width: 10%;line-height:45px;text-align: center;}
  .wrap div:nth-child(2){width: 80%;text-align: left;}
  .wrap img{width: 20px;}
  .boxList button{width:80%;height: 40px;background: #E83428; color: #fff;border: none;margin-left: 10%;position: absolute;bottom: 30px;}
  .name{color: #E83428;}
  .name span{margin-left: 20px;}
</style>
<style>
   #vux_view_box_body{background: #fff;}
  .shopaddress .weui-label { font-size: 14px;color: #3e3e3e;width:5.5rem !important;}
  .shopaddress .vux-label-desc{color:#a2a2a2 !important ;}
  .shopaddress .weui-cell {padding: 15px 15px !important;}
  .shopaddress .vux-cell-box:before{left: 0;}
  .shopaddress .vux-x-switch{border-bottom: 1px solid #D9D9D9;}
  .shopaddress .weui-cell_access .weui-cell__ft:after {height: 10px;width: 10px;}
  .vux-popup-picker-header-menu-right{color: #E83428 !important;}
</style>