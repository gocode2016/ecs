<template>
	<div class="cookerAuditAfter">
		<Menu mode="horizontal" active-name="1"  @on-select='selectHeader'>
            <div class="layout-assistant">
                <Menu-item name="1" >基本信息</Menu-item>
                <Menu-item name="2" v-if="isShowHeader">订单</Menu-item>
                <Menu-item name="3" v-if="isShowHeader">积分明细</Menu-item>
                <Menu-item name="4" v-if="isShowHeader">从业经历</Menu-item>
                <Menu-item name="5" v-if="isShowHeader">荣誉资格</Menu-item>
                <Menu-item name="6" v-if="isShowHeader" class='cer'>认证资料</Menu-item>
            </div>
        </Menu>
        <Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
        <div class="content">
            <div class="informationContent headerdiv" style="overflow: auto;">
            		<div>
            			<img  :src="infomation.ImgUrl" style="width: 6rem;min-width: 6rem;margin-left: 1rem;"/>
	        			<ul class="top_ul" style="margin-left: 1rem;">
	        				<li>
	        					<span class="lable">姓名</span><span class="value" style="margin-left: 3rem;">{{infomation.MemberName}}</span>

	        				</li>
	        				<li>
	        					<span class="lable">会员ID</span><span class="value">{{infomation.MemberId}}</span>
	        				</li>
	        				<li>
	        					<span class="lable">手机号</span><span class="value">{{infomation.MemberTelePhone}}</span>
	        				</li>
	        				<li class="Tourist">
	        					<span class="lable" style="">当前业务代表</span><span class="value" style="margin-left: 1.2rem;">{{infomation.Name}}</span>
	        				</li>
	        			</ul>
	        			<ul class="top_ul" style="margin-left: 1rem;">
	        				<li>
	        					<span class="lable">酒店区域</span><span class="value" style="margin-left: 3rem;" v-if="infomation.ProvinceName">{{infomation.ProvinceName}}/{{infomation.CityName}}/{{infomation.AreaName}}</span>

	        				</li>
	        				<li>
	        					<span class="lable">酒店地址</span><span class="value">{{infomation.HotelAddress}}</span>
	        				</li>
	        				<li>
	        					<span class="lable">酒店名称</span><span class="value">{{infomation.HotelName}}</span>
	        				</li>
	        				<li>
	        					<span class="lable" style="">工作岗位</span><span class="value" style="margin-left: 1.2rem;" v-if="infomation.PositionType">{{infomation.PositionType}}/{{infomation.Position}}</span>
	        				</li>
	        			</ul>
            		</div>


        			<!--<br />-->
        			<h1></h1>
        			<ul class="bottom_ul">
        				<li><span class="lable">所在大区</span><span class="value">{{playerInformation.RegionName}}</span></li>
        				<li><span class="lable">注册时间</span><span class="value">{{infomation.RegistDate}}</span></li>
        				<li><span class="lable">注册来源</span><span class="value">-------</span></li>
        				<li><span class="lable">队员工号</span><span class="value">{{playerInformation.WorkNum}}</span></li>
        				<li><span class="lable">客户编码</span><span class="value">{{infomation.MemberCode}}</span></li>
        				<li><span class="lable">队员手机号</span><span class="value">{{playerInformation.Telephone}}</span></li>
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
        				<li><span class="lable">家庭区域</span><span class="value" v-if="infomation.HomeProvinceName">{{infomation.HomeProvinceName}}/{{infomation.HomeCityName}}/{{infomation.HomeAreaName}}</span></li>
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
        				<li><span class="lable">完善信息时间</span><span class="value">{{infomation.ProfileTime}}</span></li>
        				<li><span class="lable">普通话水平</span><span class="value">{{infomation.ChineseLv}}</span></li>

        				<li><span class="lable">调味品采购权</span><span class="value">{{infomation.Purchaser}}</span></li>
        				<li><span class="lable">毕业院校</span><span class="value">{{infomation.School}}</span></li>
        				<li><span class="lable">婚姻状况</span><span class="value">{{infomation.IsMarry}}</span></li>
        				<li><span class="lable">专业</span><span class="value">{{infomation.Major}}</span></li>
        				<li><span class="lable">备注信息</span><span class="value">{{infomation.Remark}}</span></li>
        				<li><span class="lable">在职情况</span><span class="value">{{infomation.IsJob}}</span></li>
        				<li><span class="lable">自我介绍</span><span class="value">{{infomation.Introduction}}</span></li>




        				<!--<li><span class="lable">状态</span><span class="value" style="margin-left: 2.8rem;">{{infomation.MemberState}}</span></li>
        				<li><span class="lable">状态</span><span class="value" style="margin-left: 2.8rem;">{{infomation.MemberState}}</span></li>
        				<li><span class="lable">活跃度</span><span class="value" style="margin-left: 2rem;">活跃度数据</span></li>
        				<li><span class="lable">酒店名称</span><span class="value">{{infomation.HotelName}}</span></li>
        				<li><span class="lable">生日</span><span class="value" style="margin-left: 2.8rem;"></span></li>
        				<li><span class="lable">账号状态</span><span class="value" style="margin-left: 1.2rem;">账号状态数据</span></li>
        				<li><span class="lable">酒店编码</span><span class="value">{{infomation.HotelCode}}</span></li>
        				<li><span class="lable">分类</span><span class="value" style="margin-left: 3rem;">未认证数据</span></li>
        				<li><span class="lable">当前积分</span><span class="value" style="margin-left: 1.2rem;">{{infomation.LeaveIntegral}}</span></li>
        				<li><span class="lable">备注信息</span><span class="value">{{infomation.Remark}}</span></li>  -->
        				<li style="width: 100%;text-align: right;">
        					<Button type="primary" style="margin-right: 0.8rem;" @click='qualified' v-if='isShowQualified'>合格</Button>
        					<Button type="primary" style="margin-right: 0.8rem;" @click='editorCooker'>编辑</Button>
        				</li>
        			</ul>

            </div>
             <div class="orderContent headerdiv">
            		<ul class="orderTopUl">
            			<li class="orderTopLi">
            				<span class="lable">状态</span>
            				<Select v-model="orderSearch.OrderState" style="width:100px" class="value">
					        <Option v-for="item in stateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
            			</li>
            			<li class="orderTopLi">
            				<span class="lable">订单时间</span>
            				<DatePicker type="daterange" v-model='orderSearch.searchTime' confirm placement="bottom-end" placeholder="选择时间" style="width: 200px" class='value'></DatePicker>
            			</li>
            			<li class="orderTopLi" style="margin-left: 0.5rem;">
            				<span style="font-size: 1.3rem;">|</span>
            			</li>
            			<li class="orderTopLi" style="margin-left: 0;">
            				<Dropdown style="margin-left: 20px">
					        <Button type="primary">
					            {{orderSearch.dropItem.label}}
					            <Icon type="arrow-down-b"></Icon>
					        </Button>
					        <DropdownMenu slot="list" >
					            <DropdownItem  v-for='item in selectSearch' @click.native='dropChange(item)' :key= "item.value">{{item.label}}</DropdownItem>
					        </DropdownMenu>
					    </Dropdown>
            			</li>
            			<li class="orderTopLi" style="margin-left: 1rem;">
            				<Input v-model="orderSearch.searchValue" placeholder="请输入..." style="width: 200px"></Input>
            			</li>
            			<li class="orderTopLi" style="margin-left: 1rem;">
            				<Button type="primary" @click='orderSearchBtn' icon="ios-search">搜索</Button>
            			</li>

            		</ul>
            		<Table :columns="orderColumns" :data="orderList" style="margin-right: 1rem;"></Table>
            		<Page :total="ordertotal" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='OrderChangePage'></Page>



            </div>
            <div class="integralContent headerdiv">
	            	<ul class="integralTopUl">
            			<li class="integralTopLi">
            				<span class="lable">状态</span>
            				<Select v-model="integralSearch.integralState" style="width:100px" class="value">
					        <Option v-for="item in integralStateList" :value="item.value" :key="item.value">{{ item.label }}</Option>
					    </Select>
            			</li>
            			<li class="integralTopLi">
            				<span class="lable">积分时间</span>
            				<DatePicker type="daterange" v-model='integralSearch.searchTime' confirm placement="bottom-end" placeholder="选择时间" style="width: 200px" class='value'></DatePicker>
            			</li>
            			<li class="integralTopLi" style="margin-left: 1rem;">
            				<Button type="primary" @click='integralSearchBtn' icon="ios-search">搜索</Button>
            			</li>
            		</ul>
            		<Table :columns="integralColumns" :data="integralList" style="margin-right: 1rem;"></Table>
            		<Page :total="integraltotal" show-total  :page-size='20' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='integralChangePage'></Page>
            </div>
            <div class="hotelContent headerdiv">
            		<Table style="margin-top: 1rem;" :columns="hotelList" :data="hotel_data"></Table>
            </div>
            <div class="kudos headerdiv" >
            		<ul>
            			<li>
            				<span class="lable" >职业资格:</span>
            				<span class="value" style="margin-left: 3.9rem;">{{infomation.Qualifications}}</span>
            			</li>
            			<li>
            				<span class="lable">营养师等级:</span>
            				<span class="value" style="margin-left: 2.0rem;">{{infomation.DietitianLv}}</span>
            			</li>
            			<li>
            				<span class="lable">电视媒体曝光:</span>
            				<span class="value" style="margin-left: 2rem;">{{infomation.ExposureCount}}</span>
            			</li>
            			<li>
            				<span class="lable">荣誉职称:</span>
            				<span class="value" style="margin-left: 2.8rem;">{{infomation.Honor}}</span>
            			</li>
            			<li>
            				<span class="lable">烹饪比赛名称:</span>
            				<span class="value" style="margin-left: 2rem;">{{infomation.MatchName}}</span>
            			</li>
            			<li>
            				<span class="lable">赛事级别:</span>
            				<span class="value" style="margin-left: 2.8rem;">{{infomation.MatchLv}}</span>
            			</li>
            			<li>
            				<span class="lable">比赛名次:</span>
            				<span class="value" style="margin-left: 3.9rem;">{{infomation.MatchNum}}</span>
            			</li>
            		</ul>
            </div>
       		<div class="headerdiv certification">
       			<Modal title="图片" v-model="imgModal">
				    <img :src="modelImageSrc" style="width: 30rem;display: block;margin: 0 auto;"/>
				</Modal>
				<Modal
			        v-model="notThrough"
			        title="未通过"
			        width='400px'
			        @on-ok="cerAuditBtn(-1)">
			        <Input v-model="Remark" type="textarea" :autosize="{minRows: 5,maxRows: 8}" placeholder="未通过原因"></Input>
			    </Modal>
       			<div style="margin-top: 1rem;">
       				<h3>酒店信息</h3>
       				<!--其它-厨师岗位-->
       				<ul class="cerUl cook">
       					<li>
       						<span>真实姓名:</span>
       						<span>{{infomation.MemberName}}</span>
       					</li>
       					<li>
       						<span>工作岗位:</span>
       						<span>{{infomation.PositionType}}/{{infomation.Position}}</span>
       					</li>
       					<li>
       						<span>酒店区域:</span>
       						<span>{{infomation.ProvinceName}}/{{infomation.CityName}}/{{infomation.AreaName}}</span>
       					</li>
       					<li>
       						<span>酒店名称:</span>
       						<span>{{infomation.HotelName}}</span>
       					</li>
       					<li>
       						<span>酒店地址:</span>
       						<span>{{infomation.HotelAddress}}</span>
       					</li>
       					<li>
       						<span>主营类型:</span>
       						<span>{{infomation.OperationType}}</span>
       					</li>
       					<li>
       						<span>酒店人均消费:</span>
       						<span>{{infomation.PerConsumption}}</span>
       					</li>
       					<li>
       						<span>调味品采购权:</span>
       						<span>{{infomation.Purchaser}}</span>
       					</li>
       					<li>
       						<span>现在使用欣和产品:</span>
       						<span>{{infomation.IsUseShinhoStr}}</span>
       					</li>
       				</ul>
       				<!--其它-美食爱好者-->
       				<ul class="cerUl other_foodie">
       					<li>
       						<span>真实姓名:</span>
       						<span>{{infomation.MemberName}}</span>
       					</li>
       					<li>
       						<span>工作岗位:</span>
       						<span>{{infomation.PositionType}}/{{infomation.Position}}</span>
       					</li>
       					<li>
       						<span>是否有孩子:</span>
       						<span>{{infomation.IsAnyChild}}</span>
       					</li>
       					<li>
       						<span>您的家庭收入:</span>
       						<span>{{infomation.HomeIncome}}</span>
       					</li>
       					<li>
       						<span>您的烹饪频率:</span>
       						<span>{{infomation.CookRate}}</span>
       					</li>
       				</ul>
       				<!--其它-调味品供货商-->
       				<ul class="cerUl other_supplier">
       					<li>
       						<span>真实姓名:</span>
       						<span>{{infomation.MemberName}}</span>
       					</li>
       					<li>
       						<span>工作岗位:</span>
       						<span>{{infomation.PositionType}}/{{infomation.Position}}</span>
       					</li>
       					<li>
       						<span>所在批发市场名称:</span>
       						<span>{{infomation.WholesaleName}}</span>
       					</li>
       					<li>
       						<span>门店区域:</span>
       						<span>{{infomation.ProvinceName}}/{{infomation.CityName}}/{{infomation.AreaName}}</span>
       					</li>
       					<li>
       						<span>门店名称:</span>
       						<span>{{infomation.HotelName}}</span>
       					</li>
       					<li>
       						<span>门店地址:</span>
       						<span>{{infomation.HotelAddress}}</span>
       					</li>
       					<li>
       						<span>门店经营范围:</span>
       						<span>{{infomation.ShopOperateSize}}</span>
       					</li>
       				</ul>
       			</div>
       			<div style="margin-top: 1rem;">
       				<h3>照片资料</h3>
					<div class="demo-upload-list">
						<img  :src="cerInformation.AuthImg1" style="height: 8rem;"/>
			            <div class="demo-upload-list-cover">
			                <Icon type="ios-eye-outline" 	@click.native="showImageModal(cerInformation.AuthImg1)"></Icon>
			            </div>
					</div>
					<div class="demo-upload-list">
						<img  :src="cerInformation.AuthImg2" style="height: 8rem;"/>
			            <div class="demo-upload-list-cover">
			                <Icon type="ios-eye-outline" 	@click.native="showImageModal(cerInformation.AuthImg2)"></Icon>
			            </div>
					</div>
       			</div>
       			<div style="margin-top: 1rem;">
       				<h3>信息录入</h3>
       				<ul style="" class="entryUl">
       					<li class="li">
       						<span class="lable">身份信息:</span>
       						<Input v-model='entryInformation.CardId'  placeholder="身份信息"  class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">生日:</span>
       						<DatePicker v-model='entryInformation.Birthday' type="date" placeholder="选择生日" class="value Birthday"></DatePicker>
       					</li>
       					<li class="li">
       						<span class="lable">家庭地址:</span>
       						<Input v-model='entryInformation.HomeAddress' placeholder="家庭地址"  class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">家庭区域</span>
							<Cascader v-model='entryInformation.homeAre' class='value are' :data="areaData"></Cascader>
       					</li>
       					<li class="li">
       						<span class="lable">烹饪比赛名称:</span>
       						<Input v-model='entryInformation.MatchName' placeholder="烹饪比赛名称" class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">赛事级别:</span>
       						<Input v-model='entryInformation.MatchLv' placeholder="赛事级别" class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">比赛名次:</span>
       						<Input v-model='entryInformation.MatchNum' placeholder="比赛名次" class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">职业资格:</span>
       						<Input v-model='entryInformation.Qualifications' placeholder="职业资格" class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">营养师等级:</span>
       						<Input v-model='entryInformation.DietitianLv'  placeholder="营养师等级" class="value"></Input>
       					</li>
       					<li class="li">
       						<span class="lable">荣誉职称:</span>
       						<Input v-model='entryInformation.Honor'  placeholder="荣誉职称" class="value"></Input>
       					</li>

       					<li style="text-align: center;padding-top: 1rem;overflow: auto;width: 43rem;">
       						<Button type="primary" @click="entryIfoUpData">信息上传</Button>
       					</li>
       				</ul>
       			</div>
       			<div v-if="cerInformation.AuthState===-1" style="margin-top: 1rem;">
       				<h3>未通过原因:</h3>
       				<span style="margin-left: 3rem;margin-top: 0.5rem;display: block;color: red;">{{cerInformation.Remark}}</span>
       				<!--<span style="margin-left: 3rem;margin-top: 0.5rem;display: block;color: red;">哈哈哈哈</span>-->
       			</div>
       			<div style="width: 60rem;margin-top: 1rem;text-align: center;" v-if="cerInformation.AuthState===-1 || cerInformation.AuthState===0">
       				<Button type="primary" style="margin-left: -8rem;" @click="cerAuditBtn(1)">通过</Button>
       				<Button type="error" style="margin-left: 2rem;" @click="notThroughBtn">不通过</Button>
       			</div>
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
				playerInformation:[//部分队员信息
				],
				URL:{//api/MemberProfile/GetMemberProfile
					infomation:URLHeader.user+'/api/MemberProfile/GetMemberProfile',//新基本信息
					qualified:URLHeader.user+'/api/Member/ReviewMember',//合格链接
					hotel:URLHeader.user+'/api/Member/GetMemberResume',//从业经历
					player:URLHeader.user+'/api/Member/GetMemberById',//一些队员信息
					binding:URLHeader.user+'/api/SaleMan/TransferMember',//绑定队员确认动作
				},
				//订单=====================
				ordertotal:0,//总条数
				orderSearch:{
					OrderState:'全部',//状态
					searchTime:[],//时间
					searchValue:'',//输入框
					dropItem:{//默认为订单
						value:2,
						label:'订单号'
					},//下拉框选择
				},
				selectSearch:[
					{
						value:0,
						label:'收件人'
					},
					{
						value:1,
						label:'订单号'
					},
					{
						value:2,
						label:'手机号'
					},
				],
				stateList:[
					{
                        value: '全部',
                        label: '全部'
                    },
                    {
                        value: '求助中',
                        label: '求助中'
                    },
                    {
                        value: '未提交',
                        label: '未提交'
                    },
                    {
                        value: '备货中',
                        label: '备货中'
                    },
                    {
                        value: '已发货',
                        label: '已发货'
                    },
				],
				orderList:[],//订单数据
				orderColumns:[
					{
						title:'No.',
						align: 'center',
						type:'index',
					},
					{
						title:'订单号',
						align: 'center',
						key:'OrderId'
					},
					{
						title:'下单会员',
						align: 'center',
						key:'MemberId',
					},
					{
						title:'收货人',
						align: 'center',
						key:'OrderName',
					},
					{
						title:'收货人手机',
						align: 'center',
						width:110,
						key:'OrderTelephone',
					},
					{
						title:'收货地址',
						align: 'center',
						width:150,
						key:'OrderAddress',
					},
					{
						title:'订单来源',
						align: 'center',
						key:'OrderFrom',
					},
					{
						title:'消费积分',
						align: 'center',
						key:'OrderPrice',
					},
					{
						title:'订单状态',
						align: 'center',
						key:'OrderState',
					},
					{
						title:'下单时间',
						align: 'center',
						width:110,
						key:'AddDate',
					},

				],
				//积分明细----------------------
				integraltotal:0,//总条数
				integralSearch:{//积分明细搜索条件
					integralState:0,
					searchTime:[],//积分时间段
				},
				integralStateList:[//积分状态
					{
                        value: 0,
                        label: '全部'
                    },
                    {
                        value: 1,
                        label: '收入'
                    },
                    {
                        value: 2,
                        label: '支出'
                    },
				],
				integralList:[],//积分明细列表数据
				integralColumns:[
//					{
//                      title:'NO.',
//                      align: 'center',
//                      type:'index'
//
//                   },
					 {
                        title: '数量',
                        align: 'center',
                        key: 'num',
                        width:120
                    },
         			{
                        title: '时间',
                        width:110,
                        key: 'CreatDate',
                        align: 'center',
                    },
                    {
                        title: '分类',
                        key: 'IntegralTypeStr',
                        align: 'center',
                    },
                    {
                        title: '内容',
                        align: 'center',
                        key: 'IntegralSource'
                    },
                    {
                        title: '说明',
                        align: 'center',
                        key: 'Remark'
                    },
//                  {
//                      title: '数量',
//                      align: 'center',
//                      key: 'num',
//                      width:120
//                  },
                    {
                        title: '积分额',
                        align: 'center',
                        key: 'IntegralNum',
                    },

				],
				//从业经历------------------
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
//                       width:80,
                        align: 'center',
                    },
                    {
                        title: '酒店市',
                        align: 'center',
//                      width:80,
                        key: 'CityName'
                    },
                    {
                        title: '酒店区',
                        align: 'center',
//                       width:80,
                        key: 'AreaName'
                    },
                    {
                        title: '酒店地址',
                        align: 'center',
//                       width:120,
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
//                       width:90,
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
				//------------------------------荣誉资格------------------------
				kudosData:{//其它信息
				},
				//------------------------------认证资料------------------------
				certificationPlayerid:'',//绑定队员id
				imgModal:false,//大图弹框
				modelImageSrc:'',//大图地址
				notThrough:false,//不通过弹框
				Remark:'',//不通过原因
				areaData:[],//省市区
				cerInformation:{//会员展示信息  微信段填写的
					AuthImg1:'../../../../../static/image/placeholderBag.png',//身份证
					AuthImg2:'../../../../../static/image/placeholderBag.png',//身份证
					AuthState:'',//1通过;-1未通过  0未审核
					Remark:'',//未通过原因
				},
				entryInformation:{//信息 录入
					CardId:'',//省份证id
					Birthday:'',//生日
					HomeAddress:'',// 家庭地址
					homeAre:[],//家庭区域
					HomeProvinceId:'',//省id
					HomeProvinceName:'',//省名称
					HomeCityId:'',//城市id
					HomeCityName:'',//城市名称
					HomeAreaId:'',//区id
					HomeAreaName:'',//区名称
					MatchName:'',//烹饪比赛名称
					MatchLv:'',//赛事级别
					MatchNum:'',//比赛名次
					Qualifications:'',//职业资格
					DietitianLv:'',//营养师等级
					Honor:'',//荣誉职称
					MemberId:'',//
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
					$('.kudos').show();
					break;
					case 6:
					$('.certification').show();
					break;
				}
			},
			requestCookerInfomation(){//请求基本信息
				var self =this;
				$('.loding').show()
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
		        	  	if (JSON.parse(json).length>0) {
		        	  		if (dataAll.RecommendId>0) {
			        	  		dataAll.RecommendId='已认证'
			        	  	} else{
			        	  		dataAll.RecommendId='未认证'
			        	  	}
			        	  	if (dataAll.ImgUrl==null || dataAll.ImgUrl=='') {//占位图
//			        	  		console.log(2)
			        	  		dataAll.ImgUrl = '../../../../../static/image/HeadPortrait.png'
			        	  	}
			        	  	if (dataAll.IsJob==1) {//在职情况
			        	  		dataAll.IsJob = '在职'
			        	  	}else if (dataAll.IsJob==0) {
			        	  		dataAll.IsJob = '离职'
			        	  	}
			        	  	if (dataAll.IsMarry==1) {//婚姻情况
			        	  		dataAll.IsMarry = '已婚'
			        	  	}else if (dataAll.IsMarry==0) {
			        	  		dataAll.IsMarry = '未婚'
			        	  	}
			        	  	if (dataAll.IsShare==1) {//是否乐于分享
			        	  		dataAll.IsShare = '是'
			        	  	}else if (dataAll.IsShare==0) {
			        	  		dataAll.IsShare = '否'
			        	  	}
			        	  	if (dataAll.Sex==1) {//性别
			        	  		dataAll.Sex = '男'
			        	  	}else if (dataAll.Sex==0) {
			        	  		dataAll.Sex = '女'
			        	  	}
			        	  	if (dataAll.IsManyExp==1) {//多城市从业经验
			        	  		dataAll.IsManyExp = '有'
			        	  	}else if (dataAll.IsManyExp==0) {
			        	  		dataAll.IsManyExp = '无'
			        	  	}
			        	  	if (dataAll.MemberActiveState==1) {//活跃度搞
         						dataAll.MemberActiveState='高'
         				} else if (dataAll.MemberActiveState==2) {
         					dataAll.MemberActiveState='低'
         				}
         				//现在是否使用欣和产品
         				//infomation.PerConsumption
         				if (dataAll.IsUseShinho==1) {//是
         					dataAll.IsUseShinhoStr = '是'
         				} else if (dataAll.IsUseShinho==0) {
         					dataAll.IsUseShinhoStr = '否'
         				}
						//厨龄
//						console.log(new Date().getFullYear())
//						console.log(new Date(dataAll.JoinDate).getFullYear())
//						console.log(dataAll.JoinDate)
						if (dataAll.JoinDate !=null) {
							dataAll.KitchenAge=new Date().getFullYear()-new Date(dataAll.JoinDate).getFullYear()+1;
						}
				        	self.infomation=dataAll
				        	//Tourist
				        	if (	self.infomation.Name==null) {//游客无业务代表
				        		$('.Tourist').hide()
				        	}

		        	  	}
		        	  	//认证资料  赋值开始
		        	  	//酒店信息
		        	  	var infomation =self.infomation
		        	  	console.log(infomation)
					$('.other_supplier').hide()
					$('.other_foodie').hide()
					$('.cook').hide()
					//infomation.Positio ='调味品供货商'
					//console.log(infomation.Positio)
		        	  	if (infomation.Position=='调味品供货商') {
		        	  		$('.other_supplier').show()
		        	  	} else if (infomation.Position=='美食爱好者') {
		        	  		$('.other_foodie').show()
		        	  	} else{//厨师类
		        	  		$('.cook').show()
		        	  	}
					self.entryInformation = {//信息 录入
						CardId:infomation.CardId,//省份证id
						Birthday:infomation.Birthday,//生日
						HomeAddress:infomation.HomeAddress,// 家庭地址
						homeAre:[infomation.HomeProvinceId,infomation.HomeCityId,infomation.HomeAreaId],//家庭区域
						HomeProvinceId:infomation.HomeProvinceId,//省id
						HomeProvinceName:infomation.HomeProvinceName,//省名称
						HomeCityId:infomation.HomeCityId,//城市id
						HomeCityName:infomation.HomeCityName,//城市名称
						HomeAreaId:infomation.HomeAreaId,//区id
						HomeAreaName:infomation.HomeAreaName,//区名称
						MatchName:infomation.MatchName,//烹饪比赛名称
						MatchLv:infomation.MatchLv,//赛事级别
						MatchNum:infomation.MatchNum,//比赛名次
						Qualifications:infomation.Qualifications,//职业资格
						DietitianLv:infomation.DietitianLv,//营养师等级
						Honor:infomation.Honor,//荣誉职称
						MemberId:infomation.MemberId,//
					}
		        	  	////认证资料  赋值结束
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

			        	 var dataAll = JSON.parse(json)[0];

			        	  self.playerInformation = dataAll
					//console.log(JSON.parse(json)[0])
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
			//-------------------------------------订单-----------------------------------------------
			OrderChangePage(index){//清单页面页面切换
				this.requestOrderData(index)
			},
			orderSearchBtn(){//订单搜索
				this.requestOrderData(1)
			},
			dropChange(name){//drop  选择操作
				this.orderSearch.dropItem = name
				console.log(this.orderSearch)
			},
			requestOrderData(index){//订单数据请求
				var self =this
				self.orderList =[]
				var StartTime=''//开始时间
				var EndTime = ''//结束时间
				if (self.orderSearch.searchTime.length>0) {
					var one = new Date(self.orderSearch.searchTime[0]);
					var two = new Date(self.orderSearch.searchTime[1]);
					 StartTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 EndTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
				}else{
					StartTime= ''
					EndTime = ''
				}
				var OrderTelephone = ''//电话
				var OrderName =''//收货人姓名
				var OrderId=''//订单编号
				var item= this.orderSearch.dropItem.value
				if (item==0) {//收货人
					OrderName = self.orderSearch.searchValue
				}else if (item==1) {//订单号
					OrderId = self.orderSearch.searchValue
				}else if (item==2) {//手机号
					OrderTelephone = self.orderSearch.searchValue
				}

				var DATA = '{"OrderType":0,"OrderState":"'+self.orderSearch.OrderState+'","OrderTelephone":"'+OrderTelephone +'","OrderId":"'+OrderId+'","OrderName":"'+OrderName+'","StartTime":"'+StartTime+'","EndTime":"'+EndTime+'","PageSize":20,"IndexPage":'+index+',"MemberId":'+self.MemberId+'}'
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ShoppingMall+'/api/Order/GetOrderList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {
			        	  var dataAll =JSON.parse(json)
			        	  //console.log(JSON.parse(json))
			        	  self.ordertotal = parseInt(dataAll.Count)
			        	  self.orderList= dataAll.data
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
			},//integralSearchBtn
			//-------------------------------------积分明细-----------------------------------------------
			integralChangePage(index){//分页
				this.requestIntegralData(index)
			},
			integralSearchBtn(){//积分明细搜索按钮
//				console.log(this.integralSearch)
				this.requestIntegralData(1)
			},
			requestIntegralData(index){//api/MemberIntegralDetail/FindMemberIntegralList
				var self =this

				var StartTime=''//开始时间
				var EndTime = ''//结束时间
				if (self.integralSearch.searchTime.length>0) {
					var one = new Date(self.integralSearch.searchTime[0]);
					var two = new Date(self.integralSearch.searchTime[1]);
					 StartTime = one.getFullYear() + '-' + (one.getMonth() + 1) + '-' + one.getDate()
					 EndTime = two.getFullYear() + '-' + (two.getMonth() + 1) + '-' + two.getDate()
				}else{
					StartTime= ''
					EndTime = ''
				}
				self.integralList =[]
				var DATA = '{"State":'+self.integralSearch.integralState+',"StartDate":"'+StartTime+'","EndDate":"'+EndTime+'","MemberId":'+self.MemberId+',"PageSize":20,"IndexPage":'+index+'}'
				//console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.user+'/api/MemberIntegralDetail/FindMemberIntegralList',///cts/core/
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        data:DATA,
			        success : function(json) {
			        	  	var dataAll =JSON.parse(json)
//			        	  console.log(JSON.parse(json))
			        	  	self.integraltotal = parseInt(dataAll.Count)
						dataAll.data.map((item) =>{
							if (item.IntegralType==2) {//支出
								item.IntegralTypeStr='支出'
							}else{
								item.IntegralTypeStr='收入'
							}
						})
			        	  self.integralList= dataAll.data
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
			},
			//-------------------------------------从业经历-----------------------------------------------
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
					//console.log(JSON.parse(json))
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
			},
			//--------------------------------------荣誉----------------------------------------------
			requestkudosData(){

			},
			//--------------------------------------认证资料----------------------------------------------
			certificationConfirmInformation(){//认证基本信息请求
				var self =this
				$.ajax({//图片
			        type :"post",
			        url :URLHeader.user+'/api/MemberAuth/GetMemberAuth?memberId='+self.MemberId,//此接口只针对实名认证(注册码认证不调用此接口)
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",
			        success : function(json) {// 1 为家庭
			        	 //var dataAll = JSON.parse(json)[0];
						console.log(json)
						if (json=='null') {//未 填写认证资料
							$('.cer').hide()
						} else{
							self.cerInformation = JSON.parse(json)
						}
//						self.cerInformation = {//会员展示信息  微信段填写的
//							AuthImg1:'../../../../../static/image/placeholderBag.png',//身份证
//							AuthImg2:'../../../../../static/image/placeholderBag.png',//身份证
//							AuthState:0,//1通过;-1未通过  0未审核
//							Remark:'',//未通过原因
//						}
						//录入信息 在基本信息中赋值
//						var infomation =self.infomation
//						console.log(infomation)
//						self.entryInformation = {//信息 录入
//							CardId:infomation.CardId,//省份证id
//							Birthday:infomation.Birthday,//生日
//							HomeAddress:infomation.HomeAddress,// 家庭地址
//							homeAre:[infomation.HomeProvinceId,infomation.HomeCityId,infomation.HomeAreaId],//家庭区域
//							HomeProvinceId:infomation.HomeProvinceId,//省id
//							HomeProvinceName:infomation.HomeProvinceName,//省名称
//							HomeCityId:infomation.HomeCityId,//城市id
//							HomeCityName:infomation.HomeCityName,//城市名称
//							HomeAreaId:infomation.HomeAreaId,//区id
//							HomeAreaName:infomation.HomeAreaName,//区名称
//							MatchName:infomation.MatchName,//烹饪比赛名称
//							MatchLv:infomation.MatchLv,//赛事级别
//							MatchNum:infomation.MatchNum,//比赛名次
//							Qualifications:infomation.Qualifications,//职业资格
//							DietitianLv:infomation.DietitianLv,//营养师等级
//							Honor:infomation.Honor,//荣誉职称
//							MemberId:infomation.MemberId,//
//						}

						//console.log(self.entryInformation)

			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
			},
			entryIfoUpData(){//信息上传
				$('.loding').show()
				var self =this
				var  infomation = self.entryInformation
				//省市区
				var area =$('.are input').val().split(' / ');//名字
				infomation.ProvinceName=area[0];//省
				infomation.CityName=area[1];//市
				infomation.AreaName=area[2];//区
				//homeAre
				infomation.HomeProvinceId = infomation.homeAre[0]
				infomation.HomeCityId = infomation.homeAre[1]
				infomation.HomeAreaId = infomation.homeAre[2]
				//生日
				console.log(infomation)
				if (infomation.Birthday!='') {
					//console.log('pp')
					var Time = new Date(infomation.Birthday);
					infomation.Birthday = Time.getFullYear() + '-' + (Time.getMonth() + 1) + '-' + Time.getDate()
				}else{
					infomation.Birthday  =''
				}

				var DATA= '{"CardId":"'+infomation.CardId+'","Birthday":"'+infomation.Birthday+'","HomeAddress":"'+infomation.HomeAddress+'","HomeProvinceId":"'+infomation.HomeProvinceId+'","HomeProvinceName":"'+infomation.HomeProvinceName+'","HomeCityId":"'+infomation.HomeCityId+'","HomeCityName":"'+infomation.HomeCityName+'","HomeAreaId":"'+infomation.HomeAreaId+'","HomeAreaName":"'+infomation.HomeAreaName+'","MatchName":"'+infomation.MatchName+'","MatchLv":"'+infomation.MatchLv+'","MatchNum":"'+infomation.MatchNum+'","Qualifications":"'+infomation.Qualifications+'","DietitianLv":"'+infomation.DietitianLv+'","Honor":"'+infomation.Honor+'","MemberId":'+infomation.MemberId+'}'
				console.log(DATA)
				$.ajax({//信息上传
			        type :"post",
			        url :URLHeader.user+'/api/MemberProfile/SetAuthInfo',
			        dataType : 'json',
			        data:DATA,
			        contentType: "application/json; charset=utf-8",
			        success : function(json) {// 1 为家庭
			        	 //var dataAll = JSON.parse(json)[0];
						console.log(json)
						if (json=='Exc Success') {
							self.$Message.success('录入数据上传成功');
							self.requestCookerInfomation()
						} else{
							self.$Message.error('录入数据上传失败');
						}
						$('.loding').hide()
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });
			},
			notThroughBtn(){//不通过
				this.notThrough = true
				console.log(this.notThrough)
			},
			cerAuditBtn(AuthState){//通过1  不通过-1  按钮
				var self =this
				var DATA
				$('.loding').show()
				if (AuthState=='1') {//通过
					DATA= '{"MemberId":'+self.MemberId+',"Remark":"审核通过","AuthState":'+AuthState+'}'
				} else{
					DATA= '{"MemberId":'+self.MemberId+',"Remark":"'+self.Remark+'","AuthState":'+AuthState+'}'
					//this.notThrough = false
				}
				console.log(DATA)
				$.ajax({//
			        type :"post",
			        url :URLHeader.user+'/api/MemberAuth/AuthSuccess',
			        dataType : 'json',
			        data:DATA,
			        contentType: "application/json; charset=utf-8",
			        success : function(json) {// 1 为家庭
			        	 //var dataAll = JSON.parse(json)[0];
						console.log(json)
						if (json=='Exc Success') {
							self.$Message.success('审核成功');

						} else{
							self.$Message.error('审核失败');
						}
						$('.loding').hide()
			        },
			        error : function(error) {
			        		console.log(error)
			        }
	   		    });

			},
			showImageModal(img){//大图
				this.modelImageSrc= img
				this.imgModal = true
			},


		},
		mounted:function(){
			$('.content .headerdiv').hide();
			$('.informationContent').show();
			var hash= window.location.hash.split('cookerAuditAfter?')[1].split('&');
			this.MemberId = hash[0].split('=')[1];
			//isShowQualified
			if (hash[1].split('=')[1]==0) {//0为未审批
				this.isShowHeader = false;//头部滑动
				this.isShowQualified=true;//合格按钮
			} else{
				this.isShowQualified=false;
				this.isShowHeader = true;
			}
			this.isShowQualified=false;
			this.isShowHeader = true;
			this.requestCookerInfomation()

			//this.requestkudosData();
			//=========订单
			this.requestOrderData(1)
			$('.ivu-table-header table,.ivu-table-body table').css({
				width:'100%'
			})
			//========积分明细
			this.requestIntegralData(1)
			//=======从业经历
			this.requestHotel();
			//=======认证资料
			this.areaData = Provence//区域 省市区
			this.certificationConfirmInformation()

		}
	}
</script>

<style scoped>
.cookerAuditAfter{
	margin: 0 auto;
	padding: 0;
}
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
	/*margin-top: 1rem;	*/
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
	width: 35%;
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
	/*height: 1.8rem;
	line-height: 1.8rem;*/
	min-height: 1.8rem;
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
/*订单*/
.orderContent .orderTopUl{
	overflow: auto;
	margin-bottom: 0.5rem;
}
.orderContent .orderTopUl .orderTopLi{
	display: block;
	float: left;
	margin-left: 2rem;
}
.orderContent .orderTopUl .orderTopLi .value{
	margin-left: 0.8rem;
}
.orderContent .orderTopUl .orderTopLi .lable{
	font-size: 0.8rem;
	font-weight: bold;
}
/*积分明细*/
.integralContent .integralTopUl{
	overflow: auto;
	margin-bottom: 0.5rem;
}
.integralContent .integralTopUl .integralTopLi{
	display: block;
	float: left;
	margin-left: 2rem;
}
.integralContent .integralTopUl .integralTopLi .value{
	margin-left: 0.8rem;
}
.integralContent .integralTopUl .integralTopLi .lable{
	font-size: 0.8rem;
	/*font-weight: bold;*/
}
/*荣誉资格*/
.kudos ul {
	width: 70%;
	margin: 0 auto;
	border: 1px dashed gainsboro;
	overflow: auto;
	border-radius:8px ;
	/*text-align: center;*/
}
.kudos ul li {
	height: 2rem;
	line-height: 2rem;
	display: block;
	float: left;
	width: 50%;
}
.kudos ul li .lable{
	margin-left: 3rem;
	font-weight: bold;
}
.kudos ul li .value{
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
/*认证资料*/
.cerUl{
	margin-left: 3rem;
	margin-top: 1rem;
	width: 50rem;
	overflow: auto;
}
.cerUl li{
	width: 25rem;
	float: left;
	margin-top: 0.5rem;
}
.cerUl li span:first-child{
	display: block;
	float: left;
}
.cerUl li span:last-child{
	margin-left: 1rem;
}
/*信息录入*/
.entryUl{
	margin-left: 3rem;margin-top: 1rem;border: 1px dashed gray;width: 43rem;padding: 1rem;border-radius: 4px;overflow: auto;
}
.entryUl .li{
	width: 20rem;overflow: auto;margin-top: 0.8rem;float: left;
}
.entryUl .lable{
	display: block;float: left;width: 6rem;
}
.entryUl .value{
	display: block;float: left;width: 12rem;margin-left: 0.5rem;
}
/*大图*/
.demo-upload-list{
    display: inline-block;
   /* width: 100px;*/
    /*height: 60px;*/
  	height: 8rem;
    text-align: center;
    border: 1px solid transparent;
    border-radius: 4px;
    overflow: hidden;
    background: #fff;
    position: relative;
    box-shadow: 0 1px 1px rgba(0,0,0,.2);
    margin-left: 3rem;
    margin-top: 1rem;
}
.demo-upload-list-cover{
    display: none;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: rgba(0,0,0,.6);
}
.demo-upload-list:hover .demo-upload-list-cover{
    display: block;
}
.demo-upload-list-cover i{
    color: #fff;
    font-size: 25px;
    cursor: pointer;
    margin: 0 2px;
    margin-top: 3rem;
}
</style>
