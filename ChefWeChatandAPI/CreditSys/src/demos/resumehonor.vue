<template>
  <div class="resumehonor">
  	
  	<div class="honorDiv">  		
	  <p class="honor_text">完善此板块信息，您将获得10积分的奖励！</p> 
	  <x-input title="烹饪比赛名称" placeholder="请输入" v-model="competitionName"></x-input>
	  <popup-picker title="赛事级别" :data="gamelist" v-model="game" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
	  <popup-picker title="比赛名次" :data="rankinglist" v-model="ranking" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
	  <popup-picker title="职业资格" :data="competencelist" v-model="competence" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
	  <popup-picker title="营养师等级" :data="gradelist" v-model="grade" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
	  <popup-picker title="电视媒体曝光" :data="frequencylist" v-model="frequency" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
	  <popup-picker title="荣誉职称" :data="honorlist" v-model="honor" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
  	</div>
  	<x-button @click.native='saveClick'>保存</x-button>
  	
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import offer from '../offer.js'
import { XInput,PopupPicker,XButton,cookie,Toast } from 'vux'
export default {
  components: {   
    XInput,
    PopupPicker,   
    XButton,
    cookie,
    Toast
  },
  data(){
  	return{
  		btnNum:0,
  		isSave:'',//是否保存过
  		competitionName:'',
  		gamelist:[['世界级','全国级','省级','市级','区级','无']],
  		game:[],
  		rankinglist:[['特等奖','一等奖','二等奖','三等奖','无']],
  		ranking:[],
  		competencelist:[['初级','中级','高级','技师','高级技师','无']],
  		competence:[],
  		gradelist:[['一级','二级','三级','四级','五级','无']],
  		grade:[],
  		frequencylist:[['无','1~5次','5~10次','10次以上']],
  		frequency:[],
  		honorlist:[['首席技师','劳模','菜系烹饪名师','菜系烹饪大师','中国烹饪名师','中国烹饪大师','无']],
  		honor:[],
  		userId:10000
  	}
  },
  methods:{
  	saveClick(){//保存
  		if(this.competitionName==''||this.game==''||this.ranking==''||this.competence==''||this.grade==''||this.frequency==''||this.honor==''){
  			this.$vux.toast.text('内容不能为空', 'middle');
  		}else{
  			this.btnNum++;
  			if(this.btnNum==1){
  				this.updateHonor();//更新荣誉资格
  			}
  		}
  	},
  	updateHonor(){//更新荣誉资格
  		  var _this=this;
  			var params={
				  "MatchLv":this.game[0],//赛事级别
				  "MatchNum":this.ranking[0],//比赛名次
					"Qualifications":this.competence[0],//职业资格
					"DietitianLv" :this.grade[0],//营养师等级
					"ExposureCount" :this.frequency[0],//媒体曝光次数
					"Honor":this.honor[0],//个人荣誉
					"MatchName":this.competitionName,//比赛名称
					"MemberId":this.userId
				}
				this.$http({
					method:'post',
					url:apiUrl.updateHonor,
					data:params
				}).then(function(response){
          if(response.data=="Exc Success"){
					  if(_this.isSave==0 && _this.btnNum==1){//没有保存过  进行保存
	     	  		_this.$vux.toast.text('保存成功,您已增加10积分','middle');
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
					console.log(info);
					_this.competitionName=info.MatchName;//比赛名称
					info.MatchLv==null||info.MatchLv=="" ? _this.game=[] : _this.game=[info.MatchLv];//赛事级别
					info.MatchNum==null||info.MatchNum=="" ? _this.ranking=[] : _this.ranking=[info.MatchNum];//比赛名次
					info.Qualifications==null||info.Qualifications=="" ? _this.competence=[] : _this.competence=[info.Qualifications];//职业资格
					info.DietitianLv==null||info.DietitianLv=="" ? _this.grade=[] : _this.grade=[info.DietitianLv];//营养师等级
					info.ExposureCount==null||info.ExposureCount=="" ? _this.frequency=[] : _this.frequency=[info.ExposureCount];//电视媒体曝光
					info.Honor==null||info.Honor=="" ? _this.honor=[] : _this.honor=[info.Honor];//荣誉职称
					_this.competitionName==""||_this.competitionName==null ? _this.isSave=0 : _this.isSave=1;
				}else{
					_this.isSave=0;
				}
			})
   },
   updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':10,
				'MemberId':this.userId,
				'IntegralSource':'简历完善',
				'Remark':'荣誉资格'
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
.honorDiv{margin-left: 15px;overflow-x:hidden;}
.honor_text{color: #E83428;font-size: 12px;margin-top: 15px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumehonor .weui-cell:before{border-top: 0px !important;}
.resumehonor .vux-cell-box:before{border-top: 0px !important;}
.resumehonor .weui-cell{padding: 10px 18px 10px 0 !important;border-bottom: 1px solid #c8c7cc;}
.resumehonor .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
.resumehonor .vux-label {font-size: 14px;color: #3e3e3e;}
.resumehonor .weui-label {font-size: 14px;color: #3e3e3e;}
.resumehonor .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;}
.resumehonor .weui-btn:after {border: none;}
.vux-popup-picker-header-menu-right { color: #e83428 !important; }
.resumehonor .weui-input {text-align: right;font-size: 12px;color: #a3a3a3;}
.resumehonor .weui-icon-clear:before{display:none;}
.resumehonor .weui-input,.resumehonor .vux-popup-picker-value{font-size: 14px;color: #3E3E3E;}
</style>