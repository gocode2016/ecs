<template>
	<!--营销活动列表-->
	<div class="marketingList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="marketingList_Top" style="overflow: auto;">
			<ul style="margin-left: 2rem;">
				<!--<li class="top_Li">
					<span class="top_lable">营销类型:</span>
					<Select v-model="search.marketingType" style="width:100px">
				        <Option v-for="item in marketingTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>-->
				<li class="top_Li">
					<span class="top_lable">活动时间:</span>
					<DatePicker type="date" placeholder="活动时间" v-model="search.ActDate" style='width: 130px;'></DatePicker>
				</li>
				<li class="top_Li">
					<span class="top_lable">状态:</span>
					<Select v-model="search.ActState" style="width:100px">
				        <Option v-for="item in stateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="top_Li">
					<span class="top_lable">活动名称:</span>
					<Input v-model="search.ActName" placeholder="请输入活动名称..." style="width: 170px"></Input>
				</li>
				<li class="top_Li">
					<Button type="primary" icon="ios-search" @click="getActivityList">筛选</Button>
				</li>
				<li class="top_Li">
					<Button type="primary" @click="activitiesOfNew">新增营销</Button>
				</li>
			</ul>
		</div>
		<div class="marketingList_Body">
			<Table  :columns="columns" :data="marketingList_Data"></Table>
			<Page  :total="tableTotal" :current="marketingList_indexPage" show-elevator  :page-size='30' @on-change='changgePage' style='margin-right: 3rem;float: right;margin-top: 1rem;padding-bottom: 1rem;' show-total></Page>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				tableTotal:0,//总数量
				marketingList_indexPage:1,//当前页
				search:{//查询条件
					//marketingType:'',//营销类型
					ActDate:'',//活动时间
					ActState:'0',//状态
					ActName:'',//活动名称
				},
				marketingList_Data:[
					
				],
				marketingTypeList:[//营销类型列表
					{
                        value: '',
                        label: '全部'
                    },
                    {
                        value: '0',
                        label: '抽奖'
                    },
                    {
                        value: '1',
                        label: '签到'
                    },
				],
				stateList:[
					{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '1',
                        label: '启用'
                    },
                    {
                        value: '-1',
                        label: '停用'
                    },
				],
				columns:[
					{
						title:'No',
						type:'index',
						width:120,
						align: 'center',
					},
					{
						title:'活动名称',
						key:'ActName',
						width:140,
						align: 'center',
						render:(h,params) =>{
							return h('div',[
								h('span',{
								  style:{
										cursor:'pointer',
										color:'red'
									},
                                   on:{
                                   	click: () =>{
                                   		this.signInSee(params.row)
                                   	}
                                   }
								},params.row.ActName)
							])
						}
					},
					{
						title:'营销类型',
						key:'ActivityType',
						align: 'center',
					},
					{
						title:'状态',
						key:'ActStateStr',
						align: 'center',
					},
					{
						title:'有效期',
						key:'EndAndStart',
						align: 'center',
						width:155
					},
					{
						title:'创建时间',
						key:'RegistDate',
						align: 'center',
						width:170
					},
					{
						title:'操作',
						align: 'center',
						render:(h,params) =>{
							return h('div',[
								h('Button',{
									props: {
                                        type: 'primary',
                                        size: 'small'
                                   },
                                   on:{
                                   	click: () =>{
                                   		this.editor(params.row)
                                   	}
                                   }
								},'编辑')
							])
						}
					},
				],
			}
		},
		methods:{
			editor(name){//编辑
				console.log(name)
				this.$router.push({path: '/marketingAdd',query: {ActivityId:name.ActivityId}});//营销活动编辑
				
			},
			activitiesOfNew(){//活动新增
				this.$router.push({path: '/marketingAdd'});//营销活动新增
			},
			signInSee(name){//签到查看
				//this.$router.push({path: '/signInToView'});//签到出啊看
				this.$router.push({path: '/signInToView',query: {ActivityId:name.ActivityId}});//签到查看
			},
			getActivityList(){
				var self= this
				var url = URLHeader.marketing+'/api/SignActivity/GetActivityList'
				//console.log(URLHeader)
				//console.log(url)
				//ActState状态 
				//ActName活动名字
				//ActDate活动时间
				self.marketingList_Data = []
				var ActDate = ''
				if (self.search.ActDate) {
					var time = new Date(self.search.ActDate);  
					ActDate = time.getFullYear() + '-' + (time.getMonth() + 1) + '-' + time.getDate();
				} else{
					ActDate = ''
				}
				
				var search = this.search
				var DATA = '{"ActState":'+search.ActState+',"ActName":"'+search.ActName+'","ActDate":"'+ActDate+'","PageSize":30,"IndexPage":'+self.marketingList_indexPage+'}'
				console.log(DATA)
				$('.loding').show()
				$.ajax({
					type:"post",
					url:url,
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						var dataAll = JSON.parse(json);
						self.tableTotal = parseInt(dataAll.Count)
						//self.marketingList_Data = dataAll.data
						dataAll.data.map((item) =>{
//							item.ActStateStr
							if (item.ActState ==1) {//状态
								item.ActStateStr = '启用'
							} else{//-1
								item.ActStateStr = '停用'
							}
							item.EndAndStart = item.StartTime+item.EndTime
							self.marketingList_Data.push(item)
						})
						console.log(dataAll)
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('数据获取失败');
					}
				});
			},
			changgePage(index){
				this.marketingList_indexPage = index
				this.getActivityList()
				console.log(index)
			}
		},
		mounted:function(){
			this.getActivityList()
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
.marketingList .marketingList_Top .top_Li{
	float: left;
	margin-left: 1rem;
}
.marketingList .marketingList_Top .top_Li .top_lable{
	/*margin-left: 2rem;*/
	margin-right: 0.7rem;
	font-size: 14px;
	/*font-weight: bold;*/
}
.marketingList .marketingList_Body{
	margin-top: 1rem;
	margin-left: 1rem;
	
}
</style>