<template>
	<div class="auditAdd">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h1 style="margin: 0 auto;padding-bottom: 1rem;border: 1px dashed gray;text-align: center;height: 2.5rem;border-radius: 4px;line-height: 2.5rem; width: 15rem;">报名厨师信息新增</h1>
		<div class="BasicInformation">
			<h2 style="border-bottom: 1px solid black;width: 6rem;padding:0 0.5rem;padding-left: 0.8rem;">基本信息</h2>
			<div style="overflow: auto;">
				<ul>
					<li class="informationLi">
						<span class="labie">姓名</span>
						<!--<span class="value">张三</span>-->
						<!--<Input v-model="name" class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
						<!--<span style="margin-left: 2rem;">{{name}}</span>-->
						<Input v-model="name" disabled placeholder="厨师姓名"  style="width: 128px;margin-left: 2rem;"></Input>
					</li>
					
					<li class="informationLi">
						<span class="labie">出生日期</span>
						<!--<Input v-model="Birthday"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
						<DatePicker type="date" placeholder="选择日期" v-model="Birthday" style="width: 128px;margin-left: 0.6rem;" class='dataTime'></DatePicker>
					</li>
					
					<li class="informationLi">
						<span class="labie">赛区</span>
						<Select v-model="division" style="width:130px;margin-left: 2.1rem;">
					        <Option v-for="item in divisionList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
						<!--<Input v-model="division"  class="inputInformation" placeholder="请输入..." style="width: 130px;"></Input>-->
					</li>
				</ul>
				<ul>
					<li class="informationLi"style="width: 15rem;">
						<span class="labie">会员ID</span>
						<Input v-model="MemberId" placeholder="请输入会员ID"  style="width: 128px;margin-left: 1.1rem;"></Input>
						<Button type="primary"  size="small" @click='searchMember'>查询</Button>
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
					<Input v-model="InnovativeDisheName" placeholder="请输入创新菜名" style="width: 150px;margin-left: 1rem;"></Input>
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
				ISMember:false,//非会员  查询时用
				
				UPImageMember:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Member',
				UPImageCook:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Cook',
				SaveSuccess:true,//默认保存成功
				ChefId:'',//厨师id
				ChefActivityId:'',//活动id
				//
				name:'',//姓名
				MemberId:'',//会员ID
				ApplyType:'',//途径
				Birthday:'',//出生日期
				ReferrerName:'',//推荐人
				division:'',//赛区
				ChefPicUrl:'../../../../../static/image/HeadPortrait.png',//个人默认照片
				ChefHotelPicUrl:'../../../../../static/image/HeadPortrait.png',//酒店认照片
				//
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
				if ($('.dataTime input').val()) {//更改时出生日期时
   					self.Birthday=$('.dataTime input').val();//开始时间
				}else{
					this.$Message.warning('请选择生日');
				}
				if (self.ISMember) {//会员
					//基本信息保存
					self.basicInformationPreservation()
				} else{//非会员
					this.$Message.warning('此人不是公司会员');
				}
				
				
				
				
				
				
				//console.log(this.ingredientsOne)
				//this.searchMember()
				//this.basicInformationPreservation()//基本信息保存
//				this.foodInformationPreservation(1)//创新菜保存
//				this.foodInformationPreservation(2)//本地菜保存
//				if (this.SaveSuccess) {//基本信息  菜  数去全都保存成功
//					this.$Message.success('数据保存完成');
//				} else{
//					this.$Message.error('数据保存异常');
//				}
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

            //===============================================本地菜===============================================

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
	         		//DishId:this.localDishId
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
	         		//DishId:this.localDishId
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
	         		//DishId:this.localDishId
	         	}
	         	this.seasoningList1.push(item);
            },
            removeSeasoning1(item){//本地菜调料删除
            		this.seasoningList1.splice(this.seasoningList1.indexOf(item), 1)
            },
			addStep1(){//本地菜新增步骤
				var item = {
	         		StepName:'',
	         		//DishId:this.localDishId
	         	}
	         	this.step1.push(item);
			},
			 removeStep1(item){//本地菜删除步骤
				this.step1.splice(this.step1.indexOf(item), 1)
			},
             //===============================================创新菜===============================================

            handleInnovativeDishImageSuccess(res, file){//创新菜
            		$('.loding').hide()
	             console.log(file)
	             var data = JSON.parse(res);
	             this.InnovativeDish = URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
	            // console.log( this.InnovativeDish)
            },
            addMainingredient2(){//创新菜新增主料
            		var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1',
	         		//DishId:this.InnovativeDishId
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
	         		//DishId:this.InnovativeDishId
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
	         		//DishId:this.InnovativeDishId
	         	}
	         	this.seasoningList2.push(item);
            },
            removeSeasoning2(item){//创新草调料删除
            		this.seasoningList2.splice(this.seasoningList2.indexOf(item), 1)
            },
            addStep2(){//创新菜菜新增步骤
				var item = {
	         		StepName:'',
	         		//DishId:this.InnovativeDishId
	         	}
	         	this.step2.push(item);
			},
			 removeStep2(item){//创新菜删除步骤
				this.step2.splice(this.step2.indexOf(item), 1)
			},
			ApplyTypeChange(){//监听途径   当自我推荐时  推荐人隐藏
				if (this.ApplyType =='自我推荐') {
					//console.log('自我推荐')
					this.ReferrerName = ''
					$('.BasicInformation ul .Referrer').hide()
				} else{
					//console.log('其它')
					$('.BasicInformation ul .Referrer').show()
				}
			},
			searchMember(){//新增时先    查询是否为会员
				var self = this
//				name:'',//姓名
//				MemberId:'',//会员ID
//				self.MemberId = parseInt(+self.MemberId)
				var  MemberId = ''
				if (self.MemberId=='') {
					MemberId = '0'
				}else{
					MemberId = self.MemberId
				}
				var DATA = '{"Name":"","MemberId":"'+MemberId+'"}'
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Chef/GetMemberInfo',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						console.log(json)
						if (json=='No') {
							self.$Message.error({
				                content: '不是会员,请确认填写正确',
				                duration: 4
				            });
				            self.ISMember = false  //不是会员
						} else{
							 var dataAll = JSON.parse(json)[0];
							self.MemberId = dataAll.MemberId
							self.name = dataAll.MemberName
							  self.ISMember = true  //是会员
						}
					},
					error:function(error){
						console.log(error)
						self.$Message.error({
				                content: '确认会员查询错误',
				                duration: 4
				        });
					}
				});
			},
			basicInformationPreservation(){//基本信息保存
				var self= this
				//var DATA = '{"ChefId":"'+self.ChefId+'","MatchRegionId":"'+self.division+'","ApplyType":"'+self.ApplyType+'","ReferrerName":"'+self.ReferrerName+'","Birthday":"'+self.Birthday+'","ChefHotelPicUrl":"'+self.ChefHotelPicUrl+'","ChefPicUrl":"'+self.ChefPicUrl+'"}'
				//酒店照片判断
				var ChefHotelPicUrl = ''
				if (self.ChefHotelPicUrl=='../../../../../static/image/HeadPortrait.png') {
					ChefHotelPicUrl = ''
				} else{
					ChefHotelPicUrl = self.ChefHotelPicUrl
				}
				//个人照片判断
				var ChefPicUrl = ''
				if (self.ChefPicUrl=='../../../../../static/image/HeadPortrait.png') {
					ChefPicUrl = ''
				} else{
					ChefPicUrl = self.ChefPicUrl
				}
				
				
				var DATA ='{"MemberId":"'+self.MemberId+'","ChefActivityId":"'+self.ChefActivityId+'","MatchRegionId":"'+self.division+'","ApplyType":"'+self.ApplyType+'","ReferrerName":"'+self.ReferrerName+'","Birthday":"'+self.Birthday+'","ChefPicUrl":"'+ChefPicUrl+'","ChefHotelPicUrl":"'+ChefHotelPicUrl+'"}'
				console.log(DATA)
				
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Chef/AddChefInfo',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						if (json=='厨师已存在') {//
							self.$Message.error({
				                content: '厨师已经报名存在,请到编辑页编辑!',
				                duration: 4
				            });
				            console.log('已存在')
						}else if (json=='No') {
							self.$Message.error({
				                content: '不是会员,请确认填写正确',
				                duration: 4
				            });
				            console.log('NO')
						}else{
							self.ChefId = JSON.parse(json);
							console.log("厨师ID="+self.ChefId)
							self.foodInformationPreservation(1)//创新菜
							self.foodInformationPreservation(2)//本地菜
							
							console.log('正常')
						}

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
				var DATA='';
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
					console.log(ingredientsLocal)
					arr = self.Main_ingredientList2.concat(self.Accessories_List2,self.seasoningList2);
					var dishMaterial = JSON.stringify(arr)
					ingredientsLocal = JSON.stringify(ingredientsLocal)
					var DishStep = JSON.stringify(self.step2)
					self.InnovativeDisheStory=self.InnovativeDisheStory.replace("\"", "\\\'"); 
					//DATA ='{"dishChef":{"DishChefId":"'+self.InnovativeDishId+'","DishStory":"'+self.InnovativeDisheStory+'","DishPicUrl":"'+self.InnovativeDish+'","DishName":"'+self.InnovativeDisheName+'"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"FlavourRecRole":'+ingredientsLocal+'}'
					var InnovativeDish=''
					if (self.InnovativeDish=='../../../../../static/image/placeholderBag.png') {
						InnovativeDish = ''
					} else{
						InnovativeDish = self.InnovativeDish
					}
					DATA = '{"dishChef":{"ChefId":"'+self.ChefId+'","DishPicUrl":"'+InnovativeDish+'","DishStory":"'+self.InnovativeDisheStory+'","DishName":"'+self.InnovativeDisheName+'","DishType":"1"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"flavourRecRole":'+ingredientsLocal+'}'
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
					//DATA ='{"dishChef":{"DishChefId":"'+self.localDishId+'","DishStory":"'+self.localStory+'","DishPicUrl":"'+self.localDishImg+'","DishName":"'+self.localDishName+'"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"FlavourRecRole":'+ingredientsLocal+'}'
					var localDishImg=''
					if (self.localDishImg=='../../../../../static/image/placeholderBag.png') {
						localDishImg = ''
					} else{
						localDishImg = self.localDishImg
					}
					DATA = '{"dishChef":{"ChefId":"'+self.ChefId+'","DishPicUrl":"'+localDishImg+'","DishStory":"'+self.localStory+'","DishName":"'+self.localDishName+'","DishType":"2"},"dishMaterial":'+dishMaterial+',"dishStep":'+DishStep+',"flavourRecRole":'+ingredientsLocal+'}'
					console.log(DATA)
				}
				
				
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Chef/AddDishChefInfo',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						console.log(json)
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
			getDivisionList(){//获取赛区  指定调味品
				var self= this
				//console.log("活动ID"+self.ChefActivityId)
				$.ajax({
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
	   		     $.ajax({//指定调味品
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
			var hash= window.location.hash.split('?')[1].split('=')[1];//获取活动id
			//this.ChefId = hash
			this.ChefActivityId = hash
			$('.loding').hide()
			this.getDivisionList()//获取所有赛区
			//------------本地菜----------
			this.addMainingredient1()//主料
			this.addAccessories1()//辅料
			this.addseasoning1()//调料
			this.addStep1()//步骤
			//------------创新菜----------
			this.addMainingredient2()//主料
			this.addAccessories2()//辅料
			this.addseasoning2()//调料
			this.addStep2()//步骤
//			this.getChefInformation()//获取厨师基本信息
//			
//			this.getChefDish(2)//获取本地菜
//			this.getChefDish(1)//获取创新菜
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
.auditAdd{
	border: 1px solid gainsboro;
	border-radius: 8px;
	padding: 2rem;
	padding-top: 1rem;
}
.auditAdd .BasicInformation{
	border-bottom: 1px dashed gainsboro;
}
.auditAdd .BasicInformation ul{
	/*margin-top: 1rem;
	overflow: auto;
	width: 30rem;
	float: left;
	margin-left: 2rem;*/
	display: block;
	float: left;
	width:18rem;
}
.auditAdd .BasicInformation ul .informationLi {
	display: block;
	height: 3rem;
	line-height: 3rem;
	width: 13rem;
	float: left;
	margin-left: 2rem;
}
.auditAdd .BasicInformation ul .informationLi .labie{
	display: block;
	float: left;
}
.auditAdd .BasicInformation ul .informationLi .inputInformation{
	
	margin-right: 1.5rem;
	display: block;
	float: right;
}
/*.auditAdd .BasicInformation ul li span{
	font-size: 0.9rem;
	color: grey;
}*/
.auditAdd .BasicInformation ul .informationLi .labie{
	color: grey;
}
.auditAdd .BasicInformation ul .informationLi .value{
	margin-left: 1rem;
	color: black;
}
.auditAdd .BasicInformation .right{
	float: left;
	margin-left: 3rem;
}
/**/
.auditAdd .dish{
	overflow: auto;
}
.auditAdd .dish .localDish, .auditAdd .dish .InnovativeDishes{
	float: left;
	width: 29.5rem;
}
.auditAdd .dish .localDish{
	border-right: 1px solid gainsboro;
}
.auditAdd .dish .ingredients{
	overflow: auto;
}
.auditAdd .dish .InnovativeDishes{
	margin-left: 0.5rem;
}
.auditAdd .dish .ingredients li{
	display: block;
	height: 2rem;
	width: 15rem;
	margin: 0.5rem auto;
	line-height: 2rem;
}
/*.auditAdd .dish .ingredients li .labie{
	display: block;
	float: left;
}
.auditAdd .dish .ingredients li .value{
	display: block;
	float: right;
}*/
.auditAdd .dish .steps{
	margin-top: 1rem;
}
.auditAdd .dish .steps li{
	margin-top: 0.5rem;
	padding-left: 20px;
	width: 25rem;
	margin-left: 2rem;
	display: block;
	/*list-style:square*/
}
</style>