<template>
  <div class="resume">
  	<!--头部-->
  	<div class="resume_top">
	  <div class="personphoto"><div class="myphoto"><img :src="userImg"/></div></div>   	 
	  <p class="resume_btn"><button @click="resumePreview">简历预览</button></p>
	  <p class="resume_text">简历完善度：{{reslevel}}%<img src="../../static/credit/help.png" @click="question"></p>
    </div>
    <!--信息完善-->
    <div class="resume_box">
		  <cell title="基本信息" link="/component/resumeinformation"  :value="inforPerfect" is-link></cell>
		  <cell title="酒店信息"  link="/component/resumehotel" :value="hotelPerfect" is-link></cell>
		  <cell title="从厨理念"  link="/component/resumeconcept" :value="conceptPerfect" is-link></cell>
		  <cell title="职业技能"  link="/component/resumeskill" :value="skillPerfect" is-link></cell>
		  <cell title="荣誉资格"  link="/component/resumehonor" :value="honorPerfect" is-link></cell>
		  <cell title="从业经历"  :value="exPerfect" is-link  @click.native="jump"></cell>
		  <cell title="自我介绍"  link="/component/resumeintroduce" :value="introPerfect" is-link></cell>  
    </div>
    <!--点击问号信息提示-->
    <div class="alert-mask" v-show='alertFlag'>
      <div class="menu">
         <p>简历填写的选项及字段越全，完成度就会越高，从而获得的积分和机会就会越多。</p>
         <button @click="menuBtn">知道了</button>
      </div>
    </div>
    
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { Cell,cookie } from 'vux'
var save='';
export default {
  components: {
    Cell,
    cookie
  },
  data(){
  	return{
  		reslevel:0,//简历完善度
  		userData:{},
  		userId:'',
  		userImg:'',
  		alertFlag:false,
  		Headimgurl:'',//微信头像
  		inforPerfect:'15分',
		  hotelPerfect:'20分',
  		conceptPerfect:'5分',
  		skillPerfect:'15分',
  		honorPerfect:'10分',
  		exPerfect:'15分',
  		introPerfect:'5分',
  	}
  },
  methods:{
  	resumePreview(){
  		this.$router.push({path:'/component/resumepreview'})
  	},
  	question(){
  		this.alertFlag=true;
  	},
  	menuBtn(){
  		this.alertFlag=false;
  	},
  	jump(){//从业经历
  		if(save==1){//从业经历列表
  			this.$router.push({path:'/component/resumeexperiences'})
  		}else if(save==0){//新增从业经历
  			this.$router.push({path:'/component/resumeexperience',query:{resumeId:0}})
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
//				console.log(data);
				if(data.length!=0){
					if(data[0].ImgUrl!=null&&data[0].ImgUrl!=""){
	        	_this.userImg=data[0].ImgUrl;
	        }else{
	        	_this.userImg=_this.Headimgurl;
	        }
					if(data[0].Nature!=null&&data[0].Nature!=""){
					  //基本信息完整
					  _this.inforPerfect='已完善';
					  $('.resume_box .weui-cell__ft').eq(0).css('color','#ccc');
					  _this.reslevel+=20;
					}
					if(data[0].PerConsumption!=null&&data[0].PerConsumption!=""){
						//酒店信息完整
						_this.hotelPerfect='已完善';
					  $('.resume_box .weui-cell__ft').eq(1).css('color','#ccc');
					  _this.reslevel+=20;
					}
					if(data[0].JobReason!=null&&data[0].JobReason!=""){
						//从厨理念完整
						_this.conceptPerfect='已完善';
						 $('.resume_box .weui-cell__ft').eq(2).css('color','#ccc');
						 _this.reslevel+=5;
					}
					if(data[0].Major!=null&&data[0].Major!=""){
						//职业技能完整
						_this.skillPerfect='已完善';
						 $('.resume_box .weui-cell__ft').eq(3).css('color','#ccc');
						 _this.reslevel+=20;
					}
					if(data[0].MatchName!=null&&data[0].MatchName!=""){
						//荣誉资格完整
						_this.honorPerfect='已完善';
						 $('.resume_box .weui-cell__ft').eq(4).css('color','#ccc');
						 _this.reslevel+=15;
					}
					if(data[0].Introduction!=null&&data[0].Introduction!=""){
						//自我介绍完整
						_this.introPerfect='已完善';
						$('.resume_box .weui-cell__ft').eq(6).css('color','#ccc');
						_this.reslevel+=5;
					}		
				}else{
	        	_this.userImg=_this.Headimgurl;
				}	        
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
				var data=JSON.parse(response.data);
				if(data.length!=0){
					//从业经历完整
					_this.exPerfect='已完善';
					$('.resume_box .weui-cell__ft').eq(5).css('color','#ccc');
					_this.reslevel+=15;
					save=1;
				}else{
					//从业经历待完善
					save=0;
				}
			})
    }
  },
  mounted(){
  	$('#vux_view_box_body').css('background','#f7f7f7');
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
	  
	  this.Headimgurl=this.userData.Headimgurl;
	  this.getMemberProfile();//获取简历信息
	  this.getMemberResumeList();//获取从业经历列表
  }
}
</script>

<style scoped>
.resume_top{width: 100%;height: 200px;background: url('../../static/credit/perback.png') no-repeat;background-size: 100% 100%;}
.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;}
.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 3px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
.myphoto img{width:100%;height: 100%;border-radius: 50%;}
.resume_btn{text-align:center;}
.resume_btn button{width: 88px;height: 27px;border: 1px solid #E83428;border-radius: 5px;margin-top: 15px;background: #fff;color: #ff2524;}
.resume_text{font-size:12px;color: #9c9c9c;padding-left: 12px;margin-top: 22px;}
.resume_text img{width: 16px;margin-left: 10px;position: relative;top: 3px;}
.resume_box{padding-left:10px;padding-right:10px;}
.alert-mask{ width: 100%; height: 100%; background: rgba(0,0,0,0.5); position: fixed; top: 0px; }
.menu{width:70%;background: #FFFFFF; border-radius: 10px; margin: 200px auto;overflow: hidden;}
.menu p{font-size: 13px; color: #3E3E3E; width: 80%; margin-left: 10%;padding: 20px 0px;}
.menu button{width: 100%;height:33px;background: #fff;color: #E83428;outline: none;border: none;border-top: 1px solid #e3e3e3;}
</style>
<style>
#vux_view_box_body{background:#f7f7f7;}
.resume .weui-cell{background: #fff ;margin-top: 7px;font-size: 13px;color: #3E3E3E;height: 25px;padding-left: 3px;padding-right: 8px;border-radius: 5px;}
.resume .weui-cell:before{border-top: none;}
.resume .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;margin-left: 16px;}
.resume .weui-cell__ft{font-size: 12px;color: #E83428;}
</style>