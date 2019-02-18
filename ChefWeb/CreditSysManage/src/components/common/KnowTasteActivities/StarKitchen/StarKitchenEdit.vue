<template>
	<div class="starKitchenEdit">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<Tabs value="name1" @on-click='pp'>
	        <Tab-pane label="星厨信息" name="name1" >
	        		<div class="name1" style="padding-top:1.5rem ;">
	        			<div class="left">
	        				<h2 style="margin-left: 1rem;margin-bottom: 1rem;">基本信息</h2>
	        				<div class="top" style="margin: 0 auto;text-align: center;padding-bottom: 1rem;border-bottom: 1px dashed gainsboro;">
	        					<img :src="bagImage" style="max-width:20rem;max-height: 15rem;"/>
	        					<br />
	        					<Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="handleBigImageSuccess"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImageMember
						        style="display: inline-block;width:80px;" >
						         <Button type="primary">选择大图</Button>
						    </Upload>
	        				</div>
	        				<div style="text-align: center;float: left;margin-left: 1rem;min-width: 10rem;">
	        					<img style="max-width: 10rem;margin-bottom: 1rem;max-height: 10rem;margin-top: 1rem;" :src="headerImage" />
	        					<br />
	        					<Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="handleHeaderSuccess"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImageMember
						        style="display: inline-block;width:80px;" >
						       <!--<div style="width:8rem;height:2rem;line-height: 58px;text-align: center;line-height: 2rem;">
						            <Button type="primary">上传</Button>
						        </div>-->
						         <Button type="primary">选择头像</Button>
						    </Upload>
	        				</div>
	        				<ul style="display: block;float: left;margin-left: 1rem;">
	        					<li class="li">
	        						<span style="margin-right: 1.6rem;">姓名</span>
	        						<Input  placeholder="请输入姓名" v-model='ChefStarName' style="width: 180px;margin-left: 1rem;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.6rem;">岗位</span>
	        						<Input  placeholder="请输入岗位"  v-model='ChefStarPosition' style="width: 180px;margin-left: 1rem;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.1rem;">星厨酒店</span>
	        						<Input  placeholder="请输入星厨酒店" v-model="HotelName" style="width: 180px;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.1rem;">城市</span>
	        						<Input  placeholder="请输入城市" v-model="CityName" style="width: 180px;margin-left: 1.5rem;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.8rem;">活动页显示</span>
	        						<Radio-group v-model="IsDisplay">
							        <Radio label="1" >是</Radio>
							        <Radio label="0">否</Radio>
							    </Radio-group>
	        					</li>
	        					<li class="li">
	        						<span>活动页排序</span>
	        						<Input  placeholder="活动页排序" v-model='PriorityId' style="width: 180px;margin-left: 0.5rem;"></Input>
	        					</li>
	        				</ul>
	        				
	        			</div>
	        			<div class="right">
	        				<h2 style="margin-left: 2rem;margin-bottom: 1rem;">星厨介绍</h2>
	        				 <div class="rtf-content">
					        <quill-editor ref="myTextEditor" v-model="content" :config="editorOption"></quill-editor>
					    </div>
					    <Button type="primary" size="large" style="margin-left:29rem;margin-top: 0rem;" @click='saveData'>保存</Button>
	        			</div>
	        		</div>
	        </Tab-pane>
	        <Tab-pane label="菜品1" name="name2">
	        		<div class="name2">
	        			<div class="left">
	        				<h2 style="margin-left: 1rem;margin-bottom: 1rem;">菜品故事</h2>
	        				<div class="rtf-content">
					        <quill-editor ref="myTextEditor" v-model="content2" :config="editorOption2"></quill-editor>
					    </div>
	        			</div>
	        			<div class="right">
	        				<div class="top" style="">
	        					<h1 style="padding-bottom:1rem;text-align: center;">星厨菜品录入</h1>
	        					<img :src="name2Image" style="display: block;float:left;max-width: 15rem;max-height: 20rem;margin-left: 1.5rem;"/>
	        					<div style="float: left;margin-left: 1rem;margin-top: 1rem;">
	        					<span style="margin-left: 1rem;">菜品名:</span>
						    <Input  placeholder="请输入菜品名称..." style="width: 170px;margin-left: 1.7rem;" v-model='dishName1'></Input>
						    <div style="margin-top: 1rem;position: relative;">
		        					<span style="margin-left: 1rem;margin-top: 1rem;">配料推荐:</span>
		        					<Select v-model="ingredientsOne" multiple style="width:10.5rem;margin-left:1rem;" placeholder='配料推荐'>
							      <Option  v-for="item in ingredientsList" :value="item.FlavourRecId" :key="item.FlavourRecId">{{ item.FlavourName }}</Option>
							    </Select>
		        				</div>
	        					</div>
	        					<Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="handleName2Success"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImageCook
						        style="display:block;width:80px;margin-bottom: 1rem;margin-left: 6.5rem;margin-top: 8rem;" >
						       <!--<div style="width:8rem;height:2rem;line-height: 58px;text-align: center;line-height: 2rem;">
						            <Button type="primary">上传</Button>
						        </div>-->
						         <Button type="primary">选择菜品</Button>
						    </Upload>
	        				</div>
	        				<div style="overflow: auto;">
		        				<div class="Main_ingredient" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">主料</h3>
		        					<ul>
		        						<li v-for='item in Main_ingredientList1' style="margin-top: 0.5rem;">
		        							<Input v-model='item.Material' placeholder="主料" style="width: 140px;"></Input>
		        							<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
		        							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
		        							<a @click='removeMain1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						<li>
		        							<Button type="primary" @click="addMainingredient1" style="margin-left:5.7rem;margin-top:0.5rem;">新增主料</Button>
		        						</li>
		        					</ul>
		        				</div>
		        				<div class="Accessories" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">辅料</h3>
		        					<ul>
		        						<li v-for='item in Accessories_List1' style="margin-top: 0.5rem;">
		        							<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
		        							<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
		        							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
		        							<a @click='removeAccessories1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						<li>
		        							<Button type="primary" @click="addAccessories1" style="margin-left:5.7rem;margin-top:0.5rem;">新增辅料</Button>
		        						</li>
		        					</ul>
		        				</div>
	        				</div>
	        				<div style="overflow: auto;">
	        					<div class="seasoning" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">调料</h3>
		        					<ul>
		        						<li v-for='item in seasoningList1' style="margin-top: 0.5rem;">
		        							<Input v-model='item.Material' placeholder="调料" style="width: 140px;"></Input>
		        							<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
		        							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
		        							<a @click='removeSeasoning1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						<li>
		        							<Button type="primary" @click="addseasoning1" style="margin-left:5.7rem;margin-top:0.5rem;">新增调料</Button>
		        						</li>
		        					</ul>
		        				</div>
		        				<div class="seasoning" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">烹饪步骤</h3>
		        					<ul style="padding-left: 0.9rem;">
		        						<li v-for='item in step1' style="margin-top: 0.5rem;list-style-type: decimal;">
		        							<Input v-model='item.StepName' placeholder="请输入" type="textarea" style="width: 210px;"></Input>
		        							<a @click='removeStep1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						
		        							<Button type="primary" @click="addStep1" style="margin-left:5.7rem;margin-top:0.5rem;">新增步骤</Button>
		        					</ul>
		        				</div>
	        				</div>
	        				<Button type="primary" @click='saveDish(1,starKitchenDishId1)' style="margin-left: 22rem;margin-top: 1rem;">菜品保存</Button>
	        			</div>
	        		</div>
	        </Tab-pane>
	        <Tab-pane label="菜品2" name="name3">
	        		<div class="name3">
	        			<div class="left">
	        				<h2 style="margin-left: 1rem;margin-bottom: 1rem;">菜品故事</h2>
	        				<div class="rtf-content">
					        <quill-editor ref="myTextEditor" v-model="content3" :config="editorOption3"></quill-editor>
					    </div>
	        			</div>
	        			<div class="right">
	        				<div class="top" style="">
	        					<h1 style="padding-bottom:1rem;text-align: center;">星厨菜品录入</h1>
	        					<img :src="name3Image" style="display: block;float:left;max-width: 15rem;max-height: 20rem;margin-left: 1.5rem;"/>
	        					<div style="float: left;margin-left: 1rem;margin-top: 1rem;">
	        					<span style="margin-left: 1rem;">菜品名:</span>
						    <Input  placeholder="请输入菜品名称..." v-model='dishName2' style="width: 170px;margin-left: 1.7rem;"></Input>
						    <div style="margin-top: 1rem;position: relative;">
						    		<span style="margin-left: 1rem;margin-top: 1rem;">配料推荐:</span>
		        					<Select v-model="ingredientsTwo" multiple style="width:10.5rem;margin-left:1rem;" placeholder='配料推荐'>
							      <Option  v-for="item in ingredientsList" :value="item.FlavourRecId" :key="item.FlavourRecId">{{ item.FlavourName }}</Option>
							    </Select>
						    </div>
	        					</div>
	        					<Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="handleName3Success"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImageCook
						        style="display:block;width:80px;margin-bottom: 1rem;margin-left: 6.5rem;margin-top: 8rem;" >
						       <!--<div style="width:8rem;height:2rem;line-height: 58px;text-align: center;line-height: 2rem;">
						            <Button type="primary">上传</Button>
						        </div>-->
						         <Button type="primary">选择菜品</Button>
						    </Upload> 
	        				</div>
	        		
	        				<div style="overflow: auto;">
		        				<div class="Main_ingredient" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">主料</h3>
		        					<ul>
		        						<li v-for='item in Main_ingredientList2' style="margin-top: 0.5rem;">
		        							<Input v-model='item.Material' placeholder="主料" style="width: 140px;"></Input>
		        							<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
		        							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
		        							<a @click='removeMain2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						<li>
		        							
		        							<Button type="primary" @click="addMainingredient2" style="margin-left:5.7rem;margin-top:0.5rem;">新增主料</Button>
		        						</li>
		        					</ul>
		        				</div>
		        				<div class="Accessories" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">辅料</h3>
		        					<ul>
		        						<li v-for='item in Accessories_List2' style="margin-top: 0.5rem;">
		        							<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
		        							<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
		        							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
		        							<a @click='removeAccessories2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						<li>
		        							<Button type="primary" @click="addAccessories2" style="margin-left:5.7rem;margin-top:0.5rem;">新增辅料</Button>
		        						</li>
		        					</ul>
		        				</div>
	        				</div>
	        				<div style="overflow: auto;">
	        					<div class="seasoning" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">调料</h3>
		        					<ul>
		        						<li v-for='item in seasoningList2' style="margin-top: 0.5rem;">
		        							<Input v-model='item.Material' placeholder="调料" style="width: 140px;"></Input>
		        							<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
		        							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
		        							<a @click='removeSeasoning2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        						<li>
		        							<Button type="primary" @click="addseasoning2" style="margin-left:5.7rem;margin-top:0.5rem;">新增调料</Button>
		        						</li>
		        					</ul>
		        				</div>
		        				<div class="seasoning" style="float: left;">
		        					<h3 style="text-align: center;margin-top: 1rem;">烹饪步骤</h3>
		        					<ul style="padding-left: 1rem;">
		        						<li v-for='item in step2' style="margin-top: 0.5rem;list-style-type: decimal;">
		        						
		        							<Input v-model='item.StepName' placeholder="请输入" type="textarea" style="width: 210px;"></Input>
		        							<a @click='removeStep2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
		        						</li>
		        							<Button type="primary" @click="addStep2" style="margin-left:5.7rem;margin-top:0.5rem;">新增步骤</Button>
		        					</ul>
		        				</div>
	        				</div>
	        				<Button type="primary" @click='saveDish(2,starKitchenDishId2)' style="margin-left: 22rem;margin-top: 1rem;">菜品保存</Button>
	        			</div>
	        		</div>
	        </Tab-pane>
	   </Tabs>
	</div>
</template>

<script>
	 import { quillEditor } from 'vue-quill-editor';//编写框
	export default{
		data(){
			return{
				UPImageMember:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Member',
				UPImageCook:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Cook',
				starKitchenId:'',//星厨ID
				ChefActivityId:'',//活动ID
				sessionStorage:sessionStorage.getItem('loginkey'),//账号登录人
				headerImage:'../../../../../static/image/HeadPortrait.png',//默认头像
				bagImage:'../../../../../static/image/placeholderBag.png',//大图
				content: "",//存储编辑框
				editorOption: {},
                 ChefStarName:'',//星厨姓名
                 ChefStarPosition:'',//星厨头衔
                 HotelName:'',//星厨酒店
                 CityName:'',//城市
                 PriorityId:'',//活动页排序
				IsDisplay:0,//是否显示
				ingredientsList:[//调料库数据
				],
				ingredientsOne:[],
				ingredientsTwo:[],
               //===================================================name2
                 starKitchenDishId1:'',//菜品id
                 content2: '',//存储编辑框
				editorOption2: {
                 },
                 dishName1:'',//菜品1名称
                 name2Image:'../../../../../static/image/placeholderBag.png',//name2图
				Main_ingredientList1:[//主料数据
				],
				Accessories_List1:[//辅料

				],
				seasoningList1:[//调料数据
				],
				step1:[//步骤
				],
				//===================================================name3
				starKitchenDishId2:'',//菜品id
				 content3: '',//存储编辑框
				 editorOption3: {
                 },
                 dishName2:'',//菜品名
               	name3Image:'../../../../../static/image/placeholderBag.png',//name3图
				Main_ingredientList2:[//主料数据

				],
				Accessories_List2:[//辅料

				],
				seasoningList2:[//调料数据

				],
				step2:[//步骤

				],
				
			}
		},
		 components: {
            quillEditor
       },
		methods:{
			pp(name){
				console.log(name)
			},
			saveData(){//基本信息保存    starKitchen/UpdateSingle
				$('.loding').show()
				var self =this;
				self.content = self.content.replace(/\"/g,"'");//把双引号改为单引号
				//self.content=self.content.replace("\"", "\\\'"); 
				let DATA = '{"ChefStarId":'+self.starKitchenId+',"Introduce":"'+self.content+'","PicUrl":"'+self.bagImage+'","ChefStarName":"'+self.ChefStarName+'","HeadPicUrl":"'+self.headerImage+'","ChefStarPosition":"'+self.ChefStarPosition+'","HotelName":"'+self.HotelName+'","CityName":"'+self.CityName+'","PriorityId":"'+self.PriorityId+'","IsDisplay":'+self.IsDisplay+',"UpdatePerson":"'+self.sessionStorage+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/ChefStar/UpdateChefStar',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						console.log(json)
						$('.loding').hide()
						if (json=='No') {
							//self.$Message.error('数据保存异常');
							self.$Message.error({
								content:'数据保存异常',
								duration: 4
							});
						} else{
							//self.$Message.success('菜品保存成功');
							self.$Message.success({
								content:'菜品保存成功',
								duration: 4
							});
						}
					},
					error:function(error){
						console.log(error)
					}
				});	
			},
			  handleHeaderSuccess (res, file) {//上传头像成功
                $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.headerImage =URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
            },
             handleFormatError (file) {//文件格式
            		$('.loding').hide()
                this.$Notice.warning({
                    title: '文件格式不正确',
                    desc: '文件 ' + file.name + ' 格式不正确，请上传 jpg 或 png 格式的图片。'
                });
                 
            },
             handleMaxSize (file) {//文件大小
            		console.log(file)
            		$('.loding').hide()
                this.$Notice.warning({
                    title: '超出文件大小限制',
                    desc: '文件 ' + file.name + ' 太大，不能超过 2M。'
                });
            },
             handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
         	},
	         handleBigImageSuccess(res, file){//大图上传
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.bagImage = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
	         },
	         getstarKitchenInformation(){//获取星厨基本信息
				var self =this;
				//self.starKitchenId= 61;
				//URLHeader.ECActivities+/api/ChefStar/GetChefStarByCSId?csId={csId}
				$.ajax({//获取活动名称
			        type :"get",
			        url :URLHeader.ECActivities+'/api/ChefStar/GetChefStarByCSId?csId='+self.starKitchenId,
			        dataType : 'json',
			        success : function(json) {
			        		var dataAll =JSON.parse(json)
			        		//console.log(dataAll)
			        		self.ChefStarName = dataAll.ChefStarName  //星厨姓名
			        		self.ChefStarPosition = dataAll.ChefStarPosition //星厨头衔
			        		self.HotelName =dataAll.HotelName //星厨酒店
			        		self.CityName = dataAll.CityName//城市
			        		self.PriorityId = dataAll.PriorityId //排序
			        		self.IsDisplay =dataAll.IsDisplay //活动显示
			        		self.bagImage = dataAll.PicUrl //大图
			        		self.headerImage =dataAll.HeadPicUrl //头像
			        		self.content =dataAll.Introduce  
						$('.loding').hide()
			        },
			        error : function(error) { 
			        	
			        }
	   		    });

	         },
	         getDishInformation(DishType){//获取菜品数据
	         	var self= this;
	         	//URLHeader.ECActivities+/api/ChefStar/GetDishChefStar?caId={caId}&dishType={dishType}&chefStarId={chefStarId}
	         	$.ajax({//获取活动名称
			        type :"get",
			        url :URLHeader.ECActivities+'/api/ChefStar/GetDishChefStar?caId='+self.ChefActivityId+'&dishType='+DishType+'&chefStarId='+self.starKitchenId,
			        dataType : 'json',
			        success : function(json) {
			        	if (json !='No') {
			        		var dataAll =JSON.parse(json).dish;
			        		console.log(dataAll)
			        		if (DishType=='1') {//菜品一
			        			self.dishOneParsing(dataAll);
			        		} else{//菜品二
			        			self.dishTwoParsing(dataAll)
			        		}	
			        		self.ingredientsList =dataAll.AllSelect//全部配料
			        	}else{
			        		if (DishType=='1') {//菜品一
			        			self.starKitchenDishId1='0';  //菜品1id
			        		} else{//菜品二
			        			self.starKitchenDishId2='0';  //菜品2id
			        		}	
			        	}
			        		
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });	
	         },
	         dishOneParsing(data){//解析菜品1
	         	console.log(data)
	         	var self =this;
	         	var Basic = data.Basic;
	         	self.content2 = Basic.DishStory//菜品故事
	         	self.dishName1 = Basic.DishName//菜品名称
	         	self.name2Image = Basic.DishPicUrl  //菜品照片
	         	self.starKitchenDishId1=Basic.DishChefStarId;  //菜品1id
	         	//推荐配料
	         	for (var i=0;i<data.Select.length;i++) {
	         		self.ingredientsOne.push(data.Select[i].FlavourRecId)
	         	}
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
	         	
	         },
	         dishTwoParsing(data){//解析菜品2
	         	//console.log(data)
	         	var self =this;
	         	var Basic = data.Basic;
	         	self.content3 = Basic.DishStory//菜品故事
	         	self.dishName2 = Basic.DishName//菜品名称
	         	self.name3Image = Basic.DishPicUrl  //菜品照片
	         	self.starKitchenDishId2=Basic.DishChefStarId;  //菜品2id
	         	for (var i=0;i<data.Select.length;i++) {
	         		self.ingredientsTwo.push(data.Select[i].FlavourRecId)
	         	}
	         	
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
	         	self.step2 = data.DishStep;
	         	//推荐配料
	         	//FlavourRecId
	         },
	        //====================================================name2=========================================================
	         handleName2Success(res, file){//菜品1上传成功
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.name2Image = URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
	         },
	         removeMain1(item){//主料删除
	         	this.Main_ingredientList1.splice(this.Main_ingredientList1.indexOf(item), 1)
	         },
	         addMainingredient1(){//新加主料
	         	var self =this
	         	var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1',
	         		DishId:self.starKitchenDishId1,
	         	}
	         	this.Main_ingredientList1.push(item);
	         	
	         },
	         removeAccessories1(item){//辅料删除   Accessories_List
	         	this.Accessories_List1.splice(this.Accessories_List1.indexOf(item), 1)
			},
			addAccessories1(){//辅料新增
				var self = this;
				var item = {
					DishId:self.starKitchenDishId1,
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'2'
	         	}
	         	this.Accessories_List1.push(item);
			},
			removeSeasoning1(item){//调料删除
				this.seasoningList1.splice(this.seasoningList1.indexOf(item), 1)
			},
			addseasoning1(){//调料新增
				var self =this
				var item = {
					DishId:self.starKitchenDishId1,
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'3'
	         	}
	         	this.seasoningList1.push(item);
			},
			removeStep1(item){//删除步骤
				this.step1.splice(this.step1.indexOf(item), 1)
			},
			addStep1(){//新增步骤
				var self =this
				var item = {
					DishId:self.starKitchenDishId1,
	         		StepName:'',
	         	}
	         	this.step1.push(item);
			},
			saveDish(dishType,starKitchenDishId){//菜品1保存
				$('.loding').show()
				var self =this
				var DATA;
				var arr=[];
				var flavourRecRole=[];//推荐配料
				if (dishType=='1') {//菜品1
					arr = self.Main_ingredientList1.concat(self.Accessories_List1,self.seasoningList1);
					var dishMaterial =JSON.stringify(arr)
					for (var i = 0;i<self.step1.length;i++) {
						delete self.step1[i]["StepId"]
					}
					self.content2 = self.content2.replace(/\"/g,"'");//把双引号改为单引号
					//self.content2=self.content2.replace("\"", "\\\'"); 
					var DishStep = JSON.stringify(self.step1)  //
					for (var t = 0;t<self.ingredientsOne.length;t++) {
						var item = {
							RoleId:self.starKitchenId,
							FlavourRecId:self.ingredientsOne[t],
							DishId:self.starKitchenDishId1
						}
						flavourRecRole.push(item)
					}
					flavourRecRole = JSON.stringify(flavourRecRole)
					DATA='{"dishChefStar":{"DishChefStarId":"'+self.starKitchenDishId1+'","ChefStarId":"'+self.starKitchenId+'","DishName":"'+self.dishName1+'","DishStory":"'+self.content2+'","DishPicUrl":"'+self.name2Image+'","DishType":1,"UpdatePerson":"'+self.sessionStorage+'"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"flavourRecRole":'+flavourRecRole+'}'
				} else{
					arr = self.Main_ingredientList2.concat(self.Accessories_List2,self.seasoningList2);
					var dishMaterial =JSON.stringify(arr)
					for (var i = 0;i<self.step2.length;i++) {
						delete self.step2[i]["StepId"]
					}
					self.content3 = self.content3.replace(/\"/g,"'");//把双引号改为单引号
					//self.content3=self.content3.replace("\"", "\\\'"); 
					var DishStep = JSON.stringify(self.step2)  //
					var flavourRecRole=[] ;
					for (var t = 0;t<self.ingredientsTwo.length;t++) {
						var item = {
							RoleId:self.starKitchenId,
							FlavourRecId:self.ingredientsTwo[t],
							DishId:self.starKitchenDishId2
						}
						flavourRecRole.push(item)
					}
					flavourRecRole = JSON.stringify(flavourRecRole)
					DATA='{"dishChefStar":{"DishChefStarId":"'+self.starKitchenDishId2+'","ChefStarId":"'+self.starKitchenId+'","DishName":"'+self.dishName2+'","DishStory":"'+self.content3+'","DishPicUrl":"'+self.name3Image+'","DishType":2,"UpdatePerson":"'+self.sessionStorage+'"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"flavourRecRole":'+flavourRecRole+'}'
				}
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/ChefStar/UpdateDishChefStar',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						console.log(json)
						$('.loding').hide()
						if (json=='No') {
							//self.$Message.error('数据保存异常');
							self.$Message.error({
								content:'数据保存异常',
								duration: 4
							});
						} else{
							//self.$Message.success('菜品保存成功');
							self.$Message.success({
								content:'菜品保存成功',
								duration: 4
							});
						}
					},
					error:function(error){
						console.log(error)
					}
				});
			},
			
			//====================================================name3=========================================================
			handleName3Success(res, file){//菜品2上传成功
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.name3Image = URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
	         },
	         removeMain2(item){//主料删除
	         	this.Main_ingredientList2.splice(this.Main_ingredientList2.indexOf(item), 1)
	         },
	         addMainingredient2(){//新加主料
	         	var self = this;
	         	var item = {
	         		DishId:self.starKitchenDishId2,
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1'
	         	}
	         	this.Main_ingredientList2.push(item);
	         	
	         	console.log(this.Main_ingredientList2)
	         },
	         removeAccessories2(item){//辅料删除   Accessories_List
	         	this.Accessories_List2.splice(this.Accessories_List2.indexOf(item), 1)
	         
			},
			addAccessories2(){//辅料新增
				var self =this
				var item = {
					DishId:self.starKitchenDishId2,
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'2'
	         	}
	         	this.Accessories_List2.push(item);
			},
			removeSeasoning2(item){//调料删除
				this.seasoningList2.splice(this.seasoningList2.indexOf(item), 1)
			},
			addseasoning2(){//调料新增
				var self =this
				var item = {
					DishId:self.starKitchenDishId2,
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'3'
	         	}
	         	this.seasoningList2.push(item);
			},
			removeStep2(item){//删除步骤
				this.step2.splice(this.step2.indexOf(item), 1)
			},
			addStep2(){//新增步骤
				var self =this
				var item = {
					DishId:self.starKitchenDishId2,
	         		StepName:'',
	         	}
	         	this.step2.push(item);
			}
		},
		mounted:function(){
			var self =this;
			//$('.starKitchenEdit .rtf-content .ql-video').hide()//隐藏视频功能
			$('.starKitchenEdit .name2 .rtf-content .ql-video').hide()//隐藏视频功能
			$('.starKitchenEdit .name3 .rtf-content .ql-video').hide()//隐藏视频功能
			get_URL();
			self.getstarKitchenInformation();//星厨基本信息
			self.getDishInformation(1);//菜品1
			self.getDishInformation(2);//菜品2
			$('.starKitchenEdit .rtf-content .ql-container .ql-tooltip').css({//视频地址弹框移动
				'margin-left':'9rem'
			})
			function get_URL(){//获取地址参数
    				var hash= window.location.hash.split('?')[1].split('&');
				self.starKitchenId = hash[0].split('=')[1]//星厨id
				self.ChefActivityId = hash[1].split('=')[1]//活动id
				//console.log(self.Tutorid+'&'+self.ChefActivityId)
				$.ajax({//获取星厨推荐配料
			        type :"get",
			        url :URLHeader.ECActivities+'/api/ChefStar/GetChefStarFlavourRec?caId='+self.ChefActivityId,
			        dataType : 'json',
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 	console.log(dataAll)
			        	 self.ingredientsList =dataAll//推荐配料
			        	 $('.loding').hide()
			        },
			        error : function(error) { 
			        }
	   		    });
    			}
			
		}
	}
</script>
<style>
	html, body { min-width: 1210px; }
	.ql-container{
	height: 80%;
}
</style>

<style scoped>
.demo-spin-icon-load{
    animation: ani-demo-spin 1s linear infinite;
}
@keyframes ani-demo-spin {
    from { transform: rotate(0deg);}
    50%  { transform: rotate(180deg);}
    to   { transform: rotate(360deg);}
}
.starKitchenEdit .name1{
	/*height: 100%;*/
	width:70rem;
	margin: 0 auto;
}
.starKitchenEdit .name1 .left,.starKitchenEdit .name1 .right{
	float: left;
	width: 40rem;
}
.starKitchenEdit .name1 .right{
	height: 100%;
	border-left:1px dashed gray ;
}
.starKitchenEdit .name1 .left{
	padding-right: 1rem;
	width: 30rem;
	min-height: 40rem;
}
.starKitchenEdit .name1 .left ul .li{
	margin-top: 1rem;
}
.starKitchenEdit .name1 .right .rtf-content {
    /*height: 38rem;*/
    width: 33rem;
    margin-left: 1rem;
  }
  /*name2*/
 .starKitchenEdit .name2{
	width: 70rem;
	height: 100%;
	margin: 0 auto;
}
.starKitchenEdit .name2 .left,.starKitchenEdit .name2 .right{
	float: left;
}
.starKitchenEdit .name2 .left{
	width: 30rem;	
	height: 100%;
}
.starKitchenEdit .name2 .right{
	width: 40rem;	
	border-left: 1px dashed gray;
}
.starKitchenEdit .name2 .left .rtf-content {
	
   /* height: 30rem;*/
    padding-right: 1rem;
    margin-left: 2rem;
  }
  /*主料*/
 .Main_ingredient  ul {
 	margin-left: 1rem;
 	margin-top: 1rem;	
 	width: 16rem;	
 	border: 1px dashed gray;
 	padding: 0.5rem;
 	border-radius:4px ;
 }
 /*辅料*/
.Accessories  ul {
 	margin-left: 1rem;
 	margin-top: 1rem;	
 	width: 16rem;	
 	border: 1px dashed gray;
 	padding: 0.5rem;
 	border-radius:4px ;
 }
 /*调料*/
 .seasoning  ul {
 	margin-left: 1rem;
 	margin-top: 1rem;	
 	width: 16rem;	
 	border: 1px dashed gray;
 	padding: 0.5rem;
 	border-radius:4px ;
 }
 
 /*=======name3*/
.starKitchenEdit .name3{
	margin: 0 auto;
	width: 70rem;
	height: 100%;
}
.starKitchenEdit .name3 .left,.starKitchenEdit .name3 .right{
	float: left;
}
.starKitchenEdit .name3 .left{
	height: 100%;
	width: 30rem;	
	
}
.starKitchenEdit .name3 .right{
	width: 40rem;	
	border-left: 1px dashed gray;
}
.starKitchenEdit .name3 .left .rtf-content {
   /* height: 30rem;*/
    padding-right: 1rem;
    margin-left: 2rem;
  }
.quill-editor{
	height: 35rem;
}
</style>
