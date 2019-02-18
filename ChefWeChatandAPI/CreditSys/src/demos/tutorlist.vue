<template>
  <div class="tutorlist">
  	
    <scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2" @on-pullup-loading="load" >
      <div class="box2">
         <panel  :list="list" :type="type"></panel>   
      </div>
    </scroller>
    
  </div>  
</template>
<script>
import apiUrl from "../apiUrls.js"
import {Panel,Scroller,AjaxPlugin} from 'vux'
export default {
  components: {
    Panel,
    Scroller,
    AjaxPlugin
  },
  data () {
    return{
    	count:'',
      type: '6',
      list: [],
      pageIndex:1,
      pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
      }
    }
  },
  mounted(){
  	this.getTutorList();
  },
  methods:{
		load () {
	      setTimeout(() => {
	      	this.pageIndex+=1;
					this.getTutorList();
	        setTimeout(() => {
	          this.$refs.demo2.donePullup()
	        }, 1)
	        if (this.list.length ==this.count) { // unload plugin
	          setTimeout(() => {
	            this.$refs.demo2.disablePullup()
	          }, 100)
	        }
	      }, 5)
	  },
	  getTutorList(){
        var _this =this;
        var params='?chefActivityId='+1+'&pageIndex='+this.pageIndex+'&pageSize=4';
			  this.$http({		  	
			  	method:'GET',		  	
			  	url:apiUrl.getTutorList+params
			  }).then(function(response){
			  	  var data=JSON.parse(response.data);
			  	  var dataList=data.data;
//			  	  console.log(dataList);
			  	  _this.count=data.Count;
			  		for(var i =0;i<dataList.length;i++){
			  			var li = dataList[i];
			  			li.AreaName=li.AreaName.replace('|','  ');
			  			var item = {
			  					src: li.HeadPicUrl,
					        title: li.TutorName,
					        smallTitle: li.TutorPosition,
					        desc:li.TutorHotel,
					        category:'赛区：'+li.AreaName,
					        url: '/component/tutor',
					        tutorId:li.TutorId
			  			}		
			  			if(i%2!=0){
			  				item.class='odd';
			  			}
			  			_this.list.push(item);			  			
			  		}
//			  		console.log(_this.list);
			  })
	  }
  }
}
</script>
<style scoped>
</style>
<style>
	#vux_view_box_body{background: #FFFFFF;}
  .tutorlist .weui-media-box__desc{margin-top: 8px;color: #888888 !important;}
  .tutorlist .weui-media-box__des{color:#3E3E3E !important;margin-top: 5px;}
  .tutorlist .weui-media-box{height: 120px;padding: 15px 20px ;letter-spacing: 1px;}
  .tutorlist .weui-media-box_appmsg .weui-media-box__hd{margin-right:1.5em;position:relative;width: 100px; height: 100px;border: 0.5px solid #E83428;border-radius: 50%;overflow: hidden;}
  .tutorlist .weui-media-box__title{font-weight: 100;color: #3E3E3E;}
  .tutorlist .small_title{color:#e83428;font-size: 13px !important;}
  .tutorlist .weui-media-box_appmsg .weui-media-box__thumb {width: 150px !important;max-height: 207px;margin-top: 10px;margin-left: -25px;}
</style>
