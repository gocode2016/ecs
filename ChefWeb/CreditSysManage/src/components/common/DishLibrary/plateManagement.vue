<template>
  <!--版块管理-->
  <div class="plateManagement">
    <div>
    	<Button type="primary"  style="margin:0  0.5rem;" @click='add_click'>新增</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='isRelease(1)'>发布</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='isRelease(0)'>不发布</Button>
	    <Date-picker type="daterange" v-model='plate.datetime' placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 180px;margin-left: 10%;" ></Date-picker>
			<Input  placeholder="可搜索表内任意字段" style="width: 150px" v-model='plate.information'></Input>
			<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>查询</Button>
    </div>

		<Table style="margin-top: 2rem;" :columns="table_columns" :data="tabInfoList" @on-selection-change='select_click' @on-sort-change="sort_change"></Table>
		<Page  :total='plate.count' :current='plate.pageIndex' show-elevator class='page' :page-size='10' @on-change='changgePage' show-total style='margin-bottom: 1rem;'></Page>
		<!--新增-->
    <Modal title="创建新板块" v-model="createFlag" width='20%' :mask-closable="false" class="modal" >
			<Input  placeholder="输入板块名称" style="width: 60%;margin:10px 20%;" v-model='plateName'></Input>
			<p><Button type="primary"  style="width:30%;margin:20px 35%;" @click='create_click'>创建</Button></p>
    </Modal>
    <!--修改-->
    <Modal title="修改板块内容" v-model="editFlag" width='20%' :mask-closable="false" class="modal" >
			<Input v-model='editPlateName' style="width: 60%;margin:10px 20%;" ></Input>
			<p style="text-align: center;" class="edit_text"><span>菜品：{{editInfo.CpCount}}</span><span>视频：{{editInfo.SpCount}}</span><span>专页：{{editInfo.ZtCount}}</span></p>
			<p><Button type="primary"  style="width:30%;margin:20px 35%;" @click='edit_click'>确定</Button></p>
    </Modal>
    <!--删除提示弹框-->
    <Modal v-model="model_delete" width="360">
      <p slot="header" style="color:#f60;text-align:center">
        <Icon type="information-circled"></Icon>
        <span>删除确认</span>
      </p>
      <div style="text-align:center">
        <p>您确定要删除此条数据吗?</p>
      </div>
      <div slot="footer" style="text-align: center;">
        <Button type="error" size="small" @click="delBtn">删除</Button>
      </div>
    </Modal>

  </div>
</template>

<script>
	export default{
		components:{

		},
		data(){
			return{
				createFlag:false,
				editFlag:false,
				editInfo:{},//编辑弹框 信息
				plateName:'',//板块名称
				editPlateName:'',//编辑板块名称
				BanKuaiId:'',//板块id
				selectArr:[],//复选框选中元素
				StartTime:'',//开始日期
				EndTime:'',//结束日期
				model_delete:false,//删除提示框
        deleteData:'',//删除数据
				plate:{
					datetime:[],//搜索时间
					information:'',//搜索关键字
					count:0,
					pageIndex:1
				},
				table_columns:[
					{
							title:'选择',
	            type: 'selection',
	            align: 'center',
	            width:'70'
	        },
	        {
	            title:'序号',
	            align: 'center',
	            type: 'index',
	            width:'70'
	        },
						{
	            title:'板块ID',
	            align: 'center',
	            key:'BanKuaiId'
	        },
	        {
	            title: '板块名称',
	            align: 'center',
	            key: 'BanKuaiName',
	        },
	        {
	            title: '菜品数量',
	            align: 'center',
	            key: 'CpCount',
//	            sortable: 'custom',
	            sortable: true
	        },
	        {
	            title: '视频数量',
	            align: 'center',
	            key: 'SpCount',
//	            sortable: 'custom',
	            sortable: true
	        },
	        {
	            title: '专页数量',
	            align: 'center',
	            key: 'ZtCount',
	            sortable: 'custom',
	            sortable: true
	        },
	        {
	            title: '创建时间',
	            align: 'center',
	            key: 'CreateDate'
	        },
	        {
	            title: '状态',
	            align: 'center',
	            key: 'IsPublish'
	        },
	        {
	            title: '操作',
	            align: 'center',
	            width:150,
	            render: (h, params) => {
	                return h('div', [
	                    h('Button', {
	                        props: {
	                            type: 'primary',
	                            size: 'small'
	                        },
	                        style:{
	                        	width:'50px'
	                        },
	                        on: {
	                            click: () => {
	                                this.redact_click(params)
	                            }
	                        }
	                    }, '编辑'),
	                    h('Button', {
	                        props: {
	                            type: 'error',
	                            size: 'small'
	                        },
	                        style:{
	                        	width:'50px',
	                        	marginLeft:'0.8rem'
	                        },
	                        on: {
	                            click: () => {
	                                this.delete_click(params)
	                            }
	                        }
	                    }, '删除')
	                ]);
	            }
	        }
				],
				tabInfoList:[

				]
			}
		},
		methods:{
			search_click(){//查询
				if(this.plate.datetime.length<=0 || this.plate.datetime[0]==null){
					this.StartTime = '';
					this.EndTime = '';
				}else{
					var one = new Date(this.plate.datetime[0]);
					var two = new Date(this.plate.datetime[1]);
					var omonth = test(one.getMonth() + 1);
					var odate = test(one.getDate());
					var tmonth = test(two.getMonth() + 1);
					var tdate = test(two.getDate());
					function test(val){
						if(val<10){
							val='0'+val;
						}
						return val;
					}
					this.StartTime = one.getFullYear() + '-' + omonth + '-' + odate ;
					this.EndTime = two.getFullYear() + '-' + tmonth + '-' + tdate;
				}
				this.plate.pageIndex = 1;
				this.getInfoList();//获取菜谱分级列表
			},
			add_click(){//新增
				this.plateName = '';//板块名称置空
				this.createFlag = true;//新增弹框
			},
			create_click(){//创建板块名称
				if(this.plateName == ''){
					this.$Message.error('内容不能为空');
				}else{
						var self = this;
						var params = {
							"BanKuaiName": this.plateName
						}
						$.ajax({
							type:'post',
							url:URLHeader.foodLibrary+'/Api/BanKuai/Create',
							dataTape:'json',
							contentType:'application/json; charset=utf-8',
							data:JSON.stringify(params),
							success:function(res){
								var data = JSON.parse(res);
								if(data.status == 'succ'){
									if(data.message == '创建成功'){//创建成功后
										self.$Message.success('创建成功');
										self.createFlag = false;
										self.plateName = '';
										self.getInfoList();//获取板块管理列表
									}
								}else{
									self.$Message.error(data.message);
								}
							},
							error:function(error){
								self.$Message.error('创建失败');
							}
						})
				}
			},
			select_click(params){//选中复选框
//				console.log(params);
				this.selectArr = params;
			},
			isRelease(flag){//是否发布
				if(this.selectArr.length <= 0){
					this.$Message.error('请选择');
				}else{
					var BanKuaiId = this.selectArr[0].BanKuaiId ;
					var self = this ;
					var params = {
					    "Flag": flag,//0不发布 1发布
					    "BanKuaiId": BanKuaiId
					}
					$.ajax({
						type:'post',
						url:URLHeader.foodLibrary+'/Api/BanKuai/Publish',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(params),
						success:function(res){
							var data=JSON.parse(res);
							if(data.status == "succ"){
								self.$Message.success('操作成功');
								self.getInfoList();//获取板块管理列表
							}
					  },
						error:function(error){
							self.$Message.error('请求失败');
						}
					})
				}
			},
			redact_click(params){//编辑
//				console.log(params.row);
				this.editInfo = params.row;
				this.editFlag=true;//编辑弹框
				this.editPlateName = params.row.BanKuaiName;
				this.BanKuaiId = params.row.BanKuaiId;
			},
			edit_click(){//编辑确认
				if(this.editPlateName == ''){
					this.$Message.error('内容不能为空');
				}else{
					var self = this;
					var params = {
						"BanKuaiId": this.BanKuaiId,
						"BanKuaiName": this.editPlateName
					}
					$.ajax({
						type:'post',
						url:URLHeader.foodLibrary+'/Api/BanKuai/Edit',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(params),
						success:function(res){
							var data = JSON.parse(res);
							if(data.status == 'succ'){
								if(data.message == "更新成功"){//创建成功后
									self.$Message.success('更新成功');
									self.editFlag = false;
									self.getInfoList();//获取板块管理列表
								}
							}else{
								self.$Message.error(data.message);
							}
						},
						error:function(error){
							self.$Message.error('更新失败');
						}
					})
				}
			},
			delete_click(params){//点击列表删除按钮
				this.model_delete = true;
				this.deleteData = params;
			},
			delBtn(){//点击弹框删除按钮
				if(this.deleteData.row.IsPublish=='已发布'&&(this.deleteData.row.CpCount!=0 || this.deleteData.row.SpCount!=0 || this.deleteData.row.ZtCount!=0)){//不能删除
					this.$Message.error('已发布，无法删除');
				  this.model_delete = false;
				}else{
			  	this.deleteApi(this.deleteData);//删除接口
				}
			},
			deleteApi(params){//删除接口
				var BanKuaiId = params.row.BanKuaiId;
				var self = this;
				var params = {
					"Flag": 0,//0不启用 1启用
    			"BanKuaiId": BanKuaiId
				}
				$.ajax({
					type:'post',
					url:URLHeader.foodLibrary+'/Api/BanKuai/Enable',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(params),
					success:function(res){
						var data = JSON.parse(res);
						if(data.status == "succ"){//删除成功
							self.$Message.success('删除成功');
							self.model_delete = false;//隐藏删除弹框
							self.getInfoList();//获取板块管理列表
						}else{
						  self.$Message.error(data.message);
						}
					},
					error:function(error){
						self.$Message.error('删除失败');
					}
				})
			},
			sort_change(val){//tab表头排序
//				console.log(val);
			},
			changgePage(index){//分页切换
				this.plate.pageIndex = index;
				this.getInfoList();//获取板块管理列表
			},
			getInfoList(){//获取板块管理列表
				var self = this;
				var params = {
					"page": this.plate.pageIndex,
			    "pagesize": 10,
			    "startdate": this.StartTime,
			    "enddate":this.EndTime,
			    "keyword": this.plate.information
				}
				$.ajax({
					type:'post',
					url:URLHeader.foodLibrary+'/Api/BanKuai/Query',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(params),
					success:function(res){
						var data = JSON.parse(res);
						self.plate.count = data.totalcount;//条数
						self.tabInfoList = data.data;
						for(let i in self.tabInfoList){
							if(self.tabInfoList[i].CpCount == null){//菜品数量
								self.tabInfoList[i].CpCount = 0;
							}
							if(self.tabInfoList[i].SpCount == null){//视频数量
								self.tabInfoList[i].SpCount = 0;
							}
							if(self.tabInfoList[i].TwCount == null){//图文数量
								self.tabInfoList[i].TwCount = 0;
							}
							if(self.tabInfoList[i].ZtCount == null){//专页数量
								self.tabInfoList[i].ZtCount = 0;
							}
						}
//						console.log(self.tabInfoList);
					},
					error:function(error){
//						console.log(error);
					}
				})
			}
		},
		mounted(){
			this.getInfoList();//获取板块管理列表
		}
	}
</script>

<style scoped>
.page{
	margin-top: 1rem;
	float: right;
}
.edit_text span{
	width: 30%;
	display: inline-block;
}
</style>
<style>
.modal .ivu-modal-footer{display: none !important;}
.ivu-modal-footer{border: none !important;}
.ivu-modal-header{border: none !important;}
</style>
