<template>
  <div class="trainlist">
  	<!--头部-->
    <div class="header">
    	<div @click='timelist'>上传时间<img :src="img1" class="topImg"><img :src="img2" class="botImg"></div>
    	<div @click="looklist">浏览人数<img :src="img3" class="topImg"><img :src="img4" class="botImg"></div>
    </div>    

    <scroller lock-x scrollbar-y use-pullup height="-108" @on-pullup-loading="load" ref="demo"
    :pullup-config="pullupConfig">
      <div class="box2">
        <div class="videoDiv" v-for="(item,index) in trainlist" :key="index">
					<img v-bind:src="item.Url" class="videoimg" @click="jump(item.CuInterId)" v-show='item.CurriculumType==1'/>
					<iframe @click="jump(item.CuInterId)" frameborder='0' allowfullscreen='true' :src='item.Url' width="100%" height="172px" v-show='item.CurriculumType==2'></iframe>
					<p class="videotext" @click="jump(item.CuInterId)">{{item.CurriculumName}}</p>
					<p class="videonum"><img :src="item.praiseImg" @click="praiseClick(item.IsPraise,item.CuInterId,index)"><span class="likenum">{{item.PraiseCount}}</span><span class="looknum">{{item.VisitCount}}人看过</span><span>{{item.CreateTime}}</span></p>
				</div>
      </div>
    </scroller>
		
  </div>
</template>

<script>
import { cookie ,Scroller} from 'vux'
import apiUrl from '../apiUrls.js'
var updateTime=0;//上传时间 0未选择  1升序  2降序
var looknum=0;//浏览人数 0未选择  1升序  2降序
export default {
  components: {
    cookie,
    Scroller
  },
  data () {
	  return{	  
	  	PageIndex:1,
	  	openId:'',
	  	userData:{},
	  	img1:'',
    	img2:'',
      img3:'',
    	img4:'',
	  	trainlist:[],
	  	count:'',
	  	pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
      }
	  }
  },
  methods:{ 
  	timelist(){
  		this.PageIndex=1;
  		if(updateTime==1){//升序
  			//降序
        this.clickBtn(2,0,2,'time','t');
  		}else if(updateTime==2||updateTime==0){//降序 或 未选择
			//升序
        this.clickBtn(1,0,1,'time','f');
		  }		
  	},
  	looklist(){
  		this.PageIndex=1;
  		if(looknum==1){//升序
  			//降序
        this.clickBtn(0,2,4,'count','t');
  		}else if(looknum==2||looknum==0){//降序 或 未选择
  			//升序
        this.clickBtn(0,1,3,'count','f');
  		}
  	},
  	load () {
      setTimeout(() => {
      	this.PageIndex+=1;
	      if(updateTime==1){
	      	this.getFlowList('time','f');
	      }else if(updateTime==2){
	      	this.getFlowList('time','t');
	      }else if(looknum==1){
	      	this.getFlowList('count','f');
	      }else if(looknum==2){
	      	this.getFlowList('count','t');
	      }
        setTimeout(() => {
          this.$refs.demo.donePullup()
        }, 100)
        if (this.trainlist.length ==this.count) { // unload plugin
          setTimeout(() => {
            this.$refs.demo.disablePullup()
          }, 100)
        }
      }, 500)
	  },
  	getFlowList(OrderBy,IsDesc){//获取培训交流列表
  		var _this=this;
  		var params={
  		   "OrderBy":OrderBy,
  		   "IsDesc":IsDesc,
  		   "OpenId":this.openId,
  		   "PageIndex":this.PageIndex
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getFlowList,
  			data:params
  		}).then(function(response){ 			
  			var data=JSON.parse(response.data);
			  _this.count=data.Count;
  			_this.trainlist=_this.trainlist.concat(data.data);
  			for(var i=0;i<_this.trainlist.length;i++){
  				var trainlist=_this.trainlist[i];
          trainlist.CreateTime=trainlist.CreateTime.replace(/-/g,'/');
  				if(trainlist.IsPraise=='f'){
  					trainlist.praiseImg='../../static/credit/zanw.png';
  				}else if(trainlist.IsPraise=='t'){
  					trainlist.praiseImg='../../static/credit/zanr.png';
  				}
  			}
  		})
  	},
  	praiseClick(IsPraise,CuInterId,index){//点赞
  		var _this=this;
  		if(IsPraise=='f'){//未点赞
  			var params={
  		     "CuInterId":CuInterId,
  		     "OpenId":this.openId
	  		}
	  		this.$http({//点赞
	  			method:'post',
	  			url:apiUrl.addCultivatePraise,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			_this.trainlist[index].PraiseCount=data.PraiseCount;
	  			_this.trainlist[index].IsPraise='t';
	  			_this.trainlist[index].praiseImg='../../static/credit/zanr.png';
	  		})
  		}
  	},
  	changeImg(){
  		this.img1='../../static/credit/topc.png';
    	this.img2='../../static/credit/botc.png';
      this.img3='../../static/credit/topc.png';
    	this.img4='../../static/credit/botc.png';
  	},
  	clickBtn(num1,num2,num,type,ft){
  		updateTime=num1;//上传时间状态
  		looknum=num2;//浏览人数状态
  		this.trainlist=[];
  		this.changeImg();
  		if(num==1){//图片
  			this.img1='../../static/credit/topr.png';
  		}else if(num==2){
  			this.img2='../../static/credit/botr.png';
  		}else if(num==3){
  			this.img3='../../static/credit/topr.png';
  		}else if(num==4){
  			this.img4='../../static/credit/botr.png';
  		}    	
    	this.getFlowList(type,ft);//获取菜品列表   type上传时间或浏览人数  ft升序或降序	
  	},
  	jump(CuInterId){
  		this.$router.push({path:"/component/traindetail",query:{CuInterId:CuInterId}})
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
    this.clickBtn(1,0,1,'time','f');
  }
}
</script>
<style scoped>
	.header img{width: 9px;}
	.topImg{position: relative;top: -6px;left: 5px;}
	.botImg{ position: relative;left: -4px;top: 1px;}
	.header{width: 100%;height: 62px;background: #FFFFFF;display: flex;display: flex;padding-left: 15%;padding-right: 15%;}
	.header div{width: 35%;height: 62px;line-height: 62px;text-align: center;font-size: 14px;color: #5d5d5d;}
	.videoDiv{height: 250px;width: 100%;}
	.videoimg{width: 100%;height: 172px;}
	.videotext{font-size: 15px;padding-left: 8px;letter-spacing: 2px;}
	.videonum{padding-left: 8px;font-size: 11px;color: #afafaf;}
	.videonum *{display: inline-block;vertical-align: middle;}
	.videonum img{width: 18px;}
	.likenum{margin-left: 12px;margin-right: 40px;}
	.looknum{margin-right: 40px;}
</style>
<style>
	#vux_view_box_body{background: #FFFFFF;}
</style>