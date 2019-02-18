<template>
	<div class="IntegralrRuleList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="IntegralrRuleListTop">
			<ul class="TopUl" style="width:70rem;">
				<li class="TopLi">
					<span>有效时间</span>
					<DatePicker  v-model="searchCriteria.dataTime" type="daterange" placement="bottom-end" placeholder="请选择有效时间" style="width: 180px;margin-left: 0.5rem;"></DatePicker>
				</li>
				<li class="TopLi">
					<span>规则状态</span>
					<Select v-model="searchCriteria.RuleState" style="width:80px;position: relative;margin-left: 0.5rem;" placeholder = '规则状态'>
				        <Option v-for="item in RuleStateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>商品名称</span>
					<Input v-model="searchCriteria.GoodsName" placeholder="商品名称" style="width: 180px;margin-left: 0.5rem;"></Input>
				</li>
				<li class="TopLi">
					<span>规则名称</span>
					<Input v-model="searchCriteria.RuleName" placeholder="规则名称" style="width: 180px;margin-left: 0.5rem;"></Input>
				</li>
				<li class="TopLi">
					<Button type="primary" icon="ios-search" @click ='searchRule'>搜索</Button>
				</li>
				<li class="TopLi">
					<Button type="primary" @click="addRuleBtn">添加</Button>
				</li>
				<!--<li>
					<a href="http://jifen.shinho.net.cn/Upload/QrCode/912c8aaa-0d6f-48c0-b9e2-13b8cca7cfcf.xls">下载</a>
				</li>-->
			</ul>
		</div>
		<div class="body">
			<Modal
		        :title="ModalTile"
		        v-model="modelShow"
		        width='700'
		        :mask-closable="false">
		       	<ul class="Modal_Ul" v-show="modelData.ISselectGoods" style="margin: 0 auto;width: 30rem;">
		       		<Form ref="modelData" :model="modelData" :rules="ruleValidate" :label-width="100" label-position	='left' >
		       			<li class="Modal_li">
		       				<FormItem label="规则名称" prop="RuleName" class='formitem'>
					            <Input v-model="modelData.RuleName" style="width: 200px;margin-left: 0.5rem;"  placeholder="规则名称"></Input>
					        </FormItem>
			       			<!--<span>规则名称</span>
			       			<Input v-model="modelData.RuleName" placeholder="规则名称" style="width: 230px;margin-left: 0.5rem;"></Input>-->
			       		</li>
			       		<li class="Modal_li">
			       			<FormItem label="商品名称" prop="GoodsName" class='formitem'>
					            <Input v-model="modelData.GoodsName" disabled placeholder="商品名字" style="width: 200px;margin-left: 0.5rem;"></Input>
					            <Button type="primary" @click="selectGoodsBtn">选择商品</Button>
					        </FormItem>
			       			<!--<span>商品名称</span>
			       			<Input v-model="modelData.GoodsName" disabled placeholder="商品名字" style="width: 230px;margin-left: 0.5rem;"></Input>-->
			       			 <img :src='modelData.GoodsImageUrl' style="display: block;margin-top: 1rem;margin-left: 10rem;width: 10rem;"/>
			       		</li>
			       		<li class="Modal_li">
			       			<!--<span>有效时间</span>-->
		       				<FormItem label="开始时间" prop="StartDate" class='formitem'>
					            <DatePicker v-model="modelData.StartDate" type="datetime" placeholder="开始时间" style="width: 200px;"></DatePicker>
					        </FormItem>
					        <FormItem label="结束时间" prop="EndDate" class='formitem'>
					            <DatePicker v-model="modelData.EndDate" type="datetime" placeholder="结束时间" style="width: 200px"></DatePicker>
					        </FormItem>			       			
			       		</li>
			       		<li class="Modal_li">
			       			<FormItem label="积分" prop="Integral" class='formitem'>
					            <Input v-model="modelData.Integral" placeholder="积分" style="width: 200px;"></Input>
					        </FormItem>
			       			<!--<span>积分</span>-->
			       			<!--<Input v-model="modelData.Integral" placeholder="积分" style="width: 230px;margin-left: 0.5rem;"></Input>-->
			       		</li>
			       		
			       		<li class="Modal_li">
			       			<span>规则状态</span>
			       			<span style="margin-left: 0.8rem;">{{modelData.RuleStateStr}}</span>
			       		</li>
			       		<li class="Modal_li">
			       			<FormItem label="规则类型" prop="RuleType" class='formitem'>
					        		<Select v-model="modelData.RuleType" :disabled ='modelData.disabled' style="width:200px;position: relative;" placeholder = '规则类型' @on-change='RuleTypeChange'>
						        		<Option v-for="item in modalRuleTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
						   	 	</Select>
					        </FormItem>
			       			
			       			<!--<span>规则类型</span>
			       			<Select v-model="modelData.RuleType" :disabled ='modelData.disabled' style="width:230px;position: relative;margin-left: 0.5rem;" placeholder = '规则类型' @on-change='RuleTypeChange'>
						        <Option v-for="item in modalRuleTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
						    </Select>-->
			       		</li>
			       		<li class="Modal_li ScanLimit">
			       			<span>扫码限制</span>
			       			<Input v-model="modelData.ScanLimit"  placeholder="扫码限制" style="width: 200px;"></Input>
			       		</li>
			       		<li class="Modal_li ScanLimit" style="overflow: auto;">
			       			<span style="float: left;">概率分布</span>
			       			<table style="float: left;">
							  <tr>
							    <th>No.</th>
							    <th>积分值</th>
							    <th>投放数量</th>
							    <th>扫中概率</th>
							     <th v-if="!modelData.disabled">
							     	<Button type="primary" @click="addProbabilityBtn" shape="circle" icon="plus" size="small" style="margin-left: 1rem;"></Button>
							     </th>
							  </tr>
							  	<tr v-for = '(item,index) in probabilityList'>
								    <td>
								    		{{index+1}}
								    </td>
								    <td>
								    		<InputNumber  v-model="item.Integral" class='InputNumber'></InputNumber>
								    		<!--<Input v-model='item.Integral'  placeholder="积分值" style="width: 80px;"></Input>-->
								    </td>
								    <td>
								    		<InputNumber  v-model="item.PutNum" class='InputNumber'></InputNumber>
								    		<!--<Input v-model='item.PutNum' placeholder="投放数量" style="width: 80px;"></Input>-->
								    </td>
								    <td>
								    		<InputNumber  v-model="item.Chance" class='InputNumber'></InputNumber>
								    		<!--<Input v-model='item.Chance'  placeholder="扫中概率" style="width: 80px;"></Input>-->
								    </td>
								    <td v-if="!modelData.disabled">
								    		<Button type="primary" @click="deleteProbabilityBtn(item)" shape="circle" icon="minus-round" size="small" style="margin-left: 1rem;"></Button>
								    </td>
								</tr>
							  
							</table>
			       		</li>
			       		
			       		<li class="Modal_li">
			       			<FormItem label="备注"  class='formitem'>
					            	<Input v-model="modelData.Remark"   type="textarea" :autosize="{minRows: 4,maxRows: 8}" style="width: 17rem;float: left;" placeholder="请输入备注内容"></Input>
					        </FormItem>
			       			<!--<span style="display: block;float: left;">备注</span>-->
<!--			       			<Input v-model="modelData.Remark"   type="textarea" :autosize="{minRows: 4,maxRows: 8}" style="width: 17rem;margin-left: 0.8rem;float: left;" placeholder="请输入备注内容"></Input>
-->			       		</li>
		       		</Form>
		       		
		       	</ul>
		       	<div v-show="modelData.selectShow" style="overflow: auto;">
		       		<ul class="TopUl">
						<li class="TopLi">
							<span>商品状态</span>
							<Select v-model="modelData.State" style="width:100px;position: relative;margin-left: 0.5rem;" placeholder = '商品状态'>
						        <Option v-for="item in modalStateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
						    </Select>
						</li>
						<li class="TopLi">
							<span>商品名字</span>
							<Input v-model="modelData.searchGoodsName" placeholder="商品名字" style="width: 100px;margin-left: 0.5rem;"></Input>
						</li>
						<li class="TopLi">
							<Button type="primary" icon="ios-search" @click ='searchGoods'>搜索</Button>
						</li>
					</ul>
		       		
		       		<Table :columns="modelColumns" :data="modelList" ></Table>
		       		<Button type="primary" size="large"   @click="GoodsSaveBtn" style="margin-left: 40%;margin-top: 1rem;">保存</Button>
		           <!-- <Button type="primary" size="large"   @click="GoodsCancelBtn" style="margin-top: 1rem;">返回</Button>-->
		       		<Page :total="modelData.total" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;' @on-change='ModalchangePage'></Page>
		       	</div>
		       	<div slot="footer" v-show="modelData.ISselectGoods">
		            <Button type="primary" size="large"   @click="ruleModalOk('modelData')">确定</Button>
		            <Button type="primary" size="large"   @click="ruleModalCancel">取消</Button>
		        </div>
		    </Modal>
		    
		    <Modal v-model="modalDeletShow" width="360">
		        <p slot="header" style="color:#f60;text-align:center">
		            <Icon type="information-circled"></Icon>
		            <span>删除确认</span>
		        </p>
		        <div style="text-align:center">
		            <p>您确定要删除此规则吗?</p>
		            <p></p>
		        </div>
		        <div slot="footer">
		            <Button type="error" size="large" long  @click="deletRule">删除</Button>
		        </div>
		    </Modal>
			<Table :columns="RuleColumns" :data="RuleList" ></Table>
			
			<Page :total="total" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>
<script>

	export default{
		data(){
			return{
				ruleValidate:{
					RuleName:[{//规则名称
						required: true,
						message: '请输入规则名称',
//						trigger: 'blur'
					}],
					GoodsName:[{//商品名称
						required: true,
						message: '请输入商品名称',
					}],
					StartDate: [{//开始时间
						required: true,
						type: 'date',
						message: '请选择开始时间', 
						trigger: 'change'
					}],
					EndDate: [{//结束时间
						required: true,
						type: 'date',
						message: '请选择结束时间', 
						trigger: 'change'
					}],
					Integral:[{//积分
						required: true,
						message: '请输入积分',
//						trigger: 'blur'
					}],
					RuleType:[{ //规则类型
						required: true,
						message: '请选择规则类型',
//						trigger: 'change'
					}],
				},
				modalDeletShow:false,//删除确认
				modalDeletID:'',
				ISAddState:true,//新增模式
				ModalTile:'普通规则新增',
				total:0,//page总条数
				searchCriteria:{//搜索条件
					RuleName:'',//规则名称
					GoodsName:'',//商品名称
					RuleState:0,//规则状态
					dataTime:[],//有效时间
				},
				modelShow:false,//积分商品显示 true
				modelData:{//弹框数据
					ISselectGoods:true,//弹框底部和ul
					selectShow:false,//选择商品界面
					total:0,//商品总条数
					State:0,//商品状态
					searchGoodsName:'',//查询商品名字
					
					
					RuleId:'',//规则id  (只适用于编辑)
					RuleName:'',//规则名称
					GoodsId:'',//商品 id
					GoodsName:'',//商品名称
					GoodsImageUrl:'../../../../../static/image/placeholderBag.png',//商品图片
					Integral:'',//积分
					StartDate:'',//开始时间
					EndDate:'',//结束时间
					RuleState:1,//规则状态 新增默认为1有效
					RuleStateStr:'有效',//字符规则状态
					CreateDate:'',//创建时间
					Remark:'',//备注
					RuleType:1,//规则类型
					disabled:false,//规则类型可选
					ScanLimit:'',//扫码限制
				},
				probabilityList:[//概率
					{	
						RuleId:0,
						Integral:null,
						PutNum:null,
						Chance:null,
					},
					
				],
				modalStateList:[//弹框  商品状态  数据
					{
                        value: 0,
                        label: '全部'
                    },
                    {
                        value: 1,
                        label: '上架'
                    },
                    {
                        value: 2,
                        label: '下架'
                    },
                     {
                        value: 100,
                        label: '已删除'
                    },
				],
				modalRuleTypeList:[//弹框  撒号那个品状态
					{
                        value: 1,
                        label: '普通规则'
                    },
                    {
                        value: 2,
                        label: '活动规则'
                    },
				],
				RuleStateList:[//商品状态列表
					{
                        value: 1,
                        label: '有效'
                    },
                    {
                        value: 2,
                        label: '无效'
                    },
                     {
                        value: 0,
                        label: '全部'
                    },
				],
				modelList:[],
				modelColumns:[//model表头
					{
						title:'选择',
						width:60,
						align: 'center',
						render: (h, params) => {
                            return h('div', [
                                h('Radio', {
                                    props: {
                                    	type:'person',
                                    	value: params.row.select,
                                    },
                                    style: {
										width:'2rem',
										
                                    },
                                     on: {
	                                	'on-change':()=>{
	                                		this.RadioChange(params)	
	                                	},
	                                }
                                },
                               
                                ),
                            ]);
                     	}
					},
					{
						title:'商品编码',
						width:90,
						align: 'center',
						key:'GoodsNo'
					},
					{
						title:'商品内部编码',
						align: 'center',
						key:'GoodsInterNo',
					},
					{
						title:'商品名称',
						align: 'center',
						key:'GoodsName',
					},
					{
						title:'价格（元)',
						align: 'center',
						key:'GoodsPrice',
					},
//					{
//						title:'积分总额',
//						align: 'center',
//						key:'OrderTelephone',
//						width:110,
//					},
					{
						title:'商品状态',
						align: 'center',
						key:'GoodsStateStr',
					},
					{
						title:'添加时间',
						align: 'center',
						width:110,
						key:'CreateDate',
					},			
				],
				RuleList:[//表格数据
				],
				RuleColumns:[//商品;列表表头
					{
						title:'规则名称',
						align: 'center',
						key:'RuleName'
					},
					{
						title:'编码',
						align: 'center',
						width:80,
						key:'GoodsNo',
					},
					{
						title:'内部编码',
						align: 'center',
						width:110,
						key:'GoodsInterNo',
					},
					{
						title:'商品名称',
						align: 'center',
						key:'GoodsName',
					},
					{
						title:'积分',
						align: 'center',
						width:90,
						key:'Integral',
					},
					{
						title:'类型',
						align: 'center',
						width:90,
						key:'RuleTypeStr',//1 普通规则
					},
					{
						title:'状态',
						align: 'center',
						width:80,
						key:'RuleStateStr',//1 有效
					},
					{
						title:'有效时间',
						align: 'center',
						width:160,
						key:'dateTime',
					},
					{
						title:'添加时间',
						align: 'center',
						width:110,
						key:'CreateDate',
					},
					{
						title:'操作',
						width:150,
						align: 'center',
						render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    style: {
										width:'3rem'
                                    },
                                    on: {
                                        click: () => {
                                            this.editRule(params)
                                        }
                                    }
                                }, '编辑'),
                                 h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                    style: {
										width:'3rem',
										marginLeft:'0.8rem'
                                    },
                                    on: {
                                        click: () => {
                                            this.ruleDelet(params)
                                        }
                                    }
                                }, '删除'),
                               
                            ]);
                        }
					},
				],
			}
		},
		methods:{
			RuleTypeChange(name){//规则 下拉触发事件
				if (name==1) {//普通规则
					$('.ScanLimit').hide()
				} else{//活动规则
					$('.ScanLimit').show()
				}
				console.log(name)
			},
			deleteProbabilityBtn(name){//概率删除
				this.probabilityList.splice(this.probabilityList.indexOf(name), 1)
			},
			addProbabilityBtn(){//概率新增
				var item = {	
						RuleId:0,
						Integral:null,
						PutNum:null,
						Chance:null,
					}
				this.probabilityList.push(item)
			},
			RadioChange(name){//商品单选  触发
				var self =this
				var index = name.index
				this.modelList.map((item,i)=>{
					if (i==index) {
						item.select = true
						self.modelData.GoodsId =name.row.GoodsId
						self.modelData.GoodsName =name.row.GoodsName
						self.modelData.GoodsImageUrl =name.row.GoodsImageUrl
						console.log(name)
					} else{
						item.select = false
					}
				})	
			},
			selectGoodsBtn(){//弹框中  选择商品
					this.modelData.ISselectGoods = false//弹框底部和ul
					this.modelData.selectShow = true//选择商品界面
					this.searchGoods()//商品数据获取
					console.log('切换')
			},
			ruleDelet(name){//删除按钮
				this.modalDeletID = name.row.RuleId
				this.modalDeletShow=true
			},
			deletRule(){//删除确认  弹框
				var self =this
				console.log(this.modalDeletID)
				this.modalDeletShow=false
				if (self.modalDeletID)  {
					$.ajax({
				        type :"get",
				        url :URLHeader.Integral+'/api/IntegralRule/RemoveIntegralRule?ruleId='+this.modalDeletID,///cts/core/
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        success : function(json) {
				        	  console.log(json)
				        	  self.$Message.success('数据删除成功');
				          self.requestTableData(1)
				          self.modalDeletID = ''
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		   });
				} else{
					self.$Message.error('此商品缺少goodsId');
				}
					
			},
			editRule(name){//商品编辑
				console.log(name.row)
				this.cleanModelData()
				var self =this
				this.ModalTile = '普通规则编辑'
				this.ISAddState =false//编辑模式
				var item = name.row
				if (item.RuleType==2) {//活动规则
					$.ajax({
				        type :"post",
				        url :URLHeader.Integral+'/api/IntegralRule/GetRuleInfo?ruleId='+name.row.RuleId,///cts/core/
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        success : function(json) {
				        	  	var dataAll =JSON.parse(json)
				        	  	//console.log(dataAll)
				        	  	self.probabilityList = dataAll
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		    });
				}
				this.modelData.RuleId=item.RuleId
				this.modelData.RuleName= item.RuleName
				this.modelData.GoodsId= item.GoodsId
				this.modelData.GoodsName= item.GoodsName
				this.modelData.GoodsImageUrl= item.GoodsImageUrl
				this.modelData.Integral= item.Integral
//				this.modelData.StartDate = item.StartDate
//				console.log(this.modelData.StartDate)
				this.modelData.StartDate = new Date(item.StartDate)
				this.modelData.EndDate =	new Date(item.EndDate)
				this.modelData.CreateDate= item.CreateDate
				this.modelData.RuleState= item.RuleState//默认新增
				if (this.modelData.RuleState==1) {//有效
					this.modelData.RuleStateStr = '有效'
				} else{
					this.modelData.RuleStateStr = '无效有效'
				}
				this.modelData.Remark= item.Remark
				this.modelData.ScanLimit= item.ScanLimit
				if (item.ScanLimit==0) {
					this.modelData.ScanLimit= ''
				} else{
					this.modelData.ScanLimit= item.ScanLimit
				}
				this.modelData.disabled =true//规则类型不可选
				this.modelData.RuleType= item.RuleType
				if (this.modelData.RuleType==1) {//普通规则
					$('.ScanLimit').hide()
				} else{//活动规则
					$('.ScanLimit').show()
				}
				this.modelShow = true
				
			},
			addRuleBtn(){//
				 this.cleanModelData()//清除弹框数据
//				this.ModalTile = '积分商品新增'
				this.ModalTile = '普通规则新增'
				this.ISAddState =true//新增模式
				this.modelShow =true
			},
			p(s) {
				return s < 10 ? '0' + s: s;
			},
			ruleModalCancel(){
				this.modelShow = false
			},
			ruleModalOk(name){//商品新增或编辑Ok按钮				
				var self = this
				var error =false//标志概率是否完整
				if (self.modelData.RuleType==2) {
					self.probabilityList.map((item,index) =>{
						if (!item.Integral || !item.PutNum || !item.Chance) {
							error = true
						}
						if (index ==self.probabilityList.length-1) {//概率值遍历
							
							if (!self.modelData.ScanLimit) {
								error = true
//								console.log('不完整')
							}
							console.log(error)
						}
					})
					
				}
				
				
				this.$refs[name].validate((valid) => {//弹框数据(不包含概率)
                    if (valid && !error) {//概率填写完整  其他信息完整
	                    	if (self.ISAddState) {//新增模式
							self.AddGoodsInformation()
						} else{//编辑模式
							console.log(self.modelData)
							self.EideGoodsInformation()
						}
						console.log('完整')
						this.modelShow = false
                    } else {
                        this.$Message.error('请填完整数据!');
                        this.$refs['modelData'].resetFields();
                    }
                })   
				
				
				
				
				
				//console.log(self.ISAddState)
//				console.log(self.modelData.RuleType)
//				var error =false
//				this.probabilityList.map((item) =>{
//					if (!item.RuleId || !item.Integral ||!item.PutNum ||!item.Chance) {
//						error = true
//					}
//				})
//				
//							
//				if (self.modelData.RuleName && self.modelData.GoodsId && self.modelData.Integral && self.modelData.StartDate && self.modelData.EndDate && self.modelData.RuleState && self.modelData.RuleType) {//商品编号 商品内部编码 商品名称 价格  商品状态
//					
//					if (self.modelData.RuleType==1) {//普通规则
//						console.log('普通')
//					} else if (self.modelData.RuleType==2 && !error) {
//						console.log('活动')
//					}else{
//						console.log('错误')
//					}
//					
//					
//					
////					if (self.modelData.RuleType==2&&self.modelData.ScanLimit || self.modelData.RuleType==1) {
////						if (self.ISAddState) {//新增模式
////							self.AddGoodsInformation()
////						} else{//编辑模式
////							self.EideGoodsInformation()
////						}
////					} else{
////						self.$Message.warning('请将信息填写完整');
////					}	
//				} else{
//					self.$Message.warning('请将信息填写完整');
//				}	
//				
			},
			AddGoodsInformation(){//商品新增
				var self =this
				//创建时间
				var myDate = new Date();
				//获取当前年
				var year=myDate.getFullYear();
				//获取当前月
				var month=myDate.getMonth()+1;
				//获取当前日
				var date=myDate.getDate(); 
				var h=myDate.getHours();       //获取当前小时数(0-23)
				var m=myDate.getMinutes();     //获取当前分钟数(0-59)
				var s=myDate.getSeconds();  
				self.modelData.CreateDate=year+'-'+this.p(month)+"-"+this.p(date)+" "+this.p(h)+':'+this.p(m)+":"+this.p(s);
				//开始时间与结束时间
				var one =self.modelData.StartDate
				var two = self.modelData.EndDate
				self.modelData.StartDate = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()+ ' ' + one.getHours() + ':' + one.getMinutes() + ':' + one.getSeconds()
				self.modelData.EndDate = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()+' ' + two.getHours() + ':' + two.getMinutes() + ':' + two.getSeconds()
				//当 普通规则时
				if (self.modelData.RuleType==1) {//普通规则
					self.modelData.ScanLimit = '0'
				}else{//概率规则
					
				}
				var arr =  JSON.stringify(this.probabilityList)
				var DATA = '{"RuleName":"'+self.modelData.RuleName+'","GoodsId":'+self.modelData.GoodsId+',"Integral":'+self.modelData.Integral+',"StartDate":"'+self.modelData.StartDate+'","EndDate":"'+self.modelData.EndDate+'","RuleState":1,"CreateDate":"'+self.modelData.CreateDate+'","Remark":"'+self.modelData.Remark+'","RuleType":'+self.modelData.RuleType+',"ScanLimit":'+self.modelData.ScanLimit+',"ChanceList":'+arr+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralRule/AddIntegralRule',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  console.log(json)
			        	  self.$Message.success('数据保存成功');
			        	  self.cleanModelData()//清除弹框数据
			        	  self.requestTableData(1)//表格数据刷新
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			EideGoodsInformation(){//商品编辑
				//api/IntegralGoods/UpdateIntegralGoods
				var self =this
				//开始时间与结束时间
				var one =self.modelData.StartDate
				var two = self.modelData.EndDate
				self.modelData.StartDate = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()+ ' ' + one.getHours() + ':' + one.getMinutes() + ':' + one.getSeconds()
				self.modelData.EndDate = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()+' ' + two.getHours() + ':' + two.getMinutes() + ':' + two.getSeconds()
				
//				if (one.toString().indexOf("GMT")>=0) {//包含
//					self.modelData.StartDate = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()+ ' ' + one.getHours() + ':' + one.getMinutes() + ':' + one.getSeconds()
//				}
//				if (two.toString().indexOf("GMT")>=0) {
//					self.modelData.EndDate = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()+' ' + two.getHours() + ':' + two.getMinutes() + ':' + two.getSeconds()
//				}
				console.log(self.modelData.StartDate)
				//当 普通规则时
				if (self.modelData.RuleType==1) {
					self.modelData.ScanLimit = '0'
				}
				var arr =  JSON.stringify(this.probabilityList)
				var DATA = '{"RuleId":'+self.modelData.RuleId+',"RuleName":"'+self.modelData.RuleName+'","GoodsId":'+self.modelData.GoodsId+',"Integral":'+self.modelData.Integral+',"StartDate":"'+self.modelData.StartDate+'","EndDate":"'+self.modelData.EndDate+'","RuleState":1,"CreateDate":"'+self.modelData.CreateDate+'","Remark":"'+self.modelData.Remark+'","RuleType":'+self.modelData.RuleType+',"ScanLimit":'+self.modelData.ScanLimit+',"ChanceList":'+arr+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralRule/UpdateIntegralRule',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  console.log(json)
			        	  self.$Message.success('数据编辑成功');
			        	  self.cleanModelData()//清除弹框数据
			        	  self.requestTableData(1)//表格数据刷新
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			
			},
			cleanModelData(){//清除弹框数据
				this.modelData.ISselectGoods = true//填写信息ul
				this.modelData.selectShow = false//选择商品ul
				this.searchGoods()//选择商品 数据源重新加载
				this.modelData.RuleId=''
				this.modelData.RuleName=''
				this.modelData.GoodsId=''
				this.modelData.GoodsName=''
				this.modelData.GoodsImageUrl='../../../../../static/image/placeholderBag.png'
				this.modelData.Integral=''
				this.modelData.StartDate=''
				this.modelData.EndDate=''
				this.modelData.RuleState='1'//默认新增
				this.modelData.Remark=''
				this.modelData.ScanLimit=''
				this.modelData.RuleType=1
				this.modelData.disabled =false//规则类型可选
				this.probabilityList = [//概率
					{	
						RuleId:0,
						Integral:null,
						PutNum:null,
						Chance:null,
						
					}
				]
				this.$refs['modelData'].resetFields();
				$('.ScanLimit').hide()
			},
			searchRule(){//搜索按钮
				this.requestTableData(1)
				console.log(this.searchCriteria)
				console.log('搜索开始')
			},
			changePage(index){//页面切换
				console.log('当前页码'+index)
				this.requestTableData(index)
			},
			requestTableData(index){//初始数据请求  规则列表
				var self = this
				self.RuleList = []
				$('.loding').show()
				var StartTime=''//开始时间
				var EndTime = ''//结束时间
				if (self.searchCriteria.dataTime.length>0) {
					var one = new Date(self.searchCriteria.dataTime[0]); 
					var two = new Date(self.searchCriteria.dataTime[1]);
					 StartTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 EndTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
				}else{
					StartTime= ''
					EndTime = ''
				}

				var DATA = '{"GoodsName":"'+self.searchCriteria.GoodsName+'","RuleState":'+self.searchCriteria.RuleState+',"RuleName":"'+self.searchCriteria.RuleName+'","StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","PageSize":20,"IndexPage":'+index+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralRule/GetRuleList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  var dataAll =JSON.parse(json)
			        	  self.total = parseInt(dataAll.Count)	
			        	  for (var i= 0 ;i<dataAll.data.length;i++) {
			        	  	if (dataAll.data[i].RuleType==1) {
			        	  		dataAll.data[i].RuleTypeStr='普通规则'
			        	  	}else{
			        	  		dataAll.data[i].RuleTypeStr='活动规则'
			        	  	}
			        	  	if (dataAll.data[i].RuleState==1) {
			        	  		dataAll.data[i].RuleStateStr='有效'
			        	  	}else{
			        	  		dataAll.data[i].RuleStateStr='无效'
			        	  	}
			        	  	//有效时间拼接
			        	  	dataAll.data[i].dateTime = dataAll.data[i].StartDate+'-'+dataAll.data[i].EndDate
			        	  	$('.loding').hide()
			        	  }
			        	  self.RuleList = dataAll.data
			        	  console.log(JSON.parse(json))
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			//==============================商品  函数处理
			searchGoods(){
				this.requestModalTable(1)
			},
			ModalchangePage(index){
				this.requestModalTable(index)
			},
			requestModalTable(index){
				var self = this
				self.modelList = []
				var DATA = '{"GoodsName":"'+self.modelData.searchGoodsName+'","State":'+self.modelData.State+',"PageSize":10,"IndexPage":'+index+'}'
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralGoods/GetIntegralGoodsList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
			        	  self.modelData.total = parseInt(dataAll.Count)
			        	  for (var i = 0;i<dataAll.data.length;i++) {
			        	  	if (dataAll.data[i].GoodsState==1) {//上架
			        	  		dataAll.data[i].GoodsStateStr='上架'
			        	  	} else if (dataAll.data[i].GoodsState==2) {//下架
			        	  		dataAll.data[i].GoodsStateStr='下架'
			        	  	} else if (dataAll.data[i].GoodsState==100) {
			        	  		dataAll.data[i].GoodsStateStr='已删除'
			        	  	}
			        	  	dataAll.data[i].select=false
			        	  }			        	  
			        	  
			        	  self.modelList = dataAll.data
			        	 // console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			GoodsSaveBtn(){//弹框商品   保存
				this.modelData.ISselectGoods = true//弹框底部和ul
				this.modelData.selectShow = false//选择商品界面
			},
		},
		mounted:function(){
			this.requestTableData(1)
			$('.ScanLimit').hide()
		}
	}
</script>
<style>
	/*html, body { min-width: 1300px; }*/
</style>
<style scoped>
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
@keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }
.IntegralrRuleList {
	margin: 0 auto;
	padding: 0
}
.IntegralrRuleList .IntegralrRuleListTop{
	border-bottom: 1px solid gainsboro;
	
}
.IntegralrRuleList .IntegralrRuleListTop .TopUl{
	height: 2.9rem;
	margin-left: 1rem;
}
.IntegralrRuleList .IntegralrRuleListTop .TopUl .TopLi {
	display: block;
	float: left;
	margin-left: 1rem;
	height: 2rem;
	line-height: 2rem;
	
}
.formitem{
	margin-left: 2rem;
}
.Modal_Ul{
	overflow: auto;
}
.Modal_Ul .Modal_li{
	margin-bottom: 0.6rem;
	/*overflow: auto;*/
	line-height: 2rem;
}
.Modal_Ul .Modal_li span{
	width: 4.5rem;
	display: inline-block;
	margin-left: 2rem;
	margin-right: 2rem;
}
.ScanLimit table tr{
	text-align: center;
}
.TopUl{
	height: 2.9rem;
	margin-left: 2rem;
}
.TopUl .TopLi {
	display: block;
	float: left;
	margin-left: 1rem;
	height: 2rem;
	line-height: 2rem;
	
}
</style>