
<style scoped>
	/*html, body { min-height: 800px; }*/
	.ivu-menu-light {
		background: none;
	}
.layout {
        border: 1px solid #d7dde4;
        background: #f5f7f9;
        position: relative;
        overflow: hidden;
}

.layout-breadcrumb {
    padding: 10px 15px 0;
}

.layout-content {
   min-height: 578px;
    /*margin: 15px;*/
    /*margin-top: 1rem;*/
    overflow: hidden;
    background: #fff;
    border-radius: 4px;
}

.layout-content-main {
    padding: 20px 10px;
}

.layout-copy {
    text-align: center;
    padding: 10px 0 20px;
    color: #9ea7b4;
}

.layout-menu-left {
    /*background: #464c5b;*/
   /*background: white;*/
   background: #464c5b;
   background: none;
    /*min-width: 100px;*/
}

.layout-header {
    height: 60px;
    background: #fff;
    box-shadow: 0 1px 1px rgba(0, 0, 0, .1);
}

.layout-logo-left {
    width: 90%;
    height: 30px;
    background: #5b6270;
    border-radius: 3px;
    margin: 15px auto;
    line-height: 30px;
    color: white;
    text-align: center;
    vertical-align: middle;
    overflow: hidden;
}

.layout-ceiling-main a {
    color: #9ba7b5;
}

/*.layout-hide-text .layout-text {
    display: none;
}*/

.ivu-col {
    transition: width .2s ease-in-out;
}
.layout-ceiling-main{

	width: 100%;
}
.layout-ceiling{
       /* background: #464c5b;*/
       /* padding: 10px 0;*/
        overflow: hidden;
       /* background: url('../../assets/images/topHeader.png');*/
    /*  background-image: url(../../assets/img/topHeader.png);*/
   background: rgb(24,98,173);
   height: 4rem;
   line-height: 4rem;
   /*   background-size:10px ;*/

    }
.layout_top{
	color: white;
	float: right;
	padding: 0.3rem 0;
	margin-right: 15px;
}
.layout_top a {
	font-size: 0.8rem;
	font-weight: bold;
	color: white;
}
.layout-ceiling span{
	color: white;
	font-size: 1.5rem;
	margin-left: 1rem;
	font-weight: 700;
}
</style>
<style>
	html, body { min-width: 1200px; }
.layout-menu-left .ivu-menu-vertical.ivu-menu-light:after{
		width: 0;
	}
.layout-menu-left .ivu-menu-item-selected{
	background: lightgray;
}
.layout-menu-left .ivu-menu-vertical .ivu-menu-item:hover, .ivu-menu-vertical .ivu-menu-submenu-title:hover{
	background: gray;
}
.layout .ivu-menu-vertical .ivu-menu-submenu .ivu-menu-item{
	padding-left:2.8rem;
}

</style>
<template>

<div class="layout" :class="{'layout-hide-text': spanLeft < 5}">
	<div class="layout-ceiling-main">
            <div class="layout-ceiling">
            	<span>餐饮积分后台管理系统</span>
	            <div class="layout_top">
	                <!--<a >注册登录</a> |
	                <a>帮助中心</a> |
	                <a >安全中心</a> |
	                <a >服务大厅</a>-->
	                <a  style="margin-right: 2rem;">
	                   <p style="display: inline; font-size: 14px; font-weight: bold;margin-right: 0.3rem;">当前管理员：{{user}}</p>
	                   <Icon type="android-person" style='font-size: 1.5rem;'></Icon>
	                </a>
	                <!--<a href="login"  @click='cleanUser'>
		                <Icon type="log-out" style="font-size: 24px; margin-left: 20px;"></Icon>
		            </a>-->
	               <!-- <a href="login" v-on:click="cleanUser">退出登录</a>-->
	            </div>
	        </div>

            <!--<Dropdown style="margin-left: 20px">
                <a href="javascript:void(0)" @click='infoClick'>
                    <Icon type="android-notifications"></Icon>
                </a>
                <Dropdown-menu slot="list">
                    <Dropdown-item>系统消息</Dropdown-item>
                    <Dropdown-item>系统消息</Dropdown-item>
                    <Dropdown-item>系统消息</Dropdown-item>
                </Dropdown-menu>
            </Dropdown>-->


        </div>
    <Row type="flex">
        <i-col :span="spanLeft" class="layout-menu-left">
            <Menu :active-name="setActive" theme="light" width="auto" @on-select="routeTo">
                <!--<div class="layout-logo-left">
                    <h3>后台管理</h3>
                </div>-->
                 <Submenu name="1">
                 	<template slot="title">
                        <Icon type="ios-people"></Icon>
                        用户
                      </template>
                     <Menu-item name="players">
	                    <Icon type="android-contacts" :size="iconSize"></Icon>
	                    <span class="layout-text">队员</span>
	                </Menu-item>
	                <Menu-item name="cooker">
	                    <Icon type="android-people" :size="iconSize"></Icon>
	                    <span class="layout-text">会员</span>
	                </Menu-item>
                </Submenu>

                <Submenu name="2">
                		<template slot="title">
                        <Icon type="ios-keypad"></Icon>
                        知味活动
                     </template>
                		<Menu-item name="activitiesList">
                    		<Icon type="android-clipboard" :size="iconSize"></Icon>
                    		<span class="layout-text">活动列表</span>
               	   </Menu-item>
               	   <Menu-item name="TrainingCommunication">
                    		<Icon type="ios-pricetags-outline" :size="iconSize"></Icon>
                    		<span class="layout-text">培训交流</span>
               	   </Menu-item>

                	</Submenu>
                	<Submenu name="3">
                		<template slot="title">
                        <Icon type="android-list"></Icon>
                        页面设置
                     </template>
                		<!--<Menu-item name="PageList">
                    		<Icon type="document" :size="iconSize"></Icon>
                    		<span class="layout-text">页面内容列表</span>
               	   </Menu-item>-->
               	   <Menu-item name="shufflingList">
                    		<Icon type="images" :size="iconSize"></Icon>
                    		<span class="layout-text">轮播图列表</span>
               	   </Menu-item>

                	</Submenu>
                	<Submenu name="4">
                        <template slot="title">
                                <Icon type="android-cart"></Icon>
                            积分商城
                        </template>
                        <Menu-item name="GoodsList">
		                    <Icon type="ios-analytics" :size="iconSize"></Icon>
		                    <span class="layout-text">商品列表</span>
		                </Menu-item>
		                <Menu-item name="SingleList">
		                    <Icon type="aperture" :size="iconSize"></Icon>
		                    <span class="layout-text">单品列表</span>
		                </Menu-item>
		                 <Menu-item name="orderList">
		                   <Icon type="ios-paper-outline"></Icon>
		                    <span class="layout-text">订单列表</span>
		                </Menu-item>
                 </Submenu>
             	<Submenu name="5">
                    <template slot="title">
                            <Icon type="card"></Icon>
                        积分管理
                    </template>
                    <Menu-item name="IntegralrRuleList">
	                    <Icon type="ios-paper"></Icon>
	                    <span class="layout-text">积分规则</span>
	                </Menu-item>
	                <Menu-item name="IntegralGoodsList">
	                   <Icon type="disc"></Icon>
	                    <span class="layout-text">积分商品</span>
	                </Menu-item>
	                 <!--<Menu-item name="MemberIntegralList">
	                   <Icon type="person"></Icon>
	                    <span class="layout-text">会员积分</span>
	                </Menu-item>-->
	                <Menu-item name="ProductCodeList">
	                  <Icon type="ios-grid-view"></Icon>
	                    <span class="layout-text">产品二维码</span>
	                </Menu-item>
	                <!--<Menu-item name="IntegratingRedEnvelopeList">
	                   <Icon type="ios-briefcase"></Icon>
	                    <span class="layout-text">积分红包</span>
	                </Menu-item>-->
                 </Submenu>
                 <Submenu name="5">
                    <template slot="title">
                        	<Icon type="ios-body"></Icon>
                        营销活动
                    </template>
                    <Menu-item name="marketingList">
	                    <Icon type="android-document"></Icon>
	                    <span class="layout-text">营销列表</span>
	                </Menu-item>
                 </Submenu>
                 <Submenu name="5">
                    <template slot="title">
                        	<Icon type="cube"></Icon>
                        产品库
                    </template>
                    <Menu-item name="productList">
                    		<Icon type="leaf"></Icon>
	                    <span class="layout-text">产品</span>
	                </Menu-item>
                    <Menu-item name="feedBackFoodList">
	                    <Icon type="android-restaurant"></Icon>
	                    <span class="layout-text">反馈菜</span>
	                </Menu-item>
                 </Submenu>
                  <Submenu name="6">
                    <template slot="title">
                      <Icon type="cube"></Icon>
                      厨师节活动
                    </template>
                    <Menu-item name="lotteryshow">
                      <Icon type="leaf"></Icon>
                      <span class="layout-text">抽奖</span>
                    </Menu-item>
                    <Menu-item name="cookdrawprize">
                      <Icon type="android-restaurant"></Icon>
                      <span class="layout-text">后台</span>
                    </Menu-item>
                  </Submenu>
                  <Submenu name="7">
                    <template slot="title">
                      <Icon type="social-buffer"></Icon>
                    菜品库
                    </template>
                    <Menu-item name="cookbookList">
                      <Icon type="ios-medical-outline"></Icon>
                      <span class="layout-text">菜谱分级</span>
                    </Menu-item>
                    <Menu-item name="plateManagement">
                      <Icon type="ios-paper-outline"></Icon>
                      <span class="layout-text">板块管理</span>
                    </Menu-item>
                    <Menu-item name="dishesManagement">
                      <Icon type="happy-outline"></Icon>
                      <span class="layout-text">菜品内容管理</span>
                    </Menu-item>
                    <Menu-item name="topicPage">
                      <Icon type="happy-outline"></Icon>
                      <span class="layout-text">专题页面管理</span>
                    </Menu-item>
                  </Submenu>
            </Menu>


        </i-col>
        <i-col :span="spanRight">
            <!--<div class="layout-header">
                <i-button type="text" @click.native="toggleClick">
                    <Icon type="navicon" size="32"></Icon>
                </i-button>
            </div>-->
            <!--<div class="layout-breadcrumb">
                <Breadcrumb>
                    <Breadcrumb-item>首页</Breadcrumb-item>
                    <Breadcrumb-item>页面</Breadcrumb-item>
                    <Breadcrumb-item>{{this.$route.path.replace('/','')}}</Breadcrumb-item>
                </Breadcrumb>
            </div>-->
            <div class="layout-content">
                <div class="layout-content-main">
                    <transition mode="out-in">
                        <router-view></router-view>
                    </transition>
                </div>
            </div>
          	<div class="layout-copy">
                2015-2017 &copy; 餐饮积分系统
            </div>
        </i-col>
    </Row>

</div>

</template>

<script>

export default {

    data() {
            return {
            		user:sessionStorage.getItem('loginkey'),
                spanLeft: 3,
                spanRight: 21,
                page: ['about','form','table','markdown-viewer', 'markdown-editor-1', 'markdown-editor-2','rtf','upload','echarts']
            }
        },
        computed: {
            iconSize() {
                return this.spanLeft === 3 ? 14 : 24;
            },
            setActive() {
              return this.$route.path.replace('/','');
            }

        },
        methods: {
            toggleClick() {
                    if (this.spanLeft === 3) {
                        this.spanLeft = 1;

                        this.spanRight = 22;
                    } else {
                        this.spanLeft = 3;
                        this.spanRight = 21;
                    }
                },
             routeTo(e) {
                 // console.log(e);
//					if (e=='cooker') {
//						this.$router.push({path:'/'+e,query:{index:'1'}});
//					} else{
//						this.$router.push(e);
//					}
					this.clearsessionStorage()
                     this.$router.push(e);
                    if (e=='ProductCodeList') {//产品微微吗
                    	$('body,html').css({
                    		width:'1420px'
                    	})
//                  	console.log(123)
                    } else{
                    	$('body,html').css({
                    		 width:'auto'
                    	})
                    }

            },
            cleanUser(){
//         	 	sessionStorage.getItem('loginkey')
           	 	 sessionStorage.setItem("loginkey",'');
            },
            clearsessionStorage(){
            		sessionStorage.setItem("cookSearch",'{}');//会员搜索条件
            }
        },
        mounted:function(){
        		console.log(sessionStorage.getItem('loginkey'))
			$('.layout-menu-left .ivu-menu li').css({
                    	"border-right":"none"
                    })
        		if(!sessionStorage.getItem('loginkey')){
				  this.$router.push('login');
			}
        }
}

</script>
