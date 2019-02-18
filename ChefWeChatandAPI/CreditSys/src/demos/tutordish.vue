<template>
  <div class="tutordish" style="height: 100%;">
  	 	<div class="page">
				<div class="content">
					 <!--头部-->
			     <img v-bind:src="dishTutor.DishPic" class="dishImg"/>
			     <p class="look">{{dishTutor.VisitCount}}人看过</p>
			     <div class="dishname">{{dishTutor.DishName}}</div>
			     <p class="good">
			     	 <img v-bind:src="img" @click='prasieClick'/>
			     	 <span>{{dishTutor.PrasieCount}}</span>
			     </p>
			     <!--菜品故事-->
			     <div class="show">
				     <div class="alike">
				     	 <img src="../../static/credit/book1.png" />
				     	 <span>菜品故事</span>
				     </div>
				     <p class="storytext"></p>
			     </div>
			     <!--备料展示-->
			     <div class="alike show show1">
			     	 <img src="../../static/credit/show.png" />
			     	 <span>备料展示</span>
			     </div>
			     <group v-for="(item,index) in dishlist" :key="index" gutter=0>
				     <cell :title='item.type' class='cellTitle'></cell>
				     <cell-form-preview :list="item.list"></cell-form-preview>
			     </group> 
			     <!--烹饪步骤-->
			     <div class="show">
				     <div class="alike">
				     	 <img src="../../static/credit/cook.png" />
				     	 <span>烹饪步骤</span>
				     </div> 
				     <p class="cooktext" v-for="(item,index) in dishStep" :key="index">{{item.Step}}、{{item.StepName}}</p>
			     </div>
			     <!--导师推荐-->
			     <div class="alike show">
			     	 <img src="../../static/credit/recom.png" />
			     	 <span>导师推荐</span>
			     </div>      
			     <div class="scroller-box">
					    <div class="modify-scroller">
					      <div class="box1">
					        <div class="box1-item" v-for="(item,index) in flavourRec" :key="index">
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
				</div>
				<!--底部-->
					<div class="footer">
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
			</div>
	</div>
 </div>
</template>

<script>
import { CellFormPreview, Group, Cell,Scroller,cookie} from 'vux'
import apiUrl from "../apiUrls.js"
var pageIndex=1;//传参页数
var pageSize=3;//传参个数
export default {
  components: {
    CellFormPreview,
    Group, 
    Cell,
    Scroller,
    cookie
  },
  data () {
    return{
    	openId:'',
    	userData:{},
    	dishTutor:{},
    	dishMaterial:[],//调料展示
    	dishStep:[],//菜品步骤
    	flavourRec:[],//导师推荐
    	flagPrasie:'',//点赞状态
      img:'',//点赞图标
      collectFlag:'',//收藏状态
      collectImg:'',//收藏图标
      colShow:false,//收藏成功
      dishId:'',//菜品id
      list1:[],
      list2:[],
      list3:[],
      dishlist:[],
      speakdata:"",//留言数据
      flag1:true,//底栏显示
    	message:'',//留言内容
    	datetime:'',//留言时间
    	leng:500,//内容长度
    	count:'',//留言个数
    	pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
      }
    }
  },
  mounted(){	
  	this.dishId=this.$route.query.dishId;
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
	  this.dataRequest();	
	  this.wordRequest();	  
  },
  methods:{  	
  	prasieClick(){//点赞
  		var _this=this;
  		if(this.flagPrasie=='f'){//未点赞			
        var params={
        	"DishId":this.dishId,
        	"OpenId": this.openId,
        	"DishType":1
        }
  			this.$http({ 	
  				method:'POST',
  				url:apiUrl.addPrasie,
  				data:params
  			}).then(function(response){
//				 console.log(JSON.parse(response.data));
				   var data=JSON.parse(response.data);
				   _this.dishTutor.PrasieCount=data.PrasieCount;
				   _this.img='../../static/credit/zanr.png';
			     _this.flagPrasie='t';
  			})
  		}
  	},
  	collectClick(){//收藏
  		var _this=this;
  		var params={
  			"DishId":this.dishId,
  			"OpenId": this.openId,
  			"DishType":1
  		}
  		if(this.collectFlag=='f'){//未收藏  			
  			this.$http({ 	//收藏
  				method:'POST',
  				url:apiUrl.addCollect,
  				data:params
  			}).then(function(response){
//					 console.log(response.data);
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
//				 console.log(response.data);
					 if(response.data=="OK"){
					 	 _this.collectImg='../../static/credit/love01.png';
	  			   _this.collectFlag='f';
					 }
  			})
  		}
  	},
  	dataRequest(){//数据请求
  		var _this=this;
  		var params={
  			"DishId":this.dishId ,
	  		"OpenId": this.openId,
	  		"DishType":1
  		}
		  this.$http({
		  	method:'POST',
		  	url:apiUrl.getTutorDish,
		  	data:params
		  }).then(function(response){		  	
		    var data=JSON.parse(response.data);
		    console.log(data);
	  	  _this.dishTutor=data.dishTutor;//头部数据
	  	  _this.dishStep=data.dishStep;//烹饪步骤
	  	  _this.flavourRec=data.flavourRec;//导师推荐
	  	  _this.dishMaterial=data.dishMaterial;//备料展示
	  	  _this.flagPrasie=data.flagPrasie;//点赞状态
	  	  _this.collectFlag=data.IsCollect;//收藏状态
	  	  $('.storytext').html(_this.dishTutor.DishStory);//菜品故事
	  	  $('.storytext p').css({
	  	  	'margin-top':'10px',	  	  	
	  	  })
	  	  $('.storytext img').css({
	  	  	'width':'100%',	  	  	
	  	  })
        //设置导师推荐部分整体宽度
	  	  $(".box1").width(115*_this.flavourRec.length);
	      //点赞状态
	      _this.flagPrasie=='t' ? _this.img='../../static/credit/zanr.png' :  _this.img='../../static/credit/zanw.png';
				//收藏状态
				_this.collectFlag=='t' ? _this.collectImg='../../static/credit/love02.png' : _this.collectImg='../../static/credit/love01.png';
	      for(var i=0;i<_this.dishMaterial.length;i++){
	     	  var dishMater=_this.dishMaterial[i];
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
  	wordRequest(){//留言请求
  		var _this =this;
  		var params='?dishId='+this.dishId+'&pageIndex='+pageIndex+'&pageSize='+pageSize
  		this.$http({
		  	method:'GET',
		  	url:apiUrl.getDishComment+params
		  }).then(function(response){	
		  	  var data=JSON.parse(response.data);
		  	  _this.speakdata=data.data;
		  	  _this.count=data.Count;	  
//		  	  console.log(_this.speakdata);
//        留言自适应高度
          _this.count > 3 ? $('.tutordish .scrollheight').height(360) : $('.tutordish .scrollheight').height(_this.count*120);
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
		   var _this=this;
		   setTimeout(function(){
		   	 $('.sendsec textarea').focus();
		   },100);
		   $('.sendto').height(30);
		   $(document).click(function(){
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
  		var wordSize=$('.wordUl li').length;//当前留言长度
  		var _this=this;
  		if(this.message!=''){  			 		
	  		//新增菜品留言
	  		var params={
	  			"DishId":this.dishId ,
	  			"OpenId": this.openId,
	  			"MemberName":this.userData.Nickname,
	  			"HeadPicUrl":this.userData.Headimgurl,
	  			"Comment":this.message
	  		}
	  		this.$http({
	  			method:'POST',
	  			url:apiUrl.addDishComment,
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
	.tutordish{background: #ffffff;}
	.dishImg{width: 100%;}
	.look{color: #8c8c8c;text-align: right;padding-right: 10px;font-size: 12px;}
	.dishname{width: 50%;height:35px;margin-left: 25%;background: url('../../static/credit/back.png');background-size: 100% 100%;color:white;line-height: 35px;text-indent: 1em;font-size: 18px;}	
	.good{text-align: right;padding-right: 10px;font-size: 12px;}
	.good *{display: inline-block;vertical-align: middle;}
	.good img{width: 20px;}
	.good span{color: #8c8c8c;}
	.alike *{display: inline-block;vertical-align: middle;}
	.alike img{width: 20px;}
	.alike span{color: #e83426;font-size: 15px;font-weight: bold;}
  .show{margin-top: 20px;padding-left: 10px;padding-right: 10px;}
	.show1{margin-bottom: 20px;}
	.storytext{font-size: 13px;color: #3E3E3E;}	
	.cellTitle{background: #f9f9f9 !important;color: #e83426 !important;}	
	.cooktext{font-size: 13px;color: #3e3e3e;margin-top: 15px;}
	.box1 { height: 130px;position: relative;margin-top: 20px;}
	.box1-item {width: 100px;height: 130px;display:inline-block;margin-left: 15px;float: left;text-align: center;font-size: 12px;}
	.box1-item p{margin-top: 10px;color: #3E3E3E;}
	.box{width: 100px;height: 100px;}
	.box img{width: 100%;height: 100%;}
	.wordUl{width: 100%;list-style: none;}
	.wordUl li{display: flex; padding-top: 30px;padding-bottom: 20px;}
	.leftDiv{width: 18%;}
	.rightDiv{width: 82%;font-size: 12px;color: #282828;padding-right: 10px;}
	.rightDiv p:nth-child(1){font-weight: bold;}
	.rightDiv p:nth-child(2){word-wrap: break-word;}
	.rightDiv p:nth-child(3){text-align: right;color: #bebebe;	margin-top: 15px;}
	.leftDiv img{width: 33px;margin-left: 20px;}
	.sendto{height: 0px;}
	.send{width: 100%;background: #FFFFFF;}
	.sendfir{width: 100%;height: 50px;border-top: 1px solid #efefef;}
	.sendfir input{width:80%;height: 35px;border: none;background: #f6f6f6;margin-left: 4%;border-radius: 5px;text-indent: 1em;margin-top: 8px;outline: none;}
	.sendfir img{width: 20px;margin-left: 4%;position: relative;top: 6px;}
	.sendsec{width: 90%;margin-left: 5%;text-align: right;}
	.txtArea{height:45px;background:#f6f6f6;position: relative;}
	.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
	.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px;}
	.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;margin-top: 10px;margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
	.colSuc{width: 50%;position: absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
	
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
	.tutordish .weui-cell:before{border-top: none !important;}
	.tutordish .weui-form-preview__label {min-width: 0 !important;color: #3e3e3e !important;font-size: 13px;}
  .tutordish .weui-form-preview__value {color: #6c6c6c;font-size: 13px;}
  .tutordish .weui-cells:after{display: none;}
  /*推荐调料左右滑动*/
  .tutordish .scroller-box { position: relative; }
	.tutordish .scroller-box .mask-scroller { width: 100%;position: absolute; left: 0; bottom: 0; background: red; }
	.tutordish .scroller-box .modify-scroller { overflow: auto; }
	.tutordish .scroller-box .modify-scroller { overflow-x: visible; }
	.tutordish .scroller-box .modify-scroller::-webkit-scrollbar { display: none; }
	.tutordish .scroller-box .modify-scroller:horizontal { display: none; }
</style>