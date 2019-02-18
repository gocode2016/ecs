<template>
	<div class="activitiesList" >
	    <ul>
	    		<li v-for="todos in cardList">
	    			<Card class='card' >
			        <p slot="title" class="title" style="text-align: center;color: coral;"  @click='titleClick(todos.ChefActivityId)'>{{todos.ChefActivityName}}</p>
			       	<ul class="cardUl">
			       		<li><span class="lable">状态</span><span class="value" style="margin-left: 2.7rem;" :class="todos.isRed">{{todos.state}}</span></li>
			       		<!--<li><span class="lable">开始时间</span><span class="value">{{todos.starTime}}</span></li>
			       		<li><span class="lable">结束时间</span><span class="value">{{todos.endTime}}</span></li>-->
			       		<li>
			       			<Button v-bind:class="{ showBtn: todos.isShow }" type="primary" size="small" @click='editInformation(todos.ChefActivityId)'>编辑信息</Button>
			       			<Button v-bind:class="{ showBtn: todos.isShow }" type="primary" size="small" @click='configurationTeacher(todos.ChefActivityId)'>配置导师</Button>
			       			<Button style="margin-top: 1rem;" v-bind:class="{ showBtn: todos.isShow }" type="primary" size="small" @click='configurationCook(todos.ChefActivityId)'>配置星厨</Button>
			       			<Button style="margin-top: 1rem;" v-bind:class="{ showBtn: todos.isShow }" type="primary" size="small" @click='SeasoningConfiguration(todos.ChefActivityId)'>配置调料</Button>
			       		</li>
			       	</ul>
			    </Card>
	    		</li>
	    		<li style="padding-bottom: 3rem;">
	    			<Card class='card' style='height:13.5rem;text-align: center;'>
		   			<img style="height: 7rem;margin-top:1rem ;cursor: pointer;" @click='addActivities' src="../../../../assets/img/activites/addActivities.png" />
		   			<p style="text-align: center;color: blue;cursor: pointer;" @click='addActivities'>新增活动</p>
			    </Card>
	    		</li>
	    </ul>
	    <Spin fix class='loding'>
                <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
                <div>Loading</div>
            </Spin>
	    <Modal
		        v-model="isShowAddActive"
		        title="新增活动"
		        @on-ok="addActives"
		        width="400">
		        <ul class="modalUl">
		        		<li>
		        			<span>活动名称:</span>
		        			<Input  placeholder="请输入活动名称" style="width: 200px;margin-left: 1rem;" v-model='activeName'></Input>
		        		</li>
		        		<li style="margin-top: 1rem;">
		        			<span style="">是否显示:</span>
						<Select style="width:200px;margin-left: 1rem;" v-model='selectShow'>
					        <Option v-for="item in showList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
		        		</li>
		        		
		        </ul>
		    </Modal>
    </div>
</template>

<script>
	export default{
		data(){
			return{
				isRed:'',//进行中颜色变化
				isShowAddActive:false,//新增活动弹框
				activeName:'',//存储活动名字
				selectShow:'',//存储选择是否显示
				session:sessionStorage.getItem('loginkey'),
				URL:{
					getCard:URLHeader.ECActivities+'/api/ChefActivity/GetChefActivityAll',//获取卡片(活动)列表
					addActives:URLHeader.ECActivities+'/api/ChefActivity/AddChefActivity',//活动新增
				},
				cardList:[//卡片(活动)数据表
				],
				showList:[//是否显示
					{
	                    value: '1',
	                    label: '是'
               		 },{
	                    value: '0',
	                    label: '否'
               		 }
				],
		}
		
	},
	methods:{
		addActivities(){
			this.isShowAddActive=true;
			//
		},
		requestCard(){
			var self = this;
			self.cardList=[];
			$.ajax({
		        type :"get",
		        url :self.URL.getCard,
		        dataType : 'json',
//		        contentType: "application/json; charset=utf-8",				
		        success : function(json) {
		        	 var dataAll = JSON.parse(json);
		        	 	for (var i =0;i<dataAll.length;i++) {
		        	 			if (dataAll[i].IsEnable==1) {
		        	 				dataAll[i].state = '进行中';
		        	 				dataAll[i].isRed = 'red';
		        	 				dataAll[i].isShow=false;//显示三个按钮
		        	 			} else{
		        	 				dataAll[i].state = '已结束'
		        	 				dataAll[i].isShow=false;
		        	 			}
		        	 			self.cardList.push(dataAll[i])
		        	 	}
		        	 	$('.loding').hide()
		        	  console.log(dataAll)
		        	 
		        },
		        error : function(error) { 
					
		        }
   		    });
			
		},
		addActives(){//新增弹框确认按钮
			var self =this;
			
			if (self.activeName && self.selectShow) {
				let DATA ='{"ChefActivityName":"'+self.activeName+'","CreatePerson":"'+self.session+'","UpdatePerson":"'+self.session+'","IsEnable":"'+self.selectShow+'"}'
				$.ajax({
			        type :"post",
			        url :self.URL.addActives,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			         	// var dataAll = JSON.parse(json);
			         	self.$Notice.success({
		                    title: '添加成功',
		                });
			        		self.requestCard();
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
				
				
			} else{
				self.$Message.warning('数据不能为空');
			}
			
		},
		editInformation(name){//编辑信息
			
			this.$router.push({path: '/editorActivities',query: {ChefActivityId: name}});//跳转到编辑信息页
		},
		configurationTeacher(name){//配置导师.vue
			this.$router.push({path: '/teacherListAll',query: {ChefActivityId: name}});//跳转到导师列表页
		},
		configurationCook(name){//配置星厨
			
			this.$router.push({path: '/StarKitchenList',query: {ChefActivityId: name}});//跳转到编辑信息页
		},
		titleClick(name){//标题头
			console.log(name)
			this.$router.push({path: '/registrationInformation',query: {ChefActivityId: name}});//跳转报名列表页
		},
		SeasoningConfiguration(name){//配置调料   
			this.$router.push({path: '/SeasoningConfiguration',query: {ChefActivityId: name}});//跳转到调料配置页
		}
	},
	mounted:function(){
			this.requestCard();
		}
	}
</script>

<style scoped>
	.showBtn{
		display: none;
	}
.activitiesList ul li {
	display: block;
	float: left;
	margin-left: 1rem;
	margin-top: 1rem;
}
.activitiesList .card{
	width: 12.5rem;
	height: 13.5rem;
}
.activitiesList .card .title{
	cursor: pointer;
}
.activitiesList .cardUl{
	
	overflow: auto;
}
.activitiesList .lable{
	margin-left: 0.6rem;
}
.activitiesList .value{
	margin-left: 0.6rem;
}
.red{
	color: red;
}
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
@keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }
</style>