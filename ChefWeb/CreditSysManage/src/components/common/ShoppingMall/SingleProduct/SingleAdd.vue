<template>
	<div class="SingleAdd">
		<Spin fix class='loding'>
        		<Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
        <div>Loading</div>
        </Spin>
		<h2 style="margin: 0 auto;text-align: center;margin-top: 1rem;">单品页面</h2>
		<div style="border-bottom: 1px dashed gray;overflow: auto;">
			<div class="left" style="margin-top: 2rem;">
				<span style="margin-left: 4rem;">商品编码</span>
				<Input :disabled='disabled'  size="large" placeholder="请输入" style="width: 200px;margin-left: 1rem;"  v-model="ProductInformation.ProductId"></Input>
				<Button type="primary" @click="elect" class="elect">选中</Button>
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
					<span>出厂价</span>
					<Input   v-model="ProductInformation.MarketPrice" size="large" placeholder="请输入" style="width: 200px;margin-left: 1.8rem;"></Input>
				</li>
			</ul>
			<ul >
				<li class="bottomLI">
					<span>库存</span>
					<Input   v-model="ProductInformation.Stock" size="large" placeholder="请输入" style="width: 200px;margin-left: 2.2rem;"></Input>
				</li>
				<!--<li class="bottomLI">
					<span>排序</span>
					<Input   v-model="ProductInformation.Sort" size="large" placeholder="请输入" style="width: 200px;margin-left: 2.2rem;"></Input>
				</li>-->
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
				disabled:true,//为新增模式 禁止输入
				NormsId:'',//判断之前此规格下是否已经存在信息
				selectInformation:{
					
				},//之前的信息
				ImgUrl:'../../../../../static/image/placeholderBag.png',//图片
				ProductInformation:{
					SkuName:'',//选中规格+名称
					ProductId:'',//商品编码
					NormsId:'',//选中的规格
					
					SkuCode:'',//物料编码
					Price:'',//售价
					MarketPrice:'',//出厂价
					Stock:'',//库存
					Sort:'',//排序
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
				console.log(self.ProductInformation.SkuId)
				console.log('SkuCode='+self.ProductInformation.SkuCode)
				console.log('NormsId='+self.ProductInformation.NormsId)
				console.log('Stock='+self.ProductInformation.Stock)
				console.log('Sort='+self.ProductInformation.Sort)
				
				self.ProductInformation.Stock=self.ProductInformation.Stock.toString()
				console.log('MarketPrice='+self.ProductInformation.MarketPrice)
				console.log('Price='+self.ProductInformation.Price)
				self.ProductInformation.Price= self.ProductInformation.Price.toString()
				console.log('IsShow='+self.ProductInformation.IsShow)
//				if(self.ProductInformation.Price){
//					console.log('pp')
//				}
				if (self.ProductInformation.SkuCode && self.ProductInformation.NormsId && self.ProductInformation.Stock && self.ProductInformation.MarketPrice && self.ProductInformation.Price && self.ProductInformation.IsShow!=null) {//检测数据是否填写完整
					
					if (self.specificationList.length>0) {
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
						var url=''
						var DATA =''
						var lv=''
						if (self.ProductInformation.SkuId=='') {//新增
							 url=URLHeader.ShoppingMall+'/api/SKU/AddSKU'
							DATA = '{"SkuName":"'+self.ProductInformation.SkuName+'","ProductId":"'+self.ProductInformation.ProductId+'","SkuCode":"'+self.ProductInformation.SkuCode+'","NormsId":"'+self.ProductInformation.NormsId+'","Stock":"'+self.ProductInformation.Stock+'","MarketPrice":"'+self.ProductInformation.MarketPrice+'","Price":"'+self.ProductInformation.Price+'","Sort":0,"Limit":0,"IsShow":'+self.ProductInformation.IsShow+',"AddDate":"'+self.ProductInformation.AddDate+'","IsEnable":0}';
							lv='新增'
						} else{
							url=URLHeader.ShoppingMall+'/api/SKU/UpdateSKU'
						     DATA = '{"SkuId":"'+self.ProductInformation.SkuId+'","SkuName":"'+self.ProductInformation.SkuName+'","ProductId":"'+self.ProductInformation.ProductId+'","SkuCode":"'+self.ProductInformation.SkuCode+'","NormsId":"'+self.ProductInformation.NormsId+'","Stock":"'+self.ProductInformation.Stock+'","MarketPrice":"'+self.ProductInformation.MarketPrice+'","Price":"'+self.ProductInformation.Price+'","Sort":0,"Limit":0,"IsShow":'+self.ProductInformation.IsShow+',"AddDate":"'+self.ProductInformation.AddDate+'","IsEnable":0}';
							lv='编辑'
						}
						console.log(self.ProductInformation)
						console.log(DATA)
						console.log(lv)
						$('.loding').show()
						$.ajax({//数据保存
					        type :"post",
					        url :url,
					        dataType : 'json',
					        contentType: "application/json; charset=utf-8",
					        data:DATA,
					        success : function(json) {
					        	   self.$Message.success('数据保存成功');
					        	   $('.loding').hide()
					        	   if (self.ProductInformation.SkuId=='') {//新增记录
					        	    		self.ProductInformation.SkuId=json
					        	    		console.log('SkuId'+self.ProductInformation.SkuId)
					        	   }
					        	   //console.log(json)
					        },
					        error : function(error) { 
					        		console.log(error)
					        }
			   		    });
					} else{
						self.$Message.warning('请输入商品编码,并点击选中');
					}

				}else{
					self.$Message.warning('请将数据填写完整');
				}

			},
			elect(){//选中按钮方法
				var self = this
				self.ProductInformation.SkuName =''
				self.ProductInformation.SkuCode =''
				self.ProductInformation.Price =''
				self.ProductInformation.MarketPrice =''
				self.ProductInformation.Stock =''
				self.ProductInformation.Sort = ''
				self.ProductInformation.IsShow =null
				self.ProductInformation.AddDate =''
				self.NormsId=''
				this.requestProductInformation(this.ProductInformation.ProductId)
				console.log(this.ProductInformation)
			},
			requestProductInformation(ProductId){//获取规格列表
				var self =this
				$.ajax({//图片接口
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Product/GetProductById?productId='+ProductId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		//console.log(json)
			        		if(json!=0){
			        			 var dataAll =JSON.parse(json)[0]
			        			 console.log(dataAll)
					        	 self.ImgUrl = dataAll.ImgUrl
			        		}else{
			        			self.$Notice.warning({
			                    desc: '此商品无数据,请重新输入! '
			                });
			        		}
			        	  // console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        		self.$Notice.warning({
			                    desc: '此商品无数据,请重新输入! '
			             });
			        }
	   		    });
	   		    
	   		    $.ajax({//规格数据
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Norms/GetSysProductNorms?productId='+ProductId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		var dataAll = JSON.parse(json)
			        		if (dataAll!='') {
			        			self.specificationList = dataAll
			        			console.log(dataAll)
			        			self.TypeName = self.specificationList[0].TypeName
			        		}
			        		
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    
	   		    
			},
			requestAllData(){//编辑  获取选中规格的详细数据
			//api/SKU/GetSkuByNorms?normsId={normsId}
				var self =this
				$('.loding').show()
				$.ajax({//规格数据
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/SKU/GetSkuByNorms?normsId='+this.NormsId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		//var dataAll = JSON.parse(json)[0]
						var dataAll = JSON.parse(json).SkuData[0]
			        		self.ProductInformation = dataAll
			        		console.log(dataAll)
			        		 self.requestProductInformation(self.ProductInformation.ProductId)
			        		$('.loding').hide()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		   });
			},
		},
		watch: {
		    'ProductInformation.NormsId' () {//规格切换
				var self = this
					self.ProductInformation.SkuName =''
					self.ProductInformation.SkuCode =''
					self.ProductInformation.Price =''
					self.ProductInformation.MarketPrice =''
					self.ProductInformation.Stock =''
					self.ProductInformation.Sort = ''
					self.ProductInformation.IsShow =null
					self.ProductInformation.AddDate =''
					self.ProductInformation.SkuId=''
				$.ajax({//规格下的数据
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/SKU/GetSkuByNorms?normsId='+self.ProductInformation.NormsId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		console.log(json)
			        		//var dataAll = JSON.parse(json)[0]
			        		var dataAll = JSON.parse(json).SkuData[0]
			        		if (dataAll !=null) {
			        			 //console.log(dataAll)
			        			self.ProductInformation = dataAll
			        			//console.log(dataAll)
			        		}
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
		   		   });

		      
		     },
	   },
		mounted:function(){
			var self =this;
			if (window.location.hash.indexOf("NormsId") > 0) {//编辑模式
				self.disabled = true//商品编码禁止输入
				$('.elect').hide()//选中按钮隐藏
				self.NormsId= window.location.hash.split('NormsId=')[1];
				self.requestAllData()//请求之前数据
			}else{
				self.disabled=false//商品编码可以输入
				$('.elect').show()//选中按钮隐藏
				$('.loding').hide()
			}
//			console.log(self.NormsId)
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
.SingleAdd{
	border: 1px solid gainsboro;
	width: 60rem;
	padding-bottom: 1rem;
	border-radius:6px ;
	margin: 1rem auto;
	overflow: auto;
}
.SingleAdd .left,.SingleAdd .right{
	float: left;
	width: 29rem;
}
.SingleAdd .right .specificationLI{
	display: block;float: left;
	width: 5rem;
}
.SingleAdd .bottom ul{
	display: block;
	float: left;
	width: 29rem;
	padding-left: 4rem;
}
.SingleAdd .bottom ul .bottomLI {
	margin-top: 1rem;
}
</style>