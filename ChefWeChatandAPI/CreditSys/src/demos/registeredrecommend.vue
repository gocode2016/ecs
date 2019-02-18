<template>
	<div class='registeredrecommend'>
		<div class="wdmicon">
			<p>专业厨师服务平台</p>
		</div>
		<div class="inputDiv">
			<p style="color: #E83428;font-size: 12px;">注册成功可增加20积分</p>
			<group gutter='0.5rem'>
				<x-input title="真实姓名" v-model="registerParams.MemberName"></x-input>
			</group>
			<group gutter='0.5rem'>
				<x-input title="联系方式" v-model="registerParams.TelePhone" keyboard="number" is-type="china-mobile"></x-input>
			</group>
			<group gutter='0.5rem'>
				<x-input title="短信验证码" class="weui-vcode" v-model="identcode" @on-blur="testIdent"></x-input>
				<span class='identcode' @click="getIdent" v-if="isShow">{{indentText}}</span>
				<span class='countNum' v-else>{{countDown}}秒</span>
			</group>
			<group gutter='0.5rem'>
				<popup-picker title="工作岗位" :data="offer" :columns="2" v-model="registerParams.offerArr" @on-change="offerShow" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
			</group>

			<div v-if="offerId==1">
				<group gutter='0.5rem'>
					<x-input title="酒店区域" v-if="offerId==1&&IsHave==0" v-model="registerParams.addressArr[0]" :readonly='true' @on-focus="getLocation" show-name value-text-align='left'></x-input>
					<popup-picker title="酒店区域" v-if="offerId==1&&IsHave==1" :data="address" :columns="3" v-model="registerParams.addressArr" @on-change="getCity" show-name value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<x-input title="酒店名称" v-if="offerId==1&&IsHave==0" :readonly='true' v-model="registerParams.HotelName" @on-focus="getHoteldesc"></x-input>
					<x-input title="酒店名称" v-if="offerId==1&&IsHave==1" v-model="registerParams.HotelName"></x-input>
				</group>
				<group gutter='0.5rem'>
					<x-input title="酒店地址" v-if="offerId==1&&IsHave==0" :readonly='true' v-model="registerParams.HotelArea" @on-focus="getHoteladd"></x-input>
					<x-input title="酒店地址" v-if="offerId==1&&IsHave==1" v-model="registerParams.HotelArea"></x-input>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="酒店主营" :data="hotelselllist" v-model="registerParams.HotelSell" value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="酒店人均消费" :data="hotelSpend" v-model="registerParams.HotelSpend" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="现在使用欣和产品" :data="ifUse" v-model="registerParams.IfUse" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="调味品采购权" :data="productSource" v-model="registerParams.ProductSource" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
			</div>

			<div v-if="offerId==2">
				<group gutter='0.5rem'>
					<datetime title="生日" v-model="registerParams.MemberBirthday" :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align='right'></datetime>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="性别" :data="MemberGender" v-model="registerParams.MemberGender" cancel-text='取消' confirm-text='确定' show-name value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<x-input title="家庭区域" v-if="offerId==2&&IsHave==0" v-model="registerParams.addressArr[0]" :readonly='true' @on-focus="getLocation" show-name value-text-align='left'></x-input>
					<popup-picker title="家庭区域" v-if="offerId==2&&IsHave==1" :data="address" :columns="3" v-model="registerParams.addressArr" @on-change="getCity" show-name value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<x-input title="家庭地址" v-if="offerId==2&&IsHave==0" :readonly='true' v-model="registerParams.HotelArea" @on-focus="getHoteldesc"></x-input>
					<x-input title="家庭地址" v-if="offerId==2&&IsHave==1" v-model="registerParams.HotelArea"></x-input>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="婚姻状况" :data="Marriage" v-model="registerParams.Marriage" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="是否有孩子" :data="ifUse" v-model="registerParams.HaveChild" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="您的家庭收入" :data="HomeIncome" v-model="registerParams.HomeIncome" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="您的烹饪频率" :data="CookieNum" v-model="registerParams.CookieNum" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
			</div>

			<div v-if="offerId==3">
				<group gutter='0.5rem'>
					<x-input title="所在批发市场名称" v-model="registerParams.MarketsaleName"></x-input>
				</group>
				<group gutter='0.5rem'>
					<x-input title="门店区域" v-if="offerId==3&&IsHave==0" v-model="registerParams.addressArr[0]" :readonly='true' @on-focus="getLocation" show-name value-text-align='left'></x-input>
					<popup-picker title="门店区域" v-if="offerId==3&&IsHave==1" :data="address" :columns="3" v-model="registerParams.addressArr" @on-change="getCity" show-name value-text-align='left'></popup-picker>
				</group>
				<group gutter='0.5rem'>
					<x-input title="门店地址" v-if="offerId==3&&IsHave==0" :readonly='true' v-model="registerParams.HotelArea" @on-focus="getHoteldesc"></x-input>
					<x-input title="门店地址" v-if="offerId==3&&IsHave==1" v-model="registerParams.HotelArea"></x-input>
				</group>
				<group gutter='0.5rem'>
					<x-input title="门店名称" v-if="offerId==3&&IsHave==0" :readonly='true' v-model="registerParams.HotelName" @on-focus="getHoteladd"></x-input>
					<x-input title="门店名称" v-if="offerId==3&&IsHave==1" v-model="registerParams.HotelName"></x-input>
				</group>
				<group gutter='0.5rem'>
					<popup-picker title="门店经营范围" :data="MarketScope" v-model="registerParams.MarketScope" show-name cancel-text='取消' confirm-text='确定' value-text-align='left'></popup-picker>
				</group>
			</div>
			<button @click="submitClick">确认</button>
		</div>
		<!--注册成功提示页面-->
		<div class="sucDiv" v-show='sucFlag'>
			<img src="../../static/credit/success.png" />
			<p>恭喜你！注册成功</p>
			<p>即将跳转的个人中心</p>
		</div>
		<!-- 筛选酒店名称地址页面 -->
		<div class="selectDiv" v-if='isSele'>
			<div class="selectbox">
				<ul v-for="(item,index) in HotelList">
					<li v-bind:class="{ selectColor: index==isActive }" @click='selectMethods(index,item)'>
						<p>{{item.name}}</p>
						<p>{{item.adress}}</p>
					</li>
				</ul>
			</div>
		</div>
		<!-- 没找到地址页面 -->
		<div class="noHaveDiv" v-if='isNone'>
			<div class='noHavebox'>
				<div>
					<p>没有找到您在地图上的位置</p>
					<p>请手动输入酒店信息</p>
				</div>
				<div @click="isNone=false">
					<p>确定</p>
				</div>
			</div>
		</div>
		<!-- 遮罩层页面 -->
		<div class="cover" v-if='isShowcover'></div>

	</div>
</template>
<script>
	import apiUrl from '../apiUrls.js';
	import offer from '../jobs.js';
	import wx from 'weixin-js-sdk'
	import { Cell, Group, XInput, PopupPicker, Alert, XButton, Loading, Toast, Datetime, cookie } from 'vux'
	var flag = 0;
	var btnNum = 0;
	// 所有值是为0，否为1
	// 身份信息为1,2,3类
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
			Datetime,
			cookie
		},
		data() {
			return {
				sucFlag: false, //显示注册成功页面
				indentText: '获取验证码',
				isShow: true,
				userData: {},
				num1: 0,
				num2: 0,
				num3: 0,
				countDown: 60,
				provinceList: [],
				cityList: [],
				areaList: [],
				url1: apiUrl.checkMemberTelephone, // 手机号API
				url2: apiUrl.sendRegistSMS, // 验证码API
				url4: apiUrl.createMember, // 提交API
				identcode: '', // 短信验证码
				identcode1: '', // 返回短信验证码
				offerId: '1', //身份信息，1为普通，2为美食爱好者，3为调味品供货商
				offer: [],
				address: [],
				IsHave: '1', //是否自动定位，0为自动定位，1为手动输入
				isShowcover: false, //遮罩层
				isSele: false, //筛选框
				isNone: false, //获取位置返回酒店为空
				isActive: -1, //筛选酒店添加类名
				timestamp: '',
				nonceStr: '',
				signature: '',
				hotelSpend: [], //酒店人均消费
				registerParams: {
					"RecommendId": 0, //推荐人id 
					"offerArr": [],
					"addressArr": [], //酒店区域 家庭区域 门店区域
					"HotelArea": '', //酒店地址 家庭地址 门店地址
					"HotelName": '', //酒店名称 门店名称
					"ProvinceId": 0,
					"ProvinceName": '', //省
					"CityId": 0,
					"CityName": '', //市
					"AreaId": 0,
					"AreaName": '', //区
					"MemberName": '', //成员名
					"TelePhone": '',
					"Position": '', //职位
					"PositionType": '', //职位类别
					"OpenId": '',
					"NickName": '',
					"HeadImgUrl": '',
					"HotelSell": [], //酒店主营
					"HotelSpend": [], //酒店人均消费
					"IfUse": [], //是否使用产品
					"ProductSource": [], //采购权
					"MemberGender": [], //性别
					"MemberBirthday": '', //生日
					"Marriage": [], //婚姻
					"HaveChild": [], //孩子
					"HomeIncome": [], //家庭收入
					"CookieNum": [], //做饭频率
					"MarketsaleName": '', //批发市场名称
					"MarketScope": [] //门店经营范围
				},
				hotelselllist: [
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
				ifUse: [
					[{
						name: '是',
						value: '1'
					}, {
						name: '否',
						value: '0'
					}]
				],
				productSource: [
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
				MemberGender: [
					[{
						name: '男',
						value: '1'
					}, {
						name: '女',
						value: '0'
					}]
				],
				Marriage: [
					[{
						name: '已婚',
						value: '1'
					}, {
						name: '未婚',
						value: '0'
					}]
				],
				HomeIncome: [
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
				CookieNum: [
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
				MarketScope: [
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
				HotelList: [{
					name: '北京市欣和酒店',
					value: '北京市欣和酒店111',
					adress: '北京市朝阳区123号'
				}, {
					name: '北京市欣和酒店222',
					value: '北京市欣和酒店',
					adress: '北京市朝阳区123号'
				}, {
					name: '北京市欣和酒店333',
					value: '北京市欣和酒店',
					adress: '北京市朝阳区123号'
				}, {
					name: '北京市欣和酒店333',
					value: '北京市欣和酒店',
					adress: '北京市朝阳区123号'
				}],
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
				]
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
					console.log(data);
					//省
					var provinceList = data.Province;
					_this.num1 = provinceList[provinceList.length - 1].ProvinceId;
					for(let i in provinceList) {
						_this.address.push({
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
						_this.address.push({
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
						_this.address.push({
							name: areaList[i].AreaName.toString(),
							value: idx.toString(),
							parent: idx2.toString(),
							areaLevel: areaList[i].AreaLevel
						})
					}
					_this.areaList = areaList;
					console.log(_this.address);
				})
			},
			switchAddress() {
				// 已获取的后台省市区数据
				var provinceList = this.provinceList;
				var cityList = this.cityList;
				var areaList = this.areaList;
				// 将前台省市区联动数据转换回后台省市区数据
				// 转换省市区id
				var provinceId = Number(this.registerParams.addressArr[0]) - 0;
				var cityId = Number(this.registerParams.addressArr[1]) - this.num1;
				var areaId = Number(this.registerParams.addressArr[2]) - this.num2 - this.num1;
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
				
				//提交推荐同行注册信息
				this.recommendMemberRegist();
			},
			submitClick() { //点击提交  判读是否填写完整
				//没有填写完整 提示
				if(this.offerId == 1) {
					if(this.registerParams.MemberName == '') {
						this.showPlugin('真实姓名不能为空');
					} else if(this.registerParams.TelePhone == '') {
						this.showPlugin('手机号不能为空');
					} else if(this.identcode == '') {
						this.showPlugin('短信验证码不能为空');
					} else if(this.registerParams.offerArr == '') {
						this.showPlugin('工作岗位不能为空')
					} else if(this.registerParams.HotelArea == '') {
						this.showPlugin('酒店地址不能为空');
					} else if(this.registerParams.HotelName == '') {
						this.showPlugin('酒店名称不能为空');
					} else if(this.registerParams.addressArr == '') {
						this.showPlugin('酒店区域不能为空');
					} else if(this.registerParams.HotelSell == '') {
						this.showPlugin('酒店主营不能为空');
					} else if(this.registerParams.HotelSpend == '') {
						this.showPlugin('酒店人均消费不能为空');
					} else if(this.registerParams.IfUse == '') {
						this.showPlugin('现在使用欣和产品不能为空');
					} else if(this.registerParams.ProductSource == '') {
						this.showPlugin('调味品采购权不能为空');
					} else if(this.registerParams.OpenId==''||this.registerParams.OpenId==undefined||this.registerParams.OpenId==null){
        				this.showPlugin ('出现错误，请重新提交');
          			}else if(this.identcode!=this.identcode1){
						this.showPlugin('短信验证码有误');
						flag = 2;
          			}else{
          				this.getTele();//验证手机号
          			}
				}else if(this.offerId == 2) { //美食爱好者
					if(this.registerParams.MemberName == '') {
						this.showPlugin('真实姓名不能为空');
					} else if(this.registerParams.TelePhone == '') {
						this.showPlugin('手机号不能为空');
					} else if(this.identcode == '') {
						this.showPlugin('短信验证码不能为空');
					} else if(this.registerParams.offerArr == '') {
						this.showPlugin('工作岗位不能为空')
					} else if(this.registerParams.MemberBirthday == '') {
						this.showPlugin('生日不能为空');
					} else if(this.registerParams.MemberGender == '') {
						this.showPlugin('性别不能为空');
					} else if(this.registerParams.addressArr == '') {
						this.showPlugin('家庭区域不能为空');
					} else if(this.registerParams.HotelArea == '') {
						this.showPlugin('家庭地址不能为空');
					} else if(this.registerParams.Marriage == '') {
						this.showPlugin('婚姻状况不能为空');
					} else if(this.registerParams.HaveChild == '') {
						this.showPlugin('是否有孩子不能为空');
					} else if(this.registerParams.HomeIncome == '') {
						this.showPlugin('您的家庭收入不能为空');
					} else if(this.registerParams.CookieNum == '') {
						this.showPlugin('您的烹饪频率不能为空');
					}else if(this.registerParams.OpenId==''||this.registerParams.OpenId==undefined||this.registerParams.OpenId==null){
        				this.showPlugin ('出现错误，请重新提交');
          			}else if(this.identcode!=this.identcode1){
						this.showPlugin('短信验证码有误');
						flag = 2;
			        }else{
			          	this.getTele();//验证手机号
			        }
				}else if(this.offerId == 3) { //调味品供货商
					if(this.registerParams.MemberName == '') {
						this.showPlugin('真实姓名不能为空');
					} else if(this.registerParams.TelePhone == '') {
						this.showPlugin('手机号不能为空');
					} else if(this.identcode == '') {
						this.showPlugin('短信验证码不能为空');
					} else if(this.registerParams.offerArr == '') {
						this.showPlugin('工作岗位不能为空')
					} else if(this.registerParams.MarketsaleName == '') {
						this.showPlugin('所在批发市场名称不能为空');
					} else if(this.registerParams.addressArr == '') {
						this.showPlugin('门店区域不能为空');
					} else if(this.registerParams.HotelArea == '') {
						this.showPlugin('门店地址不能为空');
					} else if(this.registerParams.HotelName == '') {
						this.showPlugin('门店名称不能为空');
					} else if(this.registerParams.MarketScope == '') {
						this.showPlugin('门店经营范围不能为空');
					}else if(this.registerParams.OpenId==''||this.registerParams.OpenId==undefined||this.registerParams.OpenId==null){
        				this.showPlugin ('出现错误，请重新提交');
          			}else if(this.identcode!=this.identcode1){
						this.showPlugin('短信验证码有误');
						flag = 2;
          			}else{
          				this.getTele();//验证手机号
          			}
				}
			},
			recommendMemberRegist() { //推荐同行注册
				var _this=this;
				var IsUseShinho = Number(this.registerParams.IfUse[0]); //是否使用欣和产品
				var Sex = Number(this.registerParams.MemberGender[0]); //性别
				var IsMarry = Number(this.registerParams.Marriage[0]); //婚姻状况
				var IsAnyChild = Number(this.registerParams.HaveChild[0]); //是否有孩子

				if(this.offerId == 1) { //酒店会员身份 以下字段置空
					//美食爱好者
					this.registerParams.MemberBirthday = ""; //生日
					Sex = ""; //性别
					IsMarry = ""; //婚姻状况
					IsAnyChild = ""; //是否有孩子
					this.registerParams.HomeIncome[0] = ""; //家庭收入
					this.registerParams.CookieNum[0] = ""; //烹饪频率
					//调味品供货商
					this.registerParams.MarketsaleName = ""; //批发市场名称
					this.registerParams.MarketScope[0] = ""; //门店经营范围
				} else if(this.offerId == 2) { //美食爱好者
					//酒店会员 
					this.registerParams.HotelName = ""; //酒店名称
					this.registerParams.HotelSell[0] = ""; //酒店主营
					this.registerParams.HotelSpend[0] = ""; //酒店人均消费
					IsUseShinho = ""; //是否使用欣和产品
					this.registerParams.ProductSource[0] = ""; //调味品采购权
					//调味品供货商
					this.registerParams.MarketsaleName = ""; //批发市场名称
					this.registerParams.MarketScope[0] = ""; //门店经营范围
				} else if(this.offerId == 3) { //调味品供货商
					//酒店会员 
					this.registerParams.HotelSell[0] = ""; //酒店主营
					this.registerParams.HotelSpend[0] = ""; //酒店人均消费
					IsUseShinho = ""; //是否使用欣和产品
					this.registerParams.ProductSource[0] = ""; //调味品采购权
					//美食爱好者
					this.registerParams.MemberBirthday = ""; //生日
					Sex = ""; //性别
					IsMarry = ""; //婚姻状况
					IsAnyChild = ""; //是否有孩子
					this.registerParams.HomeIncome[0] = ""; //家庭收入
					this.registerParams.CookieNum[0] = ""; //烹饪频率
				}

				var params = {
					'MemberRecId': this.registerParams.RecommendId, //推荐人id
					'Phone': this.registerParams.TelePhone,
					'OpenId': this.registerParams.OpenId,
					'Nickname': this.registerParams.NickName,
					'HeadImgUrl': this.registerParams.HeadImgUrl,
					'MemberName': this.registerParams.MemberName,
					'PositionType': this.registerParams.offerArr[0],
					'Position': this.registerParams.offerArr[1],
					'ProvinceId': this.registerParams.ProvinceId,
					'ProvinceName': this.registerParams.ProvinceName,
					'CityId': this.registerParams.CityId,
					'CityName': this.registerParams.CityName,
					'AreaId': this.registerParams.AreaId,
					'AreaName': this.registerParams.AreaName,
					'HotelName': this.registerParams.HotelName, //酒店名称 门店名称
					'HotelAddress': this.registerParams.HotelArea, //酒店地址 门店地址 家庭地址
					//酒店会员 
					'OperationType': this.registerParams.HotelSell[0], //酒店主营
					'PerConsumption': this.registerParams.HotelSpend[0], //酒店人均消费
					'IsUseShinho': IsUseShinho, //是否使用欣和产品
					'Purchaser': this.registerParams.ProductSource[0], //调味品采购权
					//美食爱好者
					'Birthday': this.registerParams.MemberBirthday, //生日
					'Sex': Sex, //性别
					'IsMarry': IsMarry, //婚姻状况
					'IsAnyChild': IsAnyChild, //是否有孩子
					'HomeIncome': this.registerParams.HomeIncome[0], //家庭收入
					'CookRate': this.registerParams.CookieNum[0], //烹饪频率
					//调味品供货商
					'WholesaleName': this.registerParams.MarketsaleName, //批发市场名称
					'ShopOperateSize': this.registerParams.MarketScope[0] //门店经营范围
				}
				this.$http({
					method: 'post',
					url: apiUrl.recommendMemberRegist,
					data: params
				}).then(function(response) {
//					alert(response.data);
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
	         	  
			         	_this.updateMemberIntegral(20,data,2);//当前注册用户增加20积分
				        if(_this.registerParams.RecommendId!=0){//推荐人ID 推荐同行注册
				           	_this.getRecQualifications();//获取推荐人的推荐总人数 增加积分
				        }else{//正常不会进入该判断
               				_this.$vux.loading.hide();//loading图标消失
		           			setTimeout(function(){             	 	 
		         	 	 		 _this.sucFlag=true;
		         	  		},1)  
				         	setTimeout(function(){
				         	 	_this.$router.push('/component/personal');
				         	},1000) 
		                }
	        		}
				})
			},
			//验证手机号
			getTele() {
				var _this = this;
				var box = /^1[3|4|5|7|8]\d{9}$/;
				if(!box.test(this.registerParams.TelePhone)) {
					this.showPlugin('请输入正确的手机号');
					flag = 1;
				} else {
					this.$http({
						method: 'POST',
						url: this.url1,
						data: {
							'TelePhone': this.registerParams.TelePhone
						}
					}).then(function(respose) {
						if(respose.data != 0) {
							//不可以注册
							_this.showPlugin('该手机号已注册');
							flag = 1;
						}else{
							btnNum++;
							if(btnNum == 1) {
								_this.showLoading(); //显示loading
								_this.switchAddress();
							}
						}
					})
				}
			},
			//验证城市接口获取人均消费
			getCity(value) {
//				console.log(this.registerParams.addressArr);
				//				console.log(this.num2,this.num1);//358 34
				//区id
				var areaId = Number(value[2]) - this.num2 - this.num1;
				for(let i in this.areaList) {
					var arealist = this.areaList[i];
					if(areaId == arealist.AreaId) {
						var areaLevel = arealist.AreaLevel;
					}
				}
				// 调取验证城市接口，获取人均消费数组
				console.log(areaLevel);
				if(areaLevel == '一级') {
					this.hotelSpend = this.hotelSpend1;
				} else if(areaLevel == '二级') {
					this.hotelSpend = this.hotelSpend2;
				} else if(areaLevel == '三级') {
					this.hotelSpend = this.hotelSpend3;
				} else if(areaLevel == '四级') {
					this.hotelSpend = this.hotelSpend4;
				}
			},
			// 判定工作和岗位
			offerShow(value) {
				//				console.log(this.registerParams.offerArr);
				if(this.registerParams.offerArr[1] == '美食爱好者') {
					this.offerId = 2;
				} else if(this.registerParams.offerArr[1] == '调味品供货商') {
					this.offerId = 3;
				} else {
					this.offerId = 1;
				}
			},
			//获取验证码
			getIdent() {
				var _this = this;
				var box = /^1[3|4|5|7|8]\d{9}$/;
				if(!box.test(this.registerParams.TelePhone)) {
					this.showPlugin('请输入正确的手机号');
					flag = 1;
				} else {
					this.$http({
						method: 'POST',
						url: this.url1,
						data: {
							'TelePhone': this.registerParams.TelePhone
						}
					}).then(function(response) {
						//	 	    console.log(response.data);
						if(response.data != 0) { //不可以注册
							_this.showPlugin('该手机号已注册');
							flag = 1;
						} else { //可以注册 //60s倒计时
							_this.isShow = false;
							var count = setInterval(function() {
								_this.countDown--;
								if(_this.countDown == -1) {
									clearInterval(count);
									_this.countDown = 60;
									_this.isShow = true;
									_this.indentText = '重新获取';
								}
							}, 1000)
							_this.$http({
								method: 'POST',
								url: _this.url2,
								data: {
									'Telephone': _this.registerParams.TelePhone
								}
							}).then(function(respose) {
								_this.identcode1 = respose.data;
							})
						}
					})
				}
			},
			//检测验证码
			testIdent() {
				if(this.identcode != this.identcode1) {
					this.showPlugin('验证码不正确');
					flag = 2;
				}
			},
			getRecQualifications() {//获取推荐名额
				var _this = this;
				this.$http({
					method: 'get',
					url: apiUrl.getRecQualifications + '?MemberId=' +  this.registerParams.RecommendId
				}).then(function(response) {
					if(response.data <= 5) {//增加积分 根据推荐人数
						_this.updateMemberIntegral(20,_this.registerParams.RecommendId,1);
					}else if(response.data <= 10) {
						_this.updateMemberIntegral(30,_this.registerParams.RecommendId,1);
					}else if(response.data <= 15) {
						_this.updateMemberIntegral(40,_this.registerParams.RecommendId,1);
					}else if(response.data <= 20) {
						_this.updateMemberIntegral(50,_this.registerParams.RecommendId,1);
					}else {
					    _this.$vux.loading.hide(); //loading图标消失
						setTimeout(function() {
							_this.sucFlag = true;
						}, 1)
						setTimeout(function() {
							_this.$router.push('/component/personal');
						}, 1000)
					}
				})
			},
			updateMemberIntegral(val,userId,num) { //更改积分
				if(num==1){//给推荐人增加积分
					var IntegralSource='推荐奖励';
					var Remark='推荐同行注册';
				}else if(num==2){//当前注册人 增加积分
					var IntegralSource='注册';
					var Remark='简历完善';
				}
				var _this = this;
				var params = {
					'Operation': 'plus',
					'Integral': val,
					'MemberId': userId,
					'IntegralSource': IntegralSource,
					'Remark': Remark
				}
				this.$http({
					method: 'post',
					url: apiUrl.updateMemberIntegral,
					data: params
				}).then(function(response) {
					if(num==1){
					    _this.$vux.loading.hide(); //loading图标消失
						setTimeout(function() {
							_this.sucFlag = true;
						}, 1)
						setTimeout(function() {
							_this.$router.push('/component/personal');
						}, 1000)
					}
				})
			},
			setCookie(cname,cvalue,exdays){//设置cookie
			    var d = new Date();
			    d.setTime(d.getTime()+(exdays*24*60*60*1000));
			  	var expires = "expires="+d.toUTCString();
			  	document.cookie = cname + "=" + cvalue + "; " + expires;
			},
			//提醒样式
			showPlugin(text) {
				var _this = this;
				this.$vux.alert.show({
					content: this.$t(text),
					onHide() {
						if(flag == 1) {
							_this.registerParams.TelePhone = '';
							flag = 0;
						}
						if(flag == 2) {
							_this.identcode = '';
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
			},
			setConfig() { //配置config
				var _this = this;
				this.$http({
					method: 'GET',
					url: apiUrl.getTimestampAndNonceStr
				}).then(function(response) {
					var data = response.data;
					var dataArr = data.split('|');
					_this.timestamp = parseInt(dataArr[0]); //时间戳
					_this.nonceStr = dataArr[1]; //随机字符串

					var params = '?noncestr=' + _this.nonceStr + '&timestamp=' + _this.timestamp + '&url=' + _this.currentUrl;
					_this.$http({
						method: 'GET',
						url: apiUrl.getJsapiTicketSignature + params
					}).then(function(response) {
						_this.signature = response.data;
						wx.config({
							debug: false,
							appId: apiUrl.appId,
							timestamp: _this.timestamp,
							nonceStr: _this.nonceStr,
							signature: _this.signature,
							jsApiList: [
								'getLocation'
							]
						})
					})
				})
			},
			getLocation() { //调取定位
				console.log("开始调定位")
				var _this = this;
				console.log(_this.offerId)
				_this.IsHave = 0
				console.log(_this.IsHave)
				if(_this.offerId == 1) { //根据身份不同虚拟信息
					_this.getCity() //调取酒店人均消费，临时，需删
					_this.registerParams.addressArr.push("北京市朝阳区")
				} else if(_this.offerId == 2) {
					_this.registerParams.HomeAddress = []
					_this.registerParams.HomeAddress[0] = "北京市朝阳区"
					console.log(_this.registerParams.HomeAddress[0])
				} else if(_this.offerId == 3) {
					_this.registerParams.MarketArea = []
					_this.registerParams.MarketArea[0] = "北京市朝阳区"
					console.log(_this.registerParams.MarketArea[0])
				}
				// --------- 以上为虚拟数据  --------

				wx.getLocation({
					type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
					success: function(res) {
						var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
						var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
						var speed = res.speed; // 速度，以米/每秒计
						var accuracy = res.accuracy; // 位置精度
						if(data.length == '') {
							// 定位成功之后判断如果为空则 _this.IsHave=="1" 并弹出没找到提示指引手动输入并弹出遮罩层并显示为空弹框
							_this.IsHave = "1"
							_this.isShowcover = true;
							_this.isNone = true;
						} else {
							// 成功之后判断如果不为空则继续选择调取后台接口
							_this.IsHave == "0"
							// 调取接口获取值
							// 一类获取酒店区域，酒店名称，酒店地址
							// _this.registerParams.addressArr[0]=="北京市朝阳区";
							// _this.HotelList=_this.HotelList
							if(_this.offerId == 1) { //根据身份不同配置信息
								_this.getCity() //调取酒店人均消费，临时，需删
								//  _this.registerParams.addressArr=
								//  _this.HotelList=
							} else if(_this.offerId == 2) {
								//  _this.registerParams.HomeAddress=
								//  _this.HotelList=
							} else if(_this.offerId == 3) {
								//  _this.registerParams.MarketArea=
								//  _this.HotelList=
							}
						}
					},
					fail: function() {
						_this.IsHave == "1"
						_this.showPlugin("无法获取您的城市信息,请稍后重试!")
					},
					cancel: function() {
						_this.IsHave == "1"
						_this.showPlugin("无法获取您的城市信息,请允许获取您的位置信息")
					}
				});
			},
			getHoteldesc() { //获取酒店名称酒店地址
				var _this = this;
				if(_this.offerId == 1) { //根据身份不同模拟位置信息
					if(_this.registerParams.addressArr[0] == '' || _this.registerParams.addressArr[0] == undefined) {
						_this.showPlugin("请先输入酒店区域")
					} else {
						_this.isShowcover = true;
						_this.isSele = true;
					}
				} else if(_this.offerId == 2) {
					if(_this.registerParams.HomeAddress[0] == '' || _this.registerParams.HomeAddress[0] == undefined) {
						_this.showPlugin("请先输入家庭区域")
					} else {
						_this.isShowcover = true;
						_this.isSele = true;
					}
					_this.HotelList = [{
						name: '',
						value: '',
						adress: '1111'
					}, {
						name: '',
						value: '',
						adress: '2222'
					}, {
						name: '',
						value: '',
						adress: '333'
					}]
				} else if(_this.offerId == 3) {
					if(_this.registerParams.MarketArea[0] == '' || _this.registerParams.MarketArea[0] == undefined) {
						_this.showPlugin("请先输入门店区域")
					} else {
						_this.isShowcover = true;
						_this.isSele = true;
					}
					_this.HotelList = [{
						name: '111111',
						value: '11111',
						adress: '00001111'
					}, {
						name: '2222',
						value: '000222',
						adress: '0002222'
					}, {
						name: '333',
						value: '333',
						adress: '0000333'
					}, {
						name: '444',
						value: '444',
						adress: '0000444'
					}]
				}
			},
			getHoteladd() { //点位模式中给酒店地址加判断，酒店名称和酒店区域为空则不可填写
				var _this = this;
				if(_this.offerId == 1) { //根据身份不同模拟位置信息
					if(_this.registerParams.addressArr[0] == '' || _this.registerParams.addressArr[0] == undefined) { //酒店区域为空
						_this.showPlugin("请先输入酒店区域")
					} else {
						if(_this.registerParams.HotelName == '' || _this.registerParams.HotelName == undefined) { //酒店名称为空
							_this.showPlugin("请先输入酒店名称")
						}
					}
				} else if(_this.offerId == 3) { //根据身份不同模拟位置信息
					if(_this.registerParams.MarketArea[0] == '' || _this.registerParams.MarketArea[0] == undefined) { //门店区域为空
						_this.showPlugin("请先输入门店区域")
					} else {
						if(_this.registerParams.MarketAddress == '' || _this.registerParams.MarketAddress == undefined) { //门店地址为空
							_this.showPlugin("请先输入门店地址")
						}
					}
				}
			},
			selectMethods(index, item) { //获取位置后后台返回的选择酒店地址点击事件
				var _this = this;
				_this.isActive = index;
				console.log(item)
				_this.isShowcover = false;
				_this.isSele = false;
				if(_this.offerId == 1) { //根据身份给参数赋值
					_this.registerParams.HotelName = item.name
					_this.registerParams.HotelArea = item.adress
				} else if(_this.offerId == 2) {
					_this.registerParams.MemberAddress = item.adress
				} else if(_this.offerId == 3) {
					_this.registerParams.MarketName = item.name
					_this.registerParams.MarketAddress = item.adress
				}
			}
		},
		mounted: function() {
			this.getRegion(); //获取省市区
			this.offer = offer.offerArr;// 设置岗位数据
			//获取openId   
			this.userData = cookie.get('WeiXinUser', function(val) {
				var a = val.split("&");
				var obj = {};
				for(var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
			})
			this.registerParams.OpenId = this.userData.Openid;
			this.registerParams.NickName = decodeURI(this.userData.Nickname);
			this.registerParams.HeadImgUrl = this.userData.Headimgurl;

			if(this.userData.UserId>0&&this.userData.UserType==2){
			 	this.$router.push('/component/personal');
			}else if(this.userData.UserId>0&&this.userData.UserType==1){//已经是队员
			 	this.$router.push('/component/teammember');
			}

			//推荐ID   
			//      var locationUrl=window.location.href;
			//      if(locationUrl.indexOf('RecommendId')<0){//没有推荐id
			//      	this.MemberRecId=0;
			//      }else{
			//      	var RecommendArr=locationUrl.split('?')[1];
			//      	this.MemberRecId=parseInt(RecommendArr.split('=')[1]);
			//      }
			
			this.registerParams.RecommendId=this.$route.query.RecommendId;//推荐ID  
//			this.registerParams.RecommendId='30932';//测试

			//设置弹框字段
			$('.weui-dialog__btn_primary').html('确定');

			//获取当前url 配置config
			var uri = window.location.href.split('#')[0];
			this.currentUrl = encodeURIComponent(uri);
			this.setConfig();
		}
	}
</script>
<style scoped>
	::-webkit-scrollbar {
		display: none
	}
	
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
		height: 75%;
		overflow: scroll;
		margin-top: 3%;
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
	
	.registeredrecommend button {
		width: 100%;
		margin-top: 25px;
		background: #e83428;
		border: none;
		color: #fff;
		font-size: 16px;
		border-radius: 5px;
		height: 46px;
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
	.registeredrecommend {
		background: url('../../static/credit/register_bgnew.png') no-repeat left bottom;
		background-size: 100% 100%;
		height: 100%;
		width: 100%;
		position: relative;
	}
	
	.registeredrecommend .weui-cells {
		border: 1px solid #dddddd;
		border-radius: 5px;
		margin-top: 10px !important;
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
	
	.registeredrecommend .weui-cell {
		font-size: 13px;
		color: #3e3e3e;
		height: 46px
	}
	
	.registeredrecommend .weui-cell__ft:after {
		display: none !important;
	}
	
	.registeredrecommend .weui-vcode .weui-icon-clear:before {
		display: none !important;
	}
	
	.registeredrecommend .weui-cell {
		padding: 0 15px!important;
	}
	
	.registeredrecommend .weui-label {
		width: 130px !important;
	}
	
	.registeredrecommend .weui-input {
		height: 46px;
		line-height: 46px;
	}
	/* 自动定位页面 */
	
	.cover {
		height: 100%;
		width: 100%;
		background-color: rgba(0, 0, 0, .5);
		position: absolute;
		top: 0;
		left: 0;
	}
	/* 没找到位置页面 */
	
	.noHaveDiv {
		position: absolute;
		top: 35%;
		left: 0;
		text-align: center;
		height: 170px;
		width: 100%;
	}
	
	.noHavebox {
		margin: 0px 10%;
		background-color: #fff;
		border-radius: 2%;
	}
	
	.noHavebox div:nth-child(1) {
		height: 70px;
		padding: 20px 0;
	}
	
	.noHavebox div:nth-child(2) {
		height: 40px;
		color: red;
	}
	/* 定位后酒店名称酒店地址筛选弹框 */
	
	.selectDiv {
		position: absolute;
		top: 35%;
		left: 0;
		text-align: center;
		height: 180px;
		width: 100%;
		z-index: 5
	}
	
	.selectbox {
		margin: 0px 10%;
		height: 180px;
		background-color: #fff;
		border-radius: 2%;
		background-color: white;
		overflow-y: auto;
	}
	
	.selectbox ul {
		font-size: 14px;
		text-align: left;
		color: red;
	}
	
	.selectbox ul li {
		padding: 10px 0;
		border-bottom: 1px solid #cccccc;
	}
	
	.selectbox ul li p {
		padding: 0 0 0 10px;
	}
	
	.selectbox ul li p:nth-child(2) {
		font-size: 12px;
		color: black;
	}
	
	.selectColor {
		background-color: #ccc;
	}
</style>