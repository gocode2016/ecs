<template>
	<div class="playersDetails">
		<Menu mode="horizontal" active-name="1"  @on-select='selectHeader'>
            <div class="layout-assistant">
                <Menu-item name="1" >基本信息</Menu-item>
                <Menu-item name="2" class='two' v-if="isShowHeaderTwo">绑定厨师</Menu-item>
            </div>
        </Menu>
        <div class="content">
        		<div class="basicInformation">
        			<ul class="InformationUl">
        				<li>
        					<span class="title">姓名:</span>
        					<span class="value">{{infomationList.Name}}</span>
        				</li>
        				<li>
        					<span class="title">工号:</span>
        					<span class="value" >{{infomationList.WorkNum}}</span>
        				</li>
        				<li>
        					<span class="title">手机号:</span>
        					<span class="value">{{infomationList.Telephone}}</span>
        				</li>
        				<li>
        					<span class="title">所在大区:</span>
        					<span class="value" >{{infomationList.RegionName}}</span>
        				</li>
        				<li style="height: auto;line-height:normal;overflow: auto;">
        					<span class="title" style="display: block;float: left;">工作区域:</span>
        					<div style="float: left;margin-left: 1rem;">
        						<p class="value"  v-for="item in work_area">{{item.ProvinceName}}/{{item.CityName}}/{{item.AreaName}} </p>
        					</div>
        					
        				</li>
        				<li>
        					<span class="title">身份证:</span>
        					<span class="value" >{{infomationList.CardId}}</span>
        				</li>
        				<li>
        					<span class="title">注册时间:</span>
        					<span class="value" >{{infomationList.RegistDate}}</span>
        				</li>
        				<li style="text-align: right;">
        					<Button type="ghost" v-if="isApproval" @click='qualifiedClick'>合格</Button>
        					<Button type="ghost" @click='redact'>编辑</Button>
        				</li>
        			</ul>
        			<Spin fix class='loding'>
	                <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
	                <div>Loading</div>
	            </Spin>
        		</div>
        		<div class="BindingChef">
        			<div class="search_div">
        				<span>客户编码状态:</span>
					<Select v-model="Codestate" style="width:100px;margin-left: 1rem;" placeholder='状态选择'>
				        <Option v-for="item in code_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
					<span style="font: 1rem;margin: 0 0.8rem;">注册时间:</span>
					<Date-picker type="daterange" placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 200px" v-model='timeAll'></Date-picker>
					<Input icon="ios-search" placeholder="姓名/酒店/手机号" style="width: 200px" v-model='information'></Input>
					<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>搜索</Button>
					<Button type="primary"  style="margin:0  0.5rem;" @click='transferData'>批量转移</Button>
				</div>
        			<Table style="margin-top: 1rem;" :columns="table_columns"   :data="cook_tableList" @on-selection-change="selectCooks"></Table>
        			<Page  :total="Count" show-elevator class='page' :page-size='10' @on-change='changgePage'></Page>
        			<Modal  
			        v-model="ModalShow"
			        title="厨师批量转移"
			        @on-ok="Confirm_transfer">
			        <div>
			        		<Input v-model="searchPlayer" placeholder="请输入要转入队员工号" style="width: 200px"></Input>
			        		<Button type="primary" style="margin-left: 1rem;" icon="ios-search" @click='palyerSeach'>搜索</Button>
			        </div>
					<Table style="margin-top: 1rem;" :columns="serch_player"    :data="search_player_data"></Table>
					
			    </Modal>	
        		</div>
           
        </div>
	</div>
</template>

<script>
	export default {
		data (){
			return {
				pageIndex:1,
				Count:0,//分页总条数
				isApproval: true,//是否审批完成false
				isShowHeaderTwo:false,//绑定厨师是否显示
				SalemanId:'',//用户唯一id
				work_area:[],//工作区域
				infomationList:{
					Name:'',//姓名
					WorkNum:'',//工号
					Telephone:'',//手机号
					RegionName:'',//所在大区
					workingArea:'',//工作区域
					CardId:'',//身份证号
					RegistDate:'',//注册时间
				},
				URL:{
					infomation:URLHeader.user+'/api/SaleMan/GetSaleManInfo',//基本信息
					qualified:URLHeader.user+'/api/SaleMan/ReviewSaleman',//合格事件
					BindingChef:URLHeader.user+'/api/Member/GetPageMember',//绑定厨师 Table
					searchPlayer:URLHeader.user+'/api/SaleMan/GetSaleManById',//批量转移  查询队员
					TransferMember:URLHeader.user+'/api/SaleMan/TransferMember',//转移动作
				},
				searchPlayer:'',//弹框的搜索框
				ModalShow:false,//转移弹框
				cheLlist:[],//存储转移数据
				Codestate:'-1',//完善编码
				information:'',//模糊查询信息
				starData:'',//开始时间
         		endData:'',//结束时间
         		timeAll:'',//时间选择绑定
         		serch_player:[//转移弹框表头
                     {	
                        title:'姓名',
                        align: 'center',
                        key:'Name'
                     },
                     {	
                        title:'工号',
                       
                        align: 'center',
                        key:'WorkNum'
                     },
         		],
         		search_player_data:[//转移弹框数据
         			
         		],
         		code_List:[//状态列表
         			{
                        value: '-1',
                        label: '全部'
                    },
                    {
                        value: '0',
                        label: '未完善'
                    },
                    {
                        value: '1',
                        label: '已完善'
                    }
         		],
         		table_columns:[//table表头
         			{	
                        type: 'selection',
                        align: 'center',
	                     width:60,
                     },
//                   {	
//                      title:'客户编码',
//                      width:70,
//                      align: 'center',
//                      key:'MemberCode'
//                   },
         			{	
                        title:'NO.',
                        width:70,
                        align: 'center',
                        key:'MemberId'
                     },
                     {
                        title: '姓名',
                        align: 'center',
                        width:90,
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
                                            this.playersDetails(params)
                                      }
                               }
                               
                            },params.row.MemberName)
                          }
                    },
                    {
                        title: '联系方式',
                        align: 'center',
                        key: 'MemberTelePhone',
                     	width:120
                    },
//                  {
//                      title: '所在大区',
//                    	width:100,
//                      key: 'RegionName',
//                      align: 'center',
//                  },
                    {
                        title: '酒店省',
                        key: 'ProvinceName',
	                    width:75,
                        align: 'center',
                    },
                    {
                        title: '酒店市',
                        align: 'center',
                        width:75,
                        key: 'CityName'
                    },
                    {
                        title: '酒店区',
                        align: 'center',
                        width:75,
                        key: 'AreaName'
                    },
                    {
                        title: '酒店地址',
                        align: 'center',
                        width:100,
                        key: 'HotelAddress'
                    },
                    {
                        title: '酒店名称',
                        align: 'center',
						width:100,
                        key: 'HotelName'
                    },
                    {
                        title: '注册时间',
                        align: 'center',
                        key: 'RegistDate',
                        width:100,
                    },
                    {
                        title: '状态',
                        align: 'center',
                        width:80,
                        key: 'MemberState'
                    },
                    {
                        title: '积分余额',
                        align: 'center',
                        width:90,
                        key: 'LeaveIntegral'
                    }
         		],
         		cook_tableList:[],//table数据
			}
		},
		methods:{
			selectHeader(name){//选择头部滑动
				var self = this;
				if (name==1) {//基本信息
					$('.BindingChef').hide();
					$('.basicInformation').show();
				} else{
					$('.basicInformation').hide();
					$('.BindingChef').show()
					$('.BindingChef .ivu-table-header table').css({
						'width':'100%'
					});
					$('.BindingChef .ivu-table-body table').css({
						'width':'100%'
					})
					if (!self.cook_tableList.length) {
						self.$Notice.warning({
		                    title: '此队员下无绑定厨师'
		                });
					} 
					 
					
				}
			},
			redact(){//编辑按钮
				var RegistState;
				if (this.isApproval) {
					RegistState='1'
				} else{
					RegistState='0'
				}
				this.$router.push({path: '/playersEditor', query: {SalemanId: this.SalemanId,RegistState:RegistState}});//跳转到详情页
			},
			requestInformation(){
				var self =this;
				let DATA ='{"SalemanId":"'+self.SalemanId+'"}';
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :self.URL.infomation,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	// var dataAll = JSON.parse(json);
			        	console.log(JSON.parse(json))
			        	self.work_area = JSON.parse(json).Area//工作区域 
			        	var dataAll = JSON.parse(JSON.parse(json).Data);
					dataAll.workingArea =dataAll.ProvinceName+dataAll.CityName+dataAll.AreaName;
					self.infomationList=dataAll;
					if (dataAll.RegistState==0) {//0为未审批
						self.isApproval = true;
						self.isShowHeaderTwo = false;
						$('.layout-assistant').css({
							'width':'120px'
						})
					} else{
						self.requestBindingChef();
						self.isShowHeaderTwo = true;
						self.isApproval = false;
					}
					
					$('.loding').hide()
			       console.log(self.infomationList)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			qualifiedClick(){//合格按钮
				var self = this;
				var arr =[self.SalemanId];
				let DATA= JSON.stringify(arr)
				$.ajax({
			        type :"post",
			        url :self.URL.qualified,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
						self.$Message.success('审批成功');
						self.$router.push({path: '/playersDetails', query: {SalemanId:self.SalemanId,RegistState:'1'}});//跳转到详情页
						$('.layout-assistant').css({
							'width':'300px'
						})
						self.isShowHeaderTwo = true;
						self.isApproval = false;
						self.requestBindingChef();
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
				
			},
			//---------------------------------------------------------此下为绑定厨师---------------------------------------------------------------------
			requestBindingChef(){//厨师绑定 信息请求
				var self =this;
				
         		if ($('.dataTime input').val()) {
         			self.starData=$('.dataTime input').val().substring(0,10);//开始时间
         			self.endData =$('.dataTime input').val().substring(12);//结束时间
         		}else{
         			self.starData=''//开始时间
         			self.endData =''//结束时间
         		}
				let DATA ='{"IndexPage":"'+self.pageIndex+'","PageSize":"10","Identity":"0","SalemanId":"'+self.SalemanId+'","CodeState":"'+self.Codestate+'","SearchInfo":"'+self.information+'","RegistBeginTime":"'+self.starData+'","RegistEndTime":"'+self.endData+'"}';
				//console.log(DATA)
				self.cook_tableList=[];
	   		    $.ajax({
			        type :"post",
			        url :self.URL.BindingChef,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
					if (dataAll.data.length>0) {
						 self.Count = parseInt(dataAll.Count);
						self.dataIntegration(dataAll.data);
					}
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			dataIntegration(data){//数据整合
				var self =this;
				for (var i=0;i<data.length;i++) {
					if (data[i].MemberState==0) {//审批状态判断 _disabled: true
         				data[i].MemberState='未审批'
         			} else{
         				data[i].MemberState='已审批'
         			}
					self.cook_tableList.push(data[i])		
				}
				console.log(self.cook_tableList)
			},
			search_click(){//搜索按钮
				this.requestBindingChef();
				
			},
			transferData(){//批量转移
				if (this.cheLlist.length>0) {
					this.ModalShow=true;
				} else{
					this.$Message.warning('请选择要转移的厨师');
				}
				//弹框展示
				
			},
			selectCooks(name){//多选事件 cheLlist
				console.log(name)
				var self = this;
				self.cheLlist=[];
				for (var i= 0;i<name.length;i++) {
					self.cheLlist.push(name[i].MemberId)
				}
				console.log('选中数据'+self.cheLlist)
				
			},
			playersDetails(name){//姓名点击
				console.log(name)
				var state;
				if (name.row.MemberState=='已审批') {
					state="1"
				} else{
					state="0"
				}
//				this.$router.push({path: '/cookersDetails', query: {MemberId: name.row.MemberId,MemberState:state}});//跳转到详情页
				//cookerAuditAfter.vue
				this.$router.push({path: '/cookerAuditAfter', query: {MemberId: name.row.MemberId,MemberState:state}});//跳转到详情页
			},
			palyerSeach(){//转移弹框的  搜索按钮
				//searchPlayer
				var self = this;
				let DATA ='{"WorkNum":"'+self.searchPlayer+'"}';
				self.search_player_data=[]
				$.ajax({
			        type :"post",
			        url :self.URL.searchPlayer,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 for (var i = 0;i<dataAll.length;i++) {
			        	 	self.search_player_data.push(dataAll[i])
			        	 }
			        	 console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			Confirm_transfer(){//确认转移
				var self =this;
				if (self.search_player_data.length>0) {
					console.log(self.cheLlist.length)
					let DATA=JSON.stringify(self.cheLlist)
					console.log('会员SalemanId='+self.search_player_data[0].SalemanId)
					console.log('转移数据'+DATA)
					$.ajax({
				        type :"post",
				        url :self.URL.TransferMember+'?salemanId='+self.search_player_data[0].SalemanId,
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:DATA,
				        success : function(json) {
				        	 console.log(json)
				        	 self.$Message.success('数据转移成功');
							self.pageIndex='1';
							self.state='-1';
							self.information='',
							self.starData='',
							self.endData=''
							self.timeAll='';
				        	 self.requestBindingChef();
				        },
				        error : function(error) { 
				        		console.log(error)
				        		this.$Message.error('数据转移错误');
				        }
		   		    });
				} else{
					self.$Message.warning('请输入正确工号');
				}	
			},
			changgePage(index){
				this.pageIndex = index;
				this.requestBindingChef();
			}
		},
		mounted:function(){
			var self =this;
			//
			this.SalemanId =this.$route.query.SalemanId;
			self.requestInformation()


			
			
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
.layout-assistant{
    width: 300px;
   /* margin: 0 auto;*/
   bottom: 10px;
    text-align: left;
    height: 3rem;
    line-height: 3rem;
}
.ivu-menu-horizontal.ivu-menu-light:after{
	bottom: 11px;
}
.content{
	margin-top: 1rem;	
}

.BindingChef{
	display: none;
}
.InformationUl{
	text-align: center;
	border: 1px solid gainsboro;
	width: 38%;
	margin-left: 7rem;	
	border-radius:7px ;
	text-align: left;
	padding-bottom: 1rem;
	padding-right: 1rem;
	padding-top: 1rem;
	position: relative;
}
.InformationUl li{
	margin-left: 2rem;
	margin-top: 0.5rem;
	height: 2rem;	
	line-height: 2rem;
}
.InformationUl li span{
	display: block;
	float: left;
	position: absolute;
}
.title{
	/*font-size: 1.2rem;*/
	font-weight: bold;
	padding-right: 1rem;
	
}
.value{
	/*font-size: 1.2rem;*/
	padding-left: 1rem;
	color: grey;
	left: 3rem;
	margin-left: 6rem;
}
.BindingChef{
	width: 100%;
	margin: 0 auto;
}
.page{
	margin-top: 1rem;
	float: right;
}
</style>
<style>
	html, body { min-width: 1200px; }
.BindingChef .ivu-table-header table{
	width: 100%;
}
.BindingChef .ivu-table-body table{
	width: 100%;
}
</style>