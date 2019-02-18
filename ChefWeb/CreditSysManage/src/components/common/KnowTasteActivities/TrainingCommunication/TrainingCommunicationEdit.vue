<template>
	<div class="TrainingCommunicationAdd">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h1 style="width: 13rem;margin:1rem auto;border: 1px dashed gray;text-align: center;border-radius:4px ;margin-bottom: 2rem;">培训交流编辑</h1>
		<div class="content">
			<div class="left" style="border-right:1px dashed gray  ;">
				<ul>
					<li style="height: 3rem;">
						<span>课程名称</span>
						<Input v-model="information.CurriculumName" placeholder="请输入课程名称..." style="width: 200px;margin: 0 1rem;"></Input>
					</li>
					<li style="overflow: auto">
						<span style="display: block;float: left;">课程类型</span>
						<Tabs v-model='information.CurriculumType' type="card" @on-click='tabeClick' style='display: block;float: left;width: 13rem;margin-left: 1.4rem;'>
					        <TabPane label="图文课程" name="1">
					        		<div style="margin-top: 1rem;" class="imgDiv">
									<img :src="information.imgUrl" style="max-width: 10rem;margin: 0 auto;display: block;" />
									<p style="text-align: center;margin: 1rem auto;">封面图</p>
									<Upload
								        ref="upload"
								        :show-upload-list="false"
								        :on-success="handleImgSuccess"
								        :format="['jpg','jpeg','png']"
								        :max-size="2048"
								        :before-upload='handleBeforeUpload'
								        :on-format-error="handleFormatError"
								        :on-exceeded-size="handleMaxSize"
								        :multiple=false
								        type="drag"
								        :action=UPImageMember
								        style="display:block;width:80px;margin: 0 auto;" >
								         <Button type="primary">选择图片</Button>
								    </Upload>
								</div>
					        </TabPane>
					        <TabPane label="视频课程" name="2">
					        		<div class="urlDiv" style="width: 20rem;">
									<span>视频地址</span>
									<!--<Input v-model="information.vidoUrl" placeholder="请输入视频地址..." style="width: 150px"></Input>-->
									<Input v-model="information.vidoUrl" type="textarea" :autosize="{minRows: 2,maxRows: 5}" style="width: 150px" placeholder="视频地址"></Input>
								</div>
					        </TabPane>
					   </Tabs>
					</li>
					<li style="margin: 1rem auto;">
						<span>排序</span>
						<Input v-model="information.Priority" placeholder="请输入序号"  style="width: 200px;margin-left: 2.5rem;"></Input>
					</li>
					<li>
						<span>状态</span>
						<Select v-model="information.IsDisplay" style="width:200px;margin-left: 2.5rem;" placeholder="请选择">
					        <Option v-for="item in isShowList" :value='item.value'  :key='item.value'>{{item.label}}</Option>
					    </Select>
					</li>
					<li style="margin-top: 1rem;">
						<span style="">课程内容</span>
						<div class="rtf-content" >
					        <quill-editor ref="myTextEditor" v-model="information.content" :config="editorOption" style='height: 25rem;'></quill-editor>
					    </div>
					</li>

				</ul>
				
			</div>
			<div class="right">
				<span>课程讲师</span>
				<Button type="primary" style="margin-left: 1rem;" @click='addTeacher'>新增讲师</Button>
				<Modal
			        title="讲师新增及编辑"
			        v-model="modal"
			        :mask-closable="false"
			        @on-ok='confirm'>
			        <div style="overflow: auto;">
				        <div style="display: block;float: left;">
				        		 <img :src="modelInformation.HeadPicUrl" style="max-width: 8rem;"/>
				        		 <Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="handleTeacherImgSuccess"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImageMember
						        style="display:block;width:80px;margin-left: 1.5rem;" >
						         <Button type="primary">选择照片</Button>
						    </Upload>
				        </div>
				       	<div style="display: block;float: left;margin-left: 1rem;">
				       		<ul>
				       			<li>
				       				<span>讲师姓名</span>
				       				<Input v-model="modelInformation.LecturerName" placeholder="请输入..." style="width: 200px;margin-left: 1rem;"></Input>
				       			</li>
				       			<li style="margin:1rem auto ;overflow: auto;">
				       				<span style="display: block;float: left;height: 2rem;line-height: 2rem;">岗位</span>
									<!--<Cascader :data="jobsList" v-model="modelInformation.Post" style="width: 200px;float: left;margin-left: 2.8rem;"></Cascader>-->
									<Input v-model="modelInformation.Post" placeholder="请输入..." style="width: 200px;margin-left: 2.7rem;"></Input>
				       			</li>
				       			<li style="margin-top: 1rem;">
				       				<span>酒店</span>
				       				<Input v-model="modelInformation.HotelName" placeholder="请输入..." style="width: 200px;margin-left: 2.5rem;"></Input>
				       			</li>
				       		</ul>
				       	</div>
			       </div>
			    </Modal>
				<ul>
					<li v-for='(item,index) in cardList'>
						<div class="card">
							<img :src="item.HeadPicUrl"  style="max-width: 6rem;margin-top: 0.5rem;display: block;float: left;max-height: 6rem;margin-left: 1rem;"/>
							<div style="display: block;float: left;width: 10rem;height: 7rem;margin-left: 1rem;">
								<li>
									<span class="lable">讲师姓名:</span>
									<span class="value">{{item.LecturerName}}</span>
								</li>
								<li>
									<span class="lable">岗位:</span>
									<span class="value">{{item.Post}}</span>
								</li>
								<li>
									<span class="lable">酒店:</span>
									<span class="value">{{item.HotelName}}</span>
								</li>
							</div>
							<div style="display: block;float: right;width: 4rem;">
								<Button type="primary" @click='editTeacher(item,index)' size="small" style="margin: 0.5rem auto;" >编辑</Button>
								<Button type="primary" @click='deletTeacher(item)' size="small" >删除</Button>
							</div>
						</div>
					</li>
				</ul>
				
			</div>
		</div>
		<Button type="primary" @click='saveData'  style="margin-left: 57rem;">保存</Button>
	</div>
</template>

<script>
	 import { quillEditor } from 'vue-quill-editor';//编写框
	export default{
		data(){
			return{
				UPImageMember:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Member',
				UpdatePerson:sessionStorage.getItem('loginkey'),//当前登录人
				CuInterId:0,//活动id
				saveSucceed:true,//默认保存失败
				CreatePerson:sessionStorage.getItem('loginkey'),//创始人
				modal:false,//讲师弹框  关闭
				isAddTeacher:true,//新增状态
				selectTeacherIndex:0,//记录  编辑数据
				coursesName:'',
				information:{
					CurriculumName:'',//课程名字
					imgUrl:'../../../../../static/image/placeholderBag.png',//图片地址
					CurriculumType:'',//类型  1为图片  2视频
					vidoUrl:'',//视频地址
					Priority:'',//排序
					IsDisplay:'',//状态  是否显示
					content:'',//课程内容
					
				},
				isShowList:[
					{
						value:'1',
						label:'显示'
					},
					{
						value:'0',
						label:'隐藏'
					},
				],
				cardList:[//讲师数据源
				],
				modelInformation:{//弹框数据   
					HotelName:'',//酒店名字
					HeadPicUrl:'../../../../../static/image/HeadPortrait.png',//讲师图片
					LecturerName:'',//讲师姓名
					//Post:[],//工作岗位
					Post:'',//工作岗位
					LecturerId:'',//讲师id
					CuInterId:'',//交流活动id
				},
				
				editorOption: {
               },
			}
		},
		components: {
            quillEditor
       },
		methods:{
			tabeClick(name){
				this.information.CurriculumType=name
				console.log(this.information.CurriculumType)
				if (name=='1') {
					$('.TrainingCommunicationAdd .content .left .ivu-tabs-tabpane').css({
						height:'100%'
					})
				} else{
					$('.TrainingCommunicationAdd .content .left .ivu-tabs-tabpane').css({
						height:'6rem'
					})
				}
			},
			 handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
         	},
         	 handleImgSuccess (res, file) {//上传封面图片
                $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.information.imgUrl = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
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
            //===================右侧
            handleTeacherImgSuccess(res, file){
            	   $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.modelInformation.HeadPicUrl = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
            },
            addTeacher(){//导师新增
            	
            		this.modelInformation.LecturerName=''
            		//this.modelInformation.Post=[]
            		this.modelInformation.Post=''
            		this.modelInformation.HotelName=''
            		this.modelInformation.HeadPicUrl='../../../../../static/image/HeadPortrait.png'
            		this.modelInformation.CuInterId = this.CuInterId//活动id
            		this.modelInformation.LecturerId = '0'//讲师id  新增的讲师默认为0
            		this.isAddTeacher = true;//设置为新增状态
            		this.modal=true;
            		
            },
            confirm(){//确认按钮
            		var item;
            		var self =this;
            		if (this.modelInformation.HeadPicUrl=="../../../../../static/image/HeadPortrait.png") {
            			this.modelInformation.HeadPicUrl=''
            		}
            		item = {
            			LecturerName:this.modelInformation.LecturerName,
					//Post:this.modelInformation.Post.toString(),
					Post:this.modelInformation.Post,
					HotelName:this.modelInformation.HotelName,
					HeadPicUrl:this.modelInformation.HeadPicUrl,
            		}
            		if (this.isAddTeacher) {//新增
            			item.CuInterId = this.CuInterId
            			item.LecturerId = '0'//讲师id  新增的讲师默认为0
            			this.cardList.push(item)
            		} else{//编辑
            			item.CuInterId = self.cardList[self.selectTeacherIndex].CuInterId  //活动id
            			item.LecturerId = self.cardList[self.selectTeacherIndex].LecturerId //讲师id
            			self.cardList[self.selectTeacherIndex] = item
            			
            		}
            		console.log(item)
            		this.modelInformation.LecturerName=''
            		//this.modelInformation.Post=[]
            		this.modelInformation.Post='',
            		this.modelInformation.HotelName=''
            		this.modelInformation.HeadPicUrl='../../../../../static/image/HeadPortrait.png'
            	
            },
            editTeacher(item,index){//编辑按钮
				this.isAddTeacher = false;//设置为编辑状态
				
				this.selectTeacherIndex = index;//编辑是用
				this.modelInformation.HeadPicUrl = item.HeadPicUrl;
				//this.modelInformation.Post = item.Post.split(",");
				this.modelInformation.Post = item.Post
				this.modelInformation.LecturerName = item.LecturerName;
				this.modelInformation.HotelName = item.HotelName;
				this.modal=true
				
            		//console.log(item)
            },
            deletTeacher(item){//删除
           	 	//
           	 	//:8003/api/Lecturer/DelLecturer
           	 	var self =this
           	 	if (item.LecturerId) {//数据库存在的
           	 		var DATA = '{"LecturerId":"'+item.LecturerId+'","CuInterId":"'+item.CuInterId+'"}'
           	 		$.ajax({
						type:"post",
						url:URLHeader.ECActivities+'/api/Lecturer/DelLecturer',
						contentType:'application/json; charset=utf-8',
						data:DATA,
						async:true,
						success:function(json){
							//var dataAll = JSON.parse(json);
								if (json=='OK') {
									self.cardList.splice(this.cardList.indexOf(item), 1)
									//self.$Message.success('数据删除成功');
									self.$Message.success({
										content:'数据删除成功',
										duration: 4
									});
								} else{
									//self.$Message.error('数据删除失败');
									self.$Message.error({
										content:'数据删除失败',
										duration: 4
									});
								}
		
							console.log(json)
						},
						error:function(error){
							console.log(error)
							//self.$Message.error('数据删除异常');
							self.$Message.error({
								content:'数据删除异常',
								duration: 4
							});
						}
					});
           	 	} else{//新增的讲师
           	 		this.cardList.splice(this.cardList.indexOf(item), 1)
           	 	}
           	 	console.log(item)
            },
            saveData(){//保存
            	//:8003/api/Lecturer/AddCulLecturerSingle
	            var self =this
	            var url = ''
	            if (self.information.CurriculumType=='1') {//图片
	            		if (self.information.imgUrl ==='../../../../../static/image/placeholderBag.png') {
	            			url = ''
	            		}else{
	            			url = self.information.imgUrl
	            		}
	            		
	            } else{//视频
	            		url = self.information.vidoUrl
	            }
	            //cardList
	            var LecturerList = JSON.stringify(self.cardList)
	            self.information.content = self.information.content.replace(/\"/g,"'");//把双引号改为单引号
	            var DATA = '{"Cultivate":{"CuInterId":"'+self.CuInterId+'","CurriculumName":"'+self.information.CurriculumName+'","CurriculumType":"'+self.information.CurriculumType+'","Url":"'+url+'","Priority":"'+self.information.Priority+'","IsDisplay":"'+self.information.IsDisplay+'","Introduce":"'+self.information.content+'","UpdatePerson":"'+self.UpdatePerson+'"},"Lecturers":'+LecturerList+'}'
				console.log(DATA)
				$.ajax({
					type:"post",
					url:URLHeader.ECActivities+'/api/Lecturer/UpdateCultivateLecturer',
					contentType:'application/json; charset=utf-8',
					data:DATA,
					async:true,
					success:function(json){
						//var dataAll = JSON.parse(json);
							if (json=='OK') {
								//self.$Message.success('数据保存成功');
								self.$Message.success({
									content:'数据保存成功',
									duration: 4
								});
								self.saveSucceed = false//设置后  禁止重复保存
							} else{
								//self.$Message.error('保存失败,请确认信息填写完整');
								self.$Message.error({
									content:'保存失败,请确认信息填写完整',
									duration: 4
								});
							}
	
						console.log(json)
					},
					error:function(error){
						console.log(error)
						//self.$Message.error('保存异常');
						self.$Message.error({
							content:'保存异常',
							duration: 4
						});
					}
				});
//	           if (self.saveSucceed) {//保存成功

//	           } else{//重复保存提示
//	           		self.$Message.warning('数据已经保存成功,请勿重复保存,若编辑请前往编辑页面');
//	           }
	           
	          
            	
            },
            requestActive(){//获取活动数据
            	//:8003/api/Lecturer/GetCultivate?cu={cu}
            	var self= this
	            	$.ajax({
					type:"GET",
					url:URLHeader.ECActivities+'/api/Lecturer/GetLecturerByCuInterId?cuId='+self.CuInterId,
					contentType:'application/json; charset=utf-8',
					async:true,
					success:function(json){
						var dataAll = JSON.parse(json);
						var Cultivate = dataAll.Cultivate//基本信息
						//基本信息
						self.information.CurriculumType =Cultivate.CurriculumType.toString()//设置类型展示
						self.information.CurriculumName = Cultivate.CurriculumName//课程名字
						if (self.information.CurriculumType ==1) {//封面
							self.information.imgUrl=Cultivate.Url
						} else{//视频地址
							self.information.vidoUrl=Cultivate.Url
						}
						self.information.Priority = Cultivate.Priority//排序
						self.information.IsDisplay = Cultivate.IsDisplay.toString() //状态
						self.information.content = Cultivate.Introduce//课程内容
						//讲师数据
						var Lecturer = dataAll.Lecturer//讲师数据源
						self.cardList = Lecturer
						
						
						
						
						$('.loding').hide()
						console.log(dataAll)
					},
					error:function(error){
						console.log(error)
						//self.$Message.error('保存异常');
						self.$Message.error({
							content:'保存异常',
							duration: 4
						});
					}
				});
            }
		},
		mounted:function(){
			this.information.CurriculumType =1
			$('.TrainingCommunicationAdd .content .left .ivu-tabs-bar').css({
				width:'15rem'
			})
			this.CuInterId= window.location.hash.split('=')[1];
			
			this.requestActive()
		}
	}
</script>
<style>
	.TrainingCommunicationAdd  .rtf-content .ql-video{
	/*富文本视频连接隐藏*/
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
.TrainingCommunicationAdd{
		width: 65rem;
		overflow: auto;
}
.TrainingCommunicationAdd .content{
	overflow: auto;
}
.TrainingCommunicationAdd .content .left, .TrainingCommunicationAdd .content .right{
	float: left;
	margin-left: 1rem;
	width: 36rem;
	
}
.TrainingCommunicationAdd .content .right{
	width: 24rem;
}
.TrainingCommunicationAdd .content .left ul{
	overflow: auto;
	margin-left: 5rem;
	height: 60rem;
}
.TrainingCommunicationAdd  .content.left ul li {
	display: block;
	float: left;
	width: 28rem;
	margin: 0.5rem auto;
}
.TrainingCommunicationAdd .content .left .rtf-content {
	display: block;
	/*float: left;*/
	width: 30rem;
	margin-top: 1rem;
}
/**/
.TrainingCommunicationAdd .content .right ul{
	overflow: auto;
}
.TrainingCommunicationAdd .content .right ul li{
	margin: 0.5rem auto;
	display: block;
	overflow: auto;
}
.TrainingCommunicationAdd .content .right ul li .card{
	width: 24rem;
	height: 7rem;
	border:1px solid gray;
	border-radius:4px ;
}
.TrainingCommunicationAdd .content .right ul li .card .lable{
	margin-right: 0.5rem;
}

</style>
