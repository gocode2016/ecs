<template>
	<div class="CheckDetails">
		<div class="BasicInformation">
			<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">基本信息</h2>
			<div style="overflow: auto;">
				<ul class="informationUl">
					<li>
						<span class="labie">姓名</span>
						<span class="value">{{information.MemberName}}</span>
					</li>
					
					<li>
						<span class="labie">出生日期</span>
						<span class="value">{{information.Birthday}}</span>
					</li>
					
					<li>
						<span class="labie">赛区</span>
						<span class="value">{{information.AreaName}}</span>
					</li>
					
				</ul>
				<ul class="informationUl">
					<li>
						<span class="labie">途径</span>
						<span class="value">{{information.ApplyType}}</span>
					</li>
					<li class="referrer">
						<span class="labie" >推荐人</span>
						<span class="value" style="width: 2rem;overflow: hidden;">{{information.ReferrerName}}</span>
					</li>
					<li >
						<span class="labie">酒店</span>
						<span class="value">{{information.ChefHotelName}}</span>
					</li>
				</ul>
				<div class="right">
					<img :src="information.ChefPicUrl" style="display: block;width: 8rem;float: left;"/>
					<p style="text-align: center;">个人照片</p>
				</div>
				<div class="right">
					<img :src="information.ChefHotelPicUrl" style="display: block;width: 8rem;float: left;"/>
					<p style="text-align: center;">酒店照片</p>
				</div>
				
			</div>
		</div>
		<div class="dish">
			<div class="localDish">
				<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">本地菜</h2>
				<img  :src="dishOne.Basic.DishPicUrl" style="width: 8rem;display: block;margin: 1rem auto;"/>
				<h3 style="text-align: center;">{{dishOne.Basic.DishName}}</h3>
				<h2>菜品故事</h2>
				<p style="width: 25rem;margin: 0 auto;border: 1px dashed gainsboro;border-radius:4px;padding: 0.5rem;">{{dishOne.Basic.DishStory}}</p>
				<h2>食材明细</h2>
				<h3 style="text-align: center;">主料</h3>
				<ul class="ingredients" >
					<li v-for="item in dishOne.mainMaterial">
						<span class="labie">{{item.Material}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h3 style="text-align: center;">辅料</h3>
				<ul class="ingredients">
					<li v-for="item in dishOne.accessories">
						<span class="labie">{{item.Material}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h3 style="text-align: center;">调料</h3>
				<ul class="ingredients">
					<li v-for="item in dishOne.seasoning">
						<span class="labie">{{item.Material}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>	
				</ul>
				<h3 style="text-align: center;">指定调味品</h3>
				<ul class="ingredients" style="margin: 1rem 0;">
					<li v-for="item in dishOne.recommendations">
						<span class="labie">{{item.FlavourName}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h2>烹饪步骤</h2>
				<ul class="steps">
					<li v-for="(item,index) in dishOne.DishStep">
						{{index+1}} {{item.StepName}}
					</li>
					
				</ul>
			</div>
			<div class="InnovativeDishes">
				<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">创新菜</h2>
				<img  :src="dishTwo.Basic.DishPicUrl" style="width: 8rem;display: block;margin: 1rem auto;"/>
				<h3 style="text-align: center;">{{dishTwo.Basic.DishName}}</h3>
				<h2>菜品故事</h2>
				<p style="width: 25rem;margin: 0 auto;border: 1px dashed gainsboro;border-radius:4px;padding: 0.5rem;">{{dishTwo.Basic.DishStory}}</p>
				<h2>食材明细</h2>
				<h3 style="text-align: center;">主料</h3>
				<ul class="ingredients">
					<li v-for="item in dishTwo.mainMaterial">
						<span class="labie">{{item.Material}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h3 style="text-align: center;">辅料</h3>
				<ul class="ingredients">
					<li v-for="item in dishTwo.accessories">
						<span class="labie">{{item.Material}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h3 style="text-align: center;">调料</h3>
				<ul class="ingredients">
					<li v-for="item in dishTwo.seasoning">
						<span class="labie">{{item.Material}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h3 style="text-align: center;">指定调味品</h3>
				<ul class="ingredients">
					<li v-for="item in dishTwo.recommendations">
						<span class="labie">{{item.FlavourName}}</span>
						<span class="value">{{item.Unit}}</span>
					</li>
				</ul>
				<h2>烹饪步骤</h2>
				<ul class="steps">
					<li v-for="(item,index) in dishTwo.DishStep">
						{{index+1}} {{item.StepName}}
					</li>
				</ul>
	
			</div>
		</div>
		<Button type="primary" @click='redact' style="margin-left: 55rem;margin-top: 2rem;">编辑</Button>
	
	</div>
</template>

<script>
	export default{
		data(){
			return{
				ChefId:'',//厨师id
				information:{//基本信息
					MemberName:'',//姓名
					Birthday:'',//出生日期
					ReferrerName:'',//推荐人
					ApplyType:'',//途径
					ChefPicUrl:'../../../../../static/image/HeadPortrait.png',//厨师头像
					ChefHotelPicUrl:'../../../../../static/image/HeadPortrait.png',//酒店图片
					
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
		watch: {  
        		'information.ApplyType': 'ApplyTypeChange', //监听途径值 
   	 	},
		methods:{
			ApplyTypeChange(){//监听途径   当自我推荐时  推荐人隐藏
				if (this.information.ApplyType =='自我推荐') {
					//console.log('自我推荐')
					$(' ul .referrer').hide()
				} else{
					//console.log('其它')
					$(' ul .referrer').show()
				}
			},
			redact(){//编辑
				//console.log(123)
				this.$router.push({path: '/enrolEdit',query: {ChefId: this.ChefId}});//跳转到编辑信息页
			},
			getChefInformation(){//获取厨师基本信息
				//URLHeader.ECActivities+/api/DishChef/GetChefByChefId?chefId=1
				var self =this
				$.ajax({//获取厨师基本信息
			        type :"get",
			        url :URLHeader.ECActivities+"/api/DishChef/GetChefByChefId?chefId="+self.ChefId,
			        dataType : 'json',
			        success : function(json) {
			        	 var dataAll = JSON.parse(json)[0];
			        	 
			        	 self.information = dataAll
			        	 console.log(dataAll)
			        },
			        error : function(error) { 
						
			        }
	   		    });
			},
			getChefDish(dishType){
				//URLHeader.ECActivities+/api/DishChef/GetAuditDishChef?chefId=1&dishType=1
				var self =this
				var DATA =URLHeader.ECActivities+"/api/DishChef/GetAuditDishChef?chefId="+self.ChefId+"&dishType="+dishType
				//console.log(DATA)
				$.ajax({//获取厨师菜品
			        type :"get",
			        url :DATA,
			        dataType : 'json',
			        success : function(json) {
			        	 	var dataAll = JSON.parse(json).dish;
			        	  	var Basic = dataAll.Basic//基本信息
			        	  	var DishMaterial = dataAll.DishMaterial//食材
			        	  	var DishStep= dataAll.DishStep//步骤
			        	  	
//			        	 	console.log(JSON.parse(json))
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
			        	 		
			        	 		console.log(self.dishTwo.recommendations)
			        	 		//console.log(self.dishOne)
			        	 		
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
			//
			var hash= window.location.hash.split('?')[1].split('=')[1];//获取厨师id
			this.ChefId = hash
			this.getChefInformation()//获取厨师基本信息
			this.getChefDish(1)//获取厨师菜品1创新菜
			this.getChefDish(2)//获取厨师菜品2本地菜
		}
	}
</script>

<style scoped>
.CheckDetails{
	border: 1px solid gainsboro;
	border-radius: 8px;
	padding: 2rem;
	padding-top: 1rem;
}
.CheckDetails .BasicInformation{
	border-bottom: 1px dashed gainsboro;
}
.CheckDetails .BasicInformation .informationUl{
	display: block;
	float: left;
	width:18rem;
	/*margin-top: 1rem;
	overflow: auto;
	width: 30rem;
	float: left;
	margin-left: 2rem;*/
}
.CheckDetails .BasicInformation ul li {
	display: block;
	
	height: 3rem;
	line-height: 3rem;
	margin-left: 5rem;
	float: left;
}
.CheckDetails .BasicInformation ul li span{
	font-size: 0.9rem;
	color: grey;
}
.CheckDetails .BasicInformation ul li .value{
	margin-left: 1rem;
	color: black;
}
.CheckDetails .BasicInformation .right{
	float: left;
	
	margin-left: 3rem;
}
/**/
.CheckDetails .dish{
	overflow: auto;
}
.CheckDetails .dish .localDish, .CheckDetails .dish .InnovativeDishes{
	float: left;
	width: 29.5rem;
	
}
.CheckDetails .dish .localDish{
	border-right: 1px solid gainsboro;
}
.CheckDetails .dish .ingredients{
	overflow: auto;
	
}
.CheckDetails .dish .InnovativeDishes{
	margin-left: 0.5rem;
	
}
.CheckDetails .dish .ingredients li{
	display: block;
	margin: 0 auto;
	height: 1.5rem;
	width: 15rem;
	line-height: 1.5rem;
	border-bottom: 1px dashed gainsboro;
	margin-bottom: 0.4rem;
}
.CheckDetails .dish .ingredients li .labie{
	display: block;
	float: left;
}
.CheckDetails .dish .ingredients li .value{
	display: block;
	float: right;
}
.CheckDetails .dish .steps{
	margin-top: 1rem;
}
.CheckDetails .dish .steps li{
	margin-top: 0.5rem;
	width: 25rem;
	margin-left: 2rem;
	display: block;
	border-bottom: 1px dashed gainsboro;
	margin-bottom: 0.4rem;
	/*list-style:square*/
}
</style>