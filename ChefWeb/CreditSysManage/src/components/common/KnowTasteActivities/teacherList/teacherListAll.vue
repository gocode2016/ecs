<template>
	<div class="teacherListAll">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
        <div style="overflow: auto;">
			<ul class="teacherList_ul">
				<li class="teacherList_li" style="margin-left: 1rem;margin-right: 3rem;line-height: 32px;">
					<span style="font-size: 1rem;margin-left: 1rem;height: 1rem;font-weight: bold;">新增导师</span>
				</li>
				<li class="teacherList_li">
					<span>参赛城市:</span>
					<Select v-model="search.city" style="width:100px;margin-left: 1rem;" placeholder = '参赛城市'>
				         <Option v-for="item in cityList" :value="item.AreaName" :key="item.AreaName">{{ item.AreaName }}</Option>
				    </Select>
				</li>
				<li class="teacherList_li">
					<span>星厨姓名:</span>
					<Input v-model="search.name" placeholder="导师姓名" style="width: 160px;margin-left: 1rem;"></Input>
				</li>
				<li class="teacherList_li">
					<Button type="primary" icon="ios-search" size="large" @click='requestTeacherList' style="margin-left: 0rem;">搜索</Button>
				</li>
				<li class="teacherList_li">
					<Button type="primary" size="large" @click='addCooker' style="margin-left: 1rem;">新增导师</Button>
				</li>
			</ul>
			
		</div>
		<!--<div>
			<span style="font-size: 1rem;margin-left: 2rem;height: 1rem;">导师列表</span>
			<Button type="primary" size="large" @click='addCooker' style="margin-left: 80%;">新增导师</Button>
		</div>-->
		<Table style="margin-top: 1rem;margin-left: 1rem;" :columns="teacher_columns"  :data="teacher_tableList" ></Table>
		<Page  :total="Count" show-elevator class='page' :page-size='20' @on-change='changgePage' style='margin-bottom: 1rem;'></Page>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				chefactivityid:'',//
				pageIndex:1,//当前页数
				Count:1,//数据总条数
				cityList:[//参赛城市
				],
				search:{//搜索条件
					city:'全部',//参赛城市
					name:''//星厨姓名
				},
				url_teacher:URLHeader.ECActivities+'/api/Tutor/GetAllTutor?',//列表数据
				teacher_tableList:[//厨师列表数据
//					{
//						TutorId:'1',
//						TutorName:'小明',
//						AreaName:'沧州赛区 济南赛区',
//						TutorComment:'王者',
//						AreaName:'蛋炒饭',
//						HotelAddress:'咖喱鸡块'
//					}	
				],
				teacher_columns:[//列表头
						{	
                        title:'导师ID',
                        width:90,
                        align: 'center',
                        key:'TutorId'
                     },
                     {
                        title: '姓名',
                        align: 'center',
                        key: 'TutorName',
                         render: (h, params) => {
                            return h('span',{
                            	style:{
                            		cursor:'pointer',
                            		display: 'block',
							    padding: '12px 0px',
							    color:'red',
                            	},
               				 on: {
								click: () => {
                                        this.teacherName(params)
                                    }
                               }
                               
                            },params.row.TutorName)
                          },
                    },
                    {
                        title: '指导赛区',
                        key: 'AreaName',
                        width:200,
                        align: 'center',
                    },
                    {
                        title: '头衔',
                        align: 'center',
                        key: 'TutorComment'
                    },
                    {
                        title: '菜品1',
                        align: 'center',
                        key: 'DishOne'
                    },
                    {
                        title: '菜品2',
                        align: 'center',
                        key: 'DishTow'
                    },
                     {
                        title: '操作',
                        align: 'center',
                        key: 'operation',
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                    on: {
                                        click: () => {
                                            this.redact(params)
                                        }
                                    }
                                }, '编辑')
                            ]);
                        }
                    }
				],
				
			}
		},
		methods:{
			redact(name){//编辑事件
				//console.log(name.row)
				this.$router.push({path: '/teacherEdit',query: {Tutorid: name.row.TutorId,ChefActivityId:name.row.ChefActivityId}});//跳转到厨师编辑
			},
			addCooker(){//新增
				this.$router.push({path: '/addTeacher',query: {ChefActivityId: this.chefactivityid}});//跳转到厨师新增
			},
			teacherName(name){//导师姓名点击
				this.$router.push({path: '/teacherDetails',query: {Tutorid: name.row.TutorId,ChefActivityId:name.row.ChefActivityId}});//跳转到导师详情
			},
			requestTeacherList(){
				
				var self=this
				$('.loding').show()
				self.Starkitchen_tableList = []
				var DATA ='{"TutorName":"'+self.search.name+'","AreaName":"'+self.search.city+'","PageIndex":"'+self.pageIndex+'","PageSize":"20","ChefActivityId":"'+self.chefactivityid+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Tutor/GetAllTutor',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
			        	 var dataAll = JSON.parse(json);
			        	 self.Count =parseInt(dataAll.Count);
			        	 self.teacher_tableList = dataAll.data;
			        	 $('.loding').hide()
			        	 	console.log(dataAll)						
					},
					error:function(error){
						console.log(error)
					}
				});
				
				
				
				
				
				
				
				
//				$('.loding').show()
//				var self =this;
//				if (self.teacher_tableList.length) {
//					self.teacher_tableList = [];
//				}
//				let URL =self.url_teacher+'chefactivityid='+self.chefactivityid+'&PageIndex='+self.pageIndex+'&PageSize=20';
////				console.log(URL)
//				$.ajax({
//			        type :"get",
//			        url :URL,
//			        dataType : 'json',
//			        success : function(json) {
//			        	 var dataAll = JSON.parse(json);
//			        	 self.Count =parseInt(dataAll.Count);
//			        	 self.teacher_tableList = dataAll.data;
//			        	 $('.loding').hide()
//			        	 	console.log(dataAll)
//			        },
//			        error : function(error) { 
//						
//			        }
//	   		    });
			},
			changgePage(index){//页数切换
				this.pageIndex =index;
				this.requestTeacherList()
			},
			getCompetingCity(){//获取参赛城市
				var self =this
				$.ajax({//获取活动名称
			        type :"get",
			        url :URLHeader.ECActivities+'/api/DishTutor/GetcaNameMRFRBycaId?caId='+self.chefactivityid,
			        dataType : 'json',
			        success : function(json) {
			        		 var dataAll = JSON.parse(json);
			        	 	console.log(dataAll)
			        	 	var item={
			        	 		AreaName:'全部'
			        	 	}
			        	 	dataAll.MR.splice(0, 0, item);  
			        	 	self.cityList = dataAll.MR 
			        	 	//console.log(self.cityList)
			        },
			        error : function(error) { 
			        }
	   		    });
			},
		},
		mounted:function(){
			var self =this;
			
			get_URL();
			function get_URL(){//获取地址参数
    				var hash= window.location.hash.split('?')[1].split('=')[1];
    				self.chefactivityid=hash;
    			}
			this.getCompetingCity()
			self.requestTeacherList();
		}
	}
</script>

<style scoped>
.page{
	margin-top: 1rem;
	float: right;
}
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
@keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }	
.teacherListAll{
		/*min-width: 70rem;*/
	}
.teacherListAll	.page{
	margin-top: 1rem;
	float: right;
}
.teacherListAll  .teacherList_li{
	display: block;
	float: left;
	margin-left: 2rem;
}
</style>
