<template>
  <div class="chefdetail">
  	
    <div class="chefbox">
    	<p><span>认证方式:</span>实名认证</p>
    	<p><span>通过时间:</span>{{chefinfo.AuthTime}}</p>
    	<p><span>真实姓名:</span>{{chefinfo.MemberName}}</p>
    	<p><span>工作岗位:</span>{{chefinfo.PositionType}}{{chefinfo.Position}}</p>
    	<p><span>酒店区域:</span>{{chefinfo.ProvinceName}}{{chefinfo.CityName}}{{chefinfo.AreaName}}</p>
    	<p><span>酒店名称:</span>{{chefinfo.HotelName}}</p>
    	<p><span>酒店地址:</span>{{chefinfo.HotelAddress}}</p>
    	<p><span>主营类型:</span>{{chefinfo.OperationType}}</p>
    	<p><span>酒店人均消费:</span>{{chefinfo.PerConsumption}}</p>
    	<p><span>现在使用欣和产品:</span>{{chefinfo.IsUseShinho}}</p>
    	<p><span>调味品采购权:</span>{{chefinfo.Purchaser}}</p>
    </div>
    <p class="photo_text">照片认证：</p>
    <div class="photorz">
    	<div><img :src="chefinfo.AuthImg1"/></div>
    	<span>+</span>
    	<div><img :src="chefinfo.AuthImg2"/></div>
    </div>
    <button class="btnDiv" @click="mymemberClick">成为我的</button>
    
    <!--提示弹框-->
    <div class="mask" v-show="maskFlag">
	    <div class="mask_box">
	      <img src="../../static/credit/prizex.png" @click="closeClick"/>
	      <p class="box_text">是否已确认过该会员隶属于您负责的区域范围？</p>
	      <p><button @click="closeClick">否</button><button @click="confirmClick">是</button></p>
	    </div>
    </div>
    
  </div>
</template>

<script>
import { cookie,Loading } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		cookie,
		Loading
	},
	data(){
		return{
			userData:{},
			userId:'',//队员id
			memberId:'',//会员id
			chefinfo:{},//会员信息
			maskFlag:false,//弹框 默认隐藏
		}
	},
	methods:{
		getMemberProfile(){//获取会员信息
			var _this=this;
			var params={
			  'MemberId':this.memberId
			}
			this.$http({
				method:'POST',
				url:apiUrl.getMemberProfile,
				data:params
			}).then(function(response){	
				var data=JSON.parse(response.data);
				console.log(data);
				_this.chefinfo=data[0];
				if(data[0].IsUseShinho==null||data[0].IsUseShinho==''){
					data[0].IsUseShinho='';
				}else if(data[0].IsUseShinho==0){
					data[0].IsUseShinho='否';
				}else if(data[0].IsUseShinho==1){
					data[0].IsUseShinho='是';
				}
			})
		},
		bindMember(){//成为我的 成为该队员名下会员
	  		var _this=this;
	  		var params={
  		    'MemberId':this.memberId.toString(),
					'SalemanId':this.userId.toString()
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.bindMember,
	  			data:params
	  		}).then(function(response){
	  			if(response.data=="Exc Success"){
	  				_this.$vux.loading.hide();//loading图标消失
		  			_this.maskFlag=false;
		  			setTimeout(function(){
		  				_this.$router.push('/component/associatechef');
		  			},500)
	  			}
	  		})
	  },
		mymemberClick(){//成为我的
  		//弹出提示框
  		this.maskFlag=true;
	  },
  	closeClick(){//弹框 点击否或x号
  		this.maskFlag=false;
  	},
  	confirmClick(){//点击 是 调成为我的接口
  		this.showLoading();//loading
	  	this.bindMember();//成为我的 成为该队员名下会员
  	},
  	showLoading () {//loading 图标
      this.$vux.loading.show({
        text: 'Loading'
      })
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
    this.userId=this.userData.UserId;//队员id
    
		this.memberId=this.$route.query.memberId;//会员id
		this.getMemberProfile();//获取会员信息
	}
}
</script>

<style scoped>
.chefbox,.photo_text{padding-left: 10px;font-size: 13px;color: #3E3E3E;}    
.chefbox p{line-height: 40px;border-bottom: 1px solid #f4f4f4;}
.chefbox p span{margin-right: 10px;}
.photo_text{line-height: 40px;}
.photorz{display: flex;padding: 0 5%;}
.photorz div{width: 35%;height: 90px;}
.photorz div img{width:100%;height: 100%;}
.photorz span{font-size: 30px;color: #E83428;margin: 0 12%;line-height: 90px;}
.chefdetail .btnDiv{width: 90%;height: 40px;background: #E83428;color: #fff;border-radius: 5px;border: none;margin: 50px 5% 20px 5%;}
/*弹框*/
.mask{width: 100%;height: 100%;background: rgba(0,0,0,0.5);position: absolute;top: 0;z-index: 1000;}
.mask .mask_box{width: 80%;background: #fff;border-radius: 5px;margin-top: 20%;margin-left: 10%;border: 1px solid #fff;}
.mask_box img{width: 20px;float: right;margin-right: 10px;margin-top: 1%;}
.mask_box p{padding: 0 10%;font-size: 14px;}
.mask_box button{width: 28%;height:24px;border: none;border-radius: 5px;margin-top: 10%;margin-bottom: 20px;}
.mask_box button:nth-child(1){background: #f4f4f4;color: #3E3E3E;}
.mask_box button:nth-child(2){background: #E83428;color: #fff;margin-left: 43%;}
.box_text{margin-top: 10%;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>