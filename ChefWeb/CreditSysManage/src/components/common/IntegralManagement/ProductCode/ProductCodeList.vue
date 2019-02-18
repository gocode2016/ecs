<template>
	<div class="ProductCodeList">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<div class="ProductCodeListTop">
			<ul class="TopUl">
				<li class="TopLi">
					<span>名称</span>
					<Input v-model="searchCriteria.Name" placeholder="商品名称" style="width: 100px;margin-left: 0.5rem;"></Input>
				</li>
				<li class="TopLi">
					<span>产品类别</span>
					<Select v-model="searchCriteria.Code" style="width:160px;position: relative;margin-left: 0.5rem;" placeholder = '请选择产品类别'>
				        <Option v-for="item in ProductCategoryList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>规格包装</span>
					<Select v-model="searchCriteria.Range" style="width:100px;position: relative;margin-left: 0.5rem;" placeholder = '请选择规格'>
				        <Option v-for="item in SpecificationsList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>生产工厂</span>
					<Select v-model="searchCriteria.Source" style="width:120px;position: relative;margin-left: 0.5rem;" placeholder = '请选择工厂'>
				        <Option v-for="item in factoryList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>月份</span>
					<Select v-model="searchCriteria.March" style="width:100px;position: relative;margin-left: 0.5rem;" placeholder = '请选择月份'>
				        <Option v-for="item in monthList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<span>状态</span>
					<Select v-model="searchCriteria.State" style="width:100px;position: relative;margin-left: 0.5rem;" placeholder = '请选择状态'>
				        <Option v-for="item in stateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				    </Select>
				</li>
				<li class="TopLi">
					<Button type="primary" icon="ios-search" @click ='searchGoods'>搜索</Button>
				</li>
				<li class="TopLi">
					<Button type="primary" @click="generationBtn">生成</Button>
				</li>
				
			</ul>
		</div>
		<div class="body">
			<Modal
		        title="生成二维码"
		        v-model="modelShow"
		        @on-ok="GoodsModalOk"
		        :mask-closable="false">
		       	<ul class="Modal_Ul">
		       		<li class="Modal_li">
		       			<span>产品类别</span>
						<Select v-model="modelData.GoodsId" style="width:200px;position: relative;margin-left: 0.5rem;" placeholder = '请选择产品类别'>
					        <Option v-for="item in ModalProductCategoryList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
		       		</li>
		       		<li class="Modal_li">
		       			<span>规格包装</span>
						<Select v-model="modelData.Range" style="width:200px;position: relative;margin-left: 0.5rem;" placeholder = '请选择规格'>
					        <Option v-for="item in ModalSpecificationsList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
		       		</li>
		       		<li class="Modal_li">
		       			<span>生产工厂</span>
						<Select v-model="modelData.Source" style="width:200px;position: relative;margin-left: 0.5rem;" placeholder = '请选择工厂'>
					        <Option v-for="item in ModalfactoryList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
		       		</li>
		       		<li class="Modal_li">
		       			<span>月份</span>
						<Select v-model="modelData.March" style="width:200px;position: relative;margin-left: 0.5rem;" placeholder = '请选择月份'>
					        <Option v-for="item in ModalmonthList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
		       		</li>
		       		</li>
		       		<li class="Modal_li">
		       			<span style="display: block;float: left;">数量</span>
		       			<Input v-model="modelData.Num" placeholder="请输入生成数量" style="width: 200px;margin-left: 0.7rem;"></Input>
		       		</li>
		       	</ul>
		   </Modal>
		   <a :href="UploadURL" class='UploadURL' style="display: none;"></a>
			<Table :columns="QrCodeColumns" :data="QrCodeList" ></Table>
			
			<Page :total="total" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='changePage'></Page>
		</div>
	</div>
</template>
<script>

	export default{
		data(){
			return{
				setTime:null,//定时刷新数据
				UploadURL:'',//下载地址
				modalDeletID:'',
				modelShow:false,//积分商品显示 true
				total:2,//page总条数
				searchCriteria:{//搜索条件
					Code:'',//产品类别
					Name:'',//商品名字
					Range:0,//规格包装
					Source:0,//生产工厂
					March:0,//月份
					State:0,//状态
				},
				modelData:{//弹框数据
					GoodsId:null,//产品类别
					codeNumb:'',//数量
					Range:null,//规格包装
					Source:null,//生产工厂
					March:null,//月份
					Num:'',//数量
				},
				ModalProductCategoryList:[//产品类别
				],
				ModalSpecificationsList:[//规格包装
					{
                        value: 10,
                        label: '160ml'
                    },
                    {
                        value: 15,
                        label: '460ml'
                    },
                    {
                        value: 16,
                        label: '500ml'
                    },
                    {
                        value: 20,
                        label: '1L'
                    },
                    
                    {
                        value: 23,
                        label: '1.78L'
                    },
                    
                    {
                        value: 24,
                        label: '1.8L'
                    },
                    {
                        value: 26,
                        label: '2L'
                    },
                    
                    {
                        value: 28,
                        label: '3.78L'
                    },
                    {
                        value: 41,
                        label: '600g'
                    },
                    
                    {
                        value: 42,
                        label: '605g'
                    },
                    
                    {
                        value: 43,
                        label: '630g'
                    },
                    {
                        value: 46,
                        label: '2.2kg'
                    },
                    
                    {
                        value: 47,
                        label: '2.3kg'
                    },
                    {
                        value: 60,
                        label: '5kg'
                    },
                    {
                        value: 62,
                        label: '6.18kg'
                    },
                    {
                        value: 63,
                        label: '580G'
                    }
				],
				ModalfactoryList:[//生产工厂
					{
                        value: 10,
                        label: '欣和企业'
                    },
                    {
                        value: 11,
                        label: '味达美工厂'
                    },
                    {
                        value: 12,
                        label: '神州1工厂'
                    },
                    {
                        value: 13,
                        label: '蓬莱工厂'
                    },
                    
                    {
                        value: 14,
                        label: '济南欣昌工厂'
                    },
                    
                    {
                        value: 15,
                        label: '济南宜和'
                    },
                    {
                        value: 16,
                        label: '味达美开发区工厂'
                    },
                    
                    {
                        value: 17,
                        label: '烟台欣尚 '
                    },
                    
				],
				ModalmonthList:[//月份
					{
                        value: 1,
                        label: '一月'
                    },
                    {
                        value: 2,
                        label: '二月'
                    },
                    {
                        value: 3,
                        label: '三月'
                    },
                    {
                        value: 4,
                        label: '四月'
                    },
                    
                    {
                        value: 5,
                        label: '五月'
                    },
                    
                    {
                        value: 6,
                        label: '六月'
                    },
                    {
                        value: 7,
                        label: '七月'
                    },
                    {
                        value: 8,
                        label: '八月'
                    },
                    {
                        value: 9,
                        label: '九月'
                    },
                    {
                        value: 10,
                        label: '十月'
                    },
                    {
                        value: 11,
                        label: '十一月'
                    },
                    {
                        value: 12,
                        label: '十二月'
                    },
				],
				ProductCategoryList:[//产品类别
					{
                        value: '',
                        label: '全部'
                    },
                    {
                        value: '08',
                        label: '605G蒜蓉辣椒酱'
                    },
                    {
                        value: '01',
                        label: '600G味达美压锅酱'
                    },
                     {
                        value: '03',
                        label: '1L味达美冰糖老抽'
                    },
                    {
                        value: '04',
                        label: '2L味达美海鲜捞汁'
                    },
                    {
                        value: '05',
                        label: '2L味达美酸辣捞汁'
                    },
                    {
                        value: '06',
                        label: '1L剁椒鱼头鲜豉油'
                    },
                     {
                        value: '07',
                        label: '2L剁椒鱼头鲜豉油'
                    },
                    {
                        value: '02',
                        label: '2L味达美味极鲜酱油'
                    },
                    {
                        value: '09',
                        label: '1.8L味达美冰糖老抽酱油 '
                    },
                    {
                        value: '10',
                        label: '2.2kg葱伴侣黄豆酱'
                    },
                     {
                        value: '11',
                        label: '5kg葱伴侣黄豆酱'
                    },
                    {
                        value: '12',
                        label: '630g红烧焖酱'
                    },
                    
                    {
                        value: '13',
                        label: '尚品蚝油瓶20170913测试'
                    },
                    {
                        value: '91',
                        label: '味达美臻品蚝油'
                    },
                    {
                        value: '92',
                        label: 'B'
                    },
                    {
                        value: '14',
                        label: '6.18KG味达美尚品蚝油'
                    },
				],
				SpecificationsList:[//规格包装
					{
                        value: 0,
                        label: '全部'
                    },
					{
                        value: 10,
                        label: '160ml'
                    },
                    {
                        value: 15,
                        label: '460ml'
                    },
                    {
                        value: 16,
                        label: '500ml'
                    },
                    {
                        value: 20,
                        label: '1L'
                    },
                    
                    {
                        value: 23,
                        label: '1.78L'
                    },
                    
                    {
                        value: 24,
                        label: '1.8L'
                    },
                    {
                        value: 26,
                        label: '2L'
                    },
                    
                    {
                        value: 28,
                        label: '3.78L'
                    },
                    {
                        value: 41,
                        label: '600g'
                    },
                    
                    {
                        value: 42,
                        label: '605g'
                    },
                    
                    {
                        value: 43,
                        label: '630g'
                    },
                    {
                        value: 46,
                        label: '2.2kg'
                    },
                    
                    {
                        value: 47,
                        label: '2.3kg'
                    },
                    {
                        value: 60,
                        label: '5kg'
                    },
                    {
                        value: 62,
                        label: '6.18kg'
                    },
                    {
                        value: 63,
                        label: '580G'
                    }
				],
				factoryList:[//生产工厂
					{
                        value: 0,
                        label: '全部'
                    },
					{
                        value: 10,
                        label: '欣和企业'
                    },
                    {
                        value: 11,
                        label: '味达美工厂'
                    },
                    {
                        value: 12,
                        label: '神州1工厂'
                    },
                    {
                        value: 13,
                        label: '蓬莱工厂'
                    },
                    
                    {
                        value: 14,
                        label: '济南欣昌工厂'
                    },
                    
                    {
                        value: 15,
                        label: '济南宜和'
                    },
                    {
                        value: 16,
                        label: '味达美开发区工厂'
                    },
                    
                    {
                        value: 17,
                        label: '烟台欣尚 '
                    },
                    
				],
				monthList:[//月份
					{
                        value: 0,
                        label: '全部'
                    },
					{
                        value: 1,
                        label: '一月'
                    },
                    {
                        value: 2,
                        label: '二月'
                    },
                    {
                        value: 3,
                        label: '三月'
                    },
                    {
                        value: 4,
                        label: '四月'
                    },
                    
                    {
                        value: 5,
                        label: '五月'
                    },
                    
                    {
                        value: 6,
                        label: '六月'
                    },
                    {
                        value: 7,
                        label: '七月'
                    },
                    {
                        value: 8,
                        label: '八月'
                    },
                    {
                        value: 9,
                        label: '九月'
                    },
                    {
                        value: 10,
                        label: '十月'
                    },
                    {
                        value: 11,
                        label: '十一月'
                    },
                    {
                        value: 12,
                        label: '十二月'
                    },
				],
				stateList:[
					{
                        value: 0,
                        label: '全部'
                    },
                    {
                        value: 1,
                        label: '已完成'
                    },
                    {
                        value: 2,
                        label: '未完成'
                    },
				],
				QrCodeList:[//表格数据
				],
				QrCodeColumns:[//商品;列表表头
					{
						title:'名称',
						width:180,
						align: 'center',
						key:'Name'
					},
					{
						title:'规格包装',
						align: 'center',
						key:'Range',
					},
					{
						title:'生产工厂',
						align: 'center',
						key:'Source',
					},
					{
						title:'月份',
						align: 'center',
						key:'March',
					},
					{
						title:'数量',
						align: 'center',
						key:'Num',
					},
					{
						title:'状态',
						align: 'center',
						key:'State',
					},
					{
						title:'生成时间',
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
                                    class:{
                                    	hidenDonload:params.row.hidenDonload
                                    },
                                    style: {
										width:'3rem'
                                    },
                                    on: {
                                        click: () => {
                                            this.Upload(params)
                                        }
                                    }
                                }, '下载'),
                                 h('span', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                     class:{
                                    	hidenDonload:!params.row.hidenDonload
                                    },
                                    style: {
										width:'3rem',
										color:'red'
                                    },
                                }, '生成中...'),
                               
                            ]);
                        }
					},
				],
			}
		},
		methods:{
			Upload(name){//模板下载
				this.UploadURL = URLHeader.Integral+name.row.DownLoadUrl
				console.log(this.UploadURL)
				window.location.href = this.UploadURL
			},
			generationBtn(){//
				this.cleanModelData()//弹框数据清空
				this.modelShow =true
			},
			p(s) {
				return s < 10 ? '0' + s: s;
			},
			GoodsModalOk(){//商品新增或编辑Ok按钮				
				var self = this
//				 public int PackageId { get; set; }  
//				 public string Name { get; set; }  
//				 public int GoodsId { get; set; }
//				 public string Url { get; set; }
//				 public int Range { get; set; }
//				 public string GoodsCode { get; set; }
//				 public int Source { get; set; }
//				 public int March { get; set; }
//				 public int Num { get; set; }
//				 public int State { get; set; }
//				 public string Remark { get; set; }
//				 public string DownLoadUrl { get; set; }
//				 public DateTime CreateDate { get; set; }
				var self =this
//					GoodsId:'0',//产品类别
//					codeNumb:'',//数量
//					Range:0,//规格包装
//					Source:0,//生产工厂
//					March:0,//月份
//					Num:'',//数量
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
				
				if (self.modelData.GoodsId && self.modelData.Range && self.modelData.Source && self.modelData.March && self.modelData.Num) {
					var DATA = '{"Name":"","GoodsId":'+self.modelData.GoodsId+',"Url":"","Range":'+self.modelData.Range+',"GoodsCode":"","Source":'+self.modelData.Source+',"March":'+self.modelData.March+',"Num":'+self.modelData.Num+',"State":0,"Remark":"","DownLoadUrl":"","CreateDate":"'+self.modelData.CreateDate+'"}'
					console.log(DATA)
					$.ajax({
				        type :"post",
				        url :URLHeader.Integral+'/api/IntegralGoodsQrc/Generate',///cts/core/
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
				} else{
					self.$Message.error('请将数据填写完整');
				}
				
					
			},
			cleanModelData(){//清除弹框数据
				this.modelData.GoodsId = null//产品类别
				this.modelData.Num = ''//数量
				this.modelData.Range = null//规格包装
				this.modelData.Source = null//工厂
				this.modelData.March = null//月份
				this.modelData.CreateDate=''
			},
			searchGoods(){//搜索按钮
				this.requestTableData(1)
				console.log('搜索开始')
			},
			changePage(index){//页面切换
				console.log('当前页码'+index)
				this.requestTableData(index)
			},
			getMoadlGoodsIDSelect(){//弹框  产品类别数据源
				var self= this
				$.ajax({
			        type :"get",
			        url :URLHeader.Integral+'/api/IntegralGoods/GetIntegralGoodsSelectList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        	  self.ModalProductCategoryList = JSON.parse(json)
			        	   //console.log( self.ModalProductCategoryList)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			requestTableData(index){//api/IntegralGoods/GetIntegralGoodsSelectList
				var self = this
				var hish = window.location.hash
				if(hish!='#/ProductCodeList'){
				    window.clearInterval(self.setTime); 
				    return
				}
				
				$('.loding').show()
				self.GoodsList = []
				var DATA = '{"Name":"'+self.searchCriteria.Name+'","Code":"'+self.searchCriteria.Code+'","Range":'+self.searchCriteria.Range+',"Source":'+self.searchCriteria.Source+',"March":'+self.searchCriteria.March+',"State":'+self.searchCriteria.State+',"PageSize":20,"IndexPage":'+index+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.Integral+'/api/IntegralGoodsQrc/GetGoodsQrcList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
//			        	   console.log(JSON.parse(json))
			        	  self.total = parseInt(dataAll.Count)
			        	  dataAll.data.map((item) =>{
			        	  	self.SpecificationsList.map((one) =>{//规格包装遍历筛选
			        	  		if (item.Range===one.value) {
			        	  			item.Range= one.label
			        	  		}
			        	  	})
			        	  	
			        	  	self.factoryList.map((one) =>{//生产工厂
			        	  		if (item.Source===one.value) {
			        	  			item.Source= one.label
			        	  		}
			        	  	})
			        	  	
			        	  	self.monthList.map((one) =>{//月份遍历
			        	  		if (item.March===one.value) {
			        	  			item.March= one.label
			        	  		}
			        	  	})
			        	  	
			        	  	self.stateList.map((one) =>{//状态
			        	  		if (item.State===one.value) {
			        	  			item.State= one.label
			        	  		}
			        	  	})
			        	  	if (item.State=='未完成') {
			        	  		item.hidenDonload = true
			        	  	}else{
			        	  		item.hidenDonload = false
			        	  	}
			        	  	
			        	  })
			        	  
			        	  console.log(dataAll.data)
			        	  self.QrCodeList = dataAll.data
			        	  $('.loding').hide() 
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
		},
		mounted:function(){
			var self =this
			this.requestTableData(1)
			this.getMoadlGoodsIDSelect()
//			this.setTime= setInterval(function(){
//				self.requestTableData(1)
//　　			},60000);
			
		}
	}
</script>
<style>
	body,html{
		width: 1420px;
	}
	.hidenDonload{
	display: none;
}
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
.ProductCodeList {
	margin: 0 auto;
	padding: 0;
	
}
.ProductCodeList .ProductCodeListTop{
	/*border-bottom: 1px solid gainsboro;*/
}
.ProductCodeList .ProductCodeListTop .TopUl{
	width: 1380px;
	height: 2.9rem;
}
.ProductCodeList .ProductCodeListTop .TopUl .TopLi {
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