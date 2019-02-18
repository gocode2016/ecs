<template>
	<!--产品列表-->
	<div class="productList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="productList_Top">
			<ul style="margin-left: 0rem;">
				<!--<li class="search_li">
					<span class="lable">状态:</span>
					<Select v-model="search.IsApply" style="width:100px">
				        <Option v-for="item in stateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>-->
				<li class="search_li">
					<span class="lable">产品分类:</span>
					<Select v-model="search.productType" style="width:100px">
				        <Option v-for="item in productTypeList" :value="item.ProductFirstId" :key="item.ProductFirstId">{{ item.ProductFirstName }}</Option>
				    </Select>
				</li>
				<!--<li class="search_li">
					<span class="lable">上传时间:</span>
					<DatePicker v-model='search.dataTime' type="daterange" show-week-numbers placement="bottom-end" placeholder="选择上传时间" style="width: 180px"></DatePicker>
				</li>-->
				<li class="search_li">
					<span class="lable">商品名称</span>
					<Input v-model="search.Name" placeholder="商品名称" style="width: 150px"></Input>
				</li>
				<li class="search_li">
					<Button type="primary" icon="ios-search" @click="getproductList">筛选</Button>
					<!--<Button type="error" @click="deleteBtn">删除产品</Button>-->
					<Button type="primary" @click="addProductBtn">新增产品</Button>
				</li>
			</ul>
		</div>
		<div class="productList_Body">
			<Table :columns="columns" :data="productData"></Table>
			<Page :total="total" :current='productIndex' show-total  :page-size='10' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>

</template>

<script>
	export default{
		data(){
			return{
				search:{
					//IsApply:'-1',//状态
					productType:0,//产品分类
					dataTime:[],//上传时间
					Name:'',//输入框
				},
				total:0,//总条数
				productIndex:1,//当前页
				productData:[
//					{
//						name:'张三',
//						dishname:'西红柿鸡蛋',//
//						productname:'味达美',
//						productType:'基础调味',
//						twoType:'酱油',
//						uptime:'2017-6-15 10:47:09',//
//						state:'审核通过',//
//						ISHiden:false,//	true 隐藏审核					
//					}
				],//列表数据
				columns:[
					{
                        type: 'selection',
                        align: 'center',
                        width:70,
                    },
					{
						title:'No.',
                        type: 'index',
                        align: 'center',
                         width:80,
                    },
                    {
						title:'产品名称',
						key:'ProductName',
                        align: 'center',
                    },
                    {
						title:'包含规格',
						key:'Specification',
                        align: 'center',
                    },
                    {
						title:'分类',
						key:'ProductFirstName',
                        align: 'center',
                    },
                    {
						title:'二级分类',
						key:'ProductSecondName',
                        align: 'center',
                    },
                    {
						title:'最后修改时间',
						key:'UpdateTime',
						width:140,
                        align: 'center',
                    },
                    {
                        title: '操作',
                        align: 'center',
                        key: 'operation',
                        width:200,
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small',
                                        
                                    },
                                    on: {
                                        click: () => {
                                            this.editorBtn(params)
                                        }
                                    }
                                }, '编辑'),
                            ]);
                        }
                     }
				],
				stateList:[//状态数据源
					{
                        value: '-1',
                        label: '全部'
                    },
                    {
                        value: '0',
                        label: '显示'
                    },
                    {
                        value: '1',
                        label: '隐藏'
                    },
				],
				productTypeList:[//产品分类数据源
//					{
//                      value: '0',
//                      label: '全部'
//                  },
//                  {
//                      value: '1',
//                      label: '基础调味'
//                  },
//                  {
//                      value: '2',
//                      label: '复合调味'
//                  },
//                  {
//                      value: '3',
//                      label: '休闲食品'
//                  },
				],
			}
		},
		methods:{
			editorBtn(name){//编辑
				this.$router.push({path: '/edit&Add',query: {ProductId: name.row.ProductId}});//产品编辑
			},
			deleteBtn(){//删除按钮
				
			},
			addProductBtn(){//商品新新增
				this.$router.push({path: '/edit&Add',query: {ProductId: 0}});//产品新增
			},
			changePage(index){//分页切换
				this.productIndex = index
				this.getproductList()
			},
			getSelectData(){//下拉框数据
				var self =this
				$.ajax({//一级分类
			        type :"get",
			        url :URLHeader.ECActivities+'/api/Product/GetProductFirstAll',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			       	 	var dataAll =JSON.parse(json)
			       	 	var one = {
					     		ProductFirstName:'全部',
					     		ProductFirstId:0
					     	}
					     	//物料类别
					     	self.productTypeList= dataAll
							self.productTypeList.splice(0, 0, one);
			        		
			        		//console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });	
			},
			getproductList(){//获取table列表
				var self =this
				$('.loding').show()
				var search = this.search
				self.productData = []
				var StartTime=''//开始时间
				var EndTime = ''//结束时间
				if (search.dataTime[0]!=null) {
					var one = new Date(search.dataTime[0]); 
					var two = new Date(search.dataTime[1]);
					 StartTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 EndTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
					// console.log(search.dataTime)
				}else{
					StartTime= ''
					EndTime = ''
				}
				//var DATA = '{"IsApply":'+search.IsApply+',"BeginTime":"'+StartTime+'","EndTime":"'+EndTime+'","Name":"'+search.Name+'","PageIndex":'+self.productIndex+'}'
				var DATA = '{"ProductFirstId":"'+search.productType+'","ProductName":"'+search.Name+'","PageIndex":'+self.productIndex+'}'
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetPKIList',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  	var dataAll =JSON.parse(json)
			        	  	self.total = parseInt(dataAll.totalCount)
			        	  	self.productData = dataAll.Info
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
			this.getSelectData()//下拉框数据
			this.getproductList()
		}
	}
</script>
<style>
.productList .ISHiden{
	display:none
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
.productList{
	overflow: auto;
}
.productList .productList_Top{
	overflow: auto;
	padding-bottom: 0.8rem;
	/*border-bottom: 1px solid gainsboro;*/
}
.productList .productList_Top .search_li{
	float: left;
	margin-left: 1rem;
}
.productList .productList_Top .search_li .lable{
	margin-right:0.8rem ;
}

</style>