<template>
	<!--产品新增与编辑-->
	<div class="editAdd">
		<Spin fix class='loding'>
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>Loading</div>
        </Spin>
		<Tabs type="card"  @on-click='tabClickChang' :value="tabName">
	        <TabPane label="产品信息" name='productIfo' class='productIfo tab'>
	        		<Modal
				    title="规格"
				    width='300'
				    v-model="addSize_Modal"
				    :mask-closable="false">
				    <ul>
    						<li v-for='item in sizeList' style="margin-top: 0.5rem;">
    							<Input v-model='item.Amount' placeholder="规格" style="width: 140px;"></Input>
    							<Select v-model="item.Unit" style="width:80px">
							    <Option v-for="item in unitList" :value="item.value" :key="item.value">{{ item.label }}</Option>
							</Select>
    							<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
    							<a  style="cursor: pointer;" @click='removeSizeBtn(item)'> <Icon type="close-circled" ></Icon></a>
    						</li>
    						<li>
    							<Button type="primary"  style="display: block;margin: 0 auto;margin-top:0.5rem;" @click="addSIzeBtn">添加规格</Button>
    						</li>
    					</ul>
				</Modal>
				<ul style="width: 35rem;padding-bottom: 1rem;float: left;">
					<li class="li">
						<span class="lable">产品名称:</span>
						 <Input class="value" v-model="productIfoData.ProductName" style="width: 200px;"  placeholder="产品名称"></Input>
					</li>
					<li class='li'>
						<span class="lable">产品分类:</span>
						<Select class="value" v-model="productIfoData.ProductFirstId" @on-change="oneSelectChange"  style="width:90px" placeholder = '请选择'>
				        		<Option v-for="item in oneTypeList" :value="item.ProductFirstId" :key="item.ProductFirstId">{{ item.ProductFirstName }}</Option>
				   	 	</Select>
				   	 	<Select class="value" v-model="productIfoData.ProductSecondId" style="width:90px;" placeholder = '请选择'>
				        		<Option v-for="item in twoTypeList" :value="item.ProductSecondId" :key="item.ProductSecondId">{{ item.ProductSecondName }}</Option>
				   	 	</Select>
					</li>
					<li class="li">
						<span class="lable">品牌名称:</span>
						<Select class="value" v-model="productIfoData.BrandName"  style="width:200px" placeholder = '请选择'>
				        		<Option v-for="item in brandNameList" :value="item.value" :key="item.value">{{ item.label }}</Option>
				   	 	</Select>
					</li>
					<li class="li">
	       				<span class="lable">规格</span>
	       				<Button type="warning" class="value" @click="createSizeBtn">创建规格</Button>
	       			</li>
					<li class="li">
						<span class="lable" style="display:block;float: left;">基本信息:</span>
						<div class="rtf-content" style="width: 27rem;display: block;float: left;margin-left: 1.2rem;">
					        <quill-editor ref="myTextEditor" v-model="productIfoData.ProductBasicInfo" :config="editorOption_basicInf"></quill-editor>
					    </div>
					</li>
					<li class="li">
						<span class="lable" style="display:block;float: left;">产品特点:</span>
						<div class="rtf-content" style="width: 27rem;display: block;float: left;margin-left: 1.2rem;">
					        <quill-editor ref="myTextEditor" v-model="productIfoData.ProductFeature" :config="editorOption_basicInf"></quill-editor>
					    </div>
					</li>
					
				</ul>
	       		<ul style="width: 28rem;float: left;">
	       			<li class="li">
	       				<span class="lable">产品图</span>
	       				<div style="margin-left: 1rem;" class="value">
	       					<img :src="productIfoData.ProductPicURL" style="height:8.5rem;border: 1px dashed gainsboro;border-radius:4px ;"/>
	        					<br />
	        					<Upload
						        ref="upload"
						        :show-upload-list="false"
						        :on-success="productSuccess"
						        :format="['jpg','jpeg','png']"
						        :max-size="2048"
						        :before-upload='handleBeforeUpload'
						        :on-format-error="handleFormatError"
						        :on-exceeded-size="handleMaxSize"
						        :multiple=false
						        type="drag"
						        :action=UPImage
						        style="display:block;width:80px;margin: 0 auto;" >
						       <!--<div style="width:8rem;height:2rem;line-height: 58px;text-align: center;line-height: 2rem;">
						            <Button type="primary">上传</Button>
						        </div>-->
						         <Button type="primary">选择大图</Button>
						    </Upload>
	       				</div>
	       			</li>
	       			
	       			<li class="li">
						<span class="lable" style="display:block;float: left;">产品价值:</span>
						<div class="rtf-content" style="width: 27rem;display: block;float: left;margin-left: 1.2rem;">
					        <quill-editor ref="myTextEditor" v-model="productIfoData.ProductValue" :config="editorOption_basicInf"></quill-editor>
					    </div>
					</li>
					<li class="li">
						<span class="lable" style="display:block;float: left;">烹饪技法:</span>
						<div class="rtf-content" style="width: 27rem;display: block;float: left;margin-left: 1.2rem;">
					        <quill-editor ref="myTextEditor" v-model="productIfoData.CookingSkill" :config="editorOption_basicInf"></quill-editor>
					    </div>
					</li>
	       		</ul>
	       		<div style="width: 42rem;">
	       			<Button type="error" @click="saveBtn">保存</Button>
	       		</div>
	        </TabPane>
	        <TabPane label="规格" name='Size' :disabled='SizeDisabled' class='Size tab'>
	        		<ul style="border-bottom: 2px dashed gray;overflow: auto;padding-bottom: 1rem;">
	        			<li style="width: 10.5rem;float: left;" v-for="(item,index) in sizeTopList">
	        				<h3 style="text-align: center;margin-bottom: 0.3rem;">{{item.Amount}}{{item.Unit}}</h3>
	        				<img :src="item.SpecificationPicUrl" style="width: 10rem;display: block;margin: 0 auto;border: 1px dashed gainsboro;border-radius:4px ;"/>
        					<br />
        					<Upload
					        ref="upload"
					        :show-upload-list="false"
					        :on-success="sizeImgSuccess"
					        :format="['jpg','jpeg','png']"
					        :max-size="2048"
					        :before-upload='handleBeforeUpload'
					        :on-format-error="handleFormatError"
					        :on-exceeded-size="handleMaxSize"
					        :multiple=false
					        type="drag"
					        :action=UPImage
					        style="width: 80px;display: inline-flex;" >
					       <!--<div style="width:8rem;height:2rem;line-height: 58px;text-align: center;line-height: 2rem;">
					            <Button type="primary">上传</Button>
					        </div>-->
					         <Button type="primary" @click="changeImg(index)">选择图片</Button>
					    </Upload>
					     <Button type="primary" @click="changeIfo(index)">配置信息</Button>
	        			</li>
	        		</ul>
	        		<div class="Information">
	        			<ul style="margin-top: 1rem;margin-left: 1rem;overflow: auto;">
	        				<li class="typeLi" style="margin-right: 2rem;width: 100%;height: 2rem;">
	        					<span style="font-size: 14px;font-weight: bold;">规格</span>
	        					<span style="font-size: 14px;font-weight: bold;margin-left: 7rem;">{{selectSizeTitle}}</span>
	        				</li>
	        				<li class="typeLi" style="margin-right: 2rem;">
	        					<h3>选择虚拟分类</h3>
	        					<h3 style="margin-top: 1.8rem;">排序</h3>
	        				</li>
	        				<li class="typeLi" v-for="(itme,index) in typeList" style="margin-left: 1rem;width: 6rem;">
	        					<Button type="success" style="display: block;margin: 0 auto;">{{itme.VCName}}</Button>
	        					<!-- @click="clikTypeBtn(index)"-->
	        					<Input v-model="itme.PriorityId" placeholder="请输入" style="width: 90px;;margin: 0 auto;margin-top: 1rem;display: block;"></Input>
	        					<Checkbox  v-model="itme.IsDisplay" @on-change = 'checkBoxChange(itme)'  style='margin-left: 1rem;margin-top: 0.5rem;'>显示</Checkbox>
	        				</li>
	        				<li style="margin-top: 10rem;">
	        					<h3 style="float: left;">物料编码</h3>
	        					<Input v-model="sizeInformation.MaterialNum" placeholder="物料编码" style="width: 200px;margin-left: 5rem;"></Input>
	        				</li>
	        				<li style="margin-top: 1rem;overflow: auto;">
	        					<h3 style="float: left;">销售区域</h3>
	        					<Select  v-model="SpecificationConfArea" multiple style="width:200px;margin-left: 5rem;" class="SpecificationConfArea">
						        <Option v-for="item in areaData" :value="item.value" :key="item.value">{{ item.label }}</Option>
						    </Select>
	        				</li>
	        				<li style="margin-top: 1rem;overflow: auto;display: none;" class="Introduce">
	        					<h3 style="float: left;">新品介绍</h3>
	        					<Input v-model="sizeInformation.Introduce" style="width: 200px;margin-left: 5rem;" type="textarea" :autosize="{minRows: 3,maxRows: 5}" placeholder="请输入"></Input>
	        				</li>
	        			</ul>
	        			<Button type="error" @click="saveSizeData()" style="float: left;margin-left: 15rem;margin-top: 1rem;">保存</Button>
	        		</div>
	        		
	        </TabPane>
	        <TabPane label="研发信息" name='researchIfo' :disabled='researchIfoDisabled' class='researchIfo tab'>
	        		<div style="overflow: -webkit-paged-y;">
		        		<div class="left">
		        			<h3 style="display: block;float: left;">研发故事</h3>
		        			<div class="rtf-content" style="display: block;float: left;margin-left: 1rem;">
					        <quill-editor ref="myTextEditor" v-model="researchData.RDStory" :config="editorOption_basicInf" style='width: 30rem;height: 20rem;'></quill-editor>
					    </div>
		        		</div>
		        		
		        		<div class="left" style="margin-left: 4rem;">
		        			<Modal
				        title="讲师新增及编辑"
				        v-model="addTeacherModel"
				        :mask-closable="false"
				        @on-ok='confirmBtn'>
					        <div style="overflow: auto;">
						        <div style="display: block;float: left;">
						        		 <img :src="teacherDetal.MasterHeaderPicUrl" style="max-width: 8rem;"/>
						        		 <Upload
								        ref="upload"
								        :show-upload-list="false"
								        :on-success="handleTeacherImgSuccess"
								        :format="['jpg','jpeg','png']"
								        :max-size="2048"
								        :before-upload='handleBeforeUpload'
								        :on-format-error="handleFormatError"
								        :on-exceeded-size="handleMaxSize"
								        :multiple=false
								        type="drag"
								        :action=UPImage
								        style="display:block;width:80px;margin-left: 1.5rem;" >
								         <Button type="primary">选择照片</Button>
								    </Upload>
						        </div>
						       	<div style="display: block;float: left;margin-left: 1rem;">
						       		<ul>
						       			<li>
						       				<span>讲师姓名</span>
						       				<Input v-model="teacherDetal.MasterName" placeholder="请输入..." style="width: 200px;margin-left: 1rem;"></Input>
						       			</li>
						       			<li style="margin:1rem auto ;overflow: auto;">
						       				<span style="display: block;float: left;height: 2rem;line-height: 2rem;">岗位</span>
											<!--<Cascader :data="jobsList" v-model="modelInformation.Post" style="width: 200px;float: left;margin-left: 2.8rem;"></Cascader>-->
											<Input v-model="teacherDetal.MasterStation" placeholder="请输入..." style="width: 200px;margin-left: 2.7rem;"></Input>
						       			</li>
						       			<li style="margin-top: 1rem;">
						       				<span>酒店</span>
						       				<Input v-model="teacherDetal.MasterHotal" placeholder="请输入..." style="width: 200px;margin-left: 2.5rem;"></Input>
						       			</li>
						       		</ul>
						       	</div>
					       </div>
					    </Modal>
		        			<div>
		        				<span>课程讲师</span>
							<Button type="primary" @click='addTeacherBtn' style="margin-left: 1rem;margin-right: 2rem;" >新增讲师</Button>
							<Checkbox v-model="researchData.IsDisPlay" style='color: red;font-weight:bold ;'>是否启用</Checkbox>
		        			</div>
		        			
		        			<ul style="margin-top: 1rem;">
							<li v-for='(item,index) in researchData.RDMaster' style="margin-top: 0.8rem;">
								<div class="card">
									<img :src="item.MasterHeaderPicUrl"  style="max-width: 6rem;margin-top: 0.5rem;display: block;float: left;max-height: 6rem;margin-left: 1rem;"/>
									<div style="display: block;float: left;width: 13rem;height: 7rem;">
										<li class="teacherIfo">
											<span class="lable">大师姓名:</span>
											<span class="value">{{item.MasterName}}</span>
										</li>
										<li class="teacherIfo">
											<span class="lable">岗位:</span>
											<span class="value">{{item.MasterStation}}</span>
										</li>
										<li class="teacherIfo">
											<span class="lable">酒店:</span>
											<span class="value">{{item.MasterHotal}}</span>
										</li>
									</div>
									<div style="display: block;float: right;width: 4rem;">
										<Button type="primary" @click='editTeacherBtn(item,index)' size="small" style="margin: 0.5rem auto;" >编辑</Button>
										<Button type="primary" @click='deletTeacherBtn(item)' size="small" >删除</Button>
										
									</div>
								</div>
							</li>
						</ul>
		        		</div>
		        	</div>
	        		<Button type="error" @click="saveResearchData"  style="display: block;margin: 0 auto;margin-top: 1rem;">保存</Button>
	        </TabPane>
	        <TabPane label="应用菜品" name='useIfo' :disabled='useIfoDisabled' class='useIfo tab'>
	        		<div class="applyDish">
	        			<div class="applyDish_top" style="overflow: auto;margin-bottom: 1rem;">
	        				<ul style="float:left;margin-left: 1rem;">
	        					<li class="applyDish_topLi">
	        						<span>菜品名</span>
	        						<Input v-model="applyDishSearch.dishName" placeholder="菜品名" style="width: 150px;margin-left: 0.5rem;"></Input>
	        					</li>
	        					
	        					<li class="applyDish_topLi">
	        						<span>状态</span>
	        						<Select v-model="applyDishSearch.IsDisplay" style="width:150px">
							        <Option value="0">全部</Option>
							        <Option value="true">显示</Option>
							        <Option value="false">隐藏</Option>
							    </Select>
	        					</li>
	        					<li class="applyDish_topLi">
	        						<Button type="primary" icon="ios-search" @click="getApplyDishList">搜索</Button>
	        					</li>
	        					
	        					<li class="applyDish_topLi">
	        						<Button type="primary" @click="addDishBtn">新增</Button>
	        					</li>
	        				</ul>
	        			</div>
	        			<Modal v-model="applyDishModal" width="1000" :styles="{top: '20px'}" :mask-closable="false">
				        <p slot="header" style="text-align:center">
				            <Icon type="information-circled"></Icon>
				            <span>应用菜品</span>
				        </p>
				        <div style="overflow:auto;height: 39rem;">
				           <div style="width: 31.5rem;padding-bottom: 0.5rem;margin-left: 2rem;float: left;margin-top: 1rem;height: 38rem;">
								<Upload
							        ref="upload"
							        :show-upload-list="false"
							        :on-success="dishPictureSuccess"
							        :format="['jpg','jpeg','png']"
							        :max-size="2048"
							        :before-upload='handleBeforeUpload'
							        :on-format-error="handleFormatError"
							        :on-exceeded-size="handleMaxSize"
							        :multiple=false
							        type="drag"
							        :action=UPImage
							        style="margin-left: 4.5rem;width:9rem;padding-bottom: 1rem;" >
							        <img :src="dishInformation.DishPicUrl" style="width: 9rem;"/>
							    </Upload>
							    <div style="">
							   	 	<span>菜品名称:</span>
							   		 <Input v-model="dishInformation.DishName" placeholder="菜品名" style="width: 160px;margin-left: 1rem;"></Input>
							    </div>
							    <!--<div style="margin: 0.5rem 0;">
							   	 	<span>排序:</span>
							   		 <Input v-model="dishInformation.dishSort" placeholder="排序" style="width: 160px;margin-left: 2.5rem;"></Input>
							    </div>-->
							    <div style="margin-top: 1rem;"> 
							    		<span>是否显示</span>
							    		
							    		<Checkbox v-model="dishInformation.IsDisplay" style='color: red;font-weight:bold;margin-left: 1.2rem;'>显示</Checkbox>
							    </div>
								<div style="width: 32rem;margin: 0 auto;margin-top: 1rem;overflow: auto;padding-bottom: 1rem;">
									<span style="float: left;display: block;margin-right: 1rem;">菜品故事</span>
									<div class="rtf-content" style="width: 27rem;display: block;margin-left: 4.5rem;height: 23rem;">
								        <quill-editor ref="myTextEditor" v-model="dishInformation.DishStory" :config="editorOption_basicInf" style='height: 18rem;'></quill-editor>
								    </div>
								</div>
							</div>
							<div style="float: left;margin-left: 1rem;padding-left: 2rem;">
								<div class="Main_ingredient" style="margin: 0 auto;text-align: center;">
									<h3 style="text-align: center;margin-top: 1rem;">主料</h3>
									<ul>
										<li v-for='(item,index) in Main_ingredientList' style="margin-top: 0.5rem;">
											<span>{{index+1}}</span>
											<Input v-model='item.Material' placeholder="主料" style="width: 140px;"></Input>
											<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
											<!--<Input v-model='item.weight' placeholder="重量" style="width: 80px;"></Input>-->
											<a @click='removeMain(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
										</li>
										<li>
											<Button type="primary" @click="addMainingredient" style="margin-top:0.5rem;">新增主料</Button>
										</li>
									</ul>
								</div>
								<div class="Accessories" style="text-align: center;">
									<h3 style="text-align: center;margin-top: 1rem;">辅料</h3>
									<ul>
										<li v-for='(item,index) in Accessories_List' style="margin-top: 0.5rem;">
											<span>{{index+1}}</span>
											<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
											<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
											<a @click='removeAccessories(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
										</li>
										<li>
											<Button type="primary" @click="addAccessories" style="margin-top:0.5rem;">新增辅料</Button>
										</li>
									</ul>
								</div>
								<div class="seasoning">
									<h3 style="text-align: center;margin-top: 1rem;">推荐调料</h3>
									<ul>
										<li v-for='(item,index) in RMseasoningList' style="width: 15rem;margin: 0 auto;margin-top: 0.5rem;">
											<span>{{index+1}}</span>
											<!--<Input v-model='item.Material' placeholder="调料" style="width: 140px;"></Input>-->
											<Select v-model="item.FlavourRecId"  style="width:140px;">
										        <Option v-for="item in seasoningAllList" :value="item.FlavourRecId" :key="item.FlavourRecId"	>{{ item.ProductName }}</Option>
										    </Select>
											<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
											<a @click='removeRMseasoning(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
										</li>
										<li style="margin-top: 0.5rem;width: 15rem;margin: 0 auto;margin-top: 0.5rem;text-align: center;">
											<Button type="primary" @click="addRMseasoning" style="margin-top:0.5rem;">新增推荐调料</Button>
										</li>
									</ul>
								</div>
								<div class="Accessories" style="text-align: center;">
									<h3 style="text-align: center;margin-top: 1rem;">调料</h3>
									<ul>
										<li v-for='(item,index) in seasoning_List' style="margin-top: 0.5rem;">
											<span>{{index+1}}</span>
											<Input v-model='item.Material' placeholder="食材" style="width: 140px;"></Input>
											<Input-number :max="9999" :min="0" v-model='item.Unit' style="width: 70px;"></Input-number>
											<a @click='removeSeasoning(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
										</li>
										<li>
											<Button type="primary" @click="addSeasoning" style="margin-top:0.5rem;">新增调料</Button>
										</li>
									</ul>
								</div>
								<div class="Step" style="text-align: center;">
									<h3 style="text-align: center;margin-top: 1rem;">烹饪步骤</h3>
									<ul style="padding-left: 1rem;">
										<li v-for='(item,index) in step' style="margin-top: 0.5rem;">
											<span>{{index+1}}</span>
											<Input v-model='item.StepName' placeholder="请输入" type="textarea" style="width: 210px;"></Input>
											<a @click='removeStep(item)' style="cursor: pointer;"> <Icon type="close-circled" ></Icon></a>
										</li>
											<Button type="primary" @click="addStep" style="margin-top:0.5rem;">新增步骤</Button>
									</ul>
								</div>
							</div>
				        </div>
				        <div slot="footer">
				        		<Button type="primary" @click="confirmDishBtn">确认</Button>
				            <Button type="error" @click="cancelDishBtn">取消</Button>
				        </div>
				    </Modal>
					<Table :columns="columns" :data="useDishList"></Table>
					<Page :total="useTotal" :current='usePageIndex' show-total  :page-size='10' style='float: right;margin-top: 1rem;margin-right: 1rem;margin-bottom: 1rem;' @on-change='useIfochangePage'></Page>
				</div>
	        </TabPane>
	    </Tabs>
	</div>
</template>

<script>
//import editor from '../../../tools/Quilleditor.vue'
 import { quillEditor } from 'vue-quill-editor';//编写框
	export default{
		data(){
			return{
				tabName:'productIfo',//tab的name
				ISAdd:true,//新增模式
				UPImage:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Produce',//图片上传地址
				editorOption_basicInf:{},
				ProductId:'',//0 表示新增  大于零表示修改产品信息
				//canCrop:false,//剪切
				//================================产品信息===tabName================================
				addSize_Modal:false,//添加规格弹框
				productIfoData:{
					BrandName:'',//品牌名称
					CookingSkill:'',//烹饪技法 (富文本)
					CreatePerson:'',//创始人
					ProductBasicInfo:'',//基本信息 (富文本)
					ProductFeature:'',//产品特点 (富文本)
					ProductFirstId:'',//分类1
					productName:'',//产品名
					ProductPicURL:'../../../../../static/image/HeadPortrait.png',//上传图片
					ProductSecondId:'',//分类2
					ProductValue:'',//产品价值 (富文本)
					UpdatePerson:'',//修改人
	
				},
				sizeList:[
//					{
//						name:'',
//						unit:''
//					},
//					{
//						SpecificationId:'0',//0为新增
//					}
				],
				oneTypeList:[//分类一
				],
				twoTypeList:[//分类二
				],
				brandNameList:[//拼盘
					{
                        value: '味达美',
                        label: '味达美'
                    },
                    {
                        value: '六月鲜',
                        label: '六月鲜'
                    },
                    {
                        value: '葱伴侣',
                        label: '葱伴侣'
                    },
                     {
                        value: '黄飞红',
                        label: '黄飞红'
                    },
                    {
                        value: '禾然有机',
                        label: '禾然有机'
                    },
                    {
                        value: '遵循自然',
                        label: '遵循自然'
                    },
                    {
                        value: '有所思',
                        label: '有所思'
                    },
                    {
                        value: '竹笙',
                        label: '竹笙'
                    },
                    {
                        value: '醯官醋',
                        label: '醯官醋'
                    },
				],
				unitList:[
					{
                        value: 'g',
                        label: 'g'
                   },
                    {
                        value: 'kg',
                        label: 'kg'
                    },
                    {
                        value: 'ml',
                        label: 'ml'
                    },
                    {
                        value: 'L',
                        label: 'L'
                    },
				],
				//================================规格===Size================================
				SizeDisabled:false,//规格页禁用
				changeImgIndex:0,//点击图片按钮 确认选的第几个()
				changeIfIoIndex:0,//点击配置信息按钮
				areaData:[],//省市数据 
				sizeTopList:[//规格头像信息
				],
				selectSizeTitle:'',//点击配置规格后显示的规格名
				sizeInformation:{
					SpecificationConfId:'0',//为0表示新增
					SpecificationId:'0',//规格id
					MaterialNum:'',//物料编码
					Introduce:'',//新品介绍
					CreatePerson:sessionStorage.getItem('loginkey'),//创建人
					UpdatePerson:sessionStorage.getItem('loginkey'),//更新人
				},
				typeList:[
				],
				SpecificationConfArea:[],//省
//				getSizeList:[//规格 数据
//					{
//						image:'../../../../../static/image/HeadPortrait.png',
//						size:'100g',
//						ifo:{
//							materialCode:'',//物料编码
//							area:[],//销售区域
//							introduce:'',//新品介绍
//							typeList:[
//								{
//									isSelect:false,//是否选中
//									typeTitle:'新品上架',
//									sort:'',//排除
//									isShow:false,//显示
//								},
//								{
//									isSelect:false,
//									typeTitle:'经典爆款',
//									sort:'',
//									isShow:false,//显示
//								},
//								{
//									isSelect:false,
//									typeTitle:'EC新品',
//									sort:'',
//									isShow:false,//显示
//								},
//								{
//									isSelect:false,
//									typeTitle:'灵感秀场',
//									sort:'',
//									isShow:false,//显示
//								},
//							],
//						},
//					},
//				],
				
				//================================研发信息===researchIfo================================
				researchIfoDisabled:false,//研发页禁用
				addTeacherModel:false,//新增导师 弹框
				isAddTeacher:true,//大师新增
				selectTeacherIndex:0,//点击编辑的 第几位老师
				teacherDetal:{
					RDMasterId:'',//大师id
					RDId:'',//研发整体id
					MasterHeaderPicUrl:'',//图片
					MasterName:'',//姓名
					MasterStation:'',//岗位
					MasterHotal:'',//酒店
//					HeadPicUrl:'../../../../../static/image/HeadPortrait.png',//图片
//					LecturerName:'诸葛亮',//名字
//					Post:'行政总厨',//岗位
//					HotelName:'荆州国际大酒店',//酒店名
				},
				researchData:{
					RDId:'',//研发整体id
					IsDisPlay:false,//是否启用
					RDStory:'',//研发故事
					RDMaster:[//研发导师
//						{
//							HeadPicUrl:'../../../../../static/image/HeadPortrait.png',//图片
//							LecturerName:'诸葛亮',//名字
//							Post:'行政总厨',//岗位
//							HotelName:'荆州国际大酒店',//酒店名
//						},
					]
				},
				//================================应用菜品===useIfo================================
				useIfoDisabled:false,//应用菜品禁用
				useTotal:0,//总条数
				usePageIndex:1,//当前页数
				applyDishSearch:{//应用菜搜索条件
					dishName:'',//
					IsDisplay:'',//状态
				},
				dishAdd:true,//菜品新增
				applyDishModal:false,//菜品新增编辑
				dishInformation:{//菜品信息
					DishProductId:'0',//菜品ID
					dishSort:'',
					DishStory:'',//菜品故事
					DishPicUrl:'../../../../../static/image/HeadPortrait.png',//菜品图
					DishName:'',//菜品名
					IsDisPlay:true,//是否显示 是
					CreatePerson:sessionStorage.getItem('loginkey'),//创始人
					UpdatePerson:sessionStorage.getItem('loginkey'),//修改人
				},
				Main_ingredientList:[//主料数据
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'1'
					}
				],
				Accessories_List:[//辅料
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'2'
					}
				],
				seasoning_List:[//调料
					{	
						Material:'',
	         			Unit:0,
	         			MaterialType:'3'
	         		}
				],
				RMseasoningList:[//推荐调料数据
					{	
						FlavourRecId:'',
	         			Unit:0,
	         		}
				],
				step:[//步骤
					{
						StepName:''
					}
				],
				//---------
				seasoningAllList:[//调料 推荐数据源
				],
				useDishList:[],//应用菜列表
				columns:[
					{
                        type: 'selection',
                        align: 'center',
                        width:60,
                    },
					{
					   title:'No.',
                        type: 'index',
                        align: 'center',
                    },
                    {
						title:'菜品名称',
						key:'DishName',
                         align: 'center',
                    },
                    {
						title:'上传时间',
						key:'CreateTime',
                         align: 'center',
                    },
                    {
						title:'状态',
						key:'IsDisplayStr',
                         align: 'center',
                    },
                    {
                        title: '操作',
                        align: 'center',
                        width:100,
                        key: 'operation',
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'primary',
                                        size: 'small',
                                    },
                                    on: {
                                        click: () => {
                                            this.dishEditorBtn(params)
                                        }
                                    }
                                }, '编辑'),
                            ]);
                        }
                     }
				],
			}
		},
		components: {
            //editor
             quillEditor
     	},
     	watch: {
		    'ProductId' () {//产品id
				if (this.ProductId>0) {
					console.log('编辑')
					this.tabName = 'productIfo'
					this.useIfoDisabled= false//应用菜
					this.researchIfoDisabled= false//研发信息
					this.SizeDisabled= false//规格
				} else{//
					console.log('新增')
					this.useIfoDisabled= true//应用菜
					this.researchIfoDisabled= true//研发信息
					this.SizeDisabled= true//规格
					
				}
		     },
	   },
		methods:{
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
                    desc: '文件 ' + file.name + ' 太大，不能超过 2M。'
                });
            },
            handleBeforeUpload () {//选择图片
          		$('.loding').show()
//              return check;
         	},
			tabClickChang(name){//页签切换
				console.log(name)
//				this.getProductIfoData()//获取产品信息
//				//=====规格=====
//				this.getSizeTopList()
//				//=====研发菜品=====
//				this.getResearchIfo()//获取研发信息
//				//=====应用菜=====
//				this.getApplyDishList()//应用菜品 列表
				if (name=='Size') {//规格
					this.getVirtualList()//获取虚拟
					this.getSizeTopList()//获取规格信息
					$('.Information').hide()//隐藏虚拟信息
					//console.log(1)
				}else if (name=='productIfo') {//产品信息
					this.getProductIfoData()//获取产品信息
				}else if (name =='researchIfo') {//研发信息
					this.getResearchIfo()//获取研发信息
				}else if(name=='useIfo'){
					this.getApplyDishList()//应用菜品 列表
					this.getGetSDish()//推荐调料
				}
			},
			//======================================产品信息======================================
			productSuccess (res, file) {//上传头像成功
                $('.loding').hide()
                console.log(file)
                var data = JSON.parse(res);
                this.productIfoData.ProductPicURL = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
            },
            oneSelectChange(name){//类型别选择
            		//console.log(name)
            		var self =this
            		$.ajax({//一级分类
			        type :"get",
			        url :URLHeader.ECActivities+'/api/Product/GetProductFirstAll',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			       	 	var dataAll =JSON.parse(json)
			       	 	
			        		self.oneTypeList = dataAll
			        		$('.loding').hide()
			        		//console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    var DATA ='{"ProductFirstId":"'+name+'"}'
	   		    if (name!=undefined) {
	   		    		$.ajax({//二级分类
				        type :"post",
				        url :URLHeader.ECActivities+'/api/Product/GetProductSecond',//
				        dataType : 'json',
				        contentType: "application/json; charset=utf-8",				
				        data:DATA,
				        success : function(json) {
				        	//dateTime
				        	  	var dataAll =JSON.parse(json)
				        	 	self.twoTypeList = dataAll
				        	 	 //console.log(dataAll)
				        },
				        error : function(error) { 
				        		console.log(error)
				        }
		   		    });
	   		    }else{
	   		    		self.productIfoData.ProductSecondId = ''//清空二级分类数据
			       	self.twoTypeList= []
	   		    }
            },
			createSizeBtn(){//创建规格按钮
				this.addSize_Modal = true
			},
			addSIzeBtn(){//本地增加规格
//				var item = {
//	         		name:'',
//	         		unit:'',
//	         	}
				var item = {
					"SpecificationId": 0,
					"ProductId": 0,
					"Amount": '',
					"Unit": "",
					"CreatePerson": sessionStorage.getItem('loginkey'),
					"UpdatePerson": sessionStorage.getItem('loginkey'),
					}
	         	this.sizeList.push(item);
			},
			removeSizeBtn(item){//删除规格
				this.sizeList.splice(this.sizeList.indexOf(item), 1)
			},
			getProductIfoData(){//获取产品信息
				var self = this
				var DATA = '{"ProductId":"'+this.ProductId+'"}'
				$('.loding').show()
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetPKIOne',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		//console.log(json)
			        		if (json=='No') {//洗澡能
//			        			self.$Message.error({
//				                content: '产品信息获取错误',
//				                duration: 10
//				            });
							self.productIfoData = {
								BrandName:'',//品牌名称
								CookingSkill:'',//烹饪技法 (富文本)
								CreatePerson:'',//创始人
								ProductBasicInfo:'',//基本信息 (富文本)
								ProductFeature:'',//产品特点 (富文本)
								ProductFirstId:'',//分类1
								productName:'',//产品名
								ProductPicURL:'../../../../../static/image/HeadPortrait.png',//上传图片
								ProductSecondId:'',//分类2
								ProductValue:'',//产品价值 (富文本)
								UpdatePerson:'',//修改人
							}
							self.sizeList = []
			        		} else{
			        			//self.$Message.success('产品信息保存成功');
			        			//productIfoData sizeList
			        			var dataAll =JSON.parse(json)
			        			self.productIfoData = dataAll.ProductKuInfo//
			        			self.sizeList = dataAll.Specification
//			        			console.log(dataAll.ProductKuInfo)
//			        			console.log(self.productIfoData)
							
			        		}
			        	  	$('.loding').hide()
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '产品信息获取失败,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			saveBtn(){//保存
				$('.loding').show()
				var self = this
				self.productIfoData.CreatePerson = sessionStorage.getItem('loginkey')
				self.productIfoData.UpdatePerson = sessionStorage.getItem('loginkey')
//				console.log(self.productIfoData)
//				console.log(self.sizeList)
				var Specification = JSON.stringify(self.sizeList)//新增规格
				self.productIfoData.ProductBasicInfo = self.productIfoData.ProductBasicInfo.replace(/\"/g,"'");//把双引号改为单引号(基本信息)
				self.productIfoData.ProductFeature = self.productIfoData.ProductFeature.replace(/\"/g,"'");//把双引号改为单引号(产品特点)
				self.productIfoData.ProductValue = self.productIfoData.ProductValue.replace(/\"/g,"'");//把双引号改为单引号(产品价值)
				self.productIfoData.CookingSkill = self.productIfoData.CookingSkill.replace(/\"/g,"'");//把双引号改为单引号(烹饪技法)
				var Ifo = self.productIfoData
				
				var DATA = '{"ProductId":"'+self.ProductId+'","ProductName":"'+Ifo.ProductName+'","ProductFirstId":'+Ifo.ProductFirstId+',"ProductSecondId":'+Ifo.ProductSecondId+',"BrandName":"'+Ifo.BrandName+'","ProductBasicInfo":"'+Ifo.ProductBasicInfo+'","ProductFeature":"'+Ifo.ProductFeature+'","ProductPicURL":"'+Ifo.ProductPicURL+'","ProductValue":"'+Ifo.ProductValue+'","CookingSkill":"'+Ifo.CookingSkill+'","CreatePerson":"'+Ifo.CreatePerson+'","UpdatePerson":"'+Ifo.UpdatePerson+'","Specification":'+Specification+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/AddProductKuInfo',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		if (json=='No') {
			        			self.$Message.error({
				                content: '产品信息数据保存错误',
				                duration: 10
				            });
			        		} else{
			        			$('.loding').hide()
			        			self.$Message.success('产品信息保存成功');
			        			var dataAll =JSON.parse(json)
			        			self.ProductId = dataAll.ProductId
			        			//=====产品信息=====
							self.getProductIfoData()//获取产品信息
			        		}
			        	  	console.log(json)
			        	  	$('.loding').hide()
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '产品信息保存失败,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			//======================================规格======================================
			sizeImgSuccess(res, file){
				//$('.loding').hide()
               // console.log(file)
               	var self = this
               	var data = JSON.parse(res);
             	this.sizeTopList[this.changeImgIndex].SpecificationPicUrl = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
             	 //sizeTopList
             	 var topOne = this.sizeTopList[this.changeImgIndex]
             	var DATA = ' {"SpecificationId":"'+topOne.SpecificationId+'","SpecificationPicUrl":"'+topOne.SpecificationPicUrl+'","UpdatePerson":"'+sessionStorage.getItem('loginkey')+'"}' 
              	$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/UpdateSPPic',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		if (json=='No') {
			        			self.$Message.error({
				                content: '图片据保存错误',
				                duration: 10
				            });
			        		} else{
			        			$('.loding').hide()
			        			self.$Message.success('图片据保存成功');
			        		}
			        	  	//console.log(json)
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '图片据保存错误,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			changeImg(index){//记录 修改的是第几个 图片照片
				this.changeImgIndex = index
				console.log(this.changeImgIndex)
			},
			cleanIfo(){//清空配置信息
				this.sizeInformation = {
					SpecificationConfId:'0',//为0表示新增
					SpecificationId:'0',//规格id
					MaterialNum:'',//物料编码
					Introduce:'',//新品介绍
					CreatePerson:'',//创建人
					UpdatePerson:'',//更新人
				}
				this.typeList = []
				this.getVirtualList()//重新加载虚拟数据
				this.SpecificationConfArea = []
				$('.Introduce').hide()
//				this.typeList.map((item) =>{
//					item.IsDisplay = false,//显示不勾选
//					item.PriorityId = ''//排序
//				})
			},
			changeIfo(index){//点击 配置信息
				this.changeIfIoIndex = index
				var self= this
				self.cleanIfo()
				var DATA = '{"SpecificationId":"'+this.sizeTopList[index].SpecificationId+'"}'
				self.selectSizeTitle = this.sizeTopList[index].Amount+this.sizeTopList[index].Unit
				//console.log(DATA)
				
				$('.Information').hide()
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetSpecificationConf',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		self.sizeInformation.SpecificationId = self.sizeTopList[index].SpecificationId
			        		if (json==-100) {//新增
							//sizeTopList
							self.sizeInformation.SpecificationConfId = '0'
			        		} else{//有数据
			        			$('.loding').hide()
			        			var dataAll =JSON.parse(json)
			        			console.log(dataAll)
			//				sizeInformation:{
			//					SpecificationConfId:'0',//为0表示新增
			//					SpecificationId:'0',//规格id
			//					MaterialNum:'',//物料编码
			//					Introduce:'',//新品介绍
			//					CreatePerson:'',//创建人
			//					UpdatePerson:'',//更新人
			//				},
			//				typeList:[
			//				],
			//				SpecificationConfArea:[],//省
							self.sizeInformation = dataAll.SConfInfo//基本信息
							self.sizeInformation.UpdatePerson = sessionStorage.getItem('loginkey')//跟新人员
							//省
							self.SpecificationConfArea= []
							dataAll.SConfAreaInfo.map((item) =>{
								self.SpecificationConfArea.push(item.ProvinceId)
							})
							//虚拟信息
							dataAll.SConfVCInfo.map((one) =>{
								self.typeList.map((two) =>{
									if (one.VCId==two.VCId) {
										two.IsDisplay = true
										two.PriorityId = one.PriorityId
										if(two.IsIntroduce==1 && two.VCName=='EC新品'){
											$('.Introduce').show()
										}
									}
								})
							})
			        		}
			        	  	
			        	  	$('.Information').fadeIn()
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '配置信息加载错误,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			 getVirtualList(){//虚拟数据列表
				//Product   GetVCList
				var self = this 
				$.ajax({
			        type :"get",
			        async:false,
			        url :URLHeader.ECActivities+'/api/Product/GetVCList',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        success : function(json) {
			        		//console.log(json)
						var dataAll =JSON.parse(json)
						self.typeList = []
						dataAll.map((item) =>{
							item.IsDisplay = false,//显示不勾选
							item.PriorityId = ''//排序
							delete item.CreateTime
							delete item.UpdateTime
							self.typeList.push(item)
						})
			        	 	//console.log(dataAll)
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '虚拟分类数据获取失败,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			checkBoxChange(name){//点击显示触发
				console.log(name)
				if (name.VCName=='EC新品' && name.IsDisplay) {//选中EC新品  新品介绍显示
					$('.Introduce').show()
				}
				
				if (name.VCName=='EC新品' && !name.IsDisplay) {//取消EC新品  新品介绍隐藏 且数据清空
					$('.Introduce').hide()
					this.sizeInformation.Introduce = ''
				}
			},
			getSizeTopList(){//获取头部信息列表(图片列表)
				var self =this
				$('.loding').show()
				var DATA = '{"ProductId":"'+self.ProductId+'"}'
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetSpecificationOnly',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		if (json==-100) {
			        			self.$Message.error({
				                content: '暂无规格数据',
				                duration: 10
				            });
				            self.sizeTopList = []
			        		} else{
			        			self.sizeTopList= []
							var dataAll =JSON.parse(json)
							$('.loding').hide()
							dataAll.map((item) =>{
								if (item.SpecificationPicUrl==null) {
									item.SpecificationPicUrl = '../../../../../static/image/HeadPortrait.png'
								}
								self.sizeTopList.push(item)
							})
							self.sizeTopList = dataAll
							//console.log(dataAll)
			        		}
						//console.log(json)
			        	 	
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '规格数据获取失败,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			saveSizeData(){//规格保存
				//SpecificationConfArea sizeInformation  typeList
				//this.sizeTopList[this.changeIfIoIndex].SpecificationId
				var self =this
//				sizeInformation:{
//					SpecificationConfId:'0',//为0表示新增
//					SpecificationId:'0',//规格id
//					MaterialNum:'',//物料编码
//					Introduce:'',//新品介绍
//					CreatePerson:'',//创建人
//					UpdatePerson:'',//更新人
//				},
//				typeList:[
//				],
//				SpecificationConfArea:[],//省
				$('.loding').show()
				var one = self.sizeInformation
				//虚拟
				var virtualArr= []
				self.typeList.map((item) =>{
					if(item.IsDisplay){
						var p = {
							VCId:item.VCId,
							PriorityId:item.PriorityId,
							IsDisplay:item.IsDisplay.toString()
						}
						virtualArr.push(p)
					}
				})
				var SpecificationConfVC = JSON.stringify(virtualArr)
				//console.log(SpecificationConfVC)
				//省
				var areaArr= []
				$('.SpecificationConfArea .ivu-select-selection .ivu-tag .ivu-tag-text').each(function(index,element){
					//console.log($(element).text())
					var item = {
						ProvinceId:self.SpecificationConfArea[index],
						ProvinceName:$(element).text()
					}
					areaArr.push(item)
					
				})
				var SpecificationConfArea= JSON.stringify(areaArr)
				//console.log(areaArr)
				//console.log(self.SpecificationConfArea)
				var DATA = '{"SpecificationConfId":"'+one.SpecificationConfId+'","SpecificationId":"'+one.SpecificationId+'","MaterialNum":"'+one.MaterialNum+'","Introduce":"'+one.Introduce+'","CreatePerson":"'+one.CreatePerson+'","UpdatePerson":"'+one.UpdatePerson+'","SpecificationConfVC":'+SpecificationConfVC+',"SpecificationConfArea":'+SpecificationConfArea+'}'
				console.log(DATA)
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/AddSpecificationConf',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		console.log(json)
			        		if (json=='OK') {
			        			self.changeIfo(self.changeIfIoIndex)
			        			self.$Message.success({
				                content: '规格保存成功',
				                duration: 5
					        });
					        $('.loding').hide()
			        		} else{
			        			self.$Message.error({
			                content: '规格保存错误',
			                duration: 10
				        });
			        		}
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '规格保存错误,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
			},
			//======================================研发信息======================================
			handleTeacherImgSuccess(res, file){
				$('.loding').hide()
               // console.log(file)
                	var data = JSON.parse(res);
             	this.teacherDetal.MasterHeaderPicUrl = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
			},
			addTeacherBtn(){//新增导师按钮
				this.teacherDetal = {
					RDMasterId:'0',//大师id
					RDId:this.researchData.RDId,//研发整体id
					MasterHeaderPicUrl:'../../../../../static/image/HeadPortrait.png',//图片
					MasterName:'',//姓名
					MasterStation:'',//岗位
					MasterHotal:'',//酒店
				}
				this.isAddTeacher = true//大师新增
				this.addTeacherModel = true//新增弹框显示
			},
			confirmBtn(){//确认 新增&编辑
				var item;
            		var self =this;
//          		var MasterHeaderPicUrl= ''
            		if (this.teacherDetal.MasterHeaderPicUrl=="../../../../../static/image/HeadPortrait.png") {
            			this.teacherDetal.MasterHeaderPicUrl=''
            		}
//          		item = {
//          			LecturerName:this.teacherDetal.LecturerName,
//					//Post:this.modelInformation.Post.toString(),
//					Post:this.teacherDetal.Post,
//					HotelName:this.teacherDetal.HotelName,
//					HeadPicUrl:HeadPicUrl
//          		}
            		console.log(this.teacherDetal)
            		if (this.isAddTeacher) {//新增
            			this.researchData.RDMaster.push(self.teacherDetal)
            		} else{//编辑
            			self.researchData.RDMaster[self.selectTeacherIndex] = this.teacherDetal
            		}
            		//console.log(this.researchData.RDMaster)
			},
			editTeacherBtn(item,index){//编辑按钮
				this.isAddTeacher = false//大师新增
				this.selectTeacherIndex = index;//编辑是用
				this.teacherDetal = {
					RDMasterId:item.RDMasterId,//大师id
					RDId:item.RDId,//研发整体id
					MasterHeaderPicUrl:item.MasterHeaderPicUrl,//图片
					MasterName:item.MasterName,//姓名
					MasterStation:item.MasterStation,//岗位
					MasterHotal:item.MasterHotal,//酒店
				}
				this.addTeacherModel=true
            		//console.log(item)
            },
             deletTeacherBtn(item){//删除
           	 	this.researchData.RDMaster.splice(this.researchData.RDMaster.indexOf(item), 1)
            },
            getResearchIfo(){//获取研发信息数据
            		var self =this
            		var DATA = '{"ProductId":"'+this.ProductId+'"}'
            		 $('.loding').show()
            		$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetProductIdKuInfoRD',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		//console.log(json)
			        		if (json=='-100') {//无数据  为新增
			        			self.researchData = {
								RDId:'0',//研发整体id
								IsDisPlay:false,//是否启用
								RDStory:'',//研发故事
								RDMaster:[]//研发导师
							}
					        $('.loding').hide()
			        		} else{//编辑
			        			var dataAll =JSON.parse(json)
			        			//self.researchData
			        			if (dataAll.RDInfo.IsDisPlay==1) {//选中
			        				dataAll.RDInfo.IsDisPlay = true
			        			} else{//非选中 
			        				dataAll.RDInfo.IsDisPlay = false
			        			}
			        			self.researchData = {
								RDId:dataAll.RDInfo.RDId,//研发整体id
								IsDisPlay:dataAll.RDInfo.IsDisPlay,//是否启用
								RDStory:dataAll.RDInfo.RDStory,//研发故事
								RDMaster:dataAll.RDMaster//研发导师
							}
			        			//console.log(dataAll)
			        			$('.loding').hide()
			        		}
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '规格保存错误,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
            },
            saveResearchData(){//保存研发信息
            	 	$('.loding').show()
            		var self = this
            		self.researchData.RDStory = self.researchData.RDStory.replace(/\"/g,"'");//把双引号改为单引号(研发故事)
            		var researchData = this.researchData
            		var RDMaster= JSON.stringify(researchData.RDMaster)
            		var IsDisPlay = ''
            		if (researchData.IsDisPlay) {//勾选
            			IsDisPlay = '1'
            		} else{//不勾选
            			IsDisPlay = '0'
            		}
            		
            		var DATA = '{"RDId":"'+researchData.RDId+'","ProductId":"'+self.ProductId+'","RDStory":"'+researchData.RDStory+'","IsDisPlay":"'+IsDisPlay+'","RDMaster":'+RDMaster+'}'
            		console.log(DATA)
            		console.log(researchData)
            		$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/AddProductIdKuInfoRD',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        		console.log(json)
			        		if (json!='No') {//无数据  为新增
			        			var dataAll =JSON.parse(json).RDId
			        			self.researchData.RDId = dataAll
			        			self.getResearchIfo()//刷新保存好的数据
			        			self.$Message.success({
				                content: '研发信息保存成功',
				                duration: 3
					        });
					        
					        $('.loding').hide()
			        		} else{//编辑
			        			self.$Message.error({
				                content: '研发信息保存错误,接口错误',
				                duration: 10
					        });
			        		}
			        },
			        error : function(error) { 
			        		self.$Message.error({
			                content: '研发信息保存错误,接口错误',
			                duration: 10
				        });
			        		console.log(error)
			        }
	   		    });
            },
            //======================================应用菜品======================================
             dishPictureSuccess(res, file){//图片上传成功 返回
				$('.loding').hide()
               // console.log(file)
                	var data = JSON.parse(res);
             	this.dishInformation.DishPicUrl = URLHeader.Loading+'/Produce/'+data.Id+'.'+data.Type;
			},
            addDishBtn(){//新增按钮
            		this.cleanDishData()
            		this.dishAdd= true//新增模式
            		this.applyDishModal = true
            },
            dishEditorBtn(name){// 菜品编辑
            		this.cleanDishData()//清除数据
            		this.dishAdd= false//编辑模式
            		var self = this
            		var DATA = '{"DishProductId":"'+name.row.DishProductId+'"}'
            		//console.log(name)
            		//dishInformation//基本   Main_ingredientList主料  Accessories_List辅料  seasoning_List调料  RMseasoningList推荐调料  步骤
            		$.ajax({//菜品详细信息
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetDishProduct',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  	var dataAll =JSON.parse(json)
			        	  	//基本信息
			        	  	//Boolean(num0)
			        	  	dataAll.DishProduct.IsDisplay = Boolean(dataAll.DishProduct.IsDisplay)
			        	  	self.dishInformation = dataAll.DishProduct
			        	  	//主料,辅料,调料
			        	  	self.Main_ingredientList = []//主料
			        	  	self.Accessories_List = []//辅料
			        	  	self.seasoning_List = []//调料
			        	  	
			        	  	dataAll.DishMaterial.map((item) =>{
			        	  		//主料
			        	  		item.Unit = parseInt(item.Unit)
			        	  		if (item.MaterialType==1) {//主料
			        	  			self.Main_ingredientList.push(item)
			        	  		}
			        	  		//辅料
			        	  		if (item.MaterialType==2) {//辅料
			        	  			self.Accessories_List.push(item)
			        	  		}
			        	  		//调料
			        	  		if (item.MaterialType==3) {//
			        	  			self.seasoning_List.push(item)
			        	  		}
			        	  	})
			        	  	//推荐调料
			        	  	self.RMseasoningList = []//推荐调料
			        	  	dataAll.DishProductRF.map((item) =>{
			        	  		//item.Unit = parseInt(item.Unit)
			        	  		var one = {
			        	  			FlavourRecId:item.FlavourRecId,
			        	  			Unit:parseInt(item.Unit)
			        	  		}
			        	  		self.RMseasoningList.push(one)
			        	  	})
			        	  	//步骤
			        	  	self.step = []//步骤
			        	  	self.step = dataAll.DishStep
			        	 	console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
	   		    this.applyDishModal = true
            },
            useIfochangePage(index){//分页切换
            		this.usePageIndex = index
            		this.getApplyDishList()
            },
            getApplyDishList(){//获取应用菜列表
            		var self =this
				$('.loding').show()
				self.useDishList = []
//				applyDishSearch:{//应用菜搜索条件
//					dishName:'',//
//					IsDisplay:'',//状态
//				},
				var Search = this.applyDishSearch
				var DATA = '{"ProductId":'+self.ProductId+',"DishName":"'+Search.dishName+'","IsDisplay":"'+Search.IsDisplay+'","PageIndex":'+this.usePageIndex+'}'
				//var DATA = '{"IsApply":-1,"BeginTime":"","EndTime":"","Name":"","PageIndex":'+this.usePageIndex+'}'
            		$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetRDDishProductList',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  	var dataAll =JSON.parse(json)
			        	  	self.useTotal = parseInt(dataAll.Count)
			        	  	//self.useDishList = dataAll.Info
			        	  	dataAll.Info.map((item) =>{
			        	  		if (item.IsDisplay=='true') {//显示
			        	  			item.IsDisplayStr = '显示'
			        	  		} else{
			        	  			item.IsDisplayStr = '隐藏'
			        	  		}
			        	  		self.useDishList.push(item)
			        	  	})
			        	  	$('.loding').hide()
			         	console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
            },
			removeMain(item){//主料删除
	         	this.Main_ingredientList.splice(this.Main_ingredientList.indexOf(item), 1)
	         },
	         addMainingredient(){//新加主料
	         	var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'1'
	         	}
	         	this.Main_ingredientList.push(item);
//	         	console.log(this.Main_ingredientList)
	         },
	          removeAccessories(item){//辅料删除   Accessories_List
	         	this.Accessories_List.splice(this.Accessories_List.indexOf(item), 1)
			},
			addAccessories(){//辅料新增
				var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'2'
	         	}
	         	this.Accessories_List.push(item);
			},
			
			removeSeasoning(item){//调料删除
				this.seasoning_List.splice(this.seasoning_List.indexOf(item), 1)
			},
			addSeasoning(){//调料新增
				var item = {
	         		Material:'',
	         		Unit:0,
	         		MaterialType:'3'
	         	}
	         	this.seasoning_List.push(item);
			},
			removeRMseasoning(item){//推荐调料删除
				this.RMseasoningList.splice(this.RMseasoningList.indexOf(item), 1)
			},
			
			addRMseasoning(){//推荐调料新增
				var item = {
	         		FlavourRecId:'',
	         		Unit:0,
//	         		MaterialType:'3'
	         	}
	         	this.RMseasoningList.push(item);
			},
			removeStep(item){//删除步骤
				this.step.splice(this.step.indexOf(item), 1)
			},
			addStep(){//新增步骤
				var item = {
	         		StepName:'',
	         	}
	         	this.step.push(item);
			},
			cleanDishData(){//清除 菜品弹框数据
//				this.dishInformation = {//菜品信息
//					DishPicUrl:'../../../../../static/image/HeadPortrait.png',
//					DishName:'',//菜品名
//					DishStory:'',//菜品故事
//				}
				this.dishInformation = {//菜品信息
					DishProductId:'0',//菜品ID
					DishStory:'',//菜品故事
					DishPicUrl:'../../../../../static/image/HeadPortrait.png',//菜品图
					DishName:'',//菜品名
					IsDisplay:true,//是否显示 是
					CreatePerson:sessionStorage.getItem('loginkey'),//创始人
					UpdatePerson:sessionStorage.getItem('loginkey'),//修改人
				}
				this.Main_ingredientList = [//主料数据
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'1'
					}
				]
				this.Accessories_List = [//辅料
					{	
						Material:'',
		         		Unit:0,
		         		MaterialType:'2'
					}
				]
				this.seasoning_List = [//调料
					{	
						Material:'',
	         			Unit:0,
	         			MaterialType:'3'
					}
				]
				this.RMseasoningList = [//推荐调料数据
					{	
						FlavourRecId:'',
	         			Unit:0,
	         		}
				]
				this.step = [//步骤
					{
						StepName:''
					}
				]
			},
			getGetSDish(){//获取推荐调料
				var DATA= '{"ProductId":'+this.ProductId+'}'
				var self =this
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/GetSDish',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
			        	  	var dataAll =JSON.parse(json)
			        	  	self.seasoningAllList = dataAll
			         	//console.log(dataAll)
			        },
			        error : function(error) { 
			        		console.log(error)
			        }
	   		    });
				
			},
			confirmDishBtn(){//菜品 确认
				this.applyDishModal = false
				//this.cleanDishData()
				//dishInformation//基本   Main_ingredientList主料  Accessories_List辅料  seasoning_List调料  RMseasoningList推荐调料  步骤
				var self =this
				//基本
				self.dishInformation.DishStory = self.dishInformation.DishStory.replace(/\"/g,"'");//把双引号改为单引号(菜品故事)
				var dishInformation = self.dishInformation
				dishInformation.UpdatePerson = sessionStorage.getItem('loginkey')
				//主料,辅料,调料
				var DishMaterial = JSON.stringify(self.Main_ingredientList.concat(self.Accessories_List,self.seasoning_List))
				//推荐调料
				var FlavourRecRole = JSON.stringify(self.RMseasoningList)
				//步骤
				var DishStep =  JSON.stringify(self.step)
				var DATA = '{"DishProductId":"'+dishInformation.DishProductId+'","ProductId":"'+self.ProductId+'","DishStory":"'+dishInformation.DishStory+'","DishPicUrl":"'+dishInformation.DishPicUrl+'","DishName":"'+dishInformation.DishName+'","IsDisplay":"'+dishInformation.IsDisplay+'","CreatePerson":"'+dishInformation.CreatePerson+'","UpdatePerson":"'+dishInformation.UpdatePerson+'","DishMaterial":'+DishMaterial+',"DishStep":'+DishStep+',"FlavourRecRole":'+FlavourRecRole+'}'
				console.log(DATA)
				$('.loding').show()
				$.ajax({
			        type :"post",
			        url :URLHeader.ECActivities+'/api/Product/AddDishProduct',//
			        dataType : 'json',
			        contentType: "application/json; charset=utf-8",				
			        data:DATA,
			        success : function(json) {
			        	//dateTime
//			        	  	var dataAll =JSON.parse(json)
//			        	  	self.useTotal = parseInt(dataAll.Count)
//			        	  	self.useDishList = dataAll.Info
			        	  	$('.loding').hide()
						if (json=='OK') {
							self.$Message.success({
				                content: '菜品保存成功',
				                duration: 3
					        });
						} else{
							self.$Message.error({
				                content: '菜品保存失败',
				                duration: 7
					        });
						}
						self.getApplyDishList()//列表数据刷新
			         	console.log(json)
			        },
			        error : function(error) { 
			        		console.log(error)
			        		self.$Message.error({
				                content: '菜品保存失败,接口错误',
				                duration: 7
					        });
			        }
	   		    });
			},
			cancelDishBtn(){
				this.applyDishModal = false
				this.cleanDishData()
			},
		},
		mounted:function(){
			this.oneSelectChange()//分类 下拉框数据
			this.areaData = ProvinceArea//
			//this.areaData=ProvincesAndcities//省市数据
			
			var self =this
			this.ProductId = this.$route.query.ProductId
			

			
			
			
			//=====规格=====
			this.getVirtualList()//获取虚拟分类
			if (self.ProductId==0) {//新增
				//console.log('新增')
				//=====产品信息=====
				self.addSIzeBtn()//添加一个规格
				
			} else{//编辑
				//console.log('编辑')
				//=====产品信息=====
				this.getProductIfoData()//获取产品信息
				//=====规格=====
				this.getSizeTopList()
				//=====研发菜品=====
				this.getResearchIfo()//获取研发信息
				//=====应用菜=====
				this.getApplyDishList()//应用菜品 列表
				this.getGetSDish()//获取推荐调料数据源
			}
			
		}
	}
</script>
<style>
.productIfo .ql-container{
	height: 70%;
}
.productIfo .quill-editor{
	height: 22rem;
}
/*视频地址弹框移动*/
.productIfo .ql-snow .ql-tooltip{
	margin-left: 9rem;
}
</style>
<style scoped>
.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }
    @keyframes ani-demo-spin {
        from { transform: rotate(0deg);}
        50%  { transform: rotate(180deg);}
        to   { transform: rotate(360deg);}
    }
.editAdd{
	min-height: 35rem;
}
.tab{
	min-height: 30rem;
}
.left{
	float: left;
}
.right{
	float: right;
}
.lable{
	/*margin-left: 2rem;*/
	display: block;
	width: 4rem;
	float: left;
}
.value{
	margin-left: 1rem;
	display: block;
	float: left;
}
/*产品信息*/
.productIfo .li{
	margin-top: 1rem;
	width: 35rem;
	overflow: auto;
}
/*规格*/
.Size{
	
}
.Information{
	display: none;
}
.Size .typeLi{
	float: left;
}
/*研发信息*/
/*.teacherLi{
	border: 1px solid gainsboro;
	padding: 1rem;
	border-radius:4px ;
	width: 10rem;
}*/
.researchIfo .left ul li .card{
	width: 25rem;
	height: 7rem;
	border:1px solid gray;
	border-radius:4px ;
}
.researchIfo .left ul li .card .teacherIfo{
	overflow: auto;
	width: 13rem;
	margin-top: 0.7rem;
}
.researchIfo .left ul li .card .lable{
	margin-right: 0.5rem;
	display: block;
	float: left;
	width: 4rem;
	margin-left: 0.5rem;
}
.researchIfo .left ul li .card .value{
	display: block;
	float: left;
	width: 8rem;
	margin-left: 0;
}
/**/
.applyDish{
	padding:0.5rem;
}
.applyDish .applyDish_top .applyDish_topLi{
	float: left;
	margin-left: 1rem;
}
</style>