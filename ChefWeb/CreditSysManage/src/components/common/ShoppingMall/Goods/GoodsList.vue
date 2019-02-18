<template>
	<div class="GoodsList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="GoodsListTop">
			<ul class="TopUl">
				<li class="TopLi">
					<!--<span>商品类型</span>-->
					<span>商品类型:</span>
					<Select v-model="searchCriteria.GoodsType" style="width:100px;position: relative;margin-left: 1rem;" placeholder = '商品类型'>
				        <Option v-for="item in GoodsTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>商品分类:</span>
					<Select v-model="searchCriteria.GoodscLassification" style="width:150px;margin-left: 1rem;" placeholder = '商品类型'>
				        <Option v-for="item in GoodscLassificationList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<p style="font-size:1.5rem ;">|</p>
				</li>
				<li class="TopLi">
					<Input v-model="searchCriteria.searchData" placeholder="商品名称" style="width: 200px"></Input>
				</li>
				<li class="TopLi">
					<Button type="ghost" icon="ios-search" @click ='searchGoods'>搜索</Button>
				</li>
				<!--<li class="TopLi">
					<Button type="primary" @click='deleteGoods'>删除商品</Button>
				</li>-->
				<li class="TopLi" >
					<!--<Button type="primary" @click = 'addGoods'>新增商品</Button>-->
					<Dropdown trigger="click" >
				       <Button type="primary">
				            新增商品
				            <Icon type="arrow-down-b"></Icon>
				        </Button>
				        <DropdownMenu slot="list" >
				            <DropdownItem name='1' @click.native='addGoods(1)'>实体商品</DropdownItem>
				            <DropdownItem name='2' @click.native='addGoods(2)'>虚拟商品</DropdownItem>
				        </DropdownMenu>
				    </Dropdown>
				</li>
			</ul>
		</div>
		<div class="body">
			<Modal v-model="modalDelet" width="360">
		        <p slot="header" style="color:#f60;text-align:center">
		            <Icon type="information-circled"></Icon>
		            <span>删除商品</span>
		        </p>
		        <div style="text-align:center">
		            <p>您确认要删除此商品吗?</p>
		        </div>
		        <div slot="footer">
		            <Button type="error" size="large" long  @click="deletConfirm">删除</Button>
		        </div>
		    </Modal>
			<Table :columns="GoodsColumns" :data="GoodsList"></Table>
			<Modal
			    title="规格配置"
			    v-model="ModalShow"
			    @on-ok="ok"
			    :mask-closable="false">
			    		<div>
			    			<div style="margin-left: 1rem;">
		   					<span>规格名称</span>
		   					<Input v-model="TypeName"  placeholder="请输入" style="width: 150px;margin-left: 1rem;margin-right: 1rem;"></Input>
		   					<Button type="primary" @click="addTypeValue">添加值</Button>
		   				</div>
			    			<ul class="typeUl" >
				   			<li class="typeLi"  style="border: 1px dashed darkgray;margin: 1rem; padding: 1rem 0;border-radius:5px;padding-top: 0;">
				   				<span  v-for='item in modalList'>
				   					<Input v-model="item.TypeValue"  placeholder="请输入" style="width: 80px;margin-left: 1rem;margin-top: 1rem;"></Input>
				   				</span>
				   			</li>
				   		</ul>
			    		</div>
			</Modal>
			<Page :total="total" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;padding-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				deletProductId:'',//删除的商品id
				modalDelet:false,//删除弹框
				Isedit:false,//是否编辑模式
				ModalShow:false,//对话框
				total:0,//总条数
				searchCriteria:{//搜索条件
					GoodsType:'0',//商品类型
					GoodscLassification:'0',//商品分类
					searchData:'',//搜索条件
				},
				GoodsList:[//商品列表数据源
				],
				ProductId:'',//产品id
				ProductName:'',//产品名字
				TypeName:'',//规格名称
				modalList:[
					{
						TypeValue:'',//规格值
					},
				],
				GoodscLassificationList:[//商品分类
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
				GoodsTypeList:[//商品类型
					{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '2',
                        label: '虚拟物品'
                    },
                    {
                        value: '1',
                        label: '实体物品'
                    },
				],
				GoodsColumns:[//商品;列表表头
//					{
//						title:'',
//						align: 'center',
//						type:'selection',
//					},
					{
						title:'No.',
						align: 'center',
						width:60,
						type:'index',
					},
					{
						title:'商品名称',
						align: 'center',
						key:'ProductName',
					},
					{
						title:'商品编码',
						align: 'center',
						key:'ProductId',
					},
					{
						title:'分类',
						align: 'center',
						key:'CategoryName',
					},
//					{
//						title:'总销量',
//						align: 'center',
//						key:'GoodsName',
//					},
					{
						title:'在售SKU',
						align: 'center',
						key:'SkuCount',
					},
					{
						title:'添加时间',
						align: 'center',
						width:100,
						key:'AddDate',
					},
					{
						title:'操作',
						width:280,
						align: 'center',
						render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    style: {
                                     marginLeft: '10px'
//                                      float:'left'
                                    },
                                    on: {
                                        click: () => {
                                            this.GoodsEdit(params)
                                        }
                                    }
                                }, '编辑'),
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    }, 
                                    style: {
                                        marginLeft: '10px'
//                                      float:'left'
                                    },
                                    on: {
                                        click: () => {
                                            this.GoodsSpecifications(params)
                                        }
                                    }
                                }, '规格配置'),
                                 h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    }, 
                                    style: {
                                        marginLeft: '10px'
//                                      float:'left'
                                    },
                                    on: {
                                        click: () => {
                                            this.GoodsDelet(params)
                                        }
                                    }
                                }, '删除')
                            ]);
                        }
					},
				],
			}
		},
		methods:{
			addTypeValue(){//value 规格值新增
				//console.log(index)
				var list = {
					TypeValue:'',//规格值
				}
				this.modalList.push(list)
			},
			ok(){//确定按钮
				var self =this
				var url =''
				if (self.modalList[0].TypeValue=='') {
					console.log('无数据')
				}else{
					for (var i=0;i<self.modalList.length;i++) {
						var listOne = self.modalList[i]
						var list = {
							ProductId:parseInt(self.ProductId),//商品id
							ProductName:self.ProductName,//商品名字
							TypeName:self.TypeName,//规格名称
							TypeValue:listOne.TypeValue,//值
							IsEnable:listOne.IsEnable,//0为上架 1位下架
						}
						if (self.Isedit) {//编辑模式开始
							if (listOne.NormsId==null) {//新增
								console.log('新增')
								var DATA = '{"ProductId":'+list.ProductId+',"ProductName":"'+list.ProductName+'","TypeName":"'+list.TypeName+'","TypeValue":"'+list.TypeValue+'","IsEnable":1}'
								url =URLHeader.ShoppingMall+'/api/Norms/AddNorms'
							} else{//编辑
								var DATA = '{"NormsId":'+listOne.NormsId+',"ProductId":'+list.ProductId+',"ProductName":"'+list.ProductName+'","TypeName":"'+list.TypeName+'","TypeValue":"'+list.TypeValue+'","IsEnable":'+list.IsEnable+'}'
								url=URLHeader.ShoppingMall+'/api/Norms/UpdateNorms'
							}
							
						} else{//新增模式开始
							var DATA = '{"ProductId":'+list.ProductId+',"ProductName":"'+list.ProductName+'","TypeName":"'+list.TypeName+'","TypeValue":"'+list.TypeValue+'","IsEnable":1}'
							url =URLHeader.ShoppingMall+'/api/Norms/AddNorms'
						}
						console.log(DATA)
						$.ajax({
					        type :"post",
					        url :url,
					        dataType : 'json',
					        contentType: "application/json; charset=utf-8",				
					        data:DATA,
					        success : function(json) {
					        	console.log(json)
					        },
					        error : function(error) { 
					        		console.log(error)
					        }
			   		    });
   					}
				}

					console.log(self.modalList)
			},
			GoodsEdit(name){//商品编辑
				//console.log(name.row.ProductType)
				if (name.row.ProductType==1) {//实体
					this.$router.push({path: '/commonGoodsEdit',query: {productId:name.row.ProductId}});//实体商品编辑页
				} else{
					this.$router.push({path: '/VirtualGoodsEdit',query: {productId:name.row.ProductId}});//虚拟商品编辑页
				}	
			},
			GoodsSpecifications(name){//商品规格
				this.ModalShow =true
				this.ProductId = name.row.ProductId
				this.ProductName = name.row.ProductName
				var self =this
				self.modalList= []//数据清空
				self.TypeName=''//input清空
				$.ajax({
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Norms/GetSysProductNorms?productId='+this.ProductId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		var dataAll = JSON.parse(json)
			        		if (dataAll.length>0) {
			        			self.TypeName=dataAll[0].TypeName
			        			self.modalList = dataAll
			        			self.Isedit = true//编辑模式
			        		}else{
			        			var item={
									TypeValue:'',//规格值
								}
							self.modalList.push(item)	
							self.Isedit =false//新增模式
			        		}
			        		console.log(dataAll)
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
			addGoods(name){//新增商品按钮
				if (name==1) {//实体商品
					this.$router.push({path: '/commonGoodsAdd'});//跳转实体商品新增
				} else{//虚拟商品
					this.$router.push({path: '/VirtualGoodsAdd'});//跳转虚拟商品新增				
				}
				//
			},
			GoodsDelet(name){//删除商品
				//deletProductId
				console.log(name.row.ProductId)
				this.deletProductId = name.row.ProductId
				this.modalDelet=true
			},
			deletConfirm(name){
				var self= this
				this.modalDelet = false
				console.log(this.deletProductId)
				$.ajax({
			        type :"get",
			        url :URLHeader.ShoppingMall+'/api/Product/RemoveProduct?productId='+this.deletProductId,///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	   console.log(json)
			        	   self.requestTableData(1);
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			changePage(index){//页面切换
				console.log('当前页码'+index)
				this.requestTableData(index)
			},
			requestTableData(index){
				//:8004/api/Product/GetProductList
				var self = this
				self.GoodsList = []
				var DATA = '{"productType":"'+self.searchCriteria.GoodsType+'","categoryId":"'+self.searchCriteria.GoodscLassification +'","productName":"'+self.searchCriteria.searchData+'","PageSize":"20","IndexPage":'+index+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Product/GetProductList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
			        	  self.total = parseInt(dataAll.Count)
			        	  self.GoodsList= dataAll.data
			        	  $('.loding').hide()
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
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
@keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }
.GoodsList {
	margin: 0 auto;
	padding: 0
}
.GoodsList .GoodsListTop{
	border-bottom: 1px solid gainsboro;
}
.GoodsList .GoodsListTop .TopUl{
	height: 2.9rem;
	margin-left: 2rem;
}
.GoodsList .GoodsListTop .TopUl .TopLi {
	display: block;
	float: left;
	margin-left: 1rem;
	height: 2rem;
	line-height: 2rem;
}
.GoodsList .body .typeUl .typeLi{
	border: 1px solid gainsboro;
}
</style>