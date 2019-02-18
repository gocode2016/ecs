<template>
	<!--反馈菜审核-->
	<div class="feedBackAudit">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
        <Modal
	        title="推送消息"
	        v-model="messageModal"
	        :mask-closable="false"
	        :width='400'
	        @on-ok='pushMessageBtn(2)'>
	        <Input v-model="errorMessage" type="textarea" :rows="6" placeholder="请输入不合格的原因或修改提示"></Input>
	    </Modal>
	    <Modal title="图片" v-model="imgModal">
		    <img  :src="dishInformation.DishPicUrl" style="width: 30rem;display: block;margin: 0 auto;"/>
		</Modal>
		<ul class="feedBackAudit_Ul">
			<li class="feedBackAudit_Ul_li">
				<span class="lable">菜品名:</span>
				<span class="value">{{dishInformation.DishName}}</span>
			</li>
			<li style="height: 8rem;line-height: 7rem;overflow:hidden" class="feedBackAudit_Ul_li">
				<span class="lable">菜品图片:</span>
				<div class="demo-upload-list">
					<img  :src="dishInformation.DishPicUrl" style="height: 6rem;"/>
		            <div class="demo-upload-list-cover">
		                <Icon type="ios-eye-outline" 	@click.native="showImageModal"></Icon>
		            </div>
				</div>
				
			</li>
			<li class="feedBackAudit_Ul_li">
				<span class="lable">菜品故事:</span>
				<span class="value">{{dishInformation.DishStory}}</span>
			</li>
			<li class="feedBackAudit_Ul_li">
				<span class="lable">主料:</span>
    				<ul  class="value boderUl">
    					<li v-for='item in Main_ingredientList' style="overflow: auto;">
    						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
    						<span style="display: block;float: right;">{{item.Unit}}</span>
    					</li>
    				</ul>
			</li>
			<li class="feedBackAudit_Ul_li">
				<span class="lable">辅料:</span>
    				<ul  class="value boderUl">
    					<li v-for='item in Accessories_List' style="overflow: auto;">
    						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
    						<span style="display: block;float: right;">{{item.Unit}}</span>
    					</li>
    				</ul>
			</li>
			<li class="feedBackAudit_Ul_li">
				<span class="lable">推荐调料:</span>
    				<ul class="value boderUl">
    					<li v-for='item in seasoningList' style="overflow: auto;">
    						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.FRName}}</span>
    						<span style="display: block;float: right;">{{item.Unit}}</span>
    					</li>
    				</ul>
			</li>
			<li class="feedBackAudit_Ul_li">
				<span class="lable">其它调料:</span>
    				<ul  class="value boderUl">
    					<li v-for='item in otherSeasoningList' style="overflow: auto;">
    						<span style="display: block;float: left;margin-left: 0.5rem;">{{item.Material}}</span>
    						<span style="display: block;float: right;">{{item.Unit}}</span>
    					</li>
    				</ul>
			</li>
			<li class="feedBackAudit_Ul_li">
				<span class="lable">烹饪步骤:</span>
    				<ul style="width: 15rem;padding-bottom: 1rem;" class="value">
    					<li v-for='(item,index) in step' style="overflow: auto;margin-top: 0.6rem;">
    						<span style="margin-left: 0.5rem;display: block;width: 1rem;float: left;">{{index+1}}{{'.'}}</span>
    						<span style="margin-left: 0.5rem;display: block; float: left;width: 13rem;">{{item.StepName}}</span>
    					</li>
    				</ul>
			</li>
			<li style="text-align: center;">
				<Button type="primary" style="margin-right: 1rem;" @click="auditBtn(1)" :disabled='btnDisabled'>合格</Button>
				<Button type="error" @click="auditBtn(2)" :disabled='btnDisabled'>不合格</Button>
			</li>
		</ul>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				imgModal:false,//图片弹框
				messageModal:false,//消息弹框
				errorMessage:'',//不合格原因
				btnDisabled:false,//审核接口禁用
				MySelfDishId:'',
				dishInformation:{
					DishName:'黑暗料理',//菜品名
					DishPicUrl:'../../../../../static/image/placeholderBag.png',//菜品图片
					DishStory:'这道菜汤浓味美、酸甜适口，是丰富家庭餐桌的一道健康菜。天气寒凉时，吃饭前先喝上一小碗汤，足够暖身暖胃、悠哉自得。番茄不光味美、热量低、营养也丰富。是维生素A、维生素C和叶酸的较好来源。具有抗坏血病、润肤、保护血管、降压、助消化等功效。牛肉中的肌氨酸含量比任何其它食品都高，这使它对增长肌肉、增强力量特别有效。',//菜品故事
				},
				Main_ingredientList:[//主料
					{
						Material:'西红柿',
						Unit:'200g'
					}
				],
				Accessories_List:[//辅料
					{
						Material:'西红柿',
						Unit:'200g'
					}
				],
				seasoningList:[//推荐调料
					{
						FRName:'西红柿',
						Unit:'200g'
					}
				],
				otherSeasoningList:[//其它调料
					{
						Material:'西红柿',
						Unit:'200g'
					}
				],
				step:[
					{
						StepName:'第一步',
					}
				]
			}
		},
		methods:{
			showImageModal(){//大图
				this.imgModal = true
			},
			getDishInformation(){//获取菜品信息
				var self =this
				self.Main_ingredientList = []//主料
	        	  	self.Accessories_List= []//辅料
	        	  	self.seasoningList = []//调料
	        	  	self.otherSeasoningList = []//其它调料
	        	  	self.step = []//步骤
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
			        	  	
			        	  	dataAll.dishMaterial.map((item) =>{
			        	  		//主料
			        	  		if (item.MaterialType==1) {
			        	  			self.Main_ingredientList.push(item)
			        	  		}
			        	  		//辅料
			        	  		if (item.MaterialType==2) {
			        	  			self.Accessories_List.push(item)
			        	  		}
			        	  		//其它调料
			        	  		if (item.MaterialType==3) {
			        	  			self.otherSeasoningList.push(item)
			        	  		}
			        	  	})
			        	  	//推荐调料
			        	  	self.seasoningList = dataAll.qrole
			        	  	//步骤
			        	  	self.step = dataAll.dishStep
			        	  	//StepName
			        	  	$('.loding').hide()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			auditBtn(IsApply){//审核  1通过 2未通过
				var self =this
				$('.loding').show()
				var DATA = '{"DishId":"'+this.MySelfDishId+'","IsApply":'+IsApply+'}'
				console.log(DATA)
				 $.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/MySelfDishKu/MySelfDishIsApply',//
			        data:DATA,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		console.log(json)
			        		if (json=='OK') {
			        	  		self.$Message.success('菜品审核成功');
			        	  		self.errorMessage = ''
			        	  		if (IsApply==2) {//不合格
			        	  			self.messageModal = true//推送消息弹框
			        	  			
			        	  		}else{//合格
			        	  			self.pushMessageBtn(1)
			        	  		}
//			        	  		self.btnDisabled = true
			        	  	}else{
			        	  		self.$Message.error({
				                content: '菜品审核失败',
				                duration: 10
				            });
			        	  	}
			        	  	$('.loding').hide()
			        },
			        error : function(error) {
			        		self.$Message.error({
			                content: '菜品审核失败,接口错误',
			                duration: 10
			            });
			        		console.log(error)
			        }
	   		    });
			},
			pushMessageBtn(IsApply){//推送不合格消息
				//errorMessage
				this.messageModal = false
				var self =this
				var DATA =' {"MemberId":'+self.dishInformation.MemberId+',"MySelfDishId":'+self.dishInformation.MySelfDishId+',"DishName":"'+self.dishInformation.DishName+'","Context":"'+self.errorMessage+'","IsApply":'+IsApply+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/MySelfDishKu/SentTemplate',//
			        data:DATA,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		console.log(json)
			        		if (json=='OK') {
			        	  		self.$Message.success('消息推送成功');
			        	  	}else{
			        	  		self.$Message.error({
				                content: '消息推送失败',
				                duration: 10
				            });
			        	  	}
			        	  	$('.loding').hide()
			        },
			        error : function(error) {
			        		self.$Message.error({
			                content: '消息推送失败,接口错误',
			                duration: 10
			            });
			        		console.log(error)
			        }
	   		    });
			}
		},
		mounted:function(){
			this.MySelfDishId = this.$route.query.MySelfDishId
			this.getDishInformation()
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
.feedBackAudit{
	overflow: auto;
	width: 30rem;
	margin: 0 auto;
	border: 1px solid gainsboro;
	padding: 1rem;
	border-radius:8px ;
}
.feedBackAudit_Ul{
	margin: 0 auto;
}
.feedBackAudit_Ul .feedBackAudit_Ul_li{
	margin-top: 1rem;
	overflow: auto;
}
.feedBackAudit_Ul .feedBackAudit_Ul_li .lable{
	margin-left: 1rem;
	display: block;
	float: left;
	width: 5rem;
	margin-right: 1rem;
	font-size: 14px;
}
.feedBackAudit_Ul .feedBackAudit_Ul_li .value{
	display: block;
	float: left;
	width: 20rem;
}
.feedBackAudit_Ul .feedBackAudit_Ul_li .boderUl{
	width: 15rem;
	padding: 0.5rem;
	padding-bottom: 1rem;
	border: 1px dashed gainsboro;
	border-radius:4px ;
}
/**/
.demo-upload-list{
    display: inline-block;
   /* width: 100px;*/
    /*height: 60px;*/
   height: 6rem;
    text-align: center;
    line-height: 60px;
    border: 1px solid transparent;
    border-radius: 4px;
    overflow: hidden;
    background: #fff;
    position: relative;
    box-shadow: 0 1px 1px rgba(0,0,0,.2);
    margin-right: 4px;
}
/*.demo-upload-list img{
    width: 100%;
    height: 100%;
}*/
.demo-upload-list-cover{
    display: none;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: rgba(0,0,0,.6);
}
.demo-upload-list:hover .demo-upload-list-cover{
    display: block;
}
.demo-upload-list-cover i{
    color: #fff;
    font-size: 25px;
    cursor: pointer;
    margin: 0 2px;
    margin-top: 2rem;
}
</style>