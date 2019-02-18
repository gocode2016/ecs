<template>
	<!--产品详情查看-->
	<div>
		<img :src="ImgUrl" style="max-height: 5rem;max-width: 10rem;display: block;margin:0 auto;margin-top: 0.5rem;"/>
		<Upload
	        ref="upload"
	        :show-upload-list="false"
	        :on-success="GoodsBigImgSuccess"
	        :format="['jpg','jpeg','png']"
	        :max-size="2048"
	        :before-upload='handleBeforeUpload'
	        :on-format-error="handleFormatError"
	        :on-exceeded-size="handleMaxSize"
	        :multiple=false
	        type="drag"
	        :action=UPImageProduct
	        style="display:block;width:90px;margin: 0 auto;margin-bottom: 0.5rem;margin-top: 0.5rem;" >
	         <Button type="primary">选择商品图</Button>
	   </Upload>	
	</div>
</template>

<script>
	export default{
		data(){
			return{
				ImgUrl:'../../../../../static/image/placeholderBag.png',//商品图片
				UPImageProduct:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Product',//上传插件地址
			}
		},
		methods:{
			GoodsBigImgSuccess(res, file){//图片上传成功
				 $('.loding').hide()
	             console.log(file)
	             var data = JSON.parse(res);
	             this.ImgUrl = URLHeader.loading+'/Product/'+data.Id+'.'+data.Type;
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
                    desc: '文件 ' + file.name + ' 太大，不能超过 2M。'
                });
            },
             handleBeforeUpload () {//选择图片
          		$('.loding').show()
            },
		},
		mounted:function(){
			
		}
	}
</script>

<style scoped>
</style>