<!--<style media="screen">
  div > h1 {
    margin-bottom: 20px;
  }
</style>
<template>
    <div>
      <h1>使用iView自带的文件上传组件，方便快捷</h1>
      <Upload
          multiple
          type="drag"
          action="//jsonplaceholder.typicode.com/posts/">
          <div style="padding: 20px 0">
              <Icon type="ios-cloud-upload" size="52" style="color: #3399ff"></Icon>
              <p>点击或将文件拖拽到这里上传</p>
          </div>
      </Upload>
    </div>
</template>
<script>
    export default {

    }
</script>-->

<style>

</style>
<template>
    <div style="text-align: center;width: 40%;margin:  0 auto;">
    <!--	<img  :src="imageSrc"  style="height: 10rem;"/>-->
     <!--<Upload action="//jsonplaceholder.typicode.com/posts/" :before-upload="handleUpload" :show-upload-list	=false  :on-success="handleSuccess" :format="['jpg','jpeg','png']" type="drag">
        <Button type="ghost" icon="ios-cloud-upload-outline">上传文件</Button>
    </Upload>-->
    <img :src="imageUrl" style="height: 10rem;">
    <Upload
        ref="upload"
        :show-upload-list="false"
        :on-success="handleSuccess"
        :format="['jpg','jpeg','png']"
        :max-size="2048"
        :on-format-error="handleFormatError"
        :on-exceeded-size="handleMaxSize"
        :before-upload="handleBeforeUpload"
        :multiple=false
        type="drag"
        action="//jsonplaceholder.typicode.com/posts/"
        style="display: inline-block;width:58px;">
        <div style="width: 58px;height:58px;line-height: 58px;">
            <Icon type="camera" size="20"></Icon>
        </div>
    </Upload>
     
    </div>
</template>
<script>
    export default {
					 data () {
            return {
          	imageName:'a42bdcc1178e62b4694c830f028db5c0',
					imageUrl:'https://o5wwk8baw.qnssl.com/a42bdcc1178e62b4694c830f028db5c0/avatar'
            }
        },
        methods:{
            handleSuccess (res, file) {
                // 因为上传过程为实例，这里模拟添加 url
                console.log(file)
                this.imageUrl = 'https://o5wwk8baw.qnssl.com/7eb99afb9d5f317c912f08b5212fd69a/avatar'
                 this.imageName = '7eb99afb9d5f317c912f08b5212fd69a'
//              file.url = 'https://o5wwk8baw.qnssl.com/7eb99afb9d5f317c912f08b5212fd69a/avatar';
//              file.name = '7eb99afb9d5f317c912f08b5212fd69a';
            },
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
//          handleBeforeUpload () {
//              const check = this.defaultList.length < 2;
//              if (!check) {
//                  this.$Notice.warning({
//                      title: '最多只能上传 5 张图片。'
//                  });
//              }
//              return check;
//          }
        }
    }
</script>
