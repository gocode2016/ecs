<template>
	<div class="teacherAdd">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<Tabs value="name1">
	        <Tab-pane label="导师信息" name="name1">
	        		<div class="name1" style="padding-top:1.5rem ;">
	        			<div class="left">
	        				<h2 style="margin-left: 1rem;margin-bottom: 1rem;">基本信息</h2>
	        				<div class="top" style="margin: 0 auto;text-align: center;padding-bottom: 1rem;border-bottom: 1px dashed gainsboro;">
	        					<img :src="bagImage" style="max-height: 11rem;"/>
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
						       <!--<div style="width:8rem;height:2rem;line-height: 58px;text-align: center;line-height: 2rem;">
						            <Button type="primary">上传</Button>
						        </div>-->
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
	        				<ul style="display: block;float: right;position: relative;margin-left: 1rem;">
	        					<li class="li">
	        						<span style="margin-right: 1.6rem;">姓名</span>
	        						<Input  placeholder="请输入姓名" v-model='TutorName' style="width: 180px;margin-left: 1rem;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.6rem;">头衔</span>
	        						<Input  placeholder='请用"|"分割开'  v-model='TutorPosition' style="width: 180px;margin-left: 1rem;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.1rem;">指导赛区</span>
	        						<Select v-model="AreaName" filterable multiple style="width:180px ;">
					                <Option v-for="item in divisionList" :value="item.AreaName" :key="item.AreaName">{{ item.AreaName }}</Option>
					            </Select>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.1rem;">导师酒店</span>
	        						<Input  placeholder="请输入导师酒店" v-model="TutorHotel" style="width: 180px;"></Input>
	        					</li>
	        					<li class="li">
	        						<span style="margin-right: 1.5rem;">绑定活动</span>
	        						<!--<Select  style="width:180px">
							        <Option v-for="item in activeNameList" :value="item.value" :key="item.value">{{ item.label }}</Option>
							    </Select>-->
							    <span>{{ChefActivityName}}</span>
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
	        				<h2 style="margin-left: 2rem;margin-bottom: 1rem;">导师介绍</h2>
	        				 <div class="rtf-content">
					        <quill-editor ref="myTextEditor" v-model="content" :config="editorOption" ></quill-editor>
					    </div>
					    
	        			</div>
	        			<Button type="primary" size="large" style="margin-left:29rem;margin-top: 0rem;" @click='saveData'>保存</Button>
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
	        					<h1 style="padding-bottom:1rem;text-align: center;">导师菜品录入</h1>
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
						        style="display:block;width:80px;margin-bottom: 1rem;margin-left: 4.5rem;margin-top: 8rem;" >
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
	        				
	        				<Button type="primary" @click='saveDish(dishName1,content2,name2Image,1)' style="margin-left: 22rem;margin-top: 1rem;">菜品保存</Button>
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
	        					<h1 style="padding-bottom:1rem;text-align: center;">导师菜品录入</h1>
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
						        style="display:block;width:80px;margin-bottom: 1rem;margin-left: 4.5rem;margin-top: 8rem;" >
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
	        				<Button type="primary" @click='saveDish(dishName2,content3,name3Image,2)' style="margin-left: 22rem;margin-top: 1rem;">菜品保存</Button>
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
				DishId:'',//保存名字后返回的菜品id
				isNo:'',//判断就扣返回是否为No
				sessionStorage:sessionStorage.getItem('loginkey'),//账号登录人
				TutorId:'',//导师ID(基本信息返回,用于保存菜品)
				ChefActivityName:'活动',//活动名称
				chefactivityid:'',//记录活动id
				headerImage:'../../../../../static/image/HeadPortrait.png',//默认头像
				bagImage:'../../../../../static/image/placeholderBag.png',//大图
				name2Image:'../../../../../static/image/placeholderBag.png',//name2图
				name3Image:'../../../../../static/image/placeholderBag.png',//name3图
				content: "",//存储编辑框
				editorOption: {
                 },
               TutorName:'',//导师姓名
               TutorPosition:'',//导师头衔
               TutorHotel:'',//导师酒店
               PriorityId:'',//活动页排序
               
               AreaName:[],//记录选择指导赛区
				divisionList:[//指导赛区列表
//					{
//                      value: '菏泽',
//                      label: '菏泽'
//                  },
                   
				],
//				activeNameList:[
//					{
//                      value: '知味活动2017',
//                      label: '知味活动2017'
//                 },
//				],
				IsDisplay:0,//是否显示
				ingredientsList:[//调料库数据
				],
				ingredientsOne:[],
				ingredientsTwo:[],
               //===================================================name2
                  content2: '',//存储编辑框
				 editorOption2: {
                 },
                
                 dishName1:'',//菜品1名称
				Main_ingredientList1:[//主料数据
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'1'
					}
				],
				Accessories_List1:[//辅料
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'2'
					}
					
				],
				seasoningList1:[//调料数据
					{	
						Material:'',
	         			Unit:0,
	         			MaterialType:'3'
	         		}
				],
				step1:[//步骤
					{
						StepName:''
					}
					
				],
				//===================================================name3
				 content3: '',//存储编辑框
				 editorOption3: {
               },
               dishName2:'',//菜品名
				Main_ingredientList2:[//主料数据
					{
						Material:'',
		         		Unit:0,
		         		MaterialType:'1'
					}
				],
				Accessories_List2:[//辅料
					{
						Material:'',
		         		Unit:0,
		         		MaterialType:'2'
						
					}
				],
				seasoningList2:[//调料数据
					{
						Material:'',
	         			Unit:0,
	         			MaterialType:'3'
					}
				],
				step2:[//步骤
					{
						StepName:''
					}
					
				],
				
			}
		},
		 components: {
            quillEditor
       },
		methods:{
			saveData(){//保存
				var self =this;
				if (!self.TutorId) {
					self.content = self.content.replace(/\"/g,"'");//把双引号改为单引号		
					//self.content=self.content.replace("\"", "\\\'"); 
					var Area = self.AreaName.join('|')
					if (self.bagImage.indexOf('/static/image/')>0 || self.headerImage.indexOf('/static/image/')>0) {//图片为本地占位图
						//self.$Message.error('上传图片不能为空');
						self.$Message.error({
							content:'上传图片不能为空',
							duration: 4
						});
					} else{
						$('.loding').show()
						
						let DATA = '{"ChefActivityId":'+self.chefactivityid+',"Introduce":"'+self.content+'","PicUrl":"'+self.bagImage+'","TutorName":"'+self.TutorName+'","HeadPicUrl":"'+self.headerImage+'","AreaName":"'+Area+'","TutorPosition":"'+self.TutorPosition+'","TutorHotel":"'+self.TutorHotel+'","PriorityId":"'+self.PriorityId+'","IsDisplay":'+self.IsDisplay+',"CreatePerson":"'+self.sessionStorage+'","UpdatePerson":"'+self.sessionStorage+'"}'
						console.log(DATA)
						$.ajax({
							type:"post",
							url:URLHeader.ECActivities+'/api/Tutor/AddTutor',
							dataTape:'json',
							contentType:'application/json; charset=utf-8',
							data:DATA,
							success:function(json){
								console.log(json)
								
								if (json=='No') {
									//self.$Message.error('数据保存出错,是否填写完整');
									self.$Message.error({
										content:'数据保存出错,是否填写完整',
										duration: 4
									});
								} else{
									self.TutorId = json
									//self.$Message.success('数据保存成功');
									self.$Message.success({
										content:'数据保存成功',
										duration: 4
									});
								}
								$('.loding').hide()
							},
							error:function(error){
								console.log(error)
							}
						});
					}	
				} else{
					self.$Notice.warning({
	                    title: '基本信息只能提交一次,如需更改,请到编辑页面',
	                });
				}

			},
			  handleHeaderSuccess (res, file) {//上传头像成功
                $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.headerImage = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
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
	                this.bagImage =  URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
	         },
	        //====================================================name2=========================================================
	         handleName2Success(res, file){//菜品1上传成功
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.name2Image =  URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
	         },
	         removeMain1(item){//主料删除
	         	this.Main_ingredientList1.splice(this.Main_ingredientList1.indexOf(item), 1)
	         },
	         addMainingredient1(){//新加主料
	         	var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1'
	         	}
	         	this.Main_ingredientList1.push(item);
	         	console.log(this.Main_ingredientList1)
	         },
	         removeAccessories1(item){//辅料删除   Accessories_List
	         	this.Accessories_List1.splice(this.Accessories_List1.indexOf(item), 1)
			},
			addAccessories1(){//辅料新增
				var self = this;
				var item = {
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
				var item = {
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
				var item = {
	         		StepName:'',
	         	}
	         	this.step1.push(item);
			},
			saveDish(DishName,DishStory,DishPicUrl,DishType){//菜品保存
				var self =this
//				console.log(DishName +DishStory+DishPicUrl+DishType)
				console.log('TutorId=',self.TutorId)
//				self.TutorId=1;
				if (self.TutorId) {//基本信息已经保存
					if (DishPicUrl.indexOf('/static/image/')>0 || !DishName || !DishStory) {
						//self.$Message.warning('请确认菜品图片,菜品名字,菜品故事已经完善');
						self.$Message.warning({
							content:'请确认菜品图片,菜品名字,菜品故事已经完善',
							duration: 4
						});
					} else{
  						 DishStory = DishStory.replace(/\"/g,"'");//把双引号改为单引号
						//DishStory=DishStory.replace("\"", "\\\'"); 
						var  DATA = '{"TutorId":'+self.TutorId+',"DishName":"'+DishName+'","DishStory":"'+DishStory+'","DishPicUrl":"'+DishPicUrl+'","DishType":"'+DishType+'","CreatePerson":"'+self.sessionStorage+'","UpdatePerson":"'+self.sessionStorage+'"}';
						console.log(DATA)
						$.ajax({
							type:"post",
							url:URLHeader.ECActivities+'/api/DishTutor/AddDishTutor',
							dataTape:'json',
							contentType:'application/json; charset=utf-8',
							data:DATA,
							success:function(json){
								self.DishId = json
								self.isNo=json;
								console.log(json)
								if (self.DishId =='No') {
									//self.$Message.error('提交错误');
									self.$Message.error({
										content:'提交错误!',
										duration: 4
									});
								} else{
									self.saveDishMaterial(DishType,self.DishId)
								}
							},
							error:function(error){
								console.log(error)
							}
						});
					}
						
				} else{//基本信息未保存
					self.$Notice.warning({
	                    title: '请先保存导师基本信息',
	                });
				}	
			},
			saveDishMaterial(DishType,DishId){//调料
				var self =this;
				var DATA
				if (DishType=='1') {
					DATA = self.Main_ingredientList1.concat(self.Accessories_List1,self.seasoningList1);
				} else{
					DATA = self.Main_ingredientList2.concat(self.Accessories_List2,self.seasoningList2);
				}
				var t=0;
				for (var i = 0;i<DATA.length;i++) {
					DATA[i].DishId = DishId
					if (!DATA[i].Material || DATA[i].Unit<=0) {//记录 是否有空
						t++
					} 
				}
				if (t==0) {
					$.ajax({
						type:"post",
						url:URLHeader.ECActivities+'/api/DishTutor/AddDishMaterial',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(DATA),
						success:function(json){
							console.log(DATA)
							console.log(json)
							self.isNo=json;
							if (self.isNo=='No') {
								//self.$Message.error('调料数据提交出现问题,请重新提交');
								self.$Message.error({
									content:'调料数据提交出现问题,请重新提交!',
									duration: 4
								});
							} else{
								self.saveOneStep(DishType,DishId)
							}
							
						},
						error:function(error){
							console.log(error)
						}
					});	
				}else{
					self.$Notice.warning({
	                    title: '请把食材信息填写完整',
	                });
				}
			},
			saveOneStep(DishType,DishId){//步骤保存
				var self =this;
				var step=[];
				if (DishType=='1') {
					step = self.step1;
				} else{
					step = self.step2;
				}
				var t=0;
				for (var i = 0;i<step.length;i++) {
					step[i].DishId = DishId;
					if (!step[i].StepName) {
						t++
					}
				}
				if (t==0) {
					$.ajax({
						type:"post",
						url:URLHeader.ECActivities+'/api/DishTutor/AddDishStep',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(step),
						success:function(json){
							console.log(step)
							self.isNo=json;
							if (self.isNo=='No') {
								//self.$Message.error('步骤数据提交出现问题,请重新提交');
								self.$Message.error({
									content:'步骤数据提交出现问题,请重新提交!',
									duration: 4
								});
							} else{
								self.saverecommend(DishType,DishId)
								//self.$Message.success('菜品保存成功');
							}
						},
						error:function(error){
							console.log(error)
						}
					});
				}else{
					self.$Notice.warning({
	                    title: '请把步骤信息填写完整',
	                });
				}
				
			},
			saverecommend(DishType,DishId){//推荐配料保存
				var self =this;
				var DATA;
				if (DishType=='1') {
					DATA = self.ingredientsOne;
				} else{
					DATA = self.ingredientsTwo;
				}
				var recommendList = [];
				for (var i = 0;i<DATA.length;i++) {
					var item = {
						RoleId:self.TutorId,
						FlavourRecId:DATA[i],
						DishId:DishId
					}
					recommendList.push(item)
				}
				console.log(recommendList)
				if (DATA.length>0) {
					$.ajax({
						type:"post",
						url:URLHeader.ECActivities+'/api/DishTutor/AddFR',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(recommendList),
						success:function(json){
							console.log(json)
							if (json=='No') {
								//self.$Message.error('导师推荐调料保存异常');
								self.$Message.error({
									content:'导师推荐调料保存异常',
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
				} else{
					//self.$Message.error('推荐配料为空');
					self.$Message.error({
						content:'推荐配料为空',
						duration: 4
					});
				}
				
				
			},
			//====================================================name3=========================================================
			handleName3Success(res, file){//菜品2上传成功
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.name3Image =  URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
	         },
	         removeMain2(item){//主料删除
	         	this.Main_ingredientList2.splice(this.Main_ingredientList2.indexOf(item), 1)
	         },
	         addMainingredient2(){//新加主料
	         	var item = {
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
				var item = {
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
				var item = {
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
				var item = {
	         		StepName:'',
	         	}
	         	this.step2.push(item);
			}
		},
		mounted:function(){
			var self =this;
			$('.loding').hide()
			$('.teacherAdd .rtf-content .ql-video').hide()//隐藏视频功能
			$('.teacherAdd .rtf-content .ql-container .ql-tooltip').css({//视频地址弹框移动
				'margin-left':'9rem'
			})
			get_URL();
			function get_URL(){//获取地址参数
    				var hash= window.location.hash.split('?')[1].split('=')[1];
    				self.chefactivityid=hash;
    				$.ajax({//获取活动名称
			        type :"get",
			        url :URLHeader.ECActivities+'/api/DishTutor/GetcaNameMRFRBycaId?caId='+hash,
			        dataType : 'json',
			        success : function(json) {
			        
			        	 var dataAll = JSON.parse(json);
			        	 	console.log(dataAll)
			        	 self.ChefActivityName = dataAll.ca.ChefActivityName//活动名称
			        	 self.ingredientsList =dataAll.FR //推荐配料
			        	 self.divisionList = dataAll.MR //指导赛区
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
	.ivu-select-dropdown{
		top: 0;
	}
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
.teacherAdd .name1{
		width:70rem;
		margin: 0 auto;
	}
.teacherAdd .name1 .left,.teacherAdd .name1 .right{
	float: left;
	width: 40rem;
}
.teacherAdd .name1 .right{
}
.teacherAdd .name1 .left{
	width: 30rem;
	min-height: 40rem;
	border-right:1px dashed gray ;
	padding-right: 1rem;
}
.teacherAdd .name1 .left ul .li{
	margin-top: 1rem;
}
.teacherAdd .name1 .right .rtf-content {
	
  /*  height: 380px;*/
    width: 33rem;
    margin-left: 1rem;
  }
  /*name2*/
 .teacherAdd .name2{
	width: 70rem;
	height: 100%;
	margin: 0 auto;
}
.teacherAdd .name2 .left,.teacherAdd .name2 .right{
	float: left;
}
.teacherAdd .name2 .left{
	overflow: auto;
	height: 100%;
	width: 30rem;	
	
}
.teacherAdd .name2 .right{
	width: 40rem;	
	border-left: 1px dashed gray;
}
.teacherAdd .name2 .left .rtf-content {
    /*height: 380px;*/
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
.teacherAdd .name3{
	margin: 0 auto;
	width: 70rem;
	height: 100%;
}
.teacherAdd .name3 .left,.teacherAdd .name3 .right{
	float: left;
}
.teacherAdd .name3 .left{
	overflow: auto;
	height: 100%;
	width: 30rem;	
	
}
.teacherAdd .name3 .right{
	width: 40rem;	
	border-left: 1px dashed gray;
}
.teacherAdd .name3 .left .rtf-content {
   /* height: 380px;*/
    padding-right: 1rem;
    margin-left: 2rem;
  }
.quill-editor{
	height: 35rem;
}

</style>
