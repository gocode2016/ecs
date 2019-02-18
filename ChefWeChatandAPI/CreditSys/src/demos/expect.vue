<template>
  <div class="expect">
     <p class="text_kf">更多功能正在开发中</p>
     <p class="text_qd">敬请期待...</p>
  </div>
</template>

<script>
//import { } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		
	},
	data(){
		return{
			
		}
	},
	methods:{
		
	},
	mounted(){
		if(this.$route.query.shop==0){
			$('.text_kf').hide();
			$('.text_qd').css('margin-top','140px');
		}
	}
}
</script>

<style scoped>
.expect .text_kf{margin-top: 140px;width: 60%;line-height: 40px;border: 1px solid #c7c7c7;color: #c7c7c7;font-size: 17px;text-align: center;margin-left: 20%;
                      border-radius: 10px;}
.expect .text_qd{font-size: 25px;color: #fd2727;text-shadow: 1px 1px 0 #fed4d5;text-align: center;margin-top: 15px;letter-spacing: 1px;font-weight: lighter;}   
               
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>