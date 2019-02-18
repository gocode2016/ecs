<template>
	<div class="SingleEdit">
		<h2 style="margin: 0 auto;text-align: center;margin-top: 1rem;">单品编辑</h2>
		<div style="border-bottom: 1px dashed gray;overflow: auto;">
			<div class="left" style="margin-top: 2rem;">
				<span style="margin-left: 4rem;">商品编码:</span>
				<!--<Input  size="large" placeholder="请输入" style="width: 200px;margin-left: 1rem;"  v-model="ProductInformation.ProductId"></Input>-->
				<span style="margin-left: 1rem;">{{ProductInformation.ProductId}}</span>
				<!--<Button type="primary" @click="elect">选中</Button>-->
			</div>
			<div class="right" style="margin-bottom: 1rem;">
				<img :src="ImgUrl" style="max-width: 13rem;display: block;margin: 1rem auto;"/>
				<span style="display: block;float: left;margin-left: 6rem;">{{TypeName}}</span>
				    <RadioGroup v-model="ProductInformation.NormsId" style='display: block;float: left;margin-left: 1rem;width: 14rem;' >
				        <Radio v-for='item in specificationList' :label="item.NormsId" :key='item.NormsId'>{{item.TypeValue}}</Radio>
				    </RadioGroup>
			</div>
		</div>
		<div class="bottom" style="overflow: auto;">
			<ul >
				<li class="bottomLI">
					<span>物料编码</span>
					<Input  v-model="ProductInformation.SkuCode"  size="large" placeholder="请输入" style="width: 200px;margin-left: 1rem;"></Input>
				</li>
				<li class="bottomLI">
					<span>售价</span>
					<Input  v-model="ProductInformation.Price"  size="large" placeholder="请输入" style="width: 200px;margin-left: 2.5rem;"></Input>
				</li>
				<li class="bottomLI">
					<span>市场价</span>
					<Input   v-model="ProductInformation.MarketPrice" size="large" placeholder="请输入" style="width: 200px;margin-left: 1.8rem;"></Input>
				</li>
			</ul>
			<ul >
				<li class="bottomLI">
					<span>库存</span>
					<Input   v-model="ProductInformation.Stock" size="large" placeholder="请输入" style="width: 200px;margin-left: 2.2rem;"></Input>
				</li>
				<li class="bottomLI">
					<span>上架状态</span>
					<Select v-model="ProductInformation.IsShow" style="width:200px;margin-left: 0.8rem;">
				        <Option v-for="item in shelvesList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
			</ul>
		</div>
		<Button type="primary" style="float: right;margin-right: 3rem;" @click="saveData">保存</Button>
	</div>
</template>

<script>
	export default{
		data(){//:8004/api/SKU/GetSkuByNorms?normsId={normsId}
			return{
				NormsId:'',
				ImgUrl:'../../../../../static/image/placeholderBag.png',//图片
				ProductInformation:{
					SkuName:'',//选中规格+名称
					ProductId:'',//商品编码
					NormsId:'',//选中的规格
					
					SkuCode:'',//物料编码
					Price:'',//售价
					MarketPrice:'',//出厂价
					Stock:'',//库存
					IsShow:null,//上架状态
					AddDate:'',//上传时间
				},
				TypeName:'',//规格名称
				specificationList:[//规格列表
				],
				shelvesList:[
					{
                        value: 1,
                        label: '上架'
                     },
                     {
                        value: 0,
                        label: '下架'
                     },
				],
			}
		},
		methods:{
			p(s) {
				return s < 10 ? '0' + s: s;
			},
			saveData(){//数据保存
//				SkuName     选中规格名称
//				ProductId  商品编码
//				SkuCode   物料编码
//				NormsId 选中规格id
//				Stock     库存
//				MarketPrice 市场价
//				Price   售价
//				Sort   排序
//				Limit   限制默认0
//				IsShow   上下架状态
//				AddDate  上传时间
				var self =this					
					if (self.specificationList.length>0) {//点击选中后  获取规格数据   
						//:8004/api/SKU/UpdateSKU
						if (self.ProductInformation.SkuCode && self.ProductInformation.NormsId && self.ProductInformation.Stock && self.ProductInformation.MarketPrice && self.ProductInformation.Price && self.ProductInformation.IsShow>=0) {//检测数据是否填写完整
							for (var i = 0;i<self.specificationList.length;i++) {
								if (self.specificationList[i].NormsId==self.ProductInformation.NormsId) {
									self.ProductInformation.SkuName = self.specificationList[i].ProductName+self.specificationList[i].TypeValue
								}
							}
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
							self.ProductInformation.AddDate = now
							
							var DATA = '{"SkuId":"'+self.ProductInformation.SkuId+'","SkuName":"'+self.ProductInformation.SkuName+'","ProductId":"'+self.ProductInformation.ProductId+'","SkuCode":"'+self.ProductInformation.SkuCode+'","NormsId":"'+self.ProductInformation.NormsId+'","Stock":"'+self.ProductInformation.Stock+'","MarketPrice":"'+self.ProductInformation.MarketPrice+'","Price":"'+self.ProductInformation.Price+'","Sort":0,"Limit":0,"IsShow":'+self.ProductInformation.IsShow+',"AddDate":"'+self.ProductInformation.AddDate+'"}';
							console.log(DATA)
							$.ajax({//数据保存
						        type :"post",
						        url :URLHeader.ShoppingMall+'/api/SKU/UpdateSKU',
						        dataType : 'json',
						        contentType: "application/json; charset=utf-8",
						        data:DATA,
						        success : function(json) {
						        	  //var dataAll =JSON.parse(json)[0]
						        	   self.$Message.success('数据保存成功');
						        	   console.log(json)
						        },
						        error : function(error) { 
						        		console.log(error)
						        }
				   		    });							
						} else{//数据未填写完整
							console.log(self.ProductInformation.IsShow)
						}	
					}else{
						
						self.$Message.warning('请输入商品编码,并点击选中');
					}
			}, 
//			elect(){//选中方法
//				this.requestProductInformation()
//				console.log(this.ProductInformation.NormsId)
//			},
			requestProductInformation(ProductId){
				var self =this
				$.ajax({//图片接口
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Product/GetProductById?productId='+ProductId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)[0]
						self.ImgUrl = dataAll.ImgUrl
			        	  // console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    
	   		    $.ajax({//规格数据
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Norms/GetProductNorms?productId='+ProductId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		var dataAll = JSON.parse(json)
			        		self.specificationList = dataAll
			        		self.TypeName = self.specificationList[0].TypeName
			        		//console.log(self.specificationList)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    
	   		    
			},
			requestAllData(){
			//api/SKU/GetSkuByNorms?normsId={normsId}
				var self =this
				$.ajax({//规格数据
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/SKU/GetSkuByNorms?normsId='+this.NormsId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		var dataAll = JSON.parse(json)[0]
			        		self.ProductInformation = dataAll
			        		console.log(dataAll)
			        		 self.requestProductInformation(self.ProductInformation.ProductId)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		   });
			},
			
		},
		
//		watch: {
//		    'ProductInformation.NormsId' () {//规格切换
//		       // console.log(this.ProductInformation.NormsId)
//		        $.ajax({//规格数据
//			        type :"get",
//			        url :URLHeader.ShoppingMall+'/api/SKU/GetSkuByNorms?normsId='+this.ProductInformation.NormsId,
//			        dataType : 'json',
//			        contentType: "application/json; charset=utf-8",				
//			        success : function(json) {
//			        		var dataAll = JSON.parse(json)
//			        		console.log(dataAll)
//			        },
//			        error : function(error) { 
//			        		console.log(error)
//			        }
//	   		    });
//		       
//		       
//		     },
//	   },
		mounted:function(){
			this.NormsId= window.location.hash.split('NormsId=')[1];
			this.requestAllData()
		}
	}
</script>

<style scoped>
.SingleEdit{
	border: 1px solid gainsboro;
	width: 60rem;
	padding-bottom: 1rem;
	border-radius:6px ;
	margin: 1rem auto;
	overflow: auto;
}
.SingleEdit .left,.SingleEdit .right{
	float: left;
	width: 29rem;
}
.SingleEdit .right .specificationLI{
	display: block;float: left;
	width: 5rem;
}
.SingleEdit .bottom ul{
	display: block;
	float: left;
	width: 29rem;
	padding-left: 4rem;
}
.SingleEdit .bottom ul .bottomLI {
	margin-top: 1rem;
}
</style>