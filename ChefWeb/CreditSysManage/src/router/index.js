import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);


export default new Router({
//	mode: 'history',//开起后可以去除#
    routes: [{
            path: '/',
            redirect: '/login'
        },
        {
            path: '/players',
            component: resolve => require(['../components/common/index.vue'], resolve),
            children: [{
                    path: '',
                    component: resolve => require(['../components/common/user/players/players.vue'], resolve)
                },
                //==================================用户=====================================
                {
                    path: '/players',//队员
                    component: resolve => require(['../components/common/user/players/players.vue'], resolve)

                },
                {
                    path: '/playersDetails',//队员详情页
                    component: resolve => require(['../components/common/user/players/playersDetails.vue'], resolve)
                },

                {
                    path: '/playersEditor',//队员编辑
                    component: resolve => require(['../components/common/user/players/playersEditor.vue'], resolve)
                },
                {
                    path: '/cooker',//厨师页面
                    component: resolve => require(['../components/common/user/members/cooker.vue'], resolve)
                },
                {
                    path: '/cookersDetails',//厨师详情页
                    component: resolve => require(['../components/common/user/members/cookersDetails.vue'], resolve)
                },
                {
                    path: '/cookerAuditAfter',//厨师详情页  审核后
                    component: resolve => require(['../components/common/user/members/cookerAuditAfter.vue'], resolve)
                },
                {
                    path: '/cookerEditor',//厨师编辑
                    component: resolve => require(['../components/common/user/members/cookerEditor.vue'], resolve)
                },
                //==================================知味活动=====================================
                {
                    path: '/activitiesList',//活动列表
                    component: resolve => require(['../components/common/KnowTasteActivities/activities/activitiesList.vue'], resolve)
                },
                 {
                    path: '/editorActivities',//活动编辑
                    component: resolve => require(['../components/common/KnowTasteActivities/activities/editorActivities.vue'], resolve)
                },
                {
                    path: '/teacherListAll',//导师列表
                    component: resolve => require(['../components/common/KnowTasteActivities/teacherList/teacherListAll.vue'], resolve)
                },
                {
                    path: '/teacherDetails',//导师详情
                    component: resolve => require(['../components/common/KnowTasteActivities/teacherList/teacherDetails.vue'], resolve)
                },
                {
                    path: '/addTeacher',//导师新增
                    component: resolve => require(['../components/common/KnowTasteActivities/teacherList/addTeacher.vue'], resolve)
                },
                {
                    path: '/teacherEdit',//导师编辑
                    component: resolve => require(['../components/common/KnowTasteActivities/teacherList/teacherEdit.vue'], resolve)
                },
                {
                    path: '/SeasoningConfiguration',//调料配置
                    component: resolve => require(['../components/common/KnowTasteActivities/Seasoning/SeasoningConfiguration.vue'], resolve)
                },
                 {
                    path: '/StarKitchenList',//星厨列表
                    component: resolve => require(['../components/common/KnowTasteActivities/StarKitchen/StarKitchenList.vue'], resolve)
                },
                 {
                    path: '/addStarKitchen',//星厨新增
                    component: resolve => require(['../components/common/KnowTasteActivities/StarKitchen/addStarKitchen.vue'], resolve)
                },
                 {
                    path: '/StarKitchenEdit',//星厨编辑
                    component: resolve => require(['../components/common/KnowTasteActivities/StarKitchen/StarKitchenEdit.vue'], resolve)
                },
                 {
                    path: '/StarKitchenDetail',//星厨详情
                    component: resolve => require(['../components/common/KnowTasteActivities/StarKitchen/StarKitchenDetail.vue'], resolve)
                },
                {
                    path: '/registrationInformation',//报名列表页
                    component: resolve => require(['../components/common/KnowTasteActivities/registrationInformation/registrationInformation.vue'], resolve)
                },
                 {
                    path: '/auditAdd',//报名新增
                    component: resolve => require(['../components/common/KnowTasteActivities/registrationInformation/auditAdd.vue'], resolve)
                },
                {
                    path: '/CheckDetails',//报名详情
                    component: resolve => require(['../components/common/KnowTasteActivities/registrationInformation/CheckDetails.vue'], resolve)
                },
                 {
                    path: '/auditView',//报名审核
                    component: resolve => require(['../components/common/KnowTasteActivities/registrationInformation/auditView.vue'], resolve)
                },
                {
                    path: '/enrolEdit',//报名编辑
                    component: resolve => require(['../components/common/KnowTasteActivities/registrationInformation/enrolEdit.vue'], resolve)
                },
                //=========活动交流========
                {
                    path: '/TrainingCommunication',//培训交流
                    component: resolve => require(['../components/common/KnowTasteActivities/TrainingCommunication/TrainingCommunication.vue'], resolve)
                },
                {
                    path: '/TrainingCommunicationAdd',//活动交流新增
                    component: resolve => require(['../components/common/KnowTasteActivities/TrainingCommunication/TrainingCommunicationAdd.vue'], resolve)
                },
                {
                    path: '/TrainingCommunicationEdit',//活动交流编辑
                    component: resolve => require(['../components/common/KnowTasteActivities/TrainingCommunication/TrainingCommunicationEdit.vue'], resolve)
                },
                //==================================页面设置=====================================.vue
                {
                    path: '/PageList',//页面设置列表页
                    component: resolve => require(['../components/common/PageSetup/PageContent/PageList.vue'], resolve)
                },
                {
                    path: '/PageDetail',//页面设置详情页
                    component: resolve => require(['../components/common/PageSetup/PageContent/PageDetail.vue'], resolve)
                },
                //======轮播图======
                {
                    path: '/shufflingList',//轮播图
                    component: resolve => require(['../components/common/PageSetup/shufflingImg/shufflingList.vue'], resolve)
                },
                 //==================================积分商城=====================================.vue
                {
                    path: '/GoodsList',//商品列表
                    component: resolve => require(['../components/common/ShoppingMall/Goods/GoodsList.vue'], resolve)
                },
                 {
                    path: '/commonGoodsAdd',//普通商品新增
                    component: resolve => require(['../components/common/ShoppingMall/Goods/commonGoodsAdd.vue'], resolve)
                },
                 {
                    path: '/commonGoodsEdit',//普通商品编辑
                    component: resolve => require(['../components/common/ShoppingMall/Goods/commonGoodsEdit.vue'], resolve)
                },
                 {
                    path: '/VirtualGoodsAdd',//虚拟商品新增
                    component: resolve => require(['../components/common/ShoppingMall/Goods/VirtualGoodsAdd.vue'], resolve)
                },
                 {
                    path: '/VirtualGoodsEdit',//虚拟商品编辑
                    component: resolve => require(['../components/common/ShoppingMall/Goods/VirtualGoodsEdit.vue'], resolve)
                },
                 //======单品======
                 {
                    path: '/SingleList',//虚拟商品编辑
                    component: resolve => require(['../components/common/ShoppingMall/SingleProduct/SingleList.vue'], resolve)
                },
                {
                    path: '/SingleList',//单品列表
                    component: resolve => require(['../components/common/ShoppingMall/SingleProduct/SingleList.vue'], resolve)
                },
                {
                    path: '/SingleAdd',//单品新增
                    component: resolve => require(['../components/common/ShoppingMall/SingleProduct/SingleAdd.vue'], resolve)
                },
                {
                    path: '/SingleEdit',//单品编辑
                    component: resolve => require(['../components/common/ShoppingMall/SingleProduct/SingleEdit.vue'], resolve)
                },
                {
                    path: '/SinglePromotions',//单品促销方案
                    component: resolve => require(['../components/common/ShoppingMall/SingleProduct/SinglePromotions.vue'], resolve)
                },
                 //======订单======
                  {
                    path: '/orderList',//订单列表
                    component: resolve => require(['../components/common/ShoppingMall/order/orderList.vue'], resolve)
                  },
                  {
                    path: '/orderDetails',//订单详情
                    component: resolve => require(['../components/common/ShoppingMall/order/orderDetails.vue'], resolve)
                  },
                  //==================================积分管理=====================================.vue
                 //======积分规则======
                 {
                    path: '/IntegralrRuleList',//积分规则列表
                    component: resolve => require(['../components/common/IntegralManagement/IntegralrRule/IntegralrRuleList.vue'], resolve)
                  },
                  //======积分商品======
                 {
                    path: '/IntegralGoodsList',//积分商品列表
                    component: resolve => require(['../components/common/IntegralManagement/IntegralGoods/IntegralGoodsList.vue'], resolve)
                  },
                 //======会员积分======
                 {
                    path: '/MemberIntegralList',//会员积分列表
                    component: resolve => require(['../components/common/IntegralManagement/MemberIntegral/MemberIntegralList.vue'], resolve)
                  },
                  //======产品二维码======
                 {
                    path: '/ProductCodeList',//产品二维码列表
                    component: resolve => require(['../components/common/IntegralManagement/ProductCode/ProductCodeList.vue'], resolve)
                  },
                  //======积分红包======
                 {
                    path: '/IntegratingRedEnvelopeList',//积分红包列表
                    component: resolve => require(['../components/common/IntegralManagement/IntegratingRedEnvelope/IntegratingRedEnvelopeList.vue'], resolve)
                  },
                  //==================================营销活动=====================================.vue
                  {
                    path: '/marketingList',//营销活动列表
                    component: resolve => require(['../components/common/marketingActivities/marketingList.vue'], resolve)
                  },
                  {
                    path: '/marketingAdd',//活动新增
                    component: resolve => require(['../components/common/marketingActivities/marketingAdd.vue'], resolve)
                  },
                  {
                    path: '/signInToView',//签到查看
                    component: resolve => require(['../components/common/marketingActivities/signInToView.vue'], resolve)
                  },
                  {
                    path: '/lotteryToSee',//抽奖查看
                    component: resolve => require(['../components/common/marketingActivities/lotteryToSee.vue'], resolve)
                  },
                  //==================================产品库=====================================.vue
                   //======反馈菜======
                 {
                    path: '/feedBackFoodList',//反馈菜列表
                    component: resolve => require(['../components/common/productLibrary/feedBackFood/feedBackFoodList.vue'], resolve)
                  },
                  {
                    path: '/feedBackAudit',//反馈菜审核
                    component: resolve => require(['../components/common/productLibrary/feedBackFood/feedBackAudit.vue'], resolve)
                  },
                   {
                    path: '/feedBackEditor',//反馈菜编辑
                    component: resolve => require(['../components/common/productLibrary/feedBackFood/feedBackEditor.vue'], resolve)
                  },
                   //======产品======
                 {
                    path: '/productList',//产品列表
                    component: resolve => require(['../components/common/productLibrary/product/productList.vue'], resolve)
                  },
                  {
                    path: '/edit&Add',//产品编辑与新增
                    component: resolve => require(['../components/common/productLibrary/product/edit&Add.vue'], resolve)
                  },
                  {
                    path: '/productView',//产品详情
                    component: resolve => require(['../components/common/productLibrary/product/productView.vue'], resolve)
                  },
                  //=======厨师节活动=========
                  {
                    path: '/cookdrawprize',//产品详情
                    component: resolve => require(['../components/common/DrawPrize/cookdrawprize.vue'], resolve)
                  },
                  //=======菜品库=========
                  {
                    path: '/cookbookList',//菜谱分级
                    component: resolve => require(['../components/common/DishLibrary/cookbookList.vue'], resolve)
                  },
                  {
                    path: '/plateManagement',//板块管理
                    component: resolve => require(['../components/common/DishLibrary/plateManagement.vue'], resolve)
                  },
                  {
                    path: '/dishesManagement',//菜品内容管理
                    component: resolve => require(['../components/common/DishLibrary/dishesManagement.vue'], resolve)
                  },
              {
                path: '/topicPage',//菜品内容管理
                component: resolve => require(['../components/common/DishLibrary/topicPage.vue'], resolve)
              },


//              {
//                  path: '/form',
//                  component: resolve => require(['../components/page/form.vue'], resolve)
//              },
//              {
//                  path: '/table',
//                  component: resolve => require(['../components/page/table.vue'], resolve)
//              },
//              {
//                  path: '/markdown-viewer',
//                  component: resolve => require(['../components/page/markdown-viewer.vue'], resolve)
//              },
//              {
//                  path: '/markdown-editor-1',
//                  component: resolve => require(['../components/page/markdown-editor-1.vue'], resolve)
//              },
//              {
//                  path: '/markdown-editor-2',
//                  component: resolve => require(['../components/page/markdown-editor-2.vue'], resolve)
//              },
//              {
//                  path: '/rtf',
//                  component: resolve => require(['../components/page/rtf.vue'], resolve)
//              },{
//                  path: '/upload',
//                  component: resolve => require(['../components/page/upload.vue'], resolve)
//              },{
//                  path: '/echarts',
//                  component: resolve => require(['../components/page/echarts.vue'], resolve)
//              }
            ]
        },
        {
            path: '/login',
            component: resolve => require(['../components/common/login.vue'], resolve)
        },
      /*
      * 临时路径，抽奖活动页面pc
      */
      {
        path: '/lottery',
        component: resolve => require(['../components/common/Draw/lottery.vue'], resolve)
      },
      {
        path: '/lotteryshow',
        component: resolve => require(['../components/common/Draw/lotteryshow.vue'], resolve)
      }
    ]
})
