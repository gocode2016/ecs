<template>
	<div class="playersEditor">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h1>队员编辑</h1>
		<ul>
			<li>
				<label>姓名:</label>
				<Input  class="input" v-model="infomationList.Name" size="large" placeholder="请输入姓名" style="width: 200px"></Input>
			</li>
			<li>
				<label>工号:</label>
				<Input class="input" v-model="infomationList.WorkNum" size="large" placeholder="请输入工号" style="width: 200px"></Input>
			</li>
			<li>
				<label>手机:</label>
				<Input class="input" v-model="infomationList.Telephone" size="large" placeholder="请输入手机号" style="width: 200px"></Input>
			</li>
			<li>
				<label>身份证号:</label>
				<Input class="input" v-model="infomationList.CardId" size="large" placeholder="请输入姓名" style="width: 200px"></Input>
			</li>
			<li>
				<label>注册时间:</label>
				<span class="input" style="margin-right: 5rem;font-size: 0.9rem;">{{infomationList.RegistDate}}</span>
			</li>
			<li>
				<label>所在大区:</label>
				<Select v-model="infomationList.RegionId" style="width:200px" placeholder='大区选择' class="input select">
			        <Option v-for="item in regional_List" :value="item.value" :key="item.value">{{ item.label }}</Option>
			    </Select>
			</li>
			<!--<li style="overflow: auto;height: auto;line-height: none;">
				<label>工作区域:</label>
				<Cascader class='area' :data="areaData"  v-model="hotel_Area" style="width: 197px;display: block;float: left;margin-left: 2.8rem;"></Cascader>
				<div style="width: 280px;float: left;margin-bottom: 1rem;">
					<div v-for= '(item,index) in areaList' :key="item.id" style="overflow: auto;margin-bottom: 0.5rem;">
						<Cascader class='area' :data="areaData"   v-model="item.hotel_Area" style="width: 200px;display: block;float: left;margin-left: 2.8rem;"></Cascader>
						<a v-if="index==0" @click="addArea" style="display: block;float: left;margin-left: 1rem;font-size:20px;height: 29px;line-height: 29px;"><Icon type="plus-circled"></Icon></a>
						<a v-if="index!=0" @click="removeArea(item)" style="display: block;float: left;margin-left: 1rem;font-size:20px;height: 29px;line-height: 29px;"><Icon type="close-circled"></Icon></a>
					</div>
				</div>
				
			</li>-->
			<li>
				<label>是否启用:</label>
				<Select v-model="infomationList.IsEnable" style="width:200px" placeholder='请选择' class="input select">
			        <Option v-for="item in account_StatusList" :value="item.value" :key="item.value">{{ item.label }}</Option>
			    </Select>
			</li>
			<li style="height: 5.5rem;line-height: 5.5rem;">
				<label>备注信息:</label>
				<Input v-model="infomationList.Remark" class="input" type="textarea" :rows="4" placeholder="备注信息..." style="width: 200px;"></Input>
			</li>
			<li style="text-align: right;margin-top: 0.5rem;">
				<Button type="info" style="margin-right: 7rem;" @click='saveData'>保存</Button>
			</li>
		</ul>
		
		<ul>
			<li>
				<span style="display: block;float:left ;">工作区域</span>
				<Cascader class='area' :data="areaData"  @on-change='cascaserChange'  v-model="work_Area" style="width: 200px;display: block;float:left;margin-left: 1rem;height: 2.8rem;line-height: 2.8rem;margin-right: 1rem;"></Cascader>
				<Button type="success" @click="uploadArea">上传区域</Button>
			</li>
			<li v-for="(item,index) in areaList" style="border: 2px dotted gainsboro;width: 18.5rem;margin-left: 4rem;margin-bottom: 0.5rem;">
				<span style="margin-left: 2rem;display: block;float: left;width: 11rem;">
					{{item.ProvinceName}}/{{item.CityName}}/{{item.AreaName}}
				</span>
				<!--<a  @click="removeArea(item)" style="display: block;float: left;margin-left: 1rem;font-size:20px;height: 2.8rem;line-height:2.8rem;"><Icon type="close-circled"></Icon></a>-->
				 <Button @click="removeArea(item)" type="error" size="small" style="display: block;float: left;margin-left: 1rem;margin-top: 0.6rem;">删除</Button>
			</li>
			
		</ul>
	</div>
</template>
	
<script>
	export default{
		data(){
			return{
				areaData:[],//省市区数据源
				hotel_Area:[],//选择后  存储
				SalemanId:'',//会员id
				work_Area:[],
				work_ifo:{//工作区域
					SalemanId:'',
					ProvinceName:'',//省名字
					CityName:'',//市名
					AreaName:'',//区名
					ProvinceId:null,//省id
					CityId:null,//市id
					AreaId:null,//区id
				},
				areaList:[
//					{
//						ProvinceName:'北京市',//省名字
//						CityName:'北京',//市名
//						AreaName:'东城',//区名
//						SalemanAreaId:'',//删除用
//					},
					
				],
				infomationList:{
//					Name:'',//姓名
//					WorkNum:'',//工号
//					Telephone:'',//手机号
//					RegionId:'',//所在大区
//					workingArea:'',//工作区域
//					CardId:'',//身份证号
//					RegistDate:'',//注册时间
//					Remark:'',//备注信息
				},
				URLchange:URLHeader.user+'/api/SaleMan/UpdateSaleMan',//保存接口
				URLdetale:URLHeader.user+'/api/SaleMan/GetSaleManInfo',//队员详情
				account_Status:'',//账号状态
				account_StatusList:[//账号状态
					{
                        value: 0,
                        label: '启用'
                    },{
                        value: 1,
                        label: '禁用'
                    },
				],
				regional_List:[
         			{
                        value: 1,
                        label: '餐饮营销本部华北营业部'
                    },
                    {
                        value: 2,
                        label: '餐饮营销本部东北营业处'
                    },
                    {
                        value: 3,
                        label: '餐饮营销本部华中营业处'
                    },
                    {
                        value: 4,
                        label: '餐饮营销本部华东营业部'
                    },
                    {
                        value: 5,
                        label: '餐饮营销本部西北营业处'
                    },
                    {
                        value: 6,
                        label: '餐饮营销本部西南营业处'
                    },
                    {
                        value: 7,
                        label: '餐饮营销本部鲁东营业处'
                    },
                    {
                        value: 8,
                        label: '餐饮营销本部鲁中营业处'
                    },
                    {
                        value: 9,
                        label: '餐饮营销本部鲁西南营业处'
                    },
                    {
                        value: 10,
                        label: '餐饮营销本部鲁西北营业处'
                    }
         		],
			}
		},
		methods:{
			requestData(){
				var self = this;
				$('.loding').show()
				let DATA ='{"SalemanId":"'+self.SalemanId+'"}';
				console.log('SalemanId='+self.SalemanId)
				$.ajax({
			        type :"post",
			        url :self.URLdetale,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		//console.log(JSON.parse(json))
			        		var dataAll = JSON.parse(JSON.parse(json).Data);
			        		//var dataAll = json.Data
			        		dataAll.RegionId = dataAll.RegionId;
			        		self.hotel_Area.push(dataAll.ProvinceId)
			        		self.hotel_Area.push(dataAll.CityId)
			        		self.hotel_Area.push(dataAll.AreaId)
				    		self.infomationList = dataAll;
				    		
				    		self.areaList = JSON.parse(json).Area
				    		$('.loding').hide()
				    		//console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
			},
			saveData(){
				var self =this;
				self.regional_List.map((item) =>{
					if (item.value ==self.infomationList.RegionId) {
						self.infomationList.RegionName = item.label
					}
				})
//				this.infomationList.IsEnable = parseInt(this.account_Status)
				
				var self =this;
				//省市区
				//console.log(self.hotel_Area)
//				var area =$('.area input').val().split(' / ');
//				self.infomationList.ProvinceName=area[0];//省
//				self.infomationList.CityName=area[1];//市
//				self.infomationList.AreaName=area[2];//区
//				
//				self.infomationList.ProvinceId=self.hotel_Area[0];//省
//				self.infomationList.CityId=self.hotel_Area[1];//市
//				self.infomationList.AreaId=self.hotel_Area[2];//区
				self.infomationList.ProvinceName='';//省
				self.infomationList.CityName='';//市
				self.infomationList.AreaName='';//区
				
				self.infomationList.ProvinceId=0;//省
				self.infomationList.CityId=0;//市
				self.infomationList.AreaId=0;//区
				
				
				let DATA =JSON.stringify(this.infomationList);
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :self.URLchange,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
						self.$Message.success('保存数据成功');
						self.$router.push({path: '/players'});//跳转到队员列表
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		   });
			},
			cascaserChange(value,selectedData){//多级选择完后触发
				console.log(selectedData)
				if (selectedData.length>0) {
					this.work_ifo = {//工作区域
						SalemanId:this.SalemanId,
						ProvinceName:selectedData[0].label,//省名字
						CityName:selectedData[1].label,//市名
						AreaName:selectedData[2].label,//区名
						ProvinceId:selectedData[0].value,//省id
						CityId:selectedData[1].value,//市id
						AreaId:selectedData[2].value,//区id
					}
				}
			},
	         removeArea(item){//工作区域删除   Accessories_List
	         	var self =this
	         	$('.loding').show()
	         	$.ajax({
			        type :"post",
			        url :URLHeader.user+'/api/SaleMan/RemoveSalemanArea?SalemanAreaId='+item.SalemanAreaId,
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		//console.log(json)
						if (json=='Exc Success') {
							self.$Message.success('工作区域删除成功');
							self.areaList.splice(self.areaList.indexOf(item), 1)
						} else{
							self.$Message.error('工作区域删除错误');
						}
						$('.loding').hide()
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		  });
			},
			uploadArea(){//工作区域上传
				var self =this
				if (this.work_Area.length>0) {
					$('.loding').show()
					$.ajax({
				        type :"post",
				        url :URLHeader.user+'/api/SaleMan/AddSalemanArea',
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:JSON.stringify(self.work_ifo),
				        success : function(json) {
							if (json=='Exc Success') {
								self.$Message.success('工作区域上传成功');
								self.areaList.push(self.work_ifo)
								self.work_ifo = {//工作区域
									SalemanId:'',
									ProvinceName:'',//省名字
									CityName:'',//市名
									AreaName:'',//区名
									ProvinceId:null,//省id
									CityId:null,//市id
									AreaId:null,//区id
								}
							} else{
								self.$Message.error('工作区域上传错误');
								self.work_ifo = {//工作区域
									SalemanId:'',
									ProvinceName:'',//省名字
									CityName:'',//市名
									AreaName:'',//区名
									ProvinceId:null,//省id
									CityId:null,//市id
									AreaId:null,//区id
								}
							}
							$('.loding').hide()
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		  	});
					
				} else{
					self.$Message.error('请选择工作区域.');
				}
					
			}
		},
		mounted:function(){
			this.areaData=Provence;//区域数据	
			var hash= window.location.hash.split('playersEditor?')[1].split('&');
			this.SalemanId = hash[0].split('=')[1];
			this.requestData();
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
h1{
	margin: 1rem auto;
	text-align: center;
}
.playersEditor{
	width: 54rem;
	margin: 0 auto;
}
.playersEditor ul{
	float: left;
	border: 1px dashed gainsboro;
	border-radius: 6px;
	display: block;
	margin:  0 auto;
	margin-left: 1rem;
	width: 26rem;
	padding: 1rem 1rem;
	overflow: auto;
	margin-bottom: 1rem;
}	
.playersEditor ul li{
	height: 2.8rem;
	line-height: 2.8rem;
}
.playersEditor ul li label{
	padding-right: 0.8rem;
	font-size: 1.1rem;
	margin: auto;
	display: block;
	float: left;
	font-weight: bold;
}
.playersEditor ul li .input{
	display: block;
	float: right;
	margin-right: 3rem;
}
.playersEditor ul li .select{
	margin-left: 0.8rem;
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