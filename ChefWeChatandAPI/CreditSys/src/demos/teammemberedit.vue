<template>
    <div class="teammemberedit">
        <x-input title="姓名" text-align="right" v-model="MemberName"></x-input> 
        <x-input title="联系方式" text-align="right" v-model="TelePhone" keyboard="number" is-type="china-mobile" ></x-input>
        <popup-picker title="大区" :data="region" :columns="2" v-model="regionArr" show-name cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
        <popup-picker title="工作地点" :data="address" :columns="3" v-model="addressArr" show-name  cancel-text='取消' confirm-text='确定' value-text-align='right'></popup-picker>
        <x-button  @click.native="updateSaleMan" style="margin-top: 25px;">保存</x-button>
    </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import region from '../region.js';
import { XInput, Group, PopupPicker, AjaxPlugin, XButton,cookie,Toast,Loading } from 'vux'

export default {
  components: {
    XInput,
    Group,
    PopupPicker,
    AjaxPlugin,
    XButton,
    cookie,
    Toast,
    Loading
  },
  data () {
    return {
    	userData:'',
  		userId:'',
  		salemanInfo:{},
        provinceList: [],
        cityList: [],
        areaList: [],
        registerParams:{
            "ProvinceId": 0,
            "ProvinceName": '',   //省
            "CityId": 0,
            "CityName": '',       //市
            "AreaId": 0,
            "AreaName": '',       //区
            "RegionId": 0,        //大区id
            "RegionName": '',         //大区
        },
        MemberName:'',
        TelePhone:'',
        regionArr:[],
        addressArr:[],
        region:[],       
        address: []       
    }
  },
  methods: {
  	getSaleManInfo(){//获取会员信息
  		var _this=this;
  		var params={
  		  "SalemanId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSaleManInfo,
  			data:params
  		}).then(function(response){
			var data = JSON.parse(response.data);
			_this.salemanInfo = JSON.parse(data.Data);
//			console.log(_this.salemanInfo);
			
			_this.MemberName=_this.salemanInfo.Name;//姓名
			_this.TelePhone=_this.salemanInfo.Telephone;//电话
            _this.regionArr.push(_this.salemanInfo.RegionId + '|' + _this.salemanInfo.RegionName);//大区id 大区名称
			var ProvinceId=(_this.salemanInfo.ProvinceId+0).toString();
			var CityId=(_this.salemanInfo.CityId+34).toString();
			var AreaId=(_this.salemanInfo.AreaId+34+358).toString();
			_this.addressArr=[ProvinceId,CityId,AreaId];//酒店区域
  		})
  	},
	updateSaleMan(){//修改队员信息
		var _this=this;
	  	if(this.MemberName==''||this.TelePhone==''||this.regionArr==''||this.addressArr==''){
	  		this.$vux.toast.text('内容不能为空', 'middle');
	  	}else{
	  		this.showLoading () ;
	  		var region = this.regionArr[0].split("|");
	  		this.switchAddress();
	  		var params={
		        "AreaId":this.registerParams.AreaId,
                "AreaName":this.registerParams.AreaName,
			    "CardId":this.salemanInfo.CardId,
			    "CityId":this.registerParams.CityId,
			    "CityName":this.registerParams.CityName,
                "ImportDate":"",
			    "IsEnable":0,
				"Name":this.MemberName,
                "ProvinceId":this.registerParams.ProvinceId,
				"ProvinceName":this.registerParams.ProvinceName,
				"RegionId":region[0],
				"RegionName":region[1],
				"RegistDate":this.salemanInfo.RegistDate,
				"RegistState":1,
				"Remark":"",
				"SalemanId":this.salemanInfo.SalemanId,
				"Telephone":this.TelePhone,
				"WorkNum":this.salemanInfo.WorkNum
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.updateSaleMan,
	  			data:params
	  		}).then(function(response){
//	  			console.log(response.data);
	  			_this.$vux.loading.hide();//loading图标消失
	  			_this.$vux.toast.text('保存成功', 'middle');
	  		})
	  	}
	},
    getAddressList() {
	    var _this = this;
	    // 省
	    this.$http({
	        method:'GET',
	        url:apiUrl.getAllProvince,
	        data:{}
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
            method:'GET',
            url:apiUrl.getCityList,
            data:{}
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
	      method:'GET',
	      url:apiUrl.getAreaList,
	      data:{}
        }).then(function(respose){
            var obj = respose.data;
            var areaList = JSON.parse(obj);
            console.log(_this.num1, _this.num2);
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

        // console.log(provinceName, cityName, areaName);
        this.registerParams.ProvinceId = provinceId;
        this.registerParams.ProvinceName = provinceName;
        this.registerParams.CityId = cityId;
        this.registerParams.CityName = cityName;
        this.registerParams.AreaId = areaId;
        this.registerParams.AreaName = areaName;
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
  mounted: function(){
	  // 获取省市区及改写数据
	  // 切记给后台传数据的时候，
	  // 省值不变，
	  // 市值减去市列表的最后一条数据的id值，再传
	  // 区值减去区列表的最后一条数据的id值再减去市列表的最后一条数据的id值，再传
	  this.getAddressList();  
	  // 设置大区数据
	  this.region = region.regionArr;
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
	  this.getSaleManInfo();//获取队员信息
  }



}
</script>
<style scoped>
  .teammemberedit button { width: 35%; height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 30%;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
  .teammemberedit .weui-cell__ft:after{height: 8px !important;width: 8px !important;}
  .vux-popup-picker-header-menu-right{color: #E83428 !important;}
  .teammemberedit .weui-label {font-size: 13px;color: #3e3e3e;}
  .teammemberedit .weui-input,.vux-popup-picker-value{font-size: 13px;color: #888888;}
  .vux-x-input,.weui-cell{border-bottom: 1px solid #d9d9d9;}
  .weui-cell:before{border-top: 0 !important;}
</style>
