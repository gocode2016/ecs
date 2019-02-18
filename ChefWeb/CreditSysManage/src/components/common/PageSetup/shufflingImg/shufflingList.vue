<template>
	<div class="shufflingList">
		<Modal title="对话框标题" v-model="modal" width='400' :mask-closable="false" class="modal" @on-ok='confirm'>
			<ul style="margin-left: 1.5rem;">
	        		<li>
	        			<span>页面标题</span>
	        			<!--<span style="margin-left: 3rem;">知味山东2017</span>-->
	        			<Select v-model="titleSelect" style="width:200px;margin-left: 1.5rem;">
					    <Option v-for="item in titleList" :value="item.value" :key="item.value">{{ item.lable }}</Option>
					</Select>
	        		</li>
	        		<li>
	        			<span>跳转链接</span>
	        			<Input v-model='modalLink' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
	        		</li>
	        		<li style="width: 19rem;margin: 0;">
	        			<div style="overflow: auto;">
	        				<span style="display: block;float: left;">图片</span>
	        				<img  :src="modalImg" style="max-width: 12.5rem;display: block;float: left;margin-left: 3.2rem;"/>
	        			</div>
	        			<Upload
				        ref="upload"
				        :show-upload-list="false"
				        :on-success="ImgSuccess"
				        :format="['jpg','jpeg','png']"
				        :max-size="2048"
				        :before-upload='handleBeforeUpload'
				        :on-format-error="handleFormatError"
				        :on-exceeded-size="handleMaxSize"
				        :multiple=false
				        type="drag"
				        :action=UPImageActivity
				        style="display:block;width:80px;margin-left:8rem;margin-top:0.5rem ;" >
				         <Button type="primary">选择图片</Button>
				    </Upload>
	        		</li>
	        		<li>
	        			<span>显示</span>
	        			<Radio-group v-model="modalShow" style='margin-left:3rem;'>
				        <Radio  v-for='item in showAll' :label='item.value'  :key="item.value"  style='margin-left: 1rem;'>{{item.lable}}</Radio>
		   			</Radio-group>
	        		</li>
	        		<li>
	        			<span>排序</span>
	        			<Input v-model="modalSort" placeholder="请输入..." style="width: 200px;margin-left: 3rem;"></Input>
	        		</li>
	        </ul>
	    </Modal>
	    <Button type="primary" style="margin-left: 88%;" @click='addImgInformation'>新增</Button>
	    <Button type="primary" @click='deletSelectImg'>删除</Button>
		<Table style="margin-top: 1rem;" :columns="shufflingList_columns" :data="shufflingList_tableList" @on-selection-change='selectDelet'></Table>
		<Page  :total="total" show-elevator class='page' :page-size='10' @on-change='changgePage' style='margin-right: 3rem;float: right;margin-top: 1rem;padding-bottom: 1rem;' show-total></Page>
	</div>
</template>
<script>
	export default{
		data(){
			return{
				UPImageActivity:URLHeader.Tools+"/api/Image/ImgUpload?img_type=Activity",
				total:1,//总数
				userName:sessionStorage.getItem('loginkey'),
				deletImg:[],//要删除的信息
				isAddImg:true,//添加状态
				//editItem:{},//编辑保存信息
				modal:false,//弹框
				modalLink:'',//弹框链接
				modalImg:'../../../../../static/image/placeholderBag.png',//弹框图片地址
				modalShow:'',//显示选择
				modalSort:'',//排序
				titleSelect:'',//标题
				modalBannerId:'',
				titleList:[//
					{
						value:'0',
						lable:'当前活动'
					},
					{
						value:'1',
						lable:'积分商城'
					},
					{
						value:'2',
						lable:'产品库'
					},
					{
						value:'3',
						lable:'菜品库'
					}
				],
				showAll:[
					{
						value:'1',
						lable:'显示'
					},
					{
						value:'0',
						lable:'隐藏'
					},
				],
				shufflingList_tableList:[

				],
				shufflingList_columns:[
         			{	
         				title:'选择',
                        type: 'selection',
                        align: 'center',
                        width:80
                     },
         			{	
                        title:'NO.',
                        align: 'center',
                        type: 'index',
                        width:80
                     },
         			{
                        title: '页面标题',
                        key: 'BannerType',
                        align: 'center',
                        width:150
                    },
                    {
                        title: '图片预览',
                        align: 'center',
                        key: 'PicUrl',
                        render:(h,paeams) =>{
                        		return h('img',{
                        			attrs:{
                        				src:paeams.row.PicUrl
                        			},
                        			style:{
                        				maxWidth:'5rem'
                        			}
                        		});
                        }
                    },
                     {
                        title: '跳转链接',
                        align: 'center',
                        key: 'Link',
                        width:300
                    },
                    {
                        title: '显示',
                        align: 'center',
                        key: 'IsDisplay',
                        
                    },
                    {
                        title: '排序',
                        align: 'center',
                        key: 'PriorityId',
                    },
                     {
                        title: '操作',
                        align: 'center',
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                    style:{
                                    	width:'50px'
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
			}
		},
		methods:{
			redact(name){//编辑按钮
				console.log(name);
				var self =this
//				this.editItem = name.row
				this.isAddImg = false;
				//titleSelect
				if (name.row.BannerType=='当前活动') {
    				self.titleSelect='0'
    			}else if (name.row.BannerType=='积分商城') {
    				self.titleSelect='1'
    			}else if (name.row.BannerType=='产品库') {
    				self.titleSelect='2'
    			}else if (name.row.BannerType=='菜品库') {
    				self.titleSelect='3'
    			}
				if (name.row.IsDisplay=='是') {
					self.modalShow='1'
				} else{
					self.modalShow='0'
				}
				self.modalLink=name.row.Link
				self.modalImg=name.row.PicUrl
				if (self.modalImg=='') {
					self.modalImg='../../../../../static/image/placeholderBag.png'
				}
				self.modalBannerId = name.row.BannerId
				self.modalSort=name.row.PriorityId
				console.log(self.modalShow)
				this.modal = true;
			},
			addImgInformation(){//新增按钮
				this.isAddImg = true;
				this.modal = true;
				//弹框数据清空
				this.titleSelect=''
				this.modalLink=''
				this.modalImg='../../../../../static/image/placeholderBag.png'
				this.modalShow=''
				this.modalSort=''
			},
			confirm(){//确认按钮
				var self =this 
				var item;
				if (self.isAddImg) {//新增
					if (self.modalImg =='../../../../../static/image/placeholderBag.png') {
						self.modalImg = ''
					}
					
					if (self.modalImg && self.modalLink && self.modalSort && self.modalShow &&self.titleSelect) {
						
						var DATA = '{"PicUrl":"'+self.modalImg+'","Link":"'+self.modalLink+'","PriorityId":"'+self.modalSort+'","IsDisplay":"'+self.modalShow+'","CreatePerson":"'+self.userName+'","BannerType":"'+self.titleSelect+'"}'
						console.log(DATA)
						$.ajax({
							type:"post",
							url:URLHeader.ECActivities+'/api/Banner/AddBanner',
							contentType:'application/json; charset=utf-8',
							data:DATA,
							async:true,
							success:function(json){
								//var dataAll = JSON.parse(json);
								console.log("轮播图"+json)
								if (json=='OK') {
									self.$Message.success('新增成功');
									self.getShufflingList(1)
								} else{
									self.$Message.error('新增失败');
								}
							},
							error:function(error){
								console.log(error)
								self.$Message.error('新增失败');
							}
						});
					}else{
						self.$Message.error('请吧信息填写完整');
						
					}
					
					
				} else{//编辑
					console.log('编辑')
					var DATA = '{"BannerId":"'+self.modalBannerId+'","PicUrl":"'+self.modalImg+'","Link":"'+self.modalLink+'","PriorityId":"'+self.modalSort+'","IsDisplay":"'+self.modalShow+'","UpdatePerson":"'+self.userName+'","BannerType":"'+self.titleSelect+'"}'
					console.log(DATA)
					$.ajax({
						type:"post",
						url:URLHeader.ECActivities+'/api/Banner/UpdateSingle',
						contentType:'application/json; charset=utf-8',
						data:DATA,
						async:true,
						success:function(json){
							//var dataAll = JSON.parse(json);
//								console.log("轮播图"+json)
								if (json=='OK') {
									self.$Message.success('编辑成功');
									self.getShufflingList(1)
								} else{
									self.$Message.error('编辑失败');
								}
							console.log(json)
						},
						error:function(error){
							console.log(error)
							self.$Message.error('编辑失败');
						}
					});
					
				}

				
				self.modalLink=''
				self.modalImg='../../../../../static/image/placeholderBag.png'
				self.modalShow=''
				self.modalSort=''
				self.titleSelect=''
				self.modalBannerId = ''
		},
			selectDelet(name){//每次选择计算(table方法)
				console.log(name)
				this.deletImg = [];
				for (var i = 0;i<name.length;i++) {
					this.deletImg.push(name[i].BannerId)
				}
//				this.deletImg = name
			},
			deletSelectImg(){
				var self =this
				//URLHeader.ECActivities+/Help/api/Banner/DelBannerMore
				var arr = this.deletImg.toString()
				var DATA ='{"BannerIds":"'+arr+'"}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Banner/DelBannerMore',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						//var dataAll = JSON.parse(json);
							//	console.log("轮播图"+json)
								if (json=='OK') {
									self.$Message.success('删除成功');
									self.getShufflingList(1)
								} else{
									self.$Message.error('删除失败');
								}
						console.log(json)
					},
					error:function(error){
						console.log(error)
						self.$Message.error('删除失败');
					}
				});
				
//				for (var i = 0;i<self.deletImg.length;i++) {
//					this.shufflingList_tableList.splice(this.shufflingList_tableList.indexOf(self.deletImg[i]), 1)
//				}	
			},
			 handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
         	},
         	ImgSuccess (res, file) {//上传头像成功
                $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.modalImg = URLHeader.Loading+'/Activity/'+data.Id+'.'+data.Type;
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
            getShufflingList(index){
            	//URLHeader.ECActivities+/api/Banner/GetBannerList?pageIndex={pageIndex}
            	    var self = this
            		$.ajax({
			        type :"get",
			        url :URLHeader.ECActivities+'/api/Banner/GetBannerList?pageIndex='+index+'&pageCount=10',
			        dataType : 'json',
			        success : function(json) {
			        		self.shufflingList_tableList = []
			        		var dataAll = JSON.parse(json);
			        		self.total = parseInt(dataAll.Count)
			        		console.log(JSON.parse(json))
			        		for (var i = 0;i<dataAll.data.length;i++) {
			        			if (dataAll.data[i].BannerType===0) {
			        				dataAll.data[i].BannerType='当前活动'
			        			}else if (dataAll.data[i].BannerType===1) {
			        				dataAll.data[i].BannerType='积分商城'
			        			}else if (dataAll.data[i].BannerType===2) {
			        				dataAll.data[i].BannerType='产品库'
			        			}else if (dataAll.data[i].BannerType===3) {
			        				dataAll.data[i].BannerType='菜品库'
			        			}
			        			self.shufflingList_tableList.push(dataAll.data[i])
			        		}
//			        	  	self.shufflingList_tableList = dataAll.data  //设置轮播图列表
			        	  	
			        //	 $('.loding').hide()
			        		
//			        	 	console.log(self.shufflingList_tableList)
			        },
			        error : function(error) { 
						
			        }
	   		    });
            },
            changgePage(index){
           	 	this.getShufflingList(index)
            },
		},
		mounted:function(){
			this.getShufflingList(1)
		}
	}
</script>

<style scoped>
.modal ul li {
	display: block;
	margin: 0.5rem auto;
	
}
</style>