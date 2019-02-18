<template>
  <div class="contribution">
    
    <p class="dateTitle">
    	<span>{{fullyear}}年{{month}}月</span>
    	<span class="screen" @click="screenClick"><img src="../../static/credit/screenImg.png">筛选</span>
    </p>
    <p class="box box_name">
    	<span>产品</span><span>数量</span><span>单位</span><span>单价</span><span>小计</span>
    </p>
    <p class="box box_content" v-for="(item,index) in contriList" :key="index">
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
import { cookie, Flexbox, FlexboxItem } from 'vux'
import apiUrl from "../apiUrls.js"
export default({
	components: {
		cookie,
		Flexbox, 
		FlexboxItem
	},
	data(){
		return {
			userData:{},
			userId:'',
      datelist:['1月','2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月'],
      maskFlag:false	,
      contriList:[],//列表
      total:0,//总价
      localedate:'',//当前时间
      fullyear:'',//年
      month:'',//月
      selectyear:''//选择年
		}
	},
	methods: {
		getBCList(){//获取业绩贡献列表
    	var _this=this;
    	var params={
    		"SalemanId":this.userId,
    		"MemberId":0,
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
    monthClick(index){//点击月份
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
    },
    selectYear(val){//选择年份
    	if(val=='reduce'){//上一年
    		this.selectyear--;
    	}else if(val=='add'){//下一年
    		this.selectyear++;
    	}
    	$('.mask_month div span').removeClass('month_style');
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
	  this.userId=parseInt(this.userData.UserId);
		this.getDate();//获取当前日期
	}
})
</script>

<style scoped>
.contribution{height: 100%;background: #fff;overflow: hidden;}
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
.box_content{padding:15px 5px;border-bottom: 1px solid #f4f4f4;font-size: 12px;}
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
