<template>
  <div class="dishcookbook">
  	
  	<div class="content">
			<div v-for="(item,index) in dishInfo" :key="index" >
				<p class="dish_classify">{{item.FirstName}}</p>
				<checker v-model="demoCheckbox" type="checkbox" default-item-class="demo-item" selected-item-class="demo-item-selected" @on-change="change">
				  <checker-item :value="items" v-for="(items,index) in item.SecondList" :key="index" style="margin-top: 10px;" >{{ items.SecondName }}</checker-item>
				</checker>
	  	</div>
  	</div>
  	
  	<div>
	  	<p class="total_text">总共（{{dishCount}}道）</p>
	  	<button @click="confirmClick">确定</button>
  	</div>
  	
  </div>
</template>

<script>
import { Checker,CheckerItem} from 'vux'
import apiUrl from '../apiUrls.js'
export default{
	components:{
		Checker,
    CheckerItem
	},
	data(){
		return{
			demoCheckbox:[],
			dishInfo:[],//菜谱分类
			dishCount:0,//菜品数量
			dishTotal:0,//菜品总数量
			secondIdArr:[],
			secondId:''
		}
	},
	methods:{
		change(value){//获取选中的secondId
      var selectArr = value;
      this.secondIdArr = [];
      for(let i in selectArr){
      	var selectarr = selectArr[i];
      	this.secondIdArr.push(selectarr.SecondId);
      }
      if(this.secondIdArr.length != 0){
			  this.secondId = this.secondIdArr.join(',');
      	this.getCaiPinCount();//获取菜品数量
      }else{
				this.dishCount = this.dishTotal;
				$('button').html('确定')
      }
		},
		getAllCategory(){//获取所有分类
			var self = this;
			this.$http({
				method:'get',
				url:apiUrl.getAllCategory
			}).then(function(res){
//				self.dishInfo = JSON.parse(res.data);
				self.dishInfo = res.data;//测试
//				console.log(self.dishInfo);
				for(let i in self.dishInfo){
					self.dishCount += self.dishInfo[i].Total;
				}
				self.dishTotal = self.dishCount
			})
		},
		getCaiPinCount(){//获取菜品数量
			var self = this;
			this.$http({
				method:'get',
				url:apiUrl.getCaiPinCount + '?SecondId=' + this.secondId
			}).then(function(res){
				self.dishCount = JSON.parse(res.data)
				console.log(self.dishCount);
				if(self.dishCount == 0){//没有相关菜品
					$('button').html('没有相关菜谱')
				}else{
					$('button').html('确定')
				}
			})
		},
		confirmClick(){//点击确定
      if(this.secondIdArr.length != 0 && this.dishCount != 0){
        this.$router.push({path:'/component/dishstyle',query:{secondId:this.secondId}});
			}
		}
	},
	mounted(){
		this.getAllCategory();//获取所有分类
	}
}
</script>

<style scoped>
.dishcookbook{display: flex;flex-direction: column;height: 100%;}
.content::-webkit-scrollbar {display:none}/*滚动条不显示*/
.content {overflow: auto;}/*flex内容区*/
.dish_classify{font-size: 1em;color: #e83428;text-align: center;margin-top: 20px;}
.dish_classify:before,.dish_classify:after{content: '——';color:#e83428;width: 25%;}          
.dish_classify:before{margin-right: 10px;} /*调整背景横线的左右距离*/
.dish_classify:after {margin-left: 10px;} 
.total_text{text-align: center;color: #e83428;font-size: 14px;margin: 20px 0;}
.dishcookbook button{font-size:16px;letter-spacing:1px;width: 90%;height: 40px;margin-left: 5%;color: #fff;background: #e83428;border: none;border-radius: 5px;margin-bottom: 20px;}
.demo-item {
  width: 30%;
  line-height: 40px;
  text-align: center;
  background: #f1f1f1;
  border-radius: 5px;
  margin-left: 2.5%;
  font-size: 14px;
}
.demo-item-selected {
  background: #e83428;
  color: #fff;
  font-size: 14px;
}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>