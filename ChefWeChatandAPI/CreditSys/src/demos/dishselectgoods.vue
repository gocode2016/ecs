<template>
  <div class="dishselectgoods">
    
    <p class="goodstop">(以下调味品中选择一样作为调料)</p>
    <!--产品展示-->
    <div class="goodsbox">
    	<checker v-model="demo" type="checkbox" default-item-class="demo-item" selected-item-class="demo-item-selected" @on-change="change">
	      <checker-item  v-for="i in goodsList" :key="i.num" :value="i">
	         <img :src="i.FRPicUrl" class="productImg"/>
	         <img src="../../static/credit/dish_select.png" class="selectImg"/>
	         <p>{{i.FRName}}</p>
	      </checker-item>
	    </checker>
    </div>
    <button @click="nextClick" class="nextBtn">下一步</button>
    
    <!--提示菜品超出数量-->
    <div class="alert-mask" v-show='maskFlag'>
      <div class="menu">
         <p>{{protext}}</p>
         <button @click="menuBtn">知道了</button>
      </div>
    </div>
    
  </div>
</template>

<script>
import { Checker, CheckerItem,cookie } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Checker, 
		CheckerItem,
		cookie
	},
	data(){
		return{
			demo: [],
			goodsList:[{
				num:1,
				img:'../../static/credit/prizeimg3.png',
				name:'味达美冰糖老抽1'
			},{
				num:2,
				img:'../../static/credit/prizeimg3.png',
				name:'味达美冰糖老抽2'
			},{
				num:3,
				img:'../../static/credit/prizeimg3.png',
				name:'味达美冰糖老抽3'
			}],
			checkArr:[],
			maskFlag:false,
			protext:'',
			userData:{},
			userId:''
		}
	},
	methods:{
		getMySelfFR(){//获取推荐调料
			var _this=this;
  		this.$http({
  			method:'get',
  			url:apiUrl.getMySelfFR
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			_this.goodsList=JSON.parse(response.data);
  			for(var i in _this.goodsList){
  				var goodsList=_this.goodsList[i];
  				goodsList.num=parseInt(i)+1;
  			}
//			console.log(_this.goodsList);
  		})
		},
		change(value){
//			console.log(value);
			$('.demo-item img').css('opacity','1');
			$('.selectImg').css('display','none');
			this.checkArr=value;
			for(var i in this.checkArr){
				var checkarr=this.checkArr[i];
				$('.demo-item .productImg').eq(checkarr.num-1).css('opacity','0.5');
				$('.selectImg').eq(checkarr.num-1).css('display','block');
			}
		},
		isOverLoad(MySelfFRId){//上传菜品数量限制
			var _this=this;
			var params={
				"MySelfFRId":MySelfFRId,
				"MemberId":this.userId
			}
  		this.$http({
  			method:'post',
  			url:apiUrl.isOverLoad,
  			data:params
  		}).then(function(response){
			  var data=JSON.parse(response.data);
			  if(data.IsOverLoad=='t'){//可以上传菜品
			  	_this.$router.push({path:'/component/dishupload',query:{dataArr:_this.checkArr,mySelfDishId:0}});
			  }else if(data.IsOverLoad=='f'){//超出上传数量 不能上传
			  	_this.maskFlag=true;
			  	_this.protext="本次活动仅能上传一道菜品哦";
			  }
  		})
		},
		menuBtn(){
			this.maskFlag=false;
		},
		nextClick(){//点击下一步
			if(this.checkArr.length==0){//没有选中调料
				this.maskFlag=true;
			  this.protext="请选择调味品";
			}else{
				var MySelfFRId=this.checkArr[0].MySelfFRId;
				this.isOverLoad(MySelfFRId);//判断上传菜品数量是否超出
			}
		}
	},
	mounted(){
		$('.demo-item').height($('.demo-item').width());//设置图片高度
		this.getMySelfFR();//获取推荐调料
		
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
		
	}
}
</script>

<style scoped>
.dishselectgoods{height: 100%;background: #fff;overflow: hidden;}
.goodstop{font-size: 12px;color: #e83428;margin-top: 25px;margin-left: 17px;}
.goodsbox{margin-top: 15px;padding: 0 4%;}
.demo-item {width: 31%;margin-left:2%; margin-top: 12px; text-align: center;position: relative;}
.demo-item .productImg{width: 100%;height: 100%;}
.selectImg{width: 20px;position: absolute;bottom: 5px;left: 66%;display: none;}
.demo-item p{font-size: 12px;}
.demo-item-selected {background:#000;}
.dishselectgoods .nextBtn{width: 90%;margin-left: 5%;height: 40px;background: url('../../static/credit/dishbtn.png') no-repeat left center;
                       background-size:100% 100%;border: none;border-radius: 5px;color: #fff;position: fixed;bottom: 30px;}
.alert-mask{ width: 100%; height: 100%; background: rgba(0,0,0,0.5); position: fixed; top: 0px;}
.menu{width:60%;background: #FFFFFF; border-radius: 10px; margin: 150px auto;border:1px solid #fff;padding: 10px 0;text-align: center;}
.menu p{ font-size: 13px; color: #3E3E3E;margin-top: 20px; }
.menu button{ width: 50%; height: 33px; background: #fff; color: #E83428; border: 1px solid #E83428; border-radius: 20px; margin-top: 18px; outline: none; }
</style>
<style>

</style>