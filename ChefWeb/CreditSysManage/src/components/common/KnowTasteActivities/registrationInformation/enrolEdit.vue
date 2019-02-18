<template>
	<div class="enrolEdit">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h1 style="margin: 0 auto;padding-bottom: 1rem;border: 1px dashed gray;text-align: center;height: 2.5rem;border-radius: 4px;line-height: 2.5rem; width: 15rem;">报名厨师信息编辑</h1>
		<div class="BasicInformation">
			<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">基本信息</h2>
			<div style="overflow: auto;">
				<ul>
					<li class="informationLi">
						<span class="labie">姓名</span>
						<!--<span class="value">张三</span>-->
						<!--<Input v-model="name" class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
						<span style="margin-left: 2rem;">{{name}}</span>
					</li>
					
					<li class="informationLi">
						<span class="labie">出生日期</span>
						<!--<Input v-model="Birthday"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
						<DatePicker type="date" placeholder="选择日期" v-model="Birthday" style="width: 128px;margin-left: 0.5rem;" class='dataTime'></DatePicker>
					</li>
					
					<li class="informationLi">
						<span class="labie">赛区</span>
						<Select v-model="division" style="width:130px;margin-left: 1.8rem;">
					        <Option v-for="item in divisionList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
						<!--<Input v-model="division"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
					</li>
				</ul>
				<ul>
					<li class="informationLi">
						<span class="labie">酒店</span>
						<!--<Input v-model="ChefHotelName"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
						<span style="margin-left: 2rem;">{{ChefHotelName}}</span>
					</li>
					<li class="informationLi">
						<span class="labie">途径</span>
						<Select v-model="ApplyType" style="width:130px;margin-left: 1.8rem;">
					        <Option v-for="item in ApplyTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
						<!--<Input v-model="pathway"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
					</li>
					<li class="informationLi Referrer">
						<span class="labie">推荐人</span>
						<Input v-model="ReferrerName"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>
					</li>
					
				</ul>
				<div class="right">
					<Upload
				        ref="upload"
				        :show-upload-list="false"
				        :on-success="handlePersonImageSuccess"
				        :format="['jpg','jpeg','png']"
				        :max-size="2048"
				        :before-upload='handleBeforeUpload'
				        :on-format-error="handleFormatError"
				        :on-exceeded-size="handleMaxSize"
				        :multiple=false
				        type="drag"
				        :action=UPImageMember
				        style="display: inline-block;width:8rem;" >
				        <img :src="ChefPicUrl" style="display: block;max-width: 8rem;float: left;"/>
				    </Upload>
					<p style="text-align: center;">个人照片</p>
				</div>
				<div class="right">
					<!--<img src="../../../../../dist/static/image/HeadPortrait.png" style="display: block;width: 8rem;float: left;"/>-->
					<Upload
				        ref="upload"
				        :show-upload-list="false"
				        :on-success="handleHotelImageSuccess"
				        :format="['jpg','jpeg','png']"
				        :max-size="2048"
				        :before-upload='handleBeforeUpload'
				        :on-format-error="handleFormatError"
				        :on-exceeded-size="handleMaxSize"
				        :multiple=false
				        type="drag"
				        :action=UPImageMember
				        style="display: inline-block;width:8rem;" >
				        <img :src="ChefHotelPicUrl" style="display: block;width: 8rem;float: left;"/>
				    </Upload>
					<p style="text-align: center;">酒店照片</p>
				</div>
				
			</div>
		</div>
		<div class="dish">
			<div class="localDish">
				<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">本地菜</h2>
				<!--<img  src="../../../../../static/image/placeholderBag.png" style="width: 15rem;display: block;margin: 1rem auto;"/>-->
				<Upload
			        ref="upload"
			        :show-upload-list="false"
			        :on-success="handleLocalDishImageSuccess"
			        :format="['jpg','jpeg','png']"
			        :max-size="2048"
			        :before-upload='handleBeforeUpload'
			        :on-format-error="handleFormatError"
			        :on-exceeded-size="handleMaxSize"
			        :multiple=false
			        type="drag"
			        :action=UPImageCook
			        style="width: 8rem;display: block;margin: 1rem auto;" >
			        <img :src="localDishImg" style="display: block;margin:0 auto;width: 8rem;"/>
			   </Upload>
				<div>
					<span style="margin-left: 7rem;">菜品名:</span>
					<Input v-model="localDishName" placeholder="请输入本地菜名" style="width: 150px;margin-left: 1rem;"></Input>
				</div>
				<h2>菜品故事</h2>
				<Input v-model="localStory" type="textarea" :autosize='{minRows: 3,maxRows: 10}' placeholder="请输入..." style="margin: 1rem auto;width: 28rem;"></Input>
				<h2>食材明细</h2>
				<h3 style="text-align: center;">主料</h3>
				<ul class="ingredients">
					<li v-for='item in Main_ingredientList1' style="margin-top: 0.5rem;">
						<Input v-model='item.Material' placeholder="主料" style="width: 140px;"></Input>
						<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<a @click='removeMain1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addMainingredient1" style="margin-left:8.4rem;margin-top:0.5rem;">新增主料</Button>
					</li>
				</ul>
				<h3 style="text-align: center;">辅料</h3>
				<ul class="ingredients">
					<li v-for='item in Accessories_List1' style="margin-top: 0.5rem;">
						<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
						<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
						<a @click='removeAccessories1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addAccessories1" style="margin-left:8.4rem;margin-top:0.5rem;">新增辅料</Button>
					</li>
				</ul>
				<h3 style="text-align: center;">调料</h3>
				<ul class="ingredients">
					<li v-for='item in seasoningList1' style="margin-top: 0.5rem;">
						<Input v-model='item.Material' placeholder="调料" style="width: 140px;"></Input>
						<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
						<a @click='removeSeasoning1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addseasoning1" style="margin-left:5.7rem;margin-top:0.5rem;">新增调料</Button>
					</li>
				</ul>
				
				<h3 style="text-align: center;margin: 1rem;">指定调味品</h3>
				<div style="margin-top: 1rem;">
    					<!--<span style="margin-left: 5rem;margin-top: 1rem;">配料推荐:</span>-->
    					<!--<Select v-model="ingredientsOne" multiple style="width:12rem;margin-left:1rem;" placeholder='配料推荐'>
				      <Option  v-for="item in ingredientsList" :value="item.FlavourRecId" :key="item.FlavourRecId">{{ item.FlavourName }}</Option>
				    </Select>-->
				    <div style="width: 70%;margin: 0 auto;padding: 1rem 0;">
				    		<CheckboxGroup v-model="ingredientsOne" >
					        <Checkbox  v-for="item in ingredientsOneList" :label="item.FlavourRecId" :key="item.FlavourRecId" style='margin-top: 0.5rem;width: 8rem;margin-left: 1.5rem;'>
				        			<span style=""> {{ item.FlavourName }}</span>
				          		 <!--<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;display: block;"  placeholder="重量"></Input-number>	-->
				          		 <Input v-model="item.Unit" placeholder="用量" style="width: 70px;display: block;"></Input>
					        </Checkbox>
					    </CheckboxGroup>
					    <!--<ul>
					    		<li v-for='item in LocalDishreCommend' style="margin-top: 1rem;">
					    			<Input v-model='item.FlavourName'  placeholder="调料" style="width: 100px;margin-left: 1rem;"></Input>
					    			<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"  placeholder="重量"></Input-number>
					    			<Input v-model='item.FlavourRecId'   placeholder="重量" style="width: 30px;margin-left: 0.5rem;"></Input>
					    			<span>g</span>
					    		</li>
					    		
					    </ul>-->
				    </div>
    				</div>
				
				<h2>烹饪步骤</h2>
				<ul style="padding-left: 0.9rem;" class="steps">
					<li v-for='item in step1' style="margin-top: 0.5rem;list-style-type: decimal;">
						<Input v-model='item.StepName' placeholder="请输入" type="textarea" style="width: 210px;"></Input>
						<a @click='removeStep1(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
						<Button type="primary" @click="addStep1" style="margin-left:5.7rem;margin-top:0.5rem;">新增步骤</Button>
				</ul>

			</div>
			<div class="InnovativeDishes">
				<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">创新菜</h2>
				<!--<img  src="../../../../../static/image/placeholderBag.png" style="width: 15rem;display: block;margin: 1rem auto;"/>-->
				<Upload
			        ref="upload"
			        :show-upload-list="false"
			        :on-success="handleInnovativeDishImageSuccess"
			        :format="['jpg','jpeg','png']"
			        :max-size="2048"
			        :before-upload='handleBeforeUpload'
			        :on-format-error="handleFormatError"
			        :on-exceeded-size="handleMaxSize"
			        :multiple=false
			        type="drag"
			        :action=UPImageCook
			        style="width: 8rem;display: block;margin: 1rem auto;" >
			        <img :src="InnovativeDish" style="display: block;margin:0 auto;width: 8rem;"/>
			    </Upload>
				<div>
					<span style="margin-left: 7rem;">菜品名:</span>
					<Input v-model="InnovativeDisheName" placeholder="请输入本地菜名" style="width: 150px;margin-left: 1rem;"></Input>
				</div>
				<h2>菜品故事</h2>
				<Input v-model="InnovativeDisheStory" type="textarea" :autosize='{minRows: 3,maxRows: 10}' placeholder="请输入..." style="margin: 1rem auto;width: 28rem;"></Input>
				<h2>食材明细</h2>
				<h3 style="text-align: center;">主料</h3>
				<ul class="ingredients">
					<li v-for='item in Main_ingredientList2' style="margin-top: 0.5rem;">
						<Input v-model='item.Material' placeholder="主料" style="width: 140px;"></Input>
						<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<a @click='removeMain2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addMainingredient2" style="margin-left:8.4rem;margin-top:0.5rem;">新增主料</Button>
					</li>
				</ul>
				<h3 style="text-align: center;">辅料</h3>
				<ul class="ingredients">
					<li v-for='item in Accessories_List2' style="margin-top: 0.5rem;">
						<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
						<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
						<a @click='removeAccessories2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addAccessories2" style="margin-left:8.4rem;margin-top:0.5rem;">新增辅料</Button>
					</li>
				</ul>
				<h3 style="text-align: center;">调料</h3>
				<ul class="ingredients">
					<li v-for='item in seasoningList2' style="margin-top: 0.5rem;">
						<Input v-model='item.Material' placeholder="调料" style="width: 140px;"></Input>
						<Input-number :max="100000" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
						<a @click='removeSeasoning2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addseasoning2" style="margin-left:5.7rem;margin-top:0.5rem;">新增调料</Button>
					</li>
				</ul>
				<h3 style="text-align: center;">指定调味品</h3>
				<div style="margin-top: 1rem;">
    					<!--<span style="margin-left: 5rem;margin-top: 1rem;">配料推荐:</span>-->
    					<!--<Select v-model="ingredientsOne" multiple style="width:12rem;margin-left:1rem;" placeholder='配料推荐'>
				      <Option  v-for="item in ingredientsList" :value="item.FlavourRecId" :key="item.FlavourRecId">{{ item.FlavourName }}</Option>
				    </Select>-->
				    <div style="width: 70%;margin: 0 auto;padding: 1rem 0;">
				    		<CheckboxGroup v-model="ingredientsTwo" >
					        <Checkbox  v-for="item in ingredientsTwoList" :label="item.FlavourRecId" :key="item.FlavourRecId" style='margin-top: 0.5rem;width: 8rem;margin-left: 1.5rem;'>
				        			<span style=""> {{ item.FlavourName }}</span>
				          		 <!--<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;display: block;"  placeholder="重量"></Input-number>	-->
				          		 <Input v-model="item.Unit" placeholder="用量" style="width: 70px;display: block;"></Input>
					        </Checkbox>
					    </CheckboxGroup>
					    <!--<ul>
					    		<li v-for='item in LocalDishreCommend' style="margin-top: 1rem;">
					    			<Input v-model='item.FlavourName'  placeholder="调料" style="width: 100px;margin-left: 1rem;"></Input>
					    			<Input-number :max="10000" :min="0" v-model='item.Unit' style="width: 70px;"  placeholder="重量"></Input-number>
					    			<Input v-model='item.FlavourRecId'   placeholder="重量" style="width: 30px;margin-left: 0.5rem;"></Input>
					    			<span>g</span>
					    		</li>
					    		
					    </ul>-->
				    </div>
    				</div>
				
				<h2>烹饪步骤</h2>
				<ul style="padding-left: 0.9rem;" class="steps">
					<li v-for='(item,index ) in step2' style="margin-top: 0.5rem;list-style-type: decimal;">
						<span>{{index+1}}</span>
						<Input v-model='item.StepName' placeholder="请输入" type="textarea" style="width: 210px;margin-left: 1rem;"></Input>
						<a @click='removeStep2(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
						<Button type="primary" @click="addStep2" style="margin-left:5.7rem;margin-top:0.5rem;">新增步骤</Button>
				</ul>
	
			</div>
		</div>
		<Button type="primary" @click='redact' style="margin-left: 55rem;margin-top: 2rem;">保存</Button>
	
	</div>
</template>

<script>
	export default{
		data(){
			return{
				UPImageMember:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Member',
				UPImageCook:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Cook',
				SaveSuccess:true,//默认保存成功
				ChefId:'',//厨师id
				ChefActivityId:'',//活动id
				name:'',//姓名
				ApplyType:'',//途径
				Birthday:'',//出生日期
				ReferrerName:'',//推荐人
				division:'',//赛区
				ChefHotelName:'',//酒店
				ChefPicUrl:'../../../../../static/image/HeadPortrait.png',//个人默认照片
				ChefHotelPicUrl:'../../../../../static/image/HeadPortrait.png',//酒店认照片
				ApplyTypeList:[
					{
                        value: '自我推荐',
                        label: '自我推荐'
                    },
                    {
                        value: '行业大师推荐',
                        label: '行业大师推荐'
                    },
                    {
                        value: '酒店推荐',
                        label: '酒店推荐'
                    },
                     {
                        value: '欣和业务推荐',
                        label: '欣和业务推荐'
                    },
				],
				divisionList:[//赛区
//					{
//                      value: '烟台',
//                      label: '烟台'
//                  },
					
				],
				ingredientsList:[//调料库数据
//					{
//						FlavourRecId:'1',//FlavourRecId
//						FlavourName:'味达美',//FlavourName
//					},
				],
				//================本地菜
				localDishId:'',//本地菜id
				localDishImg:'../../../../../static/image/placeholderBag.png',//本地菜照片
				localDishName:'',//本地菜名字
				localStory:'',//本地菜品故事
				ingredientsOne:[],//推荐配料  存放已选推荐配料的id
				ingredientsOneList:[],//所有配料
				Main_ingredientList1:[//本地菜主料
//					{	
//						Material:'西红柿',
//		         		Unit:0,
//		         		MaterialType:'1'
//					},
				],
				Accessories_List1:[//辅料
//					{	
//						Material:'盐',
//		         		Unit:0,
//		         		MaterialType:'2'
//					},
					
				],
				seasoningList1:[//本地菜调料
//					{	
//						Material:'葱伴侣',
//	         			Unit:0,
//	         			MaterialType:'3'
//	         		},
				],
				step1:[//本地菜步骤
//					{
//						StepName:'准备好食材,新鲜牛腩一块、番茄4个、姜一块、葱一根'
//					},
					
				],
				//================创新菜
				InnovativeDishId:'',//创新菜id
				InnovativeDish:'../../../../../static/image/placeholderBag.png',
				InnovativeDisheName:'',//创新菜菜名
				InnovativeDisheStory:'',//创新菜菜品故事
				ingredientsTwo:[],//创新菜推荐配料
				ingredientsTwoList:[],//所有配料
				Main_ingredientList2:[//创新菜
//					{	
//						Material:'西红柿',
//		         		Unit:0,
//		         		MaterialType:'1'
//					},
				
				],
				Accessories_List2:[//辅料
//					{	
//						Material:'盐',
//		         		Unit:0,
//		         		MaterialType:'2'
//					},
				],
				seasoningList2:[//菜调料
//					{	
//						Material:'葱伴侣',
//	         			Unit:0,
//	         			MaterialType:'3'
//	         		},
				],
				step2:[//创新菜步骤
//					{
//						StepId:'1'
//						StepName:'准备好食材,新鲜牛腩一块、番茄4个、姜一块、葱一根'
//					},
				],
			}
		},
		methods:{
			redact(){//编辑
				//console.log(this.Birthday)
				var self = this
				if ($('.dataTime input').val().indexOf('(CST)') && $('.dataTime input').val()) {//更改时出生日期时
 					self.Birthday=$('.dataTime input').val();//开始时间
				}else{
					this.$Message.warning('请选择生日');
				}
				if (self.ApplyType =='自我推荐') {
					self.ReferrerName = ''
				}
				console.log(this.ingredientsOne)
				this.basicInformationPreservation()//基本信息保存
				this.foodInformationPreservation(1)//创新菜保存
				this.foodInformationPreservation(2)//本地菜保存
				if (this.SaveSuccess) {//基本信息  菜  数去全都保存成功
					this.$Message.success('数据保存完成');
				} else{
					this.$Message.error('数据保存异常');
				}
			},
			handlePersonImageSuccess(res, file){//个人照片上传
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.ChefPicUrl = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
	         },
	           handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
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
            handleHotelImageSuccess(res, file){//酒店照片上传
            		$('.loding').hide()
	             console.log(file)
	             var data = JSON.parse(res);
	             this.ChefHotelPicUrl = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
            },
            getChefInformation(){//获取厨师基本信息
				var self =this
				$.ajax({//获取厨师基本信息
			        type :"get",
			        url :URLHeader.ECActivities+"/api/DishChef/GetChefByChefId?chefId="+self.ChefId,
			        dataType : 'json',
			        success : function(json) {
			        	 var dataAll = JSON.parse(json)[0];
			        
			        
					self.name=dataAll.MemberName//姓名
					self.ApplyType=dataAll.ApplyType//途径
					self.ReferrerName=dataAll.ReferrerName//推荐人
					self.ChefHotelName=dataAll.ChefHotelName//酒店
					self.Birthday = dataAll.Birthday//出生日期
					self.ChefPicUrl=dataAll.ChefPicUrl//厨师头像
					self.ChefHotelPicUrl=dataAll.ChefHotelPicUrl//厨师头像
					self.division = dataAll.MatchRegionId//赛区id
					 self.ChefActivityId=dataAll.ChefActivityId//活动id
			        	 self.getDivisionList()//赛区  获取所有赛区
			        },
			        error : function(error) { 
						
			        }
	   		    });
			},
			getChefDish(dishType){//获取菜品信息
				var self =this
				var DATA =URLHeader.ECActivities+"/api/DishChef/GetAuditDishChef?chefId="+self.ChefId+"&dishType="+dishType
				//console.log(DATA)
				$.ajax({//获取厨师菜品
			        type :"get",
			        url :DATA,
			        dataType : 'json',
			        success : function(json) {
			        		
			        		if (json!='No') {
			        			var dataAll = JSON.parse(json).dish;
				        	 	var FlavourRec = JSON.parse(json)//
				        	 	self.ingredientsList = FlavourRec.FlavourRec//获取所有厨师推荐调料  
				        	 	if (dishType=='1') {//创新菜
				        	 		//console.log(FlavourRec)
				        	 		for (var i = 0;i<FlavourRec.FlavourRec.length;i++) {
				        	 			FlavourRec.FlavourRec[i].Unit = 0
				        	 			self.ingredientsTwoList.push(FlavourRec.FlavourRec[i])
				        	 			self.ingredientsOneList.push(FlavourRec.FlavourRec[i])
				        	 			for (var y = 0;y<FlavourRec.select.length;y++) {
				        	 				if (FlavourRec.select[y].FlavourRecId==FlavourRec.FlavourRec[i].FlavourRecId) {//已选的
				        	 					FlavourRec.FlavourRec[i].Unit = parseInt(FlavourRec.select[y].Unit)
				        	 					self.ingredientsTwo.push(FlavourRec.FlavourRec[i].FlavourRecId)
				        	 				}
				        	 			}
				        	 		}
				        	 		console.log(self.ingredientsTwoList)
				        	 		self.parsingInnovativeDish(dataAll)
				        	 	} else{//本地菜
				        	 		//console.log(dataAll)
				        	 		//ingredientsOneList
				        	 		for (var i = 0;i<FlavourRec.FlavourRec.length;i++) {
				        	 			FlavourRec.FlavourRec[i].Unit = 0
				        	 			self.ingredientsTwoList.push(FlavourRec.FlavourRec[i])
				        	 			self.ingredientsOneList.push(FlavourRec.FlavourRec[i])
				        	 			for (var y = 0;y<FlavourRec.select.length;y++) {
				        	 				if (FlavourRec.select[y].FlavourRecId==FlavourRec.FlavourRec[i].FlavourRecId) {//已选的
				        	 					FlavourRec.FlavourRec[i].Unit = parseInt(FlavourRec.select[y].Unit)
				        	 					self.ingredientsOne.push(FlavourRec.FlavourRec[i].FlavourRecId)
				        	 				}
				        	 			}
				        	 		}
				        	 		//console.log(self.ingredientsOneList)
	//			        	 		console.log(self.ingredientsOne)
				        	 		self.parsingLocalDish(dataAll)
				        	 	}
			        		} else{
			        			if (dishType=='1') {//创新菜
			        				self.InnovativeDishId = '0'
			        			} else{//本地菜
			        				self.localDishId ='0'
			        			}
			        			//console.log(DATA)
			        		}
			        	  	//console.log(dataAll)
			        },
			        error : function(error) { 
						
			        }
	   		    });				
			},
            //===============================================本地菜===============================================
            parsingLocalDish(data){//本地菜解析
//          		console.log(data)
            		var self =this
            		this.localDishId = data.Basic.DishChefId//菜品id
            		this.localDishImg = data.Basic.DishPicUrl//菜品图片
            		this.localStory = data.Basic.DishStory//菜品故事
            		this.localDishName = data.Basic.DishName//菜品名字

            		//=====================食材信息=====================
            		var DishMaterial = data.DishMaterial
            		for (var i = 0;i<DishMaterial.length;i++) {//parseInt
            			//console.log(5)
            			DishMaterial[i].Unit=parseInt(DishMaterial[i].Unit)
	        	 		if (DishMaterial[i].MaterialType=='1') {//主料
	        	 			self.Main_ingredientList1.push(data.DishMaterial[i])
	        	 		}
	        	 		if (DishMaterial[i].MaterialType=='2') {//辅料
	        	 			self.Accessories_List1.push(DishMaterial[i])
	        	 		}
	        	 		if (DishMaterial[i].MaterialType=='3') {//调料
	        	 			self.seasoningList1.push(DishMaterial[i])
	        	 		}
//	        	 		if (DishMaterial[i].MaterialType=='4') {//指定调料
//	        	 			self.ingredientsOne.push(DishMaterial[i])
//	        	 		}
	        	 	}
            		//=================推荐调料=======================

            		
            		//===================步骤========================
            		
            		//step2
            		var DishStep = data.DishStep
            		for (var t = 0;t<DishStep.length;t++) {
            			self.step1.push(DishStep[t])
            		}
            		
            		
            		
            },
            handleLocalDishImageSuccess(res, file){//本地菜
            		$('.loding').hide()
	             console.log(file)
	             var data = JSON.parse(res);
	             this.localDishImg = URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
            },
            addMainingredient1(){//本地菜主料
            		var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1',
	         		DishId:this.localDishId
	         	}
	         	this.Main_ingredientList1.push(item);
	         	console.log(this.Main_ingredientList1)
            },
            removeMain1(item){//本地菜主料删除
            		this.Main_ingredientList1.splice(this.Main_ingredientList1.indexOf(item), 1)
            },
            addAccessories1(){//本地菜辅料
            		var self = this;
				var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'2',
	         		DishId:this.localDishId
	         	}
	         	this.Accessories_List1.push(item);
            },
            removeAccessories1(item){//本地菜辅料删除
            		this.Accessories_List1.splice(this.Accessories_List1.indexOf(item), 1)
            },
            addseasoning1(){//本地菜新增调料
            		var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'3',
	         		DishId:this.localDishId
	         	}
	         	this.seasoningList1.push(item);
            },
            removeSeasoning1(item){//本地菜调料删除
            		this.seasoningList1.splice(this.seasoningList1.indexOf(item), 1)
            },
			addStep1(){//本地菜新增步骤
				var item = {
	         		StepName:'',
	         		DishId:this.localDishId
	         	}
	         	this.step1.push(item);
			},
			 removeStep1(item){//本地菜删除步骤
				this.step1.splice(this.step1.indexOf(item), 1)
			},
             //===============================================创新菜===============================================
             parsingInnovativeDish(data){//解析创新菜
             	var self =this
             	this.InnovativeDishId = data.Basic.DishChefId//菜品id
            		this.InnovativeDish = data.Basic.DishPicUrl//菜品图片
            		this.InnovativeDisheStory = data.Basic.DishStory//菜品故事
            		this.InnovativeDisheName = data.Basic.DishName//菜品名字
            		//=====================食材信息=====================
            		var DishMaterial = data.DishMaterial
            		for (var i = 0;i<DishMaterial.length;i++) {//parseInt
            			//console.log(5)
            			DishMaterial[i].Unit=parseInt(DishMaterial[i].Unit)
	        	 		if (DishMaterial[i].MaterialType=='1') {//主料
	        	 			self.Main_ingredientList2.push(data.DishMaterial[i])
	        	 		}
	        	 		if (DishMaterial[i].MaterialType=='2') {//辅料
	        	 			self.Accessories_List2.push(DishMaterial[i])
	        	 		}
	        	 		if (DishMaterial[i].MaterialType=='3') {//调料
	        	 			self.seasoningList2.push(DishMaterial[i])
	        	 		}
	        	 		if (DishMaterial[i].MaterialType=='4') {//指定调料
	        	 			self.ingredientsTwo.push(DishMaterial[i])
	        	 		}
	        	 	}
            		//===================步骤========================
            		//step2
            		var DishStep = data.DishStep
            		for (var t = 0;t<DishStep.length;t++) {
            			self.step2.push(DishStep[t])
            		}
            		//console.log(DishMaterial)
             },
            handleInnovativeDishImageSuccess(res, file){//创新菜
            		$('.loding').hide()
	             console.log(file)
	             var data = JSON.parse(res);
	             this.InnovativeDish =URLHeader.Loading+ '/Cook/'+data.Id+'.'+data.Type;
            },
            addMainingredient2(){//创新菜新增主料
            		var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1',
	         		DishId:this.InnovativeDishId
	         	}
	         	this.Main_ingredientList2.push(item);
	         	console.log(this.Main_ingredientList2)
            },
            removeMain2(item){//创新菜主料删除
            		this.Main_ingredientList2.splice(this.Main_ingredientList2.indexOf(item), 1)
            },
            addAccessories2(){//创新菜辅料
            		var self = this;
				var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'2',
	         		DishId:this.InnovativeDishId
	         	}
	         	this.Accessories_List2.push(item);
            },
            removeAccessories2(item){//创新菜辅料删除
            		this.Accessories_List2.splice(this.Accessories_List2.indexOf(item), 1)
            },
	        addseasoning2(){//创新菜新增调料
            		var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'3',
	         		DishId:this.InnovativeDishId
	         	}
	         	this.seasoningList2.push(item);
            },
            removeSeasoning2(item){//创新草调料删除
            		this.seasoningList2.splice(this.seasoningList2.indexOf(item), 1)
            },
            addStep2(){//创新菜菜新增步骤
				var item = {
	         		StepName:'',
	         		DishId:this.InnovativeDishId
	         	}
	         	this.step2.push(item);
			},
			 removeStep2(item){//创新菜删除步骤
				this.step2.splice(this.step2.indexOf(item), 1)
			},
			ApplyTypeChange(){//监听途径   当自我推荐时  推荐人隐藏
				if (this.ApplyType =='自我推荐') {
					//console.log('自我推荐')
					$('.BasicInformation ul .Referrer').hide()
				} else{
					//console.log('其它')
					$('.BasicInformation ul .Referrer').show()
				}
			},
			basicInformationPreservation(){//基本信息保存
				//URLHeader.ECActivities+/api/DishChef/UpdateChef
				var self= this
				
				var DATA = '{"ChefId":"'+self.ChefId+'","MatchRegionId":"'+self.division+'","ApplyType":"'+self.ApplyType+'","ReferrerName":"'+self.ReferrerName+'","Birthday":"'+self.Birthday+'","ChefHotelPicUrl":"'+self.ChefHotelPicUrl+'","ChefPicUrl":"'+self.ChefPicUrl+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/DishChef/UpdateChef',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						//var dataAll = JSON.parse(json);
						if (json==='OK') {
							//SaveSuccess
							self.SaveSuccess = true
						} else{
							self.SaveSuccess = false//基本信息保存错误
							self.$Notice.error({
			                    title: '基本信息保存错误',
			                    desc: '基本信息中数据保存错误,请重新保存',
			                     duration: 4
			                });
						}
						console.log("基本信息保存"+json)
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('基本信息数据保存异常,请刷新页面重新保存');
					}
				});
			},
			foodInformationPreservation(dishType){//菜品保存
				var self =this
				var DATA;
				var arr=[];
				if(dishType=='1'){//创新菜保存
					var ingredientsLocal =[]
					for (var i = 0;i<self.ingredientsTwoList.length;i++) {
						for (var y = 0;y<self.ingredientsTwo.length;y++) {
							if (self.ingredientsTwoList[i].FlavourRecId==self.ingredientsTwo[y]) {
								delete self.ingredientsTwoList[i].ChefActivityId
								delete self.ingredientsTwoList[i].FlavourName
								delete self.ingredientsTwoList[i].FlavourPicUrl
								ingredientsLocal.push(self.ingredientsTwoList[i])
							}
						}
					}
					//console.log(ingredientsLocal)
					arr = self.Main_ingredientList2.concat(self.Accessories_List2,self.seasoningList2);
					var dishMaterial = JSON.stringify(arr)
					ingredientsLocal = JSON.stringify(ingredientsLocal)
					var DishStep = JSON.stringify(self.step2)
					self.InnovativeDisheStory=self.InnovativeDisheStory.replace("\"", "\\\'"); 
					DATA ='{"dishChef":{"ChefId":"'+self.ChefId+'","DishType":"1","DishChefId":"'+self.InnovativeDishId+'","DishStory":"'+self.InnovativeDisheStory+'","DishPicUrl":"'+self.InnovativeDish+'","DishName":"'+self.InnovativeDisheName+'"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"FlavourRecRole":'+ingredientsLocal+'}'
					console.log(DATA)
				}else{
					var ingredientsLocal =[]
					for (var i = 0;i<self.ingredientsOneList.length;i++) {
						for (var y = 0;y<self.ingredientsOne.length;y++) {
							if (self.ingredientsOneList[i].FlavourRecId==self.ingredientsOne[y]) {
								delete self.ingredientsOneList[i].ChefActivityId
								delete self.ingredientsOneList[i].FlavourName
								delete self.ingredientsOneList[i].FlavourPicUrl
								ingredientsLocal.push(self.ingredientsOneList[i])
							}
						}
					}
					arr = self.Main_ingredientList1.concat(self.Accessories_List1,self.seasoningList1);
					var dishMaterial = JSON.stringify(arr)
					var DishStep = JSON.stringify(self.step1)
					ingredientsLocal = JSON.stringify(ingredientsLocal)
					self.localStory=self.localStory.replace("\"", "\\\'"); 
					DATA ='{"dishChef":{"ChefId":"'+self.ChefId+'","DishType":"2","DishChefId":"'+self.localDishId+'","DishStory":"'+self.localStory+'","DishPicUrl":"'+self.localDishImg+'","DishName":"'+self.localDishName+'"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"FlavourRecRole":'+ingredientsLocal+'}'
				}
				//console.log(DATA)
				
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/DishChef/UpdateDishChef',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						if(dishType=='1'){//创新菜
							//self.$Message.success('数据保存成功');
							if (json==='OK') {
								//SaveSuccess
								console.log('创新菜'+json)
								self.SaveSuccess = true
							} else{
								self.SaveSuccess = false//基本信息保存错误
								self.$Notice.error({
				                    title: '创新菜保存错误',
				                    desc: '创新菜数据保存错误,请重新保存',
				                     duration: 4
				                    
				                });
							}
						}else{//本地菜
							if (json==='OK') {
								//SaveSuccess
								self.SaveSuccess = true
								console.log('本地菜'+json)
							} else{
								self.SaveSuccess = false//基本信息保存错误
								self.$Notice.error({
				                    title: '本地菜保存错误',
				                    desc: '本地菜数据保存错误,请重新保存',
				                     duration: 4
				                    
				                });
							}
						}
						
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('菜品数据保存异常,请刷新页面重新保存');
					}
				});	
			},
			getDivisionList(){//获取赛区
				var self= this
				//console.log("活动ID"+self.ChefActivityId)
				$.ajax({//所有赛区
			        type :"get",
			        url :URLHeader.ECActivities+"/api/DishChef/GetAllByChefActivityId?caId="+self.ChefActivityId,
			        dataType : 'json',
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 for (var i = 0;i<dataAll.length;i++) {
			        	 	var dataLi = {}
			        	 	dataLi.value = dataAll[i].MatchRegionId
			        	 	dataLi.label = dataAll[i].AreaName
			        	 	self.divisionList.push(dataLi)
			        	 }
			        	// console.log(self.divisionList)
			        },
			        error : function(error) { 
						
			        }
	   		    });
	   		    
	   		     $.ajax({//所有指定调味品
			        type :"get",
			        url :URLHeader.ECActivities+"/api/DishChef/GetFRChef?caId="+self.ChefActivityId,
			        dataType : 'json',
			        success : function(json) {
			        		var dataAll = JSON.parse(json);
			       	 	self.ingredientsOneList =JSON.parse(json)
			        		self.ingredientsTwoList =JSON.parse(json)
			        		 console.log(self.ingredientsList)
			        },
			        error : function(error) { 
						
			        }
	   		    });
			}
		},
		watch: {  
        		'ApplyType': 'ApplyTypeChange', //监听途径值 
   	 	},
		mounted:function(){
			var self =this
			var hash= window.location.hash.split('?')[1].split('=')[1];//获取厨师id
			this.ChefId = hash
			$('.loding').hide()

			this.getChefInformation()//获取厨师基本信息
			
			this.getChefDish(2)//获取本地菜
			this.getChefDish(1)//获取创新菜
		}
	}
</script>

<style scoped>
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
@keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }
.enrolEdit{
	border: 1px solid gainsboro;
	border-radius: 8px;
	padding: 2rem;
	padding-top: 1rem;
}
.enrolEdit .BasicInformation{
	border-bottom: 1px dashed gainsboro;
}
.enrolEdit .BasicInformation ul{
	/*margin-top: 1rem;
	overflow: auto;
	width: 30rem;
	float: left;
	margin-left: 2rem;*/
	display: block;
	float: left;
	width:18rem;
}
.enrolEdit .BasicInformation ul .informationLi {
	display: block;
	height: 3rem;
	line-height: 3rem;
	width: 13rem;
	float: left;
	margin-left: 2rem;
}
.enrolEdit .BasicInformation ul .informationLi .labie{
	display: block;
	float: left;
}
.enrolEdit .BasicInformation ul .informationLi .inputInformation{
	
	margin-right: 1.5rem;
	display: block;
	float: right;
}
/*.enrolEdit .BasicInformation ul li span{
	font-size: 0.9rem;
	color: grey;
}*/
.enrolEdit .BasicInformation ul .informationLi .labie{
	color: grey;
}
.enrolEdit .BasicInformation ul .informationLi .value{
	margin-left: 1rem;
	color: black;
}
.enrolEdit .BasicInformation .right{
	float: left;
	margin-left: 3rem;
}
/**/
.enrolEdit .dish{
	overflow: auto;
}
.enrolEdit .dish .localDish, .enrolEdit .dish .InnovativeDishes{
	float: left;
	width: 29.5rem;
}
.enrolEdit .dish .localDish{
	border-right: 1px solid gainsboro;
}
.enrolEdit .dish .ingredients{
	overflow: auto;
}
.enrolEdit .dish .InnovativeDishes{
	margin-left: 0.5rem;
}
.enrolEdit .dish .ingredients li{
	display: block;
	height: 2rem;
	width: 15rem;
	margin: 0.5rem auto;
	line-height: 2rem;
}
/*.enrolEdit .dish .ingredients li .labie{
	display: block;
	float: left;
}
.enrolEdit .dish .ingredients li .value{
	display: block;
	float: right;
}*/
.enrolEdit .dish .steps{
	margin-top: 1rem;
}
.enrolEdit .dish .steps li{
	margin-top: 0.5rem;
	padding-left: 20px;
	width: 25rem;
	margin-left: 2rem;
	display: block;
	/*list-style:square*/
}
</style>