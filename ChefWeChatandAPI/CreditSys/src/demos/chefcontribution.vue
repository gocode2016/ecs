<template>
	<div class="chefcontribution">
		
		<!--用户部分-->
	    <div class="person_top">
	    	 <div class="personphoto"><div class="myphoto"><img :src="userImg"/></div></div>   	 
	    	 <p class="myName">{{chefInfo.MemberName}}</p>
	    	 <grid :rows="4">
			      <grid-item>
			        <p class="grid-center grid-center1">ID</p>
			        <p class="grid-center">{{chefInfo.MemberId}}</p>
			      </grid-item>
			      <grid-item>
			        <p class="grid-center grid-center1">等级</p>
			        <p class="grid-center" v-if='showFlag'>会员已认证</p>
			        <p class="grid-center wrz" v-else>会员未认证</p>
			      </grid-item>
			      <grid-item>
			        <p class="grid-center grid-center1">当前积分</p>
			        <p class="grid-center">{{chefInfo.LeaveIntegral}}</p>
			      </grid-item>
	          <grid-item>
	            <p class="grid-center grid-center1">总积分</p>
	            <p class="grid-center">{{chefInfo.TotalIntegral}}</p>
	          </grid-item>
	      </grid>
	    </div>
	    
	    <!--生意贡献度-->
	    <p class="dateTitle">
	    	<span>{{fullyear}}年{{month}}月</span>
	    	<span class="screen" @click="screenClick"><img src="../../static/credit/screenImg.png">筛选</span>
	    </p>
	    <p class="box box_name">
	    	<span>产品</span><span>数量</span><span>单位</span><span>单价</span><span>小计</span>
	    </p>
	    <p class="box box_content"  v-for="(item,index) in contriList" :key="index">
	    	<span>{{item.GoodsName}}</span>
	    	<span>{{item.total}}</span>
	    	<span>{{item.Unit}}</span>
	    	<span>{{item.Price}}</span>
	    	<span>{{item.zongjia}}元</span>
	    </p>
	    <p class="total">总计：{{total}}元</p>
	    
	    <!--筛选月份-->
	    <div class="mask" v-show="maskFlag">
	      <div class="mask_box">
	      	<p class="mask_year"><img src="../../static/credit/l_jt.png" @click="selectYear('reduce')">{{selectyear}}年<img src="../../static/credit/jt.png"  @click="selectYear('add')"></p>
	      	<div class="mask_month">
	      		<div v-for="(item,index) in datelist" :key="index" @click="monthClick(index)"><span>{{item}}</span></div>
	      	</div>
	      </div>
	    </div>
		
	</div>
</template>

<script>
import { Grid, GridItem ,cookie } from 'vux'
import apiUrl from '../apiUrls.js'
export default({
	components:{
		Grid, 
		GridItem,
		cookie
	},
	data(){
		return{
			showFlag:false,
			userImg:'',//头像
	  		userData:'',
	  		userId:'',//队员 当前用户
	  		memberId:'',//会员
	  		chefInfo:{},//厨师信息
	  		maskFlag:false,
	  		datelist:['1月','2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月'],
	  		contriList:[],//列表
		    total:0,//总价
		    localedate:'',//当前时间
		    fullyear:'',//年
		    month:'',//月
		    selectyear:''//选择年份
		}
	},
	methods:{
		getMemberById(){//获取厨师信息
	  		var _this=this;
	  		var params={
	  		  'MemberId':this.memberId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getMemberById,
	  			data:params
	  		}).then(function(response){  	
	  			var data=JSON.parse(response.data); 
	//			console.log(data);
				if(data.length!=0){
				  _this.chefInfo=data[0];
		  		  if(_this.chefInfo.RecommendId==0){//0未认证  
		  			_this.showFlag=false;
		  		  }else if(_this.chefInfo.RecommendId>0){//>0已认证
		  			_this.showFlag=true;
		  		  }
		  		  _this.chefInfo.ImgUrl==''||_this.chefInfo.ImgUrl==null ? _this.userImg='../../static/credit/perimg.png' : _this.userImg=_this.chefInfo.ImgUrl;
				}else{
				  _this.userImg='../../static/credit/perimg.png';
				}
	  		})
	  	},
	  	getBCList(){//获取业绩贡献列表
	    	var _this=this;
	    	var params={
	    		"SalemanId":this.userId,
	    		"MemberId":this.memberId,
	    		"Day":this.localedate
	    	}
	    	this.$http({
	    		method:'post',
	    		url:apiUrl.getBCList,
	    		data:params
	    	}).then(function(res){
		        var data=JSON.parse(res.data);
		        _this.contriList=data;
		        _this.total=0;
		        for(let i=0;i<_this.contriList.length;i++){
		        	let contriList=_this.contriList[i];
		        	_this.total+=contriList.zongjia;
		        }
	    	})
	    },
	    getDate(){//获取当前日期
	    	var date=new Date();
	    	this.localedate=date.toLocaleDateString();
	    	this.localedate=this.localedate.replace(/\//g,'-');
	    	var arr=this.localedate.split('-');
	    	this.fullyear=arr[0];
    	    this.selectyear=arr[0];
	    	this.month=arr[1];
	    	this.getBCList();//获取业绩贡献列表
    		$('.mask_month div span').eq(this.month-1).addClass('month_style');
	    },
	  	screenClick(){//点击筛选
	    	this.maskFlag=true;
	    },
	    selectYear(val){//选择年份
	    	if(val=='reduce'){//上一年
	    		this.selectyear--;
	    	}else if(val=='add'){//下一年
	    		this.selectyear++;
	    	}
	    	$('.mask_month div span').removeClass('month_style');
	    },
	    monthClick(index){
            var _this=this;
	    	$('.mask_month div span').removeClass('month_style');
	    	$('.mask_month div span').eq(index).addClass('month_style');
	    	setTimeout(function(){
	    		_this.maskFlag=false;
	    	},100)
	    	var arr=this.localedate.split('-');
    	    arr.splice(0,1,this.selectyear);
	    	arr.splice(1,1,index+1);
	    	this.fullyear=arr[0];
	    	this.month=arr[1];
	    	this.localedate=arr.join('-');
	    	this.getBCList();//获取业绩贡献列表
	    }
	},
	mounted(){
	  this.memberId=this.$route.query.userId;//会员id
	  this.getMemberById();//会员信息
	  this.userData=cookie.get('WeiXinUser',function(val){
  		  var a = val.split("&");
		  var obj = {};
		  for (var i = 0; i < a.length; i++) {
			var b = a[i].split("=");
			obj[b[0]] = b[1];
		  }
		  return obj;
  	  }) 
	  this.userId=parseInt(this.userData.UserId);//队员id
	  this.getDate();//获取当前日期
	}
})
</script>

<style scoped>
.person_top{width: 100%;height: 200px;background: url('../../static/credit/perback.png') no-repeat;background-size: 100% 100%;border-bottom: 3px solid #F8F8F8;}
.personphoto{width: 100%;height: 90px;padding-top: 20px;position: relative;}
.myphoto{box-shadow:0px 1px 5px 0px #ddd;width:80px;height:80px;background: #FFFFFF;border-radius: 50%;border: 3px solid #fff;position:absolute;left: 50%;margin-left: -45px;overflow: hidden;}	        
.myphoto img{width:100%;}
.myName{text-align: center;}
.grid-center{text-align: center;font-size: 12px;color: #3E3E3E;}
.grid-center1{color: #E83428;}
.wrz:after{content: " "; display: inline-block; height: 6px; width: 6px;border-width: 1.5px 1.5px 0 0;border-color: #E83428;border-style: solid;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);}
.chefcontribution{height: 100%;background: #fff;overflow: hidden;}
.dateTitle{text-align: center;color: #ea3426;font-size: 15px;margin-top: 18px;}
.screen{color: #3e3e3e;position: absolute;right: 20px;}
.screen *{display: inline-block;vertical-align: middle;}
.screen img{width: 15px;margin-right: 10px;}
.box span{display: inline-block;color: #3E3E3E;text-align: center;}
.box span:nth-child(1){width: 40%;}
.box span:nth-child(2){width: 14%;}
.box span:nth-child(3){width: 14%;}
.box span:nth-child(4){width: 14%;}
.box span:nth-child(5){width: 14%;}
.box_name{font-size: 14px;margin-top: 20px;border-bottom: 2px solid #f4f4f4;}
.box_name span:nth-child(2){margin-left: 1%;}
.box_name span:nth-child(5){margin-left: 3%;}
.box_content{font-size: 12px;padding:15px 5px;border-bottom: 1px solid #f4f4f4;}
.box_content span:nth-child(1){text-align: left;overflow: hidden;text-overflow: ellipsis;white-space:nowrap;position: relative; top: 5px;}
.box_content span:nth-child(5){text-align: right;}
.total{font-size: 14px;color: #3E3E3E;margin-left: 5px;margin-top: 40px;}
.mask{width: 100%;height: 100%;background: rgba(0,0,0,0.5);position: fixed;top: 0;}
.mask .mask_box{width: 94%;background: #fff;margin-top: 70px;padding: 0 3%;}
.mask_year{font-size: 15px;color: #e83727;border-bottom: 1px solid #e83727;text-align: center;line-height: 50px;}
.mask_month{width:100%;display: flex;justify-content: space-between;flex-wrap: wrap;font-size: 12px;}
.mask_month div{width: 25%;text-align: center;padding: 10px 0;line-height: 30px;}
.month_style{display:inline-block;width: 30px;line-height: 30px;background: #ff0100;color: #fff;border-radius: 50%;}
.mask_year img:nth-child(1),.mask_year img:nth-child(2){width: 20px;position: relative;left: -27%;top: 2px;}
.mask_year img:nth-child(2){left: 27%;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.chefcontribution .weui-grid{padding: 13px 13px !important;}
.chefcontribution .weui-grids:before{border-top: 0 !important;}
.chefcontribution .weui-grid:before{border-right: 0 !important;}
.chefcontribution .weui-grid:after{border-bottom: 0 !important;}
</style>