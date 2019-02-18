<style>

</style>
<template>
	<div class="palyers">
		<div class="palyers_main">
			<div class="search_div">
				<span>状态选择:</span>
				<Select v-model="state" style="width:100px;margin-left: 1rem;margin-right: 1rem;" placeholder='状态选择'>
			        <Option v-for="item in state_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
			    </Select>
			    <span>大区选择:</span>
			    <Select v-model="regional" style="width:180px;margin-left: 1rem;" placeholder='大区选择'>
			        <Option v-for="item in regional_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
			    </Select>
				<span style="font: 1rem;margin: 0 0.8rem;">注册时间:</span>
				<Date-picker type="daterange" placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 200px" ></Date-picker>
				<Input  placeholder="姓名/工号/手机号" style="width: 200px" v-model='information'></Input>
				<Button type="ghost"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>搜索</Button>
				<Button type="primary"  @click='batchReview'>批量审批</Button>
			</div>
			<Spin fix class='loding'>
                <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
                <div>Loading</div>
            </Spin>
			<Table style="margin-top: 1rem;" :columns="table_columns" :data="players_tableList" @on-selection-change="selectPlayer"></Table>
			<Page  :total="Count" show-elevator class='page' :page-size='20' @on-change='changgePage' style='margin-right: 3rem;margin-bottom: 1rem;' show-total></Page>
		</div>
	</div>
</template>

<script>
	export default {
         data () {
         	return{
         		pagIndex:1,//当前页数
         		Count:0,//总条数
         		state:'-1',//状态选择
         		regional:'0',//大区选择
         		starData:'',//开始时间
         		endData:'',//结束时间
         		information:'',//搜索框内容
         		URL:{
         			searchUrl:URLHeader.user+'/api/SaleMan/GetSaleManPageList'
         		},
         		ReviewLiset:[],
         		state_List:[
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
                        label: '已审核'
                    }

         		],
         		regional_List:[
         			{
                        value: '0',
                        label: '全部大区'
                    },
         			{
                        value: '1',
                        label: '餐饮营销本部华北营业部'
                    },
                    {
                        value: '2',
                        label: '餐饮营销本部东北营业处'
                    },
                    {
                        value: '3',
                        label: '餐饮营销本部华中营业处'
                    },
                    {
                        value: '4',
                        label: '餐饮营销本部华东营业部'
                    },
                    {
                        value: '5',
                        label: '餐饮营销本部西北营业处'
                    },
                    {
                        value: '6',
                        label: '餐饮营销本部西南营业处'
                    },
                    {
                        value: '7',
                        label: '餐饮营销本部鲁东营业处'
                    },
                    {
                        value: '8',
                        label: '餐饮营销本部鲁中营业处'
                    },
                    {
                        value: '9',
                        label: '餐饮营销本部鲁西南营业处'
                    },
                    {
                        value: '10',
                        label: '餐饮营销本部鲁西北营业处'
                    }
         		],
         		table_columns:[
         			{
         				title:'选择',
                type: 'selection',
                align: 'center',
                width:60,
              },
         			{
                title:'ID',
                align: 'center',
                width:90,
                key:'SalemanId'

              },
         			{
                title: '所在大区',
                width:180,
                key: 'RegionName',
                align: 'center',
              },
              {
                title: '岗位',
                width:180,
                key: 'Position',
                align: 'center',
              },
//                  {
//                      title: '省',
//                      key: 'ProvinceName',
//                      width:80,
//                      align: 'center',
//                  },
//                  {
//                      title: '市',
//                      align: 'center',
//                      width:80,
//                      key: 'CityName',
//                  },
//                  {
//                      title: '区',
//                      align: 'center',
//                      key: 'AreaName',
//                  },
                    {
                        title: '姓名',
                        align: 'center',
                        key: 'Name',
//                       render: (h, params) => {
//                          return h('span',{
//                          	style:{
//                          		cursor:'pointer',
//                          		display: 'block',
//							    width: '100%',
//							    padding: '12px 0px',
//							    color:'red',
//                          	},
//             				 on: {
//									click: () => {
//                                          this.playersDetails(params)
//                                      }
//                             }
//
//                          },params.row.Name)
//                        },
                    },
                    {
                        title: '联系方式',
                        align: 'center',
                        key: 'Telephone',
                        width:120
                    },
                    {
                        title: '工号',
                        align: 'center',
                        key: 'WorkNum',
                        width:90
                    },
                     {
                        title: '注册时间',
                        align: 'center',
                        key: 'RegistDate',
                        width:110
                    },
                     {
                        title: '审批状态',
                        align: 'center',
                        key: 'IsEnable'
                    },
//                  {
//                  		 title: '账号状态',
//                      align: 'center',
//                      key: 'RegistStateStr'
//                  },
                     {
                        title: '操作',
                        align: 'center',
                        key: 'operation',
                         width:80,
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                    on: {
                                        click: () => {
//                                          this.redact(params)
											this.playersDetails(params)
                                        }
                                    }
                                }, '编辑')
                            ]);
                        }
                    },
         		],
         		players_tableList:[//队员列表数据

         		],
         	}
         },
         methods: {
         	search_click(){//搜索按钮事件
         		//console.log('开始时间='+this.starData+'结束时间='+this.endData);
         		this.searchPlayers();
         	},
         	redact(name){//编辑页面跳转
         		//console.log(name)
         		this.$router.push({path: '/playersEditor', query: {SalemanId: name.row.SalemanId,RegistState:name.row.RegistState}});//跳转到详情页
         	},
         	playersDetails(name){//点击姓名跳转
         		this.$router.push({path: '/playersDetails', query: {SalemanId: name.row.SalemanId}});//跳转到详情页
         	},
         	searchPlayers(){//数据请求
         		var self = this;
         		if ($('.dataTime input').val()) {
         			self.starData=$('.dataTime input').val().substring(0,10);//开始时间
         			self.endData =$('.dataTime input').val().substring(12);//结束时间
         		}
         		//console.log("状态="+this.state+'大区='+this.regional+'开始时间='+this.starData+'结束时间='+this.endData+'搜索框='+this.information)
         		self.players_tableList=[];
         		let DATA ='{"IndexPage":"'+self.pagIndex+'","PageSize":"20","RegionId":"'+self.regional+'","RegistState":"'+self.state+'","SearchInfo":"'+self.information+'","RegistBeginTime":"'+self.starData+'","RegistEndTime":"'+self.endData+'"}';
       		//console.log(DATA)
         		$.ajax({
			        type :"post",
			        url :self.URL.searchUrl,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 self.Count = parseInt(dataAll.Count);
//			        	 console.log(self.TotalPage)
			        	  console.log(dataAll)
			        	 self.dataIntegration(dataAll.data);
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });

         	},
         	dataIntegration(data){//数据整合
         		var self = this;
         		for (var i=0;i<data.length;i++) {
         			var dataLi = data[i];
         			if (dataLi.RegistState==0) {//审批状态判断 _disabled: true
         				dataLi.IsEnable='未审批'
         				dataLi._disabled =false;
         			} else{
         				dataLi.IsEnable='已审批'
         				dataLi._disabled =true;
         			}
         			//RegistStateStr
//       			if (dataLi.RegistState==0) {//正常
//       				dataLi.RegistStateStr='正常'
//       			} else{
//       				dataLi.RegistStateStr='禁用'
//       			}

         			self.players_tableList.push(dataLi);
         			$('.palyers .ivu-table table').css({
		          		'width':'100%'
		          	})
         			$('.loding').hide()
         		}
         	},
         	batchReview(){//审批按钮  ReviewLiset数据源
         		var self =this;
         		$.ajax({
			        type :"post",
			        url :URLHeader.user+'/api/SaleMan/ReviewSaleman',
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:JSON.stringify(self.ReviewLiset),
			        success : function(json) {
			        	 //var dataAll = JSON.parse(json);
			        	 if (json=='true') {
			        	 	self.$Message.success('审核成功');
			        	 } else{
			        	 	self.$Message.error('审核异常');
			        	 }
			        	  console.log(json)
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
         	},
         	selectPlayer(name){//多选方法
         		var self =this;
         		self.ReviewLiset=[];
         		for (var i = 0;i<name.length;i++) {
         			self.ReviewLiset.push(name[i].SalemanId)
         		}
         	},
         	changgePage(index){//分页切换
         		this.pagIndex = index;
         		this.searchPlayers();
         		console.log('第'+index+'页')
         	}
         },
          mounted: function() {
          	var self =this;
          	self.searchPlayers();
          	$('.palyers .ivu-table-wrapper .ivu-table .ivu-table-header table thead tr th .ivu-checkbox').css({//隐藏头部多选
          		'display':'none'
          	})
         }
    }
</script>

<style scoped>
.palyers .search_div{
		text-align: left;
		border-bottom: 1px solid gainsboro;
		padding-bottom: 0.7rem;
	}
.page{
	margin-top: 1rem;
	float: right;
}
.palyers .ivu-table-header table{
	width: 100%;
}
.palyers .ivu-table-body table{
	width: 100%;
}
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
    @keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }
</style>
