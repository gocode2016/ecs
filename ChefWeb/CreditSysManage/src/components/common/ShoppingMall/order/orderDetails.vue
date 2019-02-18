<template>
	<div class="orderDetails">
		<div class="orderDetailsTop">
			<div style="margin-bottom: 1rem;overflow: auto;">
				<ul class="orderDetailsTop_Ul">
					<li class="orderDetailsTop_Li">
						<span>订单编号</span>
						<span class="value">{{orderData.OrderId}}</span>
					</li>
					<li class="orderDetailsTop_Li">
						<span>下单会员</span>
						<span class="value">{{orderData.MemberName}}</span>
					</li>
				</ul>
				<ul class="orderDetailsTop_Ul" style="width: 17rem;">
					<li class="orderDetailsTop_Li">
						<span>下单会员ID</span>
						<span class="value" style="margin-left: 0.8rem;">{{orderData.MemberId}}</span>
					</li>
					<li class="orderDetailsTop_Li">
						<span>下单时间</span>
						<span class="value">{{orderData.AddDate}}</span>
					</li>
				</ul>
				<ul class="orderDetailsTop_Ul" style="width: 12rem;">
					<li class="orderDetailsTop_Li">
						<span>订单来源</span>
						<span class="value">{{orderData.OrderFrom}}</span>
					</li>
					<li class="orderDetailsTop_Li">
						<span>订单分类</span>
						<span class="value">{{orderData.OrderType}}</span>
					</li>
				</ul>
				<ul class="orderDetailsTop_Ul" style="width: 10rem;">
					<li class="orderDetailsTop_Li">
						<span>消费积分</span>
						<span class="value">{{orderData.OrderPrice}}</span>
					</li>
					<li class="orderDetailsTop_Li">
						<span>订单状态</span>
						<span class="value">{{orderData.OrderState}}</span>
					</li>
				</ul>
			</div>
			<div style="position: relative;overflow: auto;">
				<Button type="primary" style="display: block;float: left;margin-left: 50rem;" @click="nullify" v-if='nullifyHiden'>作废</Button>
				<Button type="primary" style="display: block;float: left;margin-left: 1.5rem;" @click="shipping" v-if="shipingHide">发货</Button>
				
			</div>
			
			<!--<Button type="primary" style="margin-left: 0.5rem;" @click="helpDetails">求助详情</Button>-->
			<Modal v-model="modalDeletShow" width="360">
		        <p slot="header" style="color:#f60;text-align:center">
		            <Icon type="information-circled"></Icon>
		            <span>作废确认</span>
		        </p>
		        <div style="text-align:center">
		            <p>您确定要作废此规则吗?</p>
		            <p></p>
		        </div>
		        <div slot="footer">
		            <Button type="error" size="large" long  @click="deletOrder">作废</Button>
		        </div>
		    </Modal>
			<Modal
		        title="发货快递"
		        v-model="shippingModel"
		        @on-ok="shippingModelOk"
		        :mask-closable="false">
		        <ul style="margin-left: 2rem;">
			        	<li style="margin-bottom: 1rem;">
			        		<span>快递</span>
			        		<Select v-model="shippingData.LogisticsType" style="width:200px;margin-left: 2.5rem;">
					        <Option v-for="item in ExpressdeliveryList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
			        	</li>
		        		<li style="margin-bottom: 1rem ;">
		        			<span>快递单号</span>
		        			<Input v-model="shippingData.LogisticsNo" placeholder="快递单号" style="width: 200px;margin-left: 1rem;"></Input>
		        		</li>
		        </ul>
		    </Modal>
		    <Modal
		        title="求助详情"
		        v-model="HelpDetailsModel"
		        width='520'
		        :mask-closable="false">
		        <ul style="margin-left: 1rem;overflow: auto;border-bottom: 1px dashed gainsboro;">
			        	<li style="margin-bottom: 1rem;display: block;float: left;width: 13rem;">
			        		<span>截止时间</span>
			        		<span style="margin-left: 1rem;">2017年7月13日11:32:44</span>
			        	</li>
		        		<li style="margin-bottom: 1rem;display: block;float: left;width: 13rem;text-align: right;">
		        			<span>已筹集</span>
		        			<span style="margin-left: 1rem;">199分</span>
		        		</li>
		        		<li style="margin-bottom: 1rem;display: block;float: left;width: 13rem;">
			        		<span>当前状态</span>
			        		<span style="margin-left: 1rem;">进行中</span>
			        	</li>
		        		<li style="margin-bottom: 1rem;display: block;float: left;width: 13rem;text-align: right;">
		        			<span>目标积分</span>
		        			<span style="margin-left: 1rem;">600分</span>
		        		</li>
		        </ul>
		        <Table :columns="HelpDetailsData.helpColumns" :data="HelpDetailsData.helpList" style="width: 450px;margin-top: 1rem;margin-left: 1.5rem;"></Table>
		    </Modal>
		</div>
		<div class="orderDetailsBody">
			<ul class="orderDetailsBody_Ul">
					<li class="orderDetailsBody_Li" >
						<span>收货人</span>
						<span class="value" style="margin-left: 3rem;">{{orderData.OrderName}}</span>
					</li>
					<li class="orderDetailsBody_Li">
						<span>收货人手机</span>
						<span class="value">{{orderData.OrderTelephone}}</span>
					</li>
					<li class="orderDetailsBody_Li">
						<span>收货地址</span>
						<span class="value" style="margin-left: 2.4rem;">{{orderData.OrderAddress}}</span>
					</li>
				</ul>
				<div style="float: left;">
					<ul class="logistics_Ul logistics" style="height: 17rem;overflow: auto;border: 1px dashed gainsboro;border-radius: 4px;padding:0 1rem;">
						<li >
							<span>物流信息</span>
							<span style="margin-left: 1rem;"> 
								{{orderData.LogisticsType}} {{orderData.LogisticsNo}}
							</span>
						</li>
						<li class="logistics_Li" v-for="item in LogisticsInformation" style="overflow: auto;margin-top: 0.8rem;">
							<span style="display: block;float: left;">{{item.ftime}}</span>
							<span style="display: block;float:right;width: 17rem;" class="value">{{item.context}}</span>
						</li>
					</ul>
				</div>
				
		</div>
		<div class="orderDetailsFooter">
			<div class="foot_left" style="padding-right: 1rem;">
				<Table :columns="orderDetails_columns_left" :data="leftData" style="margin-top: 1rem;"></Table>
			</div>
			<div class="foot_right">
				<h3 style="margin-left: 2rem;margin-top: 1rem;">备注信息</h3>
				<p style="margin-left: 3rem;margin-top: 0.5rem;">{{orderData.OrderRemark}}</p>
				<Button type="primary" style="display: block;float: right;margin-right: 3rem;" @click="editBtn">备注编辑</Button>
				<Modal
			        title="备注信息"
			        v-model="remarkModel"
			        @on-ok="remarkModelOk"
			        :mask-closable="false">
						<Input v-model="remarkModelText" :autosize="{minRows: 4,maxRows: 8}" type="textarea"  style="width: 25rem;margin-left: 3rem;" placeholder="请输入备注内容"></Input>
			   </Modal>
			</div>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				LogisticsInformation:[],//物流信息
				modalDeletShow:false,//删除确认弹框
				OrderId:'',
				shipingHide:true,//发货按钮是否隐藏
				nullifyHiden:true,//作废按钮是副隐藏
				orderData:{},//订单数据
				shippingModel:false,//发货弹框
				shippingData:{//发货弹框数据
					LogisticsType:'',//快递类型
					LogisticsNo:'',//快递单号
				},
				HelpDetailsModel:false,//求助详情弹框
				HelpDetailsData:{
					helpList:[//求助数据
						{
							meberName:'吴洪根',
							time:'2017年7月13日11:32:44',
							price:'99分'
						}
					],
					helpColumns:[//求助列表
						{
							title:'热心会员',
							align: 'center',
							key:'meberName',
						},
						{
							title:'转增时间',
							align: 'center',
							width:120,
							key:'time',
						},
						{
							title:'转增额',
							align: 'center',
							key:'price',
						},
					],
				},
				remarkModel:false,//备注弹框
				remarkModelText:'',//编辑备注弹框
				orderDetails_columns_left:[//订单商品表头
					{
						title:'No.',
						align: 'center',
						type:'index',
					},
					{
						title:'商品名称',
						align: 'center',
						key:'SkuName',
					},
					{
						title:'物料编码',
						align: 'center',
						key:'SkuCode',
					},
//					{
//						title:'规格',
//						align: 'center',
//						key:'ProductSpecification',
//					},
					{
						title:'数量',
						align: 'center',
						key:'Count',
					},
				],
				leftData:[//左侧数据
//					{
//						ProductName:'味达美定制厨师服',
//						ProductCode:'3242134',
//						ProductSpecification:'XXL     红色',
//						ProductNum:'12'
//					}
				],
				ExpressdeliveryList:[//快递数据
					{
                        value: '申通',
                        label: '申通'
                    },
                    {
                        value: '圆通',
                        label: '圆通'
                    },
                    {
                        value: '中通',
                        label: '中通'
                    },
                    {
                        value: '韵达',
                        label: '韵达'
                    },
                    {
                        value: 'EMS',
                        label: 'EMS'
                    },
                    {
                        value: '汇通',
                        label: '汇通'
                    },
				],
				
			}
		},
		methods:{
			shipping(){//发货按钮
				console.log('发货')
				this.shippingData.LogisticsNo =''
				this.shippingData.LogisticsType=''
				this.shippingModel =true
			},
			shippingModelOk(){//快递确定
				var self =this
				if (this.shippingData.LogisticsNo && this.shippingData.LogisticsType) {
					var DATA = '{"OrderId":"'+this.OrderId+'","LogisticsNo":"'+this.shippingData.LogisticsNo+'","LogisticsType":"'+this.shippingData.LogisticsType+'"}'
					console.log(DATA)
					$.ajax({
				        type :"post",
				        url :URLHeader.ShoppingMall+'/api/Order/SendLogistics',///cts/core/
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:DATA,
				        success : function(json) {
				        	 console.log(json)
				        	 self.requestOrderData()
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		    });
				} else{
					self.$Message.warning('快递信息未填写完整,上传失败');
				}	
			},
			nullify(){//作废按钮
				console.log('作废')
				this.modalDeletShow=true
			},
			deletOrder(){//删除确认
				var self =this
				var DATA1 = '{"OrderId":'+self.OrderId+',"State":"已取消"}'
				
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/UpdateOrderState',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA1,
			        success : function(json) {
			        	console.log(DATA1)
			        	 console.log(json)
			        	// self.requestOrderData()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    var DATA2 = '{"OrderId":'+self.OrderId+',"MemberId":'+self.orderData.MemberId+',"Integral":'+self.orderData.OrderPrice+',}'
	   		  
	   		    $.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/CallBack',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA2,
			        success : function(json) {
			        	  console.log(DATA2)
			        	 console.log(json)
			        	// self.requestOrderData()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
				this.modalDeletShow=false
			},
			helpDetails(){//求助按钮
				console.log('求助详情')
				this.HelpDetailsModel=true
			},
			editBtn(){//编辑按钮
				this.remarkModel = true
				this.remarkModelText = this.orderData.OrderRemark

			},
			remarkModelOk(){//备注弹框Ok按钮
				var self =this
				var DATA = '{"OrderId":"'+this.OrderId+'","Remark":"'+this.remarkModelText+'"}'
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/UpdateRemark',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 console.log(json)
			        	 self.requestOrderData()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			requestOrderData(){
				var self =this
				$.ajax({//订单基本信息
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Order/GetOrderInfo?orderId='+self.OrderId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)[0]
			        	  
			        	  
			        	  
			        	 
			        	  console.log(JSON.parse(json)[0])
			        	  	if (dataAll.OrderType==1) {//实体
			        	  		console.log('实体商品')
			        	  		dataAll.OrderType='实体商品'
			        	  		//=====================发货按钮操作
			        	  		if (dataAll.LogisticsNo=='') {//未操作 发货按钮
				        			self.shipingHide = true
				        		} else{//已操作  发货按钮
				        			self.shipingHide = false
				        		}
				        		console.log(self.shipingHide)
				        		//=======================作废按钮操作
				        		if (dataAll.OrderState=='备货中' || dataAll.OrderState=='未提交') {
				        			self.nullifyHiden = true//作废按钮显示
				        		} else{
				        			self.nullifyHiden = false//作废按钮隐藏
				        		}
							
			        	  	} else{//虚拟
			        	  		dataAll.OrderType='虚拟商品'
			        	  		self.shipingHide = false//发货按钮全部隐藏
			        	  		self.nullifyHiden = false//作废按钮隐藏
			        	  	}	
			        	  	 self.orderData = dataAll
			        	  	//=======================是否显示物流信息
			        	  	if (dataAll.LogisticsNo!='') {
			        	  		//LogisticsNo物流单号
			        	  		//LogisticsType物流名称
			        	  		//self.requestLogisticsInformation('申通','3345093768311')
			        	  		self.requestLogisticsInformation(dataAll.LogisticsType,dataAll.LogisticsNo)
			        	  	}else{
			        	  		$('.logistics').hide()
			        	  		//self.requestLogisticsInformation('申通','3345093768311')
			        	  	}
			        	  	
			        	  	
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    console.log(URLHeader.ShoppingMall+'/api/Order/GetOrderSKU?orderId='+self.OrderId)
	   		    $.ajax({//订单商品数据
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Order/GetOrderSKU?orderId='+self.OrderId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	    var dataAll =JSON.parse(json)
			        	    //console.log(JSON.parse(json))
			        		self.leftData = dataAll
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });    
			},
			requestLogisticsInformation(code,postId){//物流信息请求
				var self =this
				var codeStr = ''
				if (code=='中通') {
					codeStr='zhongtong'
				}else if (code=='申通') {
					codeStr='shentong'
				}else if (code=='圆通') {
					codeStr='yuantong'
				}else if (code=='EMS') {
					codeStr='ems'
				}else if (code=='韵达') {
					codeStr='yunda'
				}else if (code=='汇通') {
					codeStr='huitongkuaidi'
				}

//				var code = 'shentong';//物流拼音
//				var postId='3345093768311';//物流单号
				$.ajax({
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Order/GetLogisticsInfo?code='+codeStr+'&postId='+postId,///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	 var dataAll =JSON.parse(json).data
			        	 self.LogisticsInformation= dataAll
			        	 console.log(JSON.parse(json))
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			
		},
		mounted:function(){
			this.OrderId= window.location.hash.split('OrderId=')[1];
			this.requestOrderData()
			
			var leftHeight = $('.foot_left').height()
			var rightHeight = $('.foot_right').height()
			if (leftHeight>rightHeight) {
				$('.foot_left').css({
					borderRight: '1px dashed gainsboro'
				})
			} else{
				$('.foot_right').css({
					borderLeft: '1px dashed gainsboro'
				})
			}
			
			
			
		}
	}
</script>

<style scoped>
.orderDetails{
	border: 1px solid gainsboro;
	overflow: auto;
	margin-left: 0;
	/*width: 64rem;*/
	padding: 1rem;
	border-radius: 6px;
}
.orderDetails .orderDetailsTop{
	overflow: auto;
	margin-bottom: 1rem;
}
.orderDetails .orderDetailsTop .orderDetailsTop_Ul{
	display: block;
	float: left;
	width: 13rem;
	margin-left: 2rem;
}
.orderDetails .orderDetailsTop .orderDetailsTop_Ul .orderDetailsTop_Li{
	margin-top: 0.8rem;
}
.orderDetails .orderDetailsTop .orderDetailsTop_Ul .orderDetailsTop_Li .value{
	margin-left: 1.5rem;
}
/*body*/
.orderDetails .orderDetailsBody{
	border-bottom: 1px dashed gainsboro;
	border-top: 1px dashed gainsboro;
	overflow: auto;
	padding: 1rem 0;
}
.orderDetails .orderDetailsBody .orderDetailsBody_Ul{
	display: block;
	float: left;
	width: 25rem;
	margin-left: 2rem;
}
.orderDetails .orderDetailsBody .orderDetailsBody_Ul .orderDetailsBody_Li{
	margin-top: 0.8rem;
	
}
.orderDetails .orderDetailsBody .orderDetailsBody_Ul .orderDetailsBody_Li .value{
	margin-left: 1.5rem;
}
.orderDetails .orderDetailsBody .logistics_Ul{
	display: block;
	float: left;
	width: 34rem;
}
.orderDetails .orderDetailsBody .logistics_Ul li {
	margin-top: 0.5rem;
}
.orderDetails .orderDetailsBody .logistics_Ul .value{
	/*margin-left: 1rem;*/
	margin-right: 1rem;
}
.orderDetails .orderDetailsBody .logistics_Ul .logistics_Li{
	margin-top: 0.2rem;
	margin-left: 4rem;
}
/*foot*/
.orderDetails .orderDetailsFooter{
	overflow: auto;
	width: 64rem;
}
.orderDetails .orderDetailsFooter .foot_left,.orderDetails .orderDetailsFooter .foot_right{
	float: left;
	margin: 0 auto;
	width: 31rem;
	overflow: auto;
}
</style>