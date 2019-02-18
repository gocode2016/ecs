<template>
  <div class="resumeintroduce">
  	
  	<div class="introDiv">  		
	  <p class="intro_text">完善此板块信息，您将获得5积分的奖励！</p>
	  <p class="intro_title">自我介绍</p>	  	
  	</div>
  	<x-textarea  v-model='introText' placeholder="请补充填写您的个人介绍，后厨管理的成功案例或者创新经验" :show-counter="false"></x-textarea>
  	<x-button @click.native="saveClick">保存</x-button>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { XTextarea,XButton,cookie,Toast } from 'vux'
export default {
  components: {
    XTextarea,
    XButton,
    cookie,
    Toast
  },
  data(){
  	return{
  		btnNum:0,
  		isSave:'',//是否保存
  	  introText:'',
  	  userId:'',
  	  userData:{}
  	}
  },
  methods:{
  	updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':5,
				'MemberId':this.userId,
				'IntegralSource':'简历完善',
				'Remark':'自我介绍'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
//			console.log(response);
  		})
  	},
  	saveClick(){
  		if(this.introText==''){
  			this.$vux.toast.text('内容不能为空', 'middle');
  		}else{
  			this.btnNum++;
  			if(this.btnNum==1){
  				this.UpdateIntroduction();//更新自我介绍
  			}
  		}
  	},
  	UpdateIntroduction(){//更新自我介绍
  		  var _this=this;
  			var params={
  		    "Introduction":this.introText,
  		    "MemberId":this.userId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.UpdateIntroduction,
	  			data:params
	  		}).then(function(response){
			  		if(_this.isSave==0 && _this.btnNum==1){//没有保存过  进行保存
	     	  		_this.$vux.toast.text('保存成功,您已增加5积分','middle');
	     	  		_this.updateMemberIntegral();//更改积分
	     	  	}else if(_this.isSave==1){
	     	  		_this.$vux.toast.text('保存成功','middle');
	     	  	}
			      setTimeout(function(){
			      	_this.$router.push('/component/resume');
			      },500)
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
//				console.log(data);
				if(data.length!=0){
					if(data[0].Introduction==null||data[0].Introduction==""){
						_this.isSave=0;
					}else{
						_this.introText=data[0].Introduction;
					  _this.isSave=1;
					}
				}else{
					_this.isSave=0;
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
	  this.getMemberProfile();//获取信息
  }
}
</script>

<style scoped>
.introDiv{margin-left: 15px;}
.intro_text{color: #E83428;font-size: 12px;margin-top: 15px;}
.intro_title{color: #3E3E3E;font-size: 15px;margin-top: 10px;margin-bottom: 10px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumeintroduce .weui-cell:before{border-top: 0px !important;}
.resumeintroduce .weui-cell{border: 1px solid #858585;}
.resumeintroduce .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;}
.resumeintroduce .weui-btn:after {border: none;}
.resumeintroduce .vux-x-textarea {height: 185px;width: 88%;margin-left: 4%;padding: 8px 6px;}
</style>