<template>
	<div class="auditView">
		<h1 style="text-align: center;margin-bottom: 1rem;width: 10rem;margin: 0 auto;border: 1px dashed gray;border-radius: 4px;">报名厨师审核</h1>
		<h2 style="border-bottom: 1px solid black;width: 6rem;padding-left: 0.7rem;margin-bottom: 2rem;" v-if='imformationTitle'>基本信息</h2>
		<div class="Information">
			<ul style="width: 30rem;overflow: auto;">
				<li>
					<span class="lable">姓名</span>
					<span class="value" style="margin-left:5rem ;">{{chefInformation.MemberName}}</span>
				</li>
				<li>
					<span class="lable">出生日期</span>
					<span class="value" >{{chefInformation.Birthday}}</span>
				</li>
				<li>
					<span class="lable">赛区</span>
					<span class="value" style="margin-left:5.2rem ;">{{chefInformation.AreaName}}</span>
				</li>
				<li>
					<span class="lable">途径</span>
					<span class="value" style="margin-left:5.2rem ;">{{chefInformation.ApplyType}}</span>
				</li>
				<li>
					<span class="lable">推荐人</span>
					<span class="value" style="margin-left:4.2rem ;">{{chefInformation.ReferrerName}}</span>
				</li>
				<li>
					<span class="lable" style="height: 6rem;line-height: 6rem;display: block;float: left;">个人照片</span>
					<img :src="chefInformation.ChefPicUrl" style="display: block;float: left;height: 6rem;margin-left: 3.5rem;max-width:13rem" />
				</li>
				<li>
					<span class="lable" style="height: 6rem;line-height: 6rem;display: block;float: left;">酒店照片</span>
					<img :src="chefInformation.ChefHotelPicUrl" style="display: block;float: left;height: 6rem;margin-left: 3.5rem;max-width:13rem" />
				</li>
			</ul>
			<Button type="primary" @click='NextPage(1)' style="margin-left: 15rem;margin-top: 1rem;">下一步</Button>
		</div>
			<div class="dish">
				<Modal
			        title="推送消息"
			        v-model="modal"
			        :mask-closable="false"
			        :width='400'
			        @on-ok='confirm'>
			       
			        <Input v-model="errorMessage" type="textarea" :rows="6" placeholder="请输入不合格的原因或修改提示"></Input>
			    </Modal>
				<div class="localDish">
					<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">本地菜</h2>
					<img  :src="dishOne.Basic.DishPicUrl" style="width: 8rem;display: block;margin: 1rem auto;"/>
					<h3 style="text-align: center;">{{dishOne.Basic.DishName}}</h3>
					<h2>菜品故事</h2>
					<p style="width: 25rem;margin: 0 auto;border: 1px dashed gainsboro;border-radius:4px;padding: 0.5rem;">
						{{dishOne.Basic.DishStory}}
					</p>
					<h2>食材明细</h2>
					<h3 style="text-align: center;">主料</h3>
					<ul class="ingredients">
						<li v-for='item in dishOne.mainMaterial'>
							<span class="labie">{{item.Material}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h3 style="text-align: center;">辅料</h3>
					<ul class="ingredients">
						<li v-for='item in dishOne.accessories'>
							<span class="labie">{{item.Material}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h3 style="text-align: center;">调料</h3>
					<ul class="ingredients">
						<li v-for='item in dishOne.seasoning'>
							<span class="labie">{{item.Material}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h3 style="text-align: center;">指定调味品</h3>
					<ul class="ingredients">
						<li v-for='item in dishOne.recommendations'>
							<span class="labie">{{item.FlavourName}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h2>烹饪步骤</h2>
					<ul class="steps">
						<li v-for='(item,index) in dishOne.DishStep'>
							{{index+1}} {{"."}} {{item.StepName}}
						</li>
					</ul>
				</div>
				<div class="InnovativeDishes">
					<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">创新菜</h2>
					<img  :src="dishTwo.Basic.DishPicUrl" style="width: 8rem;display: block;margin: 1rem auto;"/>
					<h3 style="text-align: center;">{{dishTwo.Basic.DishName}}</h3>
					<h2>菜品故事</h2>
					<p style="width: 25rem;margin: 0 auto;border: 1px dashed gainsboro;border-radius:4px;padding: 0.5rem;">
						{{dishTwo.Basic.DishStory}}		
					</p>
					<h2>食材明细</h2>
					<h3 style="text-align: center;">主料</h3>
					<ul class="ingredients">
						<li v-for='item in dishTwo.mainMaterial'>
							<span class="labie">{{item.Material}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h3 style="text-align: center;">辅料</h3>
					<ul class="ingredients">
						<li v-for='item in dishTwo.accessories'>
							<span class="labie">{{item.Material}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
						
					</ul>
					<h3 style="text-align: center;">调料</h3>
					<ul class="ingredients">
						<li v-for='item in dishTwo.seasoning'>
							<span class="labie">{{item.Material}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h3 style="text-align: center;">指定调味品</h3>
					<ul class="ingredients">
						<li v-for='item in dishTwo.recommendations'>
							<span class="labie">{{item.FlavourName}}</span>
							<span class="value">{{item.Unit +'g'}}</span>
						</li>
					</ul>
					<h2>烹饪步骤</h2>
					<ul class="steps">
						<li v-for='(item,index) in dishTwo.DishStep'>
							{{index+1}} {{"."}} {{item.StepName}}
						</li>
					</ul>
		
				</div>
				<Button type="primary" @click='qualified'  style="margin-left: 43rem;margin-top: 2rem;">合格</Button>
				<Button type="primary" @click='unqualified' style="margin-top: 2rem;margin-left: 1rem;">不合格</Button>
			</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				ChefId:'',//厨师id
				OpenId:'',//合格不合格
				imformationTitle:true,//基本信息是否显示
				modal:false,//弹框
				errorMessage:'',//弹框发送错误信息
				chefInformation:{//厨师基本信息
					
				},
				dishOne:{//本地菜
					Basic:{},
					mainMaterial:[],//主料
					accessories:[],//辅料
					seasoning:[],//调料
					recommendations:[],//推荐调料
					DishStep:[],//步骤
				},
				dishTwo:{//创新菜
					Basic:{},
					mainMaterial:[],//主料
					accessories:[],//辅料
					seasoning:[],//调料
					recommendations:[],//推荐调料
					DishStep:[],//步骤
				}
			}
		},
		methods:{
			NextPage(type){//基本信息下一页
				var self =this
				if (type=='1') {//跳转本地菜
					console.log(type)
					$('.Information').hide()
					$('.auditView .dish').show()
					self.imformationTitle=false
					
				} else{
					
				}
			},
			qualified(){//合格
				//this.$router.push({path: '/registrationInformation',query: {ChefActivityId: name}});//跳转报名列表页
				//URLHeader.ECActivities+/api/DishChef/SetApplyDihsChef
				var self =this
				var DATA ='{"ChefId":"'+this.ChefId+'","IsApply":1,"ApplyPerson":"'+sessionStorage.getItem('loginkey')+'","OpenId":"'+this.OpenId+'","Context":""}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/DishChef/SetApplyDihsChef',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						if (json=='OK') {
							self.$Message.success('审核成功');
						} else{
							self.$Message.error('审核失败');
						}
					},
					error:function(error){
						console.log(error)
						self.$Message.error('审核异常');
					}
				});
				this.$Message.success('审核成功');
			},
			unqualified(){//不合格
				this.modal=true
				
			},
			confirm(){//不合格确认
				var self =this
				console.log(this.OpenId)
				//this.OpenId = 'o11Z-ji3vCREpQ7WSdv1YR3I79Jw'//自己的openid				
				var DATA ='{"ChefId":"'+this.ChefId+'","IsApply":2,"ApplyPerson":"'+sessionStorage.getItem('loginkey')+'","OpenId":"'+this.OpenId+'","Context":"'+this.errorMessage+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/DishChef/SetApplyDihsChef',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						console.log(json)
						if (json=='OK') {
							self.$Message.success('信息发送成功');
						} else{
							self.$Message.error('信息发送失败');
						}
					},
					error:function(error){
						console.log(error)
						self.$Message.error('信息发送失败');
					}
				});
				
				
			},
			getChefInformation(){//获取厨师基本信息
				var self =this
				$.ajax({//获取厨师基本信息
			        type :"get",
			        url :URLHeader.ECActivities+"/api/DishChef/GetChefByChefId?chefId="+self.ChefId,
			        dataType : 'json',
			        success : function(json) {
			        	 var dataAll = JSON.parse(json)[0];
			        	 self.OpenId = dataAll.OpenId
			         console.log(dataAll)
			         self.chefInformation = dataAll	
			        },
			        error : function(error) { 
						
			        }
	   		    });
			},
			getChefDish(dishType){
				var self = this
				var dishUrl =URLHeader.ECActivities+"/api/DishChef/GetAuditDishChef?chefId="+self.ChefId+"&dishType="+dishType
				
				$.ajax({//获取厨师菜品
			        type :"get",
			        url :dishUrl,
			        dataType : 'json',
			        success : function(json) {
			        	 	var dataAll = JSON.parse(json).dish;
			        	 	console.log(dataAll)
			        	 	
			        	 	var Basic = dataAll.Basic//基本信息
			        	  	var DishMaterial = dataAll.DishMaterial//食材
			        	  	var DishStep= dataAll.DishStep//步骤
			        	  	
			        	 	var dishInformation={};
			        	 	dishInformation.DishName = Basic.DishName//姓名
			        	 	dishInformation.DishPicUrl = Basic.DishPicUrl//照片
			        	 	dishInformation.DishStory = Basic.DishStory//故事
			        	 	//===========================================食材明细解析===========================================
			        	 	var mainMaterial=[]//主料
			        	 	var accessories = []//辅料
			        	 	var seasoning=[]//调料
			        	 	var recommendations=[]//推荐调料
			        	 	for (var i = 0;i<DishMaterial.length;i++) {
			        	 		if (DishMaterial[i].MaterialType=='1') {//主料
			        	 			mainMaterial.push(DishMaterial[i])
			        	 		}
			        	 		if (DishMaterial[i].MaterialType=='2') {//辅料
			        	 			accessories.push(DishMaterial[i])
			        	 		}
			        	 		if (DishMaterial[i].MaterialType=='3') {//调料
			        	 			seasoning.push(DishMaterial[i])
			        	 		}
//			        	 		if (DishMaterial[i].MaterialType=='4') {//指定调料
//			        	 			recommendations.push(DishMaterial[i])
//			        	 		}
			        	 	}	
			        	 	
			        	 	recommendations = JSON.parse(json).select//指定调料
//			        	 	console.log(recommendations)
			        	 	//===========================================烹饪步骤解析===========================================
			        	 	var Step=[]
			        	 	for (var t=0;t<DishStep.length;t++) {
			        	 		Step.push(DishStep[t])
			        	 	}
			        	 	//console.log(self.dishOne)
			        	 	if (dishType=='1') {//创新菜
			        	 		self.dishTwo.Basic = dishInformation//菜品基本信息
			        	 		self.dishTwo.mainMaterial=mainMaterial//主料
			        	 		self.dishTwo.accessories=accessories//辅料
			        	 		self.dishTwo.seasoning=seasoning//调料
			        	 		self.dishTwo.recommendations=recommendations//指定调料
			        	 		self.dishTwo.DishStep  = Step//步骤
			        	 		
			        	 		
			        	 		console.log(self.dishTwo)
			        	 		
			        	 	} else{//本地菜
			        	 		self.dishOne.Basic = dishInformation//菜品基本信息
			        	 		self.dishOne.mainMaterial=mainMaterial//主料
			        	 		self.dishOne.accessories=accessories//辅料
			        	 		self.dishOne.seasoning=seasoning//调料
			        	 		self.dishOne.recommendations=recommendations//指定调料
			        	 		self.dishOne.DishStep  = Step//步骤
			        	 		//console.log(self.dishTwo)
			        	 	}
			        	  	 
			        },
			        error : function(error) { 
						
			        }
	   		    });
	   	    }
		},
		mounted:function(){
			var self= this
			var hash= window.location.hash.split('?')[1].split('=')[1];//获取厨师id
			this.ChefId = hash
			
			this.getChefInformation()//获取基本信息
			this.getChefDish(1)//创新菜
			this.getChefDish(2)//本地菜
		}
	}
</script>

<style scoped>
.auditView{
	padding: 1rem;
}
.auditView .Information {
	width:32rem ;
	border: 1px dashed gray;
	border-radius: 8px;
	padding: 1rem;
	margin: 0 auto;
}
.auditView .Information ul{
	display: block;
	margin: 0 auto;
}
.auditView .Information ul li {
	overflow: auto;
	display: block;
	width: 20rem;
	margin: 0 auto;
	margin-top: 1rem;
	
	font-size: 1rem;
}
.auditView .Information ul li .labie{
	color: gray;
}
.auditView .Information ul li .value{
	margin-left: 3rem;
	color: black;
}
/**/
.auditView .dish{
	overflow: auto;
	display: none;
	margin-top: 1rem;
}
.auditView .dish .localDish, .auditView .dish .InnovativeDishes{
	float: left;
	width: 29.5rem;
	
}
.auditView .dish .localDish{
	border-right: 1px solid gainsboro;
}
.auditView .dish .ingredients{
	overflow: auto;
	
}
.auditView .dish .InnovativeDishes{
	margin-left: 0.5rem;
	
}
.auditView .dish .ingredients li{
	display: block;
	margin: 0 auto;
	height: 1.5rem;
	width: 15rem;
	line-height: 1.5rem;
	border-bottom: 1px dashed gainsboro;
	margin-bottom: 0.4rem;
}
.auditView .dish .ingredients li .labie{
	display: block;
	float: left;
}
.auditView .dish .ingredients li .value{
	display: block;
	float: right;
}
.auditView .dish .steps{
	margin-top: 1rem;
}
.auditView .dish .steps li{
	margin-top: 0.5rem;
	width: 23rem;
	margin-left: 3rem;
	display: block;
	border-bottom: 1px dashed gainsboro;
	margin-bottom: 0.4rem;
	/*list-style:square*/
}
</style>