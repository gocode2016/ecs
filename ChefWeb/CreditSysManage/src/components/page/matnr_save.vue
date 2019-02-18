<template>
    <div class="container-fluid">
        <div class="row bg-title">
            <div class="col-lg-3 col-md-2 col-sm-12 col-xs-12" style="margin-bottom: 5px;">
                <h4 class="page-title"><i class="mdi mdi-arrow-left-bold fa-fw"></i><a href="#/data_matnr">返回原料列表</a></h4>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-5 col-xs-5" style="margin-bottom: 5px;"></div>
            <div class="col-lg-2 col-md-2 col-sm-7 col-xs-7" style="margin-bottom: 5px;"></div>
            <div class="col-lg-5 col-md-6 col-sm-12 col-xs-12 titlecusrecbut" style="margin-bottom: 5px; text-align: right;"></div>
        </div>
    
        <div class="row">
            <div class="col-md-12">
                <div class="white-box">
                    <!-- Tabstyle start -->
                    <h3 class="box-title m-b-0">基本信息</h3>
    
                    <hr class="m-t-10 m-b-20">
                    <Form :model="basicFormItem" :label-width="100">
                        <Form-item label="原料编码：">
                            <Input v-model="basicFormItem.matnrcode" placeholder="" disabled style="width:150px;"></Input>
                        </Form-item>
                        <Form-item label="原料名称：">
                            <Input v-model="basicFormItem.matnrname" placeholder="请输入" style="width:300px;"></Input>
                        </Form-item>
                        <Form-item label="Matnr Name：">
                            <Input v-model="basicFormItem.enname" placeholder="请输入" style="width:300px;"></Input>
                        </Form-item>
                
                        <Form-item label="单位：">
                            <Select v-model="basicFormItem.unitid" style="width:100px">
                                <Option v-for="item in unitList" :value="item.unitid" :key="item.unitid">{{ item.unitname }}</Option>
                            </Select> 
                        </Form-item>
                        <Form-item label="一级分类：">
                            <div class="input-group">
                                <ul class="nav nav-pills">
                                <template v-for="item in MatnrClassList">
                                    <li  :class="{'active':basicFormItem.matnrclass == item.classid}"  @click="getSubMatnrClassList(item.classid)">
                                        <a style="padding-top:0px;padding-bottom:0px;" data-toggle="tab" aria-expanded="false">{{ item.classname }}</a>
                                    </li>
                                </template>                  
                                </ul>
                            </div>    
                        </Form-item>
                        <Form-item label="二级分类：">
                            <div class="input-group">
                                <ul class="nav nav-pills">         
                                <template v-for="item in subMatnrClassList">
                                    <li :class="{'active':basicFormItem.twomatnrclass == item.classid}"  @click="basicFormItem.twomatnrclass = item.classid">
                                        <a style="padding-top:0px;padding-bottom:0px;" data-toggle="tab" aria-expanded="false">{{ item.classname }}</a>
                                    </li>
                                </template>
                                </ul>
                            </div>   
                        </Form-item>
                        <Form-item label="规格：">
                            <Row>
                                <Col span="5">
                                <Select v-model="basicFormItem.spec" style="width:100px">
                                    <Option v-for="item in specList" :value="item.code" :key="item.code">{{ item.textvalue }}</Option>
                                </Select> <Button type="dashed" icon="plus-round">新增</Button>
                                </Col>
                                <Col span="3" style="text-align: right;padding-right:15px;">颜色：</Col>
                                <Col span="5">
                                <Select v-model="basicFormItem.color" style="width:100px">
                                    <Option v-for="item in colorList" :value="item.code" :key="item.code">{{ item.textvalue }}</Option>
                                </Select> <Button type="dashed" icon="plus-round">新增</Button>
                                </Col>
                                <Col span="15"></Col>
                            </Row>
                        </Form-item>
                        <Form-item label="图片：">
                            <div class="demo-upload-list" v-for="item in uploadList">
                                    <template v-if="item.status === 'finished'">
                                        <img :src="imgUrlRoot+item.response">
                                        <div class="demo-upload-list-cover">
                                            <!-- <Icon type="ios-eye-outline" @click.native="handleView(item.name)"></Icon> -->
                                            <Icon type="ios-trash-outline" @click.native="handleRemove(item)"></Icon>
                                        </div>
                                    </template>
                                    <template v-else>
                                        <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
                                    </template>
                                </div>
                                <Upload ref="upload" :show-upload-list="false" :default-file-list="UploadedFile" :on-success="onUploadSucess" :format="['jpg','jpeg','png']" :max-size="2048" :on-format-error="handleFormatError" :on-exceeded-size="handleMaxSize" :before-upload="handleBeforeUpload" multiple type="drag" v-bind:action="imgUploadUrl" style="display: inline-block;width:58px;">
                                    <div style="width: 58px;height:58px;line-height: 58px;">
                                        <Icon type="camera" size="20"></Icon>
                                    </div>
                                </Upload>
                                <Modal title="查看图片" v-model="visible">
                                    <img :src="imgUrlRoot + imgName" v-if="visible" style="width: 100%">
                                </Modal>
                            <div style="color:#999;">最多可上传3张图片。<br>上传的图片分辨率控制在500*500以下，单张图片质量大小控制在300K以内</div>
                        </Form-item>
    
                    </Form>
                    <h3 class="box-title m-b-0">价格信息</h3>
    
                    <hr class="m-t-10 m-b-20">
                    <Form :model="priceFormItem" :label-width="100">
                        <Form-item label="原料价：">
                            <Row>
                                <Col span="3">
                                <Input :max="10" :min="1" v-model="priceFormItem.costprice" style="width:100px;"></Input>
                                </Col>
                                <Col span="4" style="text-align: right;padding-right:15px;">出厂价：</Col>
                                <Col span="3">
                                <Input :max="10" :min="1" v-model="priceFormItem.buyinprice" style="width:100px;"></Input>
                                </Col>
                                <Col span="4" style="text-align: right;padding-right:15px;">零售价：</Col>
                                <Col span="3">
                                <Input @on-change="changeGrossProfit" :max="10" :min="1" v-model="priceFormItem.saleprice" style="width:100px;"></Input>
                                </Col>
                                <Col span="4" style="text-align: right;padding-right:15px;">提成：</Col>
                                <Col span="3">
                                <Input :max="10" :min="1" v-model="priceFormItem.commission" style="width:100px;"></Input>
                                </Col>
                            </Row>
    
                        </Form-item>
    
                        <Form-item label="物流毛利润：">
                            <Row>
                                <Col span="3"> {{priceFormItem.logisticsgrossprofit}} </Col>
                                <Col span="4" style="text-align: right;padding-right:15px;">物流毛利率：</Col>
                                <Col span="3"> {{priceFormItem.logisticsgrossprofitrate}} </Col>
                                <Col span="4" style="text-align: right;padding-right:15px;">店铺毛利润：</Col>
                                <Col span="3"> {{priceFormItem.storegrossprofit}} </Col>
                                <Col span="4" style="text-align: right;padding-right:15px;">店铺毛利率：</Col>
                                <Col span="3"> {{priceFormItem.storegrossprofitrate}} </Col>
                            </Row>
                        </Form-item>
    
                        <Form-item label="是否可折价：">
                            <i-switch v-model="priceFormItem.isdiscount"></i-switch>
                        </Form-item>
                        <Form-item>
                            <button type="submit" @click="saveMatnr" class="btn btn-success waves-effect waves-light m-r-10">提交</button>
                            <button type="submit" @click="returnMatnrList" class="btn btn-inverse waves-effect waves-light">取消</button>
                        </Form-item>
                    </Form>
    
                </div>
            </div>
        </div>

        <div class="right-sidebar" id="matnr-right-sidebar">
            <div class="slimscrollright">
                <div class="rpanel-title"> 下拨数量
                    <span @click="closeMatnrDown">
                        <i class="ti-close matnr-side-toggle"></i>
                    </span>
                </div>
                <div class="r-panel-body" style="padding:15px;">
                    <div slot="content">
                    <Form :model="formItem">
                        <ul class="list-group list-group-full">
                            <template v-for="(store, index) in storeList">
                                <li class="list-group-item">
                                    <Form-item>
                                        <span class="pull-right">
                                            <Input-number @on-change="changeDownTotal" v-model="formItem.value[index]" :max="10" :min="0" size="small" style="width:50px;"></Input-number>
                                            <span class="small-money"> {{downUnitName}} </span>
                                        </span> {{store.storename}} 
                                    </Form-item>
                                </li>
                            </template>
                            <li class="list-group-item" style="font-weight:bold;">
                                <Form-item>                                
                                    <span class="pull-right">
                                        <span style="width:50px; display: inline-block; text-align: center;"> {{downTotal}} </span>
                                        <span class="small-money"> {{downUnitName}} </span>
                                    </span> 下拨总量 
                                </Form-item>
                            </li>
                        </ul>
                        <Button type="success" v-on:click="SetMatnrDown" shape="circle" icon="checkmark-circled" style="margin-top:10px;" long>保存</Button>
                    </Form>
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>
<script src="./matnr_save.js"></script>
<style>
.demo-upload-list {
    display: inline-block;
    width: 60px;
    height: 60px;
    text-align: center;
    line-height: 60px;
    border: 1px solid transparent;
    border-radius: 4px;
    overflow: hidden;
    background: #fff;
    position: relative;
    box-shadow: 0 1px 1px rgba(0, 0, 0, .2);
    margin-right: 4px;
}
.demo-upload-list img {
    width: 100%;
    height: 100%;
}

.demo-upload-list-cover {
    display: none;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: rgba(0, 0, 0, .6);
}

.demo-upload-list:hover .demo-upload-list-cover {
    display: block;
}

.demo-upload-list-cover i {
    color: #fff;
    font-size: 20px;
    cursor: pointer;
    margin: 0 2px;
}
.page-title a { color: #313131; }



.list-group {
    padding-left: 0;
    margin-bottom: 20px;
}

#matnr-right-sidebar .list-group-item {
    position: relative;
    display: block;
    padding: 10px 15px;
    margin-bottom: -1px;
    background-color: #fff;
    border: 1px solid #ddd;
    font-size: 12px;
    line-height: 30px;
}

#matnr-right-sidebar .list-group-item .ivu-form-item { 
    margin-bottom: 0;
}

.small-money {
    width: 50px;
    text-align: right;
    float: right;
}

</style>