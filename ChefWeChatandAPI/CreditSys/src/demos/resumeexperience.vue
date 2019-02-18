<template>
  <div class="resumeexperience">
  	
  	<div class="exDiv">  		
	  	<p class="ex_text">完善此板块信息，您将获得15积分的奖励！</p>
		  <x-input title="酒店名称" placeholder="请输入" v-model="hotelname"></x-input>
		  <datetime title="开始时间" v-model="startTime" :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align ='right'></datetime>
		  <datetime title="结束时间" v-model="endTime" :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align ='right'></datetime>
		  <popup-picker title="酒店地址"  :data="addresslist" v-model="hotelAddress" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>		  
		  <popup-picker title="岗位名称" :data="offerlist" :columns="2" v-model="offer" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>		  
  	</div>
  	<x-button @click.native='save'>保存</x-button>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import offer from '../offer.js'
import { XInput,PopupPicker,Datetime,XButton,cookie,Toast } from 'vux'
export default {
  components: {    
    XInput,
    PopupPicker,
    Datetime,
    XButton,
    cookie,
    Toast
  },
  data(){
  	return{
  		btnNum:0,
  		userId:'',
  		userData:{},
  		ResumeId:0,//从业经历id
  		hotelname:'',
  		startTime:'',
  		endTime:'',
  		addresslist:[],
  		hotelAddress:[],
  		offerlist:[],
  		offer:[],
  		resumedata:'',
  		num1: 0,
	    num2: 0,
	    num3: 0,
	    provinceList: [],
		  cityList: [],
		  areaList: [],
		  ProvinceId:0,
	    ProvinceName:'',
	    CityId:0,
	    CityName :'',
	    AreaId :0,
	    AreaName :'',
	    dataList:''
  	}
  },
  methods:{
  	getAddressList() {
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
						_this.addresslist.push({
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
						_this.addresslist.push({
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
						_this.addresslist.push({
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
	  	var provinceId = this.hotelAddress[0] - 0;
	  	var cityId = this.hotelAddress[1] - this.num1;
	  	var areaId = this.hotelAddress[2] - this.num2 - this.num1;
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
    getDate(){
    	var date=new Date();
			var year=date.getFullYear();   //年
			var month=date.getMonth()+1;   //月
			var day=date.getDate(); // 日
			var hour=date.getHours(); //时
			var minute=date.getMinutes(); //分
			var second=date.getSeconds(); //秒
			
			var month=addzero(month);
			var day=addzero(day);
			var hour=addzero(hour);
			var minute=addzero(minute);
			var second=addzero(second);
			function addzero(val){
				if(val<10){
					val="0"+val;
				}
				return val;
			}
			return year+'-'+month+'-'+day+' '+hour+':'+minute+':'+second;
    },
    save(){//点击保存
    	if(this.hotelname==''||this.startTime==''||this.endTime==''||this.hotelAddress==''||this.offer==''){
      	this.$vux.toast.text('内容不能为空', 'middle');
      }else{
      	this.btnNum++;
      	if(this.ResumeId==0&&this.btnNum==1){
      		this.saveMemberResume();//新增从业经历
      	}else if(this.ResumeId!=0&&this.btnNum==1){
      		this.updateMemberResume();//更新从业经历
      	}
      }
    },
    saveMemberResume(){//新增从业经历
    	  var _this=this;
      	this.switchAddress();
        var date=this.getDate();//当前时间
      	var params={    
			    "ProvinceId":this.ProvinceId,
					"ProvinceName":this.ProvinceName,
					"CityId":this.CityId,
					"CityName":this.CityName,
					"AreaId":this.AreaId,
					"AreaName":this.AreaName,
					"BeginTime":this.startTime,
					"EndTime":this.endTime,
					"MemberId":this.userId,
					"HotelName":this.hotelname,
					"HotelAddress":"",
					"Job":this.offer[0]+" "+this.offer[1],
					"HotelCode":"",
					"UpdateTime":date
		  	}
		  	this.$http({
		  		method:'post',
		  		url:apiUrl.saveMemberResume,
		  		data:params
		  	}).then(function(response){
			  		if(_this.dataList.length==0 && _this.btnNum==1){
			  			_this.$vux.toast.text('保存成功,您已增加15积分','middle');
	     	  		_this.updateMemberIntegral();//更改积分
			  		}else if(_this.dataList.length!=0){
			  			_this.$vux.toast.text('保存成功','middle');
			  		}
		        setTimeout(function(){
		      	  _this.$router.push('/component/resumeexperiences');
		        },500)
		  	})
    },
    getMemberResumeList(){//获取从业经历列表
  		var _this=this;
	    var params={
		  	'MemberId':this.userId
			}
			this.$http({
				method:'post',
				url:apiUrl.getMemberResumeList,
				data:params
			}).then(function(response){
				_this.dataList=JSON.parse(response.data);
			})
    },
    getMemberResume(){//获取单个从业经历
    	var _this=this;
    	var params={
		    "ResumeId":this.ResumeId
			}
			this.$http({
				method:'post',
				url:apiUrl.getMemberResume,
				data:params
			}).then(function(response){				
				_this.resumedata=JSON.parse(response.data)[0];
//				console.log(_this.resumedata);
				_this.hotelname=_this.resumedata.HotelName;
				_this.startTime=_this.resumedata.BeginTime.substring(0,10);
				_this.endTime=_this.resumedata.EndTime.substring(0,10);				
				var ProvinceId=(_this.resumedata.ProvinceId+0).toString();
				var CityId=(_this.resumedata.CityId+34).toString();
				var AreaId=(_this.resumedata.AreaId+34+358).toString();
				_this.hotelAddress=[ProvinceId,CityId,AreaId];
        var jobArr=_this.resumedata.Job.split("-");
				_this.offer=jobArr;
			})
    },
    updateMemberResume(){//更新从业经历
   	    var _this=this;
      	this.switchAddress();
        var date=this.getDate();
        var params={
  		    "ProvinceId":this.ProvinceId,
					"ProvinceName":this.ProvinceName,
					"CityId":this.CityId,
					"CityName":this.CityName,
					"AreaId":this.AreaId,
					"AreaName":this.AreaName,
					"BeginTime":this.startTime,
					"EndTime":this.endTime,
					"MemberId":this.userId,
					"HotelName":this.hotelname,
					"HotelAddress":"",
					"Job":this.offer[0]+" "+this.offer[1],
					"HotelCode":"",
					"UpdateTime":date,
					"ResumeId":this.ResumeId
  		  }
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.updateMemberResume,
	  			data:params
	  		}).then(function(response){
		  		_this.$vux.toast.text('保存成功','middle');
	        setTimeout(function(){
	      	  _this.$router.push('/component/resumeexperiences');
	        },500)
	  		})
    },
    updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':15,
				'MemberId':this.userId,
				'IntegralSource':'简历完善',
				'Remark':'从业经历'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
//			console.log(response);
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
  	this.getAddressList();
  	this.offerlist=offer.offerArr; 	
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
  	this.ResumeId=this.$route.query.resumeId;
  	if(this.ResumeId!=0){
  		this.getMemberResume();//获取单个从业经历
  	}
  	this.getMemberResumeList();//获取从业经历列表
  }
}
</script>

<style scoped>
.exDiv{margin-left: 15px;overflow-x: hidden;}
.ex_text{color: #E83428;font-size: 12px;margin-top: 15px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumeexperience .weui-cell:before{border-top: 0px !important;}
.resumeexperience .vux-cell-box:before{border-top: 0px !important;}
.resumeexperience .weui-cell{padding: 10px 18px 10px 0 !important;border-bottom: 1px solid #c8c7cc;}
.resumeexperience .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
.resumeexperience .vux-label {font-size: 14px;color: #3e3e3e;}
.resumeexperience .weui-label {font-size: 14px;color: #3e3e3e;}
.resumeexperience .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;}
.resumeexperience .weui-btn:after {border: none;}
.vux-popup-picker-header-menu-right { color: #e83428 !important; }
.dp-header .dp-item.dp-right { color: #e83428 !important; }
.resumeexperience .weui-input {text-align: right;font-size: 12px;color: #a3a3a3;}
.resumeexperience .weui-icon-clear:before{display:none;}
.resumeexperience .vux-datetime {color: #3e3e3e;font-size: 15px;}
.resumeexperience .weui-input,.resumeexperience .vux-popup-picker-value,.resumeexperience .weui-cell__ft{font-size: 14px;color: #3E3E3E;}
</style>