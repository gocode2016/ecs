<template>
  <div class="enterstep">
    <div class="stepBox">
      <step v-model="step" background-color='#fbf9fe' gutter="10px">
        <step-item title="基本信息" style="overflow: visible;"></step-item>
        <step-item title="创新菜" style="overflow: visible;"></step-item>
        <step-item title="本地菜" style="overflow: visible; flex: 0;" class="third"></step-item>
      </step>
    </div>

    <!-- 各步骤的表单内容 -->
    <div>
     <!-- :min-moving-distance = 100000 :height="currentStepH" --> 
     <!-- 该属性限定左右滑动，去掉该属性方便查看调试，正式调试完成，添加该属性，禁止左右滑动:min-moving-distance = 100000 -->
      <swiper v-model="index" :show-dots="false" height="2000px" :min-moving-distance = 100000>
        <swiper-item :key="1">
          <div class="tab-swiper vux-center">
            <!-- 基本信息表单start -->
            <div class="enterstepst">
              <div>
                  <!--基本信息-->
                  <group>
                    <x-input title="赛区选择" @on-focus="alertZone" v-model="areacity"></x-input>            
                  </group>
                  <group gutter='0.4rem'>
                    <popup-picker title="报名途径" :data="road" v-model="roadText" cancel-text='取消' confirm-text='确定' value-text-align ='left' @on-hide='recom'></popup-picker>
                  </group>
                  <group gutter='0.4rem' v-show='flag'>
                    <x-input title="推荐人" v-model="refer"></x-input>            
                  </group>
                  <group gutter='0.4rem'>       
                    <datetime title="出生日期" v-model="timeText" :min-year=1960 :max-year=2017 cancel-text='取消' confirm-text='确定' value-text-align ='left'></datetime>
                  </group>        
                  <div class="photoshow">
                    <div @click='imgUpload("photo")'><img :src='ChefPicUrl'></div>
                    <div @click='imgUpload("hotelPhoto")'><img :src='ChefHotelPicUrl'></div>
                  </div>
                  <div class="btnDiv">
                    <button @click="save1">保存</button><button @click="nextStep1">下一步</button>         
                  </div>       
              </div>
              <!--赛区选择-->
              <div v-transfer-dom>
                <popup v-model="show" position="top">
                  <div class="popup0">
                    <div class="mask">
                      <div class='zone'>
                        <!--<input class="search" placeholder="输入所选择的赛区"/>-->
                        <div class="entDiv">										    	
										    	<input type="text" placeholder="输入所选择的赛区" v-model="content" @keyup="changeArea"/> 
										    	<img src="../../static/credit/search.png" class="searchImg" @click="lookup"/>
										    </div>
										    
                        <div class="searchDiv">
                          <div v-show="searchResultShow" style="color: #e83428; text-align: center; line-height:40px; font-size: 14px;">暂无此赛区，请确认后重新输入。</div>
                          <div class="cityDiv" v-for="(item,index) in searchAreaList" :key="index">
                            <span>{{item.ProvinceName}}</span>
                            <checker v-model="areacityItem" default-item-class="areacity-item" selected-item-class="areacity-item-selected">
                              <checker-item v-for="(i,index) in item.AreaName" :key="index" :value="i.AreaName" @click.native="MatchRegionId(i.MatchRegionId)" style="width: auto;">{{i.AreaName}}</checker-item>
                            </checker>
                          </div>          
                        </div>
                        <div class="areaBtn"><button @click="cancel">取消</button><button @click="confirm">确定</button></div>        
                      </div>
                    </div> 
                  </div>
                </popup>
              </div>
            </div>
            <!-- 基本信息表单end -->
          </div>
        </swiper-item>
        <swiper-item :key="2">
          <div class="tab-swiper vux-center">
          <!-- 创新菜start -->
          <div class="enterstepnd">
            <div class="upload" @click='imgUpload("dishPhoto1")'><span v-if="dishShow">+菜品信息</span><img v-else :src='stepnd.dishPicUrl'></div>
            <input type="text" placeholder="请填写菜品名字" class="dishname" v-model="stepnd.dishName"/>
            <!--菜品故事-->
            <div class="alike show">
               <img src="../../static/credit/book1.png" />
               <span>菜品故事</span>
            </div>
            <group>
              <x-textarea placeholder="说说这道菜背后的故事吧~"  v-model="stepnd.dishStory"></x-textarea>
            </group>
            <!--备料展示-->
             <div class="alike show">
               <img src="../../static/credit/show.png" />
               <span>备料展示</span>
             </div>    
             <!--主料-->
             <p class="zliao">主料</p>
             <ul class="main">
               <li>
                 <input class="maintext" type="text" placeholder="食材">
                 <input class="weight" type="text"  placeholder="用量">
                 <span class="ke">g&nbsp;</span>
                 <img src="../../static/credit/jian.png" class="rem"/>               
               </li>      
             </ul> 
             <p class="add"><img src="../../static/credit/jia.png" @click="addZL"/></p>
             <!--辅料-->
             <p class="zliao">辅料</p>
             <ul class="access">
               <li>
                 <input class="maintext" type="text" placeholder="食材">
                 <input class="weight" type="text" placeholder="用量">
                 <span class="ke">g&nbsp;</span>
                 <img src="../../static/credit/jian.png" class="rem"/>                 
               </li>       
             </ul>
             <p class="add"><img src="../../static/credit/jia.png" @click="addFL"/></p>
              <!--调料-->
             <p class="zliao">调料</p>
             <p class="seasonText">(以下调味品中至少选择一样作为调料)</p>
             <checker v-model="stepnd.checkList" type="checkbox" default-item-class="season-item" selected-item-class="season-item-selected">
               <checker-item v-for="(item,index) in list" :key="index" :value="index" @on-item-click="onItemClick(item.FlavourName,index,item.FlavourRecId)"><img v-bind:src="item.FlavourPicUrl"><p>{{item.FlavourName}}</p></checker-item>        
             </checker>
              <!--推荐调料展示-->
             <ul class="season">
             	  <li>
	                 <input class="maintext" type="text" placeholder="调料" readonly="readonly">
	                 <input class="weight" type="text" placeholder="用量">
	                 <span class="ke">g&nbsp;</span>
	                 <img src="../../static/credit/jian.png" class="rem"/>                 
                </li>  
             </ul>            
             <p class="seasonText">(还需要其他调料吗？请写在下面)</p>
             <ul class="othersea">
               <li>
                 <input class="maintext" type="text" placeholder="调料">
                 <input class="weight" type="text" placeholder="用量">
                 <span>g&nbsp;</span>
                 <img src="../../static/credit/jian.png" class="rem"/>               
               </li>
             </ul>
             <p class="add"><img src="../../static/credit/jia.png" @click="addOther"/></p>
             <!--烹饪步骤-->
             <div class="alike show">
                 <img src="../../static/credit/cook.png" />
                 <span>烹饪步骤</span>
             </div> 
             <ul class='cookStep'>
               <li>
                  <span>1、</span>        
                  <textarea class="stepDetail" placeholder="添加步骤说明"></textarea>        
                  <img src="../../static/credit/jian.png" class="rem"/>
               </li>
             </ul>
             <p class="add"><img src="../../static/credit/jia.png" @click="addCookStep"/></p>
             <!--按钮-->
             <div class="btnDiv"><button @click="save2">保存</button><button @click="nextStep2">下一步</button></div>
          </div>
          <!-- 创新菜end -->
          </div>
        </swiper-item>
        <swiper-item :key="3">
          <div class="tab-swiper vux-center">
            <!-- 本地菜 start -->
            <div class="entersteprd">
              <div class="upload" @click='imgUpload("dishPhoto2")'><span v-if="dishLocalShow">+菜品照片</span><img v-else :src='steprd.dishPicUrl'></div>
              <input type="text" placeholder="请填写菜品名字" class="dishname" v-model="steprd.dishName"/>
              <!--菜品故事-->
              <div class="alike show">
                 <img src="../../static/credit/book1.png" />
                 <span>菜品故事</span>
              </div>
              <group>
                <x-textarea placeholder="说说这道菜背后的故事吧~" v-model="steprd.dishStory"></x-textarea>
              </group>
              <!--备料展示-->
              <div class="alike show">
                <img src="../../static/credit/show.png" />
                <span>备料展示</span>
              </div>    
             <!--主料-->
              <p class="zliao">主料</p>
              <ul class="main">
                <li>
                  <input class="maintext" type="text" placeholder="食材">
                  <input class="weight" type="text" placeholder="用量">
                  <span class="ke">g&nbsp;</span>
                  <img src="../../static/credit/jian.png" class="rem"/>               
                </li>      
              </ul> 
              <p class="add"><img src="../../static/credit/jia.png" @click="addZL1"/></p>
              <!--辅料-->
              <p class="zliao">辅料</p>
              <ul class="access">
                <li>
                  <input class="maintext" type="text" placeholder="食材">
                  <input class="weight" type="text" placeholder="用量">
                  <span class="ke">g&nbsp;</span>
                  <img src="../../static/credit/jian.png" class="rem"/>
                </li>
              </ul>
              <p class="add"><img src="../../static/credit/jia.png" @click="addFL1"/></p>
              <!--调料-->
              <p class="zliao">调料</p>
              <p class="seasonText">(以下调味品中至少选择一样作为调料)</p>     
              <checker v-model="steprd.checkList" type="checkbox" default-item-class="season-item" selected-item-class="season-item-selected">
                <checker-item v-for="(item,index) in list" :key="index" :value="index" @on-item-click="onItemClick1(item.FlavourName,index,item.FlavourRecId)"><img v-bind:src="item.FlavourPicUrl"><p>{{item.FlavourName}}</p></checker-item>
                </checker>
              <!--推荐调料展示-->
              <ul class="season">
              	<li>
	                 <input class="maintext" type="text" placeholder="调料" readonly="readonly">
	                 <input class="weight" type="text" placeholder="用量">
	                 <span class="ke">g&nbsp;</span>
	                 <img src="../../static/credit/jian.png" class="rem"/>                 
                </li> 
              </ul>
              <p class="seasonText">(还需要其他调料吗？请写在下面)</p>
              <ul class="othersea">
                <li>
                  <input class="maintext" type="text" placeholder="调料">
                  <input class="weight" type="text" placeholder="用量">
                  <span>g&nbsp;</span>
                  <img src="../../static/credit/jian.png" class="rem"/>               
                </li>
              </ul>
              <p class="add"><img src="../../static/credit/jia.png" @click="addOther1"/></p>
              <!--烹饪步骤-->
              <div class="alike show">
                <img src="../../static/credit/cook.png"/>
                <span>烹饪步骤</span>
              </div> 
              <ul class='cookStep'>
                <li>
                  <span>1、</span>
                  <textarea class="stepDetail" placeholder="添加步骤说明"></textarea>
                  <img src="../../static/credit/jian.png" class="rem"/>
                </li>
              </ul>
              <p class="add"><img src="../../static/credit/jia.png" @click="addCookStep1"/></p>
              <!--按钮-->
              <div class="btnDiv"><button @click="save3">保存</button><button @click="nextStep3">提交</button></div>
            </div>
            <!-- 本地菜 end -->
          </div>
        </swiper-item>
      </swiper>
    </div>

    <!--点击下一步信息不完整提示-->
    <div class="alert-mask" v-show='alertFlag'>
      <div class="menu">
         <p>填写完整才可以进入到下一步喔！点击【保存】按钮可以随时保存进度</p>
         <button @click="menuBtn">知道了</button>
      </div>
    </div>
    <!--点击保存信息不完整提示-->
    <div class="alert-mask" v-show='saveShow'>
      <div class="menu menus">
         <p>{{saveText}}</p>
         <button @click="menuBtn">知道了</button>
      </div>
    </div>
    <!--基本信息保存成功页面-->
    <div class="saveDiv" v-show='saveFlag'>
      <img src="../../static/credit/success.png" />
      <p>保存成功</p>
      <p>再次进入点击报名可继续填写</p>
      <button @click="closeSave">关闭</button>
    </div>
    <!--提交成功页面-->
    <div class="sucDiv" v-show='sucFlag'>
      <img src="../../static/credit/success.png" />
      <p>恭喜你！提交成功</p>
      <p>24小时内将告知审核结果</p>
      <button @click="closeBtn">关闭</button>
    </div>
    <!--报名成功页面-->
	  <div class="successDiv" v-show="successFlag">
	    <img src="../../static/credit/sucful.png"/>
	  </div>
	  <!--提示用户注册-->
    <div class="alert-mask" v-show="promptZcFlag">
      <div class="prompt-zc">
        <p>注册后才能报名哦</p>
        <button @click="goregister">去注册</button>
      </div>
    </div>
  </div>
</template>

<script>
import apiUrl from "../apiUrls.js"
import wx from 'weixin-js-sdk'
import { Step, StepItem, XButton, Tab, TabItem, Swiper, SwiperItem, Popup, XSwitch } from 'vux'
import { TransferDom, Group, XInput, PopupPicker, Checker, CheckerItem, Datetime } from 'vux'
import { XTextarea ,cookie} from 'vux'

// 调料图未被选中时状态
var tlArr=[0,0,0,0,0,0];         // 创新菜调料标识
var localTLArr=[0,0,0,0,0,0];    // 本地菜调料标识
var matchid='';//暂时记录赛区id
var cs=0;//记录调料是否为数字
export default {
  directives: {
    TransferDom
  },
  components: {
    Step, StepItem, XButton, Tab, TabItem, Swiper, SwiperItem, Popup, XSwitch,
    Group, XInput, PopupPicker, Checker, CheckerItem, Datetime, XTextarea,cookie
  },
  data () {
    return {
    	url:'',
    	dishShow:true,
    	dishLocalShow:true,
    	promptZcFlag:false,//提示用户注册
    	userData:{},//获取的cookie值
    	timestamp:'',
  		nonceStr:'',
  		signature:'',
      step: 0,
      index: 0,
      currentStepH: '0px',
      list:[],
      saveText:'',
      // 基本信息
      show: false,
      roadText: [],          //报名途径
      road: [['自我推荐','行业大师推荐','酒店推荐','欣和业务推荐']],
      timeText: '',          //出生日期
      flag: false,           //显示推荐人
      alertFlag: false,      //显示信息不完整提示
      saveFlag: false,       //显示保存成功页面
      sucFlag:false,         //显示提交成功页面
      successFlag:false,     //显示报名成功页面
      saveShow:false,        //点击保存提示信息不完整
      searchResultShow: false, //赛区搜索结果
      searchAreaList: [
          {
            AreaName: [
              {
                AreaName: "济南",
                ChefActivityId: 53,
                MatchRegionId: 2457
              },
              {
                AreaName: "潍坊",
                ChefActivityId: 53,
                MatchRegionId: 2459
              }
            ],
            ProvinceName: '山东'
          },
          {
            AreaName: [
              {
                AreaName: "德州",
                ChefActivityId: 52,
                MatchRegionId: 2451
              },
              {
                AreaName: "莱阳",
                ChefActivityId: 52,
                MatchRegionId: 2452
              }
            ],
            ProvinceName: '山西'
          },
          {
            AreaName: [
              {
                AreaName: "廊坊",
                ChefActivityId: 51,
                MatchRegionId: 2421
              },
              {
                AreaName: "北海",
                ChefActivityId: 51,
                MatchRegionId: 2432
              }
            ],
            ProvinceName: '河北'
          }
      ],    // 搜索联动筛选赛区列表
      areaList: [
          {
            AreaName: [
              {
                AreaName: "济南",
                ChefActivityId: 53,
                MatchRegionId: 2457
              },
              {
                AreaName: "潍坊",
                ChefActivityId: 53,
                MatchRegionId: 2459
              }
            ],
            ProvinceName: '山东'
          },
          {
            AreaName: [
              {
                AreaName: "德州",
                ChefActivityId: 52,
                MatchRegionId: 2451
              },
              {
                AreaName: "莱阳",
                ChefActivityId: 52,
                MatchRegionId: 2452
              }
            ],
            ProvinceName: '山西'
          },
          {
            AreaName: [
              {
                AreaName: "廊坊",
                ChefActivityId: 51,
                MatchRegionId: 2421
              },
              {
                AreaName: "北海",
                ChefActivityId: 51,
                MatchRegionId: 2432
              }
            ],
            ProvinceName: '河北'
          }
      ],
      areacityItem: '',
      content:'',        //搜索内容
      areacity: '',          //赛区
      matchId: '',           //赛区id
      refer: '',             //推荐人
      MemberId: '',           //用户id传参
      ChefActivityId:'',     //活动id传参
      ChefPicUrl: '../../static/credit/personback.png',    //个人照片
      ChefHotelPicUrl: '../../static/credit/hotelback.png', //酒店照片

      // 创新菜提交参数
      stepnd: {
        dishName: '',          // 菜名
        checkList: '',         // 已选择图片调料
        chefId:1,
	    	dishStory:'' ,
	    	dishPicUrl:'' ,
	    	dishId:1,
      },
      // 本地菜提交参数
      steprd: {
        dishName: '',          // 菜名
        checkList: '',         // 已选择图片调料
	    	dishStory:'' ,
	    	dishPicUrl:'' ,
	    	dishId:1,
      }

    }
  },
  methods: {
    changeArea(){
      // 搜索框输入变化动态筛选赛区列表
      var searchKey = this.content;
      var areaList = this.areaList;
      var searchAreaList = new Array();
      for(let i in areaList) {
        var provinceName = areaList[i].ProvinceName;
        if(provinceName.indexOf(searchKey) > -1 )
        {
            searchAreaList.push(areaList[i]);
        }else{
            var areaNameArr = areaList[i].AreaName;
            for(let j in areaNameArr) {
              var areaName = areaNameArr[j].AreaName;
              if(areaName.indexOf(searchKey) > -1) {
                searchAreaList.push(areaList[i]);
              }
            }
        }
      }
      this.searchResultShow = false;
      if(searchKey) {
        this.searchAreaList = searchAreaList;
        searchAreaList.length == 0 ? this.searchResultShow = true : '';
      }else{
        this.searchAreaList = areaList;
      }

      // end
    },
  	isWeight(val,isnext){//判断调料用量是否为数字
  		var reg = /^[0-9]+$/;    	
      var tlArr=[];
      cs=0;//调料为数字
      for(var i=0;i<$('.weight').length;i++){
	    	var weight=$('.weight').eq(i).val();
	    	tlArr.push(weight);
      }
      for(var i=0;i<tlArr.length;i++){
      	var tl=tlArr[i];
   	    if(tl!=''&&!reg.test(tl)){
				  this.saveText='调料的用量必须是数字哦';
				  this.saveShow=true;
				  cs=1;//调料不是数字
			  }
      }
      
      if(cs==0&&val=='save2'){        
        this.addDishChef(isnext);
      }else if(cs==0&&val=='save3'){
        this.addLocalDishChef(isnext);
      }

  	},
  	isApply(){//判断当前活动下是否可以报名
  		var _this=this;
  		var params={
  		  "MemberId":this.MemberId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.isApply,
  			data:params
  		}).then(function(response){
  			var data=response.data;
  			if(data=="0"){//审核中 不能报名
  				_this.sucFlag=true;
  			}else if(data=="1"){//审核通过 不能报名
  				_this.successFlag=true;
  			}else if(data=="-2"||"-1"||"2"||"4"){//可以报名
  				_this.getArea();//获取赛区
  			} 			
  		})
  	},
  	setConfig(){
  		  var _this=this;
		  	this.$http({
		  		method:'GET',
		  		url:apiUrl.getTimestampAndNonceStr
		  	}).then(function(response){
		  		var data=response.data;
		  		var dataArr=data.split('|');
		  		_this.timestamp=parseInt(dataArr[0]);//时间戳
		  		_this.nonceStr=dataArr[1];//随机字符串
		  		
		      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
		  		_this.$http({
		  			method:'GET',
		  			url:apiUrl.getJsapiTicketSignature+params
		  		}).then(function(response){
		  			 _this.signature=response.data;
		  		})
		  	})	
  	},
  	imgUpload(val){//上传头像
  		var _this=this;
      var localIds ='';
	    wx.config({
	      debug: false,
	      appId: 'wxa9067ffebe95ca17',
	      timestamp: _this.timestamp,
	      nonceStr: _this.nonceStr,
	      signature: _this.signature,
	      jsApiList: [
	        'chooseImage',
	        'uploadImage'
	      ]
			})
  	  wx.chooseImage({
		     count: 1, // 默认9
			   sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
			   sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
			   success: function (res){
		        localIds = res.localIds;				    
				    // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
				    // localId 可以用于微信手机版图片显示（目前PC版不可用）
				    if(val=='photo'){
				    	_this.ChefPicUrl=localIds[0];
				    }else if(val=='hotelPhoto'){
				    	_this.ChefHotelPicUrl=localIds[0];
				    }else if(val=='dishPhoto1'){
				    	_this.dishShow=false;
				    	_this.stepnd.dishPicUrl=localIds[0];
				    }else if(val=='dishPhoto2'){
				    	_this.dishLocalShow=false;
				    	_this.steprd.dishPicUrl=localIds[0];
				    }
				    wx.uploadImage({
						  localId:localIds[0] , // 需要上传的图片的本地ID，由chooseImage接口获得
						  isShowProgressTips: 1, // 默认为1，显示进度提示
						  success: function (res) {
							  var serverId = res.serverId; // 返回图片的服务器端ID
		            _this.$http({
		            	method:'GET',
		            	url:apiUrl.saveWeChatImage+'?serverId='+serverId
		            }).then(function(res){ 
		            	if(val=='photo'){
		            		_this.ChefPicUrl=apiUrl.getUrl+res.data;
		            	}else if(val=='hotelPhoto'){
		            		_this.ChefHotelPicUrl=apiUrl.getUrl+res.data;
		            	}else if(val=='dishPhoto1'){
							    	 _this.stepnd.dishPicUrl=apiUrl.getUrl+res.data;
							    }else if(val=='dishPhoto2')		{
							    	 _this.steprd.dishPicUrl=apiUrl.getUrl+res.data;
							    }                         	
		            })
						  }
				    })
				  },
				  fail:function(res){
				  	alert(res);
				  }
      })						 
  	},
    // 测试步骤切换流畅显示，正式发布时使用 nextStep1、nextStep2、nextStep3;
    nextStep () {
      this.step ++;
      this.index == 2 ? '' : this.index = this.step;
      if(this.index == 1) {
        // 设置创新菜的高度
        var step2H = $('.enterstepnd').height() + 45;
        $('.vux-swiper').height(step2H);
      }else{
        // 设置本地菜的高度
        var step3H = $('.entersteprd').height() + 45;
        $('.vux-swiper').height(step3H);
      }
    },
    getArea(){//获取赛区API
      var _this=this;
      this.$http({
        method:"GET",
        url:apiUrl.getMatch
      }).then(function(response){
        _this.areaList=JSON.parse(response.data);
        _this.searchAreaList=JSON.parse(response.data);
        _this.ChefActivityId=_this.areaList[0].AreaName[0].ChefActivityId;
        _this.getChef();//获取厨师信息
        _this.getFRChef();//获取推荐调料
      })
    },
    lookup(){
//  	if(this.content!=''){
//  		console.log(this.areaList);
//  	}
    },
    getFRChef(){//获取推荐调料
    	var _this=this;
    	var params='?caId='+this.ChefActivityId;
    	this.$http({
    		method:'GET',
    		url:apiUrl.getFRChef+params,
    	}).then(function(response){
    		_this.list=JSON.parse(response.data);
    		_this.getDishChef();//获取创新菜信息
			  _this.getLocalDishChef();//获取本地菜信息
    	})
    },
    addChef(){//新增厨师基本信息API
    	var _this=this;
      var params={
        "MemberId":this.MemberId,
        "ChefActivityId":this.ChefActivityId,
        "MatchRegionId":this.matchId,
        "ApplyType":this.roadText[0],
        "ReferrerName":this.refer,
        "Birthday":this.timeText,
        "ChefPicUrl":this.ChefPicUrl,
        "ChefHotelPicUrl":this.ChefHotelPicUrl
      }
      this.$http({
        method:"POST",
        url:apiUrl.addChef,
        data:params
      }).then(function(response){
      	  var data=JSON.parse(response.data);        
      		_this.stepnd.chefId=data;            
      })
    },
    getChef(){//获取厨师基本信息API
      var _this=this;
      this.$http({
        method:"GET",
        url:apiUrl.getChefMess+'?mbId='+this.MemberId+'&caId='+this.ChefActivityId
      }).then(function(response){      	
      	var datas=JSON.parse(response.data);
      	if(datas.length!=0){
      		var data=datas[0];
	        _this.areacity=data.AreaName;//赛区
	        _this.areacityItem=data.AreaName;
	        _this.roadText.push(data.ApplyType);//报名途径
	        _this.timeText=data.Birthday;//出生日期
	        _this.matchId=data.MatchRegionId;//赛区id        
	        _this.MemberId=data.MemberId;//参数
	        _this.ChefActivityId=data.ChefActivityId;//参数
	        if(data.ChefPicUrl!=''){
	        	_this.ChefPicUrl=data.ChefPicUrl;//个人照片
	        }
	        if(data.ChefHotelPicUrl!=''){
	        	_this.ChefHotelPicUrl=data.ChefHotelPicUrl;//酒店照片
	        }	        
	        if(_this.roadText[0]!='自我推荐'&&_this.roadText[0]!=''){
	          _this.flag=true;
	          _this.refer=data.ReferrerName;//推荐人
	        }  
      	}             
      })
    },
    MatchRegionId(id){//获取赛区id
    	matchid=id;     
    },
    alertZone(){
      this.show ? this.show = false : this.show = true;
    },   
    recom(){
      if(this.roadText!='自我推荐'&&this.roadText!=''){
        this.flag=true;
      }else{
        this.flag=false;
      }
    },
    menuBtn(){
      this.alertFlag=false;
      this.saveShow=false;
    },
    cancel(){
      this.show ? this.show = false : this.show = true;
    },
    confirm(){
      this.show ? this.show = false : this.show = true;
      this.areacity=this.areacityItem;
      this.matchId=matchid;
    },
    nextStep1 () {//基本信息的下一步     
      if(this.areacity==''||this.roadText==''||(this.flag==true&&this.refer=='')||this.timeText==''||this.ChefPicUrl=='../../static/credit/personback.png'||this.ChefHotelPicUrl=='../../static/credit/hotelback.png'){//内容为空弹框
        this.alertFlag=true;
      }else{
        this.addChef();//调用新增信息接口
        this.nextStep();
      }      
    },
    save1(){//点击保存
    	if(this.areacity!=''){
    	  this.addChef();//调用新增信息接口
        this.saveFlag=true;
    	}else{//提示赛区不能为空
    		this.saveShow=true;
    		this.saveText='填写完赛区才能保存哦';
    	}
    },
    addHeight(className) {
      $('.vux-swiper').height($('.'+ className).height() + 45);
    },
    minusHeight(className) {
      $('.vux-swiper').height($('.'+ className).height());
    },
    
    
    // 创新菜
    addDishChef(isnext){//增加菜品信息  
    	var _this=this;
			var params={
	    		"ChefId":this.stepnd.chefId,
	    		"DishStory":this.stepnd.dishStory ,
	    		"DishPicUrl":this.stepnd.dishPicUrl ,
	    		"DishType":1,
	    		"DishName":this.stepnd.dishName
			}
    	this.$http({
    		method:'POST',
    		url:apiUrl.addDishChef,
    		data:params
    	}).then(function(response){
    		_this.stepnd.dishId=JSON.parse(response.data);
    		_this.addDishMaterial();
		    _this.addDishStep();
		    _this.addFR();
		    if(isnext=='next2'){
		    	_this.nextStep();
		    }else{
		    	_this.saveFlag=true;
		    }	  		  
    	})
    },
    addDishMaterial(){//增加调料
    	//主料
      var zList=[];
    	for(var i=0;i<$('.enterstepnd .main li').length;i++){
    		var maintext1=$('.enterstepnd .main .maintext').eq(i).val();
    		var weight1=$('.enterstepnd .main .weight').eq(i).val();
    		var item1={
    	   "DishId":this.stepnd.dishId,
	       "Material":maintext1,
	       "Unit":weight1,
	       "MaterialType":1
    		}   		
    		zList.push(item1);
    	}    	
    	//辅料
    	var fList=[];
    	for(var i=0;i<$('.enterstepnd .access li').length;i++){
    		var maintext2=$('.enterstepnd .access .maintext').eq(i).val();
    		var weight2=$('.enterstepnd .access .weight').eq(i).val();
    		var item2={
    	   "DishId":this.stepnd.dishId,
	       "Material":maintext2,
	       "Unit":weight2,
	       "MaterialType":2
    		}    
    		fList.push(item2);
    	}   	
    	//其他调料
    	var tList=[];
    	for(var i=0;i<$('.enterstepnd .othersea li').length;i++){
    		var maintext3=$('.enterstepnd .othersea .maintext').eq(i).val();
    		var weight3=$('.enterstepnd .othersea .weight').eq(i).val();
    		var item3={
    	   "DishId":this.stepnd.dishId,
	       "Material":maintext3,
	       "Unit":weight3,
	       "MaterialType":3
    		}    		
    		tList.push(item3);
    	}    	
    	var dishMaterial=zList.concat(fList,tList);
//  	console.log(dishMaterial);
    	this.$http({
    		method:'POST',
    		url:apiUrl.addDishMaterial,
    		data:dishMaterial
    	}).then(function(response){
//  		console.log(response.data);
    	})
    },
    addDishStep(){//增加步骤
    	var cookList=[];
    	for(var i=0;i<$('.enterstepnd .cookStep li').length;i++){
    		var cook=$('.enterstepnd .cookStep .stepDetail').eq(i).val();
    		var item={
    	   "DishId":this.stepnd.dishId,
	       "StepName":cook
    		}  
    		cookList.push(item);
    	}
//  	console.log(cookList);
    	this.$http({
    		method:'POST',
    		url:apiUrl.addDishStep,
    		data:cookList
    	}).then(function(response){
//  		console.log(response.data);
    	})
    },   
    addFR(){//增加推荐调料
    	var recomList=[];
    	for(var i=0;i<$('.enterstepnd .season li').length;i++){
    		var frId=parseInt($('.enterstepnd .season li').eq(i).attr('id'));
    		var unit=parseInt($('.enterstepnd .season .weight').eq(i).val());
    		var item={
    		 "RoleId":this.stepnd.chefId,
    		 "FlavourRecId":frId,
    	   "DishId":this.stepnd.dishId,	    
    	   "Unit":unit
    		}  
    		recomList.push(item);
    	}
    	this.$http({
    		method:'POST',
    		url:apiUrl.addFR,
    		data:recomList
    	}).then(function(response){
//  		console.log(response.data);
    	})
    },
    getDishChef(){//获取创新菜信息
    	var _this=this;
    	var params='?memberId='+this.MemberId+'&dishType=1';
    	this.$http({
    		method:'GET',
    		url:apiUrl.getDishChef+params
    	}).then(function(response){    		
    		if(response.data!='No'){    			
    			var data=JSON.parse(response.data).dish;
    			//菜品信息
	    		_this.stepnd.chefId=data.Basic.ChefId;
	        _this.stepnd.dishId=data.Basic.DishChefId;
	        if(data.Basic.DishPicUrl!=''){
	        	_this.dishShow=false;
	        	_this.stepnd.dishPicUrl=data.Basic.DishPicUrl;
	        }	        
	        _this.stepnd.dishStory=data.Basic.DishStory;
	        _this.stepnd.dishName=data.Basic.DishName;   
    			
    			 //DishStep步骤 
	        var dishStep=data.DishStep;
	        for(var i=0;i<dishStep.length;i++){
	        	var step=dishStep[i];
	        	var stepNum=(i+1)+'、';
	        	if(i==0){
	        		$('.enterstepnd .cookStep span').eq(i).val(stepNum);
	        		$('.enterstepnd .cookStep .stepDetail').eq(i).val(step.StepName);
	        	}else{
	        		var li='<li><span>'+stepNum+'</span><textarea class="stepDetail" placeholder="添加步骤说明">'+step.StepName+'</textarea><img src="../../static/credit/jian.png" class="rem"/></li>';
	            $('.enterstepnd .cookStep').append(li);
	        	}       	
	        }   
	        
	        //DishMaterial调料
	        var dishMaterial=data.DishMaterial;
	        var zliao=[];
	        var fliao=[];
	        var tliao=[];//其他调料
	        for(var i=0;i<dishMaterial.length;i++){
	        	var material=dishMaterial[i];
	        	if(material.MaterialType==1){//主料
	        		zliao.push(material);
	        	}else if(material.MaterialType==2){//辅料
	        		fliao.push(material);
	        	}else if(material.MaterialType==3){//其他调料
	        		tliao.push(material);
	        	}
	        }
	        for (var i=0;i<zliao.length;i++){//主料
	        	var zl=zliao[i];
	        	if(i==0){
	        		$('.enterstepnd .main .maintext').eq(i).val(zl.Material);
	        		$('.enterstepnd .main .weight').eq(i).val(zl.Unit);
	        	}else{
	        		var li = '<li>' + 
	                  '<input class="maintext" type="text"  placeholder="食材" value='+zl.Material+'>' + 
	                  '<input class="weight" type="text" placeholder="用量" value='+zl.Unit+'>' + 
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.enterstepnd .main').append(li);
	        	} 
	        }
	        for (var i=0;i<fliao.length;i++){//辅料
	        	var fl=fliao[i];
	        	if(i==0){
	        		$('.enterstepnd .access .maintext').eq(i).val(fl.Material);
	        		$('.enterstepnd .access .weight').eq(i).val(fl.Unit);
	        	}else{
	        		var li = '<li>' + 
	                  '<input class="maintext" type="text"  placeholder="食材" value='+fl.Material+'>' +
	                  '<input class="weight" type="text" placeholder="用量" value='+fl.Unit+'>' +
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.enterstepnd .access').append(li);
	        	} 
	        }
	        for (var i=0;i<tliao.length;i++){//其他调料
	        	var otl=tliao[i];
	        	if(i==0){
	        		$('.enterstepnd .othersea .maintext').eq(i).val(otl.Material);
	        		$('.enterstepnd .othersea .weight').eq(i).val(otl.Unit);
	        	}else{
	        		var li = '<li>' + 
	                  '<input class="maintext" type="text"  placeholder="食材" value='+otl.Material+'>' + 
	                  '<input class="weight" type="text"  placeholder="用量" value='+otl.Unit+'>' + 
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.enterstepnd .othersea').append(li);
	        	} 
	        }   
	        
	        //推荐调料
	        var selectList=data.select;
	        for(var i=0;i<selectList.length;i++){
	        	var selectlist=selectList[i];
	        	if(i==0){
	        		$('.enterstepnd .season .maintext').eq(i).val(selectlist.FlavourName);
	        		$('.enterstepnd .season .weight').eq(i).val(selectlist.Unit);
	        		$('.enterstepnd .season li').eq(i).attr('id',selectlist.FlavourRecId);       		
	        	}else{
	        		var li = '<li id='+selectlist.FlavourRecId+'>' + 
	                  '<input class="maintext" type="text"  placeholder="调料" readonly="readonly" value='+selectlist.FlavourName+'>' + 
	                  '<input class="weight" type="text"  placeholder="用量" value='+selectlist.Unit+'>' + 
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.enterstepnd .season').append(li);
	        	} 
	        	for(var j=0;j<_this.list.length;j++){
	        		var list=_this.list[j];
	        		if(selectlist.FlavourRecId==list.FlavourRecId){
	        			$('.enterstepnd .season li').eq(i).addClass(""+j+"");        			
	        	    tlArr[j]=1;//选中状态    	         	
	        	    $('.enterstepnd .vux-checker-box').find('.vux-checker-item').eq(j).addClass('season-item-selected');//选中样式  
	        		}
	        	}        	
	        }      
    		}    
    	})
    },
    addZL(){
        var li = '<li>' + 
                  '<input class="maintext" type="text" placeholder="食材">' + 
                  '<input class="weight" type="text" placeholder="用量">' + 
                  '<span class="ke">g</span>' + 
                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
                 '</li>';
        $('.enterstepnd .main').append(li);
        this.addHeight('enterstepnd');
    },
    addFL(){
        var li = '<li>' + 
                  '<input class="maintext" type="text" placeholder="食材">' + 
                  '<input class="weight" type="text" placeholder="用量">' + 
                  '<span class="ke">g</span>' + 
                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
                 '</li>';  
        $('.enterstepnd .access').append(li);
        this.addHeight('enterstepnd');
    },  
    onItemClick(frName,index,frId){//添加调料
      if(tlArr[index]==0){
      	if($('.enterstepnd .season .maintext').eq(0).val()==''){
      		 $('.enterstepnd .season .maintext').eq(0).val(frName);
      		 $('.enterstepnd .season li').eq(0).addClass(''+index+'');
      		 $('.enterstepnd .season li').eq(0).attr('id',frId);
      		 tlArr[index]=1;
      	}else{
      		var li = '<li class='+index+' id='+frId+'><input class="maintext" type="text" value='+frName+' readonly="readonly"><input class="weight" type="text" placeholder="用量"><span>g&nbsp;</span><img src="../../static/credit/jian.png" class="rem"/></li>';
          $('.enterstepnd .season').append(li);
          tlArr[index]=1;
      	}     	
      }else{
        $('.'+index+'').remove();
        tlArr[index]=0;
      }      
      this.addHeight('enterstepnd');
    },
    addOther(){//添加其他调料
        var li = '<li>' + 
                 '<input class="maintext" type="text" placeholder="调料">' + 
                 '<input class="weight" type="text" placeholder="用量">' + 
                 '<span>g&nbsp;</span>' + 
                 '<img src="../../static/credit/jian.png" class="rem"/>' +               
                 '</li>';
        $('.enterstepnd .othersea').append(li);
        this.addHeight('enterstepnd');  
    },
    addCookStep(){//添加烹饪步骤
      var length=$('.enterstepnd .cookStep li').length+1;
      var li='<li><span>'+length+'、</span><textarea class="stepDetail" placeholder="添加步骤说明"></textarea><img src="../../static/credit/jian.png" class="rem"/></li>';
      $('.enterstepnd .cookStep').append(li);
      this.addHeight('enterstepnd');
    },
    nextStep2(){//创新菜下一步    
    	if(this.stepnd.dishName==''||this.stepnd.dishStory==''||this.stepnd.dishPicUrl==''){
        this.alertFlag=true;       
      }else if($('.enterstepnd .main li').length<1||$('.enterstepnd .access li').length<1||$('.enterstepnd .season li').length<1||$('.enterstepnd .othersea li').length<1||$('.enterstepnd .cookStep li').length<1){
    		this.alertFlag=true;    		
    	}else{
	    	for(var i=0;i<$('.enterstepnd .main li').length;i++){//主料
	    		var text1=$('.enterstepnd .main .maintext').eq(i).val();
	    		var weight1=$('.enterstepnd .main .weight').eq(i).val();
	    		if(text1==''||weight1==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.enterstepnd .access li').length;i++){//辅料
	    		var text2=$('.enterstepnd .access .maintext').eq(i).val();
	    		var weight2=$('.enterstepnd .access .weight').eq(i).val();
	    		if(text2==''||weight2==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.enterstepnd .othersea li').length;i++){//其他调料
	    		var text3=$('.enterstepnd .othersea .maintext').eq(i).val();
	    		var weight3=$('.enterstepnd .othersea .weight').eq(i).val();
	    		if(text3==''||weight3==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.enterstepnd .season li').length;i++){//推荐调料
	    		var weight4=$('.enterstepnd .season .weight').eq(i).val();
	    		if(weight4==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.enterstepnd .cookStep li').length;i++){//步骤
	    		var step=$('.enterstepnd .cookStep .stepDetail').eq(i).val();
	    		if(step==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	this.isWeight('save2','next2');//判断调料是否为数字
	    	//进入下一步
//	      this.addDishChef();
//      this.addDishMaterial();
//      this.addDishStep();
//      this.addFR();       
//      this.nextStep();
    	}    	
    },
    save2(){//点击保存
    	if(this.stepnd.dishName==''){
    		alert('菜名不能为空');
    	}else{
    		this.isWeight('save2');//判断调料是否为数字     
    	}
    	       			     
//    this.addDishChef();
//  	this.addDishMaterial();
//  	this.addDishStep();
//  	this.addFR();
//    this.saveFlag=true; 
    },
    
    
    // 本地菜
    addLocalDishChef(isnext){//增加菜品信息    
    	var _this=this;
			var params={
	    		"ChefId":this.stepnd.chefId,
	    		"DishStory":this.steprd.dishStory ,
	    		"DishPicUrl":this.steprd.dishPicUrl ,
	    		"DishType":2,
	    		"DishName":this.steprd.dishName
			}   	   	
    	this.$http({
    		method:'POST',
    		url:apiUrl.addDishChef,
    		data:params
    	}).then(function(response){
    		_this.steprd.dishId=JSON.parse(response.data);
    		_this.addLocalDishMaterial();
		    _this.addLocalDishStep();
		    _this.addLocalFR();
		    if(isnext=='next3'){
		    	_this.sucFlag=true;
		    }else{
		    	_this.saveFlag=true; 
		    }		      
    	})
    },
    addLocalDishMaterial(){//增加调料
    	//主料
      var zList=[];
    	for(var i=0;i<$('.entersteprd .main li').length;i++){
    		var maintext1=$('.entersteprd .main .maintext').eq(i).val();
    		var weight1=$('.entersteprd .main .weight').eq(i).val();
    		var item1={
    	   "DishId":this.steprd.dishId,
	       "Material":maintext1,
	       "Unit":weight1,
	       "MaterialType":1
    		}   		
    		zList.push(item1);
    	}    	
    	//辅料
    	var fList=[];
    	for(var i=0;i<$('.entersteprd .access li').length;i++){
    		var maintext2=$('.entersteprd .access .maintext').eq(i).val();
    		var weight2=$('.entersteprd .access .weight').eq(i).val();
    		var item2={
    	   "DishId":this.steprd.dishId,
	       "Material":maintext2,
	       "Unit":weight2,
	       "MaterialType":2
    		}    
    		fList.push(item2);
    	}   	
    	//其他调料
    	var tList=[];
    	for(var i=0;i<$('.entersteprd .othersea li').length;i++){
    		var maintext3=$('.entersteprd .othersea .maintext').eq(i).val();
    		var weight3=$('.entersteprd .othersea .weight').eq(i).val();
    		var item3={
    	   "DishId":this.steprd.dishId,
	       "Material":maintext3,
	       "Unit":weight3,
	       "MaterialType":3
    		}    		
    		tList.push(item3);
    	}    	
    	var dishMaterial=zList.concat(fList,tList);
//  	console.log(dishMaterial);
    	this.$http({
    		method:'POST',
    		url:apiUrl.addDishMaterial,
    		data:dishMaterial
    	}).then(function(response){
//  		console.log(response.data);
    	})
    },
    addLocalDishStep(){//增加步骤
    	var cookList=[];
    	for(var i=0;i<$('.entersteprd .cookStep li').length;i++){
    		var cook=$('.entersteprd .cookStep .stepDetail').eq(i).val();
    		var item={
    	   "DishId":this.steprd.dishId,
	       "StepName":cook
    		}  
    		cookList.push(item);
    	}
    	this.$http({
    		method:'POST',
    		url:apiUrl.addDishStep,
    		data:cookList
    	}).then(function(response){
//  		console.log(response.data);
    	})
    },
    addLocalFR(){//增加推荐调料
    	var recomList=[];
    	for(var i=0;i<$('.entersteprd .season li').length;i++){
    		var frId=parseInt($('.entersteprd .season li').eq(i).attr('id'));
    		var unit=parseInt($('.entersteprd .season .weight').eq(i).val());
    		var item={
    		 "RoleId":this.stepnd.chefId,
    		 "FlavourRecId":frId,
    	   "DishId":this.steprd.dishId,
    	   "Unit":unit
    		}  
    		recomList.push(item);
    	}
    	this.$http({
    		method:'POST',
    		url:apiUrl.addFR,
    		data:recomList
    	}).then(function(response){
//  		console.log(response.data);
    	})
    },
    getLocalDishChef(){//获取本地菜信息
    	var _this=this;
    	var params='?memberId='+this.MemberId+'&dishType=2';
    	this.$http({
    		method:'GET',
    		url:apiUrl.getDishChef+params
    	}).then(function(response){    		 		
        if(response.data!='No'){
        	var data=JSON.parse(response.data).dish;
        	//菜品信息
	    		_this.stepnd.chefId=data.Basic.ChefId;
	        _this.steprd.dishId=data.Basic.DishChefId;
	        if(data.Basic.DishPicUrl!=''){
	        	_this.dishLocalShow=false;
	        	_this.steprd.dishPicUrl=data.Basic.DishPicUrl;
	        }
	        _this.steprd.dishStory=data.Basic.DishStory;
	        _this.steprd.dishName=data.Basic.DishName;
	        
	        //DishStep步骤 
	        var dishStep=data.DishStep;
	        for(var i=0;i<dishStep.length;i++){
	        	var step=dishStep[i];
	        	var stepNum=(i+1)+'、';
	        	if(i==0){
	        		$('.entersteprd .cookStep span').eq(i).val(stepNum);
	        		$('.entersteprd .cookStep .stepDetail').eq(i).val(step.StepName);
	        	}else{
	        		var li='<li><span>'+stepNum+'</span><textarea class="stepDetail" placeholder="添加步骤说明">'+step.StepName+'</textarea><img src="../../static/credit/jian.png" class="rem"/></li>';
	            $('.entersteprd .cookStep').append(li);
	        	}       	
	        }
	        
	        //DishMaterial调料
	        var dishMaterial=data.DishMaterial;
	        var zliao=[];
	        var fliao=[];
	        var tliao=[];//其他调料
	        for(var i=0;i<dishMaterial.length;i++){
	        	var material=dishMaterial[i];
	        	if(material.MaterialType==1){//主料
	        		zliao.push(material);
	        	}else if(material.MaterialType==2){//辅料
	        		fliao.push(material);
	        	}else if(material.MaterialType==3){//其他调料
	        		tliao.push(material);
	        	}
	        }
	        for (var i=0;i<zliao.length;i++){//主料
	        	var zl=zliao[i];
	        	if(i==0){
	        		$('.entersteprd .main .maintext').eq(i).val(zl.Material);
	        		$('.entersteprd .main .weight').eq(i).val(zl.Unit);
	        	}else{
	        		var li = '<li>' + 
	                  '<input class="maintext" type="text" placeholder="食材" value='+zl.Material+'>' + 
	                  '<input class="weight" type="text" placeholder="用量" value='+zl.Unit+'>' + 
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.entersteprd .main').append(li);
	        	} 
	        }
	        for (var i=0;i<fliao.length;i++){//辅料
	        	var fl=fliao[i];
	        	if(i==0){
	        		$('.entersteprd .access .maintext').eq(i).val(fl.Material);
	        		$('.entersteprd .access .weight').eq(i).val(fl.Unit);
	        	}else{
	        		var li = '<li>' + 
	                  '<input class="maintext" type="text" placeholder="食材" value='+fl.Material+'>' + 
	                  '<input class="weight" type="text" placeholder="用量" value='+fl.Unit+'>' +
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.entersteprd .access').append(li);
	        	} 
	        }
	        for (var i=0;i<tliao.length;i++){//其他调料
	        	var otl=tliao[i];
	        	if(i==0){
	        		$('.entersteprd .othersea .maintext').eq(i).val(otl.Material);
	        		$('.entersteprd .othersea .weight').eq(i).val(otl.Unit);
	        	}else{
	        		var li = '<li>' + 
	                  '<input class="maintext" type="text" placeholder="食材" value='+otl.Material+'>' + 
	                  '<input class="weight" type="text" placeholder="用量" value='+otl.Unit+'>' + 
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.entersteprd .othersea').append(li);
	        	} 
	        }
	        
	        //推荐调料
	        var selectList=data.select;
	        for(var i=0;i<selectList.length;i++){
	        	var selectlist=selectList[i];
	        	if(i==0){
	        		$('.entersteprd .season .maintext').eq(i).val(selectlist.FlavourName);
	        		$('.entersteprd .season .weight').eq(i).val(selectlist.Unit);
	        		$('.entersteprd .season li').eq(i).attr('id',selectlist.FlavourRecId);       		
	        	}else{
	        		var li = '<li id='+selectlist.FlavourRecId+'>' + 
	                  '<input class="maintext" type="text"  placeholder="调料" readonly="readonly" value='+selectlist.FlavourName+'>' + 
	                  '<input class="weight" type="text"  placeholder="用量" value='+selectlist.Unit+'>' + 
	                  '<span class="ke">g</span>' + 
	                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
	                 '</li>';
	            $('.entersteprd .season').append(li);
	        	}         	
	        	for(var j=0;j<_this.list.length;j++){
	        		var list=_this.list[j];
	        		if(selectlist.FlavourRecId==list.FlavourRecId){
	        			$('.entersteprd .season li').eq(i).addClass(""+j+"");        			
	        	    localTLArr[j]=1;//选中状态    	         	
	        	    $('.entersteprd .vux-checker-box').find('.vux-checker-item').eq(j).addClass('season-item-selected');//选中样式  
	        		}
	        	}        	
	        }       
        }             
    	})
    },
    addZL1(){
        var li = '<li>' +
                  '<input class="maintext" type="text" placeholder="食材">' +
                  '<input class="weight" type="text" placeholder="用量">' +
                  '<span class="ke">g&nbsp;</span>' +
                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
                 '</li>';
        $('.entersteprd .main').append(li);
        this.addHeight('entersteprd');
    },
    addFL1(){
        var li = '<li>' +
                  '<input class="maintext" type="text" placeholder="食材">' +
                  '<input class="weight" type="text" placeholder="用量">' +
                  '<span class="ke">g&nbsp;</span>' +
                  '<img src="../../static/credit/jian.png" class="rem"/>' +               
                 '</li>';
        $('.entersteprd .access').append(li);
        this.addHeight('entersteprd');
    },
    onItemClick1(frName,index,frId){          
      if(localTLArr[index]==0){
      	if($('.entersteprd .season .maintext').eq(0).val()==''){
      		 $('.entersteprd .season .maintext').eq(0).val(frName);
      		 $('.entersteprd .season li').eq(0).addClass(''+index+'');
      		 $('.entersteprd .season li').eq(0).attr('id',frId);
      		 localTLArr[index]=1;
      	}else{
      		var li = '<li class='+index+' id='+frId+'><input class="maintext" type="text" value='+frName+' readonly="readonly"><input class="weight" type="text" placeholder="用量"><span>g&nbsp;</span><img src="../../static/credit/jian.png" class="rem"/></li>';
          $('.entersteprd .season').append(li);
          localTLArr[index]=1;
      	}     	
      }else{
        $('.'+index+'').remove();
        localTLArr[index]=0;
      }      
      this.addHeight('entersteprd');
    },
    addOther1(){
        var li = '<li>' + 
                 '<input class="maintext" type="text" placeholder="调料">' + 
                 '<input class="weight" type="text" placeholder="用量">' + 
                 '<span>g&nbsp;</span>' + 
                 '<img src="../../static/credit/jian.png" class="rem"/>' +               
                 '</li>';
        $('.entersteprd .othersea').append(li);
        this.addHeight('entersteprd');  
    },
    addCookStep1(){
      var length=$('.entersteprd .cookStep li').length+1;
      var li='<li><span>'+length+'、</span><textarea class="stepDetail" placeholder="添加步骤说明"></textarea><img src="../../static/credit/jian.png" class="rem"/></li>';
      $('.entersteprd .cookStep').append(li);
      this.addHeight('entersteprd');
    },
    nextStep3(){    
      if(this.steprd.dishName==''||this.steprd.dishStory==''||this.steprd.dishPicUrl==''){
        this.alertFlag=true;       
      }else if($('.entersteprd .main li').length<1||$('.entersteprd .access li').length<1||$('.entersteprd .season li').length<1||$('.entersteprd .othersea li').length<1||$('.entersteprd .cookStep li').length<1){
    		this.alertFlag=true;    		
    	}else{
	    	for(var i=0;i<$('.entersteprd .main li').length;i++){//主料
	    		var text1=$('.entersteprd .main .maintext').eq(i).val();
	    		var weight1=$('.entersteprd .main .weight').eq(i).val();
	    		if(text1==''||weight1==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.entersteprd .access li').length;i++){//辅料
	    		var text2=$('.entersteprd .access .maintext').eq(i).val();
	    		var weight2=$('.entersteprd .access .weight').eq(i).val();
	    		if(text2==''||weight2==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.entersteprd .othersea li').length;i++){//其他调料
	    		var text3=$('.entersteprd .othersea .maintext').eq(i).val();
	    		var weight3=$('.entersteprd .othersea .weight').eq(i).val();
	    		if(text3==''||weight3==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.entersteprd .season li').length;i++){//推荐调料
	    		var weight4=$('.entersteprd .season .weight').eq(i).val();
	    		if(weight4==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	for(var i=0;i<$('.entersteprd .cookStep li').length;i++){//步骤
	    		var step=$('.entersteprd .cookStep .stepDetail').eq(i).val();
	    		if(step==''){
	    			this.alertFlag=true;
	    			return;
	    		}
	    	}
	    	this.isWeight('save3','next3');//判断调料是否为数字
         //进入提交成功页面；
//      this.addLocalDishChef();
//      this.addLocalDishMaterial();
//      this.addLocalDishStep();
//      this.addLocalFR();
//      this.sucFlag=true;            
    	}    	
    },
    save3(){//点击保存
    	if(this.steprd.dishName==''){
    		alert('菜名不能为空');
//  		this.saveText='调料的用量必须是数字哦';
//				this.saveShow=true;
    	}else{
    		this.isWeight('save3');//判断调料是否为数字     
    	}
    	
//  	this.addLocalDishChef();
//  	this.addLocalDishMaterial();
//  	this.addLocalDishStep();
//  	this.addLocalFR();
//    this.saveFlag=true;
    },
    closeSave(){//关闭保存页面
      this.saveFlag=false;
    },
    closeBtn(){
      this.$router.push({path:'/'})
    },
    goregister(){//去注册按钮
    	this.$router.push({path:'/component/register'});
    }

  },
  mounted(){
    this.setConfig();
    // 页面初始化进入，设置各个步骤容器的高度；
    var step1H = $('.enterstepst').height() + 45;
    this.currentStepH = step1H + 'px';

    //获取MemberId
    this.userData=cookie.get('WeiXinUser',function(val){
  		  var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
  	}) 
	  this.MemberId=this.userData.UserId;
    var uri=location.href;
		this.url=encodeURIComponent(uri);
	  if(this.MemberId=="0"){//没有该用户，提示用户注册
	  	this.promptZcFlag=true;
	  }else{
	  	this.isApply();//判断是否可以报名
	  }

//  this.getArea();//获取赛区
//  this.getDishChef();//获取创新菜信息
//  this.getLocalDishChef();//获取本地菜信息

    var _this = this;
    // 创新菜、 本地菜，减项
    $('.main').on('click','.rem',function(){
      var $parents = $(this).parent().parent().parent()[0];
      _this.minusHeight($parents.className);
      $(this).parent().remove();
    })

    $('.access').on('click','.rem',function(){
      var $parents = $(this).parent().parent().parent()[0];
      _this.minusHeight($parents.className);
      $(this).parent().remove();
    })

    $('.season').on('click','.rem',function(){   
      var sz=$(this).parent()[0].className;     
      var $parents = $(this).parent().parent().parent()[0];
      if($parents.className == 'enterstepnd') {
          tlArr[sz]=0;
      }else if($parents.className == 'entersteprd') {
          localTLArr[sz]=0;
      }
      $('.' + $parents.className).find('.vux-checker-item').eq(sz).removeClass('season-item-selected');
      _this.minusHeight($parents.className);
      $(this).parent().remove();   
    })

    $('.othersea').on('click','.rem',function(){
      var $parents = $(this).parent().parent().parent()[0];
      _this.minusHeight($parents.className);
      $(this).parent().remove();
    })

    $('.cookStep').on('click','.rem',function(){
      var $parents = $(this).parent().parent().parent()[0];
      _this.minusHeight($parents.className);
      $(this).parent().remove();
    }) 
  }

}
</script>

<style lang="less">
  .stepBox .vux-step { width: 70%; margin: 0 auto; margin-top: 30px; }
  .stepBox .vux-step .vux-step-item {}
  .stepBox .vux-step .vux-step-item .vux-step-item-main {}
  .stepBox .vux-step .vux-step-item .vux-step-item-main .vux-step-item-title { position: absolute; display: block; width: 60px; left: -44px; top: 28px; text-align: center; }
  .stepBox .vux-step .vux-step-item.third .vux-step-item-main .vux-step-item-title { top: 3px; left: -18px; }
</style>
<!-- 基本信息样式start -->
<style scoped>  
  .enterstepst{ width: 90%; margin: 0 auto; background: #FFFFFF; }
  .photoshow { margin-top: 40px; display: flex; text-align: center; }
  .photoshow img{width: 100%;height:100%}
  .photoshow div{ width: 45%; height: 120px;}
  .photoshow div:nth-child(1){ margin-right: 10%;}
  .btnDiv{ margin-top: 60px; width: 90%; margin-left: auto; margin-right: auto; }
  .btnDiv button{ width: 45%; height: 40px; border: 1px solid #E83428; border-radius: 5px; background:#FFFFFF; color: #E83428; outline: none; }
  .btnDiv button:nth-child(2){ margin-left: 10%; }
  .alert-mask{ width: 100%; height: 100%; background: rgba(0,0,0,0.5); position: fixed; top: 0px; }
  .zone{ width: 100%; height: 410px; background: #FFFFFF; }
  .entDiv{width: 295px;height: 30px;background: #f8f8f8;border-radius: 5px;margin-top:20px;margin-left: 10%;display: inline-block;}
  .searchImg{width:17px;float:right;margin-right: 11px;margin-top: 7px;}
  .entDiv img,.entDiv input{display: inline-block;vertical-align: middle;}
  .entDiv input{border: none;background: none;outline: none;text-indent: 1em;}
  .searchDiv{ width: 100%; height: 300px; overflow-y:auto; border-top: 1px solid #c8c8c8; margin-top: 10px; }
  .cityDiv{ width: 80%; margin-left: 10%; margin-top: 20px; }
  .areacity-item { width: 50px; height: 26px; line-height: 26px; text-align: center; color: #3E3E3E; font-size: 13px; margin-left:8%; }
  .areacity-item-selected { color: #E83428; border-bottom:1px solid #E83428; }
  .areaBtn button{ width: 50%; height: 49px; font-size: 13px;border: none; border-top: 1px solid #E83428;box-sizing: border-box;  }
  .areaBtn button:nth-child(1){ background: #fff; color: #E83428;}
  .areaBtn button:nth-child(2){ background: #E83428; color: white;}
  .menu{ width:60%; height: 150px; background: #FFFFFF; border-radius: 10px; margin: 150px auto;border:1px solid #fff;}
  .menu p{ font-size: 13px; color: #3E3E3E; width: 80%; margin-left: 10%;margin-top: 20px; }
  .menu button{ width: 50%; height: 33px; background: #fff; color: #E83428; border: 1px solid #E83428; border-radius: 20px; margin-left: 25%; margin-top: 18px; outline: none; }
  .menus p{text-align: center;margin-top: 40px;}
  .menus button{margin-top: 40px;}
  
  .saveDiv{ width: 100%; height: 100%; position: fixed; top: 0px; background: #FFFFFF; text-align: center; }
  .saveDiv img{ width: 160px; margin-top: 90px; }
  .saveDiv p:nth-child(2){ font-size: 17px;   color: #E83428; margin-top: 15px; }
  .saveDiv p:nth-child(3){ font-size: 13px;   color: #707070; margin-top: 10px; }
  .saveDiv button{ width: 90%; height: 40px; color: white; background: #E83428; border: none; border-radius: 5px;  margin-top: 20%; }
</style>
<style>
  .enterstepst .weui-cell{font-size: 13px;color: #3e3e3e;}
  .enterstepst .weui-cells:before{border-top: none !important;}
  .enterstepst .weui-cells:after{border-bottom: none !important;}
  .enterstepst .weui-cells { border: 1px solid #dddddd; border-radius: 0.5rem; }
  .enterstepst .weui-label { width: 4.5rem !important; }
  .vux-popup-picker-header-menu-right { color: #e83428 !important; }
  .enterstepst .vux-cell-box:before{border-top: none !important;}
  .enterstepst .vux-datetime p{width: 4.5rem;}
  .enterstepst .weui-cell__ft{color: #3E3E3E !important;}
  .dp-right {color: #e83428 !important;}
  #vux_view_box_body{background: #FFFFFF !important;}
  
  .vux-step-item-head-process .vux-step-item-head-inner{border:none !important;background: #E83428 !important;}
  .vux-step-item-head-wait .vux-step-item-head-inner{border:none !important;background: #bcbcbc !important;}
  .vux-step-item-tail-finish{background:#e83428 !important;}
  .vux-step-item-head-finish .vux-step-item-head-inner{border: none !important;background: #E83428 !important;}
  .weui-icon-success-no-circle{color:white !important;}
</style>
<!-- 基本信息样式end -->

<!-- 创新菜样式start -->
<style lang="less">
  .upload{width: 100%;height: 225px;background: #fbfbfb;text-align:center; color: #8e8e8e;line-height: 225px;margin-top: 25px;} 
  .upload img{width: 100%;height: 100%;}
  .dishname{width: 90%;height: 60px;margin-left: 5%;font-size: 16px;text-align: center;outline: none;border: none;border-bottom: 1px solid #dddddd;}        
  .alike *{display: inline-block;vertical-align: middle;}
  .alike img{width: 20px;}
  .alike span{color: #e83426;font-size: 15px;font-weight: bold;} 
  .show{margin-top: 20px;padding-left: 10px;padding-right: 10px;}
  .zliao{width: 100%;height: 30px;background: #f9f9f9;color: #E83428;line-height: 30px;margin-top: 15px;text-indent: 10px;}
  .main li,.access li,.season li,.othersea li{width: 100%;height: 45px;box-sizing: border-box;border-bottom: 1px solid #e2e2e2;list-style: none;line-height: 45px;position: relative;}  
  .main li *,.access li *,.season li *,.othersea li *{float: left;}
  .maintext,.weight{height: 100%;line-height: 44px;outline: none;border: none;}
  .maintext{width: 75%;text-indent: 10px;}  
  .weight{width: 10%;text-align: left;}
  .ke{display: inline-block;height: 100%;}
  .rem{width: 15px;position: absolute;right: 10px;top: 16px;} 
  .add{ margin-top: 10px; text-align: right;}
  .add img{display: inline-block;width: 15px;margin-right: 10px;}
  .seasonText{font-size: 12px;color: #E83428; padding-left: 10px;margin-top: 15px;}
  .season-item {width: 22.6%;height: 105px;background: #f9f9f9;margin-left:2%; margin-top: 12px; text-align: center;}
  .season-item img{ width:55%;height:100%;}
  .season-item p{font-size: 12px;color:#3E3E3E;}
  .season-item-selected {background:#b4b4b4 url(../../static/credit/select01.png) no-repeat right bottom;background-size:20px;}
  .season{margin-top: 15px;margin-bottom: 50px;}
  .cookStep li{height: 45px;line-height: 40px;margin-left: 15px;list-style: none;position: relative;}
  .cookStep li span {font-size: 16px;}
  .stepDetail{width: 80%;position: absolute;top: 14px;font-size: 13px;color: #3E3E3E;outline: none;border: none;resize : none;}
</style>
<style>
  .enterstepnd .weui-textarea,.entersteprd .weui-textarea{font-size: 13px !important;text-indent: 1em;}
  .enterstepnd .vux-no-group-title:before,.entersteprd .vux-no-group-title:before{border-top: none !important;}
  .enterstepnd .vux-no-group-title:after,.entersteprd .vux-no-group-title:after{border-bottom: none !important;}
  .enterstepnd .weui-cells,.entersteprd .weui-cells{margin-top: 0 !important;} 
</style>
<!-- 创新菜end -->

<!-- 本地菜start -->
<style>
  .sucDiv,.successDiv{width: 100%;height: 100%;position: fixed;top: 0px;background: #FFFFFF;text-align: center;/*z-index: 1000;*/}    
  .sucDiv img{ width: 160px;margin-top: 90px;}
  .sucDiv p:nth-child(2){font-size: 17px;color: #E83428;margin-top: 15px;}
  .sucDiv p:nth-child(3){font-size: 13px;color: #707070;margin-top: 10px;}
  .sucDiv button{width: 90%;height: 40px;color: white;background: #E83428;border: none;border-radius: 5px;margin-top: 20%;}
  .successDiv img{width: 80%;margin-top: 150px;}
  .prompt-zc{border:1px solid #E83428;width: 200px;height:120px;border-radius:25px;background: #fff;color:#E83428;position: absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
  .prompt-zc p{text-align:center;margin-top: 20px;}
  .prompt-zc button{width: 100px;height: 30px;background:#E83428 ;color: #fff;border: none;border-radius:5px;position: absolute;bottom: 20px;left:50px;}
</style>
<!-- 本地菜end -->