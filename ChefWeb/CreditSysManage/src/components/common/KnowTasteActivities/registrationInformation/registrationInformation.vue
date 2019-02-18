<template>
	<div class="contentList">
		<!--<span style="font-size: 1rem;font-weight: bold;padding: 1rem;margin-bottom: 2rem;">内容列表</span>-->
		<div class="search_div"style="margin-top: 1rem;">
			<Select v-model="state" style="width:100px" placeholder='状态选择'>
		        <Option v-for="item in state_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
		    </Select>
		    <Select v-model="regional" style="width:180px" placeholder='大区选择'>
		        <Option v-for="item in regional_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
		    </Select>
			<span style="font: 1rem;margin: 0 0.8rem;">注册时间:</span>
			<Date-picker type="daterange" placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 200px" ></Date-picker>
			<Input  placeholder="姓名/菜品名" style="width: 200px" v-model='searchInformation'></Input>
			<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>搜索</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='auditAddBtn'>厨师新增</Button>
		</div>
		<Table style="margin-top: 1rem;" :columns="content_columns" :data="content_tableList"></Table>
		<Page  :total="Count" show-elevator class='page' :page-size='10' @on-change='changgePage' style='float: right;margin-top: 1rem;margin-right: 5rem;'></Page>
		<Modal
        title="对话框标题"
        v-model="modal"
        :mask-closable="false"
        @on-ok='modalConfirm'>
		    <RadioGroup v-model="selectDish" style='margin: 0.5rem auto;text-align: center;' >
		        <Radio v-for='item in dishAll' :label='item.value' :key='item.value'>{{item.label}}</Radio>
   			</RadioGroup>
    </Modal>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				Count:0,//总条数
				selectDish:'',//投票菜选择
				chefactivityid:'',//赛区id
				pageIndex:1,//当前页
				starData:'',//开始时间
				endData:'',//结束时间
				dishAll:[
//					{
//						value:'1',
//						label:'apple'
//					},
//					{
//						value:'2',
//						label:'android'
//					},
				],
				chefId:'',//设置投票菜记录厨师id
				modal:false,//设置投票菜弹层
				searchInformation:'',//搜索框内容
				state:'-1',//状态选择
         		regional:'0',//大区选择
				starData:'',//开始时间
         		endData:'',//结束时间
				state_List:[
         			{
                        value: '-1',
                        label: '全部'
                    },
                    {
                        value: '0',
                        label: '等待审核'
                    },
                    {
                        value: '1',
                        label: '审核通过'
                    },
                    {
                        value: '2',
                        label: '审核未通过'
                    }
         		
         		],
         		regional_List:[
         			{
                        value: '0',
                        label: '全部赛区'
                     },
         		],
				content_columns:[
         			{	
                        title:'NO.',
                        align: 'center',
                        type:'index'
                     },
                    {
                        title: '姓名',
                        align: 'center',
                        key: 'MemberName',
                         render: (h, params) => {
                            return h('span',{
                            	style:{
                            		cursor:'pointer',
                            		display: 'block',
							    width: '100%',
							    padding: '12px 0px',
							    color:'red',
                            	},
               				 on: {
									click: () => {
                                          this.CheckDetails(params)
                                      }
                               }
                               
                            },params.row.MemberName)
                          },
                           width:80
                    },
                    {
                        title: '本地菜',
                        align: 'center',
                        key: 'DishTow',
                       
                    },
                    {
                        title: '创新菜',
                        align: 'center',
                        key: 'DishOne',
                        
                    },
                     {
                        title: '赛区',
                        align: 'center',
                        key: 'AreaName',
                        width:100
                    },
                     {
                        title: '酒店',
                        align: 'center',
                        key: 'HotelName',
                        
                    },
                     {
                        title: '报名时间',
                        align: 'center',
                        key: 'CreateTime',
                        width:120
                    },
                     {
                        title: '状态',
                        align: 'center',
                        key: 'IsApply',
                        width:80,
                        render: (h, params) => {
                            return h('span',{
                            	style:{
                          		//cursor:'pointer',
                            		display: 'block',
							    width: '100%',
							    padding: '12px 0px',
							   
                            	},
               				 on: {
								click: () => {
//	                                 this.audit(params.row)
	                             }
                               }
                               
                            },params.row.IsApply)
                          },
                    },
                     {
                        title: '投票菜',
                        align: 'center',
                        key: 'DishNameSelected',
                        width:100
                    },
                     {
                        title: '票数',
                        align: 'center',
                        key: 'SelectedCount',
                        width:100
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
                                        type: 'error',
                                        size: 'small',
                                        
                                    },
                                    on: {
                                        click: () => {
                                            this.redact(params)
                                        }
                                    }
                                }, '编辑'),
                                h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small',
                                        
                                    },
                                    attrs: {
									    id:params.row.hidenSeit
									  },
                                    style: {
									    marginLeft: '1rem'
									  },
                                    on: {
                                        click: () => {
                                            this.setVote(params)
                                        }
                                    }
                                }, '设置投票菜'),
                                 h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small',
                                        
                                    },
                                    attrs: {
									    id:params.row.hidenAudit
									  },
                                    style: {
									    marginLeft: '1rem'
									  },
                                    on: {
                                        click: () => {
                                            this.setAudit(params)
                                        }
                                    }
                                }, '审核'),
                            ]);
                        }
                    },
         		],
         		content_tableList:[//队员列表数据	
         		],
			}
		},
		methods:{
			auditAddBtn(){//厨师新增页面
				this.$router.push({path: '/auditAdd',query: {chefactivityid: this.chefactivityid}});//跳转到编辑信息页
			},
			changgePage(index){//切换页码
				this.pageIndex =index
				this.getRegistrationList()
			},
			search_click(){//搜索按钮
				var self =this
				
				if ($('.dataTime input').val()) {//获取搜索时间
         			self.starData=$('.dataTime input').val().substring(0,10);//开始时间
         			self.endData =$('.dataTime input').val().substring(12);//结束时间
         		}
				self.getRegistrationList()
			},
			CheckDetails(name){//报名详情  CheckDetails
				console.log(name)
				this.$router.push({path: '/CheckDetails',query: {ChefId: name.row.ChefId}});//跳转到编辑信息页
			},
			redact(name){//编辑   enrolEdit
				this.$router.push({path: '/enrolEdit',query: {ChefId:  name.row.ChefId}});//跳转到编辑信息页
				console.log('编辑'+name)
			},
			setVote(name){//设置投票  弹出
				this.modal=true
				//console.log(name.row)
				this.dishAll=[]
				this.dishAll = [{value:name.row.DishChefOneId,label:name.row.DishOne},{value:name.row.DishChefTwoId,label:name.row.DishTow}];
				this.chefId = name.row.ChefId
				console.log(this.dishAll)
			},
			modalConfirm(){//设置投票弹框   确认按钮 
				//URLHeader.ECActivities+/api/DishChef/SetSelectedDihsChef
				var self = this
				var DATA = '{"DishChefId":"'+this.selectDish+'","ChefId":"'+this.chefId+'"}'
				console.log(this.selectDish)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/DishChef/SetSelectedDihsChef',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						console.log(json)
						self.pageIndex = 1
						self.getRegistrationList()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('菜品数据保存异常,请刷新页面重新保存');
					}
				});	
				//console.log(this.selectDish)
			},
			setAudit(name){//跳转审核页
				this.$router.push({path: '/auditView',query: {ChefId:  name.row.ChefId}});//跳转到审核信息页
				console.log('audit'+name)
			},
			getRegistrationList(){//获取报名列表
				var self =this;
				var DATA = '{"ChefActivityId":"'+self.chefactivityid+'","pageIndex":"'+self.pageIndex+'","IsApply":"'+self.state+'","MatchRegionId":"'+self.regional+'","EndTime":"'+self.endData+'","BeginTime":"'+self.starData+'","Name":"'+self.searchInformation+'"}'
				//console.log(DATA)
				self.content_tableList = []
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/DishChef/GetDishChefList',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						var dataAll = JSON.parse(json);
						self.Count = parseInt(dataAll.Count)
						console.log(dataAll)						
						for (var i =0;i<dataAll.data.length;i++) {
							var One = dataAll.data[i]
							
							if (One.IsApply ==0) {//待审核   
								One.hidenSeit = 'hidenSeit'
								One.hidenAudit = ''
								One.IsApply = '待审核'
								One.cellClassName = {
									IsApply:'red'
								}
							}
							if (One.IsApply ==1) {//审核通过
								One.hidenSeit = ''
								One.hidenAudit = 'hidenAudit'
								One.IsApply = '审核通过'
								if (One.DishNameSelected !=null) {//审核通过且设置投票菜
									One.hidenSeit = 'hidenSeit'
									One.hidenAudit = 'hidenAudit'
								}
							}
							if (One.IsApply ==2) {//审核未通过
								One.hidenSeit = 'hidenSeit'
								One.hidenAudit = 'hidenAudit'
								One.IsApply = '审核未通过'
							}
							
							self.content_tableList.push(One)
						}
						
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
					}
				});
			},
		},
		mounted:function(){
			var self =this
			get_URL()
			function get_URL(){//获取地址参数
    				var hash= window.location.hash.split('?')[1].split('=')[1];
    				self.chefactivityid=hash;
    				$.ajax({//获取赛区
			        type :"get",
			        url :URLHeader.ECActivities+"/api/DishChef/GetAllByChefActivityId?caId="+self.chefactivityid,
			        dataType : 'json',
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 	for (var i = 0;i<dataAll.length;i++) {
			        	 			var one = {};
			        	 			one.value = dataAll[i].MatchRegionId
			        	 			one.label = dataAll[i].AreaName
			        	 			self.regional_List.push(one)	
			        	 	}
			        	 	//console.log(dataAll)
			        },
			        error : function(error) { 
			        }
	   		   });	
    			}
			self.getRegistrationList();
		}
	}
</script>

<style scoped>
	
</style>
<style>
.contentList .red{
		color: red;
		
	}
.contentList #hidenSeit{
	display: none;
}
.contentList #hidenAudit{
	display: none;
}	
</style>