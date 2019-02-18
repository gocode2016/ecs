<template>
	<div class="StarkitchenList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div style="overflow: auto;">
			<ul class="StarkitchenList_ul">
				<li class="StarkitchenList_li" style="margin-left: 1rem;margin-right: 3rem;line-height: 32px;">
					<span style="font-size: 1rem;margin-left: 1rem;height: 1rem;font-weight: bold;">星厨列表</span>
				</li>
				<li class="StarkitchenList_li">
					<span>参赛城市:</span>
					<Select v-model="search.city" style="width:100px;margin-left: 1rem;" placeholder = '参赛城市'>
				         <Option v-for="item in cityList" :value="item.AreaName" :key="item.AreaName">{{ item.AreaName }}</Option>
				    </Select>
				</li>
				<li class="StarkitchenList_li">
					<span>星厨姓名:</span>
					<Input v-model="search.name" placeholder="星厨姓名" style="width: 200px;margin-left: 1rem;"></Input>
				</li>
				<li class="StarkitchenList_li">
					<Button type="primary" icon="ios-search" size="large" @click='searchStar' style="margin-left: 0rem;">搜索</Button>
				</li>
				<li class="StarkitchenList_li">
					<Button type="primary" size="large" @click='addStarkitchen' style="margin-left:0;">新增星厨</Button>
				</li>
			</ul>
			
		</div>
		<Modal v-model="modalDeletShow" width="360">
	        <p slot="header" style="color:#f60;text-align:center">
	            <Icon type="information-circled"></Icon>
	            <span>删除确认</span>
	        </p>
	        <div style="text-align:center">
	            <p>您确定要删除此星厨吗?</p>
	            <p></p>
	        </div>
	        <div slot="footer">
	            <Button type="error" size="large" long  @click="deletstarConfirm">删除</Button>
	        </div>
	    </Modal>
		<Table style="margin-top: 1rem;margin-left: 1rem;" :columns="Starkitchen_columns"  :data="Starkitchen_tableList" ></Table>
		<Page  :total="Count" show-elevator :page-size='10' @on-change='changgePage' style='display: block;float: right;margin-right:2rem;margin-top: 1rem;margin-bottom: 1rem;'></Page>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				modalDeletShow:false,//删除弹框
				deletStarID:'',//要删除的id
				Count:0,//总条数
				pageIndex:1,//当前页数
				ChefActivityId:'',//活动id
				search:{//搜索条件
					city:'全部',//参赛城市
					name:''//星厨姓名
				},
				cityList:[//参赛城市
				],
				Starkitchen_columns:[//星厨表头
//					{	
//                      title:'NO.',
//                      width:70,
//                      align: 'center',
////                      key:'ChefStarId'//星厨编号
//						type:'index'
//                   },
                     {
                        title: '姓名',
                        align: 'center',
                        key: 'ChefStarName',
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
                                        this.ChefStarName(params)
                                    }
                               }
                               
                            },params.row.ChefStarName)
                          },
                    },
                    {
                        title: '酒店',
                        key: 'HotelName',
                        width:120,
                        align: 'center',
                    },
                    {
                        title: '岗位',
                        align: 'center',
                        key: 'ChefStarPosition'
                    },
                     {
                        title: '城市',
                        align: 'center',
                        key: 'CityName'
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
                        title: '活动页显示',
                        align: 'center',
                        key: 'IsDisplay'
                    },
                     {
                        title: '排序',
                        align: 'center',
                        key: 'PriorityId'
                    },
                     {
                        title: '操作',
                        align: 'center',
                        width:130,
                        key: 'operation',
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
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
                                        size: 'small'
                                    },
                                    	style:{
		                            		marginLeft: '0.7rem',
		                            	},
                                    on: {
                                        click: () => {
                                            this.deleteStar(params)
                                        }
                                    }
                                }, '删除')
                            ]);
                        }
                    }
				],
				Starkitchen_tableList:[//星厨数据源
					{
						ChefStarId:'1',
						ChefStarName:'小明',
						AreaName:'沧州赛区 济南赛区',
						starKitchenComment:'王者',
						DishOne:'蛋炒饭',
						DishTow:'咖喱鸡块'
					}
				],
			}
		},
		methods:{
			addStarkitchen(name){//星厨新增
				this.$router.push({path: '/addStarKitchen',query: {ChefActivityId: this.chefactivityid}});//跳转到星厨新增
			},
			redact(name){//编辑
				this.$router.push({path: '/StarKitchenEdit',query: {ChefStarId: name.row.ChefStarId,ChefActivityId:name.row.ChefActivityId}});//跳转到星厨详情
			},
			deleteStar(name){//删除星厨
				this.modalDeletShow = true
				this.deletStarID = name.row.ChefStarId		
			},
			deletstarConfirm(){//弹框  确认删除
				var self =this
				console.log(this.deletStarID)
				this.modalDeletShow = false
				//ChefStar/DeleteSingleChefStar?chefStarId=1

				$.ajax({
					type:"get",
					url:URLHeader.ECActivities+'/api/ChefStar/DeleteSingleChefStar?chefStarId='+self.deletStarID,
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					success:function(json){
						if (json=='OK') {
							self.$Message.success({
								content:'星厨删除成功',
								duration: 4
							});
						} else{
							self.$Message.error({
								content:'星厨删除失败',
								duration: 4
							});
						}
						self.requestData()
						self.deletStarID=''
			        	 	console.log(json)
					},
					error:function(error){
						console.log(error)
					}
				});
				
				
				
			},
			ChefStarName(name){//星厨名字(详情)
				this.$router.push({path: '/StarKitchenDetail',query: {ChefStarId: name.row.ChefStarId,ChefActivityId:name.row.ChefActivityId}});//跳转到星厨详情
			},
			changgePage(index){//分页切换
				console.log(index)
				this.pageIndex = index
				this.requestData()
			},
			searchStar(){//数据搜索
				this.pageIndex = 1
				this.requestData()
			},
			requestData(){//数据请求
				var self=this
				$('.loding').show()
				self.Starkitchen_tableList = []
				var DATA ='{"Name":"'+self.search.name+'","City":"'+self.search.city+'","pageIndex":"'+self.pageIndex+'","ChefActivityId":"'+self.chefactivityid+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/ChefStar/GetChefStarList',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						 var dataAll = JSON.parse(json);
			        		 self.Count =parseInt(dataAll.Count);
						for (var i = 0;i<dataAll.data.length;i++) {
							if (dataAll.data[i].IsDisplay=='1') {
								dataAll.data[i].IsDisplay='显示'
							} else{
								dataAll.data[i].IsDisplay='隐藏'
							}
						}
			        	 	self.Starkitchen_tableList = dataAll.data;
			        		 $('.loding').hide()
			        	 	console.log(dataAll)
					},
					error:function(error){
						console.log(error)
					}
				});
				
				
//				$.ajax({
//			        type :"get",
//			        url :URLHeader.ECActivities+'/api/ChefStar/GetChefStarList?caId='+self.chefactivityid+'&pageIndex='+self.pageIndex,
//			        dataType : 'json',
//			        success : function(json) {
//			        	 var dataAll = JSON.parse(json);
//			        	 self.Count =parseInt(dataAll.Count);
//			        	 console.log(dataAll)
//					for (var i = 0;i<dataAll.data.length;i++) {
//						if (dataAll.data[i].IsDisplay=='1') {
//							dataAll.data[i].IsDisplay='显示'
//						} else{
//							dataAll.data[i].IsDisplay='隐藏'
//						}
//					}
//			        	 self.Starkitchen_tableList = dataAll.data;
//			        	 $('.loding').hide()
//			        	 	console.log(dataAll)
//			        },
//			        error : function(error) { 
//						
//			        }
//	   		    });
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
			this.getCompetingCity()
			this.requestData()
			function get_URL(){//获取地址参数
        				var hash= window.location.hash.split('?')[1].split('=')[1];
        				self.chefactivityid=hash;
        		}
			
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
.StarkitchenList  .StarkitchenList_li{
	display: block;
	float: left;
	margin-left: 2rem;
}
</style>