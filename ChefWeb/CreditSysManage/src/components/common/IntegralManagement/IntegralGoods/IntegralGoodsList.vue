<template>
	<div class="IntegralGoodsList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="IntegralGoodsListTop">
			<ul class="TopUl">
				<li class="TopLi">
					<span>商品状态</span>
					<Select v-model="searchCriteria.State" style="width:100px;position: relative;margin-left: 0.5rem;" placeholder = '商品状态'>
				        <Option v-for="item in GoodsStateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>商品名字</span>
					<Input v-model="searchCriteria.GoodsName" placeholder="商品名字" style="width: 100px;margin-left: 0.5rem;"></Input>
				</li>
				<li class="TopLi">
					<Button type="primary" icon="ios-search" @click ='searchGoods'>搜索</Button>
				</li>
				<li class="TopLi">
					<Button type="primary" @click="addGoodsBtn">添加</Button>
				</li>
				
			</ul>
		</div>
		<div class="body">
			<Modal
		        :title="ModalTile"
		        v-model="modelShow"
		        @on-ok="GoodsModalOk"
		        :mask-closable="false">
		       	<ul class="Modal_Ul">
		       		<li class="Modal_li">
		       			<span>商品名称</span>
		       			<Input v-model="modelData.GoodsName" placeholder="商品名字" style="width: 230px;margin-left: 0.5rem;"></Input>
		       		</li>
		       		<li class="Modal_li">
		       			<span>商品编号</span>
		       			<Input v-model="modelData.GoodsNo" :disabled='modelData.Modaldisabled' placeholder="商品编号" style="width: 230px;margin-left: 0.5rem;"></Input>
		       		</li>
		       		<li class="Modal_li">
		       			<span>商品内部编码</span>
		       			<Input v-model="modelData.GoodsInterNo" placeholder="商品内部编码" style="width: 230px;margin-left: 0.5rem;"></Input>
		       		</li>
		       		<li class="Modal_li">
		       			<span>价格(元)</span>
		       			<Input v-model="modelData.GoodsPrice" placeholder="商品价格" style="width: 230px;margin-left: 0.5rem;"></Input>
		       		</li>
		       		<li class="Modal_li">
		       			<span>商品状态</span>
		       			<Select v-model="modelData.GoodsState" style="width:230px;position: relative;margin-left: 0.5rem;" placeholder = '商品状态' class="GoodsState">
					        <Option v-for="item in modalStateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
		       		</li>
		       		<li class="Modal_li" style="overflow: auto;">
		       			<span style="display: block;float: left;line-height: 5rem;">商品图片</span>
		       			<div style="display: block;float: left;margin-left: 0.9rem;">
				        		 <img :src="modelData.GoodsImageUrl" style="height: 5rem;margin-left:1;"/>
				        		 <Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="GoodsImgSuccess"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImage
						        style="display:block;width:80px;margin: 0 auto;" >
						         <Button type="primary">选择商品</Button>
						    </Upload>
				        </div>
		       		</li>
		       		<li class="Modal_li">
		       			<span style="display: block;float: left;">备注</span>
		       			<Input v-model="modelData.Remark"   type="textarea" :autosize="{minRows: 4,maxRows: 8}" style="display: block;float: left;width: 13rem;margin-left: 1rem;" placeholder="请输入备注内容"></Input>
		       		</li>
		       	</ul>
		    </Modal>
		    <Modal v-model="modalDeletShow" width="360">
		        <p slot="header" style="color:#f60;text-align:center">
		            <Icon type="information-circled"></Icon>
		            <span>删除确认</span>
		        </p>
		        <div style="text-align:center">
		            <p>您确定要删除此商品吗?</p>
		            <p></p>
		        </div>
		        <div slot="footer">
		            <Button type="error" size="large" long  @click="deletGoods">删除</Button>
		        </div>
		    </Modal>
			<Table :columns="GoodsColumns" :data="GoodsList" ></Table>
			
			<Page :total="total" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>
<script>

	export default{
		data(){
			return{
				UPImage:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Product',//上传插件地址
				modalDeletShow:false,//删除确认
				modalDeletID:'',
				ISAddState:true,//新增模式
				ModalTile:'积分商品新增',
				modelShow:false,//积分商品显示 true
				total:2,//page总条数
				searchCriteria:{//搜索条件
					State:0,//商品状态
					GoodsName:'',//商品名字
				},
				modelData:{//弹框数据
					GoodsName:'',//商品名称
					GoodsNo:'',//商品编号
					Modaldisabled:false,//商品编号非禁用
					GoodsInterNo:'',//商品内部编码
					GoodsPrice:'',//价格
					GoodsState:null,//商品状态
					GoodsImageUrl:'../../../../../static/image/placeholderBag.png',//图片照片
					Remark:'',//备注
					CreateDate:'',//上传时间
					GoodsId:'',//商品ID  编辑时用
					
				},
				modalStateList:[//弹框  撒号那个品状态
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
				GoodsStateList:[//商品状态列表
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
				GoodsList:[//表格数据
				],
				GoodsColumns:[//商品;列表表头
					{
						title:'商品编号',
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
					{
						title:'操作',
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
                                            this.editGoods(params)
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
                                            this.orderGoodsDelet(params)
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
			orderGoodsDelet(name){//删除按钮
				this.modalDeletID= name.row.GoodsId
				this.modalDeletShow=true
			},
			deletGoods(){//删除确认  弹框
				var self =this
				console.log(this.modalDeletID)
				this.modalDeletShow=false
				if (self.modalDeletID)  {
					$.ajax({
				        type :"get",
				        url :URLHeader.Integral+'/api/IntegralGoods/RemoveIngegralGoods?goodsId='+this.modalDeletID,
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
			editGoods(name){//商品编辑
				this.cleanModelData()//弹框数据清空
				this.ModalTile = '积分商品编辑'
				this.ISAddState= false//编辑模式
				this.modelShow =true
				var item = name.row
				this.modelData.GoodsNo = item.GoodsNo
				this.modelData.Modaldisabled=true
				this.modelData.GoodsInterNo = item.GoodsInterNo
				this.modelData.GoodsName = item.GoodsName
				this.modelData.GoodsPrice = item.GoodsPrice
//				if (item.GoodsState==100) {
//					this.modelData.GoodsState =100
//				}else{
					this.modelData.GoodsState =parseInt(item.GoodsState)
//				}
				
				this.modelData.Remark = item.GoodsNo
				this.modelData.CreateDate = item.CreateDate
				this.modelData.GoodsId = item.GoodsId
				if (item.GoodsImageUrl=='') {
					this.modelData.GoodsImageUrl = '../../../../../static/image/placeholderBag.png'
				} else{
					this.modelData.GoodsImageUrl=item.GoodsImageUrl
				}
				console.log(name.row)
				
			},
			addGoodsBtn(){//
//				GoodsNo	varchar(128)	Unchecked 商品编号
//				GoodsInterNo	varchar(128)	Unchecked  商品内部编码
//				GoodsName	varchar(128)	Unchecked 商品名称
//				GoodsPrice	float	Unchecked 价格
//				GoodsState	int	Unchecked 商品状态
//				CreateDate	datetime	Unchecked  创建时间
//				Remark	varchar(128)	Checked 备注
//				GoodsImageUrl	varchar(128)	Checked  商品图片
				this.cleanModelData()//弹框数据清空
				this.ModalTile = '积分商品新增'
				this.modelData.Modaldisabled=false
				this.ISAddState =true//新增模式
				this.modelShow =true
			},
			p(s) {
				return s < 10 ? '0' + s: s;
			},
			GoodsModalOk(){//商品新增或编辑Ok按钮				
				var self = this
				console.log(self.modelData.GoodsState)
				if (self.modelData.GoodsNo && self.modelData.GoodsInterNo && self.modelData.GoodsName && self.modelData.GoodsPrice && self.modelData.GoodsState!=null) {//商品编号 商品内部编码 商品名称 价格  商品状态
					if (self.modelData.GoodsImageUrl=='../../../../../static/image/placeholderBag.png') {//图片地址设置
						self.modelData.GoodsImageUrl=''
					} 
					if (self.ISAddState) {//新增模式
						self.AddGoodsInformation()
					} else{//编辑模式
						self.EideGoodsInformation()
					}
					
					
				} else{
					self.$Message.warning('请将信息填写完整');
				}	
			},
			AddGoodsInformation(){//商品新增
				var self =this
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
				
				 
				var DATA = '{"GoodsNo":"'+self.modelData.GoodsNo+'","GoodsInterNo":"'+self.modelData.GoodsInterNo+'","GoodsName":"'+self.modelData.GoodsName+'","GoodsPrice":'+self.modelData.GoodsPrice+',"GoodsState":'+self.modelData.GoodsState+',"CreateDate":"'+self.modelData.CreateDate+'","Remark":"'+self.modelData.Remark+'","GoodsImageUrl":"'+self.modelData.GoodsImageUrl+'"}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralGoods/AddIntegralGoods',
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			       	 	console.log(json)
			        		if (json=='false') {
			        			self.$Notice.warning({
			                    title: '数据重复',
			                    desc: '商品编号重复,新增失败'
			                });
			        		} else{
			        			
			        	  		self.$Message.success('数据保存成功');
			        	 	    self.cleanModelData()//清除弹框数据
			        	  		self.requestTableData(1)//表格数据刷新
			        		}
			        	 
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			EideGoodsInformation(){//商品编辑
				//api/IntegralGoods/UpdateIntegralGoods
				var self =this
				var DATA = '{"GoodsId":"'+self.modelData.GoodsId+'","GoodsNo":"'+self.modelData.GoodsNo+'","GoodsInterNo":"'+self.modelData.GoodsInterNo+'","GoodsName":"'+self.modelData.GoodsName+'","GoodsPrice":'+self.modelData.GoodsPrice+',"GoodsState":'+self.modelData.GoodsState+',"CreateDate":"'+self.modelData.CreateDate+'","Remark":"'+self.modelData.Remark+'","GoodsImageUrl":"'+self.modelData.GoodsImageUrl+'"}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralGoods/UpdateIntegralGoods',
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			       	 	console.log(json)
			        		if (json==-1) {
			        			self.$Notice.warning({
			                    title: '数据重复',
			                    desc: '商品编号重复,编辑失败失败'
			                });
			        		} else{
			        			self.$Message.success('数据编辑成功');
			        	  		self.cleanModelData()//清除弹框数据
			        	  		self.requestTableData(1)//表格数据刷新
			        		}
			        	  
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			
			},
			cleanModelData(){//清除弹框数据
				this.modelData.GoodsNo = ''
				this.modelData.GoodsInterNo = ''
				this.modelData.GoodsName = ''
				this.modelData.GoodsPrice = ''
				this.modelData.GoodsState = null
				this.modelData.CreateDate = ''
				this.modelData.Remark = ''
				this.modelData.GoodsImageUrl = '../../../../../static/image/placeholderBag.png'
				this.modelData.GoodsId = ''
			},
			searchGoods(){//搜索按钮
				this.requestTableData(1)
				console.log('搜索开始')
			},
			changePage(index){//页面切换
				console.log('当前页码'+index)
				this.requestTableData(index)
			},
			requestTableData(index){
				var self = this
				$('.loding').show()
				self.GoodsList = []
				var DATA = '{"GoodsName":"'+self.searchCriteria.GoodsName+'","State":'+self.searchCriteria.State+',"PageSize":20,"IndexPage":'+index+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralGoods/GetIntegralGoodsList',
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
			        	  self.total = parseInt(dataAll.Count)
			        	  for (var i = 0;i<dataAll.data.length;i++) {
			        	  	if (dataAll.data[i].GoodsState==1) {//上架
			        	  		dataAll.data[i].GoodsStateStr='上架'
			        	  	} else if (dataAll.data[i].GoodsState==2) {//下架
			        	  		dataAll.data[i].GoodsStateStr='下架'
			        	  	} else if (dataAll.data[i].GoodsState==100) {
			        	  		dataAll.data[i].GoodsStateStr='已删除'
			        	  	}
			        	  }			        	  
			        	  
			        	  self.GoodsList = dataAll.data
			        	  $('.loding').hide()
			        	  console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			GoodsImgSuccess(res, file){
            	   $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.modelData.GoodsImageUrl = URLHeader.Loading+'/Product/'+data.Id+'.'+data.Type;
            },
             handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
         	},
         	handleFormatError (file) {//文件格式
            		$('.loding').hide()
                this.$Notice.warning({
                    title: '文件格式不正确',
                    desc: '文件 ' + file.name + ' 格式不正确，请上传 jpg 或 png 格式的图片。'
                });
             },
             handleMaxSize (file) {//文件大小
            		console.log(file)
            		$('.loding').hide()
                this.$Notice.warning({
                    title: '超出文件大小限制',
                    desc: '文件 ' + file.name + ' 太大，不能超过 2M。'
                });
            },
		},
		mounted:function(){
			this.requestTableData(1)
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
.IntegralGoodsList {
	margin: 0 auto;
	padding: 0
}
.IntegralGoodsList .IntegralGoodsListTop{
	border-bottom: 1px solid gainsboro;
}
.IntegralGoodsList .IntegralGoodsListTop .TopUl{
	height: 2.9rem;
	margin-left: 2rem;
}
.IntegralGoodsList .IntegralGoodsListTop .TopUl .TopLi {
	display: block;
	float: left;
	margin-left: 1rem;
	height: 2rem;
	line-height: 2rem;
	
}
.Modal_Ul{
	overflow: auto;
}
.Modal_Ul .Modal_li{
	margin-bottom: 0.6rem;
	
	/*overflow: auto;*/
}
.Modal_Ul .Modal_li span{
	width: 4.5rem;
	display: inline-block;
	margin-left: 2rem;
	margin-right: 2rem;
}
</style>