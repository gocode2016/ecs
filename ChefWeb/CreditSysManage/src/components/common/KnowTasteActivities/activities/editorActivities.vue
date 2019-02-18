<template>
	<div class="addActivities">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h1 style="text-align:center"> 活动编辑</h1>
		<div class="addActivitiesBody">
			<div style="margin: 0 auto;text-align: center;">
				<span style="">活动名称:</span>
				<Input v-model="activeName" placeholder="请输入活动名称" style="width: 200px;margin-left: 1rem;margin-right: 1rem;"></Input>
				<Button type="primary" @click='showModal'>新增区域</Button>
			</div>
			<div style="margin:1rem auto;margin-top: 1rem;width: 43rem;">
				<span style="margin-left: 23%;">是否启动:</span>
				<Select style="width:200px;margin-left: 1rem;" v-model='selectShow'>
			        <Option v-for="item in showList" :value="item.value" :key="item.value">{{ item.label }}</Option>
			    </Select>
			</div>
			<Modal
		        v-model="isShowCard"
		        :title="modalTitle"
		        @on-ok="confirm"
		        width="400">
		        <ul class="modalUl">
		        		<li>
		        			<span>赛区省:</span>
		        			<Input v-model='adddivision.ProvinceName'  placeholder="请输入赛区省份" style="width: 200px;margin-left: 1rem;"></Input>
		        		</li>
		        		<li>
		        			<span>赛区市:</span>
		        			<Input  v-model='adddivision.AreaName' placeholder="请输入赛区市" style="width: 200px;margin-left: 1rem;"></Input>
		        		</li>
		        		<!--<li>
		        			<span>赛区市:</span>
		        			<Input  placeholder="请输入赛区市" style="width: 200px;margin-left: 1rem;"></Input>
		        		</li>-->
		        		<li>
		        			<span>开始时间:</span>
		        			<Date-picker class='star' type="datetime" placeholder="选择日期" format="yyyy-MM-dd HH:mm:ss" style="width: 200px" v-model='adddivision.BeginTime' ></Date-picker>
		        		</li>
		        		<li>
		        			<span>结束时间:</span>
		        			<Date-picker class='end' type="datetime" placeholder="选择日期" format="yyyy-MM-dd HH:mm:ss" style="width: 200px" v-model='adddivision.EndTime'></Date-picker>
		        		</li>
		        </ul>
		    </Modal>
			<Table style="margin:  0 auto;margin-top: 1rem;" :columns="active_columns" :width="500" :data="active_tableList"></Table>
			<Button type="primary" style="margin-left: 88%;margin-top: 1rem;" @click='save'>保存</Button>
		</div>
		
	</div>
</template>

<script>
	export default{
		data(){
			return{
				sessionStorage:sessionStorage.getItem('loginkey'),//账号登录人
				ChefActivityId:'',//活动id
				activeName:'',//活动名称
				selectShow:'',//是否显示
				originalActiveName:'',//原始名称(用于判断是否更改)
				originalSelectShow:'',//原始是否显示(用于判断是否更改)
				isShowCard:false,//弹框
				changeIndex:0,//记录编辑的第几个数据
				isAddClick:true,//区分  弹框  是编辑 还是  新增
				modalTitle:'赛区新增',//弹框title
				starInputTime:'',//通过class获取时间
				endInputTime:'',//
				adddivision:{//新增赛区的内容
						ProvinceName:'',
						AreaName:'',
						BeginTime:'',
						EndTime:'',
						CreatePerson:'',//
						UpdatePerson:''
				},
				active_tableList:[//赛区数据
//					{
//						"ProvinceName":"山东",
//						"AreaName":"烟台赛区",
//						"ChefActivityId":2,
//						"BeginTime":"2018-01-01",
//						"EndTime":"2018-05-02",
//						"CreatePerson":"张三",
//						"UpdatePerson":"张三"
//					},
//					{
//						ProvinceName:'山东省',
//						AreaName:'烟台市',
//						BeginTime:'2017-08-12 12:40:30',
//						EndTime:'2017-08-12 12:40:30',
//					},
//					{
//						ProvinceName:'河北',
//						AreaName:'石家庄',
//						BeginTime:'2017-08-12 12:40:30',
//						EndTime:'2017-08-12 12:40:30',
//					},
//					{
//						ProvinceName:'北京',
//						AreaName:'朝阳区',
//						BeginTime:'2017-08-12 12:40:30',
//						EndTime:'2017-08-12 12:40:30',
//					}
				],
				showList:[//是否显示
					{
	                    value: '1',
	                    label: '是'
               		 },{
	                    value: '0',
	                    label: '否'
               		 }
				],
				active_columns:[
					{	
                        title:'活动省',
                        align: 'center',
                        key:'ProvinceName'
                     },
                     {	
                        title:'活动市',
                        align: 'center',
                        key:'AreaName'
                     },
                     {	
                        title:'开始时间',
                        
                        align: 'center',
                        key:'BeginTime'
                     },
                      {	
                        title:'结束时间',
                        
                        align: 'center',
                        key:'EndTime'
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
				]
			}
		},
		methods:{
			redact(name){//编辑
				this.modalTitle ='赛区编辑'
				this.isShowCard =true;
				this.isAddClick = false;
				var name =name.row;
				this.changeIndex = name._index
				this.adddivision={
					ProvinceName:name.ProvinceName,
					AreaName:name.AreaName,
					BeginTime:name.BeginTime,
					EndTime:name.EndTime,
					CreatePerson:name.CreatePerson,
					UpdatePerson:name.UpdatePerson
				};
				
				console.log(this.adddivision)
			},
			showModal(){//新增赛区弹框
				this.adddivision={
					ProvinceName:'',
					AreaName:'',
					BeginTime:'',
					EndTime:'',
					CreatePerson:'',
					UpdatePerson:'',
					ChefActivityId:''
				};
				this.isShowCard =true;
				this.isAddClick = true;
				this.modalTitle ='赛区新增'
			},
			confirm(){//新增  确定按钮
				var self =this;

				if (this.isAddClick) {//新增
					self.addModel();
					
				} else{//编辑
					self.editModel();
				}
			},
			addModel(){//新增模式
				var self =this;
				if (self.adddivision.ProvinceName && self.adddivision.AreaName && self.adddivision.BeginTime && self.adddivision.EndTime) {
					 self.adddivision.BeginTime=$('.star input').val()
					self.adddivision.EndTime=$('.end input').val()
					self.adddivision.CreatePerson=self.sessionStorage
					self.adddivision.UpdatePerson=self.sessionStorage
					self.adddivision.ChefActivityId =	parseInt(self.ChefActivityId)
					self.active_tableList.push(self.adddivision)
					console.log('新增模式')
					console.log(self.active_tableList)	
				} else{
					self.$Message.warning('请填写完整信息');
				}
				self.adddivision={
					ProvinceName:'',
					AreaName:'',
					BeginTime:'',
					EndTime:'',
					CreatePerson:'',
					UpdatePerson:'',
					ChefActivityId:''
				};
				$('.star input').val('')
				$('.end input').val('')
			},
			editModel(){//编辑模式
				var self = this;
				if (self.adddivision.ProvinceName && self.adddivision.AreaName && self.adddivision.BeginTime && self.adddivision.EndTime) {
					var p =self.adddivision; 
					
					if (self.adddivision.BeginTime.toString().indexOf('-')==-1) {
						self.adddivision.BeginTime=$('.star input').val()
						self.active_tableList[self.changeIndex].BeginTime =self.adddivision.BeginTime
					}else{
						self.active_tableList[self.changeIndex].BeginTime = p.BeginTime;
					}
					if (self.adddivision.EndTime.toString().indexOf('-')==-1) {
						self.adddivision.EndTime=$('.end input').val()
						self.active_tableList[self.changeIndex].EndTime=self.adddivision.EndTime
					}else{
						self.active_tableList[self.changeIndex].EndTime = p.EndTime;
					}
					self.active_tableList[self.changeIndex].AreaName = p.AreaName;
					self.active_tableList[self.changeIndex].ProvinceName = p.ProvinceName;
					self.active_tableList[self.changeIndex].CreatePerson=self.sessionStorage
//					self.active_tableList[self.changeIndex].UpdatePerson=self.sessionStorage
						console.log(self.active_tableList)
						console.log('编辑模式')
				}else{
					self.$Message.warning('请填写完整信息');
				}
				self.adddivision={
					ProvinceName:'',
					AreaName:'',
					BeginTime:'',
					EndTime:'',
					CreatePerson:'',
					UpdatePerson:'',
					ChefActivityId:''
				};
				$('.star input').val('')
				$('.end input').val('')
			},
			save(){//保存按钮
				var msg;
				var self = this;
				var isOK1='OK';
				var isOK2='';
				//名字和是否显示 修改
				if (self.activeName !=self.originalActiveName || self.selectShow !=self.originalSelectShow ) {
					var DATA1  = '{"ChefActivityId":"'+self.ChefActivityId+'","ChefActivityName":"'+self.activeName+'","IsEnable":'+self.selectShow+',"UpdatePerson":"123"}'
					console.log(DATA1)
					$.ajax({
				        type :"post",
				        url :URLHeader.ECActivities+'/api/ChefActivity/UpdateIsEnable',
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:DATA1,
				        success : function(json) {
				        	 isOK1=json
				        	  console.log(json)
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		   });
				}
				//区域修改或新增
//				var DATA2 =[{"ProvinceName":"山东","AreaName":"青岛赛区","ChefActivityId":2,"BeginTime":"2018-01-01","EndTime":"2018-05-02","CreatePerson":"张三","UpdatePerson":"张三"}];
				$.ajax({
				        type :"post",
				        url :URLHeader.ECActivities+'/api/MatchRegion/AddMatchRegion',
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:JSON.stringify(self.active_tableList),
				        success : function(json) {
				        	  isOK2=json
				        	if (isOK1=='OK' && isOK2=='OK') {
				        		
//				        		setTimeout(msg,3000);

				        	  	self.$Message.success('数据保存成功');
				        	  	self.loadData()
				        	} else{
				        		console.log(isOK1+"&&"+isOK2)
				        	  	self.$Message.warning('数据保存出错');
				        	}

				        	  
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		   });
		   		   
		   						
				
			},
			loadData(){//初始数据
				var self =this;
				this.ChefActivityId= window.location.hash.split('?')[1].split('=')[1];
				$.ajax({//获取活动名称
			        type :"get",
			        url :URLHeader.ECActivities+'/api/ChefActivity/GetCAMRByCA?caId='+this.ChefActivityId,
			        dataType : 'json',
	//		        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 self.activeName = dataAll.ChefActivity[0].ChefActivityName;
			        	 self.selectShow =dataAll.ChefActivity[0].IsEnable.toString();
//			        	 //记录初入界面的名称和是否显示的值(用于区分是否修改)
					 self.originalActiveName =  self.activeName
			        	 self.originalSelectShow =  self.selectShow
			        	 //表格数据
			        	 self.active_tableList = dataAll.MatchRegion
			        	 $('.loding').hide()
			        	 console.log(dataAll)
			        },
			        error : function(error) { 
			        }
	   		    });
			},
		},
		mounted:function(){
			//URLHeader.ECActivities+/api/ChefActivity/UpdateIsEnable    //名字和显示
			//URLHeader.ECActivities+/api/MatchRegion/AddMatchRegion
			this.loadData()
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
.addActivities .addActivitiesBody{
	padding: 1rem;
	margin: 0 auto;
	margin-top: 2rem;
	border: 1px dashed gray;
	border-radius:8px ;
	width: 70%;
}
.modalUl li{
	margin-bottom: 1rem;
	text-align: center;
}
</style>