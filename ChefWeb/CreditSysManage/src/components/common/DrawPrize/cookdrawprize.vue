<template>
  <div class="cookdrawprize">

    <div class="box-a">
    	<!--新增弹框-->
    	<Modal title="新增奖品信息" v-model="modal" width='400' :mask-closable="false" class="modal" @on-ok='confirm'>
			    <ul style="margin-left: 1.5rem;">
        		<li>
        			<span>奖项名称</span>
        			<Input v-model='prizeName' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
        		</li>
        		<li>
        			<span>奖品名称</span>
        			<Input v-model='prizename' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
        		</li>
        		<li>
        			<span>中奖数量</span>
        			<Input v-model='prizenum' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
        		</li>
        		<li>
        			<span>图片链接</span>
        			<Input v-model='prizelink' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
        		</li>
	        </ul>
	    </Modal>

			<Button type="primary" style="margin-left: 94%;" @click='addInformation'>新增</Button>
			<Table style="margin-top: 1rem;" :columns="table_columns" :data="prizeInfoList" @on-selection-change='selectDelet'></Table>

			<!--选择-->
			<Select v-model="model1" style="width:200px;margin-top: 40px;">
        <Option v-for="item in userList" :value="item.value" :key="item.value">{{ item.label }}</Option>
      </Select>
			<Button type="primary" style="margin-left: 1%;width: 60px;margin-top: 40px;" @click='selectConfirm'>确定</Button>
			<!--注册人数-->
			<span style="margin-left: 5%;position: relative;top: 20px;">注册人数：{{registerNum}}</span>
			<!--生成中奖用户-->
			<Button type="primary" style="margin-left: 10%;width: 100px;margin-top: 40px;" @click='createUser'>生成</Button>
			<!--根据手机号查询中奖用户-->
	    <Input v-model='queryTelephone' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 180px;margin-left:20%;margin-top: 40px;"></Input>
			<Button type="primary" style="margin-left: 1%;width: 60px;margin-top: 40px;" @click='queryMember'>查询</Button>
    </div>

    <div class="box-b" v-show="showFlag">
    	<!--新增弹框-->
    	<Modal title="编辑中奖用户" v-model="prizeUser" width='400' :mask-closable="false" class="modal" @on-ok='userconfirm'>
			    <ul style="margin-left: 1.5rem;">
        		<li>
        			<span>手机号</span>
        			<Input v-model='telephone' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
        		</li>
	        </ul>
	    </Modal>
	    <Modal title="中奖用户信息" v-model="drawMember" width='400' :mask-closable="false" class="modal" @on-ok='drawconfirm'>
			    <ul style="margin-left: 1.5rem;">
        		<li>
        			<span>奖项名称:</span><span>{{drawUserInfo.DrawName}}</span>
        		</li>
        		<li>
        			<span>奖品名称:</span><span>{{drawUserInfo.PrizeName}}</span>
        		</li>
        		<li>
        			<span>图片:</span><span>{{drawUserInfo.PrizeImg}}</span>
        		</li>
        		<li>
							<Button type="primary" style="margin-left: 36%;margin-top: 40px;width: 80px;" @click='toPrize' class="toPrize">去领奖</Button>
        		</li>
	        </ul>
	    </Modal>
			<Table style="margin-top: 1rem;" :columns="createUser_columns" :data="prizeUserList"></Table>
			<Button type="primary" style="margin-left: 48%;margin-top: 40px;width: 100px;" @click='savePrizeUser'>保存</Button>
    </div>

    <div class="box-c" v-show="drawUserFlag">
			<Table style="margin-top: 1rem;" :columns="userInfo_columns" :data="userInfoList"></Table>
    </div>

  </div>
</template>

<script>
//	import { } from 'vux'
//	import apiUrl from '../apiUrls.js'

	export default{
		components:{

		},
		data(){
			return{
				isAddImg:true,//状态 新增or编辑
				modal:false,//新增编辑 弹框
				prizeUser:false,//修改 弹框
				prizeName:'',//奖项名称
				prizename:'',//奖品名称
				prizenum:'',//中奖数量
				prizelink:'',//奖品图片链接
				prizeId:'',//列表信息标识
				showFlag:false,//生成中奖用户部分隐藏
				drawUserFlag:false,//中奖用户隐藏
				userId:'',//中奖用户id
				userprizename:'',//中奖用户的奖品名称
				telephone:'',//手机号
				nickname:'',//昵称
				userIndex:'',//修改第几个用户信息
				editData:{},//编辑信息
				deleteData:{},//删除信息
				prizeData:{},//奖项信息
				queryTelephone:'',//查询手机号
				drawMember:false,//中奖用户弹框
				drawUserInfo:{},//中奖用户信息
				userList: [],//选择奖项名称
        model1: '',//选择下拉框绑定值
        registerNum:0,//注册人数
				table_columns:[//table表头
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
                        title:'奖项名称',
                        width:150,
                        align: 'center',
                        key:'DrawName'
                     },
                    {
                        title: '奖品名称',
                        align: 'center',
                        key: 'PrizeName',
                     	width:150
                    },

                    {
                        title: '中奖数量',
                        key: 'PrizeNum',
                        width:150,
                        align: 'center',
                    },
                    {
                        title: '图片',
                        align: 'center',
                        key: 'PrizeImg'
                    },
                    {
                        title: '编辑',
                        align: 'center',
                        width:150,
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
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
                    {
                        title: '删除',
                        align: 'center',
                        width:150,
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
                                            this.delete(params)
                                        }
                                    }
                                }, '删除')
                            ]);
                        }
                    }
        ],
        prizeInfoList:[

				],
				createUser_columns:[
                    {
                        title:'NO.',
                        align: 'center',
                        type: 'index',
                        width:100
                    },
         			      {
                        title:'会员ID',
                        width:200,
                        align: 'center',
                        key:'MemberId'
                    },
                    {
                        title: '奖项名称',
                        align: 'center',
                        key: 'DrawName',
                     		width:200
                    },
                    {
                        title: '奖品名称',
                        align: 'center',
                        key: 'PrizeName',
                     	width:200
                    },
                    {
                        title: '手机号',
                        key: 'MemberTelePhone',
                        align: 'center',
                        width:200
                    },
                    {
                        title: '会员昵称',
                        align: 'center',
                        key: 'Nickname',
                        width:200
                    },
                    {
                        title: '修改',
                        align: 'center',
                        width:200,
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small'
                                    },
                                    style:{
                                    	width:'50px'
                                    },
                                    on: {
                                        click: () => {
                                            this.prizeInfo(params)
                                        }
                                    }
                                }, '修改')
                            ]);
                        }
                    }
				],
				prizeUserList:[

				],
				userInfo_columns:[
		        {
                title:'NO.',
                align: 'center',
                type: 'index',
//              width:100
            },
 			      {
                title:'会员ID',
//              width:200,
                align: 'center',
                key:'MemberId'
            },
            {
                title: '奖项名称',
                align: 'center',
                key: 'DrawName',
//           		width:200
            },
            {
                title: '奖品名称',
                align: 'center',
                key: 'PrizeName',
//           	width:200
            },
            {
                title: '手机号',
                key: 'MemberTelePhone',
                align: 'center',
//              width:200
            },
            {
                title: '会员昵称',
                align: 'center',
                key: 'Nickname',
//              width:200
            }
				],
				userInfoList:[]
			}
		},
		methods:{
			getInfo(){//获取奖品信息
				var self=this;
			  this.prizeInfoList = [];//清空数据
			  $.ajax({
						type:"get",
						url:'http://kongapi.shinho.net.cn/ecs/common/api/DrawPrize/GetAllPrizeList',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:{},
						success:function(json){
							var data=JSON.parse(json);
							self.userList=[];//筛选数据清空
              for(var i in data){
              	var item={
              		value:data[i].DrawPrizeId,
              		label:data[i].DrawName
              	}
              	self.userList.push(item);
              }
							self.prizeInfoList=data;
						},
						error:function(error){
							self.$Message.error('获取失败');
						}
				});
			},
			addInformation(){//点击新增按钮
				this.isAddImg = true;
				this.modal = true;//显示弹框
				this.prizeName='';
				this.prizename='';
				this.prizenum='';
				this.prizelink='';
			},
			redact(paramsdata){//点击编辑
				this.editData=paramsdata.row;
//				console.log(paramsdata.index);
				this.isAddImg = false;
				this.modal = true;//显示弹框
				this.prizeName=paramsdata.row.DrawName;
				this.prizename=paramsdata.row.PrizeName;
				this.prizenum=paramsdata.row.PrizeNum;
				this.prizelink=paramsdata.row.PrizeImg;
			},
			confirm(){//点击确认
				var self=this;
				if(this.isAddImg){//新增
					if(this.prizeName&&this.prizename&&this.prizenum&&this.prizelink){
						var params={
							'DrawName':this.prizeName,
			        'PrizeName':this.prizename,
			        'PrizeNum':this.prizenum,
			        'PrizeImg':this.prizelink,
			        'PrizeBatch':'',
			        'IsDelete':0,
			        'PrizeState':0
						}
						$.ajax({
							type:"post",
							url:'http://kongapi.shinho.net.cn/ecs/common/api/DrawPrize/SaveDrawPrize',
							dataTape:'json',
							contentType:'application/json; charset=utf-8',
							data:JSON.stringify(params),
							success:function(json){
								self.$Message.success('新增成功');
								self.getInfo();
							},
							error:function(error){
								self.$Message.error('新增失败');
							}
						});
					}else{
						this.$Message.error('请把信息填写完整');
					}
				}else{//编辑
					if(this.prizeName&&this.prizename&&this.prizenum&&this.prizelink){
						this.editData.DrawName=this.prizeName;
						this.editData.PrizeName=this.prizename;
						this.editData.PrizeNum=this.prizenum;
						this.editData.PrizeImg=this.prizelink;
						delete this.editData._index;
						delete this.editData._rowKey;
						$.ajax({
							type:"post",
							url:'http://kongapi.shinho.net.cn/ecs/common/api/DrawPrize/UpdateDrawPrize',
							dataTape:'json',
							contentType:'application/json; charset=utf-8',
							data:JSON.stringify(this.editData),
							success:function(json){
								self.$Message.success('编辑成功');
								self.getInfo();
							},
							error:function(error){
								self.$Message.error('编辑失败');
							}
						});
					}else{
						this.$Message.error('请把信息填写完整');
					}
				}
			},
			delete(paramsdata){//点击删除
				var self=this;
				this.deleteData = paramsdata.row;
				this.deleteData.IsDelete = 1;
//				delete this.deleteData._index;
//				delete this.deleteData._rowKey;

				$.ajax({
					type:"post",
					url:'http://kongapi.shinho.net.cn/ecs/common/api/DrawPrize/UpdateDrawPrize',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(this.deleteData),
					success:function(json){
						self.$Message.success('删除成功');
						self.getInfo();
					},
					error:function(error){
						self.$Message.error('删除失败');
					}
				});
			},
			selectDelet(paramsdata){//每次选择计算(table方法)
//				console.log(paramsdata);
        if(paramsdata.length == 1){//选中某一个复选框
        	this.prizeData = paramsdata[0];
        	console.log(this.prizeData);
        }
			},
			createUser(){//点击生成用户
				this.drawUserFlag=false;//筛选查看 中奖用户信息
				this.showFlag=true;//随机生成的中奖用户
				var self=this;
				var params={
					'Num':this.prizeData.PrizeNum
				}
				$.ajax({
					type:"post",
					url:'http://kongapi.shinho.net.cn/ecs/user/api/Member/GetActivityMember',
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:JSON.stringify(params),
					success:function(json){
						var data=JSON.parse(json);
						self.prizeUserList=data;
						for(var i in self.prizeUserList){
							self.prizeUserList[i].PrizeName=self.prizeData.PrizeName;
							self.prizeUserList[i].DrawName=self.prizeData.DrawName;
						}
					},
					error:function(error){
						self.$Message.error('生成失败');
					}
				});
			},
			prizeInfo(paramsdata){//点击修改
//				console.log(paramsdata.row);
				this.userIndex=paramsdata.index;
				this.prizeUser=true;//弹框
				this.telephone=paramsdata.row.MemberTelePhone;
			},
			userconfirm(){//确认修改
				var self=this;
				if(this.telephone){
					$.ajax({
						type:"post",
						url:'http://kongapi.shinho.net.cn/ecs/user/api/Member/GetMemberByPhone?phone='+this.telephone,
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:{},
						success:function(json){
							var data=JSON.parse(json);
							if(data.length!=0){
								data[0].PrizeName=self.prizeData.PrizeName;
								data[0].DrawName=self.prizeData.DrawName;
								self.prizeUserList.splice(self.userIndex,1,data[0]);
							}
						},
						error:function(error){
							self.$Message.error('请求失败');
						}
					});
				}else{
					this.$Message.error('请把信息填写完整');
				}
			},
			savePrizeUser(){//保存 中奖用户
        var arrData=[];
//      console.log(this.prizeUserList);
        if(this.prizeUserList.length <= 0){
					this.$Message.error('没有可保存数据');
        }else{
        	for(var i in this.prizeUserList){
	        	var item={
	        		'PrizeBatch' :this.prizeData.PrizeName,
			        'DrawPrizeId' :this.prizeData.DrawPrizeId,
			        'MemberId' :this.prizeUserList[i].MemberId,
			        'IsEnable' :0
	        	}
	        	arrData.push(item);
	        }
	        var self=this;
	        $.ajax({
						type:"post",
						url:'http://kongapi.shinho.net.cn/ecs/common/api/MemberPrize/AddMemberPrize',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:JSON.stringify(arrData),
						success:function(json){
							self.$Message.success('保存成功');
							self.prizeUserList=[];//中奖列表数据清空
							self.showFlag=false;//隐藏
						},
						error:function(error){
							self.$Message.error('保存失败');
						}
					});
        }
			},
			queryMember(){//查询手机号按钮
				var self=this;
        $.ajax({
					type:"post",
					url:'http://kongapi.shinho.net.cn/ecs/common/api/MemberPrize/PrizeByTelephone?telephone=' + this.queryTelephone,
					dataTape:'json',
					contentType:'application/json; charset=utf-8',
					data:{},
					success:function(json){
//						console.log(JSON.parse(json));
						var data=JSON.parse(json);
						if(data.length==0){//没有中奖
							self.$Message.error('没有中奖');
						}else{//中奖
							self.drawMember=true;//展示中奖用户信息
							self.drawUserInfo=data[0];
							if(data[0].IsEnable==0){//未领奖
								$('.toPrize').html('去领奖');
							}else{//已领奖
								$('.toPrize').html('已领奖');
							}
						}
					},
					error:function(error){
						self.$Message.error('请求失败');
					}
				});
			},
			toPrize(){//去领奖
				var self=this;
				if(this.drawUserInfo.IsEnable == 0){//中奖 未领奖
          $.ajax({
						type:"post",
						url:'http://kongapi.shinho.net.cn/ecs/common/api/MemberPrize/HavePrize?memberPrizeId=' + this.drawUserInfo.MemberPrizeId ,
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:{},
						success:function(json){
							if(json == "Exc Success"){
						    self.$Message.success('领奖成功');
							}else{
						    self.$Message.error('领奖失败');
							}
						},
						error:function(error){
							self.$Message.error('领奖失败');
						}
					});
				}
			},
			drawconfirm(){//查询用户中奖信息 点击确定
				this.drawMember=false;//隐藏中奖用户信息
				this.queryTelephone='';
			},
			selectConfirm(){//确定按钮  查询中奖用户
				var self=this;
				$.ajax({
						type:"post",
						url:'http://kongapi.shinho.net.cn/ecs/common/api/MemberPrize/GetMemberByDrawPrizeId?drawPrizeId=' + this.model1 ,
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:{},
						success:function(json){
							console.log(JSON.parse(json));
							self.showFlag=false;//生成中奖用户 隐藏
							self.drawUserFlag=true;//查询中奖用户 显示
							self.userInfoList=JSON.parse(json);
						},
						error:function(error){
							self.$Message.error('请求失败');
						}
				});
			},
			getRegisterNum(){//注册人数
				var self = this;
				$.ajax({
						type:"get",
						url:'http://kongapi.shinho.net.cn/ecs/common/api/MemberPrize/GetMemberByCookeday',
						dataTape:'json',
						contentType:'application/json; charset=utf-8',
						data:{},
						success:function(json){
//							console.log(json);
							self.registerNum=json;
						},
						error:function(error){
							self.$Message.error('请求失败');
						}
				});
			}
		},
		mounted(){
			this.getInfo();//获取奖品信息
			this.getRegisterNum();//获取注册人数
		}
	}
</script>

<style scoped>
.cookdrawprize{height: 10000px;}
.modal ul li {
	display: block;
	margin: 0.5rem auto;
}
</style>
<style>

</style>
