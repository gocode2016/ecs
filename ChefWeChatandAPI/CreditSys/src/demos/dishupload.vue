<template>
  <div class="dishupload">
  	<div class="dish_wrap">
    <!--菜品名称-->
    <div class="upload" @click="imgUpLoad">
    	<span v-if="dishImgFlag">十&nbsp;菜品照片</span>
    	<img v-else :src="dishImgUrl" />
    </div>
    <input type="text" placeholder="请填写菜品名字" class="dishname" v-model="dishName"/>
    
    <!--菜品故事-->
    <div class="alike show">
		 	 <img src="../../static/credit/book1.png" />
		 	 <span>菜品故事</span>
	  </div>
	  <group>
      <x-textarea  placeholder="说说这道菜背后的故事吧~"  v-model="dishStory"></x-textarea>
    </group>
    
    <!--备料展示-->
     <div class="alike show">
     	 <img src="../../static/credit/show.png" />
     	 <span>备料展示</span>
     </div>    
     <!--主料-->
     <p class="title_show">主料</p>
     <ul class="main">
     	 <li>
     	 	 <input class="maintext" type="text" placeholder="主料：如牛肉">
     	 	 <input class="weight" type="text">
     	 	 <span class='gram'>g&nbsp;</span>
     	 	 <img src="../../static/credit/jian.png" class="remove"/>    	 	     	 	
     	 </li>    	
     </ul> 
     <p class="add"><img src="../../static/credit/jia.png" @click="add('main')"/></p>
     <!--辅料-->
     <p class="title_show">辅料</p>
     <ul class="access">
     	 <li>
     	 	 <input class="maintext" type="text" placeholder="食材：如百合">
     	 	 <input class="weight" type="text">
     	 	 <span class="gram">g&nbsp;</span>
     	 	 <img src="../../static/credit/jian.png" class="remove"/>     	 	     	 	
     	 </li>     	 
     </ul>
     <p class="add"><img src="../../static/credit/jia.png" @click="add('access')"/></p>
     <!--调料-->
     <p class="title_show">调料</p>
     <ul class="season">
     	 <li v-for="(item,index) in dataArr" :key="index">
     	 	 <input class="maintext" :id="item.MySelfFRId" type="text" placeholder="自动显示对应调料标签" v-model="item.FRName" readonly >
     	 	 <input class="weight" type="text" v-model="item.Unit">
     	 	 <span class="gram">g&nbsp;</span>
     	 	 <img src="../../static/credit/jian.png" class="remove"/>     	 	     	 	
     	 </li>     	 
     </ul>
     <!--其他调料-->
     <p class="remarks">(请填写其他调料)</p>
     <ul class="othersea">
     	 <li>
     	 	 <input class="maintext" type="text" placeholder="味达美冰糖老抽">
     	 	 <input class="weight" type="text">
     	 	 <span class="gram">g&nbsp;</span>
     	 	 <img src="../../static/credit/jian.png" class="remove"/>     	 	     	 	
     	 </li>     	 
     </ul>
     <p class="add"><img src="../../static/credit/jia.png" @click="add('othersea')"/></p>
     
      <!--烹饪步骤-->
     <div class="alike show">
	     	 <img src="../../static/credit/cook.png" />
	     	 <span>烹饪步骤</span>
	   </div> 
	   <ul class='cookStep'>
	   	 <li>
		   	 	<span>1.</span> 	   	 
	        <textarea class="stepDetail" placeholder="添加步骤说明"></textarea>	       
	        <img src="../../static/credit/jian.png" class="remove"/>
	   	 </li>
	   </ul>
	   <p class="add"><img src="../../static/credit/jia.png" @click="addCookStep"/></p>
	   
	   <!--手机号-->
	   <p class="telePro">注：菜品审核通过后，将充值10元话费到本手机号，务必填写准确</p>
	   <p class="teleText">手机号：<input type="text" class="telephone" v-model="PhoneNum"></p>
	   
	   <!--按钮-->
	   <button @click="addDish" class="addBtn">提交</button>
	   
	  <!--提交成功页面-->
    <div class="sucDiv" v-show='sucFlag'>
      <img src="../../static/credit/success.png" />
      <p>恭喜你！提交成功</p>
      <p>24小时内将告知审核结果</p>
      <button @click="closeBtn">关闭</button>
    </div>
    
    <!--点击保存信息不完整提示-->
    <div class="alert-mask" v-show='maskFlag'>
      <div class="menu">
         <p>{{protext}}</p>
         <button @click="menuBtn">知道了</button>
      </div>
    </div>
	  </div>
    
  </div>
</template>

<script>
import { XTextarea, Group,cookie,Loading } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'

export default{
	components:{
		XTextarea,
		Group,
		cookie
	},
	data(){
		return{
			num:0,//点击提交的次数
			mySelfDishId:'',//菜品id 编辑菜品
			userId:'',
			userData:{},
			timestamp:'',
  		nonceStr:'',
  		signature:'',
  		currentUrl:'',//当前地址
			dishImgFlag:true,//菜品图片位置 显示文字
			dishImgUrl:'',//菜品图片
			dishName:'',//菜品名称
			PhoneNum:'',//电话
			dishStory:'',//菜品故事
			DishMaterial:[],//调料
			MySelfFRRole:[],//推荐调料
			DishStep:[],//步骤
			dataArr:[],//用户选择的推荐调料
			sucFlag:false,//提交成功
			maskFlag:false,//提示蒙版
			protext:''//信息不完整提示
		}
	},
	methods:{
		addDish(){//点击上传菜品
//			this.dishImgUrl="https://img.alicdn.com/bao/uploaded/i1/1037158632/TB2vONyr4RDOuFjSZFzXXcIipXa_!!1037158632.jpg_b.jpg";//测试
			if(this.dishImgUrl==''||this.dishName==''||this.dishStory==''||this.PhoneNum==''){
//				this.maskFlag=true;
//				this.protext="信息需要填写完整哦";
				alert('信息需要填写完整哦');
			}else if($('.main li').length==0||$('.access li').length==0||$('.othersea li').length==0||$('.season li').length==0||$('.cookStep li').length==0){
//				this.maskFlag=true;
//				this.protext="信息需要填写完整哦";
				alert('信息需要填写完整哦');
			}else{
				var reg = /^[0-9]+$/;   //调料重量必须为数字 	
	      var box=/^1\d{10}$/;//手机号正则
				this.DishMaterial=[];//主料辅料其他调料
	    	this.MySelfFRRole=[];//推荐调料
	    	this.DishStep=[];//步骤
	    	
	    	if(!box.test(this.PhoneNum)){
	    		alert("请填写正确的手机号");
	    		return;
	    	}
	    	
				for(var i=0;i<$('.main li').length;i++){//主料
	    		var text1=$('.main .maintext').eq(i).val();
	    		var weight1=$('.main .weight').eq(i).val();
	    		if(text1==''||weight1==''){
//	    			this.maskFlag=true;
//				    this.protext="信息需要填写完整哦";
				    alert('信息需要填写完整哦');
	    			return;
	    		}else if(!reg.test(weight1)){
//	    			this.maskFlag=true;
//				    this.protext="用量必须是数字哦";
				    alert('用量必须是数字哦');
	    			return;
	    		}else{
	    			var item1={
							"Material":text1,
							"Unit":weight1,
							"MaterialType":1
					  }
	    			this.DishMaterial.push(item1);
	    		}
		    }
				
				for(var i=0;i<$('.access li').length;i++){//辅料
	    		var text2=$('.access .maintext').eq(i).val();
	    		var weight2=$('.access .weight').eq(i).val();
	    		if(text2==''||weight2==''){
//	    			this.maskFlag=true;
//						this.protext="信息需要填写完整哦";
						alert('信息需要填写完整哦');
	    			return;
	    		}else if(!reg.test(weight2)){
//	    			this.maskFlag=true;
//				    this.protext="用量必须是数字哦";
				    alert('用量必须是数字哦');
	    			return;
	    		}else{
	    			var item2={
							"Material":text2,
							"Unit":weight2,
							"MaterialType":2
					  }
	    			this.DishMaterial.push(item2);
	    		}
		    }
				
				for(var i=0;i<$('.othersea li').length;i++){//其他调料
	    		var text3=$('.othersea .maintext').eq(i).val();
	    		var weight3=$('.othersea .weight').eq(i).val();
	    		if(text3==''||weight3==''){
//	    			this.maskFlag=true;
//						this.protext="信息需要填写完整哦";
				    alert('信息需要填写完整哦');
	    			return;
	    		}else if(!reg.test(weight3)){
//	    			this.maskFlag=true;
//				    this.protext="用量必须是数字哦";
				    alert('用量必须是数字哦');
	    			return;
	    		}else{
	    			var item3={
							"Material":text3,
							"Unit":weight3,
							"MaterialType":3
					  }
	    			this.DishMaterial.push(item3);
	    		}
		    }
				
				for(var i=0;i<$('.season li').length;i++){//调料
	    		var text4=$('.season .maintext').eq(i).val();
	    		var weight4=$('.season .weight').eq(i).val();
	    		if(text4==''||weight4==''){
//	    			this.maskFlag=true;
//						this.protext="信息需要填写完整哦";
				    alert('信息需要填写完整哦');
	    			return;
	    		}else if(!reg.test(weight4)){
//	    			this.maskFlag=true;
//				    this.protext="用量必须是数字哦";
				    alert('用量必须是数字哦');
	    			return;
	    		}else{
	    			var MySelfFRId=parseInt($('.season .maintext').eq(i).attr('id'));
	    			var item4={
							"MemberId":this.userId,
							"MySelfFRId":MySelfFRId,
							"Unit":weight4
						}
	    			this.MySelfFRRole.push(item4);
	    		}
		    }
				
				for(var i=0;i<$('.cookStep li').length;i++){//步骤
	    		var text5=$('.cookStep .stepDetail').eq(i).val();
	    		if(text5==''){
//	    			this.maskFlag=true;
//						this.protext="信息需要填写完整哦";
				    alert('信息需要填写完整哦');
	    			return;
	    		}else{
	    			var item5={
							"StepName":text5
						}
	    			this.DishStep.push(item5);
	    		}
		    }
				
				this.num++;
				if(this.num==1&&this.mySelfDishId==0){
					this.addDishUpload();//上传菜品
				}else if(this.num==1&&this.mySelfDishId!=0){
					this.updateMySelfDish();//修改菜品信息
				}
				
			}
		},
		addDishUpload(){//上传菜品接口
			this.showLoading () ;
			var _this=this;
			var params={
				"MySelfDishKu":{
					"MemberId":this.userId,
					"DishPicUrl":this.dishImgUrl,
					"DishStory":this.dishStory,
					"DishName":this.dishName,
					"PhoneNum":this.PhoneNum
				},
				"DishMaterial":this.DishMaterial,
				"DishStep":this.DishStep,
				"MySelfFRRole":this.MySelfFRRole
			}
  		this.$http({
  			method:'post',
  			url:apiUrl.addDishUpload,
  			data:params
  		}).then(function(response){
  			_this.$vux.loading.hide();//loading图标消失
  			if(response.data=='OK'){
  				_this.sucFlag=true;
  			}else{
  				_this.num=0;
//				_this.maskFlag=true;
//					_this.protext="网络请求超时，请重新提交";
					alert("网络请求超时，请重新提交");
  			}
  		})
		},
		getMySelfDish(){//获取菜品信息 进行编辑
			var _this=this;
			var params="?mySelfDishId="+this.mySelfDishId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getMySelfDish+params,
  		}).then(function(response){
  			var data=JSON.parse(response.data);
//			console.log(data);
  			if(data.dishInfo.IsApply==0){//菜品审核状态  0、待审核   1、审核通过   2、审核不通过
  				//待审核
  				_this.sucFlag=true;
  				$('.sucDiv p').eq(0).html('提交成功，正在审核');
  			}else if(data.dishInfo.IsApply==1){
  				//审核通过
  				_this.sucFlag=true;
  				$('.sucDiv p').eq(0).html('恭喜你！审核已通过');
  				$('.sucDiv p').eq(1).hide();
  			}else if(data.dishInfo.IsApply==2){//审核不通过 展示菜品信息进行编辑
  				//菜品信息
  				_this.dishName=data.dishInfo.DishName;//菜品名称
  				_this.dishImgFlag=false;
  				_this.dishImgUrl=data.dishInfo.DishPicUrl;//菜品图片
  				_this.dishStory=data.dishInfo.DishStory;//菜品故事
  				_this.PhoneNum=data.dishInfo.PhoneNum;//电话
  				
  				//主料 辅料 其他调料展示
  				var dishMaterial=data.dishMaterial;//调料
  				var mainArr=[];//主料
	        var accessArr=[];//辅料
	        var otherseaArr=[];//其他调料
	        for(var i=0;i<dishMaterial.length;i++){
	        	var material=dishMaterial[i];
	        	if(material.MaterialType==1){//主料
	        		mainArr.push(material);
	        	}else if(material.MaterialType==2){//辅料
	        		accessArr.push(material);
	        	}else if(material.MaterialType==3){//其他调料
	        		otherseaArr.push(material);
	        	}
	        }
	        for (var i=0;i<mainArr.length;i++){//主料
	        	var mainarr=mainArr[i];
	        	if(i==0){
	        		$('.main .maintext').eq(i).val(mainarr.Material);
	        		$('.main .weight').eq(i).val(mainarr.Unit);
	        	}else{
	        		var li = '<li data-v-a2bbc6a4>' + 
	                  '<input data-v-a2bbc6a4 class="maintext" type="text"  placeholder="食材" value='+mainarr.Material+'>' + 
	                  '<input data-v-a2bbc6a4 class="weight" type="text" placeholder="用量" value='+mainarr.Unit+'>' + 
	                  '<span data-v-a2bbc6a4 class="gram">g</span>' + 
	                  '<img data-v-a2bbc6a4 src="../../static/credit/jian.png" class="remove"/>' +               
	                 '</li>';
	            $('.main').append(li);
	        	} 
	        }
	        for (var i=0;i<accessArr.length;i++){//辅料
	        	var accessarr=accessArr[i];
	        	if(i==0){
	        		$('.access .maintext').eq(i).val(accessarr.Material);
	        		$('.access .weight').eq(i).val(accessarr.Unit);
	        	}else{
	        		var li = '<li data-v-a2bbc6a4>' + 
	                  '<input data-v-a2bbc6a4 class="maintext" type="text"  placeholder="食材" value='+accessarr.Material+'>' + 
	                  '<input data-v-a2bbc6a4 class="weight" type="text" placeholder="用量" value='+accessarr.Unit+'>' + 
	                  '<span data-v-a2bbc6a4 class="gram">g</span>' + 
	                  '<img data-v-a2bbc6a4 src="../../static/credit/jian.png" class="remove"/>' +               
	                 '</li>';
	            $('.access').append(li);
	        	} 
	        }
	        for (var i=0;i<otherseaArr.length;i++){//其他调料
	        	var otherseaarr=otherseaArr[i];
	        	if(i==0){
	        		$('.othersea .maintext').eq(i).val(otherseaarr.Material);
	        		$('.othersea .weight').eq(i).val(otherseaarr.Unit);
	        	}else{
	        		var li = '<li data-v-a2bbc6a4>' + 
	                  '<input data-v-a2bbc6a4 class="maintext" type="text"  placeholder="食材" value='+otherseaarr.Material+'>' + 
	                  '<input data-v-a2bbc6a4 class="weight" type="text" placeholder="用量" value='+otherseaarr.Unit+'>' + 
	                  '<span data-v-a2bbc6a4 class="gram">g</span>' + 
	                  '<img data-v-a2bbc6a4 src="../../static/credit/jian.png" class="remove"/>' +               
	                 '</li>';
	            $('.othersea').append(li);
	        	} 
	        }
  				
  				//推荐调料展示
  				_this.dataArr=data.qrole;//推荐调料
          
          //步骤展示
  				var dishStep=data.dishStep;//步骤
  				for(var i=0;i<dishStep.length;i++){
	        	var step=dishStep[i];
	        	if(i==0){
	        		$('.cookStep span').eq(i).val(step.StepId);
	        		$('.cookStep .stepDetail').eq(i).val(step.StepName);
	        	}else{
	        		var li='<li data-v-a2bbc6a4><span data-v-a2bbc6a4>'+step.StepId+'.</span><textarea data-v-a2bbc6a4 class="stepDetail" placeholder="添加步骤说明">'+step.StepName+'</textarea><img data-v-a2bbc6a4 src="../../static/credit/jian.png" class="remove"/></li>';
	            $('.cookStep').append(li);
	        	}       	
	        }   
  				
  			}
  		})
		},
		updateMySelfDish(){//修改菜品信息
			this.showLoading () ;
			var _this=this;
			var params={
  		  "MySelfDishKu":{
  		  	"MySelfDishId":this.mySelfDishId,
  		  	"DishPicUrl":this.dishImgUrl,
  		  	"DishStory":this.dishStory,
  		  	"DishName":this.dishName,
  		  	"IsApply":2,
  		  	"PhoneNum":this.PhoneNum
  		  },
  		  "DishMaterial":this.DishMaterial,
  		  "DishStep":this.DishStep,
  		  "MySelfFRRole":this.MySelfFRRole
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMySelfDish,
  			data:params
  		}).then(function(response){
  			_this.$vux.loading.hide();//loading图标消失
  			if(response.data=='OK'){
  				_this.sucFlag=true;
  			}else{
  				_this.num=0;
//				_this.maskFlag=true;
//					_this.protext="网络请求超时，请重新提交";
					alert("网络请求超时，请重新提交");
  			}
  		})
		},
		setConfig(){//配置config
        var _this=this;
		  	this.$http({
		  		method:'GET',
		  		url:apiUrl.getTimestampAndNonceStr
		  	}).then(function(response){
		  		var data=response.data;
		  		var dataArr=data.split('|');
		  		_this.timestamp=parseInt(dataArr[0]);//时间戳
		  		_this.nonceStr=dataArr[1];//随机字符串
		  		
		      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.currentUrl;
		  		_this.$http({
		  			method:'GET',
		  			url:apiUrl.getJsapiTicketSignature+params
		  		}).then(function(response){
		  			 _this.signature=response.data;
		  			 wx.config({
					      debug: false,
					      appId: apiUrl.appId,
					      timestamp: _this.timestamp,
					      nonceStr: _this.nonceStr,
					      signature: _this.signature,
					      jsApiList: [
					        'chooseImage',
					        'uploadImage'
					      ]
					   })
		  		})		  		
	  	  })
     },
     imgUpLoad(){//上传图片
     	  var localIds='';
     	  var _this=this;
     	  wx.chooseImage({
					  count: 1, // 默认9
					  sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
					  sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
					  success: function (res) {
					    localIds = res.localIds;				    
					    // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
					    // localId 可以用于微信手机版图片显示（目前PC版不可用）
	//								    _this.memberphoto=localIds[0];
					    wx.uploadImage({
							  localId:localIds[0] , // 需要上传的图片的本地ID，由chooseImage接口获得
							  isShowProgressTips: 1, // 默认为1，显示进度提示
							  success: function (res) {
							    var serverId = res.serverId; // 返回图片的服务器端ID
					        _this.$http({
					        	method:'GET',
					        	url:apiUrl.saveWeChatImage+'?serverId='+serverId
					        }).then(function(res){                       	
					        	_this.dishImgUrl=apiUrl.getUrl+res.data;
					        	_this.dishImgFlag=false;
					        })
							  }
					    })
					  }
			 })						 
     },
     add(val){//点击＋ 添加调料
     	  var li='<li data-v-a2bbc6a4>'+
			     	 	 '<input class="maintext" type="text" placeholder="主料：如牛肉" data-v-a2bbc6a4>'+
			     	 	 '<input class="weight" type="text" data-v-a2bbc6a4>'+
			     	 	 '<span class="gram" data-v-a2bbc6a4>g&nbsp;</span>'+
			     	 	 '<img src="../../static/credit/jian.png" class="remove" data-v-a2bbc6a4/>'+    	 	
			     	   '</li>'
	     	if(val=='main'){//添加主料
	     		$('.dishupload .main').append(li);
	     	}else if(val=='access'){//添加辅料
	     		$('.dishupload .access').append(li);
	     	}else if(val=='othersea'){//添加其他调料
	     		$('.dishupload .othersea').append(li);
	     	}
     },
     addCookStep(){//添加步骤
     	  var length=$('.dishupload .cookStep li').length+1;
	      var li='<li data-v-a2bbc6a4><span data-v-a2bbc6a4>'+length+'.</span><textarea class="stepDetail" placeholder="添加步骤说明" data-v-a2bbc6a4></textarea><img src="../../static/credit/jian.png" class="remove" data-v-a2bbc6a4/></li>';
	      $('.dishupload .cookStep').append(li);
     },
     showLoading () {//loading 图标
	      this.$vux.loading.show({
	        text: 'Loading'
	      })
     },
     closeBtn(){//提交成功页面 点击关闭
     	 this.$router.push('/component/personal');
     },
     menuBtn(){//弹框页面 点击知道了
     	this.maskFlag=false;
     }
     
	},
	mounted(){
		$('.sucDiv').height($('.dish_wrap').height());
		
		this.mySelfDishId=this.$route.query.mySelfDishId;//菜品id
		if(this.mySelfDishId!=0){//推送图文进入到菜品上传页 编辑菜品
			var mySelfDishId=window.location.href.split('?')[1];
		  this.mySelfDishId=parseInt(mySelfDishId.split('=')[1]);
		  this.getMySelfDish();//获取菜品信息
		}
		
		//获取当前url 配置config
		var uri=window.location.href.split('#')[0];
		this.currentUrl=encodeURIComponent(uri);
		this.setConfig();
		
		//获取用户信息
		this.userData=cookie.get('WeiXinUser',function(val){
	    var a = val.split("&");
			var obj = {};
			for (var i = 0; i < a.length; i++) {
				var b = a[i].split("=");
				obj[b[0]] = b[1];
			}
			return obj;
  	}) 
	  this.userId=parseInt(this.userData.UserId);
		
		//获取调料
		this.dataArr=this.$route.query.dataArr;
		for(var i in this.dataArr){
			this.dataArr[i].Unit="";
		}
		
    //删除 主料辅料其他调料
    $('.main').on('click','.remove',function(){//主料
      $(this).parent().remove();
    })

    $('.access').on('click','.remove',function(){//辅料
      $(this).parent().remove();
    })
    
    $('.othersea').on('click','.remove',function(){//其他调料
      $(this).parent().remove();
    })
	
		$('.cookStep').on('click','.remove',function(){//烹饪步骤
	    $(this).parent().remove();
	  })
		
	}
}
</script>

<style scoped>
	.upload{width: 100%;height: 225px;background: #fbfbfb;text-align:center;color: #8e8e8e;line-height: 225px;}
	.upload img{width: 100%;height: 100%;}
	.dishname{width: 90%;height: 60px;margin-left: 5%;font-size: 16px;text-align: center;outline: none;border: none;border-bottom: 1px solid #dddddd;}
	.alike *{display: inline-block;vertical-align: middle;}
	.alike img{width: 20px;}	
	.alike span{color: #e83426;font-size: 15px;font-weight: bold;}
	.show{margin-top: 20px;padding-left: 10px;padding-right: 10px;}	
	.title_show{width: 100%;height: 30px;background: #f9f9f9;color: #E83428;line-height: 30px;margin-top: 15px;text-indent: 10px;}
	.main li,.access li,.season li,.othersea li{width: 100%;height: 45px;box-sizing: border-box;border-bottom: 1px solid #e2e2e2;list-style: none;line-height: 45px;position: relative;}
	.main li *,.access li *,.season li *,.othersea li *{float: left;}
	.maintext,.weight{height: 100%;line-height: 44px;outline: none;border: none;font-size: 15px;}
	.maintext{width: 65%;text-indent: 10px;}
	.weight{width: 20%;text-align: right;}
	.gram{display: inline-block;height: 100%;border-right: 1px solid #e2e2e2;}
	.remove{width: 20px;position: absolute;right: 10px;top: 15px;}
	.add{text-align: right;padding-right: 10px;margin-top: 5px;}		
	.add img{width: 20px;}
	.remarks{margin-left: 10px;margin-top: 50px;font-size: 12px;color:  #E83428;}
	.cookStep li{height: 45px;line-height: 40px;margin-left: 15px;list-style: none;position: relative;}
	.cookStep li span{font-size:14px;color: #3E3E3E;}
	textarea{width: 80%;position: absolute;top: 12px;font-size: 13px;color: #3E3E3E;outline: none;border: none;resize : none;}
	.dishupload .addBtn{width: 90%;height: 40px;background: url('../../static/credit/dishbtn.png') no-repeat center; border: none;outline: none;
	                   margin-left: 5%;margin-top: 60px;border-radius: 5px;color: #fff;margin-bottom: 20px;}
	.telePro{font-size: 12px;color: #E83428;margin-top: 40px;padding:0 20px;letter-spacing: 1px;}
	.teleText{font-size: 12px;color: #3E3E3E;padding-left:20px;margin-top: 20px;}
	.telephone{border: none;border-bottom: 1px solid #ccc;outline: none;}
	.sucDiv{width: 100%;/*height: 100%;*/position: fixed;top: 0px;background: #FFFFFF;text-align: center;letter-spacing: 1px;z-index: 100;}    
  .sucDiv img{ width: 30%;margin-top: 90px;}
  .sucDiv p:nth-child(2){font-size: 17px;color: #E83428;margin-top: 10px;}
  .sucDiv p:nth-child(3){font-size: 13px;color: #707070;margin-top: 10px;}
  .sucDiv button{width: 90%;height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 20%;}
  .alert-mask{ width: 100%; /*height: 100%;*/ background: rgba(0,0,0,0.5); position: fixed; top: 0px;z-index: 100;}
  .menu{width:60%;background: #FFFFFF; border-radius: 10px; margin: 150px auto;border:1px solid #fff;padding: 10px 0;text-align: center;}
  .menu p{ font-size: 13px; color: #3E3E3E;margin-top: 20px;}
  .menu button{ width: 50%; height: 33px; background: #fff; color: #E83428; border: 1px solid #E83428; border-radius: 20px; margin-top: 18px; outline: none; }
</style>
<style>
  #vux_view_box_body{background: #FFFFFF !important;}
  .dishupload .weui-textarea{font-size: 13px !important;text-indent: 1em;}
  .dishupload .vux-no-group-title:before{border-top: none !important;}
  .dishupload .vux-no-group-title:after{border-bottom: none !important;}
  .dishupload .weui-cells{margin-top: 0 !important;}
</style>