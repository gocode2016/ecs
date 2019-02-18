<template>
	<div class="cooker">
		<div class="search_div">
			<span>客户编码状态:</span>
			<Select v-model="cookSearch.CodeState" style="width:100px;margin-left:  0.5rem;margin-right:  0.5rem;" placeholder='状态选择'>
		        <Option v-for="item in code_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
		    </Select>
		    <!--<span>会员状态:</span>
		    <Select v-model="cookSearch.classification" style="width:100px;margin-left: 0.5rem;" placeholder='分类选择'>
		        <Option v-for="item in classificationList" :value="item.value" :key="item.value">{{ item.label }}</Option>
		    </Select>-->

		    <span>认证状态:</span>
		    <Select v-model="cookSearch.AuthState" style="width:120px;margin-left:  0.5rem;" placeholder='认证状态'>
		        <Option v-for="item in AuthStateLise" :value="item.value" :key="item.value">{{ item.label }}</Option>
		    </Select>
			<span style="font: 1rem;margin: 0 0.8rem;">注册时间:</span>
			<Date-picker type="daterange" v-model='cookSearch.registTime' placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 180px" ></Date-picker>
			<Input  placeholder="姓名/酒店/手机号" style="width: 150px" v-model='cookSearch.information'></Input>
			<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>搜索</Button>
			<!--<Button type="primary"  @click='batchReview'>批量审批</Button>-->
			<Dropdown trigger="custom" :visible="visible" style="margin-left: 20px">
		       <Button type="primary" @click='handleOpen'>导出数据</Button>
		        <DropdownMenu slot="list">
		            <div style="width: 17rem;">
		            		<span style="margin-left: 1rem;">注册时间:</span>
		            		<Date-picker type="daterange" placement="bottom-end" v-model='exporTime' class='exporTime' placeholder="选择日期" style="width: 180px;margin-left: 1rem;" ></Date-picker>
		            </div>
		            <div style="text-align: right;margin:10px;">
		            	<Button type="primary" @click="handleClos">关闭</Button>
		                <Button type="primary" @click="exportReport">导出</Button>
		            </div>
		        </DropdownMenu>
		    </Dropdown>
			<Button type="primary"  @click='membersTransfer'>批量转移</Button>
			<Button type="primary" @click='DownloadTemplate'>模板下载</Button>


			 <input type="file" @change="importf()"  id="import" style="display: none;"/>
		</div>
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<Table style="margin-top: 1rem;" :columns="table_columns"  :data="cook_tableList" @on-selection-change="selectCooks"  ref="table"></Table>
		<Page  :total="cookSearch.Count" :current='cookSearch.pageIndex' show-elevator class='page' :page-size='20' @on-change='changgePage' show-total style='margin-bottom: 1rem;'></Page>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				visible:false,
				exporTime:[],//导出时间条件
//				pageIndex:1,
//				Count:0,//总条数
//				registTime:'',//注册时间
//				starData:'',//开始时间
//       		endData:'',//结束时间
//       		information:'',//模糊搜索
//       		CodeState:'-1',//状态
//       		classification:'0',//分类
         		cookSearch:{//搜索数据
         			pageIndex:1,
					Count:0,//总条数
					registTime:[],//注册时间
					starData:'',//开始时间
	         		endData:'',//结束时间
	         		information:'',//模糊搜索
	         		CodeState:'-1',//状态
	         		classification:'0',//分类
	         		AuthState:0,//认证转台
         		},

         		TransferMemberList:[],//Excel导入数据
         		auditList:[],//选择后存储数据
         		URL:{
         			ReviewMember:URLHeader.user+'/api/Member/ReviewMember',//批量审核厨师
         			GetPageMember:URLHeader.user+'/api/Member/GetPageMember',//厨师列表数据
         			exportReport:URLHeader.user+'/api/Member/ExportMember'
         		},
				code_List:[//状态select
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
         		classificationList:[//分类
         			{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '1',
                        label: '未认证'
                    },
                    {
                        value: '2',
                        label: '已认证'
                    }
         		],
         		AuthStateLise:[//认证状态
         			{
                        value: 0,
                        label: '全部'
                    },
                    {
                        value: 1,
                        label: '未认证'
                    },
                    {
                        value: 2,
                        label: '实名已认证'
                    },
                    {
                        value: 3,
                        label: '实名未认证'
                    },
                    {
                        value: 4,
                        label: '实名认证未通过'
                    },
                    {
                        value: 5,
                        label: '注册码认证'
                    }
         		],
         		table_columns:[//table表头
//       			{
//                      type: 'selection',
//                      align: 'center',
//                      width:70,
//                   },
         			{
                        title:'ID',
                        width:70,
                        align: 'center',
                        key:'MemberId'
                     },
                     {
                        title: '姓名',
                        align: 'center',
                        width:90,
                        key: 'MemberName',
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
//                                          this.cooksDetails(params)
//                                      }
//                             }
//
//                          },params.row.MemberName)
//                        }
                    },
                    {
                        title: '联系方式',
                        align: 'center',
                        key: 'MemberTelePhone',
                     	width:120
                    },

                    {
                        title: '酒店省',
                        key: 'ProvinceName',
                      width:80,
                        align: 'center',
                    },
                    {
                        title: '酒店市',
                        align: 'center',
                        width:80,
                        key: 'CityName'
                    },
                    {
                        title: '酒店区',
                        align: 'center',
                        width:90,
                        key: 'AreaName'
                    },
//                  {
//                      title: '酒店地址',
//                      align: 'center',
//                      width:100,
//                      key: 'HotelAddress'
//                  },
                    {
                        title: '酒店名称',
                        align: 'center',
                        width:90,
                        key: 'HotelName'
                    },
                    {
                        title: '注册时间',
                        align: 'center',
                        key: 'RegistDate',
                        width:90
                    },
//                  {
//                      title: '分类',
//                      align: 'center',
//                      key: 'classiFication',
////                      width:60
//                   },
//       			{
//                      title: '绑定队员',
////                      width:90,
//                      key: 'bindingTeam',
//                      align: 'center',
//                  },

                    {
                        title: '当前积分',
                        align: 'center',
                        width:90,
                        key: 'LeaveIntegral'
                    },
                     {
                        title: '活跃度',
                        align: 'center',
                        width:80,
                        key: 'MemberActiveStateStr'
                    },
                     {
                        title: '操作',
                        align: 'center',
                        key: 'operation',
                        width:80,
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    on: {
                                        click: () => {
//                                          this.redact(params)
											this.cooksDetails(params)
                                        }
                                    }
                                }, '详情')
                            ]);
                        }
                    }
         		],
         		cook_tableList:[],//table数据
         		export_columns:[
         			{
                        title: '会员id',
                        key: '会员id'
                    },
                    {
                        title: '会员姓名',
                        key: '会员姓名'
                    },
                    {
                        title: '会员手机号',
                        key: '会员手机号'
                    },
                    {
                        title: '新岗位',
                        key: '新岗位'
                    },
                    {
                        title: '会员状态',
                        key: '会员状态'
                    },
                    {
                        title: '剩余积分',
                        key: '剩余积分'
                    },
                    {
                        title: '备注',
                        key: '备注'
                    },
                    {
                        title: '大区',
                        key: '大区'
                    },
                    {
                        title: '完善编码时间',
                        key: '完善编码时间'
                    },
                    {
                        title: '客户编码',
                        key: '客户编码'
                    },
                    {
                        title: '工作岗位',
                        key: '工作岗位'
                    },
                    {
                        title: '总积分',
                        key: '总积分'
                    },
                    {
                        title: '截至目前正常扫描产品获得的积分',
                        key: '截至目前正常扫描产品获得的积分'
                    },
                    {
                        title: '昵称',
                        key: '昵称'
                    },
                    {
                        title: '注册时间',
                        key: '注册时间'
                    },
                    {
                        title: '活跃度',
                        key: '活跃度'
                    },
                    {
                        title: '酒店区',
                        key: '酒店区'
                    },
                    {
                        title: '酒店名称',
                        key: '酒店名称'
                    },
                    {
                        title: '酒店地址',
                        key: '酒店地址'
                    },
                    {
                        title: '酒店市',
                        key: '酒店市'
                    },
                    {
                        title: '酒店省',
                        key: '酒店省'
                    },
                     {
                        title: '队员名称',
                        key: '队员名称'
                    },
                     {
                        title: '队员工号',
                        key: '队员工号'
                    },
                    {
                        title: '队员电话',
                        key: '队员电话'
                    },
                    {
                        title: '首次推荐队员ID',
                        key: '首次推荐队员ID'
                    },
         		],

			}
		},
		methods:{
			DownloadTemplate(){//模板下载
				window.location.href =URLHeader.Tools+'/UploadFiles/MemberTransfer.xlsx'
			},
			search_click(){//搜索按钮
				this.cookSearch.pageIndex = 1
				this.requestCookerData();
			},
			batchReview(){//批量审核
				var self =this;
				if (self.auditList.length>0) {
					$.ajax({
						type:"post",
						url:self.URL.ReviewMember,
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(self.auditList),
						success:function(json){
							var dataAll = JSON.parse(json);
							self.$Message.success('审批成功');
						},
						error:function(error){

						}
					});

				} else{
					self.$Message.warning('请选择要审批的厨师');
				}

			},
			membersTransfer(){//会员批量转移按钮
				$('#import').click()
			},
			importf(){//导入函数
				var self =this
				 var wb;//读取完成的数据
				var obj = document.getElementById("import");
				if(!obj.files) {
                    return;
                }
                var f = obj.files[0];
                var reader = new FileReader();
                reader.onload = function(e) {
                    var data = e.target.result;
                        wb = XLSX.read(data, {
                            type: 'binary'
                        });
                    //wb.SheetNames[0]是获取Sheets中第一个Sheet的名字
                    //wb.Sheets[Sheet名]获取第一个Sheet的数据
                    //self.TransferMemberList= XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]])
                    var error = false
                    self.TransferMemberList = []
                 	XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]).map((item) =>{
                 		if (!item.OldSalemanId || !item.MemberId || !item.NewSalemanId) {
                 			error = true
                 		}
                 		item.MemberId = parseInt(item.MemberId)
                 		item.OldSalemanId = parseInt(item.OldSalemanId)
                 		item.NewSalemanId = parseInt(item.NewSalemanId)
                 		self.TransferMemberList.push(item)
               	     })
                 	//陈秀荣

                 	if (!error) {//数据正确
                 		var url = URLHeader.user+'/api/Member/TransferMemberList'
                 		var DATA = '{"List":'+JSON.stringify(self.TransferMemberList)+'}'
                 		console.log(DATA)
	                    $.ajax({
					        type :"post",
					        url :url,///cts/core/
					        dataType : 'json',
					        contentType: "application/json; charset=utf-8",
					        data:DATA,
					        success : function(json) {
								console.log(json)
								self.$Message.success('会员转移成功');
								self.requestCookerData()
					        },
					        error : function(error) {
					        		console.log(error)
					        		self.$Message.error('数据上传失败');
					        }
			   		    });
                 	} else{
                 		self.$Message.error({
			                content: '请认真填写数据,数据不能为空',
			                duration: 3
			            });
                 	}

                    console.log(self.TransferMemberList)

              };
                    reader.readAsBinaryString(f);

			},
			requestCookerData(){//数据请求

				var self =this;
				//console.log(self.cookSearch)
				$('.loding').show()
				if (self.cookSearch.registTime.length ==0) {
					//console.log('无时间')
         			self.cookSearch.starData='';//开始时间
         			self.cookSearch.endData ='';//结束时间
				} else{
					//console.log('有时间')
					var starTime = new Date(self.cookSearch.registTime[0]);
					self.cookSearch.starData = starTime.getFullYear() + '-' + (starTime.getMonth() + 1) + '-' + starTime.getDate()
					var endTime = new Date(self.cookSearch.registTime[1]);
					self.cookSearch.endData = endTime.getFullYear() + '-' + (endTime.getMonth() + 1) + '-' + endTime.getDate()
				}
				let DATA ='{"IndexPage":"'+self.cookSearch.pageIndex+'","PageSize":"20","Identity":"'+self.cookSearch.classification+'","SalemanId":"0","CodeState":"'+self.cookSearch.CodeState+'","SearchInfo":"'+self.cookSearch.information+'","RegistBeginTime": "'+self.cookSearch.starData+'","RegistEndTime":"'+self.cookSearch.endData+'","AuthState":'+self.cookSearch.AuthState+'}';
				self.cook_tableList=[];
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :self.URL.GetPageMember,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	  self.cookSearch.Count = parseInt(dataAll.Count);
			        	  //console.log(dataAll)
			        	 self.dataIntegration(dataAll.data);
			        	 $('.loding').hide()

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
         				data[i].IsEnable='未审批';
         				data[i]._disabled =false;
         			} else{
         				data[i].IsEnable='已审批';
         				data[i]._disabled =true;
         			}
         			if (data[i].MemberActiveState==1) {//高
         				data[i].MemberActiveStateStr='高'
         			} else{//2低
         				data[i].MemberActiveStateStr='低'
         			}
					self.cook_tableList.push(data[i])
					$('.cooker .ivu-table table').css({
						'width':'100%'
					});
				}
				$('.loding').hide()
			},
			redact(name){//操作按钮

				this.$router.push({path: '/cookerEditor', query: {MemberId:name.row.MemberId}});//跳转厨师编辑页
			},
			cooksDetails(name){//厨师详情(姓名)
				console.log(name.row)
				this.setSearchData()
				//cookerAuditAfter
				//this.$router.push({path: '/cookersDetails', query: {MemberId: name.row.MemberId,MemberState:name.row.MemberState}});//跳转到详情页

				this.$router.push({path: '/cookerAuditAfter', query: {MemberId: name.row.MemberId,MemberState:name.row.MemberState}});//跳转到详情页

			},
			selectCooks(name){//选中事件
				var self =this;
				self.auditList=[];
				for (var i= 0;i<name.length;i++) {
					self.auditList.push(name[i].MemberId);
				}
			},
			changgePage(index){//分页切换
//				this.$route.query.index = this.pageIndex
//				console.log(this.$route.query.index)
				this.cookSearch.pageIndex = index;

         		this.requestCookerData();
         		//this.$router.push({path: '/cooker', query: {index:index}});//跳转厨师编辑页
         		//this.$route.query.index = index
         		//console.log(this.$route)
         		console.log('第'+index+'页')
			},
			setSearchData(){
				var self = this.cookSearch
				var cookSearch = {
					pageIndex:self.pageIndex,
					Count:self.Count,//总条数
					registTime:self.registTime,//注册时间
					starData:self.starData,//开始时间
	         		endData:self.endData,//结束时间
	         		information:self.information,//模糊搜索
	         		CodeState:self.CodeState,//状态
	         		classification:self.classification,//分类
	         		AuthState:self.AuthState,//认证状态
				}

				 sessionStorage.setItem("cookSearch",JSON.stringify(cookSearch));
				 //console.log(sessionStorage.getItem('cookSearch'))

			},
			handleOpen(){//点击导出弹出
				this.visible = !this.visible;
			},
			handleClos(){//点击关闭
				this.visible = false;
			},
			exportReport(){//数据导出
				var self = this
				$('.loding').show()
				this.visible = false;
				//exporTime
				//$('.exporTime').
				var starData=''
				var endData = ''
				if (self.exporTime.length ==0) {
					//console.log('无时间')
         			starData='';//开始时间
         			endData ='';//结束时间
				} else{
					//console.log('有时间')
					var starTime = new Date(self.exporTime[0]);
					starData = starTime.getFullYear() + '-' + (starTime.getMonth() + 1) + '-' + starTime.getDate()
					var endTime = new Date(self.exporTime[1]);
					endData = endTime.getFullYear() + '-' + (endTime.getMonth() + 1) + '-' + endTime.getDate()
				}
//				console.log(self.exporTime)
//				console.log(starData)
//				console.log(endData)
				var DATA = '{"StartDate":"'+starData+'","EndDate":"'+endData+'"}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :self.URL.exportReport,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {
			        	 	var dataAll = JSON.parse(json);
			        	 	console.log(dataAll)
						//export_columns
						self.$refs.table.exportCsv({
		                    filename: '会员数据报表',
		                    columns: self.export_columns,
		                    data: dataAll
		                 });
						$('.loding').hide()


//			        	  self.cookSearch.Count = parseInt(dataAll.Count);
//			        	  //console.log(dataAll)
//			        	 self.dataIntegration(dataAll.data);
//

			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });

			},
		},
		mounted:function(){
			//this.pageIndex = parseInt(this.$route.query.index);

			var cookSearch = JSON.parse(sessionStorage.getItem('cookSearch'))
//			console.log(cookSearch)
			if (cookSearch.Count >0) {//存在搜索条件
				console.log('读取旧数据')
				this.cookSearch = cookSearch
				this.requestCookerData();
			} else{
				console.log('加载新数据')
				this.pageIndex = 1
				this.requestCookerData();
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
</style>
<style>

</style>
