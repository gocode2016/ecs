<template>
	<!--反馈菜列表-->
	<div class="feedBackFoodList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="feedBackFoodList_Top">
			<ul style="margin-left: 0rem;">
				<li class="search_li">
					<span class="lable">状态:</span>
					<Select v-model="search.IsApply" style="width:100px">
				        <Option v-for="item in stateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<!--<li class="search_li">
					<span class="lable">产品分类:</span>
					<Select v-model="search.productType" style="width:100px">
				        <Option v-for="item in productTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>-->
				<li class="search_li">
					<span class="lable">上传时间:</span>
					<DatePicker v-model='search.dataTime' type="daterange" show-week-numbers placement="bottom-end" placeholder="选择上传时间" style="width: 180px"></DatePicker>
				</li>
				<li class="search_li">
					<Input v-model="search.Name" placeholder="姓名/菜品/产品名" style="width: 180px"></Input>
				</li>
				<li class="search_li">
					<Button type="primary" icon="ios-search" @click="getFeedBackFoodList">筛选</Button>
				</li>
			</ul>
		</div>
		<div class="feedBackFoodList_Body">
			<Table :columns="columns" :data="FBFData"></Table>
			<Page :total="total" :current='FBFIndex' show-total  :page-size='10' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				search:{
					IsApply:'-1',//状态
//					productType:'',//产品分类
					dataTime:[],//上传时间
					Name:'',//输入框
				},
				total:0,//总条数
				FBFIndex:1,//当前页
				FBFData:[
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
						title:'No.',
                        type: 'index',
                        align: 'center',
                    },
                    {
						title:'上传者',
						key:'MemberName',
                        align: 'center',
                    },
                    {
						title:'菜品名称',
						key:'DishName',
                        align: 'center',
                    },
                    {
						title:'用料',
						key:'FRName',
                        align: 'center',
                    },
//                  {
//						title:'产品名称',
//						key:'productname',
//                      align: 'center',
//                  },
//                  {
//						title:'产品分类',
//						key:'productType',
//                      align: 'center',
//                  },
//                  {
//						title:'二级分类',
//						key:'twoType',
//                      align: 'center',
//                  },
                    {
						title:'上传时间',
						key:'CreateTime',
                        align: 'center',
                        width:140,
                    },
                     {
						title:'手机号',
						key:'PhoneNum',
                        align: 'center',
                    },
                    {
						title:'状态',
						key:'IsApplyStr',
                        align: 'center',
                    },
                    {
                        title: '操作',
                        align: 'left',
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
                                 h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small',
                                    },
                                    'class': {
									    ISHiden: params.row.ISHiden,
									  },
                                    style: {
									    marginLeft: '1rem'
									  },
                                    on: {
                                        click: () => {
                                            this.auditBtn(params)
                                        }
                                    }
                                }, '审核'),
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
                        label: '待审核'
                    },
                    {
                        value: '1',
                        label: '审核通过'
                    },
                    {
                        value: '2',
                        label: '审核未通过'
                    },
				],
				productTypeList:[//产品分类数据源
					{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '1',
                        label: '基础调味'
                    },
                    {
                        value: '2',
                        label: '复合调味'
                    },
                    {
                        value: '3',
                        label: '休闲食品'
                    },
				],
			}
		},
		methods:{
			editorBtn(name){//编辑MySelfDishId
				this.$router.push({path: '/feedBackEditor',query: {MySelfDishId: name.row.MySelfDishId}});//反馈菜编辑
			},
			auditBtn(name){//审核
				//feedBackAudit
				this.$router.push({path: '/feedBackAudit',query: {MySelfDishId: name.row.MySelfDishId}});//反馈菜审核
				console.log(name)
			},
			changePage(index){//分页切换
				this.FBFIndex = index
				this.getFeedBackFoodList()
			},
			getFeedBackFoodList(){//获取table列表
				var self =this
				$('.loding').show()
				var search = this.search
				self.FBFData = []
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
				var DATA = '{"IsApply":'+search.IsApply+',"BeginTime":"'+StartTime+'","EndTime":"'+EndTime+'","Name":"'+search.Name+'","PageIndex":'+self.FBFIndex+'}'
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/MySelfDishKu/GetMySelfDishKu',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  	var dataAll =JSON.parse(json)
			        	  	self.total = parseInt(dataAll.totalCount)
			        	 	//self.FBFData = dataAll.Info
			        	  	dataAll.Info.map((item) =>{
				        	   	if (item.IsApply==0) {//待审核
				        	   		item.IsApplyStr = '待审核'
				        	   	}else if (item.IsApply==1) {
				        	   		item.IsApplyStr = '审核通过'
//				        	   		item.ISHiden = true
				        	   	}else if (item.IsApply==2) {
				        	   		item.IsApplyStr = '审核未通过'
				        	   	}
				        	   	self.FBFData.push(item)
			        	   	})
			        	  //IsApply
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
			this.getFeedBackFoodList()
		}
	}
</script>
<style>
.feedBackFoodList .ISHiden{
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
.feedBackFoodList{
	overflow: auto;
}
.feedBackFoodList .feedBackFoodList_Top{
	overflow: auto;
	padding-bottom: 0.8rem;
	/*border-bottom: 1px solid gainsboro;*/
}
.feedBackFoodList .feedBackFoodList_Top .search_li{
	float: left;
	margin-left: 1rem;
}
.feedBackFoodList .feedBackFoodList_Top .search_li .lable{
	margin-right:0.8rem ;
}

</style>