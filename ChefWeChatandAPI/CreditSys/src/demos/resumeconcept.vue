<template>
  <div class="resumeconcept">
  	
  	<div class="ideaDiv">  		
	  	<p class="idea_text">完善此板块信息，您将获得5积分的奖励！</p>		  
		  <popup-picker title="从事行业原因" :data="reasonlist" v-model="reason" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="食材选择要求" :data="chooselist" v-model="choose" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>
		  <popup-picker title="是否乐于分享" :data="sharelist" v-model="share" cancel-text='取消' confirm-text='确定' value-text-align="right"></popup-picker>		  		  
  	</div>
  	<x-button @click.native="saveClick">保存</x-button>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { PopupPicker,XButton,cookie,Toast } from 'vux'
export default {
  components: {
    PopupPicker,
    XButton,
    cookie,
    Toast
  },
  data(){
  	return{
  		btnNum:0,
  		isSave:'',//是否保存过
  		reasonlist:[['热爱烹饪','生计需求','不清楚职业发展','人生事业']],
  		reason:[],
  		chooselist:[['新鲜食材','新奇食材','有机食材','普通食材','低价食材','无公害食材']],
  		choose:[],
  		sharelist:[['是','否']],
  		share:[],
      userId:''	,
      userData:{}
  	}
  },
  methods:{
  	saveClick(){//点击保存
  		if(this.reason==''||this.choose==''||this.share==''){
  			this.$vux.toast.text('内容不能为空', 'middle');
  		}else{
  			this.btnNum++;
  			if(this.btnNum==1){
  				this.updateCookIdea();//更新从厨理念
  			}
  		}
  	},
  	updateCookIdea(){//更新从厨理念
  		  var _this=this;
  			var share='';
  			if(this.share[0]=='否'){
  				share=0;
  			}else if(this.share[0]=='是'){
  				share=1;
  			}
  			var params={
				  'FoodDemand':this.choose[0],
		      'JobReason':this.reason[0],
		      'IsShare':share,
		      'MemberId':this.userId
		    }
	  	  this.$http({
		  		method:'POST',
		  		url:apiUrl.updateCookIdea,
		  		data:params
	  	  }).then(function(response){
          if(response.data=="Exc Success"){
				    if(_this.isSave==0 && _this.btnNum==1){//没有保存过  进行保存
	     	  		_this.$vux.toast.text('保存成功,您已增加5积分','middle');
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
					info.JobReason==null||info.JobReason=="" ? _this.reason=[] : _this.reason=[info.JobReason];//从事行业原因
					info.FoodDemand==null||info.FoodDemand=="" ? _this.choose=[] : _this.choose=[info.FoodDemand];//食材选择要求
					if(info.IsShare==0){//是否乐于分享
						_this.share=['否'];
					}else if(info.IsShare==1){
						_this.share=['是'];
					}else{
						_this.share=[];
					}
					info.JobReason==null||info.JobReason=="" ? _this.isSave=0 : _this.isSave=1;
				}else{
					_this.isSave=0;//没有保存过
				}
			})
   },
   updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':5,
				'MemberId':this.userId,
				'IntegralSource':'简历完善',
				'Remark':'从厨理念'
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
	  this.getMemberProfile();//获取基本信息
  }
}
</script>

<style scoped>
.ideaDiv{margin-left: 15px;overflow-x: hidden;}
.idea_text{color: #E83428;font-size: 12px;margin-top: 15px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumeconcept .vux-cell-box:before{border-top: 0px !important;}
.resumeconcept .weui-cell{padding: 10px 18px 10px 0 !important;border-bottom: 1px solid #c8c7cc;}
.resumeconcept .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
.resumeconcept .weui-label {font-size: 14px;color: #3e3e3e;}
.resumeconcept .weui-btn {width: 92% !important;margin-top: 40px;background: #E83428;color: white;}
.resumeconcept .weui-btn:after {border: none;}
.vux-popup-picker-header-menu-right { color: #e83428 !important; }
.resumeconcept .vux-popup-picker-value{font-size: 14px;color: #3e3e3e;}
</style>