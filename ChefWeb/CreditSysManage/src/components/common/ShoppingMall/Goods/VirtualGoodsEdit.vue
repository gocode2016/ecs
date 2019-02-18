<template>
	<div class="VirtualGoodsEdit">
		<Spin fix class='loding'>
        		<Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
        <div>Loading</div>
        </Spin>
		<h1 style="text-align: center;margin: 0rem auto;margin-top: 1rem;">虚拟商品编辑</h1>
		<ul style="overflow: auto;border-bottom:1px dashed gainsboro;padding-bottom: 1rem;">
			<li style="width: 14rem;margin-left: 2rem;" class="GoodsAddli">
					<img :src="GoodsInformation.ImgUrl" style="max-height: 5rem;max-width: 10rem;display: block;margin: 0 auto;margin-top: 1rem;"/>
					<Upload
				        ref="upload"
				        :show-upload-list="false"
				        :on-success="GoodsBigImgSuccess"
				        :format="['jpg','jpeg','png']"
				        :max-size="2048"
				        :before-upload='handleBeforeUpload'
				        :on-format-error="handleFormatError"
				        :on-exceeded-size="handleMaxSize"
				        :multiple=false
				        type="drag"
				        :action=UPImageProduct
				        style="display:block;width:90px;margin: 0 auto;margin-bottom: 0.5rem;margin-top: 0.5rem;" >
				         <Button type="primary">选择商品图</Button>
				   </Upload>				
			</li>
			<li style="width: 18rem;margin-top: 2rem;" class="GoodsAddli">
				<div>
					<span>商品名称</span>
					<Input v-model='GoodsInformation.ProductName'  style="width: 10rem;margin-left: 1rem;" placeholder="商品名称"></Input>
				</div>
				<div style="margin-top: 1rem;">
					<span>售价</span>
					<Input v-model='GoodsInformation.ProductPrice'  style="width: 10rem;margin-left: 2.5rem;" placeholder="售价"></Input>
				</div>
			</li>
			<li style="width: 20rem;margin-top: 2rem;" class="GoodsAddli">
				<div>
					<span>可售区域</span>
					<Select v-model="GoodsInformation.SaleProvince" multiple style="width:220px;margin-left: 1rem;">
				        <Option v-for="item in provinceList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</div>
				<div style="margin-top: 1rem;">
					<span>商品分类</span>
					<Select v-model="GoodsInformation.CategoryId" style="width:220px;margin-left: 1rem;" placeholder="请选择">
				        <Option v-for="item in GoodscLassificationList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</div>
				<div style="margin-top: 1rem;">
					<span>虚拟商品</span>
					<Select v-model="GoodsInformation.InventedType" style="width:220px;margin-left: 1rem;" placeholder='商品类型'>
				        <Option v-for="item in GoodsSpecies" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</div>
			</li>
		</ul>
		<!--<div class="rtf-content" style="margin-top: 1rem;height: 25rem;width: 0 auto;padding: 0 2rem;">
	        <quill-editor ref="myTextEditor" v-model="GoodsInformation.content" :config="editorOption"></quill-editor>
	    </div>-->
	    <div style="width: 50rem;margin: 0 auto;margin-top: 1rem;overflow: auto;">
	    		<editor ref="myTextEditor"
            :fileName="'myFile'"
            :canCrop="canCrop"
            :uploadUrl="UPImageProduct"
            v-model="GoodsInformation.ProductContent"></editor>
    <!--<div v-html="content"></div>-->
	    </div>
	    <Button type="primary" style="float: right;margin-top: 1rem;margin-right: 2rem;margin-bottom: 1rem;" @click="saveAlldata">保存</Button>
	</div>
</template>

<script>
	// import { quillEditor } from 'vue-quill-editor';//编写框
	import editor from '../../../tools/Quilleditor.vue'
	export default{
		data(){
			return{
				UPImageProduct:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Product',
      			content:'',
      			canCrop:false,
      			SpecificationList:[],//规格数据
				provinceList:[],//省份
				GoodsInformation:{
					ProductName:'',//商品名称
					CategoryId:0,//类目ID
					CategoryName:'',//类目名称
					ProductPrice:'',//价格
					ProductContent:'',//商品内容
					ImgUrl:'../../../../../static/image/placeholderBag.png',//商品图片
					SaleProvince:[],//可售省份
					AddDate:'',//当前时间
					goodId:'',
					InventedType:'',//虚拟商品选择
				},
				GoodscLassificationList:[//商品分类
                    {
                        value: 1000,
                        label: '读书一角'
                    },
                    {
                        value: 1001,
                        label: '试用专区'
                    },
                    {
                        value: 1002,
                        label: '活动名额'
                    },
                    {
                        value: 1003,
                        label: '后厨物料'
                    },
                    {
                        value: 1004,
                        label: '健康美味'
                    },
                    {
                        value: 1005,
                        label: '生活用品'
                    },
                    
				],
				GoodsSpecies:[
					 {
                        value: '1',
                        label: '欧飞话费充值'
                    },
				],
//				editorOption:{},
			}
		},
		components: {
            editor
     	},
		methods:{
			GoodsBigImgSuccess(res, file){//图片上传成功
				 $('.loding').hide()
	             console.log(file)
	             var data = JSON.parse(res);
	             this.GoodsInformation.ImgUrl = URLHeader.Loading+'/Product/'+data.Id+'.'+data.Type;
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
            },
             p(s) {
				return s < 10 ? '0' + s: s;
			},
			 requestGoodsInformation(){//初入界面数据请求
            		var self = this
            		var goodsId = self.GoodsInformation.goodId
            		$.ajax({
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Product/GetProductById?productId='+self.GoodsInformation.goodId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			       	 	var dataAll =JSON.parse(json)[0]
			       	 	var SaleProvince = dataAll.SaleProvince.split('|')
			       	 		dataAll.SaleProvince = [];
			        		for (var i =0;i<SaleProvince.length;i++) {
					        	dataAll.SaleProvince.push(parseInt(SaleProvince[i]))
			        		}
			        		self.GoodsInformation = dataAll
			        		self.GoodsInformation.goodId = goodsId
			        		self.GoodsInformation.InventedType = self.GoodsInformation.InventedType.toString()
			        		$('.loding').hide()
			        		console.log(self.GoodsInformation)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		     $.ajax({//获取规格
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Norms/GetProductNorms?productId='+self.GoodsInformation.goodId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		var dataAll = JSON.parse(json)
			        		if (dataAll.length>0) {
			        			self.SpecificationList = dataAll
			        		}else{	
							self.SpecificationList =[]
			        		}
			        		console.log(self.SpecificationList)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });	
           },
            saveAlldata(){//整体保存
            		var self =this
            			//获取当前时间
	            		var myDate = new Date();
					//获取当前年
					var year=myDate.getFullYear();
					//获取当前月
					var month=myDate.getMonth()+1;
					//获取当前日
					var date=myDate.getDate(); 
					var h=myDate.getHours();       //获取当前小时数(0-23)
					var m=myDate.getMinutes();     //获取当前分钟数(0-59)
					var s=myDate.getSeconds();  
					var now=year+'-'+this.p(month)+"-"+this.p(date)+" "+this.p(h)+':'+this.p(m)+":"+this.p(s);
					this.GoodsInformation.AddDate = now
					//获取类目名称
					for (var i= 0;i<this.GoodscLassificationList.length;i++) {
						if (self.GoodscLassificationList[i].value==self.GoodsInformation.CategoryId) {
							self.GoodsInformation.CategoryName =self.GoodscLassificationList[i].label
						}
					}
					//self.GoodsInformation.SaleProvince
					self.GoodsInformation.SaleProvinceStr=self.GoodsInformation.SaleProvince.join('|')
					//商品图片
					if (self.GoodsInformation.ImgUrl=='../../../../../static/image/placeholderBag.png') {//图片为空
						self.$Message.warning('商品图未选择');
					}else{
						if (self.GoodsInformation.CategoryId && self.GoodsInformation.ProductPrice &&self.GoodsInformation.ProductContent && self.GoodsInformation.SaleProvince.length>0 && self.GoodsInformation.ProductName) {
							console.log('信息完整')
							self.GoodsInformation.ProductContent = self.GoodsInformation.ProductContent.replace(/\"/g,"'");//把双引号改为单引号
							var DATA = '{"ProductId":"'+self.GoodsInformation.goodId+'","ProductName":"'+self.GoodsInformation.ProductName+'","CategoryId":'+self.GoodsInformation.CategoryId+',"CategoryName":"'+self.GoodsInformation.CategoryName+'","ProductPrice":"'+self.GoodsInformation.ProductPrice+'","ProductContent":"'+self.GoodsInformation.ProductContent+'","ImgUrl":"'+self.GoodsInformation.ImgUrl+'","SaleProvince":"'+self.GoodsInformation.SaleProvinceStr+'","AddDate":"'+self.GoodsInformation.AddDate+'","ProductType":2,"InventedType":"'+self.GoodsInformation.InventedType+'","IsEnable":0}'
							console.log(DATA)
				        		$('.loding').show()
				        		$.ajax({
						        type :"post",
						        url :URLHeader.ShoppingMall+'/api/Product/UpdateProduct',
						        dataType : 'json',
						        contentType: "application/json; charset=utf-8",				
						        data:DATA,
						        success : function(json) {
						        	console.log(json)
						        	  self.$Message.success('数据保存成功');
						        	  self.saveSpecification()
						        	  $('.loding').hide()
						        },
						        error : function(error) { 
						        		console.log(error)
						        }
				   		    });
						}else{//信息少填
							self.$Message.warning('请将信息填写完整');
						}
					}
            },
            saveSpecification(){
            		var self =this
            		//self.SpecificationList
            		
            		if (self.SpecificationList.length>0) {
            			var url =''
					for (var i=0;i<self.SpecificationList.length;i++) {
						var listOne = self.SpecificationList[i];
						listOne.ProductName =self.GoodsInformation.ProductName
						
						var DATA = '{"NormsId":'+listOne.NormsId+',"ProductId":'+listOne.ProductId+',"ProductName":"'+listOne.ProductName+'","TypeName":"'+listOne.TypeName+'","TypeValue":"'+listOne.TypeValue+'"}'
						url=URLHeader.ShoppingMall+'/api/Norms/UpdateNorms'
						console.log(DATA)
						$.ajax({
					        type :"post",
					        url :url,
					        dataType : 'json',
					        contentType: "application/json; charset=utf-8",				
					        data:DATA,
					        success : function(json) {
					        	console.log('规格名字修改'+json)
					        },
					        error : function(error) { 
					        		console.log(error)
					        }
			   		    });
   					}
            		}
					
            },
            
		},
		mounted:function(){
			this.provinceList = ProvinceArea
			
			this.GoodsInformation.goodId= window.location.hash.split('productId=')[1];
			this.requestGoodsInformation()
//			$('.VirtualGoodsEdit ul .GoodsAddli .ivu-select-placeholder').css({
//				'display':'block'
//			})
		}
	}
</script>
<style>
.VirtualGoodsEdit .ql-container{
	height: 80%;
}
.VirtualGoodsEdit .quill-editor{
	height: 22rem;
}
.VirtualGoodsEdit .rtf-content .ql-toolbar .ql-video{
	display: none;
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
.VirtualGoodsEdit {
	border: 1px dashed gainsboro;
	border-radius: 6px;
	width: 55rem;
	margin: 0 auto;
	/*height: 50rem;*/
	overflow: auto;
}
.VirtualGoodsEdit ul .GoodsAddli{
	display: block;
	float: left;
}

</style>