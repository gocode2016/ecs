<template>
	<div class='cookregister'>
		<div class="wdmicon">
			<p>专业厨师服务平台</p>
		</div>
		<div class="inputDiv">
			<group gutter='0.5rem'>
				<x-input title="联系方式" v-model="registerParams.Phone" keyboard="number" is-type="china-mobile"></x-input>
			</group>
			<group gutter='0.5rem'>
				<x-input title="姓名" v-model="registerParams.MemberName"></x-input>
			</group>
			<group gutter='0.5rem'>
				<popup-picker title="酒店区域" :data="addresslist" v-model="hotelarea" :columns="3" show-name cancel-text='取消' confirm-text='确定' value-text-align="left"></popup-picker>
			</group>
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
			<button @click="submit" class="submit">提交</button>
		</div>

		<!--注册成功提示页面-->
		<div class="sucDiv" v-show='sucFlag'>
			<img src="../../static/credit/success.png" />
			<p>恭喜你！注册成功</p>
			<p>即将跳转到个人中心</p>
		</div>
	</div>
</template>

<script>
	import apiUrl from '../apiUrls.js';
	import { Cell, Group, XInput, PopupPicker, Alert, XButton, Loading, Toast, cookie } from 'vux'
	var flag = 0;
	var btnNum = 0;
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
		data() {
			return {
				userData: {},
				userId: '',
				userType: '',
				sucFlag: false, //显示注册成功页面
				num1: 0,
			    num2: 0,
			    num3: 0,
			    provinceList: [],
				cityList: [],
				areaList: [],
				addresslist:[],//省市区联动数据
				hotelarea:[],//选中省市区
				identity:0,//注册人身份 1酒店从业者 2调味品供货商 3美食爱好者
				registerParams: {
					"Phone": '', //手机
					"MemberName":'',//姓名
					"ProvinceId": 0,
					"ProvinceName": "",
					"CityId": 0,
					"CityName": "",
					"AreaId": 0,
					"AreaName": "",
					"OpenId": '',
					"Nickname": '',
					"HeadImgUrl": ''
				}
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
				this.registerParams.ProvinceId = provinceId;
				this.registerParams.ProvinceName = provinceName;
				this.registerParams.CityId = cityId;
				this.registerParams.CityName = cityName;
				this.registerParams.AreaId = areaId;
				this.registerParams.AreaName = areaName;
				
				//提交注册信息
				this.submitApi(); 
			},
			//点击提交 进行判断
			submit() {
				if(this.registerParams.Phone == '' || this.registerParams.Name == '' || this.hotelarea == '' || this.identity == 0) {
					this.showPlugin('请将信息填写完整');
				}else{
					this.getTele(); //验证手机号
				}
			},
			getTele() {//验证手机号
				var _this = this;
				var box = /^1[3|4|5|7|8]\d{9}$/;
				if(!box.test(this.registerParams.Phone)) {
					this.showPlugin('请输入正确的手机号');
					flag = 1;
				} else {
					this.$http({
						method: 'POST',
						url: apiUrl.checkMemberTelephone,
						data: {
							'TelePhone': this.registerParams.Phone
						}
					}).then(function(respose) {
						if(respose.data != 0) {
							//不可以注册
							flag = 1;
							_this.showPlugin('该手机号已注册');
						} else {
							btnNum++;
							if(btnNum == 1) {
								_this.showLoading(); //显示loading
								_this.switchAddress();//获取省市区需要参数
							}
						}
					})
				}
			},
			submitApi() {//提交接口
				var _this = this;
				var JobType='';
				if(this.identity == 1){//酒店从业者
					JobType = '酒店从业者';
				}else if(this.identity == 2){//调味品供货商
					JobType = '调味品供货商';
				}else if(this.identity == 3){//美食爱好者
					JobType = '美食爱好者';
				}
				var params = {
					"Phone": this.registerParams.Phone,
					"MemberName": this.registerParams.MemberName,
					"JobType": JobType,
				    "ProvinceId": this.registerParams.ProvinceId,
					"ProvinceName": this.registerParams.ProvinceName,
					"CityId": this.registerParams.CityId,
					"CityName": this.registerParams.CityName,
					"AreaId": this.registerParams.AreaId,
					"AreaName": this.registerParams.AreaName,
					"OpenId": this.registerParams.OpenId,
					"Nickname": this.registerParams.Nickname,
					"HeadImgUrl": this.registerParams.HeadImgUrl,
					"Remark": "厨师节"
				}
//				console.log(params);
				this.$http({
					method: 'POST',
					url: apiUrl.createVisitorByCookday,
					data: params
				}).then(function(response) {
					var data = JSON.parse(response.data);
					if(typeof(data) != "number" || data == -1) { //需要返回会员Id     
						_this.$vux.loading.hide(); //loading图标消失
						setTimeout(function() {
							_this.showPlugin('提交信息失败,请重新提交');
							btnNum = 0;
						}, 1)
					} else { //注册成功 跳转到厨师节领奖页面
						_this.userData.UserId = data;
						_this.userData.UserType = 2;
						var cookieValue = [];
						for(var i in _this.userData) {
							cookieValue.push(i + "=" + _this.userData[i]);
						}
						cookieValue = cookieValue.join("&");
						_this.setCookie('WeiXinUser', cookieValue);

						_this.$vux.loading.hide(); //loading图标消失
						setTimeout(function() {
							_this.sucFlag = true;
						}, 1)

						setTimeout(function() {//跳转到厨师节页面
							_this.$router.push('/component/cookday');
						}, 1000)
					}
				}).catch(function(){
					_this.$vux.loading.hide(); //loading图标消失
					_this.showPlugin('提交信息失败,请重新提交');
					btnNum = 0;
				})
			},
			selectType(val){//选择身份
				if(this.identity == 0 || val != this.identity){
					this.identity = val;
				}else if(val == this.identity){
					this.identity = 0;
				}
			},
			setCookie(cname, cvalue, exdays) { //设置cookie
				var d = new Date();
				d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
				var expires = "expires=" + d.toUTCString();
				document.cookie = cname + "=" + cvalue + "; " + expires;
			},
			//提醒样式
			showPlugin(text) {
				var _this = this;
				this.$vux.alert.show({
					content: this.$t(text),
					onHide() {
						if(flag == 1) {
							_this.registerParams.Phone = '';
							flag = 0;
						}
					}
				})
			},
			showLoading() { //loading 图标
				this.$vux.loading.show({
					text: 'Loading'
				})
			},
			onChange(val) {
				const _this = this
				if(val) {
					this.$vux.toast.show({
						text: 'Hello World',
						onShow() {
							console.log('Plugin: I\'m showing')
						},
						onHide() {
							console.log('Plugin: I\'m hiding')
							_this.show9 = false
						}
					})
				} else {
					this.$vux.toast.hide()
				}
			}
		},
		mounted: function() {
			
            this.getRegion();//获取省市区联动数据
			//获取用户信息
			this.userData = cookie.get('WeiXinUser', function(val) {
				var a = val.split("&");
				var obj = {};
				for(var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
			})
			this.userId = parseInt(this.userData.UserId);
			this.userType = parseInt(this.userData.UserType);
			this.registerParams.OpenId = this.userData.Openid;
			this.registerParams.Nickname = decodeURI(this.userData.Nickname);
			this.registerParams.HeadImgUrl = this.userData.Headimgurl;

			//判断是否注册会员
			if(this.userId > 0 && this.userType == 2) {
				this.$router.push('/component/cookday');
			} else if(this.userId > 0 && this.userType == 1) { //已经是队员  不能注册会员
				this.$router.push('/component/teammember');
			}

			//设置弹框字段
			$('.weui-dialog__btn_primary').html('确定');

		}
	}
</script>
<style scoped>
	.wdmicon {
		height: 20%;
		background: url('../../static/credit/register_icon.png') no-repeat center;
		background-size: 100% 100%;
		overflow: hidden;
	}
	
	.wdmicon p {
		text-align: center;
		color: #e73229;
		font-size: 22px;
		margin-top: 10%;
		letter-spacing: 1px;
	}
	
	.inputDiv {
		width: 90%;
		margin-left: 5%;
	}
	
	.identcode,
	.countNum {
		position: absolute;
		right: 10px;
		font-size: 13px;
		display: inline-block;
		top: 13px
	}
	
	.identcode {
		color: #e83428;
	}
	
	.countNum {
		color: #b1b1b1;
	}
	
	.cookregister .submit {
		width: 100%;
		margin-top: 30px;
		background: #e83428;
		border: none;
		color: #fff;
		font-size: 16px;
		border-radius: 5px;
		height: 46px;
	}
	
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
		border: 1px solid #ddd;
		color: #6b6b6b;
		text-align: center;
		font-size: 13px;
		line-height: 30px;
		border-radius: 5px;
		outline: none;
		width: 90%;
	}
	
    .selectStyle{
		border: 1px solid #e83428 !important;
    	color:#e83428 !important;
    }
    	
	.select{
		display: inline-block;
		width: 18px;
		height:18px;
		background: #ddd;
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
	    color: #fff;
	    border-bottom: 1.5px solid;
	    border-right: 1.5px solid;
	    margin-left: 6px;
	    margin-top: 3px;
	    transform-origin: center;
	    transform:rotate(45deg);
	    -webkit-transform:rotate(45deg);
	}
	
	.selectIcon{
		background:#e83428;
	}
	
	/*会员注册成功页面*/
	
	.sucDiv {
		width: 100%;
		height: 100%;
		position: fixed;
		top: 0px;
		background: #FFFFFF;
		text-align: center;
	}
	
	.sucDiv img {
		width: 160px;
		margin-top: 90px;
	}
	
	.sucDiv p:nth-child(2) {
		font-size: 17px;
		color: #E83428;
		margin-top: 15px;
	}
	
	.sucDiv p:nth-child(3) {
		font-size: 13px;
		color: #707070;
		margin-top: 10px;
	}
</style>

<style>
	.cookregister {
		background: url('../../static/credit/register_bgnew.png') no-repeat left bottom;
		background-size: 100% 100%;
		overflow: hidden;
		height: 100%;
		width: 100%
	}
	
	.cookregister .weui-label {
		width: 4.5rem !important;
	}
	
	.cookregister .weui-cells {
		border: 1px solid #dddddd;
		border-radius: 5px;
		margin-top: 20px !important;
		position: relative;
	}
	
	.vux-popup-picker-header-menu-right {
		color: #e83428 !important;
	}
	
	.weui-dialog__btn_primary {
		color: #e83428 !important;
	}
	
	.weui-cells:before {
		border-top: 0 !important;
	}
	
	.weui-cells:after {
		border-bottom: 0 !important;
	}
	
	.vux-cell-box:before {
		border-top: 0 !important;
	}
	
	.vux-cell-box:after {
		border-bottom: 0 !important;
	}
	
	.cookregister .weui-cell {
		font-size: 13px;
		color: #3e3e3e;
		height: 46px
	}
	
	.cookregister .weui-cell__ft:after {
		display: none !important;
	}
	
	.cookregister .weui-vcode .weui-icon-clear:before {
		display: none !important;
	}
	
	.cookregister .weui-cell {
		padding: 0 15px!important;
	}
	
	.cookregister .weui-input::-webkit-input-placeholder {
		/*placeholder颜色*/
		color: #bebebe;
	}
</style>