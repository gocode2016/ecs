<template>
  <div class="logistics">
      
    <div class="logtop">
      <p><span>物流单号：</span>{{postId}}</p>
      <p><span>承运快递：</span>{{codeName}}</p>
      <p><span>发货时间：</span>{{time}}</p>
    </div>
    <div class="logbox">
    	<timeline>
				<timeline-item v-for="(item,index) in timeLine" :key="index">
					<p class="recent">{{item.context}}</p>
					<p>{{item.time}}</p>
				</timeline-item>
		  </timeline>
    </div>

  </div>
</template>

<script>
import {Timeline, TimelineItem} from 'vux'
import apiUrl from '../apiUrls.js'
export default{
	components:{
		Timeline, 
		TimelineItem
	},
	data(){
		return{
			codeName:'',//快递名称
			code:'',//快递名称 拼音
			postId:'',//快递编号
			time:'',//发货时间
			timeLine:[]
		}
	},
	methods:{
		getLogisticsInfo(){//获取物流信息
			var _this=this;
			var params="?code="+this.code+"&postId="+this.postId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getLogisticsInfo+params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data);
  			_this.timeLine=data.data;
  			for(var i=0;i<_this.timeLine.length;i++){
  				var timeline=_this.timeLine[i];
  				timeline.time=timeline.time.substring(0,19);
  				timeline.time=timeline.time.replace(/-/g,'/');
  				timeline.time=timeline.time.replace('T',' ');
  			}
  			var index=_this.timeLine.length-1;
  			_this.time=_this.timeLine[index].time;
  		})
		}
	},
	mounted(){
		this.codeName=this.$route.query.LogisticsType;
		this.postId=this.$route.query.LogisticsNo;
		if(this.codeName=='中通'){
			this.code='zhongtong';
		}else if(this.codeName=='申通'){
			this.code='shentong';
		}else if(this.codeName=='圆通'){
			this.code='yuantong';
		}else if(this.codeName=='EMS'){
			this.code='ems';
		}else if(this.codeName=='韵达'){
			this.code='yunda';
		}
		this.getLogisticsInfo();//获取物流信息
	}
}
</script>

<style scoped>
  .logtop{width: 100%;font-size: 13px;}
  .logtop p{margin-top: 15px;padding-left: 5%;}
  .logtop span{color: #606060;}
  .logbox{width: 100%;border-top: 5px solid #f4f4f4;margin-top: 15px;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.vux-timeline {padding: 0 0 0 1rem !important;}
	.vux-timeline-item{font-size: 12px;color: #6e6e6e;}
	.vux-timeline-item-content{padding: 18px 0px 10px 2rem !important;}
	.vux-timeline-item-head{top: 20px !important;width: 7px !important; height: 7px !important;left: 2px !important;}
	.vux-timeline-item-color{background: #e2e6e9 !important;}
	.vux-timeline-item-head-first{top: 20px !important;background: #f12f2d !important;}
	.vux-timeline-item-tail{top: 22px !important;background-color:#e2e6e9 !important;width: 1px !important;}
</style>
