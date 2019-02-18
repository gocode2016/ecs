<template>
  <div class="lotteryshow">
    <div class="lotteryhead">
      <img src="../../../../static/image/lottery/logo.png" alt="">
      <span>助力厨师成长</span>
    </div>
    <div class="lotterymain">
    	
    	<div class="lotteryleft" ref="father">
    		<div class="lotteryleftmain">
    			<ul ref="ul">
	          <li v-for="(item,index) in userarr" :key="index">
	            <img :src="item.HeadImgUrl" alt="">
	            <p>{{item.MemberTelePhone}}</p>
	          </li>
	        </ul>
    		</div>
      </div>
      <div class="lotteryright">
        <div class="title">
          <img src="../../../../static/image/lottery/lottery_icon.png" alt="">奖品
        </div>
        <div class="leftitem">
          <img src="../../../../static/image/lottery/cook_draw1.png" alt="" style="width: 85px">
          <p>PANGAO劲椎按摩器</p>
          <p>一等奖</p>
        </div>
        <div class="leftitem">
          <img src="../../../../static/image/lottery/cook_draw2.png" alt="" style="width: 85px">
          <p>shinho保温杯</p>
          <p>二等奖</p>
        </div>
        <div class="leftitem">
          <img src="../../../../static/image/lottery/cook_draw3.png" alt="" style="width: 85px">
          <p>shinho商务签字笔</p>
          <p>三等奖</p>
        </div>
      </div>
    </div>
    <div class="btn" @click="golottery">进入抽奖模式</div>
    
  </div>
</template>

<script>
    export default {
        name: 'lotteryshow',
        data() {
            return {
              times:null,
              userapi:'http://kongapi.shinho.net.cn/ecs/user/api/Member/GetMemberByActivity',
              num:0,
              userarr: []
            }
        },
        mounted() {
          var self = this
          document.title = '抽奖'
          $.ajax({
            type :"post",
            url :self.userapi,
            dataType : 'json',
            contentType: "application/json; charset=utf-8",
            data:{},
            success : function(json) {
              console.log(JSON.parse(json))
              self.userarr = JSON.parse(json)
              
              for(let i in self.userarr){
              	var userarr = self.userarr[i];
              	userarr.MemberTelePhone = userarr.MemberTelePhone.substr(0,3) + '****' + userarr.MemberTelePhone.substr(7);
              }
              
            },
            error : function(error) {
              console.log(error)
            }
          });
          setInterval(()=>{
            $.ajax({
              type :"post",
              url :self.userapi,
              dataType : 'json',
              contentType: "application/json; charset=utf-8",
              data:{},
              success : function(json) {
                console.log(JSON.parse(json))
                self.userarr = JSON.parse(json)
                
                for(let i in self.userarr){
	              	var userarr = self.userarr[i];
	              	userarr.MemberTelePhone = userarr.MemberTelePhone.substr(0,3) + '****' + userarr.MemberTelePhone.substr(7);
	              }
              },
              error : function(error) {
                console.log(error)
              }
            });
          },30000)
          // this.carousel()
        },
        methods: {
          golottery () {
            window.clearInterval(this.times)
            this.$router.push({path:'/lottery'})
          },
          carousel () {
              this.times=setInterval(()=>{
                if(this.$refs.ul.offsetHeight>this.$refs.father.offsetHeight){
                  this.$refs.ul.style.transform = 'translate(0, -'+ this.num*155 +'px)'
                  // this.$refs.ul2.style.transform =  'translate(0, -'+ this.num*155 +'px)'
                  if(this.num*155 == this.$refs.ul.offsetHeight){
                    this.num = 0
                  }
                  this.num++
                }
              },1500)
          }
        },
        computed: {},
        components: {}
    }
</script>

<style scoped>
  li{
    list-style: none;
  }
  .lotteryshow{
  width: 100%;
  height: 100%;
  min-width: 1000px;
  background: url("../../../../static/image/lottery/lotteryshow_back.png") #e0292e;
  background-size: 100% 100%;
  overflow: hidden;
  font-weight: bold;
}
  .lotteryhead{
    text-align: center;
    line-height: 60px;
    height: 57px;
    font-size: 50px;
    color: white;
    letter-spacing: 2px;
    margin-top: 20px;
  }
  .lotteryhead img{
  vertical-align: middle;
  display: inline-block;
  width: 67px;
  height: 51px;
}
  .lotterymain{
    width: 1100px;
    margin: auto;
    overflow: hidden;
    margin-top: 50px;
  }
  .lotteryright{
    overflow: hidden;
    float: left;
    width: 185px;
    height: 471px;
    background-color: #fff8ce;
    border-radius: 10px;
  }
  .title img{
    vertical-align:middle;
    margin-top: -3px;
    margin-right: 10px;
  }
  .title{
    width: 100%;
    height: 45px;
    background: #ffdd7b;
    color: #ff7e20;
    text-align: center;
    font-size: 20px;
    line-height: 45px;
  }
  .leftitem p{
    font-size: 16px;
    line-height: 20px;
    margin: 5px 0;
  }
  .leftitem p span{
    font-weight: bold;
  }
  .leftitem{
    text-align: center;
    padding: 11px;
  }
  .btn{
    width: 10%;
    margin:20px 45%;
    border-radius: 5px;
    font-size: 18px;
    color: #eb443b;
    text-align: center;
    line-height: 40px;
    background: url('../../../../static/image/lottery/btnback.png') no-repeat center;
    background-size: 100% 100%;
  }
  .lotteryleft::-webkit-scrollbar {display:none}
  .lotteryleftmain::-webkit-scrollbar {display:none}
  .lotteryleftmain{
  	height: 431px;
  	overflow-y: scroll;
  }
  .lotteryleft{
    width: 880px;
    float: left;
    height: 471px;
    overflow-y: scroll;
    background: url('../../../../static/image/lottery/lottery_mainbg.png') no-repeat center;
    background-size: 100% 100%;
    border-radius: 10px;
    padding: 20px 20px;
    margin-right: 3%;
  }
  .lotteryleft ul{
    transition: all 1s ease;
    overflow: hidden;
    position: relative;
  }
  .lotteryleft li{
    /*width: 100px;*/
    height: 135px;
    float: left;
    margin: 10px 20px;
  }
  .lotteryleft li p{
    font-weight: normal;
    width: 100%;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    font-size: 16px;
    text-align: center;
    line-height: 30px;
    letter-spacing: 1px;
    color: #ffffff;
  }
  .lotteryleft li img{
    background: url("../../../../static/image/HeadPortrait.png") no-repeat;
    background-size: 100% 100%;
    display: block;
    width: 100px;
    height: 100px;
    border-radius: 50%;
  }
</style>
<style>
  body,html,#app{
    width: 100%;
    height: 100%;
  }
</style>
