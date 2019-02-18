<template>
  <!--菜品内容管理-->
  <div class="dishesManagement">
    <div>
    	<Button type="primary"  style="margin:0  0.5rem;" @click='add_click'>新增</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='release_click'>发布</Button>
			<Button type="primary"  style="margin:0  0.5rem;" @click='norelease_click'>不发布</Button>
	    <Date-picker type="daterange" v-model='search.datetime' placement="bottom-end" class='dataTime' placeholder="选择日期" style="width: 180px;margin-left: 10%;" ></Date-picker>
			<Input  placeholder="菜品名" style="width: 150px" v-model='search.information'></Input>
			<Button type="primary"  icon="ios-search" style="margin:0  0.5rem;" @click='search_click'>查询</Button>
    </div>
		<Table style="margin-top: 2rem;" :columns="table_columns" :data="tabInfoList" @on-selection-change='select_click' @on-sort-change="sort_change"></Table>
	  <Page  :total='search.count' :current='search.pageIndex' show-elevator class='page' :page-size='10' @on-change='changgePage' show-total style='margin-bottom: 1rem;'></Page>

		<!--新增&编辑-->
    <Modal title="创建新菜品" v-model="modalFlag" width='50%' :mask-closable="false" class="modal createDish" >
	    <Steps :current="current" style="margin-bottom: 20px;">
	        <Step title="分类" @click.native="stepClick(0)" style="cursor:pointer"></Step>
	        <Step title="菜品内容" @click.native="stepClick(1)" style="cursor:pointer"></Step>
	        <Step title="主辅调料配置" @click.native="stepClick(2)" style="cursor:pointer"></Step>
	        <Step title="保存成功" ></Step>
	    </Steps>
	    <!--分类-->
	    <div class="box-a" v-show="current==0">
				<p><Input  placeholder="菜品名称" style="width: 30%" v-model='dish_category.CaiPinName'></Input></p>
				<ul style="margin-top: 20px;">
					<li v-for="(item,index) in dish_category.list" :key="index">
						<!--一级分类 单选-->
						<Select v-model="item.modelfirst" @on-open-change="openChange(item,index)" @on-change="firstSelectChange(item,index)" style="width:20%;margin-top: 5px;" placeholder='请选择一级分类'>
			        <Option v-for="item in firstList" :value="item.FirstId" :key="item.FirstId">{{ item.FirstName }}</Option>
			    	</Select>
			    	<!--二级分类 多选-->
			    	<Select v-model="item.modelsecond" multiple style="width:200px;margin-top: 5px;" placeholder='请选择二级分类' >
			        <Option v-for="one in item.modelsecondAll" :value="one.SecondId" :key="one.SecondId">{{ one.SecondName }}</Option>
			    	</Select>
					  <a @click='removeMenuClick(item)' style="cursor: pointer;position: relative;top: 5px;"> <Icon type="ios-close-outline" size='20'></Icon></a>
					</li>
				</ul>
			  <p><Button style="margin:5px 30% 20px 30%;"type="primary" @click="createMenuClick">新增</Button></p>
	    	<!--所属板块 多选-->
	    	<p style="margin-top:5px;">
	    		<!--<Select v-model="dish_category.BanKuaiId" multiple style="width:30%" placeholder='请选择板块'>
		        <Option v-for="(item,index) in plateList" :value="item.BanKuaiId" :key="item.BanKuaiId" >{{ item.BanKuaiName }}</Option>
		    	</Select>
		    	<span style="margin-left:5px;">板块</span>-->
		    	
		    	<Select v-model="dish_category.BanKuaiArr" multiple style="width:30%" placeholder='请选择板块' @on-change="optionChange">
		        <Option v-for="(item,index) in plateLists" :value="item.BanKuaiId" :key="item.BanKuaiId" >{{ item.BanKuaiName }}</Option>
		    	</Select>
		    	<span style="margin-left:5px;">板块</span>
		    	
	    	</p>
				<p><Input placeholder="视频地址链接" style="width: 30%" v-model='dish_category.VideoUrl'></Input><span style="margin-left: 5px;">视频链接地址</span></p>
				<!--视频图片-->
				<p class="uploadimg" style="margin-bottom: 20px;">
          <img :src="dish_category.VideoImageUrl" style="display: block;height:5rem;border-radius:4px ;"/>
		    	<Upload
			        ref="upload"
			        :before-upload='handleBeforeUpload'
			        :on-format-error="handleFormatError"
			        :on-exceeded-size="handleMaxSize"
			        :format="['jpg','jpeg','png']"
			        :show-upload-list="false"
			        :on-success="successUpload_a"
			        :max-size="3072"
			        :action=UPImage
			        style="display:inline-block;">
			        <Button type="primary" style="display:block;margin-top: 1rem;width: 120px;text-align: center;">选择视频封面图片</Button>
			    </Upload>
				</p>
	    	<!--菜品图片-->
	    	<p class="uploadimg">
          <div style="display: inline-block;position: relative;margin-right: 10px;" v-for="(item,index) in dish_category.ImageUrlList">
          	<img :src="item" style="height:5rem;border-radius:4px;margin-right: 5px;"/>
					  <a @click='deleteImg(index)' style="cursor: pointer;position: absolute;top: -10px;right: -6px;"> <Icon type="ios-close-outline" size='20'></Icon></a>
          </div>
				  <Upload
			        ref="upload"
			        :before-upload='handleBeforeUpload'
			        :on-format-error="handleFormatError"
			        :on-exceeded-size="handleMaxSize"
			        :format="['jpg','jpeg','png']"
			        :show-upload-list="false"
			        :on-success="successUpload_b"
			        :max-size="3072"
			        multiple
			        :action=UPImage
			        style="display:block;">
			         <Button type="primary" style="display:block;margin-top:1rem;width: 120px;text-align: center;">选择菜品图片</Button>
			    </Upload>

			  </p>
		  </div>

		  <!--菜品内容-->
		  <div class="box-b" v-show="current==1">
		  	<Select v-model="modeldishcontent" style="width:200px">
		        <Option v-for="item in dishContentList" :value="item.Id" :key="item.Id">{{ item.Type }}</Option>
		    </Select>
			  <Button type="primary" style="display:inline-block;width: 120px;" @click="addDishContent">增加</Button>
			 <!--富文本-->
			  <div v-for="(item,index) in dishContent.list" :key="index" style="margin-top: 30px;padding-top:20px;border-top: 1px dashed #ccc;">
					<Input style="width: 30%;margin-right: 1rem" v-model='item.text'></Input>
          <!--<Checkbox v-model="item.single">显示</Checkbox>-->
          <RadioGroup v-model="item.Display">
            <Radio label="0">不显示</Radio>
            <Radio label="1">显示</Radio>
          </RadioGroup>
		      <Button style="margin-left:36%;" type="primary" @click="deleteBtn(index)">删除</Button>
					<!--<Radio v-model="item.single" :on-change="singleChange">显示</Radio>-->
				  <div class="rtf-content" style="width: 80%;display: block;margin-top: 5px;">
						<quill-editor ref="myTextEditor" v-model="item.Content" :config="editorOption_basicInf"></quill-editor>
					</div>
					<div v-show="item.TypeId==2">
						<Input style="width: 30%;margin-top: 5px;" v-model='dishContent.HotelLongitude' placeholder="经度"></Input>
            <Input style="width: 30%;margin-top: 5px;" v-model='dishContent.HotelAtitude' placeholder="纬度"></Input>
          </div>
			  </div>
		  </div>

		  <!--主辅料配置-->
		  <div class="box-c" v-show="current==2">
        <!--主料-->
        <div class="bag">
          <p style="overflow:auto;">
            <span class="left">主料配置</span>
            <span class="right" style="width: 180px">
              <RadioGroup v-model="material.ZhuLiao.Display">
                <Radio label="1">单列显示</Radio>
                <Radio label="2">双列显示</Radio>
              </RadioGroup>
            </span>
          </p>
          <ul class="dish_ul">
            <li v-for="(item,index) in material.ZhuLiao.Data" class="dish_li">
              <Input v-model="item.Name"  placeholder="主料名称" style="width: 150px"></Input>
              <Input v-model="item.Amount"  placeholder="数量" style="width: 100px"></Input>
              <Select  style="width:100px;position: relative" v-model="item.Unit" >
                <Option v-for="item in unitList" :value="item.Unit" :key="item.Id">{{ item.Unit }}</Option>
              </Select>
              <a @click='removeOneClick(item)'  style="cursor: pointer;position: relative;top: 5px;"> <Icon type="ios-close-outline" size='20'></Icon></a>
              <a @click='addOneClick(item)' v-if="index ==material.ZhuLiao.Data.length-1"  style="cursor: pointer;position: relative;top: 5px;"> <Icon type="ios-plus" size='20'></Icon></a>
            </li>
          </ul>
        </div>
        <!--辅料-->
        <div class="bag">
          <p style="overflow:auto;">
            <span class="left">辅料配置</span>
            <span class="right" style="width: 180px">
              <RadioGroup v-model="material.FuLiao.Display">
                <Radio label="1">单列显示</Radio>
                <Radio label="2">双列显示</Radio>
              </RadioGroup>
            </span>
          </p>
          <ul class="dish_ul">
            <li v-for="(item,index) in material.FuLiao.Data" class="dish_li">
              <Input v-model="item.Name"  placeholder="辅料名称" style="width: 150px"></Input>
              <Input v-model="item.Amount"  placeholder="数量" style="width: 100px"></Input>
              <Select  style="width:100px;position: relative" v-model="item.Unit">
                <Option v-for="item in unitList" :value="item.Unit" :key="item.Id">{{ item.Unit }}</Option>
              </Select>
              <a @click='removeTwoClick(item)'  style="cursor: pointer;position: relative;top: 5px;"> <Icon type="ios-close-outline" size='20'></Icon></a>
              <a @click='addTwoClick(item)' v-if="index ==material.FuLiao.Data.length-1"  style="cursor: pointer;position: relative;top: 5px;"> <Icon type="ios-plus" size='20'></Icon></a>
            </li>
          </ul>
        </div>
        <!--配置调料-->
        <div class="bag">
          <span style="font-weight: bolder">调料配置</span>
          <div style="margin-top: 1rem">
            <Select v-model="ingredients" style="width:200px;margin-right:1rem;position: absolute">
              <Option v-for="item in Products" :value="item.ProductId" :key="item.ProductId">{{ item.ProductName }}</Option>
            </Select>
            <Button type="primary" style="margin-left: 14rem" @click="addThreeClick">新增</Button>
          </div>
          <div>
            <ul class="ul_one">
              <li class="li_one" v-for="(item,index) in material.TiaoLiao">
                <Input  v-model="item.Name" :disabled="item.ProductId==0?false:true" placeholder="调料名称" style="width: 150px"></Input>
                <Input v-model="item.Amount"  placeholder="数量" style="width: 100px"></Input>
                <Select  style="width:100px;position: absolute" v-model="item.Unit">
                  <Option v-for="item in unitList" :value="item.Unit" :key="item.Id">{{ item.Unit }}</Option>
                </Select>
                <a @click='removeThreeClick(item)'  style="cursor: pointer;position: relative;top: 5px;margin:0 0.5rem;margin-left: 7rem"> <Icon type="ios-close-outline" size='20'></Icon></a>
                <!--<Button type="primary"  @click="addFourClick(index)" size="small">新增子项</Button>-->
                <ul class="ul_two">
                  <li v-for="(two,h) in item.Items">
                    <Input  v-model="two.Name" :disabled="two.ProductId==0?false:true"  placeholder="调料名称" style="width: 150px"></Input>
                    <Input v-model="two.Amount"  placeholder="数量" style="width: 100px"></Input>
                    <Select  style="width:100px;position: absolute" v-model="two.Unit">
                      <Option v-for="un in unitList" :value="un.Unit" :key="un.Id">{{ un.Unit }}</Option>
                    </Select>
                    <a @click='removeFourClick(two,h)'  style="cursor: pointer;position: relative;top: 5px;margin:0 0.5rem;margin-left: 7rem"> <Icon type="ios-close-outline" size='20'></Icon></a>
                  </li>
                </ul>
              </li>
            </ul>
          </div>
        </div>
        <!--富文本 烹饪步骤-->
			  <div style="margin-top: 30px;padding-top:20px;">
					<Input style="width: 30%;margin-right: 1rem" v-model='material.text'></Input>
          <RadioGroup v-model="material.Display">
            <Radio label="0">不显示</Radio>
            <Radio label="1">显示</Radio>
          </RadioGroup>
				  <div class="rtf-content" style="width: 80%;display: block;margin-top: 5px;">
						<quill-editor ref="myTextEditor" v-model="material.Steps" :config="editorOption_basicInf"></quill-editor>
					</div>
			  </div>
		  </div>
		  <div v-show="current==3" style="height: 200px;">
		  	<p style="font-size: 20px;text-align: center;">已保存成功</p>
		  	<p style="text-align: center;margin-top: 30px;">
		  		<Button type="primary" @click="succBtn('1')">好的</button>
		  		<Button type="primary" @click="succBtn('2')" style="margin-left:2%;">再发一条</button>
		  	</p>
		  </div>
		  <p v-show="current!=3"><Button style="margin:30px 45%;" type="primary" @click="saveBtn" class="saveB">保存</Button></p>
    </Modal>

    <!--修改-->
    <!--<Modal title="修改板块内容" v-model="editFlag" width='60%' :mask-closable="false" class="modal" >-->

    <!--</Modal>-->
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
        <Button type="error" size="small" @click="delBtn(1)" v-if="deleteBtnFlag">删除</Button>
        <Button type="error" size="small" @click="delBtn(2)" v-else>删除</Button>
      </div>
    </Modal>
    
    <!--板块蒙版-->
    <div class="mask" v-show="maskFlag">
       <div v-show="bkFlag" style="width: 240px;border-radius:5px;background: #fff;padding: 20px;margin: 20%;">
			      <p style="margin: 10px 0;">微信首页显示不显示</p>
				  	<RadioGroup v-model="indexShow">
				        <Radio label="1">显示</Radio>
				        <Radio label="0">不显示</Radio>
				    </RadioGroup>
            <Button type="primary" size="small" @click="okClick">确定</Button>
			  </div>
    </div>
    
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
				maskFlag:false,
				bkFlag:false,
//				bkLength:0,//板块数量
				num:0,//第一次不弹框
				indexShow:'',//选择板块
			  // proData:[],
			  deleteBtnFlag:true,//列表删除按钮
			  delImgIndex:'',//删除图片index值
			  search:{
          datetime:[],//搜索时间
          information:'',//搜索关键字
          count:100,
          pageIndex:1,
        },
        selectTabArr:[],//选中的数据
        modalFlag:false,//菜品新增,&编辑
        model_delete:false,//删除提示框
				current:0,//新增弹框 显示当前步骤
				modelplate:[],//板块下拉值
				modeldishcontent:'',//菜品内容下拉值
				editorOption_basicInf:{},
				UPImage:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Produce',//图片上传地址
        firstList:[],//一级分类数据
        plateList:[],//板块数据
        plateLists:[],//板块数据
        CaiPinId:0,//保存后的
        Action:'',//Create创建  Edit编辑
        //分类
        SecondList:[],
        dish_category:{//菜品分类
          CaiPinName:'',//菜品名
//        Action:'Create',
          CaiPinId:0,//菜品id  新增为0
          list:[//新增分类 菜谱分类部分
            {
              modelfirst: '',//一级分类下拉值
              modelsecondAll:[],//二级分类所有值
              modelsecond: []//二级分类下拉值
            }
          ],
          VideoUrl:'',//视频链接
          VideoImageUrl:'',//视频封面图片
          BanKuai:[],//所选板块
          ImageUrlList:[]//菜品图片
        },
        //菜品内容
        dishContent:{//菜品内容
          Action:'Create',//Edit
          CaiPinId:'',
          HotelLongitude:'',//经度
          HotelAtitude:'',//纬度
			    list:[
            // {
            //   "TypeId": 1,
            //   "Display":1,
            //   "Content": ""
            // },
          ]
        },
//				longitude:'',//经度
//				latitude:'',//纬度
				dishContentList:[],
        //配料
        ingredients:0,//配料选择
        material:{
          CaiPinId:'',
          Action:'Create',//Create新增
          Steps:'',//步骤
          text:'烹饪步骤',
        	Display:1,//步骤是否显示 
          ZhuLiao:{//主料
            Display:1,
            Data:[
              {
                Name:'',
                Amount:'',
                Unit:null,
              }
            ]
          },
          FuLiao:{
            Display:2,
            Data:[
              {
                Name:'',
                Amount:'',
                Unit:null,
              }
            ]
          },
          TiaoLiao:[
            // {
            //   ProductId: 0,
            //   Name: "水",
            //   Amount: "50",
            //   Unit: 1,
            //   Items: [
            //     {
            //       ProductId: 10,
            //       Name: "老抽",
            //       Amount: "10",
            //       Unit: 1,
            //     },
            //   ]
            // }
          ]
        },
        unitList:[],
        Products:[],//产品
				table_columns:[
					{
						title:'选择',
              type: 'selection',
              align: 'center',
              width:'60'
          },
          {
              title:'序号',
              align: 'center',
              type: 'index',
              width:'60'
          },
          {
            title: '菜系',
            align: 'center',
            key: 'CaiXi',
          },
          {
            title: '菜式',
            align: 'center',
            key: 'CaiShi',
          },
          {
            title: '食材',
            align: 'center',
            key: 'ShiCai',
          },
          {
            title: '板块名称',
            align: 'center',
            key: 'BanKuaiName'
          },
          {
              title: '菜品名称',
              align: 'center',
              key: 'CaiPinName',
          },
          {
              title: '预览',
              align: 'center',
              key: 'VideoImage',
              render:(h,paeams) =>{
                return h('img',{
                  attrs:{
                    src:paeams.row.VideoImage
                  },
                  style:{
                    maxWidth:'5rem'
                  }
                });
              }
          },
          {
            title: '发布',
            align: 'center',
            key: 'IsPublish'
          },
          {
              title: '收藏人数',
              align: 'center',
              key: 'ScCount'
          },
          {
              title: '浏览次数',
              align: 'center',
              key: 'LlCount'
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
                      }, '编辑'),
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
				tabInfoList:[],
				bkvalue:[],//板块显示选中数据
				bklength:''//数据长度
			}
		},
		methods:{
			optionChange(value){//点击板块内容
			  this.bkvalue = value;
			  if(value.length > 0 && this.num != 0 && value.length > this.bklength){
			  	this.maskFlag = true;
			    this.bkFlag = true;
			  	this.bklength = value.length
			  }else if(this.num == 0 || value.length == 0 || value.length < this.bklength){
			  	this.num++
			  	this.bkFlag = false;
			  	this.maskFlag = false;
			  	this.bklength = value.length
			  }
			},
			okClick(){//点击确定
				var bkid = this.bkvalue[this.bkvalue.length-1]
				var index = this.bkvalue[this.bkvalue.length-1].split('&')[0];
				this.bkvalue[this.bkvalue.length-1] = index + '&' + this.indexShow
				for(let i in this.plateLists){
					var plateLists = this.plateLists[i];
					if(plateLists.BanKuaiId==bkid){
						plateLists.BanKuaiId = this.bkvalue[this.bkvalue.length-1]
					}
				}
			  this.bkFlag = false;
			  this.maskFlag = false;
			},
			deleteImg(index){//分类 删除菜品图片
				this.model_delete = true;//删除弹框
        this.deleteBtnFlag = false;//显示删除图片的按钮
        this.delImgIndex = index;
			},
			search_click(){//查询
        this.getInfoList()
			},
			add_click(){//新增
        this.CaiPinId = 0
			  this.clearModal('Create')
			  this.Action = 'Create';
				this.modalFlag = true;
				//板块显示
				this.num=0;
				this.dish_category.BanKuaiArr = [];
        this.bklength = 0
			},
			release_click(){//发布
        this.ReleaseAndNorelease(1,this.selectTabArr.join(','))
			},
			norelease_click(){//不发布
        this.ReleaseAndNorelease(0,this.selectTabArr.join(','))
			},
      ReleaseAndNorelease(type,CaiPinId){//0不发布 1发布
			  var self =this
        var data = {
          Flag:type,
          CaiPinId:CaiPinId
        }
        console.log(data)
        $.ajax({//
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/Publish',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(data),
          success:function(res){
            var res = JSON.parse(res)
            console.log(res)
            if (res.code==200){
              self.$Message.success(res.message);
              self.getInfoList()
            }else {
              self.$Message.error(res.message);
            }

          },
          error:function(error){
            self.$Message.error('数据上传失败');
          }
        })
      },
			select_click(selection){//选中复选框
        this.selectTabArr= []
        selection.map((item) =>{
          this.selectTabArr.push(item.CaiPinId)
        })
			},
			redact_click(params){//编辑
//			  console.log(params.row.CaiPinId)
        this.CaiPinId = params.row.CaiPinId
        this.clearModal('Edit');
        this.Action = 'Edit';
				this.modalFlag = true;
				this.num=0;
        this.dishCategoryGet()//分类数据获取
        this.dishContentGet()//内容数据获取
        this.disMaterialGet()//主辅调料数据获取
			},
			delete_click(params){//删除列表数据
			  this.model_delete = true
        this.CaiPinId = params.row.CaiPinId
        this.deleteBtnFlag = true;//显示删除列表数据的按钮
        console.log(this.CaiPinId)
			},
      delBtn(val){//删除确认
      	if(val == 1){//删除列表数据
      		this.deleteApi();//删除列表数据 接口
      	}else if(val == 2){//删除图片
      		this.dish_category.ImageUrlList.splice(this.delImgIndex,1);
			    this.model_delete = false;//隐藏弹框
      	}
      },
      deleteApi(){//删除列表数据 接口
      	var self = this
        var data ={
          Flag:0,
          CaiPinId:this.CaiPinId
        }
        $.ajax({
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/Enable',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(data),
          success:function(res){
            var res = JSON.parse(res)
            console.log(res)
            if (res.code==200){
              self.$Message.success('数据删除完成');
              self.getInfoList()
            }else {
              self.$Message.error('数据删除失败');
            }
          },
          error:function(error){
            self.$Message.error('数据删除失败');
          }
        })
        this.model_delete = false
      },
			sort_change(val){//tab表头排序
				console.log(val);
				this.getInfoList();//获取菜品内容管理列表
			},
			changgePage(index){//分页切换
				this.search.pageIndex = index;
				this.getInfoList()
			},
			getInfoList(){//获取菜品内容管理列表
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
        let data = {page:search.pageIndex,pagesize:'10',startdate:startdate,enddate:enddate,keyword:search.information}
        //console.log(data)
        $.ajax({//
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/Query',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:JSON.stringify(data),
          success:function(res){
            var data = JSON.parse(res)
            self.tabInfoList=[]
            self.tabInfoList = data.data
            console.log(data)
            self.search.count = data.totalcount

          },
          error:function(error){
            self.$Message.error('获取数据失败');
          }
        })
			},
      stepClick(index){
			  console.log(index)
        this.current = index;
      },
      //==============================分类==============================
			createMenuClick(){//新增菜谱分类
				var item={
				  modelfirst: '',
          modelsecondAll:[],
          modelsecond: []
				}
				this.dish_category.list.push(item);
			},
      getFirst(){//获取一级分类 和 板块 和 单位
        var self = this
        //分类
        $.ajax({//一级分类
          type:'get',
          url:URLHeader.foodLibrary+'/Api/Category/GetFirst',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            var data = JSON.parse(res).data;
            self.firstList = data
            // console.log(data)
          },
          error:function(error){
            self.$Message.error('一级分类获取失败');
          }
        })
        $.ajax({//板块
          type:'get',
          url:URLHeader.foodLibrary+'/Api/BanKuai/Get',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            var data = JSON.parse(res);
            console.log('获取板块数据')
            self.plateList = data
            console.log(data)
            
            self.plateLists = [];
            for(let i in  self.plateList){
            	var item={
            		'BanKuaiId':self.plateList[i].BanKuaiId+'&'+0,
            		'BanKuaiName':self.plateList[i].BanKuaiName
            	}
            	self.plateLists.push(item)
            }
            
          },
          error:function(error){
            self.$Message.error('板块获取失败');
          }
        })
        //菜品内容
        $.ajax({//富文本下拉
          type:'get',
          url:URLHeader.foodLibrary+'/Api/CaiPin/GetRichTextType',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            var data = JSON.parse(res);
            self.dishContentList = data
            //console.log(data)
          },
          error:function(error){
            self.$Message.error('一级分类获取失败');
          }
        })
        //主辅调料
        $.ajax({//单位
          type:'get',
          url:URLHeader.foodLibrary+'/Api/CaiPin/GetMaterialUnit',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            var data = JSON.parse(res);
            self.unitList = data
            //console.log(data)
          },
          error:function(error){
            self.$Message.error('一级分类获取失败');
          }
        })
        //Products
        $.ajax({//菜品调料  公司产品
          type:'get',
          url:URLHeader.foodLibrary+'/Api/CaiPin/GetProducts',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            var data = JSON.parse(res);
            var item = {
              ProductId:0,
              ProductName:'其它'
            }
            data.unshift(item)
            self.Products = data
            //console.log(data)
          },
          error:function(error){
            self.$Message.error('一级分类获取失败');
          }
        })
      },
      firstSelectChange(name,index){//一级分类变更
			  var self =this
        console.log('一级分类变更')
        const pro = new Promise((resolve, reject) => {
           self.getSecondIfo(name.modelfirst,resolve)
        });
			  pro.then(function (res,fal) {
          self.dish_category.list.map((item) =>{
            if (item.modelfirst==name.modelfirst) {
              //console.log(item)
              if (!item.get){//正常选择后变更
                self.dish_category.list[index].modelsecond = []
                self.dish_category.list[index].modelsecondAll = self.SecondList
              }else {//dishCategoryGet专用
                //self.dish_category.list[index].modelsecondAll = self.SecondList
                delete item.get
              }


            }
          })
        })

        // const pro = new Promise(self.getSecondIfo(resolve,name.modelfirst,data));
        // pro.then(function (res,fal) {
        //
        //   console.log(data)
        // })


        // $.ajax({
        //   type:'get',
        //   url:URLHeader.foodLibrary+'/Api/Category/GetSecondByFirst?FirstId='+name.modelfirst,
        //   dataTape:'json',
        //   contentType:'application/json; charset=utf-8',
        //   data:{},
        //   success:function(res){
        //     var data = JSON.parse(res);
        //      //console.log(data)
        //     self.dish_category.list.map((item) =>{
        //       if (item.modelfirst==name.modelfirst) {
        //         self.dish_category.list[index].modelsecond = []
        //         self.dish_category.list[index].modelsecondAll = data
        //       }
        //     })
        //   },
        //   error:function(error){
        //     self.$Message.error('二级分类获取失败');
        //   }
        // })
        //console.log(this.firstList)

      },
      getSecondIfo(modelfirst,resolve){
			  var self =this
        console.log(modelfirst)
        if (modelfirst==null ||!modelfirst){
          return false
        }
        $.ajax({
          type:'get',
          url:URLHeader.foodLibrary+'/Api/Category/GetSecondByFirst?FirstId='+modelfirst,
          dataTape:'json',
          sync:false,
          contentType:'application/json; charset=utf-8',
          data:{},
          success:function(res){
            self.SecondList = []
            self.SecondList = JSON.parse(res)
            //console.log(self.SecondList)
            //console.log(resolve)
            if (resolve){
              resolve('resolve')
            }
            //return JSON.parse(res)
          },
          error:function(error){
            self.$Message.error('二级分类获取失败');
          }
        })
      },
			removeMenuClick(item){//删除菜谱分类
				this.dish_category.list.splice(this.dish_category.list.indexOf(item),1);
			},
      dishCategorySave(){//分类数据保存
        var self = this
        var dish_category = this.dish_category
        console.log(dish_category);
        let  SecondIdArr = [];
        dish_category.list.map((one) =>{
          SecondIdArr = SecondIdArr.concat(one.modelsecond)
        })
        
        var bkArr = [];
        dish_category.BanKuaiArr.map((item) =>{
        	console.log(item);
        	var itemData = {
        		"Id": Number(item.split('&')[0]),
						"IndexShow": Number(item.split('&')[1])
        	}
        	bkArr.push(itemData)
        })
        
        
        var data = {
          CaiPinName:dish_category.CaiPinName,
          Action:this.Action,
          CaiPinId:dish_category.CaiPinId,
          VideoUrl:dish_category.VideoUrl,
          VideoImageUrl:dish_category.VideoImageUrl,
          SecondId:SecondIdArr.join(','),
//        BanKuaiId:dish_category.BanKuaiId.join(','),
          BanKuai:bkArr,
          ImageUrlList:dish_category.ImageUrlList
        }
        const DATA = JSON.stringify(data)
        console.log(data);
        $.ajax({//
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/CreateOrEditStep1',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:DATA,
          success:function(res){
            console.log(res)
            if (JSON.parse(res).status=='succ'){
              self.$Message.success('菜品内容保存完成');
              self.CaiPinId = JSON.parse(res).data
              self.nextClick()
            }else {
              self.$Message.error('菜品内容保存失败');
            }
          },
          error:function(error){
            self.$Message.error('分类保存失败');
          }
        })
      },
      dishCategoryGet(){//分类数据获取
			  var self =this
        self.GetStep1 = true,
        $.ajax({//
          type:'get',
          url:URLHeader.foodLibrary+'/Api/CaiPin/GetStep1?CaiPinId='+self.CaiPinId,
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          success:function(res){
            var data = JSON.parse(res)
            console.log(data)
            if(data.Images!=null && data.Images.length>0){
            	data.ImageUrlList = data.Images.split(',')
            }else{
            	data.ImageUrlList = []
            }
           
            data.VideoImageUrl = data.VideoImage
            data.list = []

            data.CategoryList.map((item,index) =>{
              var secondArr = [];
              item.SecondList.map((items,index) =>{
              	secondArr.push(items.SecondId)
              })
              
              var one = {
                modelfirst:item.FirstId,
                modelsecondAll:item.SecondList,
                modelsecond:secondArr,
                get:true
              }
              data.list.push(one)
            })
            
            
            delete data.Images
            delete data.VideoImage
            delete data.CategoryList
            self.dish_category = data
//
            console.log('获取分类数据');
            console.log(self.dish_category);
            
            self.dish_category.BanKuaiArr = [];
            for(let i in self.dish_category.BanKuai){
            	var BanKuai = self.dish_category.BanKuai[i];
            	var data = BanKuai.BanKuaiId+'&'+BanKuai.IndexShow
            	self.dish_category.BanKuaiArr.push(data);//展示板块
            }
            self.bklength = self.dish_category.BanKuaiArr.length;//选中板块的长度
           
            for(let i in self.plateList){
            	var plateList = self.plateList[i];
            	for(let j in self.dish_category.BanKuai){
            		var bkItem= self.dish_category.BanKuai[j];
            		if(plateList.BanKuaiId == bkItem.BanKuaiId){
            			self.plateLists[i].BanKuaiId = plateList.BanKuaiId + '&' + bkItem.IndexShow
            		}
            	}
            }
            
          },
          error:function(error){
            self.$Message.error('分类数据获取失败');
          }
        })
      },
      //==============================菜品内容逻辑处理==============================
      deleteBtn(index){//删除菜品内容
        this.dishContent.list.splice(index,1);
//      console.log(this.dishContent.list)
      },
			addDishContent(){//新增菜品内容
			  var self = this

        if (this.modeldishcontent=='' || self.dishContent.list.length==self.dishContentList.length){
			    return false
        }
        var flag =  self.dishContent.list.map((item) => {
          if (parseInt(item.TypeId)==parseInt(self.modeldishcontent)) {//
            return false
          }
        })
        //console.log(flag)
        if (flag[flag.length-1]==false){
			    return false
        }
        var text = ''
        this.dishContentList.map((item) =>{
          if (item.Id==this.modeldishcontent){
              text = item.Type
          }
        })
        var item={
          text:text,//名称
          TypeId:self.modeldishcontent,//id
          Display:'1',//是否显示
          Content:''//富文本内容
        }
        self.dishContent.list.push(item);

          console.log(this.dishContent.list)
			},
      dishContentSave(){//菜品内容保存
        var self =this
        if (!self.CaiPinId){
          self.$Message.error('菜品id不存在');
          return false
        }
        var dishContent = self.dishContent
        var List = []
        dishContent.list.map((item) =>{
          var  one = {
            TypeId:item.TypeId,
            Display:item.Display,
            Content:item.Content
          }
          List.push(one)
        })
        var data = {
          CaiPinId:self.CaiPinId,
          Action:dishContent.Action,
          HotelLongitude:dishContent.HotelLongitude,//经度
          HotelAtitude:dishContent.HotelAtitude,//纬度
          List:List

        }
        var  DATA = JSON.stringify(data)
        console.log(DATA)
        $.ajax({//
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/CreateOrEditStep2',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:DATA,
          success:function(res){
            var data = JSON.parse(res)
            console.log(data)
            if (data.status=="succ"){
              self.$Message.success('菜品内容保存完成');
              self.nextClick()
            }else {
              self.$Message.error('菜品内容保存失败');
            }

          },
          error:function(error){
            self.$Message.error('菜品内容保存失败');
          }
        })
        console.log(self.dishContent)
      },
      dishContentGet(){//菜品内容获取
			  var self =this
        $.ajax({//
          type:'get',
          url:URLHeader.foodLibrary+'/Api/CaiPin/GetStep2?CaiPinId='+self.CaiPinId,
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          success:function(res){
            var data = JSON.parse(res)
            console.log(data)
            // self.current = 1
            data.map((item) =>{
              item.text = item.Type
            })
            self.dishContent.list = data
            if(data.length != 0){//有菜品内容
            	self.dishContent.HotelLongitude = data[0].HotelLongitude;//经度
            	self.dishContent.HotelAtitude = data[0].HotelAtitude;//纬度            	
            }

          },
          error:function(error){
            self.$Message.error('分类保存失败');
          }
        })
      },
      //==============================主辅调料==============================
      removeOneClick(item){//主料删除
        this.material.ZhuLiao.Data.splice(this.material.ZhuLiao.Data.indexOf(item),1);
      },
      addOneClick(){//主料增加
        var one = {
            "Name": "",
            "Amount": "",
            "Unit": ""
          }
        this.material.ZhuLiao.Data.push(one)
      },
      removeTwoClick(item){//辅料删除
        this.material.FuLiao.Data.splice(this.material.FuLiao.Data.indexOf(item),1);
      },
      addTwoClick(){//辅料增加
        var one = {
          "Name": "",
          "Amount": "",
          "Unit": ""
        }
        this.material.FuLiao.Data.push(one)
      },
      removeThreeClick(item){//配料 一级删除
        this.material.TiaoLiao.splice(this.material.TiaoLiao.indexOf(item),1);
      },
      addThreeClick() {//一级新增

        var  name = ''
        this.Products.map((item) =>{
          if (item.ProductId==this.ingredients){
            name = item.ProductName
          }
        })
        if (this.ingredients==0){
          name=''
        }
        var item = {
            ProductId: this.ingredients,
            Name:name,
            Amount: "",
            Unit: null,
            Items: []
          }
        this.material.TiaoLiao.push(item)
			  console.log(this.material.TiaoLiao)

      },
      addFourClick(index){

        //0 为其它
        // if (!this.ingredients){
        //   return false
        // }
        var  name = ''
        var  disabled = true
        this.Products.map((item) =>{

          if (item.ProductId==this.ingredients){
            name = item.ProductName
          }
        })
        if (this.ingredients==0){
          name=''
        }
        var item = {
          ProductId: this.ingredients,
          Name:name,
          Amount: "",
          Unit: null,
          Items: []
        }
        this.material.TiaoLiao[index].Items.push(item)
      },
      removeFourClick(item,index){
        console.log(item)
        console.log(index)
        this.material.TiaoLiao[index].Items.splice(this.material.TiaoLiao[index].Items.indexOf(item),1);
      },
      dishMaterialSave(){//主辅料保存
        console.log(this.material)
        var self =this
        var material = this.material
        var data = {
          CaiPinId:self.CaiPinId,
          Action:material.Action,
          ZhuLiao:material.ZhuLiao,
          FuLiao:material.FuLiao,
          TiaoLiao:material.TiaoLiao,
          Steps:material.Steps
        }
        var  DATA = JSON.stringify(data)
        console.log(DATA)
        $.ajax({//
          type:'post',
          url:URLHeader.foodLibrary+'/Api/CaiPin/CreateOrEditStep3',
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          data:DATA,
          success:function(res){
            var  data = JSON.parse(res)
            console.log(data)
            if (data.status=="succ"){
              self.$Message.success('主辅调料配置保存完成');
              //saveB

              $('.saveB').hide()
              self.nextClick()
            }else {
              self.$Message.error('主辅调料配置保存失败');
            }


          },
          error:function(error){
            self.$Message.error('主辅调料配置保存失败');
          }
        })

      },
      disMaterialGet(){//主辅料数据获取
			  var self =this
        $.ajax({//
          type:'get',
          url:URLHeader.foodLibrary+'/Api/CaiPin/GetStep3?CaiPinId='+self.CaiPinId,
          dataTape:'json',
          contentType:'application/json; charset=utf-8',
          success:function(res){
            //console.log(res)
            var  data = JSON.parse(res)
            console.log(data)
            data.Action = 'Edit'
            data.text = '烹饪步骤'
            data.Display = '1'//默认显示
            self.material = data
          },
          error:function(error){
            self.$Message.error('主辅调料配置保存失败');
          }
        })
      },
      //==============================主辅调料结束==============================
      saveBtn(){//保存按钮
        switch (this.current) {
          case 0://分类
            this.dishCategorySave();
            break;
          case 1://菜品内容
            this.dishContentSave()
            break;
          case 2://主辅料
            this.dishMaterialSave()
            break;
          default:
            break
        }
      },
			nextClick(){//新增弹框 点击下一步
				this.current++;
				if(this.current > 3){
					this.current = 0;
				}
			},
      clearModal(Action){
			  var self =this
			  
        self.current = 0//控制步骤条
        //self.CaiPinId = CaiPinId
         // self.$refs.saveB.style.display = 'block'
        $('.saveB').show()
        //分类
        self.dish_category = {//菜品分类
            CaiPinName:'',//菜品名
//          Action:Action,//Create创建  Edit编辑
            CaiPinId:self.CaiPinId,//菜品id  新增为0
            list:[//新增分类 菜谱分类部分
	            {
	              modelfirst: '',//一级分类下拉值
	              modelsecondAll:[],//二级分类所有值
	              modelsecond: []//二级分类下拉值
	            }
            ],
            VideoUrl:'',//视频链接
            VideoImageUrl:'',//视频封面图片
            BanKuaiId:[],//所选板块
            ImageUrlList:[]//菜品图片
        }
        //菜品内容
          self.dishContent = {//菜品内容
            Action:Action,//Edit
            HotelLongitude:'',//经度
            HotelAtitude:'',//纬度
            list:[]
        }
//      self.longitude = ''//经度
//      self.latitude = ''//纬度
        //主辅调料配置
        self.material = {
            CaiPinId:self.CaiPinId,
            Action:Action,//Create新增
            Steps:'',//步骤
            text:'烹饪步骤',
        	  Display:1,//步骤是否显示 
            ZhuLiao:{//主料
              Display:1,
              Data:[
	              {
	                Name:'',
	                Amount:'',
	                Unit:null,
	              }
            	]
            },
	          FuLiao:{
	            Display:1,
	              Data:[
	              {
	                Name:'',
	                Amount:'',
	                Unit:null,
	              }
	            ]
	          },
          	TiaoLiao:[]
        }
        
//      console.log(self.dish_category.Action);
      },
      succBtn(val){//保存成功 1关闭录入菜品页面 2新增一条
      	if(val == '1'){
      		this.modalFlag = false 
      	}else if(val == '2'){
      		this.add_click();//新增一条
      		this.current = 0;
      	}
      },
			successUpload_a(res, file){//上传视频封面图片
//      console.log(file);
        var data = JSON.parse(res);
        this.dish_category.VideoImageUrl = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
			},
			successUpload_b(res, file){//上传菜品图片
        var data = JSON.parse(res);
        var dishurl = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
        this.dish_category.ImageUrlList.push(dishurl);
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
                desc: '文件 ' + file.name + ' 太大，不能超过 3M。'
            });
      },
      handleBeforeUpload () {//选择图片
      		$('.loding').show()
//              return check;
     	},

		},
		mounted(){
			this.getInfoList();//获取菜品内容管理列表
      this.getFirst()//获取下拉列表数据
		}
	}
</script>

<style scoped>
.page{
	margin-top: 1rem;
	float: right;
}
.edit_text span{
	width: 30%;
	display: inline-block;
}
.createDish p {margin-top: 5px;}
.createDish p span{width: 10%;display: inline-block;}
.uploadimg *{display: inline-block;vertical-align: bottom;}
.mask{width: 100%;height: 100%;position: fixed;top: 0;background: rgba(0,0,0,0.6);z-index: 1000000;}
</style>
<style>
.box-b .ql-container,.box-c .ql-container{
	height: 200px;
}
.box-b .quill-editor{
	/*height: 200px;*/
}
/*视频地址弹框移动*/
.box-b .ql-snow .ql-tooltip{
	margin-left: 9rem;
}
.modal .ivu-modal-footer{display: none !important;}
  /*主辅料*/
.modal .bag{
  background: rgb(239,239,239);
  padding: 1rem;
  margin-top: 1rem;
  border-radius:5px ;
}
.modal .box-c{
  overflow: auto;
}
.modal .dish_ul{
  margin-top: 1rem;
}
.modal .dish_ul .dish_li{
  margin-top: 0.5rem;
}
.modal .left{
  font-weight: bolder;
  display: block;
  float: left;
}
.modal .right{
  display: block;
  float: right;
  margin-right: 3rem;
}
.li_one{
  margin-top: 1rem;
}
.ul_two{
  margin-left: 2rem;
  margin-top: 0.5rem;
}
.ivu-modal-footer{border: none !important;}
.ivu-modal-header{border: none !important;}
</style>
