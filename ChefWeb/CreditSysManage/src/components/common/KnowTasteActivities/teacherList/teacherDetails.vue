<template>
	<div class="teacherDetails">
		<Tabs value="name1">
	        <Tab-pane label="导师信息" name="name1">
	        		<div class="name1" style="padding-top: 2rem;">
	        			<div class="left">
	        				<h2 style="margin-left: 1rem;margin-bottom: 2rem;">基本信息</h2>
	        				<img :src="PicUrl" style="max-width: 18rem;max-height: 10rem;display: block;margin: 0 auto;"/>
	        				<h3 style="text-align: center;">详情页大图</h3>
						<p style="border-bottom: 1px dashed darkgray;margin: 0.5rem;"></p>
						<img  :src="HeadPicUrl" style="display: block;float: left;margin-top: 1rem;max-width: 15rem;max-height: 10rem"/>
	        				<ul style="float: left;margin-left: 1rem;">
	        					<li><span class="lable">姓名:</span><span>{{TutorName}}</span></li>
	        					<li><span class="lable">头衔:</span><span>{{TutorPosition}}</span></li>
	        					<li><span class="lable">酒店:</span><span>{{TutorHotel}}</span></li>
	        					<li><span class="lable">绑定活动:</span><span>知味活动2017</span></li>
	        					<li><span class="lable">指导赛区:</span><span>{{AreaName}}</span></li>
	        					<li><span class="lable">活动页显示:</span><span>{{IsDisplay}}</span></li>
	        					<li><span class="lable">活动页排序:</span><span>{{PriorityId}}</span></li>
	        					
	        				</ul>
	        			</div>
	        			<div class="right" style="border-left: 1px solid gray;min-height:30rem;padding-left: 1rem;">
	        				<h2 style="margin-left: 2rem;margin-bottom: 2rem;">导师介绍</h2>
	        				<div class="Introduce1">
	        					<!--{{Introduce}}-->
	        				</div> 
	        			</div>
	        		</div>
	        </Tab-pane>
	        <Tab-pane label="菜品1" name="name2" :disabled='dishOneCar'>
	        		<div class="name2">
	        			<div class="left">
	        				<h2>菜品故事</h2>
	        				<div class="Introduce2"></div>
	        			</div>
	        			<div class="right">
	        				<img :src="name2Image" style="max-height: 10rem;max-width: 15rem;display: block;margin: 0 auto;"/>
	        				<p style="text-align: center;font-weight: bold;padding: 1rem;font-size: 1rem;">菜名:{{dishName1}}</p>
	        				<div style="min-width: 30rem;overflow: auto;">
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 14%;">
			        				<h2 style="margin: 0.5rem ;">主料</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='item in Main_ingredientList1' style="overflow: auto;">
			        						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
			        						<span style="display: block;float: right;">{{item.Unit}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 1rem;">
			        				<h2 style="margin: 0.5rem ;">辅料</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;" >
			        					<li v-for='item in Accessories_List1' style="overflow: auto;">
			        						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
			        						<span style="display: block;float: right;">{{item.Unit}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        			</div>
		        			<div style="min-width: 30rem;margin-top: 1rem;overflow: auto;">
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 14%;">
			        				<h2 style="margin: 0.5rem ;">调料</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='item in seasoningList1' style="overflow: auto;">
			        						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
			        						<span style="display: block;float: right;">{{item.Unit}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 1rem;">
			        				<h2 style="margin: 0.5rem ;">烹饪步骤</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='(item,index) in step1'>
			        						<span style="margin-left: 0.5rem;">{{index+1}}</span>
			        						<span style="margin-left: 0.5rem;">{{item.StepName}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        			</div>
		        			<div>
		        				<h3 style="text-align: center;margin: 0.5rem auto;border-bottom: 1px dotted gainsboro;">导师推荐</h3>
		        				<ul style="text-align: center;">
		        					<li v-for='item in Select1' style="display: block;float: left;margin-left: 1rem;">
		        						<Card dis-hover style='width: 9rem; '>
						                <p slot="title" style="text-align: center;font-size: 0.5rem;">{{item.FlavourName}}</p>
						                <img :src="item.FlavourPicUrl" style="max-width: 5rem;max-height: 10rem;display: block;margin: 0 auto;"/>
						            </Card>
		        					</li>
		        				</ul>
		        			</div>
	        			</div>
	        		</div>
	        </Tab-pane>
	        <Tab-pane label="菜品2" name="name3" :disabled='dishTwoCar'>
	        		<div class="name3">
	        			<div class="left">
	        				<h2>菜品故事</h2>
	        				<div class="Introduce2"></div>
	        			</div>
	        			<div class="right">
	        				<img :src="name3Image" style="max-height: 10rem;max-width: 15rem;display: block;margin: 0 auto;"/>
	        				<p style="text-align: center;font-weight: bold;padding: 1rem;font-size: 1rem;">菜名:{{dishName2}}</p>
	        				<div style="min-width: 30rem;overflow: auto;">
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 14%;">
			        				<h2 style="margin: 0.5rem ;">主料</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='item in Main_ingredientList2' style="overflow: auto;">
			        						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
			        						<span style="display: block;float: right;">{{item.Unit}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 1rem;">
			        				<h2 style="margin: 0.5rem ;">辅料</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='item in Accessories_List2' style="overflow: auto;">
			        						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
			        						<span style="display: block;float: right;">{{item.Unit}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        			</div>
		        			<div style="min-width: 30rem;margin-top: 1rem;overflow: auto;">
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 14%;">
			        				<h2 style="margin: 0.5rem ;">调料</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='item in seasoningList2 ' style="overflow: auto;">
			        						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
			        						<span style="display: block;float: right;">{{item.Unit}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        				<div style="border: 1px dashed gainsboro;overflow: auto;width: 12rem;border-radius:4px;float: left;margin-left: 1rem;">
			        				<h2 style="margin: 0.5rem ;">烹饪步骤</h2>
			        				<ul style="width: 10rem;padding-bottom: 1rem;">
			        					<li v-for='(item,index) in step2'>
			        						<span style="margin-left: 0.5rem;">{{index+1}}</span>
			        						<span style="margin-left: 0.5rem;">{{item.StepName}}</span>
			        					</li>
			        				</ul>
		        				</div>
		        			</div>
		        			<div>
		        				<h3 style="text-align: center;margin: 0.5rem auto;border-bottom: 1px dotted gainsboro;">导师推荐</h3>
		        				<ul style="text-align: center;">
		        					<li v-for='item in Select2' style="display: block;float: left;margin-left: 1rem;">
		        						<Card dis-hover style='width: 9rem; '>
						                <p slot="title" style="text-align: center;font-size: 0.5rem;">{{item.FlavourName}}</p>
						                <img :src="item.FlavourPicUrl" style="max-width: 5rem;max-height: 10rem;display: block;margin: 0 auto;"/>
						            </Card>
		        					</li>
		        				</ul>
		        			</div>
	        			</div>	        			
	        		</div>
	        </Tab-pane>
	    </Tabs>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				dishOneCar:false,//菜品一标题
				dishTwoCar:false,//菜品二标题
				Tutorid:'',//导师id
				ChefActivityId:'',//活动id
				//==========================================name1==========================================
				PicUrl:'../../../../../static/image/placeholderBag.png',//大图地址
				HeadPicUrl:'../../../../../static/image/HeadPortrait.png',//头像地址
				TutorName:'',//姓名
				TutorPosition:'',//岗位
				TutorHotel:'',//导师酒店
				AreaName:'',//指导赛区
				IsDisplay:'',//活动页是否显示
				PriorityId:'',//活动页排序
				Introduce:'',//导师故事
				//==========================================name2==========================================
				dishName1:'',//菜名
				name2Image:'../../../../../static/image/placeholderBag.png',//菜品照片
				Main_ingredientList1:[//主料
				],
				Accessories_List1:[//辅料

				],
				seasoningList1:[//调料

				],
				step1:[//步骤
				],
				Select1:[//推荐配料
				],
				//==========================================name3==========================================
				dishName2:'',//菜名
				name3Image:'../../../../../static/image/placeholderBag.png',//照片
				Main_ingredientList2:[//主料
				],
				Accessories_List2:[//辅料
				],
				seasoningList2:[//调料
				],
				step2:[//步骤
				],	
				Select2:[//推荐配料
				],
			}
		},
		methods:{
			insfomationLoad(){//获取导师基本信息
				var self =this;
				$.ajax({
			        type :"get",
			        url :URLHeader.ECActivities+'/api/Tutor/GetTutorByTutorId?tutorId='+self.Tutorid,
			        dataType : 'json',
			        success : function(json) {
			        		var dataAll =JSON.parse(json)
						self.PicUrl = dataAll.PicUrl//大图
						self.TutorName =dataAll.TutorName//姓名
						self.TutorPosition =dataAll.TutorPosition//头衔
						self.TutorHotel=dataAll.TutorHotel//倒是酒店
						self.AreaName=dataAll.AreaName//指导赛区
						if (dataAll.IsDisplay==0) {//是否显示当前导师
							self.IsDisplay='否'
						} else{
							self.IsDisplay='是'
						}
						self.PriorityId =dataAll.PriorityId//活动页排序
						self.HeadPicUrl=dataAll.HeadPicUrl//导师照片
						  var dom= $.parseHTML( dataAll.Introduce,true)//富文本
						  $('.Introduce1').append(dom)
			        },
			        error : function(error) { 
			        	
			        }
	   		    });

			},
			getDishInformation(DishType){//获取菜品
	         	var self= this;
	         	$.ajax({
			        type :"get",
			        url :URLHeader.ECActivities+'/api/DishTutor/GetToutDishSingle?caId='+self.ChefActivityId+'&dishType='+DishType+'&tutorId='+self.Tutorid,
			        dataType : 'json',
			        success : function(json) {
			        		//console.log(json)
			        		if (json!='No') {
			        			var dataAll =JSON.parse(json).dish;
				        		if (DishType=='1') {//菜品一
				        			self.dishOneParsing(dataAll);
				        		} else{//菜品二
				        			self.dishTwoParsing(dataAll)
				        		}
			        		}else{
			        			if (DishType=='1') {//菜品一
				        			self.dishOneCar = true//菜品一 标题禁止点
				        		} else{//菜品二
				        			self.dishTwoCar = true//菜品二 标题禁止点
				        		}
			        		}
			        			
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });	
	         },
	         dishOneParsing(data){
	         	console.log(data)
	         	var self =this;
	         	var Basic = data.Basic;
	         	self.dishName1 = Basic.DishName//菜品名称
	         	self.name2Image = Basic.DishPicUrl  //菜品照片
	         	var dom= $.parseHTML(Basic.DishStory,true)//菜品故事
				$('.Introduce2').append(dom)
	         	
	         	//食材
	         	var DishMaterial = data.DishMaterial 
	         	
	         	self.Main_ingredientList1=[];
	         	self.Accessories_List1=[];
	         	self.seasoningList1=[];
	         	for (var i =0;i<DishMaterial.length;i++) {
	         		DishMaterial[i].Unit = parseInt(DishMaterial[i].Unit);
	         		if (DishMaterial[i].MaterialType=='1') {//主料
	         			self.Main_ingredientList1.push(DishMaterial[i])
	         		}
	         		if (DishMaterial[i].MaterialType=='2') {//辅料
	         			self.Accessories_List1.push(DishMaterial[i])
	         		}
	         		if (DishMaterial[i].MaterialType=='3') {//调料
	         			self.seasoningList1.push(DishMaterial[i])
	         		}
	         	}
	         	//步骤
	         	self.step1 =[]
	         	var DishStep = data.DishStep
	         	
	         	self.step1 = DishStep;
	         	//推荐配料
	         	//FlavourRecId
	         	self.Select1 = data.Select
	         },
	         dishTwoParsing(data){
	         	console.log(data)
	         	var self =this;
	         	var Basic = data.Basic;
	         	self.dishName2 = Basic.DishName//菜品名称
	         	self.name3Image = Basic.DishPicUrl  //菜品照片
	         	var dom= $.parseHTML(Basic.DishStory,true)//菜品故事
				$('.Introduce3').append(dom)
	         	
	         	//食材
	         	var DishMaterial = data.DishMaterial 
	         	
	         	self.Main_ingredientList2=[];
	         	self.Accessories_List2=[];
	         	self.seasoningList2=[];
	         	for (var i =0;i<DishMaterial.length;i++) {
	         		DishMaterial[i].Unit = parseInt(DishMaterial[i].Unit);
	         		if (DishMaterial[i].MaterialType=='1') {//主料
	         			self.Main_ingredientList2.push(DishMaterial[i])
	         		}
	         		if (DishMaterial[i].MaterialType=='2') {//辅料
	         			self.Accessories_List2.push(DishMaterial[i])
	         		}
	         		if (DishMaterial[i].MaterialType=='3') {//调料
	         			self.seasoningList2.push(DishMaterial[i])
	         		}
	         	}
	         	//步骤
	         	self.step2 =[]
	         	var DishStep = data.DishStep
	         	
	         	self.step2 = DishStep;
	         	//推荐配料
	         	//FlavourRecId
	         	self.Select2 = data.Select
	         },
		},
		mounted:function(){
			var self =this;
			get_URL()//获取链接信息
			this.insfomationLoad()//导师基本信息
			
			function get_URL(){//获取地址参数
    				var hash= window.location.hash.split('?')[1].split('&');
				self.Tutorid = hash[0].split('=')[1]//导师id
				self.ChefActivityId = hash[1].split('=')[1]//活动id
//				console.log(self.Tutorid+'&'+self.ChefActivityId)
    			}
			this.getDishInformation(1)//获取菜
			this.getDishInformation(2)//获取菜
		}
	}
</script>
<style>
.teacherDetails .name1 .right  img{
	max-height: 20rem;
	max-width: 20rem;
	display: block;
	margin: 0 auto;
}
 .teacherDetails .name2 .left img{
 	max-height: 20rem;
	max-width: 20rem;
	display: block;
	margin: 0 auto;
 }
  .teacherDetails .name3 .left img{
 	max-height: 20rem;
	max-width: 20rem;
	display: block;
	margin: 0 auto;
 }
</style>
<style scoped>
.teacherDetails .name1,.teacherDetails .name2{
		width: 100%;
		overflow: auto;
		margin-left: 1rem;
	}
.teacherDetails .name1 .left,	.teacherDetails .name1 .right{
	float: left;
	width: 45%;
}
.teacherDetails .name1 .left{
	width: 45%;
}
.teacherDetails .name1 .left ul li {
	margin-top: 0.5rem;
	width: 12rem;
}
.teacherDetails .name1 .left ul li span{
	font-size: 1rem;
}
.teacherDetails .name1 .left ul li .lable{
	margin-right: 1rem;
	font-weight: bold;
}

/*name2*/
.teacherDetails .name2 .left,	.teacherDetails .name2 .right{
	float: left;
	width: 45%;
}
.teacherDetails .name2 .left{
	width:45%;	
	padding-right: 1rem;	
}
.teacherDetails .name2 .right{
	border-left:1px dashed gray ;
	padding-left: 1rem;
}

/*name3*/
.teacherDetails .name3{
	overflow: auto;
}
.teacherDetails .name3 .left{
	margin-left: 1rem;
}
.teacherDetails .name3 .left,	.teacherDetails .name3 .right{
	float: left;
	width: 45%;
}
.teacherDetails .name3 .left{
	width:45%;	
	padding-right: 1rem;	
}
.teacherDetails .name3 .right{
	border-left:1px dashed gray ;
	padding-left: 1rem;
}
.Introduce1, .Introduce2{
	border: 1px solid gainsboro;
    border-radius: 4px;
    padding: 0.5rem;
}
</style>
