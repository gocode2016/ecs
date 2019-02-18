<template>
	<div class="orderList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="orderListTop">
			<ul class="TopUl">
				<li class="TopLi">
					<!--<span>商品类型</span>-->
					<span>订单分类:</span>
					<Select v-model="searchCriteria.OrderType" style="width:100px;position: relative;" placeholder = '订单分类'>
				        <Option v-for="item in orderTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>订单状态:</span>
					<Select v-model="searchCriteria.OrderState" style="width:100px" placeholder = '订单状态'>
				        <Option v-for="item in orderStateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<p style="font-size:1.5rem ;">|</p>
				</li>
				<!--<li class="TopLi">
					<Input v-model="searchCriteria.orderNumber" placeholder="订单号" style="width: 100px"></Input>
				</li>-->
				<li class="TopLi">
					<Input v-model="searchCriteria.OrderTelephone" placeholder="会员手机" style="width: 100px"></Input>
				</li>
				<li class="TopLi">
					<Input v-model="searchCriteria.OrderName" placeholder="收货人" style="width: 100px"></Input>
				</li>
				<li class="TopLi">
    					<DatePicker v-model="searchCriteria.searchTime" format="yyyy/MM/dd" type="daterange" placement="bottom-end" placeholder="选择日期" style="float: right;width: 11rem;" class='time'></DatePicker>
				</li>
				<li class="TopLi">
					<Button type="primary" icon="ios-search" @click ='searchGoods'>搜索</Button>
				</li>
				<li class="TopLi">
					<Button type="primary" @click='importLogistics'>导入物流</Button>
					 <input type="file" @change="importf()"  id="import" style="display: none;"/>
				</li>
				<li class="TopLi">
					<Button type="primary" @click='DownloadTemplate'>模板下载</Button>
				</li>
				<li class="TopLi">
					<Button type="primary" @click='exportCsv'>订单导出</Button>
				</li>
				
			</ul>
		</div>
		<div class="body">
			<Modal
		        title="备注信息"
		        v-model="remarkModel"
		        @on-ok="remarkModelOk"
		        :mask-closable="false">
					<Input v-model="remark.remarkModelText"   type="textarea" :autosize="{minRows: 4,maxRows: 8}" style="width: 25rem;margin-left: 3rem;" placeholder="请输入备注内容"></Input>
		    </Modal>
			<Table :columns="GoodsColumns" :data="orderList" ref="table"></Table>
			
			<Page :total="total" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>
<!--<script type="text/javascript" src="../../../../../static/js/xlsx.full.min.js" ></script>-->
<script>
//	import validate from '../../../../../static/js/xlsx.full.min.js'
//	Vue.use(validate);
	export default{
		data(){
			return{
				remarkModel:false,//弹框隐藏
				remark:{
					remarkModelText:'',//备注编辑框
					OrderId:'',//订单id
				},
				total:0,//总条数
				searchCriteria:{//搜索条件
					OrderType:0,//订单分类
					OrderState:'全部',//订单状态
//					orderNumber:'',//订单号
					OrderTelephone:'',//会员手机
					OrderName:'',//订单名称
					searchTime:[],//时间
				},
				 logisticsList:[],//上传物流数据
				orderList:[
				],//表格数据
				orderStateList:[//订单分类
					{
                        value: '全部',
                        label: '全部'
                    },
                    {
                        value: '求助中',
                        label: '求助中'
                    },
                    {
                        value: '未提交',
                        label: '未提交'
                    },
                    {
                        value: '备货中',
                        label: '备货中'
                    },
                    {
                        value: '已发货',
                        label: '已发货'
                    },
                    
				],
				orderTypeList:[//订单分类
					{
                        value: 0,
                        label: '全部'
                    },
                     {
                        value: 1,
                        label: '实体交易'
                    },
                    {
                        value: 2,
                        label: '虚拟交易'
                    },
                   
				],
				GoodsColumns:[//商品;列表表头
//					{
//						title:'',
//						align: 'center',
//						width:60,
//						type:'selection',
//					},
					{
						title:'No.',
						width:60,
						align: 'center',
						type:'index',
					},
					{
						title:'订单编号',
						align: 'center',
						key:'OrderId',
						width:90,
						render: (h, params) => {
                            return h('div', [
                                h('span', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    style: {
										color:'red',
										cursor:'pointer'
                                    },
                                    on: {
                                        click: () => {
                                            this.OrderDetail(params)
                                        }
                                    }
                                   
                                }, params.row.OrderId),
                            ]);
                        }
					},
//					{
//						title:'下单会员',
//						align: 'center',
//						key:'OrderName',
//						
//					},
					{
						title:'下单会员ID',
						align: 'center',
						width:100,
						key:'MemberId',
					},
					{
						title:'收货人',
						align: 'center',
						key:'OrderName',
					},
					{
						title:'收货人手机',
						align: 'center',
						key:'OrderTelephone',
						width:110,
					},
					{
						title:'收货人地址',
						align: 'center',
						key:'OrderAddress',
					},
//					{
//						title:'订单来源',
//						align: 'center',
//						key:'ProductId',
//					},
					{
						title:'订单分类',
						align: 'center',
						key:'OrderTypeStr',
						width:85,
					},
					{
						title:'消费积分',
						align: 'center',
						key:'OrderPrice',
					},
					{
						title:'订单状态',
						align: 'center',
						key:'OrderState',
					},
					{
						title:'下单时间',
						align: 'center',
						width:110,
						key:'AddDate',
					},
					{
						title:'订单来源',
						align: 'center',
						key:'OrderFrom',
					},
					{
						title:'操作',
						align: 'center',
						render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    style: {
										width:'3rem'
                                    },
                                    on: {
                                        click: () => {
                                            this.Orderremark(params)
                                        }
                                    }
                                }, '备注'),
                               
                            ]);
                        }
					},
				],
				exportCsv_TableData:[],
				exportCsv_Columns:[
					{
						title:'订单编号',
						key:'OrderId',
					},
					{
						title:'会员ID',
						key:'MemberId',
					},
					{
						title:'会员名',
						key:'MemberName',
					},
					{
						title:'收货人姓名',
						key:'OrderName',
					},
					{
						title:'收货地址',
						key:'OrderAddress',
					},
					{
						title:'收货电话',
						key:'OrderTelephone',
					},
					{
						title:'下单时间',
						key:'AddDate',
					},
					{
						title:'单品名称',
						key:'SkuName',
					},
					{
						title:'数量',
						key:'Count',
					},
					{
						title:'订单来源',
						key:'OrderFrom',
					},
					{
						title:'备注',
						key:'OrderRemark',
					},
					{
						title:'订单状态',
						key:'OrderState',
					},
				]
			}
		},
		methods:{
			exportCsv(){//订单导出
				var self =this
				self.exportCsv_TableData = []
				 $('.loding').show()
				var StartTime=''//开始时间
				var EndTime = ''//结束时间
				if (self.searchCriteria.searchTime.length>0) {
					var one = new Date(self.searchCriteria.searchTime[0]); 
					var two = new Date(self.searchCriteria.searchTime[1]);
					 StartTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 EndTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
				}else{
					StartTime= ''
					EndTime = ''
				}
				var DATA = '{"OrderType":'+self.searchCriteria.OrderType+',"OrderState":"'+self.searchCriteria.OrderState+'","OrderId":"","OrderTelephone":"'+self.searchCriteria.OrderTelephone +'","OrderName":"'+self.searchCriteria.OrderName+'","StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","MemberId":0}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/PrintOrderList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  	var dataAll =JSON.parse(json)
			        	 	 self.exportCsv_TableData = dataAll
//			        	 	 self.exportCsv_TableData.map((item) =>{
//		        	 	 	    console.log(item.OrderRemark)
//			        	 	 })
			        	 	// console.log(self.exportCsv_TableData)
			        	 	self.$refs.table.exportCsv({
		                    filename: '订单导出',
		                    columns: self.exportCsv_Columns,
		                    data: self.exportCsv_TableData
		                 });
						 $('.loding').hide()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		   });
			},
			expor(){
				var self = this
				 console.log(self.exportCsv_TableData)
				
			},
			OrderDetail(name){//详情
				console.log('详情')
				this.$router.push({path: '/orderDetails',query: {OrderId:name.row.OrderId}});//详情页面
				//this.$router.push({path: '/orderDetails'});//详情页面
			},
			Orderremark(name){//备注操作
				this.remark.OrderId=name.row.OrderId
				this.remark.remarkModelText=name.row.OrderRemark
				this.remarkModel=true
				console.log(this.remark)
			},
			remarkModelOk(){//备注弹框OkBtn
				
				var self =this
				var DATA = '{"OrderId":"'+this.remark.OrderId+'","Remark":"'+this.remark.remarkModelText+'"}'
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/UpdateRemark',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 console.log(json)
			        	 self.requestTableData(1)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			searchGoods(){//搜索按钮
				this.requestTableData(1)
				console.log('搜索开始')
			},
			importLogistics(){//导入物流
				$('#import').click()
			},
			importf(){//导入函数
				var self =this
				 var wb;//读取完成的数据
				var obj = document.getElementById("import");
				if(!obj.files) {
                    return;
                }
                var f = obj.files[0];
                var reader = new FileReader();
                reader.onload = function(e) {
                    var data = e.target.result;
                        wb = XLSX.read(data, {
                            type: 'binary'
                        });
                    //wb.SheetNames[0]是获取Sheets中第一个Sheet的名字
                    //wb.Sheets[Sheet名]获取第一个Sheet的数据
                   // this.logisticsList= XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]])
                  
                 	XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]).map((item) =>{
                 		var one ={"OrderId":null,'LogisiticsNo':'','LogisiticsType':''}
                 		one.OrderId = item['订单号']
                 		one.LogisiticsNo = item['物流单号']
                 		one.LogisiticsType = item['物流公司']
                 		self.logisticsList.push(one)
               	  })
                    console.log( self.logisticsList)
                    $.ajax({
				        type :"post",
				        url :URLHeader.ShoppingMall+'/api/Order/ImprotLogistics',///cts/core/
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:'{"List":'+JSON.stringify(self.logisticsList)+'}',
				        success : function(json) {
							console.log(json)				        	 
				        },
				        error : function(error) { 
				        		console.log(error)
				        		self.$Message.error('数据上传失败');
				        }
		   		    });
                };
                    reader.readAsBinaryString(f);
                    
			},
			
			changePage(index){//页面切换
				console.log('当前页码'+index)
				this.requestTableData(index)
			},
			requestTableData(index){
				//:8004/api/Product/GetProductList
				var self = this
				self.orderList = []
				$('.loding').show()
				var StartTime=''//开始时间
				var EndTime = ''//结束时间
				if (self.searchCriteria.searchTime.length>0) {
					var one = new Date(self.searchCriteria.searchTime[0]); 
					var two = new Date(self.searchCriteria.searchTime[1]);
					 StartTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 EndTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
				}else{
					StartTime= ''
					EndTime = ''
				}
				var DATA = '{"OrderType":'+self.searchCriteria.OrderType+',"OrderState":"'+self.searchCriteria.OrderState+'","OrderId":"","OrderTelephone":"'+self.searchCriteria.OrderTelephone +'","OrderName":"'+self.searchCriteria.OrderName+'","StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","PageSize":20,"IndexPage":'+index+',"MemberId":0}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/GetOrderList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
			        	  console.log(JSON.parse(json))
			        	  self.total = parseInt(dataAll.Count)
			        	  dataAll.data.map((item)=>{
			        	  	if (item.OrderType==1) {
			        	  		item.OrderTypeStr = '实体交易'
			        	  	} else{
			        	  		item.OrderTypeStr = '虚拟交易'
			        	  	}
			        	  })
			        	  self.orderList= dataAll.data
			        	  $('.loding').hide()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			DownloadTemplate(){//模板下载
				window.location.href =URLHeader.Tools+'/UploadFiles/Logistics.xlsx'
			}
		},
		mounted:function(){
			this.requestTableData(1)
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
.orderList {
	margin: 0 auto;
	padding: 0
}
.orderList .orderListTop{
	border-bottom: 1px solid gainsboro;
}
.orderList .orderListTop .TopUl{
	height: 2.9rem;
	width: 73rem;
	/*margin-left: 2rem;*/
}
.orderList .orderListTop .TopUl .TopLi {
	display: block;
	float: left;
	margin-left: 1rem;
	height: 2rem;
	line-height: 2rem;
	
}
.orderList .body .typeUl .typeLi{
	border: 1px solid gainsboro;
}
</style>