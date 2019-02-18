<template>
	<!--营销活动新增-->
	<div class="marketingAdd">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<h2 style="text-align: center;border-bottom: 1px solid gainsboro;padding-bottom: 0.5rem;margin-bottom: 1rem;">营销活动</h2>
		<Form ref="marketingInformation" :model="marketingInformation" :rules="ruleValidate" :label-width="100" label-position	='left' >
			<div class="marketingAdd_left">
				<FormItem label="活动名称" prop="ActName" class='formitem'>
		            <Input v-model="marketingInformation.ActName" style="width: 170px;"  placeholder="活动名称"></Input>
		        </FormItem>
		        	<FormItem label="开始时间" prop="StartTime" class='formitem'>
		            <DatePicker type="datetime" v-model='marketingInformation.StartTime' placeholder="开始时间" style="width: 170px"></DatePicker>
		        </FormItem>
		        	<FormItem label="结束时间" prop="EndTime" class='formitem'>
		            <DatePicker type="datetime" v-model='marketingInformation.EndTime' placeholder="结束时间" style="width: 170px"></DatePicker>
		        </FormItem>
		        <FormItem label="状态" prop="ActState" class='formitem'>
		        		<Select v-model="marketingInformation.ActState" placeholder="选择状态" style='width: 170px;'>
		                <Option value="1">启用</Option>
		                <Option value="-1">停用</Option>
		            </Select>
		        </FormItem>
		        <FormItem label="奖励积分" prop="Integral"  class='formitem'>
		           <Input v-model="marketingInformation.Integral" style="width: 170px;"  placeholder="奖励积分"></Input>
		        </FormItem>
		        <FormItem label="活动描述"  class='formitem' >
		            <Input v-model="marketingInformation.ActivityDesc" style="width: 170px;" type="textarea" :autosize="{minRows: 3,maxRows: 5}" placeholder="请输入"></Input>
		        </FormItem>
			</div>
			<div class="marketingAdd_right">
				<!--<FormItem label="营销类型" prop="marketingtype" class='formitem'>
		            <RadioGroup v-model="marketingInformation.marketingtype">
		                <Radio label="0">抽奖</Radio>
		                <Radio label="1">线下签到</Radio>
		            </RadioGroup>
		        </FormItem>-->
		        <FormItem label="活动开展区域" prop="area" class='formitem'>
		        		<Cascader :data="areaData" v-model="marketingInformation.area" style='width: 170px;' class='area'></Cascader>
		        </FormItem>
		        <FormItem label="活动所在大区" prop="ActivityRegion"  class='formitem'>
		         	 <Input v-model="marketingInformation.ActivityRegion" style="width: 170px;"  placeholder="活动所在大区"></Input>
		        </FormItem>
		        <FormItem label="活动类型" prop="ActivityType"  class='formitem'>
		         	 <Input v-model="marketingInformation.ActivityType" style="width: 170px;"  placeholder="活动类型"></Input>
		        </FormItem>
		        <FormItem label="活动编码"   class='formitem'>
		         	 <Input v-model="marketingInformation.ActivityCode" style="width: 170px;"  placeholder="活动编码"></Input>
		        </FormItem>
		         
		         <FormItem label="分享页面"  class='formitem'>
		         	 <Input v-model="marketingInformation.ShareUrl" style="width: 170px;"  placeholder="分享页面"></Input>
		        </FormItem>
		         
		         
			</div>
	    </Form>

	    	<Button type="primary" @click="handleSubmit('marketingInformation')" v-if='isAdd' style="display: block;margin: 0 auto;">保存</Button>
	    	<Button type="error" @click="modificationBtn('marketingInformation')" v-if='!isAdd' style="display: block;margin: 0 auto;">修改</Button>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				isAdd:true,//新增模式
				ActivityId:'',//编辑时
				areaData:[],//省市区
				marketingInformation:{//活动信息
					ActName:'',//活动名称
					StartTime:'',//开始时间
					EndTime:'',//结束时间
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
				ruleValidate:{
					ActName:[{//活动名称
						required: true,
						message: '请输入活动名称',
						trigger: 'blur'
					}],
					StartTime: [{//开始时间
						required: true,
						type: 'date',
						message: '请选择开始时间', 
						trigger: 'change'
					}],
					EndTime: [{ //结束时间
						required: true,
						type: 'date',
						message: '请选择结束时间', 
						trigger: 'change'
					}],
					ActState: [{ //状态
						required: true,
						message: '请选择状态',
						trigger: 'change'
					}],
					Integral:[{//奖励积分
						required: true,
						message: '请输入奖励积分',
//						trigger: 'blur'
					}],
//					marketingtype:[{//营销类型
//						required: true,
//						message: '请选择营销类型',
//						trigger: 'change'
//					}],
					area:[{//线下城市
						required: true,
						message: '请选择线下城市',
//						trigger: 'change'
					}],
					
					ActivityType:[{//活动类型
						required: true,
						message: '请输入活动类型',
						trigger: 'blur'
					}],
					ShareUrl:[{//分享页面
						required: true,
						message: '请输入分享页面',
						trigger: 'blur'
					}],
					ActivityCode:[{//活动编码
						required: true,
						message: '请输入活动编码',
						trigger: 'blur'
					}],
					ActivityRegion:[{//所在大区
						required: true,
						message: '请输入所在大区',
						trigger: 'blur'
					}],
				},
				

			}
		},
		methods:{
			handleSubmit(name){//保存
				var self =this
				var url = URLHeader.marketing+'/api/SignActivity/AddActivity'
            		var Form = self.marketingInformation
				this.$refs[name].validate((valid) => {
                    if (valid) {
                    		//时间转换
                    		$('.loding').show()
		            		var star = new Date(Form.StartTime);  
						var StartTime = star.getFullYear() + '-' + (star.getMonth() + 1) + '-' + star.getDate() + ' ' + star.getHours() + ':' + star.getMinutes() + ':' + star.getSeconds();
		            		var end = new Date(Form.EndTime);  
						var EndTime = end.getFullYear() + '-' + (end.getMonth() + 1) + '-' + end.getDate() + ' ' + end.getHours() + ':' + end.getMinutes() + ':' + end.getSeconds();
						//省市整理
						var area = $('.area input').val().split('/ ')
						Form.ActProvinceName = area[0]//省
						Form.ActProvinceId = Form.area[0]//省
						Form.ActCityName = area[1]//市
						Form.ActCityId = Form.area[1]//市	
						
						var DATA = '{"ActName":"'+Form.ActName+'","StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","ActState":'+Form.ActState+',"Integral":'+Form.Integral+',"ActivityDesc":"'+Form.ActivityDesc+'","ActProvinceName":"'+Form.ActProvinceName+'","ActProvinceId":"'+Form.ActProvinceId+'","ActCityName":"'+Form.ActCityName+'","ActCityId":"'+Form.ActCityId+'","ActivityType":"'+Form.ActivityType+'","ShareUrl":"'+Form.ShareUrl+'","ActivityCode":"'+Form.ActivityCode+'","ActivityRegion":"'+Form.ActivityRegion+'"}'
						console.log(DATA)
						console.log(url)
                    		$.ajax({
							type:"post",
							url:url,
							contentType:'application/json; charset=utf-8',
							data:DATA,
							success:function(json){
								//var dataAll = JSON.parse(json);
								console.log(json)
								if (json=1) {
									$('.loding').hide()
									self.isAdd = false
									self.$Message.success('保存成功');
								} else{
									
								}
							},
							error:function(error){
								console.log(error)
								self.$Message.error('保存失败');
							}
						});
                        //this.$Message.success('Success!');
                    } else {
                        this.$Message.error('请填完整数据!');
                    }
                })
			},
			modificationBtn(name){//修改
				var self =this
				var url = URLHeader.marketing+'/api/SignActivity/UpdateActivity'
            		var Form = self.marketingInformation
				this.$refs[name].validate((valid) => {
                    if (valid) {
                    		//时间转换
                    		$('.loding').show()
		            		var star = new Date(Form.StartTime);  
						var StartTime = star.getFullYear() + '-' + (star.getMonth() + 1) + '-' + star.getDate() + ' ' + star.getHours() + ':' + star.getMinutes() + ':' + star.getSeconds();
		            		var end = new Date(Form.EndTime);  
						var EndTime = end.getFullYear() + '-' + (end.getMonth() + 1) + '-' + end.getDate() + ' ' + end.getHours() + ':' + end.getMinutes() + ':' + end.getSeconds();
						//省市整理
						var area = $('.area input').val().split('/ ')
						Form.ActProvinceName = area[0]//省
						Form.ActProvinceId = Form.area[0]//省
						Form.ActCityName = area[1]//市
						Form.ActCityId = Form.area[1]//市	
						
						var DATA = '{"ActivityId":'+self.ActivityId+',"RegistDate":"'+Form.RegistDate+'","ActName":"'+Form.ActName+'","StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","ActState":'+Form.ActState+',"Integral":'+Form.Integral+',"ActivityDesc":"'+Form.ActivityDesc+'","ActProvinceName":"'+Form.ActProvinceName+'","ActProvinceId":"'+Form.ActProvinceId+'","ActCityName":"'+Form.ActCityName+'","ActCityId":"'+Form.ActCityId+'","ActivityType":"'+Form.ActivityType+'","ShareUrl":"'+Form.ShareUrl+'","ActivityCode":"'+Form.ActivityCode+'","ActivityRegion":"'+Form.ActivityRegion+'"}'
						console.log(DATA)
						console.log(url)
                    		$.ajax({
							type:"post",
							url:url,
							contentType:'application/json; charset=utf-8',
							data:DATA,
							success:function(json){
								//var dataAll = JSON.parse(json);
								//console.log(json)
								if (json=1) {
									$('.loding').hide()
									self.isAdd = false
									self.getMarketInForm()
									self.$Message.success('修改成功');
								} else{
									
								}
							},
							error:function(error){
								console.log(error)
								self.$Message.error('修改成功失败');
							}
						});
                        //this.$Message.success('Success!');
                    } else {
                        this.$Message.error('Fail!');
                    }
                })
			},
			getMarketInForm(){//获取信息
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
						self.marketingInformation = {//活动信息
							ActName:dataAll.ActName,//活动名称
							StartTime:new Date(dataAll.StartTime),//开始时间
							EndTime:new Date(dataAll.EndTime),//结束时间
							ActState:dataAll.ActState.toString(),//状态
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
							RegistDate:dataAll.RegistDate,//新增时间
						}
						self.$refs['marketingInformation'].resetFields();
						$('.loding').hide()
					},
					error:function(error){
						console.log(error)
						self.$Message.error('数据获取失败');
					}
				});
	          
			}
		},
		mounted:function(){
			$('.loding').hide()
			this.areaData=ProvincesAndcities//省市数据
			this.ActivityId = this.$route.query.ActivityId
			if (this.ActivityId) {//编辑
				this.isAdd = false
				this.getMarketInForm()//获取信息
			} else{//新增
				this.isAdd = true
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
.marketingAdd{
	border: 1px dashed gainsboro;
	height: 32rem;
	padding: 1rem;
	border-radius:10px ;
	width: 45rem;
	margin: 0 auto;
}
.marketingAdd_left, .marketingAdd_right{
	float: left;
	width: 20rem;
	height: 24rem;
	margin: 0 auto;
}
.formitem{
	margin-left: 2rem;
}
</style>