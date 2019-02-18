<template>
  <div class="traindetail">
     <img :src="Cultivate.Url" class="headImg" v-if="isShow"/>
     <iframe v-else frameborder='0' allowfullscreen='true' :src='Cultivate.Url' width="100%" height="200px"></iframe>
     <p class="trainName">{{Cultivate.CurriculumName}}</p>
     <p class="trainNum"><img :src="pariseImg" @click="pariseClick"/><span class="likenum">{{Cultivate.PraiseCount}}</span><span class="looknum">{{Cultivate.VisitCount}}人看过</span><span>{{Cultivate.CreateTime}}</span></p>
     <!--培训介绍-->
     <div class="alike show">
     	 <img src="../../static/credit/tutor01.png"/>
     	 <span>培训内容</span>
     </div> 
     <p class="trainIntro ql-editor ql-blank"></p>
     <!--tab切换-->
     <sticky scroll-box="vux_view_box_body" :offset="46" :check-sticky-support="false">
        <tab :line-width="1">
          <tab-item selected @on-item-click='teacher'>讲师介绍</tab-item>
          <tab-item @on-item-click='word'>我要留言</tab-item>
        </tab>
     </sticky>
     <!--讲师介绍-->
     <div v-if="flag" class="tutorFlag">
       <div v-for="(item,index) in teacherList" class="tutorIntro">
       	 <div>
       	 	 <img :src="item.src"/>
       	 </div>
       	 <div class="tutorText">
       	 	 <p>{{item.title}}</p>
       	 	 <p>{{item.smallTitle}}</p>
       	 	 <p>{{item.desc}}</p>
       	 </div>
       </div>
     </div>
     <!--我要留言-->
     <div v-else>
     	 <ul class="wordUl">  	 	
 				<div class="page">
								<div class="content">
									 	<li v-for="(item,index) in speakList" :key="index">
						     	 	 	<div class="leftDiv"><img :src="item.HeadPicUrl"></div>
						     	 	 	<div class="rightDiv">
						     	 	 	 	<p>{{item.NickName}}</p>
						     	 	 	 	<p>{{item.Comment}}</p>
						     	 	 	 	<p>{{item.CreateTime}}</p>
						     	 	 	</div>
						     	</li>														 
								</div>
								<!--底部-->
									<div class="footer">
											 <div class="send">
									     	 	 <div v-if='flag1' class="sendfir">
										     	 	 <input type="text" placeholder="我有话要说..." @focus='focus'/>
									     	 	 </div>
									     	 	 <div v-else class="sendsec" @click.stop>
									     	 	 	 <div class="txtArea">
									     	 	 	 	 <textarea @input='textChange()' v-model='message' maxlength='500'></textarea>
									     	 	 	   <span>{{leng}}</span>
									     	 	 	 </div>     	 	 	
									     	 	 	 <button @click="submit">发送</button>
									     	 	 </div>
									     	 </div>					     	
								 </div>
					</div>
     	 </ul>
     </div>
  </div>
</template>

<script>
import { Tab, TabItem, Sticky ,Panel,cookie,Scroller} from 'vux'
import apiUrl from '../apiUrls.js'

export default {
  components: {
    Tab, 
    TabItem, 
    Sticky,
    Panel,
    cookie,
    Scroller
  },
  data () { 	
    return{
    	isShow:true,
    	openId:'',
    	userData:{},
    	CuInterId:'',//培训id
    	Cultivate:{},//培训信息
    	pariseImg:'',//点赞图标
    	flag:true,
    	flag1:true,
    	type: '6',
    	message:'',//留言内容
    	datetime:'',
    	leng:500,
    	teacherList:[],//讲师介绍列表
    	speakList:[],
    	count:''
    }
  },
  mounted(){
  	this.userData=cookie.get('WeiXinUser',function(val){
  		  var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
  	}) 
	  this.openId=this.userData.Openid;
  	this.CuInterId=parseInt(this.$route.query.CuInterId);
  	this.getCultivateDetail();//获取培训交流详情信息
  	this.getCultivateComment();//获取留言
  	$('.tutorIntro div').eq(0).css({
  		height:$(window).width()*0.27
  	})
  },
  methods:{
  	getCultivateDetail(){//获取培训交流详情信息
  		var _this=this;
  		var params={
  		  "CuInterId":this.CuInterId,
  		  "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getCultivateDetail,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
//			  console.log(data);
  			_this.Cultivate=data.Cultivate;
  			_this.Cultivate.CreateTime=_this.Cultivate.CreateTime.replace(/-/g,'/');
  			if(_this.Cultivate.CurriculumType==1){//图片
          _this.isShow=true;
  			}else if(_this.Cultivate.CurriculumType==2){//视频
  				_this.isShow=false;
  			}
  			$('.trainIntro').html(_this.Cultivate.Introduce);
  			$('.trainIntro img').css('width','100%');
  			var lecturer=data.Lecturer;
  			if(_this.Cultivate.IsPraise=='f'){
  				_this.pariseImg='../../static/credit/zanw.png';
  			}else if(_this.Cultivate.IsPraise=='t'){
  				_this.pariseImg='../../static/credit/zanr.png';
  			}
  			for(var i = 0;i<lecturer.length;i++){
  				var list=lecturer[i];
  				var item={
  					url:'/component/traindetail',
					  src:list.HeadPicUrl ,
		        title:list.LecturerName ,
		        smallTitle:list.Post ,
		        desc:list.HotelName
  				}
  				_this.teacherList.push(item);
  			}
  		})
  	},
  	getCultivateComment(){//获取留言
  		var _this=this;
  		var params={
  		  "CuInterId":this.CuInterId,
  		  "PageIndex":1
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getCultivateComment,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.speakList=data.data;
			  _this.count=data.Count;
//			console.log(_this.count);
  		})
  	},
  	pariseClick(){
  		var _this=this;
      if(this.Cultivate.IsPraise=='f'){//未点赞  	
        var params={
  		     "CuInterId":this.CuInterId,
  		     "OpenId":this.openId
	  		}
	  		this.$http({//点赞
	  			method:'post',
	  			url:apiUrl.addCultivatePraise,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			_this.Cultivate.PraiseCount=data.PraiseCount;
	  			_this.Cultivate.IsPraise='t';
      	  _this.pariseImg='../../static/credit/zanr.png';	  		
	  		})
      }
  	},
  	teacher(){//讲师介绍
  		this.flag=true;
  		this.flag1=true;
  	},
  	word(){//我要留言
  		this.flag=false;
  	},
  	focus(){
		   this.flag1=false;  
		   var _this=this;
		   setTimeout(function(){
		   	 $('.sendsec textarea').focus();
		   },100);
		   $('.sendto').height(30);
		   $(document).click(function(ev){
		   	 _this.flag1=true;  
		   	 $('.sendto').height(0);
		   })
  	},
  	textChange(){ 	
  		if(this.message.length>=1){//改变按钮颜色
  			$('.sendsec button').css("background","#e83426")
  		}else{
  			$('.sendsec button').css("background","#7f7f7f")
  		}
  		this.leng=500-this.message.length;
  	},
  	submit(){//发送按钮
  		var _this=this;
  		if(this.message!=''){
  			var params={
  		    "CuInterId":this.CuInterId,
  		    "OpenId":this.openId,
  		    "HeadPicUrl":this.userData.Headimgurl ,
  		    "NickName":this.userData.Nickname,
  		    "Comment":this.message
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.addCultivateComment,
	  			data:params
	  		}).then(function(response){
	  			if(response.data=='Ok'){
//	  				_this.speakList
	  				_this.getCultivateComment();
	  			}
	  			_this.message='';
		  		_this.leng=500;
		  		_this.flag1=true;
		  		$('.sendto').height(0);
	  		})  		
  		}  		
  	}
  }
}
</script>

<style scoped>
	@import '../../static/css/quill.bubble.css';
	@import '../../static/css/quill.core.css';
	@import '../../static/css/quill.snow.css';
	.headImg{width: 100%;}
	.trainName{font-size: 17px;color: #3e3e3e;letter-spacing: 2px;text-align: center;margin-top: 20px;}
	.trainNum{font-size: 11px;color: #afafaf;text-align: center;margin-top: 10px;}
	.trainNum img{width: 18px;position: relative;top: 2px;margin-left: 10px;}
	.likenum{margin-left: 13px;margin-right: 28px;}
	.looknum{margin-right: 28px;}
	.alike *{display: inline-block;vertical-align: middle;}
	.alike img{width: 20px;}
	.alike span{color: #e83426;font-size: 15px;font-weight: bold;}
  .show{margin-top: 20px;padding-left: 8px;}
	.wordUl{width: 100%;list-style: none;height: 262px;}
	.wordUl li{display: flex; padding-top: 30px;padding-bottom: 20px;}
	.leftDiv{width: 18%;}
	.rightDiv{width: 82%;font-size: 12px;color: #282828;padding-right: 10px;}
	.rightDiv p:nth-child(1){font-weight: bold;}
	.rightDiv p:nth-child(2){word-wrap: break-word;}
	.rightDiv p:nth-child(3){text-align: right;color: #bebebe;margin-top: 15px;}
	.leftDiv img{width: 33px;margin-left: 20px;}
	.sendto{height: 0px;}
	.send{width: 100%;background: #FFFFFF;}
	.sendfir{width: 100%;height: 50px;	border-top: 1px solid #efefef;}
	.sendfir input{width:92%;height: 35px;border: none;background: #f6f6f6;margin-left: 4%;border-radius: 5px;text-indent: 1em;margin-top: 8px;outline: none;}
	.sendfir img{width: 20px;margin-left: 5%;position: relative;top: 8px;}
	.sendsec{width: 90%;margin-left: 5%;text-align: right;}
	.txtArea{	height:45px;background:#f6f6f6;position: relative;}
	.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
	.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px;}
	.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;margin-top: 10px;margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
	.tutorFlag{height: 262px;}
	.tutorIntro {width: 100%;height: 100px;display: flex;flex-wrap: wrap;padding: 15px 0;border-bottom: 1px solid #f4f4f4;}
	.tutorIntro img{width: 100%;}
	.tutorIntro div:nth-child(1){width: 27%;margin-left: 10%;border-radius: 50%;overflow: hidden;border: 1px solid #e83428;box-sizing: border-box;}
	.tutorIntro div:nth-child(2){width: 53%;margin-left: 10%;font-size: 13px;color: #3E3E3E;padding-top: 5%;}
	.tutorText p:nth-child(1){font-size: 15px;}
	.tutorText p:nth-child(2){color: #E83428;}
	
			.page {
			display: -webkit-box;
	    display: -webkit-flex;
	    display: -ms-flexbox;
	    display: flex;
			-webkit-box-orient: vertical;
	    -webkit-flex-direction: column;
	    -ms-flex-direction: column;
	    flex-direction: column;/* 该框从顶部列出其内容。 */
			height: 100%;
		}
/*滚动条不显示*/
		.content::-webkit-scrollbar {
			display:none
		}

		/*flex内容区*/
		.page .content {
			position: relative;
			-webkit-box-flex: 1;
	    -webkit-flex: 1;
	    -ms-flex: 1;
	    flex: 1;
			overflow: auto;
			-webkit-overflow-scrolling: touch;
		}
</style>
<style>
	#vux_view_box_body{background: #FFFFFF;}
	.traindetail .vux-tab-ink-bar{background-color: #e81d23 !important;position: absolute;top: 44px;height: 2px !important;}
  .traindetail .vux-tab-selected{color: #e81d23 !important;}
  .traindetail .vux-sticky-box{border-bottom: 2px solid #f8f8f8 !important;background: #FFFFFF;margin-top: 10px;}
  .traindetail .vux-tab{width: 200px;margin-left: 23.3%;text-align: center;}
  .traindetail .vux-tab span{display: inline-block;width:50%;text-align: center;}
  .traindetail .vux-tab-item{font-size: 15px !important;font-weight: bold;background: none !important;}
  .traindetail .vux-fixed{position: static !important;}
</style>