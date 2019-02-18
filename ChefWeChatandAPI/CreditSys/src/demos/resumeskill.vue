<template>
  <div class="resumeskill">
  	
  	<div class="skillDiv">  		
  	  <p class="skill_text">完善此板块信息，您将获得15积分的奖励！</p>
		  <popup-picker title="学历" :data="educationlist" v-model="education" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <x-input title="专业"  placeholder="请输入" v-model="major"></x-input>
		  <x-input title="毕业院校"  placeholder="请输入" v-model="school"></x-input>
		  <x-input title="师承"  placeholder="请输入" v-model="teacher"></x-input>
		  <cell  title="擅长菜系"  @click.native="select(1)" is-link  v-model="dish"></cell>
		  <cell  title="相关工作爱好"  @click.native="select(2)" is-link  v-model="workhobby"></cell>
		  <x-input title="个人代表菜品"  placeholder="请输入" v-model="delegatadish"></x-input>
		  <popup-picker title="菜品成功原因" :data="successlist" v-model="successreason" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="未来职业发展" :data="developmentlist" v-model="development" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <cell  title="希望考察城市"  @click.native="select(3)" is-link  v-model="wishcity"></cell>
		  <popup-picker title="酒店管理决策权" :data="powerlist" v-model="power" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="调味品采购权" :data="shopPowerlist" v-model="shopPower" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="自由支配时间" :data="timelist" v-model="ctrltime" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <datetime     title="入行时间" v-model="timeText" :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align ='right'></datetime>
		  <popup-picker title="多城市从业经验" :data="morecitylist" v-model="morecity" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  
		  <div v-transfer-dom>
	      <popup v-model="show1">
	        <popup-header
	        left-text="取消"
	        right-text="确定"
	        :show-bottom-border="false"
	        @on-click-left="show1 = false"
	        @on-click-right="show1 = false" ></popup-header>
	        <group gutter="0">
	          <checklist  label-position="left" :options="dishlist"  @on-change="change1" ></checklist>
	        </group>
	      </popup>
      </div>
		  <div v-transfer-dom>
	      <popup v-model="show2">
	        <popup-header
	        left-text="取消"
	        right-text="确定"
	        :show-bottom-border="false"
	        @on-click-left="show2 = false"
	        @on-click-right="show2 = false" ></popup-header>
	        <group gutter="0">
	          <checklist  label-position="left" :options="workhobbylist"  @on-change="change2" ></checklist>
	        </group>
	      </popup>
      </div>
      <div v-transfer-dom>
	      <popup v-model="show3">
	        <popup-header
	        left-text="取消"
	        right-text="确定"
	        :show-bottom-border="false"
	        @on-click-left="show3 = false"
	        @on-click-right="show3 = false" ></popup-header>
	        <group gutter="0">
	          <checklist  label-position="left" :options="citylist"  @on-change="change3" ></checklist>
	        </group>
	      </popup>
      </div>
    
  	</div>
  	<x-button @click.native='saveClick'>保存</x-button>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { Cell,XInput,PopupPicker,Datetime,XButton,PopupHeader, Popup, TransferDom, Group, Checklist,cookie,Toast} from 'vux'
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
  		isSave:'',//是否保存过
  		show1: false,//多选
  		show2: false,
  		show3: false,
      commonList: [ 'China', 'Japan', 'America','1', '2', '3' ],
  		major:'',
  		school:'',
  		teacher:'',
  		delegatadish:'',
  		educationlist:[['初中','高中中专','大专','本科以上']],
  		education:[],
  		dishlist:['鲁菜','东北菜','川菜','湘菜','北京菜','上海菜','苏菜','浙菜','淮阳菜','中式烧烤店','川味火锅','京味火锅','潮汕打边炉','豆捞','自助餐','晋菜','陕西菜','鄂菜','赣菜','黔菜','豫菜','粤菜','徽菜',
  		'茶餐厅','闽菜','海鲜店','农家菜','创意菜','家常菜','台湾菜','客家菜','潮汕菜','清真菜','云南菜','广西菜','冀菜','新疆菜','内蒙菜','西藏菜','青海菜','海南菜','素菜','私房菜','融合菜','面包甜点','茶馆',
  		'咖啡厅','酒吧','小吃快餐','蟹宴','日本料理','韩国料理','朝鲜料理','泰国菜','新加坡菜','印度菜','越南菜','法国菜','意大利菜','西班牙菜','拉美烧烤','牛排','比萨店'],
  		dish:'',
  		workhobbylist:['烹饪技法','食材原料','名厨访问','后厨管理','菜单管理','餐饮趋势','行业新闻','促销消息','展会消息','产品消息','热菜推荐','O2O营销','电商营销','创新菜谱','餐厅运营'],
  		workhobby:'',
  		successlist:[['热卖菜','参加比赛']],
  		successreason:[],
  		developmentlist:[['自主创业','经营管理','菜品钻研']],
  		development:[],
  		citylist:['北京','天津','大连','哈尔滨','沈阳','青岛','烟台','济南','西安','郑州','南京','上海','武汉','长沙','成都','扬州','无锡'],
  		wishcity:'',
  		powerlist:[['直接决策','与老板共同决策','无决策权']],
  		power:[],
  		shopPowerlist:[['决策人','执行人','不参与']],
  		shopPower:[],
  		timelist:[['能','有时能','不能']],
  		ctrltime:[],
  		timeText:'',//入行时间
  		morecitylist:[['是','否']],//多城市从业经验
  		morecity:[],
  		userId:'',
  		userData:{}
  	}
  },
  methods:{
  	select(num){
  		if(num==1){
  			this.show1=true;
  		}else if(num==2){
  			this.show2=true;
  		}else if(num==3){
  			this.show3=true;
  		}  		
  	},
  	change1 (val) {
//    console.log('change', val);
      var dish=val.join();
      this.dish=dish;
    },
    change2 (val) {
//    console.log('change', val);
      var workhobby=val.join();
      this.workhobby=workhobby;
    },
    change3 (val) {
//    console.log('change', val);
      var wishcity=val.join();
      this.wishcity=wishcity;
    },
    saveClick(){//点击保存
    	if(this.education==''||this.major==''||this.school==''||this.teacher==''){
  			this.$vux.toast.text('内容不能为空', 'middle');
  		}else if(this.dish==''||this.workhobby==''||this.delegatadish==''||this.successreason==''){
  			this.$vux.toast.text('内容不能为空', 'middle');
  		}else if(this.development==''||this.wishcity==''||this.power==''||this.shopPower==''||this.ctrltime==''||this.timeText==''||this.morecity==''){
  			this.$vux.toast.text('内容不能为空', 'middle');
  		}else{
  			this.btnNum++;
  			if(this.btnNum==1){
  				this.updateJobSkill();//更新职业技能
  			}
  		}
    },
  	updateJobSkill(){//更新职业技能
  		  var _this=this;
  			var morecity='';
  			if(this.morecity[0]=='否'){
  				morecity=0;
  			}else if(this.morecity[0]=='是'){
  				morecity=1;
  			}
  			var params={
				  'Educations':this.education[0],
					'Major':this.major,
					'Speciality':this.workhobby,
					'School':this.school,
					'Teacher':this.teacher,
					'ExpertInCuisine':this.dish,
					'SuccessReasons':this.successreason[0],
					'Represent':this.delegatadish,
					'Development':this.development[0],
					'WishCity':this.wishcity,
					'ChoicePower':this.power[0],
					'IsCtrlTime':this.ctrltime[0],
	        'Purchaser':this.shopPower[0],
	        'IsManyExp':morecity,
          'JoinDate':this.timeText,
					'MemberId':this.userId
				}
				this.$http({
					method:'POST',
					url:apiUrl.updateJobSkill,
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
					var info=data[0];
//					console.log(info);
					info.Educations==null||info.Educations=="" ? _this.education=[] : _this.education=[info.Educations];//学历
					_this.major=info.Major;//专业
					_this.school=info.School;//毕业院校
					_this.teacher=info.Teacher;//师承
					_this.dish=info.ExpertInCuisine;//擅长菜系
					_this.workhobby=info.Speciality;//相关工作爱好
					_this.delegatadish=info.Represent;//代表菜品
					info.SuccessReasons==null||info.SuccessReasons=="" ? _this.successreason=[] : _this.successreason=[info.SuccessReasons];//菜品成功原因
					info.Development==null||info.Development=="" ? _this.development=[] :_this.development=[info.Development];//未来职业发展
					_this.wishcity=info.WishCity;//希望考察城市
					info.ChoicePower==null||info.ChoicePower=="" ? _this.power=[] : _this.power=[info.ChoicePower];//酒店管理决策权
					info.Purchaser==null||info.Purchaser=="" ? _this.shopPower=[] : _this.shopPower=[info.Purchaser];//调味品采购权
					info.IsCtrlTime==null||info.IsCtrlTime=="" ? _this.ctrltime=[] : _this.ctrltime=[info.IsCtrlTime];//自由支配时间
					if(info.JoinDate!=null&&info.JoinDate!=""){
						_this.timeText=info.JoinDate.substring(0,10);//入行时间
					}
					if(info.IsManyExp==0){//多城市从业经验
						_this.morecity=['否'];
					}else if(info.IsManyExp==1){
						_this.morecity=['是'];
					}else{
						_this.morecity=[];
					}
					if(_this.major==""||_this.major==null){//判断是否保存过
						_this.isSave=0;
					}else{
						_this.isSave=1;
					}
				}else{
					_this.isSave=0;
				}
			})
    },
    updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':15,
				'MemberId':this.userId,
				'IntegralSource':'简历完善',
				'Remark':'职业技能'
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
	  this.getMemberProfile();//获取简历信息
  }
}
</script>

<style scoped>
.skillDiv{margin-left: 15px;overflow-x:hidden ;}
.skill_text{color: #E83428;font-size: 12px;margin-top: 15px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumeskill .weui-cell:before{border-top: 0px !important;}
.resumeskill .vux-cell-box:before{border-top: 0px !important;}
.resumeskill .weui-cell{padding: 10px 18px 10px 0 !important;border-bottom: 1px solid #c8c7cc;}
.resumeskill .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
.resumeskill .vux-label {font-size: 14px;color: #3e3e3e;}
.resumeskill .weui-label{font-size: 14px;color: #3e3e3e;}
.resumeskill .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;margin-bottom: 30px;}
.resumeskill .weui-btn:after {border: none;}
.vux-popup-picker-header-menu-right { color: #e83428 !important; }
.resumeskill .weui-input {text-align: right;font-size: 12px;color: #a3a3a3;}
.resumeskill .weui-icon-clear:before{display:none;}
.v-transfer-dom .weui-cells__title + .weui-cells{height: 14rem;overflow-y:auto;}
.vux-popup-header-right{color: #E83428 !important;}
.weui-cells_checkbox .weui-check:checked + .vux-checklist-icon-checked:before{color: #E83428 !important;}
.resumeskill .vux-datetime {color: #3e3e3e;font-size: 15px;}
.resumeskill .weui-input,.resumeskill .vux-popup-picker-value,.resumeskill .weui-cell__ft{font-size: 14px;color: #3E3E3E;}
.dp-header .dp-item.dp-right {color: #e83428 !important;}
</style>