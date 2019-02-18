<!--<style media="screen">
  .rtf-content {
    height: 400px;
  }
</style>
<template>
    <div class="rtf-content">
        <quill-editor ref="myTextEditor" v-model="content" :config="editorOption"></quill-editor>
    </div>
</template>

<script>
    import { quillEditor } from 'vue-quill-editor';
    export default {
        data: function(){
            return {
                content: '',
                editorOption: {

                }
            }
        },
        components: {
            quillEditor
        }
    }
</script>-->
<style media="screen">
  .rtf-content {
    height: 400px;
  }
</style>
<template>
    <div class="rtf-content">
       <quill-editor ref="myTextEditor"
                              v-model="content"
                              :options="editorOption"
                              @focus="onEditorFocus($event)"
                              @ready="onEditorReady($event)"                 
                >
                  <div id="toolbar" slot="toolbar">
                    <button class="ql-bold">Bold</button>
                    <button class="ql-italic">Italic</button>
                    <select class="ql-size">
                      <option value="small"></option>
                      <option selected></option>
                      <option value="large"></option>
                      <option value="huge"></option>
                    </select>
                    <!-- Add subscript and superscript buttons -->
                    <!--<button type="button" class="ql-image"></button>-->
                    <button type="button" @click="customButtonClick">img</button>
                    <input type="file" class="custom-input" @change='upload' style='display: none !important;'>
                  </div>
                </quill-editor>
    </div>
</template>

<script>
    import { quillEditor } from 'vue-quill-editor';
    export default {
        data(){
			      return {
			      	content:'',
			        length: '',
			        editor: {},
			        editorOption: {
			          modules: {
			          //  imageImport: true,
			          //  imageResize: {
			          //    displaySize: true
			         //   },
			            toolbar: '#toolbar',
			             handlers: {
					            image:this.imageHandler
					          }
			          }
			        },
			      }
			  },
        components: {
            quillEditor
        },
//			computed: {
//		      editor() {
//		        return this.$refs.myTextEditor.quill
//		      }
 //  		 },
        methods: {
			    imageHandler() {
			    			console.log(233)
						  var range = this.quill.getSelection();
						  var value = prompt('What is the image URL');
						  this.quill.insertEmbed(range.index, 'image', value, Quill.sources.USER);
					},
		      onEditorFocus(editor) {
		        this.editor = editor   //当content获取到焦点的时候就 存储editor
		      },
		      onEditorReady(editor) {
		        this.editor = editor //当quill实例化完先 存储editor
		      },
		
		      customButtonClick(){
		        var range
		        console.log(233)
		        if (this.editor.getSelection() != null) { 
		          range = this.editor.getSelection()
		          this.length = range.index  //content获取到焦点，计算光标所在位置，目的为了在该位置插入img
		        } else {
		          this.length = this.content.length  //content没有获取到焦点时候 目的是为了在content末尾插入img
		        }
		        this.$el.querySelector('.custom-input').click();   //打开file 选择图片
		      },
		      upload(event){
		      	console.log('提交')
		        var reader = new FileReader();
		        var img1 = event.target.files[0];
		        reader.readAsDataURL(img1);
		        var that = this;
		        reader.onloadend = function () {
		          //上传图片
		          that.pushImg(reader.result, 1)
		        }
		      },
				   pushImg(base64, type){   
				        let self = this
				        
				        var json = {data: [{id: 0, content: base64}]}         
				        console.log(json)
		//		        api.pushImgToGeturl(json).then(function (res) { //这一块可以忽略知识调接口获取到 imgurl
		//		          if (res.data.success) {
		//		              self.contentImg = res.data.data[0].url.url    //获取到了图片的URL
		//		              console.log(self.contentImg)          
		//		              self.editor.insertEmbed(self.length, 'image', self.contentImg); // ★这里才是重点★ 插入到content中
		//		          }
		//		        })
				      }
			},
			mounted:function(){
				//this.$refs.newEditor.quill
				
			}

    }
</script>
