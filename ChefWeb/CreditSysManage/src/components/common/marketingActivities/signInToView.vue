<template>
	<!--签到查看-->
	<div class="signInToView">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="signInToView_Top">
			<ul style="float: left;margin-left: 2rem;">
				<li class="li">
					<span class="lable">活动名称</span>
					<span class="value">{{marketingInformation.ActName}}</span>
				</li>
				<li class="li">
					<span class="lable">有效期</span>
						<span class="value">{{marketingInformation.StartTime}} ~ {{marketingInformation.EndTime}}</span>
				</li>
				<li class="li">
					<span class="lable">状态</span>
						<span class="value">{{marketingInformation.ActStateStr}}</span>
				</li>
				<li class="li">
					<span class="lable">奖励积分</span>
						<span class="value">{{marketingInformation.Integral}}</span>
				</li>
				
				<li class="li">
					<span class="lable">活动所在大区</span>
					<span class="value">{{marketingInformation.ActivityRegion}}</span>
				</li>
				<li class="li">
					<span class="lable">签到人数</span>
					<!--<span class="value">{{marketingInformation.ActName}}</span>-->
				</li>
			</ul>
			<ul style="float: right;margin-left: 1rem;">
				<!--<li class="li">
					<span class="lable">销售类型</span>
					<span class="value">{{marketingInformation.ActivityType}}</span>
				</li>-->
				<!--<li class="li">
					<span class="lable">备注</span>
					<span class="value">{{marketingInformation.ActName}}</span>
				</li>-->
				<li class="li">
					<span class="lable">线下城市</span>
					<span class="value">{{marketingInformation.ActProvinceName}}/{{marketingInformation.ActCityName}}</span>
				</li>
				<li class="li">
					<span class="lable">活动类型</span>
					<span class="value">{{marketingInformation.ActivityType}}</span>
				</li>
				<li class="li">
					<span class="lable">操作</span>
					<span class="value">
						<Button type="primary" style="margin-left: 0rem;" @click="manuallyAddBtn">手动增加</Button>
						<Button type="primary" style="margin-left: 1rem;" @click="Upload">下载二维码</Button>
					</span>
				</li>
				<li class="li">
					<span class="lable">活动编码</span>
					<span class="value">{{marketingInformation.ActivityCode}}</span>
				</li>
				<li class="li">
					<span class="lable">活动描述</span>
					<span class="value">{{marketingInformation.ActivityDesc}}</span>
				</li>
				
			</ul>
			<Modal v-model="personneAddModel" width="360">
		        <p slot="header" style="color: cornflowerblue;text-align:center">
		            <Icon type="information-circled"></Icon>
		            <span>人员新增</span>
		        </p>
		        <div>
		           <Form ref="modelDate" :model="modelDate" :rules="ruleValidate" :label-width="80" label-position	='left' >
			           	<FormItem label="会员ID:" prop="UserId" >
				           <Input v-model="modelDate.UserId" style="width: 170px;"  placeholder="会员ID"></Input>
				        </FormItem>
				         <FormItem label="签到时间:" prop="SignDate">
				            <DatePicker v-model='modelDate.SignDate' type="datetime" placeholder="签到时间" style="width: 170px"></DatePicker>
				        </FormItem>
		           </Form>
		        </div>
		        <div slot="footer">
		            <Button type="primary"  @click="personnelAdditionBtn('modelDate')"  size="large" long>保存</Button>
		        </div>
		    </Modal>
		</div>
		<div class="signInToView_body">
			<div style="text-align: right;margin-bottom: 1rem;margin-top: 0.5rem;">
				<span>姓名:</span>
				<Input  placeholder="姓名" v-model= 'Search.MemberName' style="width: 150px;margin-left: 1rem;margin-right: 1rem;" number></Input>
				<span>会员ID:</span>
				<Input  placeholder="会员ID" v-model= 'Search.MemberId' style="width: 150px;margin-left: 0.5rem;"></Input>
				<Button type="primary" @click="getTableData"  style="margin-right: 1rem;margin-left: 1rem;" >查询</Button>
				<!--<Button type="primary" @click="exportCsv"  style="margin-right: 1rem;margin-left: 1rem;" >导出全部</Button>-->
				<!--<Button type="primary" style="margin-right: 1rem;margin-left: 1rem;" icon='ios-download-outline'>导出</Button>-->
			</div>
			<Table stripe :columns="columns" :data="signInToView_Data" ref="table"></Table>
			<Modal v-model="deletModel" width="360">
		        <p slot="header" style="color:#f60;text-align:center">
		            <Icon type="information-circled"></Icon>
		            <span>删除确认</span>
		        </p>
		        <div style="text-align:center">
		           <!-- <p>After this task is deleted, the downstream 10 tasks will not be implemented.</p>-->
		            <p>您确定要删除此会员吗?</p>
		        </div>
		        <div slot="footer">
		            <Button type="error" size="large" long  @click="modelDelteBtn">删除</Button>
		        </div>
		    </Modal>
			<Page  :total="tableTotal" :current="tableList_indexPage" show-elevator  :page-size='20' @on-change='changgePage' style='margin-right: 3rem;float: right;margin-top: 1rem;padding-bottom: 1rem;' show-total></Page>
		</div>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				deletModel:false,//删除弹框
				deletSignId:'',//要删除的SignId
				tableTotal:0,//总条数
				tableList_indexPage:1,//当前页数
				ActivityId:'',//活动ID
				personneAddModel:false,//新增弹框
				modelDate:{//弹框数据
					UserId:'',//会员ID
					SignDate:''//签到时间
				},
				qrCodeUrl:'https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=',//二维码链接
				marketingInformation:{//活动信息
					ActName:'',//活动名称
					StartTime:'',//开始时间
					EndTime:'',//结束时间
					ActStateStr:'',
					ActState:'',//状态
					Integral:'',//积分
					ActivityDesc:'',//活动描述
					//=========
					//marketingtype:'',//营销类型
					area:[],//区域
					ActProvinceName:'',//省
					ActProvinceId:'',//省ID
					ActCityName:'',//市
					ActCityId:'',//市ID
					
					ActivityType:'',//活动类型
					ShareUrl:'',//分享页面
					ActivityCode:'',//活动编码
					ActivityRegion:'',//活动所在大区
				},
				ruleValidate:{//弹框规则
					UserId:[{//会员ID
						required: true,
						message: '请输入会员ID',
						trigger: 'blur'
					}],
					SignDate: [{//签到时间
						required: true,
						type: 'date',
						message: '请选择签到时间', 
//						trigger: 'change'
					}],
				},
				Search:{
					MemberName:'',//姓名
					MemberId:'',//id
				},//查询条件
				signInToView_Data:[
//					{
//						1:'张三',
//						2:'3242134',
//						3:'2018-6-6 16:45:23',
//						4:'是',
//						5:'32'
//					}
				],
				columns:[
					{
						title:'No',
						align: 'center',
						type:'index'
					},
					{
						title:'姓名',
						align: 'center',
						key:'MemberName',
					},
					{
						title:'会员ID',
						align: 'center',
						key:'UserId'
					},
					{
						title:'签到时间',
						align: 'center',
						key:'SignDate'
					},
//					{
//						title:'是否转发',
//						align: 'center',
//						key:'4'
//					},
//					{
//						title:'查阅次数',
//						align: 'center',
//						key:'5'
//					},
					{
						title:'删除记录',
						align: 'center',
						render:(h,params) =>{
							return h('div',[
								h('Button',{
									props: {
                                        type: 'error',
                                        size: 'small'
                                   },
                                  
                                   on:{
                                   	click: () =>{
                                   		this.deleteBtn(params.row)
                                   	}
                                   }
								},'删除')
							])
						}
					},
				],
				exportColumns:[
					{
						title:'No',
						align: 'center',
						type:'index'
					},
					{
						title:'姓名',
						align: 'center',
						key:'MemberName',
					},
					{
						title:'会员ID',
						align: 'center',
						key:'UserId'
					},
					{
						title:'签到时间',
						align: 'center',
						key:'SignDate'
					},
				],
			}
		},
		methods:{
			deleteBtn(name){//删除
//				console.log(name)
				this.SignId = name.SignId
				this.deletModel = true
			},
			modelDelteBtn(){//删除确认
				var self =this
				var url = URLHeader.marketing+'/api/SignIn/RemoveSignLog'
				var DATA = '{"SignId":"'+self.SignId+'"}'
				$.ajax({
					type:"post",
					url:url,
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						//console.log(JSON.parse(json))
						self.getTableData()
						self.$Message.success('删除成功!');
						self.deletModel = false
						console.log(json)
					},
					error:function(error){
						console.log(error)
						self.$Message.error('删除失败');
					}
				});
			},
			manuallyAddBtn(){//手动增加按钮
				this.personneAddModel = true
			},
			personnelAdditionBtn(name){//弹框 新增人员
				var self =this
				this.$refs[name].validate((valid) => {
                    if (valid) {
                    		var modelDate =self.modelDate
//                  		console.log(this.modelDate)
						var url = URLHeader.marketing+'/api/SignIn/SignIn'
						var SignDate = new Date(modelDate.SignDate);  
						var StartTime = SignDate.getFullYear() + '-' + (SignDate.getMonth() + 1) + '-' + SignDate.getDate() + ' ' + SignDate.getHours() + ':' + SignDate.getMinutes() + ':' + SignDate.getSeconds();
						var DATA = '{"ActivityId":'+self.ActivityId+',"UserId":'+modelDate.UserId+',OpenId:"","SignDate":"'+StartTime+'"}'
						console.log(DATA)
						$.ajax({
							type:"post",
							url:url,
							contentType:'application/json; charset=utf-8',
							data:DATA,
							success:function(json){
								//console.log(JSON.parse(json))
//								var dataAll = JSON.parse(json)
								//console.log(json)
								//1 成功  2已经签过到 
								if (json=1) {
									self.getTableData()
									self.personneAddModel = false
									self.$Message.success('新增成功!');
								}else if (json=2) {
									self.personneAddModel = false
									self.$Message.success('已经签过到!');
								}
								
								console.log(json)
							},
							error:function(error){
								console.log(error)
								self.$Message.error('新增失败');
							}
						});


//                      this.$Message.success('Success!');
//                      self.personneAddModel = false
                        self.clearModel()
                        
                    } else {
                        this.$Message.error('Fail!');
                    }
                })
			},
			clearModel(){//清空弹框数据
				this.modelDate = {//弹框数据
					UserId:'',//会员ID
					SignDate:''//签到时间
				}
				this.$refs['modelDate'].resetFields();
			},
			getMarketInForm(){//获取详情 和 二维码
				var self =this
				var url = URLHeader.marketing+'/api/SignActivity/FindSignActivity?activityId='+this.ActivityId
				$('.loding').show()
				$.ajax({
					type:"post",
					url:url,
					contentType:'application/json; charset=utf-8',
					success:function(json){
						var dataAll = JSON.parse(json);
						console.log(dataAll)
						var ActStateStr =''
						if (dataAll.ActState=1) {
							ActStateStr = '启用'
						} else{
							ActStateStr = '禁用'
						}
						self.marketingInformation = {//活动信息
							ActName:dataAll.ActName,//活动名称
							StartTime:dataAll.StartTime,//开始时间
							EndTime:dataAll.EndTime,//结束时间
							ActState:dataAll.ActState.toString(),//状态(1启用,-1禁用)
							ActStateStr:ActStateStr,
							Integral:dataAll.Integral,//积分
							ActivityDesc:dataAll.ActivityDesc,//活动描述
							//=========
							area:[dataAll.ActProvinceId,dataAll.ActCityId],//区域
							ActProvinceName:dataAll.ActProvinceName,//省
							ActProvinceId:dataAll.ActProvinceId,//省ID
							ActCityName:dataAll.ActCityName,//市
							ActCityId:dataAll.ActCityId,//市ID
							
							ActivityType:dataAll.ActivityType,//活动类型
							ShareUrl:dataAll.ShareUrl,//分享页面
							ActivityCode:dataAll.ActivityCode,//活动编码
							ActivityRegion:dataAll.ActivityRegion,//活动所在大区
						}
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('数据获取失败');
					}
				});
				//$('.loding').show()
				//二维码
				var url1 = URLHeader.marketing+'/api/SignIn/CreateCode_Simple?activityId='+this.ActivityId
				$.ajax({
					type:"post",
					url:url1,
					contentType:'application/json; charset=utf-8',
					success:function(json){
						//
						self.qrCodeUrl  = self.qrCodeUrl+json
						//console.log(self.qrCodeUrl)
						//console.log(JSON.parse(json))
						//console.log(json)
						//$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('二维码获取失败');
					}
				});
				
	          
			},
			Upload(){//二维码下载
				var   link = document.createElement('a')
				link.setAttribute("download", "")
				link.href = this.qrCodeUrl
				link.click()
				console.log(this.qrCodeUrl)
				//window.location.href = this.qrCodeUrl
			},
			getTableData(){
				//列表数据
				var self =this
				var url = URLHeader.marketing+'/api/SignIn/GetSignInLog'
				var MemberId = ''
				if (!this.Search.MemberId) {//为空时(后台强烈要求传0)
					//this.Search.MemberId= '0'
					MemberId = '0'
				}else{
					MemberId = this.Search.MemberId
				}
				var DATA = '{"ActivityId":'+self.ActivityId+',"MemberName":"'+self.Search.MemberName+'","MemberId":'+MemberId+',"PageSize":20,"IndexPage":'+self.tableList_indexPage+'}'
				self.signInToView_Data = []
				$('.loding').show()
				$.ajax({
					type:"post",
					url:url,
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						console.log(JSON.parse(json))
						var dataAll = JSON.parse(json);
						self.tableTotal = parseInt(dataAll.Count)
						self.signInToView_Data = dataAll.data
						//console.log(json)
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('签到列表查询失败');
					}
				});
			},
			changgePage(index){
				this.tableList_indexPage = index
				this.getTableData()
			},
			exportCsv(){//订单导出
				var self =this
				 $('.loding').show()
				var DATA = '{"ActivityId":'+self.ActivityId+',"MemberName":"","MemberId":0,"PageSize":9999,"IndexPage":1}'
				//self.signInToView_Data = []
				var url = URLHeader.marketing+'/api/SignIn/GetSignInLog'
				$('.loding').show()
				$.ajax({
					type:"post",
					url:url,
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
//						console.log(JSON.parse(json))
//						var dataAll = JSON.parse(json);
//						self.tableTotal = parseInt(dataAll.Count)
//						self.signInToView_Data = dataAll.data
//						//console.log(json)
//						$('.loding').hide()
						self.$refs.table.exportCsv({
		                    filename: '签到人员',
		                    columns: self.exportColumns,
		                    data: JSON.parse(json).data
		                 });
		                 $('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('签到列表查询失败');
					}
				});
				
				
			},
		},
		mounted:function(){
			this.ActivityId = this.$route.query.ActivityId
			this.getMarketInForm()//头部信息
			this.getTableData()//列表数据
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
.signInToView{
	border: 1px solid gainsboro;
	overflow: auto;
	border-radius: 8px;
	padding: 1rem;
	
}
.signInToView .signInToView_Top{
	overflow: auto;
	border-bottom:1px dashed gainsboro ;
}
.signInToView .signInToView_Top .li{
	height: 2.5rem;
	line-height: 2.5rem;
}
.signInToView .signInToView_Top .li .lable{
	display: block;
	float: left;
	width: 5.5rem;
	font-size: 14px;
}
.signInToView .signInToView_Top .li .value{
	float: left;
	display: block;
	margin-left: 2rem;
	font-size: 14px;
}
.formitem{
	margin-left: 1rem;
}
.signInToView .signInToView_body{
	margin-top: 0rem;
}
</style>