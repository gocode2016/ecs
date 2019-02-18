<template>
	<div class="SeasoningConfiguration">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h3 style="float: left;margin-left: 1.5rem;">调料推荐</h3>
		<span style="width: 40rem;display: inline-block;">
			<Select v-model="role" style="width:100px;margin-left: 4rem;" placeholder="选择角色">
		        <Option v-for="item in roleList" :value="item.value" :key="item.value">{{ item.label }}</Option>
		    </Select>
		    <Input v-model="seachIfn" placeholder="调料名称" style="width: 150px;"></Input>
		    <Button type="primary" icon="ios-search" @click='searchIngredients' style="margin: 0 1rem;">搜索</Button>
		    <Button type="primary" @click='addIngredients' style="">新增配料</Button>
		</span>
		<div style="margin-top: 1rem;overflow: auto;">
			<Table style="" :columns="Seasoning_columns" :data="Seasonings_tableList" ></Table>
			<Page :total="total" show-total  :page-size='10' style='margin-top: 1rem;float: right;margin-right: 2rem;' @on-change='changePage'></Page>
			
			<Modal
		        title="配料编辑与新增"
		        v-model="seasoningModel"
		        @on-ok="seasoningModelOk"
		        width='360'
		        :mask-closable="false">
		        		<div style="overflow: auto;margin: 0 auto;">
		        			 <img :src="imgSrc" style="max-width: 10rem;max-height: 10rem;display: block;margin: 0 auto;"/>
				        <Upload
					        ref="upload"
					        :show-upload-list="false"
					        :on-success="handleImageSuccess"
					        :format="['jpg','jpeg','png']"
					        :max-size="2048"
					        :before-upload='handleBeforeUpload'
					        :on-format-error="handleFormatError"
					        :on-exceeded-size="handleMaxSize"
					        :multiple=false
					        type="drag"
					        :action=UPImageCook
					        style="width:80px;display: block;margin: 0.5rem auto;" >
					         <Button type="primary">选择照片</Button>
					    </Upload>
					    <div style="margin: 0 auto;text-align: center;">
					    		<span>类型:</span>
					        <Select v-model="FlavourType" style="width:150px;text-align: left;">
						        <Option v-for="item in FlavourTypeList" :value="item.value" :key="item.value">{{ item.label }}</Option>
						    </Select>
					        <p style="padding: 0.5rem 0;"></p>
						    <span>名称:</span>
					        <Input v-model="FlavourName" placeholder="调料名" style="width: 150px;margin-left: 0rem;"></Input>
					    </div>
					    
		        		</div>
		            
		    </Modal>
			
			
			
       </div>
		
	</div>
</template>

<script>
	export default{
		data(){
			return{
				UPImageCook:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Cook',
				seasoningModel:false,//调料弹框
				total:0,//总页数
				pageIndex:'1',//当前页数
				chefactivityid:'',//活动id
				isEdit:false,//是否为编辑模式   true为编辑   false为新增
				selectIndex:null,//选择记录第几条数据
				localImg:'../../../../../static/image/placeholderBag.png',//占位图
				imgSrc:'../../../../../static/image/placeholderBag.png',//上传后的图片地址
				FlavourName:'',//调料名
				FlavourType:'',//类型
				seachIfn:'',//搜索框内容
				//编辑或新增
				FlavourRecId:'',
				role:'0',//角色选择
				roleList:[//角色选择
					{
                        value: '0',
                        label: '全部'
                    },
                    {
                        value: '1',
                        label: '导师推荐'
                    },
                     {
                        value: '2',
                        label: '厨师推荐'
                    },
                    {
                        value: '3',
                        label: '星厨推荐'
                    },
				],
				FlavourTypeList:[//类型
					{
                        value: '1',
                        label: '导师推荐'
                    },
                     {
                        value: '2',
                        label: '厨师推荐'
                    },
                    {
                        value: '3',
                        label: '星厨推荐'
                    },
				],
				Seasoning_columns:[
					{
                        title: '名称',
                        align: 'center',
                        key: 'FlavourName',
                    },
                    {
                        title: '图片',
                        align: 'center',
                        key: 'FlavourPicUrl',
                       
                        render: (h, params) => {
                            return h('img', {
                                attrs:{
                                		src: params.row.FlavourPicUrl,
//                              		class:params.row.pp,
                                },
                                	style:{                            		
							    		maxWidth: '3rem',
//							    		maxHeight: '10rem',
                            		},
//                              h('strong', params.row.WorkNum)
                            });
                        }
                    },
                     {
                        title: '类型',
                        align: 'center',
                        key: 'roleType',
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
                    },
				],
				Seasonings_tableList:[
				],
			}
		},
		methods:{
			redact(name){//编辑
				this.reset()
				var item = name.row
				this.selectIndex =item._index
				this.FlavourName=item.FlavourName
				this.imgSrc= item.FlavourPicUrl
				this.FlavourType =item.FlavourType.toString();
				this.FlavourRecId= item.FlavourRecId.toString();
				console.log(this.FlavourType)
				this.isEdit=true;//设置编辑模式
				this.seasoningModel=true
			},
			addIngredients(){//配料新增按钮
				this.isEdit=false;//设置新增
				this.reset()
				this.seasoningModel=true
			},
			searchIngredients(){//搜索配料
				this.getAlldata(this.pageIndex)
			},
			handleImageSuccess(res, file){//新增上传图
	                var data = JSON.parse(res);
	                this.imgSrc = URLHeader.Loading+'/Cook/'+data.Id+'.'+data.Type;
	                	$('.loding').hide()
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
            handleBeforeUpload(){//上传照片前
            		$('.loding').show()
            },
            seasoningModelOk(){//调料弹框Ok
            		var self =this;
            		if (this.FlavourName && this.imgSrc && this.FlavourType) {
        			 	if (this.isEdit) {//编辑
		            		this.updateList();
		            	} else{//新增
		            		
	            			self.addList()
		            	}
		            	$('.loding').hide()
		            	self.getAlldata(self.pageIndex)
            		}else{
            			self.$Message.warning('请填写完整');
            		}
            		this.isEdit= false;//切换到新增
            		self.getAlldata(1)
//          		this.FlavourName='',
//				this.FlavourType=''
//				this.imgSrc= this.localImg;
            },
			updateList(){//编辑更新
				var self =this;
//				this.Seasonings_tableList[this.selectIndex].FlavourName = this.FlavourName
//				this.Seasonings_tableList[this.selectIndex].FlavourPicUrl = this.imgSrc
//				this.Seasonings_tableList[this.selectIndex].FlavourType = this.FlavourType
				var DATA= '{"ChefActivityId":"'+self.chefactivityid+'","FlavourName":"'+self.FlavourName+'","FlavourPicUrl":"'+self.imgSrc+'","FlavourType":"'+self.FlavourType+'","FlavourRecId":"'+self.FlavourRecId+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/FlavourRec/UpdateFlavour',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						console.log(json)
					},
					error:function(error){
						console.log(error)
					}
				});
				self.FlavourRecId=''
				
			},
			addList(){//新增
				var self =this;
				var DATA= '{"ChefActivityId":"'+self.chefactivityid+'","FlavourName":"'+self.FlavourName+'","FlavourPicUrl":"'+self.imgSrc+'","FlavourType":"'+self.FlavourType+'"}'
//      			console.log(DATA)
        			$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/FlavourRec/AddFlavourRec',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					success:function(json){
						console.log(json)
						if (json=='OK') {
							self.$Message.success('新增成功');
						} else{
							self.$Message.error('新增配料重复');
						}
						self.getAlldata(1)
					},
					error:function(error){
						console.log(error)
					}
				});
				
			},
			reset(){//清空
				this.isEdit=false;//设置新增模式
				this.FlavourName='',
				this.FlavourType=''
				this.imgSrc= this.localImg;
			},
			changePage(index){
				this.getAlldata(index)
			},
			getAlldata(index){//获取数据
				var self =this;
				$.ajax({
			        type :"get",
			        url :URLHeader.ECActivities+'/api/FlavourRec/GetSingeByCA?ca='+self.chefactivityid+'&pageIndex='+index+'&type='+self.role+'&name='+self.seachIfn,
			        dataType : 'json',
			        success : function(json) {
			        	console.log(JSON.parse(json))
			        		var dataAll=JSON.parse(json).data;
			        		self.total = parseInt(JSON.parse(json).Count)
			        		self.Seasonings_tableList =[];
			        		
			        		for (var i = 0;i<dataAll.length;i++) {
			        			if (dataAll[i].FlavourType == '1') {
			        				dataAll[i].roleType='导师推荐'
			        			}else if (dataAll[i].FlavourType == '2') {
			        				dataAll[i].roleType='厨师推荐'
			        			}else if (dataAll[i].FlavourType == '3') {
			        				dataAll[i].roleType='星厨推荐'	
			        			}
			        			self.Seasonings_tableList.push(dataAll[i])
			        		}
			        		$('.loding').hide()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });	
			}
		},
		mounted:function(){
			var self =this;
			
			get_URL();
			self.getAlldata(self.pageIndex)
			function get_URL(){//获取地址参数
    				var hash= window.location.hash.split('?')[1].split('=')[1];
    				self.chefactivityid=hash;
    				
    			}
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
.SeasoningConfiguration{
	overflow: auto;
}
</style>

<style>
	.oo{
		display: none;
		}
</style>