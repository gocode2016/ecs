<template>
	<div>
		<h2 style="text-align: center;margin-bottom: 1rem;">厨师信息编辑</h2>
		<div class="cookerEditor">
			<Modal
		        v-model="ModalShow"
		        title="更改绑定队员"
		        @on-ok="Confirm_transfer">
		        <div>
		        		<Input v-model="searchPlayer" placeholder="请输入要转入队员工号" style="width: 200px"></Input>
		        		<Button type="primary" style="margin-left: 1rem;" icon="ios-search" @click='palyerSeach'>搜索</Button>
		        </div>
				<Table style="margin-top: 1rem;" :columns="serch_player"    :data="search_player_data"></Table>
		    </Modal>
			<Spin fix class='loding'>
	            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
	            <div>Loading</div>
	        </Spin>
	        <div style="overflow: auto;margin-bottom: 1rem;">
		        <div style="float: left;margin-left: 2rem;">
					<img :src="cookerInformation.ImgUrl" style="max-width:5rem;max-height: 15rem;"/>
					<Upload
				        ref="upload"
				        :show-upload-list="false"
				        :on-success="handleHeaderImageSuccess"
				        :format="['jpg','jpeg','png']"
				        :max-size="2048"
				        :before-upload='handleBeforeUpload'
				        :on-format-error="handleFormatError"
				        :on-exceeded-size="handleMaxSize"
				        :multiple=false
				        type="drag"
				        :action=UPImageMember
				        style="display:block;width:80px;margin:0 auto;" >
				         <Button type="primary">选择大图</Button>
				    </Upload>
				</div>
				<ul class="topUl">
					<li>
						<span class="lable">姓名</span>
						<Input v-model='cookerInformation.MemberName' class="input" placeholder="请输入姓名" style="width: 180px;margin-left: 0.8rem;"></Input>
					</li>
					<li style="margin-right: 1rem;">
						<span class="lable">手机号</span>
						<Input v-model='cookerInformation.MemberTelePhone' class="input" placeholder="请输入手机号" style="width: 180px;margin-left: 0;"></Input>
					</li>
					<li style="margin-right: 1rem;">
						<span class="lable">会员ID</span>
						<span style="margin-left: 1.3rem;">{{infomation.MemberId}}</span>
					</li>
					<li class="Tourist">
						<span>当前业务代表</span>
						<span style="margin-left: 1.6rem;cursor: pointer;color: red;" @click="playersBtn(cookerInformation.RecommendId)">{{cookerInformation.Name}}</span>
						<Button style="margin-left: 2rem;margin-top: -5px;" type="primary" @click='transferShow'>转移</Button>
					</li>

				</ul>
				<ul class="topUl">
					<li>
						<span class="labie" style="display: block;float: left;">酒店区域</span>
						<Cascader class='area' :data="areaData" v-model="hotel_Area" style="width: 180px;display: block;float: left;margin-left: 1rem;"></Cascader>
					</li>
					<li style="margin-right: 1rem;">
						<span class="lable">酒店地址</span>
						<Input v-model='cookerInformation.HotelAddress' class="input" placeholder="请输入酒店地址" style="width: 180px;margin-left: 0.8rem;"></Input>
					</li>
					<li style="margin-right: 1rem;">
						<span class="lable">酒店名称</span>
						<Input v-model='cookerInformation.HotelName' class="input" placeholder="请输入酒店名称" style="width: 180px;margin-left: 0.8rem;"></Input>
					</li>
					<li style="width: 100%;">
						<span class="lable" style="display: block;float: left;">工作岗位</span>
						<Cascader :data="jobsData" v-model="jobs" style="width: 180px;float: left;margin-left: 1.1rem;"></Cascader>
					</li>

				</ul>
			</div>

			<h1></h1>
			<!--<ul class="bottomUlleft">
				<li>
					<span class="labie">酒店名称</span>
					<Input class="input" v-model='cookerInformation.HotelName' placeholder="请输入酒店名称" style="width: 200px;margin-left: 2.8rem;"></Input>
				</li>

				<li>
					<span class="labie">酒店编码</span>
					<Input class="input" v-model='cookerInformation.HotelCode' placeholder="酒店编码" style="width: 200px;margin-left: 2.8rem;"></Input>
				</li>
				<li>
					<span class="labie" style="display: block;float: left;">酒店区域</span>
					<Cascader class='area' :data="areaData" v-model="cookerInformation.area" style="width: 200px;display: block;float: left;"></Cascader>
				</li>

			</ul>
			<ul class="bottomUlright">
				<li style="width: 100%;">
					<span class="lable" style="display: block;float: left;">工作岗位</span>
					<Cascader :data="jobsData" v-model="cookerInformation.jobs" style="width: 200px;float: left;margin-left: 1.5rem;"></Cascader>
				</li>
				<li><span>备注信息</span><Input  v-model="cookerInformation.Remark" type="textarea" :rows="4" style="width: 200px;margin-left: 1.5rem;" placeholder="请输入..."></Input></li>

			</ul>-->
			<ul class="bottom_ul">
    				<li><span class="lable">所在大区</span><span class="value">{{cookerInformation.RegionName}}</span></li>
        				<li><span class="lable">注册时间</span><span class="value">{{infomation.RegistDate}}</span></li>
        				<li><span class="lable">注册来源</span><span class="value">-------</span></li>
        				<li><span class="lable">队员工号</span><span class="value">{{cookerInformation.WorkNum}}</span></li>
        				<li><span class="lable">客户编码</span><span class="value">{{infomation.MemberCode}}</span></li>
        				<li><span class="lable">队员手机号</span><span class="value">{{cookerInformation.Telephone}}</span></li>
					<li><span class="lable">客户编码完善时间</span><span class="value">{{infomation.MemberCodeTime}}</span></li>
					<li><span class="lable">分类</span><span class="value">{{infomation.RecommendId}}</span></li>
					<li><span class="lable">活跃度</span><span class="value">{{infomation.MemberActiveState}}</span></li>
        				<li><span class="lable">擅长菜系</span><span class="value">{{infomation.ExpertInCuisine}}</span></li>
        				<li><span class="lable">会员状态</span><span class="value">{{infomation.MemberState}}</span></li>
        				<li><span class="lable">是否乐于分享</span><span class="value">{{infomation.IsShare}}</span></li>
        				<li><span class="lable">从事行业原因</span><span class="value">{{infomation.JobReason}}</span></li>
        				<li><span class="lable">生日</span><span class="value">{{infomation.Birthday}}</span></li>
        				<li><span class="lable">食材选择要求</span><span class="value">{{infomation.FoodDemand}}</span></li>
        				<li><span class="lable">性别</span><span class="value">{{infomation.Sex}}</span></li>
        				<li><span class="lable">入行时间</span><span class="value">{{infomation.JoinDate}}</span></li>
        				<li><span class="lable">民族</span><span class="value">{{infomation.Nation}}</span></li>
        				<li><span class="lable">厨龄</span><span class="value">{{infomation.KitchenAge}}年</span></li>
        				<li><span class="lable">家庭区域</span><span class="value">{{infomation.HomeProvinceName}}/{{infomation.HomeCityName}}/{{infomation.HomeAreaName}}</span></li>
        				<li><span class="lable">性格</span><span class="value">{{infomation.Nature}}</span></li>
        				<li><span class="lable">家庭地址</span><span class="value">{{infomation.HomeAddress}}</span></li>
        				<li><span class="lable">业余爱好</span><span class="value">{{infomation.Hobbys}}</span></li>
        				<li><span class="lable">是否多城市从业经验</span><span class="value">{{infomation.IsManyExp}}</span></li>
        				<li><span class="lable">相关工作特长</span><span class="value">{{infomation.Speciality}}</span></li>
        				<li><span class="lable">希望考察城市</span><span class="value">{{infomation.WishCity}}</span></li>
        				<li><span class="lable">学历</span><span class="value">{{infomation.Educations}}</span></li>
        				<li><span class="lable">个人代表菜品</span><span class="value">{{infomation.Represent}}</span></li>
        				<li><span class="lable">决策权限</span><span class="value">{{infomation.ChoicePower}}</span></li>
        				<li><span class="lable">菜品成功原因</span><span class="value">{{infomation.SuccessReasons}}</span></li>
        				<li><span class="lable">是否自由支配时间</span><span class="value">{{infomation.IsCtrlTime}}</span></li>
        				<li><span class="lable">截至目前正常扫描产品获得的积分</span><span class="value">------</span></li>
        				<li><span class="lable">师承</span><span class="value">{{infomation.Teacher}}</span></li>
        				<li><span class="lable">当前积分</span><span class="value">{{infomation.LeaveIntegral}}</span></li>
        				<li><span class="lable">个人职业发展</span><span class="value">{{infomation.Development}}</span></li>
        				<li><span class="lable">累计增加总积分</span><span class="value">{{infomation.TotalIntegral}}</span></li>
        				<!--<li><span class="lable">备注信息</span><span class="value">{{infomation.Remark}}</span></li>-->
        				<li><span class="lable">完善信息时间</span><span class="value">{{infomation.ProfileTime}}</span></li>
        				<li><span class="lable">普通话水平</span><span class="value">{{infomation.ChineseLv}}</span></li>
        				<li><span class="lable">调味品采购权</span><span class="value">{{infomation.Purchaser}}</span></li>
        				<li><span class="lable">毕业院校</span><span class="value">{{infomation.School}}</span></li>
        				<li><span class="lable">婚姻状况</span><span class="value">{{infomation.IsMarry}}</span></li>
        				<li><span class="lable">专业</span><span class="value">{{infomation.Major}}</span></li>
        				<li><span class="lable">备注信息</span><span class="value">{{infomation.Remark}}</span></li>
        				<li><span class="lable">在职情况</span><span class="value">{{infomation.IsJob}}</span></li>
        				<li><span class="lable">自我介绍</span><span class="value">{{infomation.Introduction}}</span></li>
    			</ul>
			<Button type="primary" size="large" @click='saveCookerInfomation' style="display: block;margin: 1rem auto;">保存</Button>
		</div>
	</div>
</template>

<script>
	export default{
		data() {
			return {
				UPImageMember:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Member',
				infomation:{},//厨师详细信息
//				playerInformation:[//部分队员信息
//				],
				areaData:[],//区域数据
				ModalShow:false,//转移弹框
				searchPlayer:'',//转移搜索框
				MemberId:'',//厨师id
//				city :[],//市
//				zone:[],//区
//				province:[],//省
//				cookerInformation:{//厨师信息列表
//					area:[],//区域存储
//					jobs:[],//岗位存储
//				},
				hotel_Area:[],//酒店区域
				jobs:[],//岗位
				cookerInformation:{

				},
				URL:{
					infomation:URLHeader.user+'/api/MemberProfile/GetMemberProfile',//基本信息
					searchPlayer:URLHeader.user+'/api/SaleMan/GetSaleManById',//转移  查询队员
					saveInformation:URLHeader.user+'/api/Member/UpdateMember',//保存信息接口
					TransferMember:URLHeader.user+'/api/SaleMan/TransferMember',//转移动作
					player:URLHeader.user+'/api/Member/GetMemberById',//一些队员信息
				},
				serch_player:[//转移弹框表头
                     {
                        title:'姓名',

                        align: 'center',
                        key:'Name'
                     },
                     {
                        title:'工号',

                        align: 'center',
                        key:'WorkNum'
                     },
         		],
				search_player_data:[//转移弹框数据

         		],
				jobsData:[//岗位列表
					{
						value: '总厨',
                   		label: '总厨',
                   		children:[
                   		 	  {
						        value: '行政总厨',
						        label: '行政总厨',
						      },
                   			  {
						        value: '行政副总厨',
						        label: '行政副总厨',
						      }, {
						        value: '宴会厨师长',
						        label: '宴会厨师长',
						      }, {
						        value: '中餐行政总厨',
						        label: '中餐行政总厨',
						      }, {
						        value: '中餐厨师长',
						        label: '中餐厨师长',
						      }, {
						        value: '中餐行政副总厨',
						        label: '中餐行政副总厨',
						      }, {
						        value: '西餐行政总厨',
						        label: '西餐行政总厨',
						      }, {
						        value: '西餐行政副总厨',
						        label: '西餐行政副总厨',
						      },
						      {
						        value: '西餐厨师长',
						        label: '西餐厨师长',
						      },{
						        value: '日餐厨师长',
						        label: '日餐厨师长',
						      }, {
						        value: '韩餐厨师长',
						        label: '韩餐厨师长',
						      },
                   		]
					},
					{
						value: '研发出品',
                   		label: '研发出品',
                   		children:[
                   		  {
					        value: '研发总监',
					        label: '研发总监',
					      },
					      {
					        value: '研发副总监',
					        label: '研发副总监',
					      },
					      {
					        value: '研发工程师',
					        label: '研发工程师',
					      },
					      {
					        value: '出品总监',
					        label: '出品总监',
					      },
                   		]
					},
					{
						value: '后厨主管',
                   		label: '后厨主管',
                   		children:[
                   		  {
					        value: '冷菜主厨',
					        label: '冷菜主厨',
					      },
					      {
					        value: '热菜主厨',
					        label: '热菜主厨',
					      },
					      {
					        value: '点心主厨',
					        label: '点心主厨',
					      },
					      {
					        value: '糕点主厨',
					        label: '糕点主厨',
					      },
					      {
					        value: '菜系主厨',
					        label: '菜系主厨',
					      },
					      {
					        value: '宴会厨师长',
					        label: '宴会厨师长',
					      },
                   		]
					},
					{
						value: '普通厨师',
                   		label: '普通厨师',
                   		children:[
                   			  {
						        value: '炉灶',
						        label: '炉灶',
						      },
						      {
						        value: '切配',
						        label: '切配',
						      },
						      {
						        value: '打荷',
						        label: '打荷',
						      },
						      {
						        value: '烧味师',
						        label: '烧味师',
						      },
						      {
						        value: '上什',
						        label: '上什',
						      },
                   		]
					},
					{
						value: '老板采购',
                   		label: '老板采购',
                   		children:[
                   			  {
						        value: '采购',
						        label: '采购',
						      },
						      {
						        value: '总经理',
						        label: '总经理',
						      },
						      {
						        value: '董事长',
						        label: '董事长',
						      },
                   		]
					}
				],

			}
		},
		methods:{//Provence
			playersBtn(name){//队员点击
					console.log(name)
					this.$router.push({path: '/playersDetails',query: {SalemanId:name}});//队员详情
			},
			palyerSeach(){//转移搜索按钮
				var self = this;
				let DATA ='{"WorkNum":"'+self.searchPlayer+'"}';
				self.search_player_data=[]
				$.ajax({
			        type :"post",
			        url :self.URL.searchPlayer,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 for (var i = 0;i<dataAll.length;i++) {
			        	 	self.search_player_data.push(dataAll[i])
			        	 }
			        	 $('.loding').hide()
			        	 console.log(dataAll)
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });

			},
			Confirm_transfer(){//转移确认按钮
				var self =this;
				if (self.search_player_data.length>0) {
					var arr =[parseInt(self.MemberId)];
					let DATA=JSON.stringify(arr)
					console.log('转移数据'+DATA)
					console.log(self.search_player_data[0].SalemanId)
					$.ajax({
				        type :"post",
				        url :self.URL.TransferMember+'?salemanId='+self.search_player_data[0].SalemanId,
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",
				        data:DATA,
				        success : function(json) {
				        	 console.log(json)
				        	 self.$Message.success('数据转移成功');
				        	 self.requestCookerInformation();
				        },
				        error : function(error) {
				        		console.log(error)
				        		self.$Message.error('数据转移错误');
				        }
		   		    });

				} else{
					self.$Message.warning('请输入正确工号并搜索');
				}
			},
			transferShow(){//迁移按钮
				this.ModalShow=true;
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
             handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
         	},
	         handleHeaderImageSuccess(res, file){//头像上传
	         	 $('.loding').hide()
	                console.log(file)
	                var data = JSON.parse(res);
	                this.cookerInformation.ImgUrl = URLHeader.Loading+'/Member/'+data.Id+'.'+data.Type;
	         },
	         p(s) {
				return s < 10 ? '0' + s: s;
			},
			saveCookerInfomation(){
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

			var now=year+'-'+this.p(month)+"-"+this.p(date)+" "+this.p(h)+':'+this.p(m)+":"+this.p(s);


				var self =this;
				//省市区
				//console.log(self.hotel_Area)
				var area =$('.area input').val().split(' / ');
				self.cookerInformation.ProvinceName=area[0];//省
				self.cookerInformation.CityName=area[1];//市
				self.cookerInformation.AreaName=area[2];//区

				self.cookerInformation.ProvinceId=self.hotel_Area[0];//省
				self.cookerInformation.CityId=self.hotel_Area[1];//市
				self.cookerInformation.AreaId=self.hotel_Area[2];//区
//				//岗位
				self.cookerInformation.PositionType = self.jobs[0];//一级
				self.cookerInformation.Position = self.jobs[1];//二级
				//日期
				self.cookerInformation.UpdateDate = now
				var DATA=JSON.stringify(self.cookerInformation);
				//头像 HeadPortrait
				if (self.cookerInformation.ImgUrl==="../../../../../static/image/HeadPortrait.png") {
					self.cookerInformation.ImgUrl=null
				}
				console.log(self.cookerInformation)
				$.ajax({
			        type :"post",
			        url :self.URL.saveInformation,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {

			        	 if (json =='1') {
			        	 	self.$Message.success('数据保存完成');
			       	 	 self.requestCookerInformation();
			        	 } else{
			        	 	self.$Message.error('数据保存异常,请重新保存');
			        	 }
			        },
			        error : function(error) {
			        		console.log(error)
			        		self.$Message.error('数据保存异常,请确保网络畅通');
			        }
	   		   });
			},
			requestCookerInformation(){
				var self =this;
				let DATA = '{"MemberId":"'+self.MemberId+'"}'
				$.ajax({
			        type :"post",
			        url :self.URL.infomation,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {// 1 为家庭

			        	 var dataAll = JSON.parse(json)[0];
			        	 // dataAll.ImgUrl='../../../../../static/image/HeadPortrait.png'
		        	  	//console.log(JSON.parse(json)[0])
		        	  	if (dataAll.RecommendId>0) {
		        	  		dataAll.RecommendId='已认证'
		        	  	} else{
		        	  		dataAll.RecommendId='未认证'
		        	  	}
		        	  	if (dataAll.IsJob=1) {//在职情况
			        	  		dataAll.IsJob = '在职'
		        	  	}else{
		        	  		dataAll.IsJob = '离职'
		        	  	}
		        	  	if (dataAll.IsMarry=1) {//婚姻情况
		        	  		dataAll.IsMarry = '已婚'
		        	  	}else{
		        	  		dataAll.IsMarry = '未婚'
		        	  	}
		        	  	if (dataAll.IsShare=1) {//是否乐于分享
		        	  		dataAll.IsShare = '是'
		        	  	}else{
		        	  		dataAll.IsShare = '否'
		        	  	}
		        	  	if (dataAll.Sex=1) {//性别
		        	  		dataAll.Sex = '男'
		        	  	}else{
		        	  		dataAll.Sex = '女'
		        	  	}
		        	  	if (dataAll.IsManyExp=1) {//多城市从业经验
		        	  		dataAll.IsManyExp = '有'
		        	  	}else{
		        	  		dataAll.IsManyExp = '无'
		        	  	}
		        	  	if (dataAll.MemberActiveState==1) {//活跃度搞
     					dataAll.MemberActiveState='高'
     				} else{//2低
     					dataAll.MemberActiveState='低'
     				}
					//厨龄
					dataAll.KitchenAge=new Date().getFullYear()-new Date(dataAll.JoinDate).getFullYear()+1;
			        	 self.infomation=dataAll
			        	 	if (	self.infomation.Name==null) {//游客无业务代表
				        		$('.Tourist').hide()
				        	}
			        //	 console.log( self.infomation)
			        	 $('.loding').hide();
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });

	   		    //player
	   		    $.ajax({
			        type :"post",
			        url :self.URL.player,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {// 1 为家庭
			        self.hotel_Area = []//酒店区域清空
			        	 var dataAll = JSON.parse(json)[0];
			         self.hotel_Area.push(dataAll.ProvinceId)
			         self.hotel_Area.push(dataAll.CityId)
			         self.hotel_Area.push(dataAll.AreaId)
			         self.jobs = []//岗位清空
			         self.jobs.push(dataAll.PositionType)
			          self.jobs.push(dataAll.Position)
			          //若无头像
  		        	  	if (dataAll.ImgUrl==null) {
		        	  		dataAll.ImgUrl = '../../../../../static/image/HeadPortrait.png'
		        	  	}
			        	  self.cookerInformation = dataAll
					console.log(JSON.parse(json)[0])
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
			},

		},
		mounted:function(){

				this.areaData=Provence;//区域数据
				//console.log(this.areaData)
				var hash= window.location.hash.split('cookerEditor?')[1].split('&');
					this.MemberId = hash[0].split('=')[1];
					this.requestCookerInformation();
				//	this.requestArea()
		}
	}
</script>

<style scoped>
	span{
		font-size: 1rem;
	}
.cookerEditor{
	border: 1px solid gainsboro;
	padding: 1rem;
	/*height: 24rem;*/
	width: 50rem;
	margin: 0 auto;
	border-radius:10px ;
}
.topUl{
		padding-bottom: 0rem;
		overflow: auto;
		width: 18rem;
		margin-left: 2rem;
		display: block;
		float: left;
	}
.topUl li{
	display: block;
	/*float: left;*/
	/*width: 40%;*/
	height: 2.2rem;
	line-height: 2.2rem;
	margin-top: 0.3rem;
	/*margin-left: 3rem;*/
}
.topUl li .lable{
	/*margin-left: 1rem;*/
}
.topUl li .input{
	margin-left: 2rem;
}
h1{
	border: 1px dashed gainsboro;
}
.bottomUlleft,.bottomUlright{
	margin-top: 1rem;
	width: 50%;
	display: block;
	float: left;

}
.bottomUlleft li ,.bottomUlright li{
	margin-left: 3rem;
	margin-top: 1rem;
	height: 2.2rem;
	line-height: 2.2rem;
}
.bottomUlright li .labie{
	display: block;
	float: left;
}
.bottom_ul{
	margin: 0 auto;
	margin-top: 0.5rem;
	width: 90%;
	overflow: auto;
}
.bottom_ul li{
	float: left;
	width: 50%;
	height: 1.8rem;
	line-height: 1.8rem;
}
.bottom_ul li .lable{
	font-weight: bold;
}
.bottom_ul li .value{
	margin-left: 1rem;
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
