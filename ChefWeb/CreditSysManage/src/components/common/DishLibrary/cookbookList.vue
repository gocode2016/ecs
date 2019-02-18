<template>
  <!--菜谱分级-->
  <div class="cookbookList">
    <div>
    	<Button type="primary"  style="margin:0  0.5rem;" @click='add_click'>新增</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='isRelease(1)'>发布</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='isRelease(0)'>不发布</Button>
	    <Date-picker type="daterange" v-model='cooklist.datetime' placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 180px;margin-left: 10%;" ></Date-picker>
			<Input  placeholder="输入关键字" style="width: 150px" v-model='cooklist.information'></Input>
			<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>查询</Button>
    </div>

		<Table style="margin-top: 2rem;" :columns="table_columns" :data="cookInfoList" @on-selection-change='select_click'></Table>
		<Page  :total='cooklist.count' :current='cooklist.pageIndex' show-elevator class='page' :page-size='10' @on-change='changgePage' show-total style='margin-bottom: 1rem;'></Page>

    <!--新增弹框-->
    <Modal title="创建菜谱分级" v-model="createFlag" width='40%' :mask-closable="false" class="modal" >
    	<div class="box_content">
    		<div class="box_a">
				  <ul>
				  	<li v-for='(item,index) in firstClassName' style="margin-top: 5px;">
				  		<Input  v-model='item.FirstName' :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入一级分类" style="width: 200px;"></Input>
				  		<a @click='removeFirstClass(item)' style="cursor: pointer;"> <Icon type="ios-close-outline" size='20'></Icon></a>
				  	</li>
				  </ul>
					<p>
						<Button type="primary"  style="margin:10px 0;" @click='addFirstClass'>新增</Button>
						<Button type="primary"  style="margin:10px 0;" @click='firstclass_click'>创建</Button>
					</p>
    		</div>
    		<div class="box_b">
    			<Select v-model="model" placeholder='请选择一级分类' style="width:200px">
			      <Option v-for="item in firstClassList" :value="item.FirstId" :key="item.FirstId">{{ item.FirstName }}</Option>
			    </Select>
				  <p><Input  v-model='secondClassName' :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入二级分类" style="width: 200px;margin-top: 5px;"></Input></p>
					<p><Button type="primary"  style="margin:10px 0;" @click='secondclass_click'>创建</Button></p>
    		</div>
    	</div>
	  </Modal>
	  <!--修改弹框-->
	  <Modal title="修改菜谱分级" v-model="editFlag" width='20%' :mask-closable="false" class="modal">
			<Select v-model="selectFirstId" placeholder='请选择一级分类' style="width:200px">
			  <Option v-for="item in firstClassList" :value="item.FirstId" :key="item.FirstId">{{ item.FirstName }}</Option>
			</Select>
			<p><Input  v-model='editSecondName' :autosize="{minRows: 1,maxRows: 5}"  style="width: 200px;margin-top: 5px;"></Input></p>
			<p><Button type="primary"  style="margin:10px 0;" @click='edit_click'>确认</Button></p>
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
				createFlag:false,//新增弹框
				editFlag:false,//编辑弹框
        model_delete:false,//删除提示框
        deleteData:'',//删除数据
				firstClassName:[
					{
						'FirstName':''
					}
				],
				secondClassName:'',
				firstClassList:[//一级分类列表

				],
				model:'',//新增一级分类下拉框
				editFirstName:'',//编辑一级分类
				editSecondName:'',//编辑二级分类
				FirstId:'',//一级分类id
				selectFirstId:'',//编辑一级分类下拉框id
				SecondId:'',//二级分类id
				cooklist:{
					datetime:[],//搜索时间
					information:'',//搜索关键字
					count:0,//数据总条数
					pageIndex:1,//当前页数
				},
				StartTime:'',//开始日期
				EndTime:'',//结束日期
				selectArr:[],//复选框选中集合
				table_columns:[//table表头
					{
							title:'选择',
	            type: 'selection',
	            align: 'center',
	//                      width:80
	        },
	        {
	            title:'NO.',
	            align: 'center',
	            type: 'index',
	//                      width:80
	        },
						{
	            title:'一级分类名称',
	//                      width:150,
	            align: 'center',
	            key:'FirstName'
	        },
	        {
	            title: '二级分类名称',
	            align: 'center',
	            key: 'SecondName',
	//        	width:150
	        },

	        {
	            title: '排序',
	            align: 'center',
	            key: 'RowId',
	//          width:150,
	        },
	        {
	            title: '创建时间',
	            align: 'center',
	            key: 'CreateDate'
	        },
	        {
	            title: '下属菜品数量',
	            align: 'center',
	            key: 'Total'
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
        cookInfoList:[

        ],
			}
		},
		methods:{
			search_click(){//查询
				if(this.cooklist.datetime.length<=0 || this.cooklist.datetime[0]==null){
					this.StartTime = '';
					this.EndTime = '';
				}else{
					var one = new Date(this.cooklist.datetime[0]);
					var two = new Date(this.cooklist.datetime[1]);
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
				this.cooklist.pageIndex = 1;
				this.getCookInfo();//获取菜谱分级列表
			},
			add_click(){//新增
				this.createFlag = true;
				this.model = '';//一级下拉框置空
				this.secondClassName = '';//二级分类置空
			},
			addFirstClass(){//增加一级分类
				var item={
					'FirstName':''
				}
				this.firstClassName.push(item);
			},
			removeFirstClass(item){//删除当前一级分类
	      this.firstClassName.splice(this.firstClassName.indexOf(item), 1)
			},
			getFirstClass(){//获取一级分类
				var self = this;
				$.ajax({
					type:'get',
					url:URLHeader.foodLibrary+'/Api/Category/GetFirst',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:{},
					success:function(res){
						var data = JSON.parse(res);
						self.firstClassList = data.data;
					},
					error:function(error){
						self.$Message.error('获取失败');
					}
				})
			},
			firstclass_click(){//一级分类创建
				if(this.firstClassName.length == 0){
					this.$Message.error('内容不能为空');
				}
				for(let i in this.firstClassName){
					if(this.firstClassName[i].value == ''){
						this.$Message.error('内容不能为空');
						return false;
					}
				}

				var self = this;
				var params = {
				    "List": this.firstClassName
				}
				$.ajax({
					type:'post',
					url:URLHeader.foodLibrary+'/Api/Category/CreateFirst',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(params),
					success:function(res){
						var data=JSON.parse(res);
						if(data.status == 'succ'){//成功
							if(data.message == '添加成功'){
								self.$Message.success('一级创建成功');
								//创建成功后
								self.firstClassName=[//置空
									{
										'FirstName':''
									}
								]
							  self.getFirstClass();//获取一级分类
							}
						}else{
							self.$Message.error(data.message);
						}
					},
					error:function(error){
						self.$Message.error('创建失败');
					}
				})
			},
			secondclass_click(){//二级分类创建
				if(this.model == '' || this.secondClassName == ''){
					this.$Message.error('内容不能为空');
				}else{
					for(let i in this.firstClassList){//获取一级分类名称
	          var firstClassList = this.firstClassList[i];
	          if(this.model == firstClassList.FirstId){
	          	var FirstName = firstClassList.FirstName;
	          }
					}
					if(FirstName == this.secondClassName){//一二级分类名称相同时不能创建
						this.$Message.error('一二级名称相同，创建失败');
					}else{
						var self = this;
						var params={
							 "List": [
						        {
						            "SecondName": this.secondClassName,
						            "FirstId": this.model
						        }
						    ]
						}
						$.ajax({
							type:'post',
							url:URLHeader.foodLibrary+'/Api/Category/CreateSecond',
							dataTape:'json',
							contentType:'application/json; charset=utf-8',
							data:JSON.stringify(params),
							success:function(res){
								var data=JSON.parse(res);
								if(data.status == 'succ'){
									if(data.message == '添加成功'){//创建成功后
										self.$Message.success('创建成功');
										self.secondClassName='';
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
				}
			},
			isRelease(flag){//是否发布
				if(this.selectArr.length <= 0){
					this.$Message.error('请选择');
				}else{
					var secondId = this.selectArr[0].SecondId ;
					var self = this ;
					var params = {
					    "Flag": flag,//0不发布 1发布
					    "SecondId": secondId
					}
					$.ajax({
						type:'post',
						url:URLHeader.foodLibrary+'/Api/Category/Publish',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(params),
						success:function(res){
							var data=JSON.parse(res);
							if(data.status == "succ"){
								self.$Message.success('操作成功');
								self.getCookInfo();//获取菜谱分级列表
							}
						},
						error:function(error){
							self.$Message.error('请求失败');
						}
					})
				}
			},
			select_click(params){//点击tab复选框
				this.selectArr = params;
//				console.log(this.selectArr);
			},
			redact_click(params){//编辑
				this.editFirstName = params.row.FirstName;
				this.editSecondName = params.row.SecondName;
				this.FirstId = params.row.FirstId;
				this.selectFirstId = params.row.FirstId;//下拉选择
				this.SecondId = params.row.SecondId;
				this.editFlag=true;
			},
			edit_click(){//编辑弹框确认
				var self = this ;
				for(let i in this.firstClassList){//获取一级分类名称
          var firstClassList = this.firstClassList[i];
          if(this.selectFirstId == firstClassList.FirstId){
          	this.editFirstName = firstClassList.FirstName;
          }
				}
				if(this.editFirstName == this.editSecondName){//一二级名称相同无法创建
					this.$Message.error('一二级名称不能相同，修改失败');
				}else{
					var params = {
				    "FirstId": this.FirstId,
				    "FirstName": this.editFirstName,
				    "SecondId":this.SecondId,
				    "SecondName": this.editSecondName
					}
					$.ajax({
						type:'post',
						url:URLHeader.foodLibrary+'/Api/Category/Edit',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(params),
						success:function(res){
							var data=JSON.parse(res);
							if(data.status == "succ"){
								self.$Message.success('修改成功');
								//修改成功后
								self.editFlag=false;
								self.getCookInfo();//获取菜谱分级列表
							}else{
								self.$Message.error(data.message);
							}
						},
						error:function(error){
							self.$Message.error('修改失败');
						}
					})	
				}
			},
			delBtn(){//点击删除
        this.deleteApi(this.deleteData);
			},
			delete_click(params){//点击列表删除按钮
				this.model_delete = true;
				this.deleteData = params;
			},
			deleteApi(params){//操作删除接口
				var SecondId = params.row.SecondId ;
				var self = this;
				var params = {
				    "Flag": 0,//0不启用 1启用
				    "SecondId": SecondId
				}
				$.ajax({
					type:'post',
					url:URLHeader.foodLibrary+'/Api/Category/Enable',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(params),
					success:function(res){
						var data=JSON.parse(res);
//						console.log(data);
						//删除成功
						self.$Message.success('删除成功');
						self.getCookInfo();//获取菜谱分级列表
						self.deleteData = '';//删除数据清空
						self.model_delete = false;//弹框关闭
					},
					error:function(error){
						self.$Message.error('删除失败');
						self.deleteData = '';
						self.model_delete = false;//弹框关闭
					}
				})
			},
			getCookInfo(){//获取菜谱分级列表
				var self = this;
				var params = {
					  "page": this.cooklist.pageIndex,
				    "pagesize": 10,
				    "startdate":this.StartTime,
				    "enddate": this.EndTime,
				    "keyword": this.cooklist.information
				}
				$.ajax({
					type:'post',
					url:URLHeader.foodLibrary+'/Api/Category/Query',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(params),
					success:function(res){
						var data=JSON.parse(res);
//						console.log(data);
						self.cookInfoList = data.data;
						self.cooklist.count = data.totalcount;
					},
					error:function(error){
						self.$Message.success('获取失败');
					}
				})
			},
			changgePage(index){//分页切换
				this.cooklist.pageIndex = index;
        this.getCookInfo();
			}
		},
		mounted(){
			this.getCookInfo();//获取菜谱分级列表
			this.getFirstClass();//获取一级分类
		}
	}
</script>

<style scoped>
.page{
	margin-top: 1rem;
	float: right;
}
.box_content{display: flex;}
.box_content div{flex: 1;}
.box_a a{position: relative;top: 3px;}
</style>
<style>
.modal .ivu-modal-footer{display: none !important;}
.ivu-modal-footer{border: none !important;}
.ivu-modal-header{border: none !important;}
</style>
