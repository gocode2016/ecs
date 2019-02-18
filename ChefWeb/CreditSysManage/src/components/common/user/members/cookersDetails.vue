<template>
	<div>
		<Menu mode="horizontal" active-name="1"  @on-select='selectHeader'>
            <div class="layout-assistant">
            	暂时弃用
                <Menu-item name="1" >基本信息</Menu-item>
                <!--<Menu-item name="2" v-if="isShowHeader">订单</Menu-item>
                <Menu-item name="3" v-if="isShowHeader">积分明细</Menu-item>-->
                <Menu-item name="4" v-if="isShowHeader">酒店轨迹</Menu-item>
                <Menu-item name="5" v-if="isShowHeader">其它信息</Menu-item>
            </div>
        </Menu>
        <Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
        <div class="content">
            <div class="informationContent headerdiv">
            		<img  :src="infomation.ImgUrl" style="max-width: 6rem;margin-left: 4rem;"/>
        			<ul class="top_ul" style="margin-left: 16rem;">
        				<li>
        					<span class="lable">姓名</span><span class="value" style="margin-left: 3rem;">{{infomation.MemberName}}</span> 
        					
        				</li>
        				<li>
        					<span class="lable" style="">绑定队员</span><span class="value" style="margin-left: 1.2rem;">{{infomation.Name}}</span>
        				</li>
        				<li>
        					<span class="lable">厨师ID</span><span class="value">{{infomation.MemberId}}</span>
        				</li>
        				<li>
        					<span class="lable">手机号</span><span class="value">{{infomation.MemberTelePhone}}</span>
        				</li>
        			</ul>
        			
        			<!--<br />-->
        			<h1></h1>
        			<ul class="bottom_ul">
        				<li><span class="lable">所在大区</span><span class="value"></span></li>
        				<li><span class="lable">注册时间</span><span class="value">{{infomation.RegistDate}}</span></li>
        				<li><span class="lable">酒店区域</span><span class="value">{{infomation.ProvinceName}}/{{infomation.CityName}}/{{infomation.AreaName}}</span></li>
        				<li><span class="lable">工作岗位</span><span class="value">{{infomation.PositionType}}/{{infomation.Position}}</span></li>
        				<li><span class="lable">酒店地址</span><span class="value">{{infomation.HotelAddress}}</span></li>
        				<!--<li><span class="lable">状态</span><span class="value" style="margin-left: 2.8rem;">{{infomation.MemberState}}</span></li>-->
        				<li><span class="lable">活跃度</span><span class="value" style="margin-left: 2rem;">活跃度数据</span></li>
        				<li><span class="lable">酒店名称</span><span class="value">{{infomation.HotelName}}</span></li>
        			<!--	<li><span class="lable">生日</span><span class="value" style="margin-left: 2.8rem;"></span></li>-->
        				<li><span class="lable">账号状态</span><span class="value" style="margin-left: 1.2rem;">账号状态数据</span></li>
        				<li><span class="lable">酒店编码</span><span class="value">{{infomation.HotelCode}}</span></li>
        				<li><span class="lable">分类</span><span class="value" style="margin-left: 3rem;">未认证数据</span></li>
        				<li><span class="lable">当前积分</span><span class="value" style="margin-left: 1.2rem;">{{infomation.LeaveIntegral}}</span></li>
        				<li><span class="lable">备注信息</span><span class="value">{{infomation.Remark}}</span></li>  
        				<li style="width: 100%;text-align: right;">
        					<Button type="primary" style="margin-right: 0.8rem;" @click='qualified' v-if='isShowQualified'>合格</Button>
        					<Button type="primary" style="margin-right: 0.8rem;" @click='editorCooker'>编辑</Button>
        				</li>  
        			</ul>
            		
            </div>
             <div class="orderContent headerdiv">
            	2
            </div>
            <div class="integralContent headerdiv">
            	3
            </div>
            <div class="hotelContent headerdiv">
            		<Table style="margin-top: 1rem;" :columns="hotelList" :data="hotel_data"></Table>
            </div>
            <div class="otherContent headerdiv" style="display: none;">
            		<ul>
            			<li>
            				<span class="lable" >性别:</span>
            				<span class="value" style="margin-left: 3.7rem;">{{otherData.Sex}}</span>
            			</li>
            			<li>
            				<span class="lable">擅长菜系:</span>
            				<span class="value" style="margin-left: 4.8rem;">{{otherData.ExpertInCuisine}}</span>
            			</li>
            			<li>
            				<span class="lable">民族:</span>
            				<span class="value" style="margin-left: 3.7rem;">{{otherData.Nation}}</span>
            			</li>
            			<li>
            				<span class="lable">街道门牌:</span>
            				<span class="value" style="margin-left: 4.8rem;">{{otherData.HomeAddress}}</span>
            			</li>
            			<li>
            				<span class="lable">性格:</span>
            				<span class="value" style="margin-left: 3.7rem;">{{otherData.Nature}}</span>
            			</li>
            			<li>
            				<span class="lable">业余爱好:</span>
            				<span class="value" style="margin-left: 4.8rem;">{{otherData.Hobbys}}</span>
            			</li>
            			<li>
            				<span class="lable">师承:</span>
            				<span class="value" style="margin-left: 3.7rem;">{{otherData.Teacher}}</span>
            			</li>
            			<li>
            				<span class="lable">食材选择要求:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.FoodDemand}}</span>
            			</li>
            			<li>
            				<span class="lable">学历:</span>
            				<span class="value" style="margin-left: 3.7rem;">{{otherData.Educations}}</span>
            			</li>
            			<li>
            				<span class="lable">菜品成功原因:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.SuccessReasons}}</span>
            			</li>
            			<li>
            				<span class="lable">生日:</span>
            				<span class="value"  style="margin-left: 3.7rem;">{{otherData.Birthday}}</span>
            			</li>
            			<li>
            				<span class="lable">从事行业原因:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.JobReason}}</span>
            			</li>
            			<li>
            				<span class="lable">入行时间:</span>
            				<span class="value">{{otherData.JoinDate}}</span>
            			</li>
            			<li>
            				<span class="lable">个人职业发展:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.Development}}</span>
            			</li>
            			<li>
            				<span class="lable">家庭地址:</span>
            				<span class="value adress">{{otherData.ProvinceName}}/{{otherData.CityName}}/{{otherData.AreaName}}</span>
            			</li>
            			<li>
            				<span class="lable">希望考察城市:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.WishCity}}</span>
            			</li>
            			<li>
            				<span class="lable">决策权限:</span>
            				<span class="value">{{otherData.ChoicePower}}</span>
            			</li>
            			<li>
            				<span class="lable">自由支配时间:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.IsCtrlTime}}</span>
            			</li>
            			<li>
            				<span class="lable">代表菜品:</span>
            				<span class="value">{{otherData.Represent}}</span>
            			</li>
            			<li>
            				<span class="lable">是否乐于分享:</span>
            				<span class="value" style="margin-left: 3rem;">{{otherData.IsShare}}</span>
            			</li>
            			<li>
            				<span class="lable">工作特长:</span>
            				<span class="value">{{otherData.Speciality}}</span>
            			</li>
            			<li>
            				<span class="lable">多城市从业经验:</span>
            				<span class="value">{{otherData.IsManyExp}}</span>
            			</li>
            		</ul>
            </div>
        </div>
        
	</div>
</template>

<script>
	export default{
		data() {
			return {
				isShowHeader:false,//是否显示基本信息之外的
				isShowQualified:false,//合格按钮
				MemberId:'',//记录跳转进来的MemberId
				infomation:{//厨师详细信息
				},
				URL:{
					qualified:URLHeader.user+'/api/Member/ReviewMember',//合格链接
					infomation:URLHeader.user+'/api/Member/GetMemberById',//基本信息
					hotel:URLHeader.user+'/api/Member/GetMemberResume',//酒店信息
					other:URLHeader.user+'/api/MemberProfile/GetMemberProfile',//其它信息
				},
				//酒店------------------
				hotelList:[
					{	
                        title:'NO.',
//                      width:70,
                        align: 'center',
                        key:'MemberId'
                        
                     },
         			{
                        title: '录入时间',
                        width:180,
                        key: 'UpdateTime',
                        align: 'center',
                    },
                    {
                        title: '酒店省',
                        key: 'ProvinceName',
                         width:80,
                        align: 'center',
                    },
                    {
                        title: '酒店市',
                        align: 'center',
                        width:80,
                        key: 'CityName'
                    },
                    {
                        title: '酒店区',
                        align: 'center',
                         width:80,
                        key: 'AreaName'
                    },
                    {
                        title: '酒店地址',
                        align: 'center',
                         width:120,
                        key: 'HotelAddress',
                        width:120
                    },
                    {
                        title: '酒店名称',
                        align: 'center',
                        key: 'HotelName',
                        width:100
                    },
                     {
                        title: '岗位',
                        align: 'center',
                        key: 'Job',
                        width:100
                    },
                     {
                        title: '酒店编码',
                        align: 'center',
                        key: 'HotelCode',
                         width:90,
                    },
//                   {
//                      title: '操作',
//                      align: 'center',
//                      key: 'operation',
//                      render: (h, params) => {
//                          return h('div', [
//                              h('Button', {
//                                  props: {
//                                      type: 'error',
//                                      size: 'small'
//                                  },
//                                  on: {
//                                      click: () => {
//                                          this.deleteHotel(params)
//                                      }
//                                  }
//                              }, '删除')
//                          ]);
//                      }
//                  }
				],
				hotel_data:[],
				//------------------------------其它------------------------
				otherData:{//其它信息
				},
				
			}
		},
		methods:{//Provence
			selectHeader(name){//头部切换
				name=parseInt(name)
				$('.content .headerdiv').hide();
				switch(name){
					case 1:
					$('.informationContent').show();
					break;
					case 2:
					$('.orderContent').show();
					break;
					case 3:					
					$('.integralContent').show();
					break;
					case 4:
					$('.hotelContent').show();
					$('.hotelContent .ivu-table-header table').css({
						'width':'100%'
					});
					$('.hotelContent .ivu-table-body table').css({
						'width':'100%'
					})
					break;
					case 5:
					$('.otherContent').show();
					break;
				}
			},
			requestCookerInfomation(){//请求基本信息
				var self =this;
				let DATA = '{"MemberId":"'+self.MemberId+'"}'
				$.ajax({
			        type :"post",
			        url :self.URL.infomation,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        
			        	 var dataAll = JSON.parse(json)[0];
			        	 if (dataAll.MemberState=='1') {
			        	 	dataAll.MemberState='已审核'
			        	 } else{
			        	 	dataAll.MemberState='未审核'
			        	 }
			        	 if (!dataAll.Remark) {
			        	 	dataAll.Remark='--';
			        	 }
			        	  if (!dataAll.HotelCode) {
			        	 	dataAll.HotelCode='--';
			        	 }
			        	  if (!dataAll.ImgUrl) {
			        	  		dataAll.ImgUrl='../../../../../static/image/HeadPortrait.png'
			        	  }
			        	  	console.log(dataAll)
			        	 self.infomation=dataAll
			        	 $('.loding').hide();
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			qualified(){//合格按钮
				var self =this;
				let DATA = JSON.stringify([self.MemberId]);
				$.ajax({
			        type :"post",
			        url :self.URL.qualified,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        self.$Message.success('审批完成');
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
				
			},
			editorCooker(){//编辑按钮
				this.$router.push({path: '/cookerEditor', query: {MemberId:this.MemberId}});//跳转厨师编辑页
			},
			//-------------------------------------酒店历史-----------------------------------------------
			deleteHotel(){//删除酒店
				
			},
			requestHotel(){//酒店信息请求hotel
				var self =this;
				let DATA = '{"MemberId":"'+self.MemberId+'"}'
				self.hotel_data = [];
				$.ajax({
			        type :"post",
			        url :self.URL.hotel,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json);
			        	 for (var i = 0;i<dataAll.length;i++) {
			        	 	self.hotel_data.push(dataAll[i])
			        	 }
//					console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			//--------------------------------------其它信息----------------------------------------------
			requestOtherData(){
				var self =this;
				let DATA = '{"MemberId":"'+self.MemberId+'"}'
				$.ajax({
			        type :"post",
			        url :self.URL.other,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	 var dataAll = JSON.parse(json)[0];//adress
//			        	 console.log(json)
			        	 if (dataAll) {
			        	 	if (!dataAll.ProvinceName) {
			        	 	$('.adress').text('');
				        	 } 
				        	 if (dataAll.Sex=1) {
				        	 		dataAll.Sex='男'
				        	 } else{
				        	 	dataAll.Sex='女'
				        	 }
				        	 if (dataAll.IsShare=1) {//是否乐于分享
				        	 		dataAll.IsShare='是'
				        	 } else{
				        	 	dataAll.IsShare='否'
				        	 }
				        	 if (dataAll.IsManyExp=1) {//多年从业经验
				        	 		dataAll.IsManyExp='是'
				        	 } else{
				        	 	dataAll.IsManyExp='否'
				        	 }
				        	 console.log(dataAll)
				        	 self.otherData = dataAll
			        	 }else{
			        	 	$('.adress').text('');
			        	 }

			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });

				
			}
			
		},
		mounted:function(){
					$('.content .headerdiv').hide();
					$('.informationContent').show();
					var hash= window.location.hash.split('cookersDetails?')[1].split('&');
					this.MemberId = hash[0].split('=')[1];
					//isShowQualified
					if (hash[1].split('=')[1]==0) {//0为未审批
						this.isShowHeader = false;//头部滑动
						this.isShowQualified=true;//合格按钮
					} else{
						this.isShowQualified=false;
						this.isShowHeader = true;
					}
					
					this.requestCookerInfomation()
					this.requestHotel();
					this.requestOtherData();
					
					
		}
	}
</script>

<style scoped>
.layout-assistant{
    width: 600px;
   /* margin: 0 auto;*/
   bottom: 10px;
    text-align: left;
    height: 3rem;
    line-height: 3rem;
}
.ivu-menu-horizontal.ivu-menu-light:after{
	bottom: 11px;
}
.content{
	margin-top: 1rem;	
}
.informationContent{
	border: 1px solid gainsboro;
	padding: 1rem;
	border-radius: 9px;
	width: 50rem;
	margin: 0 auto;
	
}
.informationContent ul{
	margin-left: 5rem;
}
.informationContent h1{
	border-bottom: 1px dashed gray;
}
.top_ul{
	width: 40%;
	padding-bottom: 1rem;
	display: inline-block;
	
} 
.top_ul li{
	height: 1.5rem;
	line-height: 1.5rem;
}
.top_ul li .lable{
	font-weight: bold;
}
.top_ul li .value{
	margin-left: 2rem;
}
.bottom_ul{
	margin-top: 0.5rem;
	/*width: 100%;*/
	height: 11rem;
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
span {
	font-size: 0.9rem;
}
/*其它*/
.otherContent ul {
	width: 70%;
	margin: 0 auto;
	border: 1px dashed gainsboro;
	overflow: auto;
	border-radius:8px ;
	/*text-align: center;*/
}
.otherContent ul li {
	height: 2rem;
	line-height: 2rem;
	display: block;
	float: left;
	width: 50%;
}
.otherContent ul li .lable{
	margin-left: 3rem;
	font-weight: bold;
}
.otherContent ul li .value{
	margin-left: 2rem;
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