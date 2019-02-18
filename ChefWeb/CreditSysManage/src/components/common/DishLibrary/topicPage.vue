<template>
  <!--专题页面管理-->
  <div class="topicPage">
    <div>
      <Button type="primary"  style="margin:0  0.5rem;" @click='add_click'>新增</Button>
      <Date-picker type="daterange" v-model='search.datetime' placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 180px;margin-left: 10%;" ></Date-picker>
      <Input  placeholder="姓名/酒店/手机号" style="width: 150px" v-model='search.keyword'></Input>
      <Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>查询</Button>
    </div>
    <Table style="margin-top: 2rem;" :columns="table_columns" :data="tabInfoList"></Table>
    <Page  :total='search.count' :current='search.pageIndex' show-elevator class='page' :page-size='10' @on-change='changePage' show-total style='margin: 1rem;float: right'></Page>
    <!--新增-->
    <Modal title="创建专题管理" v-model="modal_topic" width='60%' :mask-closable="false" class="modal" >
      <div>
      	<p>
	      	<span>专题名称</span>
	        <Input  placeholder="页面标题" style="width: 150px;margin-left: 2rem" v-model='plateInfo.ZhuanTiName'></Input>
	      	<span>专题ID:{{plateInfo.ZhuanTiId}}</span>
        </p>
        <p style="margin-top:1rem;">URL地址:<span style="margin-left: 2rem">http://uatjifenweixin.shinho.net.cn/#/component/dishspecial?specialId={{plateInfo.ZhuanTiId}}</span></p>
        <Button type="primary" style="width:20%;margin:1rem 0 3rem 0;display: block" @click="addTextBox">新增文本框和菜品选择</Button>
      </div>
      <ul class="plateBox">
      	<li v-for="(item,index) in plateInfo.ContentList" :key="index" style="position: relative;">
          <Button type="primary" style="position: absolute;right: 10px;" @click='delete_plate_click(index)'>删除</Button>
      		<!--富文本-->
      		<div class="rtf-content" style="width: 80%;display: block;margin-top: 1rem;height: 267px;">
            <quill-editor style="height: 200px" ref="myTextEditor" v-model="item.Content" :config="editorOption_basicInf"></quill-editor>
          </div>
          <!--查找菜品-->
          <p style="margin-top: 1rem;">
          	<Input  placeholder="输入菜品名/菜品ID" style="width: 150px" v-model='item.serchdish'></Input>
          	<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='modal_search_click(index,item.serchdish)'>查找</Button>
          	<Button type="primary" style="margin:0  0.5rem;" @click='modal_hide_click(index)'>{{item.isShowText}}</Button>
          </p>
          <!--添加菜品-->
          <Tag  style="margin-top: 1rem;" v-for="items in item.CaiPinId" color="red" :key="items.CaiPinId" :name="items.CaiPinId" closable @on-close="tagClose(items)">{{items.CaiPinName}}</Tag>
          <div v-show="item.isShow">
		      	<Table width="550" height="200" border :columns="modal_Tabcolumns" :data="item.dishList" style="margin: 1rem 0"></Table>
    			</div>
      	</li>
      </ul>
      <Button type="primary" style="width:15%;margin:10rem 42%;display: block" @click="createPlates">创建</Button>
    </Modal>
    
    <!--删除提示弹框-->
    <Modal v-model="model_delete" width="360">
      <p slot="header" style="color:#f60;text-align:center">
        <Icon type="information-circled"></Icon>
        <span>删除确认</span>
      </p>
      <div style="text-align:center">
        <p>您确定要删除此条数据吗?</p>
      </div>
      <div slot="footer" style="text-align: center;">
        <Button type="error" size="small"  @click="delBtn">删除</Button>
      </div>
    </Modal>
    
  </div>
</template>

<script>
  import { quillEditor } from 'vue-quill-editor';//编写框
  export default{
    components:{
      quillEditor
    },
    data(){
      return{
//    	datalist:[],
//    	count:'',
      	plateInfo:{//专题信息
					"ZhuanTiId": 0,
					"ZhuanTiName": "",
					"Action": "Create",//新增或编辑
					"ContentList": [{
						  'serchdish':'',//搜索菜名
						  'isShow':false,//菜品列表状态
      	  	  'isShowText':'显示菜品列表',//显示或隐藏按钮
      	  	  "Content": "",//富文本内容
      	  	  "dishList":[],//菜品信息列表
							"CaiPinId": []//菜品id
						}
					]
				},
      	model_delete:false,//删除提示框
        deleteData:'',//删除数据
        editorOption_basicInf:{},
        search:{//列表查询数据
          datetime:[],//时间
          keyword:'',//关键字
          count:0,//总条数
          pageIndex:1,//当前页数
        },
        dishSearch:{//菜品查询数据
        	count:0,//总条数
          pageIndex:1,//当前页数
        },
        modalIfon:{
          ZhuanTiId:0,//0新增
          ZhuanTiName:'',//名字
          Action:'Create',//Create新增  Edit编辑
          BanKuaiId:[],//板块
          Description:'',//文案
          dishIfo:'',//查询的菜品信息
          CaiPinId:[],//选择的菜品
        },
        modal_topic:false,//新增弹框
//      plateList:[],//板块数据
        modal_Tabcolumns:[
          {
            title:'序号',
            align: 'center',
            type: 'index',
            width:'60'
          },
          {
            title: '菜品名称',
            align: 'center',
            key: 'CaiPinName',
          },
          {
            title: '操作',
            align: 'center',
            render: (h, params) => {
              return h('div', [
                h('Button', {
                  props: {
                    type: 'primary',
                    size: 'small'
                  },
                  style:{
                    width:'40px'
                  },
                  on: {
                    click: () => {
                      this.addModalsearchDish(params)
                    }
                  }
                }, '添加')
              ]);
            }
          },
        ],
//      modal_Tabdata:[],
        tabInfoList:[],//列表数据
        table_columns:[
          {
            title:'页面ID',
            align: 'center',
            key: 'ZhuanTiId',
            width:'100'
          },
          {
            title: '专题名称',
            align: 'center',
            key: 'ZhuanTiName',
          },
          {
            title: '预览',
            align: 'center',
            key: 'ZhuanTiUrl',
            // render:(h,paeams) =>{
            //   return h('img',{
            //     attrs:{
            //       src:paeams.row.ZhuanTiUrl
            //     },
            //     style:{
            //       maxWidth:'5rem'
            //     }
            //   });
            // }
          },
          {
            title: '浏览次数',
            align: 'center',
            key: 'LlCount'
          },
          {
            title: '收藏次数',
            align: 'center',
            key: 'ScCount'
          },
          {
            title: '点赞数',
            align: 'center',
            key: 'DzCount'
          },
          {
            title: '留言数',
            align: 'center',
            key: 'LyCount'
          },
          {
            title: '链接',
            align: 'center',
            key: 'ZhuanTiUrl',
            // render: (h, params) => {
            //   return h('div', [
            //     h('Button', {
            //       props: {
            //         type: 'primary',
            //         size: 'small'
            //       },
            //       style:{
            //         width:'40px'
            //       },
            //       on: {
            //         click: () => {
            //           this.copy_click(params)
            //         }
            //       }
            //     }, '复制')
            //   ]);
            // }
          },
          {
            title: '分属板块',
            align: 'center',
            key: 'BanKuaiName'
          },
          {
            title: '操作',
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
                    width:'40px'
                  },
                  on: {
                    click: () => {
                      this.redact_click(params)
                    }
                  }
                }, '修改'),
                h('Button', {
                  props: {
                    type: 'error',
                    size: 'small'
                  },
                  style:{
                    width:'40px',
                    marginLeft:'0.8rem'
                  },
                  on: {
                    click: () => {
                      this.delete_click(params)
                    }
                  }
                }, '删除')
              ]);
            }
          }
        ],
      }
    },
    methods:{
    	modal_hide_click(index){//隐藏菜品列表
        var ContentList = this.plateInfo.ContentList;
    		ContentList[index].isShow == false ? ContentList[index].isShow = true :  ContentList[index].isShow = false
    		ContentList[index].isShowText == '显示菜品列表' ? ContentList[index].isShowText = '隐藏菜品列表' :  ContentList[index].isShowText = '显示菜品列表'
    	},
    	delete_plate_click(index){//删除当前板块
    		this.plateInfo.ContentList.splice(index,1);
    	},
      add_click(){//点击新增 
      	this.modal_topic = true;
        this.plateInfo = {//专题信息
					"ZhuanTiId": 0,
					"ZhuanTiName": "",
					"Action": "Create",//新增或编辑
					"ContentList": [{
						  'serchdish':'',//搜索菜名
						  'isShow':false,//菜品列表状态
      	  	  'isShowText':'显示菜品列表',//显示或隐藏按钮
      	  	  "Content": "",//富文本内容
      	  	  "dishList":[],//菜品信息列表
							"CaiPinId": [],//菜品id
							"count":0,//总条数
							"pageIndex":1,//当前页数
						}
					]
				}
      },
      search_click(){//查询
        this.gerTabList();//获取专题页列表
      },
      redact_click(params){//修改
//    	console.log(params);
        var ZhuanTiId = params.row.ZhuanTiId;
        this.getSpecialInfo(ZhuanTiId);
      },
      getSpecialInfo(ZhuanTiId){//通过专题Id获取专题信息
      	var self = this;
      	$.ajax({
          type:'get',
          url:URLHeader.foodLibrary+'/Api/ZhuanTi/GetById?ZhuanTiId='+ZhuanTiId,
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            var data = JSON.parse(res);
        		self.modal_topic = true;
//      		console.log(data);
        		var ContentList = [];
        		for(let i in data){
        			for(let j in data[i].CaiPinList){
        				var cpitem = data[i].CaiPinList[j];
        				cpitem.plateIndex = i;
        			}
        			var item = {
							  'serchdish':'',//搜索菜名
							  'isShow':false,//菜品列表状态
	      	  	  'isShowText':'显示菜品列表',//显示或隐藏按钮
	      	  	  "Content":data[i].Content,//富文本内容
	      	  	  "dishList":[],//菜品信息列表
								"CaiPinId": data[i].CaiPinList//菜品id
	        		}
	        		ContentList.push(item);
        		}
        		
        		self.plateInfo = {//专题信息
							"ZhuanTiId": data[0].ZhuanTiId,
							"ZhuanTiName": data[0].ZhuanTiName,
							"Action": "Edit",//新增或编辑
							"ContentList": ContentList
						}
//      		console.log(self.plateInfo);
          },
          error:function(error){
            self.$Message.error('数据获取失败');
          }
        })
      },
      delete_click(params){//点击列表删除按钮
      	this.model_delete = true;
				this.deleteData = params;
      },
      delBtn(){//点击删除
        this.deleteApi(this.deleteData);
			},
      deleteApi(params){//专题启用、禁用 删除
//    	console.log(params);
      	var self = this;
      	var ZhuanTiId = params.row.ZhuanTiId;
      	var Data = {
      		"Flag": 0,//0停用 1启用
    			"ZhuanTiId": ZhuanTiId
      	}
      	$.ajax({
          type:'post',
          url:URLHeader.foodLibrary+'/Api/ZhuanTi/Enable',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(Data),
          success:function(res){
            var res = JSON.parse(res)
//          console.log(res)
            if (res.status == "succ"){
              self.$Message.success('数据删除成功');
      	      self.model_delete = false;//隐藏删除提示框
        			self.gerTabList();//获取专题页列表
            }else {
              self.$Message.error('数据删除失败');
            }
          },
          error:function(error){
            self.$Message.error('数据删除失败');
          }
        })
      },
      copy_click(params){//复制
        //console.log(params.row.ZhuanTiUrl)
        var clipBoardContent=params.row.ZhuanTiUrl;
        window.clipboardData.setData("Text",clipBoardContent);
        //alert("复制成功!");
      },
      changePage(index){//页数
        this.search.pageIndex = index
        this.gerTabList()
      },
      gerTabList(){//获取专题页列表
        let self = this
        var search = this.search
        var startdate = ''
        var enddate = ''
        if (search.datetime.length ==0 || search.datetime[0]==null) {
          startdate= '';//开始时间
          enddate = '';//结束时间
        } else{
          var starTime = new Date(search.datetime[0]);
          startdate = starTime.getFullYear() + '-' + (starTime.getMonth() + 1) + '-' + starTime.getDate()
          var endTime = new Date(search.datetime[1]);
          enddate = endTime.getFullYear() + '-' + (endTime.getMonth() + 1) + '-' + endTime.getDate()
        }
        var searchData = {
          page:search.pageIndex,
          pagesize:10,
          startdate:startdate,
          enddate:enddate,
          keyword:search.keyword
        }
        $.ajax({//
          type:'post',
          url:URLHeader.foodLibrary+'/Api/ZhuanTi/Query',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(searchData),
          success:function(res){
            var res = JSON.parse(res)
            console.log(res)
            if (res.status == "succ"){
              self.search.count = res.totalcount
              self.tabInfoList = res.data
            }else {
              self.$Message.error('数据获取失败');
            }
          },
          error:function(error){
            self.$Message.error('数据获取失败');
          }
        })
      },
//    getSelectData(){//获取板块数据
//      var self =this
//      $.ajax({//板块
//        type:'get',
//        url:URLHeader.foodLibrary+'/Api/BanKuai/Get',
//        dataTape:'json',
//        contentType:'application/json; charset=utf-8',
//        data:{},
//        success:function(res){
//          var data = JSON.parse(res);
//          self.plateList = data
//          console.log(data)
//        },
//        error:function(error){
//          self.$Message.error('板块数据获取失败');
//        }
//      })
//    },
      modal_search_click(index,dish){//弹框查找菜品
        var self = this
        let data = {
          page:'1',
          pagesize:'1000',
          startdate:'',
          enddate:'',
          keyword:dish
        }
        $.ajax({
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/Query',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(data),
          success:function(res){
            var data = JSON.parse(res);
            var dishList = data.data;
            for(let i in dishList){
            	var list = dishList[i];
            	list.plateIndex = index 
            }
            self.plateInfo.ContentList[index].dishList = dishList
            self.plateInfo.ContentList.splice(index,1,self.plateInfo.ContentList[index]);
            
            console.log(self.plateInfo.ContentList);
          },
          error:function(error){
            self.$Message.error('查找菜品失败');
          }
        })
      },
      addModalsearchDish(params){//将查询的菜品添加到当前板块
        var plateIndex =  params.row.plateIndex;//所属板块index
        var CaiPinId = this.plateInfo.ContentList[plateIndex].CaiPinId;
        if(CaiPinId.length == 0){
          CaiPinId.push(params.row)
        }else{//判断是否已经添加
        	var flag = false;//是否可以添加
        	for(let i in CaiPinId){
        		var dish = CaiPinId[i];
        		if(dish.CaiPinId != params.row.CaiPinId){
        			flag = true;
        		}else{
        			flag = false;
        			break;
        		}
        	}
        	
        	if(flag){//true添加到caipinid
            CaiPinId.push(params.row)
        	}else{
            this.$Message.error('所选菜品已存在');
        	}
        }
      },
      tagClose(item){//标签删除
        var plateIndex = item.plateIndex;
        var index = this.plateInfo.ContentList[plateIndex].CaiPinId.indexOf(item);
        this.plateInfo.ContentList[plateIndex].CaiPinId.splice(index, 1);
      },
      addTextBox(){//新增文本框和菜品选择
				var item = {
				  'serchdish':'',//搜索菜名
				  'isShow':false,//菜品列表状态
		  	  'isShowText':'显示菜品列表',//显示或隐藏按钮
		  	  "Content": "",//富文本内容
		  	  "dishList":[],//菜品信息列表
					"CaiPinId":[],//菜品id
					"count":0,//总条数
					"pageIndex":1,//当前页数
				}
				this.plateInfo.ContentList.push(item);
      },
      createPlates(){//创建 编辑板块
      	var self = this;
      	var ContentList = [];
      	for(let i in this.plateInfo.ContentList){
      		var content = this.plateInfo.ContentList[i];
      		var dishArr = [];
      		var dishId = '';
      		for(let i in content.CaiPinId){
      			dishArr.push(content.CaiPinId[i].CaiPinId);
      		}
      		dishId = dishArr.join(',');
      		var item = {
      			Content:content.Content,//富文本
      			CaiPinId:dishId//添加菜品
      		}
      		ContentList.push(item);
      	}
      	var data = {
      		"ZhuanTiId":this.plateInfo.ZhuanTiId,
					"ZhuanTiName": this.plateInfo.ZhuanTiName,
					"Action": this.plateInfo.Action,
					"ContentList": ContentList
      	}
      	console.log(data);
      	$.ajax({
          type:'post',
          url:URLHeader.foodLibrary+'/Api/ZhuanTi/CreateOrEdit',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(data),
          success:function(res){
            var data = JSON.parse(res)
            console.log(data);
            if(data.status == 'succ'){
            	if(self.plateInfo.Action == 'Edit'){
            		self.$Message.success('编辑成功');
            	}else{
            		self.$Message.success('创建成功');
            	}
            	self.gerTabList();//获取列表数据
        			self.modal_topic = false;//关闭页面
            }else{
            	self.$Message.error('提交信息失败');
            }
          },
          error:function(error){
            self.$Message.error('提交信息失败');
          }
        })
      }
    },
    mounted:function(){
      this.gerTabList();//获取列表数据
//    this.getSelectData();//获取板块数据
    }
  }
</script>

<style scoped>
.modal_li .ql-container{
  height: 200px;
}
.ql-snow .ql-tooltip{
  margin-left: 9rem;
}
.modal_li{
  margin-top: 1rem;
}
.plateBox li{background: rgba(205,199,203,0.1);margin-top: 3rem;}
</style>
<style>
.modal .ivu-modal-footer{display: none !important;}
.ivu-modal-footer{border: none !important;}
.ivu-modal-header{border: none !important;}
</style>
