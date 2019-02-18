<template>
	<!--反馈菜编辑-->
	<div class="feedBackEditor">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div style="width: 27rem;padding-bottom: 0.5rem;margin-left: 2rem;float: left;margin-top: 1rem;">
			<Upload
		        ref="upload"
		        :show-upload-list="false"
		        :on-success="dishPictureSuccess"
		        :format="['jpg','jpeg','png']"
		        :max-size="2048"
		        :before-upload='handleBeforeUpload'
		        :on-format-error="handleFormatError"
		        :on-exceeded-size="handleMaxSize"
		        :multiple=false
		        type="drag"
		        :action=UPImageDish
		        style="margin: 0 auto;display:block;width:9rem;padding-bottom: 1rem;" >
		        <img :src="dishInformation.DishPicUrl" style="width: 9rem;"/>
		    </Upload>
		    <div style="width: 15rem;margin: 0 auto;margin-left:5rem;">
		   	 	<span>菜品名称:</span>
		   		 <Input v-model="dishInformation.DishName" placeholder="菜品名" style="width: 160px"></Input>
		    </div>
		    <!--<div style="width: 27rem;margin: 0 auto;margin-top: 1rem;overflow: auto;padding-bottom: 1rem;">
				<editor ref="myTextEditor"
	            :fileName="'myFile'"
	            :canCrop="canCrop"
	            :uploadUrl="UPImageDish"
	            v-model="dishInformation.dishStory"></editor>
			</div>-->
			<div style="width: 27rem;margin: 0 auto;margin-top: 1rem;overflow: auto;padding-bottom: 1rem;">
				<span style="float: left;display: block;margin-right: 1rem;">菜品故事</span>
				<Input style="display: block;float: left;width: 22rem;" v-model="dishInformation.DishStory" type="textarea" :rows="4" placeholder="菜品故事"></Input>
			</div>
		</div>
		<div style="float: left;border-left:1px dashed gainsboro;margin-left: 0.8rem;min-height: 39rem;width: 29rem;">
			<div class="Main_ingredient" style="margin: 0 auto;text-align: center;">
				<h3 style="text-align: center;margin-top: 1rem;">主料</h3>
				<ul>
					<li v-for='(item,index) in Main_ingredientList' style="margin-top: 0.5rem;">
						<span>{{index+1}}</span>
						<Input v-model='item.Material' placeholder="主料" style="width: 140px;"></Input>
						<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
						<a @click='removeMain(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addMainingredient" style="margin-top:0.5rem;">新增主料</Button>
					</li>
				</ul>
			</div>
			<div class="Accessories" style="text-align: center;">
				<h3 style="text-align: center;margin-top: 1rem;">辅料</h3>
				<ul>
					<li v-for='(item,index) in Accessories_List' style="margin-top: 0.5rem;">
						<span>{{index+1}}</span>
						<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
						<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<a @click='removeAccessories(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li>
						<Button type="primary" @click="addAccessories" style="margin-top:0.5rem;">新增辅料</Button>
					</li>
				</ul>
			</div>
			
			<div class="seasoning">
				<h3 style="text-align: center;margin-top: 1rem;">推荐调料</h3>
				<ul>
					<li v-for='(item,index) in seasoningList' style="width: 15rem;margin: 0 auto;margin-top: 0.5rem;">
						<span>{{index+1}}</span>
						<!--<Input v-model='item.Material' placeholder="调料" style="width: 140px;"></Input>-->
						<Select v-model="item.MySelfFRId"  style="width:140px;">
					        <Option v-for="item in seasoningAllList" :value="item.MySelfFRId" :key="item.MySelfFRId"	>{{ item.FRName }}</Option>
					    </Select>
						<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<a @click='removeSeasoning(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li style="margin-top: 0.5rem;width: 15rem;margin: 0 auto;margin-top: 0.5rem;text-align: center;">
						<Button type="primary" @click="addseasoning" style="margin-top:0.5rem;">新增调料</Button>
					</li>
				</ul>
			</div>
			
			<div class="otherSeasoning">
				<h3 style="text-align: center;margin-top: 1rem;">其它调料</h3>
				<ul>
					<li v-for='(item,index) in otherSeasoningList' style="width: 15rem;margin: 0 auto;margin-top: 0.5rem;">
						<span>{{index+1}}</span>
						<Input v-model='item.Material' placeholder="其它调料" style="width: 140px;"></Input>
						<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
						<a @click='removeOtherSeasoning(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
					<li style="margin-top: 0.5rem;width: 15rem;margin: 0 auto;margin-top: 0.5rem;text-align: center;">
						<Button type="primary" @click="addOtherseasoning" style="margin-top:0.5rem;">新增其它调料</Button>
					</li>
				</ul>
			</div>
			
			<div class="Step" style="text-align: center;">
				<h3 style="text-align: center;margin-top: 1rem;">烹饪步骤</h3>
				<ul style="padding-left: 1rem;">
					<li v-for='(item,index) in step' style="margin-top: 0.5rem;">
						<span>{{index+1}}</span>
						<Input v-model='item.StepName' placeholder="请输入" type="textarea" style="width: 210px;"></Input>
						<a @click='removeStep(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
					</li>
						<Button type="primary" @click="addStep" style="margin-top:0.5rem;">新增步骤</Button>
				</ul>
			</div>
		</div>
		<div style="text-align: center;">
			<Button @click="editorBtn" type="error" style="width: 5rem;margin-top: 1rem;margin-bottom: 0.5rem;">编辑</Button>
		</div>
	</div>
</template>

<script>
import editor from '../../../tools/Quilleditor.vue'
	export default{
		data(){
			return{
				MySelfDishId:'',//菜品id
				UPImageDish:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Produce',//图片上传地址
				canCrop:false,//剪切
				dishInformation:{//菜品信息
					DishPicUrl:'../../../../../static/image/HeadPortrait.png',
					DishName:'',//菜品名
					DishStory:'',//菜品故事
				},
				Main_ingredientList:[//主料数据
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'1'
					}
				],
				Accessories_List:[//辅料
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'2'
					}
				],
				seasoningList:[//推荐调料数据
					{	
						MySelfFRId:'',
	         			Unit:0,
	         			MaterialType:'3'
	         		}
				],
				otherSeasoningList:[//其它调料数据
					{	
						Material:'',
	         			Unit:0,
	         			MaterialType:'3'
	         		}
				],
				seasoningAllList:[],//全部公司调料
				step:[//步骤
					{
						StepName:''
					}
				],
				
			}
		},
		components: {
            editor
     	},
		methods:{
			dishPictureSuccess (res, file) {//上传头像成功
                $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.dishInformation.DishPicUrl = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
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
           	//==============
           	removeMain(item){//主料删除
	         	this.Main_ingredientList.splice(this.Main_ingredientList.indexOf(item), 1)
	         },
	         addMainingredient(){//新加主料
	         	var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1'
	         	}
	         	this.Main_ingredientList.push(item);
//	         	console.log(this.Main_ingredientList)
	         },
	          removeAccessories(item){//辅料删除   Accessories_List
	         	this.Accessories_List.splice(this.Accessories_List.indexOf(item), 1)
			},
			addAccessories(){//辅料新增
				var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'2'
	         	}
	         	this.Accessories_List.push(item);
			},
			removeSeasoning(item){//调料删除
				this.seasoningList.splice(this.seasoningList.indexOf(item), 1)
			},
			addseasoning(){//推荐调料新增
				var item = {
					MemberId:this.dishInformation.MemberId,
	         		MySelfFRId:'',
	         		Unit:0,
//	         		MaterialType:'3'
	         	}
	         	this.seasoningList.push(item);
			},
			removeOtherSeasoning(item){//其它调料删除
				this.otherSeasoningList.splice(this.otherSeasoningList.indexOf(item), 1)
			},
			addOtherseasoning(){//其它调料新增
				var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'3'
	         	}
	         	this.otherSeasoningList.push(item);
			},
			removeStep(item){//删除步骤
				this.step.splice(this.step.indexOf(item), 1)
			},
			addStep(){//新增步骤
				var item = {
	         		StepName:'',
	         	}
	         	this.step.push(item);
			},
			getDishInformation(){//获取菜品信息
				var self =this
				//菜品数据
				var DATA = '{"MySelfDishId":"'+self.MySelfDishId+'"}'
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/MySelfDishKu/GetMySelfDish',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  	var dataAll =JSON.parse(json)
			        	  	console.log(dataAll)
			        	  	self.dishInformation = dataAll.dishInfo//基本信息
			        	  	self.Main_ingredientList = []//主料
			        	  	self.Accessories_List= []//辅料
			        	  	self.seasoningList = []//调料
			        	  	self.otherSeasoningList = []//其它调料
			        	  	self.step = []//步骤
			        	  	dataAll.dishMaterial.map((item) =>{
			        	  		//主料
			        	  		if (item.MaterialType==1) {
			        	  			item.Unit = parseInt(item.Unit)
			        	  			//DishId
			        	  			 delete item.DishId
			        	  			self.Main_ingredientList.push(item)
			        	  		}
			        	  		//辅料
			        	  		if (item.MaterialType==2) {
			        	  			item.Unit = parseInt(item.Unit)
			        	  			delete item.DishId
			        	  			self.Accessories_List.push(item)
			        	  		}
			        	  		//其它调料
			        	  		if (item.MaterialType==3) {
			        	  			item.Unit = parseInt(item.Unit)
			        	  			delete item.DishId
			        	  			self.otherSeasoningList.push(item)
			        	  		}
			        	  	})
			        	  	//推荐调料
			        	  	dataAll.qrole.map((item) =>{
			        	  		//item.Unit = parseInt(item.Unit)
			        	  		var MySelfFRRole = {
			        	  			MemberId:self.dishInformation.MemberId,
			        	  			MySelfFRId:item.MySelfFRId,
			        	  			Unit:parseInt(item.Unit)
			        	  		}
			        	  		
			        	  		self.seasoningList.push(MySelfFRRole)
			        	  	})
			        	  	//步骤
			        	  	//self.step = dataAll.dishStep
			        	  	dataAll.dishStep.map((item) =>{
			        	  		var StepName = {
			        	  			StepName:item.StepName
			        	  		}
			        	  		self.step.push(StepName)
			        	  	})
			        	  	//StepName
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    //推荐调料
	   		    $.ajax({
			        type :"get",
			        url :URLHeader.ECActivities+'/api/MySelfDishKu/GetMySelfFRWeb',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	  	var dataAll =JSON.parse(json)
			        	  	self.seasoningAllList = dataAll
			        	  	//console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			editorBtn(){//编辑
				var self =this
//				self.dishInformation = dataAll.dishInfo//基本信息
//	        	  	self.Main_ingredientList = []//主料
//	        	  	self.Accessories_List= []//辅料
//	        	  	self.seasoningList = []//推荐调料
//	        	  	self.otherSeasoningList = []//其它调料
//	        	  	self.step = []//步骤
				$('.loding').show()
				var dishInformation = self.dishInformation
				console.log(self.dishInformation)
				var arrMaterial = self.Main_ingredientList.concat(self.Accessories_List,self.otherSeasoningList)//主料 辅料 其他调料
				var DishMaterial = JSON.stringify(arrMaterial)
				var DishStep = JSON.stringify(self.step)//步骤
				var MySelfFRRole = JSON.stringify(self.seasoningList)//推荐调料
				var DATA = '{"MySelfDishKu":{"MySelfDishId":"'+dishInformation.MySelfDishId+'","DishPicUrl":"'+dishInformation.DishPicUrl+'","DishStory":"'+dishInformation.DishStory+'","DishName":"'+dishInformation.DishName+'"},"DishMaterial":'+DishMaterial+',"DishStep":'+DishStep+',"MySelfFRRole":'+MySelfFRRole+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/MySelfDishKu/UpdateMySelfDish',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  	//var dataAll =JSON.parse(json)
			        	  	$('.loding').hide()
			        	  	if (json=='OK') {
			        	  		self.$Message.success('数据编辑成功');
			        	  	}else{
			        	  		self.$Message.error({
				                content: '数据编辑失败',
				                duration: 10
				            });
			        	  	}
			        	  	console.log(json)
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '数据编辑失败,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			}
		},
		mounted:function(){
			$('.loding').hide()
			this.MySelfDishId = this.$route.query.MySelfDishId
			this.getDishInformation()
		}
	}
</script>
<style>
.feedBackEditor .ql-container{
	height: 70%;
}
.feedBackEditor .quill-editor{
	height: 22rem;
}
/*视频地址弹框移动*/
.feedBackEditor .ql-snow .ql-tooltip{
	margin-left: 9rem;
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
.feedBackEditor{
	overflow: auto;
	border: 1px dashed gainsboro;
	width: 60rem;
	margin: 0 auto;
	border-radius: 8px;
	min-height: 40rem;
}
</style>