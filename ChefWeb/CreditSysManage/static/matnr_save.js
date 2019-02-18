import apiUrl from '../../api_urls.js';
var _this;
export default {
    data() {
        return {
            UploadedFile: [],
            imgUrlRoot: apiUrl.StorePhoto,
            imgUploadUrl: apiUrl.UploadFile,
            imgName: '',
            visible: false,
            uploadList: [],
            matnrHandle: 'add',
            isReset: true,

            basicFormItem: {
                matnrcode: '',                      // 原料编码
                matnrname: '',                      // 原料名称
                enname: '',                         // 英文名
                unitid: '',                         // 单位id
                unitname: '',                       // 单位名
                matnrclass: '1',                    // 一级分类
                classname: '',
                twomatnrclass: '',                  // 二级分类
                twoclassname: '',
                spec: '',                           // 规格
                specname: '',
                color: '',                          // 颜色
                colorname: '',
                photo: [],                          // 图片
                img: ''              
            },
            priceFormItem: {
                costprice: null,                  // 原料价
                buyinprice: null,                 // 出厂价
                saleprice: null,                  // 零售价
                commission: null,                 // 提成金额
                logisticsgrossprofit: '',         // 物流毛利润
                logisticsgrossprofitrate: '',     // 物流毛利率
                storegrossprofit: '',             // 店铺毛利润
                storegrossprofitrate: '',         // 店铺毛利率
                isdiscount: false,                // 可否折价
                insdt: null,                      // 不详
                rownumber: 1                      // 不详
            },
            MatnrClassList: [],
            twoMatnrClassList: [],
            subMatnrClassList: [],
            unitList: [],
            specList: [],
            colorList: [],
            downUnitId: '',
            downUnitName: '',
            downTotal: 0,
            downData: {
                MatnrCode: '',
                MatnrQtyList: [
                    { StoreId: 1, Qty: 2, UnitId: 12 },
                    { StoreId: 2, Qty: 2, UnitId: 12 }
                ],
                EmpId: '',
                StoreId: ''
            },
            storeList: [],
            formItem: {
                value: [0, 0, 0]
            }
        }
    },
    methods: {
        init(){},
        handleFormatError (file) {
            this.$Notice.warning({
                title: '文件格式不正确',
                desc: '文件 ' + file.name + ' 格式不正确，请上传 jpg 或 png 格式的图片。'
            });
        },
        handleMaxSize (file) {
            this.$Notice.warning({
                title: '超出文件大小限制',
                desc: '文件 ' + file.name + ' 太大，不能超过 2M。'
            });
        },
        handleView(name) {
            this.imgName = name;
            this.visible = true;
        },
        handleBeforeUpload() {
            const check = this.uploadList.length < 3;
            if (!check) {
                this.$Notice.warning({
                    title: '最多只能上传 3 张图片。'
                });
            }
            return check;
        },
        handleRemove(file) {
            const fileList = this.$refs.upload.fileList;
            const photoArr = this.basicFormItem.photo;
            this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
            this.basicFormItem.photo.splice(photoArr.indexOf(file.name), 1);
        },        
        onUploadSucess(res, file) {
            this.basicFormItem.photo.push(res);
        },        
        changeGrossProfit () {
            // 毛利润、毛利率
            var logisticsgrossprofit = this.priceFormItem.buyinprice - this.priceFormItem.costprice;
            this.priceFormItem.logisticsgrossprofit = logisticsgrossprofit.toFixed(2); // 物流毛利
            var logisticsgrossprofitrate = (this.priceFormItem.buyinprice - this.priceFormItem.costprice) / this.priceFormItem.buyinprice;
            this.priceFormItem.logisticsgrossprofitrate = logisticsgrossprofitrate.toFixed(2) * 100 + '%'; // 物流毛利率                
            var storegrossprofit = this.priceFormItem.saleprice - this.priceFormItem.buyinprice;
            this.priceFormItem.storegrossprofit = storegrossprofit.toFixed(2); // 店铺毛利
            var storegrossprofitrate = (this.priceFormItem.saleprice - this.priceFormItem.buyinprice) / this.priceFormItem.buyinprice ; // 店铺毛利率
            this.priceFormItem.storegrossprofitrate = storegrossprofitrate.toFixed(2) * 100 + '%';
            //this.priceFormItem.commission = this.priceFormItem.commission.toFixed(2);
        },
        getType (type) {
            var url = apiUrl.GetDictByType + '?type=' + type;
            this.$ajax.get(url, {}).then(function(response) {  
                var obj = response.data;
                if(type == 'spec') { _this.specList  = JSON.parse(obj); }
                if(type == 'color') { _this.colorList = JSON.parse(obj); }
            })
            .catch(function(response) {
                console.log(response)
            })
        },
        getMatnrClassList() {
            this.$ajax.get(apiUrl.GetMatnrClassList, {}).then(function(response) {    
                var obj = response.data;
                var matnrClassArr = JSON.parse(obj);
                for(var i in matnrClassArr) {
                    if(matnrClassArr[i].parentid == '0') {
                        _this.MatnrClassList.push(matnrClassArr[i]);
                    }else{
                        _this.twoMatnrClassList.push(matnrClassArr[i]);
                    }
                }
            })
            .catch(function(response) {
                console.log(response)
            })
        },
        getSubMatnrClassList(parentid) {
            this.basicFormItem.matnrclass = parentid;
            this.subMatnrClassList = [];
            var twoMatnrClassList = this.twoMatnrClassList;
            for(var i in twoMatnrClassList) {
                if(twoMatnrClassList[i].parentid == parentid) {
                    this.subMatnrClassList.push(twoMatnrClassList[i]);
                }
            }
            this.basicFormItem.twomatnrclass = '';
        },
        returnMatnrList() {
            window.location.href = '#/data_matnr';
        },
        matnrDown(matnrcode, unitid) {
            let unitArr = this.unitList;
            let unitId = '';
            let unitName = '';
            for(let i in unitArr) {
                if(unitArr[i].unitid == unitid) {
                    this.downUnitId = unitArr[i].unitid;
                    this.downUnitName = unitArr[i].unitname;
                }
            }
            this.downData.MatnrCode = matnrcode;

            this.$ajax.get(apiUrl.GetAllStore + "?isdel=0", {})
                .then(function(response) {
                    var obj = response.data;
                    _this.storeList = JSON.parse(obj);

                    // 重置下拨
                    _this.downData.MatnrQtyList = [];
                    for (let i = 0; i < _this.storeList.length; i++) {
                        _this.formItem.value[i] = 0;
                        var matnrQty = {
                            StoreId: _this.storeList[i].storeid,
                            Qty: 0,
                            UnitId: _this.downUnitId
                        }
                        _this.downData.MatnrQtyList.push(matnrQty);
                    }
                    _this.downTotal = 0;

                    $("#matnr-right-sidebar").slideDown(50);
                    $("#matnr-right-sidebar").toggleClass("shw-rside");
                })
                .catch(function(response) {
                    console.log(response)
                })
        },
        changeDownTotal() {
            let total = 0;
            for (let i = 0; i < this.formItem.value.length; i++) {               
                this.downData.MatnrQtyList[i].Qty = this.formItem.value[i];
                total += this.formItem.value[i];
            }
            this.downTotal = total;
        },
        SetMatnrDown() {
            this.$ajax.post(apiUrl.SetMatnrDown, _this.downData).then(function(response) {
                    var data = response.data;
                    if (data.Error) {
                        swal(data.Msg, "", "error");
                    } else {
                        swal("操作成功", "", "success");
                        _this.closeMatnrDown();
                    }
                })
                .catch(function(response) {
                    console.log(response)
                })
        },
        closeMatnrDown() {
            $("#matnr-right-sidebar").slideDown(50);
            $("#matnr-right-sidebar").toggleClass("shw-rside");
             _this.resetData();
        },
        saveMatnr () {
            if (this.basicFormItem.matnrname == "") {
                swal("请输入原料名称", "", "error");
                return;
            }
            if (this.basicFormItem.enname == "") {
                swal("请输入原料英文名称", "", "error");
                return;
            }

            if(this.priceFormItem.isdiscount) {
                var isdiscount = '1';
            }else{
                var isdiscount = '0';
            }

            var imgs = this.basicFormItem.photo;
            if(imgs.length > 1) {
                var imgStr = imgs.join('|');
            }else if(imgs.length == 1){
                var imgStr = imgs[0];
            }else if(imgs.length == 0){
                var imgStr = '';
            }

            this.parms =  {
                MatnrCode:       this.basicFormItem.matnrcode,
                MatnrName:       this.basicFormItem.matnrname,
                MatnrClass:      String(this.basicFormItem.matnrclass),
                twoMatnrClass:   String(this.basicFormItem.twomatnrclass),
                EnName:          this.basicFormItem.enname,
                UnitId:          this.basicFormItem.unitid,
                Spec:            this.basicFormItem.spec,
                Color:           this.basicFormItem.color,
                Img:             imgStr,                      
                CostPrice:       this.priceFormItem.costprice,
                BuyInPrice:      this.priceFormItem.buyinprice,
                SalePrice:       this.priceFormItem.saleprice,
                Commission:      this.priceFormItem.commission,
                IsDiscount:      isdiscount
            }
            
            this.$ajax.post(apiUrl.SaveMatnr, _this.parms).then(function(res) {
                var data = res.data;             
                if (data.Error) {
                    swal(data.Msg, "", "error");
                } else {
                    if(_this.matnrHandle == 'add') {
                        _this.basicFormItem.matnrcode = data.Msg;
                        // 弹出是否下拨原料
                        swal({
                            title: "添加成功，是否下拨?",
                            text: "",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "确定",
                            cancelButtonText: "取消",
                            //closeOnConfirm: true
                        }, function(isConfirm) {
                            if (isConfirm === true) {
                                _this.matnrDown(_this.basicFormItem.matnrcode, _this.parms.UnitId);                              
                            } else {
                                _this.resetData();
                            }
                        })                       
                    }else{
                        swal("操作成功", "", "success");
                        _this.$refs.upload.clearFiles();                       
                        // _this.uploadList = _this.$refs.upload.fileList;
                        _this.returnMatnrList();
                    }
                }
            })
            .catch(function(response) {
                console.log(response)
            })
        },
        resetData () {
            this.basicFormItem = {
                matnrcode: '',                      // 原料编码
                matnrname: '',                      // 原料名称
                enname: '',                         // 英文名
                unitid: '',                         // 单位id
                unitname: '',                       // 单位名
                matnrclass: '1',                    // 一级分类
                classname: '',
                twomatnrclass: '',                  // 二级分类
                twoclassname: '',
                spec: '',                           // 规格
                specname: '',
                color: '',                          // 颜色
                colorname: '',
                photo: [],                          // 图片
                img: ''              
            },
            this.priceFormItem = {
                costprice: null,                  // 原料价
                buyinprice: null,                 // 出厂价
                saleprice: null,                  // 零售价
                commission: null,                 // 提成金额
                logisticsgrossprofit: '',         // 物流毛利润
                logisticsgrossprofitrate: '',     // 物流毛利率
                storegrossprofit: '',             // 店铺毛利润
                storegrossprofitrate: '',         // 店铺毛利率
                isdiscount: false,                // 可否折价
                insdt: null,                      // 不详
                rownumber: 1                      // 不详
            }
            this.uploadList = []
        }
    },
    mounted: function () {
        this.uploadList = this.$refs.upload.fileList;
        _this = this;
        this.init();

        this.changeGrossProfit(); // 毛利、毛利率处理
        // 获取单位
        $.getJSON('../../../static/json/unit.json', function(data){ 
            _this.unitList = data;
        }); 
        this.getType('spec');
        this.getType('color');      
        this.getMatnrClassList(); // 一级分类
        // 二级分类
        this.$ajax.get(apiUrl.GetMatnrClassList, {}).then(function(response) {    
            var obj = response.data;
            var matnrClassArr = JSON.parse(obj);
            for(var i in matnrClassArr) {
                if(matnrClassArr[i].parentid == _this.basicFormItem.matnrclass) {
                    _this.subMatnrClassList.push(matnrClassArr[i]);
                }
            }
        })
        .catch(function(response) {
            console.log(response)
        })

        let matnrHandle = store.get("matnrHandle");
        if(matnrHandle == 'edit') {
            // 编辑原料数据        
            let matnrShow = store.get("matnrShow");
            if(matnrShow.isdiscount == 0) {
                matnrShow.isdiscount = false;
            }else{
                matnrShow.isdiscount = true;
            }
            this.matnrHandle = matnrHandle;
            matnrShow.unitid = String(matnrShow.unitid),
            this.basicFormItem = matnrShow;
            this.priceFormItem = matnrShow;

            // 处理图片
            _this.$refs.upload.clearFiles();
            _this.uploadList = _this.$refs.upload.fileList;

            if(matnrShow.img) {
                this.basicFormItem.photo = matnrShow.img.split("|");
                for(let i in this.basicFormItem.photo) {
                    _this.uploadList.push({
                        name: this.basicFormItem.photo[i],
                        url: apiUrl.StorePhoto + this.basicFormItem.photo[i],
                        status: "finished",
                        response: this.basicFormItem.photo[i]
                    });
                }
            }else{
                this.basicFormItem.photo = [];
            }
        }   
    },
    computed: {

    }

}