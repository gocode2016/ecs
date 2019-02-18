<template>
  <div class="competitiondish">
     <!--头部-->
     <img v-bind:src='dishInfo.DishPicUrl' class="comdish">
     <p class="look">{{dishInfo.VisitCount}}人看过</p>
     <div class="dishname">{{dishInfo.DishName}}</div>
     <div class="voteDiv">
     	 <button class='votebtn' @click='voteBtn' v-show="showVote">我要投票</button>
     	 <span class="vote" v-show="showVote">当前票数:{{dishInfo.SelectedCount}}</span>
     	 <span class="good">
	     	 <img v-bind:src='prasieImg' @click="parsieClick"/>
	     	 <span>{{dishInfo.PrasieCount}}</span>
       </span>
     </div>
     <!--选手介绍、-->
     <div class="show">
		    <div class="alike">
		     	 <img src="../../static/credit/tutor01.png" />
		     	 <span>选手介绍</span>
		    </div>		  
		    <div class="player">
		    	<div><img v-bind:src="chefInfo.HeadPicUrl"></div>
		    	<div>
		    		<p class="title">{{chefInfo.MemberName}}</p>
		    		<p class="smalltitle">{{chefInfo.HotelName}}&nbsp;&nbsp;{{chefInfo.AreaName}}</p>
		    		<p class="smalltitle smalltitle1">本地菜&nbsp;&nbsp;<span class="localdish" @click="localdish(chefInfoLink.DishId)">{{chefInfoLink.DishName}}<img src="../../static/credit/jt.png"></span></p>
		    		<p class="smalltitle">创新菜&nbsp;&nbsp;{{chefInfoNoLink.DishName}}</p>
		    	</div>
		    </div>
     </div>
     <!--菜品故事-->
     <div class="show">
	     <div class="alike">
	     	 <img src="../../static/credit/book1.png" />
	     	 <span>菜品故事</span>
	     </div>
	     <p class="storytext">{{dishInfo.DishStory}}</p>
     </div>
     <!--备料展示-->    
	     <div class="alike show show1">
	     	 <img src="../../static/credit/show.png" />
	     	 <span>备料展示</span>
	     </div>  
	     <group v-for="(item,index) in dishlist" :key="index" gutter=0>
		     <cell :title="item.type" class='cellTitle'></cell>
		     <cell-form-preview :list="item.list"></cell-form-preview>
	     </group>
     <!--烹饪步骤-->
     <div class="show">
	     <div class="alike">
	     	 <img src="../../static/credit/cook.png" />
	     	 <span>烹饪步骤</span>
	     </div> 
	     <p class="cooktext" v-for="(item,index) in dishStep" :key="index">{{item.StepId}}、{{item.StepName}}</p>
     </div>
      <!--选手推荐-->
     <div class="alike show">
     	 <img src="../../static/credit/recom.png" />
     	 <span>选手推荐</span>
     </div> 
     
     <div class="scroller-box">
		    <div class="modify-scroller">
		      <div class="playerSelect">
		        <div class="playerSelect-item" v-for="(item,index) in dishSelect" :key="index">
		        	 <div class="box"><img v-bind:src="item.FlavourPicUrl"></div>
	       	     <p>{{item.FlavourName}}</p>
		        </div>
		      </div>
	  	   </div>
		    <div class="mask-scroller"></div>
      </div>
     
     <!--我要留言-->
     <div class="alike show">
     	 <img src="../../static/credit/tutor03.png" />
     	 <span>我要留言</span>
     </div> 
     <ul class="wordUl">
		    	 <scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2"  @on-pullup-loading="load" class='scrollheight'>
		    	 	 <div class="box2">
			     	 	 <li v-for="(item,index) in speakdata" :key="index">
			     	 	 	 <div class="leftDiv"><img v-bind:src="item.HeadPicUrl"></div>
			     	 	 	 <div class="rightDiv">
			     	 	 	 	 <p>{{item.MemebName}}</p>
			     	 	 	 	 <p>{{item.Comment}}</p>
			     	 	 	 	 <p>{{item.CreateTime}}</p>
			     	 	 	 </div>
			     	 	 </li>
		     	 	</div>
	     	 	 </scroller>
     	  </ul>
     	  <div class="sendto"></div>
	     	<!--底部-->
	     	<div class="send">
	     	 	 <div v-if='flag1' class="sendfir">
		     	 	 <input type="text" placeholder="我有话要说..." @focus='focus'/>
		     	 	 <img v-bind:src="collectImg" @click="collectClick"/>
	     	 	 </div>
	     	 	 <div v-else class="sendsec" @click.stop>
	     	 	 	 <div class="txtArea">
	     	 	 	 	 <textarea @input='textChange()' v-model='message' maxlength='500'></textarea>
	     	 	 	   <span>{{leng}}</span>
	     	 	 	 </div>     	 	 	
	     	 	 	 <button @click="submit">发送</button>
	     	 	 </div>
	     	</div>
        
        <!--收藏成功图片-->
     	  <img src="../../static/credit/collect.png" class="colSuc" v-show="colShow"/> 
  </div>
</template>

<script>
import { Panel,CellFormPreview, Cell,Scroller,Group,cookie} from 'vux'
import apiUrl from '../apiUrls.js'
var flag=0;
var pageIndex=1;//传参页数
var pageSize=3;//传参个数
export default {
  components: {
    Panel,
    CellFormPreview, 
    Cell,
    Scroller,
    Group,
    cookie
  },
  data() {
    return{
    	userData:{},
    	showVote:false,
    	colShow:false,//收藏成功显示
    	speakdata:"",//留言数据
    	flag1:true,//底栏显示
    	message:'',//留言内容
    	datetime:'',//留言时间
    	leng:500,//内容长度
    	count:'',//留言个数
    	dishId:'',//参数 菜品id
    	dishType:'',//参数 菜品类型
    	chefId:'',//参数 厨师id
    	openId:'',//参数 用户id
    	prasieImg:'',//点赞图标
    	prasieFlag:'',//点赞状态
    	collectImg:'',//收藏图标
    	collectFlag:'',//收藏状态
    	selectFlag:'',//投票状态
    	activityEnd:'',//投票是否结束 
    	dishInfo:{},//菜品信息
			dishMateria:[],//备料
			dishStep:[],//步骤
			dishSelect:[],//推荐
			chefInfo:{},//选手介绍
			chefInfoLink:{},
			chefInfoNoLink:{},
			list1:[],
			list2:[],
			list3:[],
			dishlist:[],//备料展示
    	pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
     },
    }
  },
  mounted(){	
	  this.dishId=this.$route.query.dishId;
	  this.dishType=this.$route.query.dishType;
	  this.chefId=this.$route.query.chefId;	  
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
	  
	  if(this.$route.query.isVoteDish==1){//投票菜品
	  	this.showVote=true;//显示投票按钮
	  	this.getVoteDishDetail();//投票菜品详情接口
	  }else{
	  	this.getDishDetail();//选手菜品详情接口
	  }
	  this.wordRequest();//留言接口 
    this.getPlayer();//选手介绍接口
  },
  methods:{
  	getDishDetail(){//选手菜品详情
  		var _this=this;
  		var params={
  			"DishId":this.dishId ,
  			"OpenId":this.openId, 
  			"DishType":this.dishType
  		}
  		this.$http({
  			method:'POST',
  			url:apiUrl.getDishDetail,
  			data:params
  		}).then(function(response){			  
			  var data=JSON.parse(response.data);
//			  console.log(data);
  			_this.prasieFlag=data.IsPrasie//点赞状态
  			_this.collectFlag=data.IsCollect//收藏状态
  			_this.dishInfo=data.dishInfo//信息
			  _this.dishMateria=data.dishType1DishMateria//备料
			  _this.dishStep=data.dishType1DishStep//步骤
			  _this.dishSelect=	data.select//推荐
  			//设置导师推荐部分整体宽度
	  	  $(".playerSelect").width(115*_this.dishSelect.length);
	  	  //点赞状态
	  	  _this.prasieFlag=='t' ? _this.prasieImg='../../static/credit/zanr.png' : _this.prasieImg='../../static/credit/zanw.png';
  			//收藏状态
  			_this.collectFlag=='t' ? _this.collectImg='../../static/credit/love02.png' : _this.collectImg='../../static/credit/love01.png';
  			//备料展示  	
  			_this.list1=[];
  			_this.list2=[];
  			_this.list3=[];
			  for(var i=0;i<_this.dishMateria.length;i++){
	     	  var dishMater=_this.dishMateria[i];
	     	  if(dishMater.MaterialType=="1"){
	          var item1={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list1.push(item1);
	     	  }else if(dishMater.MaterialType=="2"){
	          var item2={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list2.push(item2);
	     	  }else if(dishMater.MaterialType=="3"){
	          var item3={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list3.push(item3);
	     	  }            	  
	      }
			  for(var i=0;i<data.selectDishMateria.length;i++){
			  	var selectDishMateria=data.selectDishMateria[i];
			  	var item4={
			  		label:selectDishMateria.FlavourName,
	          value:selectDishMateria.Unit+'g'
			  	}
			  	_this.list3.push(item4);
			  }
	      _this.dishlist=[{
		      	type:"主料",
		      	list:_this.list1
		      },{
		      	type:"辅料",
		      	list:_this.list2
		      },{
		      	type:"调料",
		      	list:_this.list3
			  }]
  		})
  	},
  	getVoteDishDetail(){//投票菜品详情
  		var _this=this;
  		var params={
  			"DishId":this.dishId,
  			"OpenId":this.openId, 
  			"DishType":this.dishType
  		}
  		this.$http({
  			method:'POST',
  			url:apiUrl.getVoteDishDetail,
  			data:params
  		}).then(function(response){			  
  			var data=JSON.parse(response.data);
//			  console.log(data);
  			_this.prasieFlag=data.IsPrasie//点赞状态
  			_this.collectFlag=data.IsCollect//收藏状态
  			_this.selectFlag=data.IsSelected//投票状态
  			_this.activityEnd=data.IsEnd//f投票未结束  t投票结束
  			_this.dishInfo=data.dishInfo//菜品信息
			  _this.dishMateria=data.dishType1DishMateria//备料
			  _this.dishStep=data.dishType1DishStep//步骤
			  _this.dishSelect=	data.select//推荐			  
  			//设置导师推荐部分整体宽度
	  	  $(".playerSelect").width(115*_this.dishSelect.length);  	  
	  	   //点赞状态
	  	  _this.prasieFlag=='f' ? _this.prasieImg='../../static/credit/zanw.png' : _this.prasieImg='../../static/credit/zanr.png';
  			//收藏状态
			  _this.collectFlag=='f' ? _this.collectImg='../../static/credit/love01.png' : _this.collectImg='../../static/credit/love02.png';  			
  			//投票状态
  			if(_this.activityEnd=='t'){//投票结束
  				$('.votebtn').addClass('vote_end');
  				$('.votebtn').html('投票结束');
  			}else{//投票未结束
  				if(_this.selectFlag=='f'){//未投票
	  				//未投票样式
	  				$('.votebtn').addClass('vote_before');
	  				$('.votebtn').html('我要投票');
	  			}else if(_this.selectFlag=='t'){//已投票
	  				$('.votebtn').addClass('vote_after');
	  				$('.votebtn').html('投票成功');
	  			}
  			}
  			
  			//备料展示  	
  			_this.list1=[];
  			_this.list2=[];
  			_this.list3=[];
			  for(var i=0;i<_this.dishMateria.length;i++){
	     	  var dishMater=_this.dishMateria[i];
	     	  if(dishMater.MaterialType=="1"){
	          var item1={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list1.push(item1);
	     	  }else if(dishMater.MaterialType=="2"){
	          var item2={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list2.push(item2);
	     	  }else if(dishMater.MaterialType=="3"){
	          var item3={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list3.push(item3);
	     	  }            	  
	      }
			  for(var i=0;i<data.selectDishMateria.length;i++){
			  	var selectDishMateria=data.selectDishMateria[i];
			  	var item4={
			  		label:selectDishMateria.FlavourName,
	          value:selectDishMateria.Unit+'g'
			  	}
			  	_this.list3.push(item4);
			  }
	      _this.dishlist=[{
		      	type:"主料",
		      	list:_this.list1
		      },{
		      	type:"辅料",
		      	list:_this.list2
		      },{
		      	type:"调料",
		      	list:_this.list3
			  }]
  		})
  	},
  	getPlayer(){//选手介绍
  		var _this=this;
  		var params='?dishId='+this.dishId+'&chefId='+this.chefId+'&openId='+this.openId;
  		this.$http({
  			method:"GET",
  			url:apiUrl.getPlayerIntroduce+params
  		}).then(function(response){
			  console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data)
  			_this.chefInfo=data.ChefInfo;
  			_this.chefInfoLink=data.chefInfoLink;
  			_this.chefInfoNoLink=data.chefInfoNoLink;
  		})
  	},
  	parsieClick(){//点赞
  		var _this=this;
  		if(this.prasieFlag=='f'){//未点赞
  			var params={
	  			"DishId":this.dishId ,
	  			"OpenId":this.openId, 
	  			"DishType":this.dishType
  		  }
  			this.$http({
  				method:'POST',
	  			url:apiUrl.addDishPrasie,
	  			data:params
  			}).then(function(response){
  				 var data=JSON.parse(response.data);
  				_this.dishInfo.PrasieCount=data.PrasieCount;
  				_this.prasieImg='../../static/credit/zanr.png';
			    _this.prasieFlag='t';
  			})
  		}  		
  	},
  	collectClick(){//收藏
  		var _this=this;
  		var params={
  			"DishId":this.dishId,
  			"OpenId":this.openId,
  			"DishType":this.dishType
  		}
  		if(this.collectFlag=='f'){//未收藏
  			this.$http({ 	//收藏
  				method:'POST',
  				url:apiUrl.addCollect,
  				data:params
  			}).then(function(response){
					 if(response.data=="OK"){
					 	 _this.collectImg='../../static/credit/love02.png';
  			     _this.collectFlag='t';
  			     //收藏成功样式
			       _this.colShow=true;
  			     setTimeout(function(){
  			     	_this.colShow=false;
  			     },1000)
					 }
  			})
  		}else{//已收藏
  			this.$http({ 	//取消收藏
  				method:'POST',
  				url:apiUrl.delCollect,
  				data:params
  			}).then(function(response){
					 if(response.data=="OK"){
					 	 _this.collectImg='../../static/credit/love01.png';
	  			   _this.collectFlag='f';
					 }
  			})
  		}
  	},
  	voteBtn(){//点击投票按钮
  		if(this.activityEnd=='f'){//投票未结束
  			if(this.selectFlag=='f'){//未投票
	      	var _this=this;
	      	var params={
	      		'DishId':this.dishId,
	      		'OpenId':this.openId,
	      		'Dishtype':this.dishType,
	      		'ChefActivityId':1
	      	}
	      	this.$http({//增加投票数
	      		method:'POST',
	      		url:apiUrl.addVoteNum,
	      		data:params
	      	}).then(function(response){
	      		if(response.data!='No'){
	      			 $('.votebtn').removeClass('vote_before');
		      		 $('.votebtn').addClass('vote_after');
						   $('.votebtn').html('投票成功');
		      		 _this.dishInfo.SelectedCount=JSON.parse(response.data).SelectedCount;
	      		}      		
	      	})
	      }
  		}
  	},
  	localdish(dishId){//点击本地菜
      this.dishId=dishId;
      pageIndex=1;//传参页数
      pageSize=3;//传参个数
  		this.getDishDetail();//菜品详情
  		this.getPlayer();//选手介绍
  		this.wordRequest();//选手菜品留言
  	},
  	wordRequest(){//留言请求
  		var _this =this;
  		var params='?dishId='+this.dishId+'&index='+pageIndex+'&pageSize='+pageSize
  		this.$http({
		  	method:'GET',
		  	url:apiUrl.getDishChefComment+params
		  }).then(function(response){		  
		  	  var data=JSON.parse(response.data);
//		  	  console.log(data);
		  	  _this.speakdata=data.data;
		  	  _this.count=data.Count;	  
//		  	  留言自适应高度
          _this.count > 3 ? $('.competitiondish .scrollheight').height(360) : $('.competitiondish .scrollheight').height(_this.count*120);
		  })  
  	},
  	load () {//上拉加载
	      setTimeout(() => {
//	      	pageIndex+=1;
          pageSize+=3;
					this.wordRequest();
	        setTimeout(() => {
	          this.$refs.demo2.donePullup()
	        }, 100)
	        if (this.speakdata.length ==this.count) { // unload plugin
	          setTimeout(() => {
	            this.$refs.demo2.disablePullup()
	          }, 100)
	        }
	      }, 500)
	  },
	  focus(){
		   this.flag1=false;  
		   self=this;
		   setTimeout(function(){
		   	 $('.sendsec textarea').focus();
		   },100);
		   $('.sendto').height(30);
		   $(document).click(function(ev){
		   	 self.flag1=true;  
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
  		var wordSize=$('.wordUl li').length;//当前留言长度
  		var _this=this;
  		if(this.message!=''){	
         var params={
         	"DishId":this.dishId,
         	"OpenId":this.openId,
         	"MemberName":this.userData.Nickname,
         	"HeadPicUrl":this.userData.Headimgurl,
         	"Comment":this.message
         	}
         this.$http({//增加留言
         	 method:'POST',
         	 url:apiUrl.addDishChefComment,
         	 data:params
         }).then(function(response){
         	  pageSize=wordSize+1;
            _this.wordRequest();
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
	.competitiondish{background: #ffffff;}
	.comdish{width: 100%;height: 400px;}	
	.look{color: #8c8c8c;text-align: right;padding-right: 10px;font-size: 12px;}
	.dishname{width: 50%;height:35px;margin-left: 25%;background: url('../../static/credit/back.png');background-size: 100% 100%;color:white;line-height: 35px;text-indent: 1em;font-size: 18px;}
	.voteDiv{padding-left: 10px;padding-right: 10px;margin-top: 20px;font-size: 12px;height: 28px;line-height: 28px;}
	.votebtn{width: 85px;height: 28px;border-radius: 5px;font-size: 12px;text-indent: 20px;outline: none;}
	.vote_before{border: 1px solid #e83428;color: #e83428;background: url('../../static/credit/vote02.png') no-repeat 5px center #ffffff;background-size: 20px 20px;}	
	.vote_after{border: 1px solid #e83428;color: #ffffff;background: url('../../static/credit/vote01.png') no-repeat 5px center #e83428;background-size: 20px 20px;}
	.vote_end{border: 1px solid #a2a2a2;color: #ffffff;background: url('../../static/credit/vote01.png') no-repeat 5px center #a2a2a2;background-size: 20px 20px;}
	.vote{color: #e83428;}
	.good{float: right;}
	.good *{display: inline-block;vertical-align: middle;}
	.good img{width: 20px;}
	.good span{color: #8c8c8c;}
	.alike *{display: inline-block;vertical-align: middle;}
	.alike img{width: 20px;}
	.alike span{color: #e83426;font-size: 15px;font-weight: bold;}
  .show{margin-top: 20px;padding-left: 10px;padding-right: 10px;}
	.show1{margin-bottom: 20px;}
	.player{display: flex;margin-top: 20px;color: #3E3E3E;}
	.player div:nth-child(1){width: 30%;}
	.player div:nth-child(1) img{width: 100px;height: 100px;}
	.player div:nth-child(2){width: 70%;margin-left: 20px;}
	.title{font-size:19px;}
	.smalltitle{font-size: 13px;}
	.smalltitle1{ margin-top: 8px;}
	.localdish{color: #E83428;}
	.localdish img{width: 15px;position: relative;top: 3px;}
	.storytext{font-size: 13px;text-indent: 1em;margin-top:20px;color: #3E3E3E;}	
	.cellTitle{background: #f9f9f9 !important;color: #e83426 !important;}
	.cooktext{font-size: 13px;color: #3e3e3e;margin-top: 15px;}
	.playerSelect {height: 130px;position: relative;margin-top: 20px;}
	.playerSelect-item {width: 100px;height: 130px;display:inline-block; margin-left: 10px;float: left; text-align: center;font-size: 12px;}
	.playerSelect-item p{margin-top: 10px;color: #3E3E3E;}
	.box{width: 100px;height: 100px;background: #f9f9f9;}
	.box img{width: 50%;height: 100px;}
	.wordUl{width: 100%;list-style: none;}
	.wordUl li{display: flex;padding-top: 30px;padding-bottom: 20px;}
	.leftDiv{width: 18%;}
	.rightDiv{width: 82%;font-size: 12px;color: #282828;padding-right: 10px;}
	.rightDiv p:nth-child(1){font-weight: bold;}	
	.rightDiv p:nth-child(2){word-wrap: break-word;}
	.rightDiv p:nth-child(3){	text-align: right;color: #bebebe;margin-top: 15px;}
	.leftDiv img{width: 33px;margin-left: 20px;}
	.sendto{height: 0px;}
	.send{width: 100%;background: #FFFFFF;position: absolute;bottom:0px;}	
	.sendfir{width: 100%;height: 50px;border-top: 1px solid #efefef;}
	.sendfir input{width:80%;height: 35px;border: none;background: #f6f6f6;margin-left: 4%;border-radius: 5px;text-indent: 1em;margin-top: 8px;outline: none;}
	.sendfir img{width: 20px;margin-left: 4%;position: relative;top: 6px;}
	.sendsec{width: 90%;margin-left: 5%;text-align: right;}
	.txtArea{height:45px;background:#f6f6f6;position: relative;}		
	.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
	.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px;}
	.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;margin-top: 10px;margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
	.colSuc{width: 50%;	position:absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.competitiondish .weui-cell:before{border-top: none !important;}
	.competitiondish .weui-form-preview__label { min-width: 0 !important; color: #3e3e3e !important;font-size: 13px;}           
  .competitiondish .weui-form-preview__value {color: #6c6c6c;font-size: 13px;}       
  
  .competitiondish .scroller-box { position: relative; }
	.competitiondish .scroller-box .mask-scroller { width: 100%;position: absolute; left: 0; bottom: 0; background: red; }
	.competitiondish .scroller-box .modify-scroller { overflow: auto; }
	.competitiondish .scroller-box .modify-scroller { overflow-x: visible; }
	.competitiondish .scroller-box .modify-scroller::-webkit-scrollbar { display: none; }
	.competitiondish .scroller-box .modify-scroller:horizontal { display: none; }
</style>