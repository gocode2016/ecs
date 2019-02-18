<template>
  <div class="goregister">
    
    <div class="mask">
    	<div>
    		<p>需要注册才能参与哦！</p>
    		<button @click='jump'>马上注册</button>
    	</div>
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
				source:''
			}
		},
		methods:{
			jump(){
				if(this.source=='thanksdraw'){
					this.$router.push({path:'/component/registeredmembers',query:{source:'thanksdraw'}});
				}else{
					this.$router.push('/component/registeredmembers');
				}
			}
		},
		mounted(){
			this.source=this.$route.query.source;
		}
	}
</script>

<style scoped>
  .mask{width:100%;height:100%;position: fixed;top: 0;background: rgba(0,0,0,0.8);z-index: 1000;}
  .mask div{background: #fff;width: 50%;height:105px;position: absolute;top: 180px;left:25%;text-align: center;}
  .mask p{font-size: 12px;color: #E83428;margin-top: 30px;}
  .mask button{width: 50%;height: 25px;background: #E83428;color: #fff;border: none;outline: none;font-size: 12px;margin-top: 20px;}
</style>
<style>

</style>