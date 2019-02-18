<template>
	<div class="SingleList">
		<div class="SingleProductListTop">
			<ul class="TopUl">
				<li class="TopLi">
					<span>商品名称:</span>
					<Input v-model="searchCriteria.searchData" placeholder="商品名称" style="width: 130px;margin-left: 0.8rem;"></Input>
				</li>
				<li class="TopLi">
					<span>商品编码:</span>
					<Input v-model="searchCriteria.ProductId" placeholder="商品编码" style="width: 130px;margin-left: 0.8rem;"></Input>
				</li>
				<li class="TopLi">
					<span>单品编码:</span>
					<Input v-model="searchCriteria.SkuCode" placeholder="单品编码" style="width: 130px;margin-left: 0.8rem;"></Input>
				</li>
				<li class="TopLi">
					<Button type="ghost" icon="ios-search" @click ='searchSingleProduct'>搜索</Button>
				</li>
				<li class="TopLi" >
					<Button type="primary" @click = 'batchOperation(1)'>批量下架</Button>
				</li>
				<li class="TopLi" >
					<Button type="primary" @click = 'batchOperation(0)'>批量上架</Button>
				</li>
				<!--<li class="TopLi">
					<Button type="primary" @click='deleteSingleProduct'>删除单品</Button>
				</li>-->
				<li class="TopLi" >
					<Button type="primary" @click = 'addSingleProduct'>新增单品</Button>
				</li>
				
			</ul>
		</div>
		<div class="body">
			<Table :columns="SingleProductColumns" :data="SingleProductList" @on-selection-change='selectChange'></Table>
			<Page :total="total" show-total  :page-size='10' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				total:0,//总条数
				selectData:[],//选中列表
				searchCriteria:{//搜索条件
					SingleProductType:'0',//商品类型
					SingleProductcLassification:'0',//商品分类
					searchData:'',//搜索条件
					ProductId:'',//商品编码 接受
					ProductIdStr:'',//商品编码 发送
					SkuCode:'',//单品编码
				},
				SingleProductList:[//商品列表数据源
				],
				SingleProductcLassificationList:[//商品分类
					{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '1000',
                        label: '读书一角'
                    },
                    {
                        value: '1001',
                        label: '试用专区'
                    },
                    {
                        value: '1002',
                        label: '活动名额'
                    },
                    {
                        value: '1003',
                        label: '后厨物料'
                    },
                    {
                        value: '1004',
                        label: '健康美味'
                    },
                    {
                        value: '1005',
                        label: '生活用品'
                    },
                    
				],
				SingleProductTypeList:[//商品类型
					{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '1',
                        label: '虚拟物品'
                    },
                    {
                        value: '2',
                        label: '实体物品'
                    },
				],
				SingleProductColumns:[//商品;列表表头
					{
						title:'',
						align: 'center',
						width:60,
						type:'selection',
					},
					{
						title:'No.',
						align: 'center',
						type:'index'
					},
					{
						title:'单品名称',
						align: 'center',
						key:'SkuName',
					},
					{
						title:'单品编码',
						align: 'center',
						key:'SkuCode',
					},
					{
						title:'库存',
						align: 'center',
						key:'Stock',
					},
					{
						title:'售价',
						align: 'center',
						key:'Price',
					},
					{
						title:'状态',
						align: 'center',
						key:'IsEnableStr',
					},
					{
						title:'市场价',
						align: 'center',
						key:'MarketPrice',
					},
					{
						title:'添加时间',
						align: 'center',
						width:100,
						key:'AddDate',
					},
					{
						title:'操作',
						width:170,
						align: 'center',
						render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    style: {
//                                      marginRight: '5px'
//                                      float:'left'
                                    },
                                    on: {
                                        click: () => {
                                            this.SingleProductEdit(params)
                                        }
                                    }
                                }, '编辑'),
                                h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                    style: {
                                      marginLeft: '1rem'
//                                      float:'left'
                                    },
                                    on: {
                                        click: () => {
                                        	//SalesPromotion
                                            this.SingleProductPromotion(params)
                                        }
                                    }
                                }, '促销'),
                            ]);
                        }
					},
				],
			}
		},
		methods:{
			batchOperation(type){//1为下架0为上架
				//api/SKU/IsShowSKU?excType={excType}
				var self =this
				console.log(type)
				var SKUid = [];
				if (this.selectData.length>0) {
					for (var i = 0;i<self.selectData.length;i++) {
						SKUid.push(self.selectData[i].SkuId)
					}
					var DATA = JSON.stringify(SKUid)
					$.ajax({
				        type :"post",
				        url :URLHeader.ShoppingMall+'/api/SKU/IsShowSKU?excType='+type,
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:DATA,
				        success : function(json) {
				        	   console.log(json)
				        	   if (type==1) {
				        	   		self.$Message.success('数据下架成功');
				        	   }else{
				        	   		self.$Message.success('数据上架成功成功');
				        	   }
				        	   self.requestTableData(1)
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		    });
		   		    self.requestTableData(1)
				} else{
					self.$Message.warning('请勾选数据');
				}
				
				
				
				
				
				
			},
			selectChange(name){
				this.selectData = []
				this.selectData = name
			},
			SingleProductEdit(name){//商品编辑
				console.log(name)
					this.$router.push({path: '/SingleAdd', query: {NormsId:name.row.NormsId}});//单品编辑页
					//this.$router.push({path: '/SingleAdd', query: {NormsId:name.row.NormsId}});//单品编辑页
			},
			SingleProductPromotion(name){//促销
				this.$router.push({path: '/SinglePromotions', query: {SkuId:name.row.SkuId}});
			},
			searchSingleProduct(){//搜索按钮
				this.requestTableData(1)
				console.log('搜索开始')
			},
			addSingleProduct(name){//新增商品按钮
					this.$router.push({path: '/SingleAdd'});//跳转实体商品新增
				
			},
			changePage(index){//页面切换
				console.log('当前页码'+index)
				this.requestTableData(index)
			},
			requestTableData(index){
				//:8004/api/Product/GetProductList
				var self = this
				self.SingleProductList = []
				
				if (self.searchCriteria.ProductId=='') {
					self.searchCriteria.ProductIdStr='0'
				} else{
					self.searchCriteria.ProductIdStr=self.searchCriteria.ProductId
				}
				
				var DATA = '{"ProductName":"'+self.searchCriteria.searchData+'","ProductId":"'+self.searchCriteria.ProductIdStr+'","SkuCode":"'+self.searchCriteria.SkuCode+'","PageSize":10,"IndexPage":'+index+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/SKU/GetSKUList',
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
			        	  self.total = parseInt(dataAll.Count)
			        	  dataAll.data.map((item) =>{
			        	  	if (item.IsShow==1) {//上架
			        	  		item.IsEnableStr = '上架'
			        	  	} else{//下架
			        	  		item.IsEnableStr = '下架'
			        	  	}
			        	  })
			        	  self.SingleProductList= dataAll.data
			        	   console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			}
		},
		mounted:function(){
			this.requestTableData(1)
		}
	}
</script>

<style scoped>
.SingleList {
	margin: 0 auto;
	padding: 0
}
.SingleList .SingleProductListTop{
	border-bottom: 1px solid gainsboro;
}
.SingleList .SingleProductListTop .TopUl{
	height: 2.9rem;
	/*margin-left: 2rem;*/
}
.SingleList .SingleProductListTop .TopUl .TopLi {
	display: block;
	float: left;
	margin-left: 1rem;
	height: 2rem;
	line-height: 2rem;
}
</style>