<template>
  <div class="authenexamine">
    
    <!--成功-->
    <div class="successDiv" v-show="isSuccess==0">
	    <img src="../../static/credit/success.png" class="sucImg"/>
	    <p class="text_a">信息提交成功</p>
	    <p class="text_b">将于{{dateTime}}前告知审核结果</p>
	    <button @click="jump('/component/personal')">确定</button>
    </div>
    
    <!--失败-->
    <div class="failDiv" v-show="isSuccess==1">
    	<img src="../../static/credit/authen_fail.png" class="sucImg"/>
	    <p class="text_a">认证失败</p>
	    <p class="text_b" style="padding: 0 15%;text-align: left;">很抱歉，您的资料没有认证通过，我们会员认证仅针对专业厨师。</p>
	    <button @click="jump('/component/personal')">我的账户</button>
    </div>
    
  </div>
</template>

<script>
//	import { } from 'vux'
	import apiUrl from '../apiUrls.js'
	
	export default{
		components:{
			
		},
		data(){
			return{
				isSuccess:'',//0 成功
				dateTime:''
			}
		},
		methods:{
			jump(link){
				this.$router.push(link);
			},
			getDateTime(){
				var date=new Date();
				var time=date.getTime()+(3*24*60*60*1000);//3天后距离1970.1.1毫秒数
				date.setTime(time);
				this.dateTime=date.getFullYear()+'年'+(date.getMonth()+1)+'月'+date.getDate()+'日';
			}
		},
		mounted(){
			this.getDateTime();
			this.isSuccess=this.$route.query.isSuccess;
		}
	}
</script>

<style scoped>
.authenexamine{height: 100%;background: #fff;text-align: center;font-size: 14px;}
.sucImg{width: 30%;margin-top: 10%;}
.text_a{color: #E83428;margin-top: 2%;letter-spacing: 1px;}
.text_b{color: #6c6c6c;margin-top: 2%;letter-spacing: 1px;}
.authenexamine button{width: 40%;height:30px;background: #e83428;color: #fff;border: none;border-radius: 5px;position: absolute;
               top: 88%;margin-left: -20%;}
    
</style>
<style>

</style>