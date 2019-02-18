<style scoped>

.wrapper {
    /*position: relative;
    width: 100%;
    height: 100%;
    padding-top: 150px;
    padding-bottom: 200px;*/
    width: 100%;
    height: 100%;
    background-image:url(../../../static/image/login_bg.jpg);
    background-size: cover;
    background-position: 50%;
    position: relative;
   
   
   
    /*background: -webkit-gradient(linear, 0 0, 0 bottom, from(#ff0000), to(rgba(0, 0, 255, 0.5)));*/
    /*background: #fff;*/
    /*url(http://78rbeb.com1.z0.glb.clouddn.com/wp-content/uploads/2014/03/free-blurred-web-backgrounds-04.jpg);*/
}
.wrapper > h1{
  text-align: center;
  vertical-align: middle;
  margin-bottom: 20px;
  color: white;
  padding-top: 18%;
	float: right;
	width: 100%;
}
.login {
    margin: 0 auto;
    padding: 200px auto;
    width: 200px;
    /*height: 100%;*/
   /* margin-left: 10%;*/
}

.login-con{
	position: absolute;
	top: 50%;
	width: 290px;
	right: 160px;
	transform: translateY(-60%);
}
</style>
<style>
	html,body,#app{
		height: 100%;
		width: 100%;
	}
	
</style>
<template>

<div class="wrapper">
<!--<h1>
后台管理系统
</h1>-->
    <!--<div class="login">
        <i-form ref="formInline" :model="formInline" :rules="ruleInline">
            <Form-item prop="user">
                <Input v-model="formInline.user" placeholder="Username">
                	<Icon type="ios-person-outline" slot="prepend"></Icon>
                </Input>
            </Form-item>
            <Form-item prop="password">
                <Input v-model="formInline.password" type="password" placeholder="Password">
                	<Icon type="ios-locked-outline" slot="prepend"></Icon>
                </Input>
            </Form-item>
            <Form-item>
                <i-button type="primary" @click.native="handleSubmit('formInline')" long>登录</i-button>
            </Form-item>
        </i-form>
    </div>-->
            <div class="login-con">
	            <Card :bordered="false">
	                <p slot="title">
	                    <Icon type="log-in"></Icon>
	                    欢迎登录后台管理系统
	                </p>
	                <div class="form-con">
	                    <Form ref="formInline" :model="formInline" :rules="ruleInline">
	                        <FormItem prop="user">
	                            <Input v-model="formInline.user" placeholder="请输入用户名">
	                                <span slot="prepend">
	                                    <Icon :size="16" type="person"></Icon>
	                                </span>
	                            </Input>
	                        </FormItem>
	                        <FormItem prop="password">
	                            <Input type="password" v-model="formInline.password" placeholder="请输入密码">
	                                <span slot="prepend">
	                                    <Icon :size="14" type="locked"></Icon>
	                                </span>
	                            </Input>
	                        </FormItem>
	                        <FormItem>
	                            <Button  @click.native="handleSubmit('formInline')"  type="primary" long class="submit">登录</Button>
	                        </FormItem>
	                    </Form>
	                    <!--<p class="login-tip">输入任意用户名和密码即可</p>-->
	                </div>
	            </Card>
        </div>
</div>

</template>

<script>

export default {
    data() {
            return {
                formInline: {
                    user: '',
                    password: '',
                },
                ruleInline: {
                    user: [{
                        required: true,
                        message: '请填写用户名',
                        trigger: 'blur'
                    }],
                    password: [{
                        required: true,
                        message: '请填写密码',
                        trigger: 'blur'
                    }, {
                        type: 'string',
                        min: 5,
                        message: '密码长度不能小于6位',
                        trigger: 'blur'
                    }]
                }
            }
        },
        methods: {
            handleSubmit(name) {
            			var self =this;
                    this.$refs[name].validate((valid) => {
						
						if (valid) {
							//console.log(self.formInline)
							if (self.formInline.user =='shinhocyfw' && self.formInline.password=='cfhuiyuan10wan+') {
	                            this.$Message.success('提交成功!');
	                            sessionStorage.setItem("loginkey",self.formInline.user);
	                            this.$router.push('/players');							
							} else{
								self.$Message.error('请输入正确用户名或密码!');
							}
							
						} else{
							
						}
						
						
//                      if (valid) {
//                          this.$Message.success('提交成功!');
//                          sessionStorage.setItem("loginkey",self.formInline.user);
//                          this.$router.push('/players');
//                      } else {
//                          this.$Message.error('表单验证失败!');
//                      }
                    })
                },
                handleReset(val) {
                    console.log(val)
                    
                }
       },
       mounted:function(){
	       	$(document).keydown(function(event){  
			    if(event.keyCode==13){  
			    $('.submit').click()
			    }  
			}); 
       }
}

</script>
