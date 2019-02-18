<template>
	<div class="resumeexperiences">
		
		<ul class="exUl">
			<li v-for="(item,index) in dataList" :key="index" @click="jump(item.ResumeId)">
				<p>{{item.BeginTime}}—{{item.EndTime}}</p>
				<p class="hotelName">{{item.HotelName}}</p>
				<p>{{item.ProvinceName}}&nbsp;{{item.CityName}}&nbsp;{{item.AreaName}}</p>
				<p>{{item.Job}}</p>
			</li>
		</ul>
		<p class="addjl" @click='addexperience'>+添加从业经历</p>		
		<x-button  @click.native="save" >保存</x-button>	
	</div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { XButton,cookie,Toast } from 'vux'
export default {
  components: {    
    XButton,
    cookie,
    Toast
  },
  data(){
  	return{
  	  userId:'',
  	  userData:{},
  	  dataList:[]//列表信息
  	}
  },
  methods:{
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
			for(let i=0;i<_this.dataList.length;i++){
				var datalist=_this.dataList[i];
				var BeginTime=datalist.BeginTime.substring(0,10)				
				var EndTime=datalist.EndTime.substring(0,10);
				datalist.BeginTime=BeginTime;
				datalist.EndTime=EndTime;
			}
		})
    },
    addexperience(){//添加从业经历
      this.$router.push({path:'/component/resumeexperience',query:{resumeId:0}})
    },
    jump(resumeId){
      this.$router.push({path:'/component/resumeexperience',query:{resumeId:resumeId}})
    },
    save(){
      var _this=this;
      this.$vux.toast.text('保存成功', 'middle');
      setTimeout(function(){
      	_this.saveFlag=false;
      	_this.$router.push('/component/resume');
      },500)
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
  	this.getMemberResumeList();
  }
}
</script>

<style scoped="scoped"> 
.exUl{color: #a3a3a3;font-size: 0.75em;margin-top:20px;margin-left: 20px;}
.exUl li{list-style: none;border-bottom: 1px solid #c8c7cc;margin-bottom: 10px;}
.exUl li p{margin-bottom: 12px;}
.hotelName{color: #3E3E3E;}
.addjl{color: #E83428;font-size: 15px;float: right;margin-right: 12px;margin-top: 10px;}	
.hotelName:after {content: " "; display: inline-block;height: 6px;width: 6px;border-width: 1px 1px 0 0;border-color: #C8C8CD; border-style: solid;
                 -webkit-transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);float: right; margin-right: 18px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.resumeexperiences .weui-btn {width: 92% !important;margin-top: 75px;background: #E83428;color: white;}
.resumeexperiences .weui-btn:after {border: none;}
</style>