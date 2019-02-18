<template>
	<div class="TrainingCommunication">
		<div class="TrainingCommunicationTop">
			<span style="margin-left:3rem ;">上传时间</span>
			<DatePicker v-model="upLoadTime" format="yyyy/MM/dd" type="daterange" placement="bottom-end" placeholder="选择日期" style="width: 200px;margin-left:1rem;" class='time'></DatePicker>
			
			<Input v-model="searchName" placeholder="课程名称/讲师姓名" style="width: 200px;margin: 0 1rem;border-left:1px solid gray ;padding-left: 1rem;"></Input>
			<Button type="primary" @click='searchData' >搜索</Button>
			<Button type="primary" style="margin-left: 1rem;" @click='addTrainingCommunication'>新增</Button>
		</div>
		<Table style="margin-top: 1rem;" :columns="Communication_columns"  :data="Communication_tableList" ></Table>
		<Page  :total="Count" show-elevator class='page' :page-size='10' @on-change='changgePage' style='float: right;margin-right: 3rem;padding: 1rem 0;' show-total></Page>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				Count:0,//数据总数
				upLoadTime:'',//筛选时间
				searchName:'',//搜索框文字
				Communication_tableList:[//数据列表
				],
				Communication_columns:[
//						{	
//                      title:'NO.',
//                      width:70,
//                      align: 'center',
//                      type:'index'
//                   },
					{
                        title: '排序',
                        align: 'center',
                        key: 'Priority'
                    },
                     {
                        title: '课程名称',
                        align: 'center',
                        key: 'CurriculumName',
                        
                    },
                    {
                        title: '上传时间',
                        key: 'CreateTime',
                        width:200,
                        align: 'center',
                    },
                    {
                        title: '课程讲师',
                        align: 'center',
                        key: 'lecturer'
                    },
                    {
                        title: '类型',
                        align: 'center',
                        key: 'CurriculumType'
                    },
                    {
                        title: '状态',
                        align: 'center',
                        key: 'IsDisplay'
                    },
                    
                     {
                        title: '操作',
                        align: 'center',
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
			redact(name){//编辑页
				console.log(name)
				//this.$router.push({path: '/TrainingCommunicationEdit',query: {Tutorid: name.row.TutorId,ChefActivityId:name.row.ChefActivityId}});//跳转到活动交流编辑
				this.$router.push({path: '/TrainingCommunicationEdit',query:{CuInterId:name.row.CuInterId}});//跳转到活动交流编辑

			},
			addTrainingCommunication(){//活动新增
				this.$router.push({path: '/TrainingCommunicationAdd'});//跳转到活动交流编辑
				
			},
			searchData(){
				this.requestData(1)
			},
			requestData(index){//获取列表信息
				var self =this
				self.Communication_tableList =[]
				var starTime = ''
				var endTime = ''
				if (self.upLoadTime[0] && self.upLoadTime[1]) {
					var one = new Date(self.upLoadTime[0]); 
					var two = new Date(self.upLoadTime[1]);
					 starTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 endTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
				} else{
					starTime= ''
					endTime = ''
				}
				var DATA ='{"StarTime":"'+starTime+'","EndTime":"'+endTime+'","Name":"'+self.searchName+'","PageIndex":"'+index+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Lecturer/GetCultivateList',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						var dataAll = JSON.parse(json);
							if (dataAll.data.length>0) {
								self.Count = parseInt(dataAll.Count)
								for (var i = 0;i<dataAll.data.length;i++) {
									if (dataAll.data[i].IsDisplay=='1') {//状态
										dataAll.data[i].IsDisplay = '显示'
									}else{
										dataAll.data[i].IsDisplay = '隐藏'
									}
									if (dataAll.data[i].CurriculumType=='1') {//类型
										dataAll.data[i].CurriculumType = '图片'
									}else{
										dataAll.data[i].CurriculumType = '视频'
									}
								}
								
								
								self.Communication_tableList = dataAll.data
							} else{
								self.$Message.warning('暂无数据');
							}
						console.log(dataAll)
					},
					error:function(error){
						console.log(error)
						self.$Message.error('获取数据失败');
					}
				});
			},
			changgePage(index){
				this.requestData(index)
			}
			
		},
		mounted:function(){
			this.requestData(1)
		}
	}
</script>

<style scoped>
</style>