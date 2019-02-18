<template>
  <div class="michelinshow" id="scrolldiv">
    <div class="tab">
      <a @click="jump(1)">诠释标准</a>
      <a @click="jump(2)">名厨访谈</a>
      <a @click="jump(3)">从厨之路</a>
      <a @click="jump(4)">惊喜好礼</a>
    </div>
    <div style="width: 100%;height: 100%;overflow: auto" ref="scrolldiv">
      <div class="michelinmain" style="padding-bottom: 10px;overflow: auto">
      <div class="headbg">
        <img src="../../static/credit/michelintitle.png" alt="">
      </div>
      <div>
        <div class="tabitem" style="margin-top: 40px">
          <div class="ta" ref="tab1">
            <img src="../../static/credit/michelintitle1.png" alt="" style="top: -15px;height: 45px">
          </div>
          <div class="itemtxt" style="margin-top: 0;width: 325px">
            <p>一丝不苟的刀工，分秒必争的火候，精挑细选的食材</p>
            <p>每一道工序的背后，是厨师对于职业标准的不懈追求</p>
            <p>因为丝毫不差的标准，才能造就精妙绝伦的美味</p>
          </div>
          <div style="padding: 30px 0">
            <div class="video-box">
              <iframe frameborder='0' allowtransparency="false" allowfullscreen='true' :src='videoUrl' width="100%" height="175px"></iframe>
            </div>
            <!--<img src="../../static/credit/michelinvideo.png" alt="" class="video">-->
          </div>
        </div>
        <div class="tabitem" style="margin-top: 40px">
          <div class="ta" ref="tab2">
            <img src="../../static/credit/michelintitle2.png" alt="">
            <!--<img src="../../static/credit/michelintitle2-2.png" alt="" style="top: -15px;height: 45px">-->
          </div>
          <div class="itemtxt" style="margin-top: 0">
            <p>以标准为本，才能烹调出美味的菜品，</p>
            <p>才能衍生出传承与创新的变与不变</p>
            <p>传承是百年精华的凝聚，创新是不破不立的勇敢</p>
          </div>
          <div class="lis">
            <div class="lis1">
              <a href="https://mp.weixin.qq.com/s/gGEvr4Cscu5V23cJLBVGBQ">
                <img src="../../static/credit/michelin2.png" alt="">
              </a>
              <p class="listxt">经典</p>
              <p>是对每一道工序的严苛把控</p>
              <p>是对每一种味道的一丝不苟</p>
            </div>
            <div class="lis1">
              <a href="https://mp.weixin.qq.com/s/7JtRNv5vQj4Wy1_nPsF4AQ">
                <img src="../../static/credit/michelin1.png" alt="">
              </a>
              <p class="listxt">创新</p>
              <p>是敢于突破原有规则的气魄</p>
              <p>是取人之长补己之短的智慧</p>
            </div>
          </div>
        </div>
        <div class="tabitem">
          <div class="ta" ref="tab3">
            <img src="../../static/credit/michelintitle3.png" alt="">
          </div>
          <div class="itemtxt" style="margin-bottom: 30px">
            <p>为了追寻职业的标准，一次次练习，一次次坚守</p>
            <p>从业的道路有过欢笑，有过失意</p>
            <p>致敬未变的初心，感恩一路的前行</p>
            <p>选择你最认可的厨师标准，生成专属厨师海报</p>
          </div>
          <div class="tab3">
            <img src="../../static/credit/michelinsmpeo.png" alt="" style="width: 200px">
            <img src="../../static/credit/michelinsmall.png" alt="" class="small" ref="mybox">
            <a @click="clickshow">点击查看</a>
          </div>
        </div>
        <div class="tabitem">
          <div class="ta" ref="tab4">
            <img src="../../static/credit/michelintitle4.png" alt="">
          </div>
          <div class="itemtxt">
            <p>参与此次活动即可抽奖，更有机会获得Iphone X</p>
            <p>油盐酱醋茶一路陪伴</p>
            <p>欣和致敬标准感恩回馈</p>
          </div>
          <div class="tab3">
            <div class="mmm">
              <img src="../../static/credit/michelintab4.png" alt="" style="width: 280px" ref="mybox1" class="mybox1">
            </div>
            <a @click="godraw">点击抽奖</a>
          </div>
        </div>
      </div>
    </div>
    </div>
  </div>
</template>

<script>
//import { } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{

	},
	data(){
		return{
      smallleft: 'left:5px',
      videoUrl:'https://v.qq.com/iframe/player.html?vid=d0698huq1ky'
		}
	},
  mounted () {
    const that = this
    setInterval(function () {
      if (that.$refs.mybox.style.transform == 'scale(0.8)') {
        that.$refs.mybox.style.transform = 'scale(1)'
      } else {
        that.$refs.mybox.style.transform = 'scale(0.8)'
      }
    }, 500)
    // setInterval(function () {
    //   if (that.$refs.mybox1.style.left == '5px') {
    //     that.$refs.mybox1.style.left = '10px'
    //   } else {
    //     that.$refs.mybox1.style.left = '5px'
    //   }
    // }, 1000)
  },
	methods:{
    godraw () {
      this.$router.push({path: '/component/michelindraw'})
    },
    clickshow () {
      window.location.href="http://vip.jfyf.com/lzg_game/csdjs/weixin.php";
    },
    jump (num) {
      if (num == 1) {
        this.$refs.scrolldiv.scrollTop = this.$refs.tab1.offsetTop-70
      } else if (num == 2) {
        this.$refs.scrolldiv.scrollTop = this.$refs.tab2.offsetTop-70
      } else if (num == 3) {
        this.$refs.scrolldiv.scrollTop = this.$refs.tab3.offsetTop-70
      } else if (num == 4) {
        this.$refs.scrolldiv.scrollTop = this.$refs.tab4.offsetTop-70
      }
    }
  }
}
</script>

<style scoped>
  #scrolldiv{
    width: 100%;
    height: 100%;
    overflow: hidden;
    /*padding-top: 50px;*/
  }
  .michelinmain{
    /*margin-top: -50px;*/
    background: url("../../static/credit/michelinbg.png") no-repeat;
    background-size: 100% 100%;
  }
  .tab{
    width: 100%;
    display: flex;
    position: absolute;
    top: 10px;
    left: 0;
    z-index: 999;
  }
  .tab a{
    background: url("../../static/credit/michelintaba.png") no-repeat;
    background-size: 100% 100%;
    font-family: SourceHanSerifCN-Heavy;
    font-size: 12px;
    letter-spacing: 4px;
    color: #d0422b;
    margin: 0 5px;
    line-height: 30px;
    flex: 1;
    text-align: center;
    font-weight: bold;
  }
  .tabitem{
    margin: 0 5px 45px 20px;
    height: auto;
    background: url("../../static/credit/michelinitembg.png") no-repeat;
    background-size: 100% 100%;
  }
  .ta img{
    display: block;
    margin: auto;
    /*width: 68%;*/
    width: 250px;
    height: 85px;
    position: relative;
    top: -45px;
  }
  .headbg{
    width: 100%;
    height: auto;
    padding-top: 100px;
  }
  .headbg img{
    display: block;
    width: 68%;
    height:auto;
    margin: auto;
  }
  .itemtxt{
    width: 280px;
    margin: auto;
    margin-top: -20px;
    text-align: center;
  }
  .itemtxt p{
    font-size: 12px;
    line-height: 18px;
    color: #53826a;
  }
  .video{
    display: block;
    width: 300px;
    margin: auto;
  }
  .lis{
    overflow: hidden;
    padding-top: 20px;
    padding-bottom: 40px;
    width: 335px;
    margin: auto;
  }
  .lis1{
    float: left;
    margin-left: 12px;
  }
  .lis .listxt{
    font-size: 15px;
    font-weight: 900;
  }
  .lis p{
    font-size: 12px;
    line-height: 18px;
    color: #53826a;
  }
  .lis img{
    width: 150px;
    height: 210px;
  }
  .tab3{
    position: relative;
    padding-bottom: 40px;
    padding-top: 20px;
  }
  .tab3 img{
    display: block;
    margin: auto;
    margin-bottom: 20px;
  }
  .tab3 a{
    display: block;
    margin: auto;
    width: 150px;
    height: 40px;
    font-size: 16px;
    line-height: 35px;
    letter-spacing: 5px;
    color: #ffffff;
    background: url("../../static/credit/michelinbtn.png") no-repeat;
    background-size: 100% 100%;
    text-align: center;
  }
  .small{
    width: 305px;
    position: absolute;
    top: 0;
    left: 20px;
  }
  .mmm{
    width: 280px;
    height: 190px;
    margin: auto;
    position: relative;
  }
  .tab3 .mybox1{
    position: absolute;
  }
  .video-box{
    width: 85%;
    margin: 0 30px 0 22px;
  }
</style>
<style>
#vux_view_box_body{
  overflow: hidden;
}
</style>
