<template>
	<div class="SinglePromotions">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="SinglePromotions_top">
			<span style="margin-right: 2rem;">促销方案</span>
			<RadioGroup v-model='radio'>
		        <Radio label="1">折扣与限购</Radio>
		        <!--<Radio label="2">积分求助</Radio>-->
		    </RadioGroup>
		</div>
		<div class="SinglePromotions_body">
			<div class="discount">
				<Form ref="discount" :model="discount" :rules="ruleCustom" :label-width="80" style="width: 370px;margin: 0 auto;">
			        <FormItem label="开始时间" prop="StartTime">
			            <DatePicker v-model='discount.StartTime' type="datetime" placeholder="开始时间" style="width: 200px"></DatePicker>
			        </FormItem>
			        <FormItem label="结束时间" prop="EndTime">
			            <DatePicker v-model='discount.EndTime' type="datetime" placeholder="结束时间" style="width: 200px"></DatePicker>
			        </FormItem>
			        <FormItem label="折后价格" prop="SalePrice">
			            <Input  v-model="discount.SalePrice" style="width: 200px;"></Input>
			        </FormItem>
			        <FormItem label="限购数量" prop="Limit">
			            <Input  v-model="discount.Limit" style="width: 200px;"></Input>
			            <span style="color:burlywood">(*不限购请填0)</span>
			        </FormItem>
			        
			    </Form>
			    <Button type="primary" v-if="addModel" style="margin: 0 auto;display: block;" @click="saveDiscount('discount')">保存</Button>
			     <Button type="error" v-if="!addModel" style="margin: 0 auto;display: block;" @click="ModifyDiscount('discount')">修改</Button>
			</div>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				SkuId:'',//
				radio:'1',//方案选择
				addModel:true,//新增模式
				discount:{//折扣与限购填写
					StartTime:'',//开始时间
					EndTime:'',//结束时间
					SalePrice:'',//折后价格
					Limit:'',//限购
				},
				ruleCustom:{
					StartTime:[{//开始时间
						required: true,
						type: 'date', 
						message: '请填写开始时间',
						trigger: 'change'
					}],
					EndTime:[{//开始时间
						required: true,
						type: 'date', 
						message: '请填写结束时间',
						trigger: 'change'
					}],
					SalePrice: [{ 
						required: true,
						message: '请填写折后价格', 
					}],
					Limit:[{
						required: true,
						message: '请填限购', 
					}]
				}
				
			}
		},
		methods:{
			saveDiscount(name){//折扣  保存
				 var self = this
				 //console.log(this.discount)
				 this.$refs[name].validate((valid) => {
                    if (valid) {
                    		//时间转换
                    		var star = new Date(self.discount.StartTime);  
						var StartTime = star.getFullYear() + '-' + (star.getMonth() + 1) + '-' + star.getDate() + ' ' + star.getHours() + ':' + star.getMinutes() + ':' + star.getSeconds();
                    		var end = new Date(self.discount.EndTime);  
						var EndTime = end.getFullYear() + '-' + (end.getMonth() + 1) + '-' + end.getDate() + ' ' + end.getHours() + ':' + end.getMinutes() + ':' + end.getSeconds();
                    		var discount = self.discount
                    		var url = URLHeader.ShoppingMall+'/api/SaleActivity/AddSaleActivity'
                    		
//                  		public int SkuId { get; set; }
//						public DateTime StartTime { get; set; }
//						public DateTime EndTime { get; set; }
//						public int SalePrice { get; set; }
//						public int Limit { get; set; }
						var DATA = '{"SkuId":'+self.SkuId+',"StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","SalePrice":'+discount.SalePrice+',"Limit":'+discount.Limit+'}'
						console.log(DATA)
                    		$.ajax({
                    			type:"post",
                    			url:url,
                    			data:DATA,
                    			dataType:'json',
                    			contentType: "application/json; charset=utf-8",
                    			success:function(json){
                    				console.log(json)
                    				if (json=='1') {
                    					self.$Message.success('添加成功');
                    					self.searchDetails(self.SkuId)
                    				} else{
                    					self.$Message.error('添加失败');
                    				}
                    				
                    			},
                    			 error : function(error) { 
                    			 	self.$Message.error('添加失败');
					        		console.log(error)
					        }
                    		});
                    		
                    		
                    		//console.log(self.discount)
                    } else {
                        this.$Message.error('请将数据填写完整');
                    }
                })
			},
			ModifyDiscount(name){//修改
				var self = this
				
				console.log( this.$refs[name])
				 this.$refs[name].validate((valid) => {
                    if (valid) {
                    		//时间转换
                    		//CST
                    		//时间转换
                    		var star = new Date(self.discount.StartTime);  
						var StartTime = star.getFullYear() + '-' + (star.getMonth() + 1) + '-' + star.getDate() + ' ' + star.getHours() + ':' + star.getMinutes() + ':' + star.getSeconds();
                    		var end = new Date(self.discount.EndTime);  
						var EndTime = end.getFullYear() + '-' + (end.getMonth() + 1) + '-' + end.getDate() + ' ' + end.getHours() + ':' + end.getMinutes() + ':' + end.getSeconds();
                    		var discount = self.discount
                    		var url = URLHeader.ShoppingMall+'/api/SaleActivity/UpdateSkuSale'
                    		var DATA = '{"SkuId":'+self.SkuId+',"StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","SalePrice":'+discount.SalePrice+',"Limit":'+discount.Limit+'}'
						console.log(DATA)
                    		$.ajax({
                    			type:"post",
                    			url:url,
                    			data:DATA,
                    			dataType:'json',
                    			contentType: "application/json; charset=utf-8",
                    			success:function(json){
                    				console.log(json)
                    				if (json=='1') {
                    					self.$Message.success('修改成功');
                    				} else{
                    					self.$Message.error('修改成功');
                    				}
                    				
                    			},
                    			 error : function(error) { 
                    			 	self.$Message.error('修改成功');
					        		console.log(error)
					        }
                    		});
                    		console.log(self.discount)
                       
                    } else {
                        this.$Message.error('请将数据填写完整');
                    }
                })
			},
			searchDetails(SkuId){
				var url = URLHeader.ShoppingMall+'api/SaleActivity/FindSkuSale?skuId='+SkuId
				console.log(url)
				var self =this
				$('.loding').show()
            		$.ajax({
            			type:"post",
            			url:url,
            			dataType:'json',
            			contentType: "application/json; charset=utf-8",
            			success:function(json){
            				if (json=='null') {//新增
            					self.addModel =true
            					//console.log('不存在')
            				} else{//编辑
            				 	 self.addModel =false
            					 var dataAll =JSON.parse(json)
            					 //new Date(1000 * 60 * 1)
            					 dataAll.StartTime = new Date(dataAll.StartTime)
            					 dataAll.EndTime = new Date(dataAll.EndTime)
            					 self.discount = dataAll  
            					 //console.log('存在')
						}
            				$('.loding').hide()
            				self.$refs['discount'].resetFields();
            				//console.log(self.discount)
            			},
            			 error : function(error) { 
			        		console.log(error)
			        }
            		});
			}
		},
		mounted:function(){
			//this.router
			//this.$route.query.gs_code
			this.SkuId =this.$route.query.SkuId
			//3086
//			this.SkuId = 3086
			this.searchDetails(this.SkuId)
			//console.log()
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
.SinglePromotions{
	border: 1px solid gray;
	padding: 1rem;
	border-radius: 8px;
	width: 30rem;
	margin: 0 auto;
}
.SinglePromotions .SinglePromotions_top{
	margin-top: 1rem;
	margin-left: 1rem;
	padding-bottom: 1rem;
	border-bottom: 1px dashed grey;
	text-align: center;
}
.SinglePromotions .discount{
	margin-top: 1rem;
	margin-left: 1rem;
}
</style>