<template>
  <div class="lotteryshow">
    <div class="lotteryhead">
      <img src="../../../../static/image/lottery/logo.png" alt="">
      <span>助力厨师成长</span>
    </div>
    <div class="lotterymain">
    	
    	<div class="lotteryleft">
        <ul>
          <li v-for="(item,index) in showarr" :key="index" v-if="index<7">
            <img :src="item.HeadImgUrl" alt=""  @error="errorLoadImg">
            <p>{{item.MemberTelePhone}}</p>
          </li>
          <li style="float: right;margin-right: 35px;">
            <img :src="showarr[7].HeadImgUrl"  alt="" :onerror="errorimg" v-if="showarr[7]">
            <p>{{showarr[7]?showarr[7].MemberTelePhone:''}}</p>
          </li>
          <div style="width: 815px;height:125px;float: right">
            <li>
              <img :src="showarr[17].HeadImgUrl"  alt="" :onerror="errorimg" v-if="showarr[17]">
              <p>{{showarr[17]?showarr[17].MemberTelePhone:''}}</p>
            </li>
          </div>
          <li style="float: right;margin-right: 35px;">
            <img :src="showarr[8].HeadImgUrl"  alt="" :onerror="errorimg" v-if="showarr[8]">
            <p>{{showarr[8]?showarr[8].MemberTelePhone:''}}</p>
          </li>
          <div style="width: 815px;height:125px;float: right;">
            <li>
              <img :src="showarr[16].HeadImgUrl"  alt="" :onerror="errorimg" v-if="showarr[16]">
              <p>{{showarr[16]?showarr[16].MemberTelePhone:''}}</p>
            </li>
          </div>
          <div style="margin-right: 10px;">
	          <li v-for="(item,index) in showarr" :key="index" v-if="8<index && index<16" style="float: right;">
	            <img :src="item.HeadImgUrl" alt="">
	            <p>{{item.MemberTelePhone}}</p>
	          </li>
          </div>
        </ul>
        <div class="btnstart" @click="start">{{starttit?'开始抽奖':'结束抽奖'}}</div>
        <div style="display: none">{{test}}</div>
        <!--<div class="btnbottom">助力厨师成长第十期抽奖</div>-->
        <div class="btntit"></div>
      </div>
      <div class="lotteryright">
        <div class="title">
          <img src="../../../../static/image/lottery/lottery_icon.png" alt="">奖品
        </div>
        <div class="leftitem" :class="drawPrizeNum=='一'?'actitem':''">
          <img src="../../../../static/image/lottery/cook_draw1.png" alt="" style="width: 75px">
          <p>PANGAO劲椎按摩器</p>
          <p>一等奖</p>
        </div>
        <div class="leftitem" :class="drawPrizeNum=='二'?'actitem':''">
          <img src="../../../../static/image/lottery/cook_draw2.png" alt="" style="width: 75px">
          <p>shinho保温杯</p>
          <p>二等奖</p>
        </div>
        <div class="leftitem" :class="drawPrizeNum=='三'?'actitem':''">
          <img src="../../../../static/image/lottery/cook_draw3.png" alt="" style="width: 75px">
          <p>shinho商务签字笔</p>
          <p>三等奖</p>
        </div>
        <div class="btn" @click="showbtn">选择抽奖类别 <i class="ivu-icon ivu-icon-arrow-down-b"></i></div>
        <div class="btnshade" v-if="btn" @click="">
          <ul>
            <li v-for="(item, index) in Drawarr" :key="index" @click="setdrawPrizeId(item)"><p>{{item.DrawName}}</p></li>
          </ul>
        </div>
      </div>
      
    </div>
    <!--<div class="end btnstart" @click="endstart">结束抽奖</div>-->
    <div class="shade" v-if="shade">
      <div class="pop">
        <img src="../../../../static/image/lottery/close.png" alt="" class="close" @click="closeshade">
        <div class="luckitem">
          <li v-for="(item,index) in winningarr" :key="index">
            <img :src="item.HeadImgUrl" alt="" class="itemimg">
            <img src="../../../../static/image/lottery/king.png" alt="" class="king">
            <p>{{item.MemberTelePhone}}</p>
          </li>
        </div>
        <div class="poptit">恭喜中奖，快去领奖台领奖吧！</div>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    name: 'lotteryshow',
    data() {
      return {
        errorimg: '../../../static/image/HeadPortrait.png',
        times: null,
        shade: false,
        btn: false,
        starttit: true,
        host:'http://kongapi.shinho.net.cn/ecs/common/api/',
        host2:'http://kongapi.shinho.net.cn/ecs/user/api/',
        APIURL:{
          select: 'DrawPrize/GetPrizeList',
          userapi: 'Member/GetMemberByActivity',
          winning: 'MemberPrize/GetMemberByDrawPrizeId',
          endapi: 'SendWeChatMsg/ExcuteMemberPrize'
        },
        Drawarr: [],
        userarr: [],
        showarr: [],
        winningarr: [],
        drawPrizeId: null,
        PrizeName: null,
        test:0,
        startnum:18,
        showstartnum:0,
        drawPrizeNum:null

      }
    },
    mounted() {
      var self = this
      document.title = '抽奖'
      /*
      * 获取抽奖类别
      */
      $.ajax({
        type :"get",
        url :self.host+self.APIURL.select,
        dataType : 'json',
        contentType: "application/json; charset=utf-8",
        data:{},
        success : function(json) {
          self.Drawarr = JSON.parse(json)
          console.log(self.Drawarr);
        },
        error : function(error) {
          console.log(error)
        }
      });
      /*
      * 获取所有用户
      */
      $.ajax({
        type :"post",
        url :self.host2+self.APIURL.userapi,
        dataType : 'json',
        contentType: "application/json; charset=utf-8",
        data:{},
        success : function(json) {
          self.userarr = JSON.parse(json)
          for(let i in self.userarr){
          	var userarr = self.userarr[i];
          	userarr.MemberTelePhone = userarr.MemberTelePhone.substr(0,3) + '****' + userarr.MemberTelePhone.substr(7);
	        }
          self.userarr.forEach(function (v,k) {
            if (k<18){
              self.showarr.push(v)
            }
          })
          console.log(self.showarr)
        },
        error : function(error) {
          console.log(error)
        }
      });

    },
    methods: {
      closeshade () {
        this.shade = false
      },
      /*
      * 关闭显示抽奖类别
      */
      showbtn () {
        if(this.btn){
          this.btn = false
        }else {
          this.btn = true
        }
      },
      errorLoadImg (event) {
        event.target.classList.add("default-image");;
      },
      /*
      * 设置抽奖id
      */
      setdrawPrizeId (item) {
        this.drawPrizeId = item.DrawPrizeId
        this.PrizeName = item.PrizeName
        this.drawPrizeNum=item.DrawName.split('')[0]
        var self=this
        $.ajax({
          type :"post",
          url :self.host+self.APIURL.winning+'?drawPrizeId='+self.drawPrizeId,
          dataType : 'json',
          contentType: "application/json; charset=utf-8",
          success : function(json) {
            self.winningarr = JSON.parse(json)
            for(let i in self.winningarr){
	          	var winningarr = self.winningarr[i];
	          	winningarr.MemberTelePhone = winningarr.MemberTelePhone.substr(0,3) + '****' + winningarr.MemberTelePhone.substr(7);
		        }
          },
          error : function(error) {
            console.log(error)
          }
        });
        this.showbtn ()
      },
      /*
      * 开始抽奖
      */
      start () {
        // if (this.drawPrizeId) {
          var self = this
          if (this.starttit) {
            this.starttit = false
            this.times = setInterval(() => {
              if (self.startnum >= self.userarr.length) {
                self.startnum = 0
              }
              if (self.showstartnum > 18) {
                self.showstartnum = 0
              }
              self.showarr[self.showstartnum] = self.userarr[self.startnum]
              self.test = self.startnum
              self.showstartnum++
              self.startnum++
            }, 100)
          } else {
            $.ajax({
              type :"post",
              url :self.host2+self.APIURL.endapi,
              dataType : 'json',
              contentType: "application/json; charset=utf-8",
              data:JSON.stringify({DrawPrizeId: self.drawPrizeId,PrizeName: self.PrizeName}),
              success : function(json) {
                self.winningarr = JSON.parse(json)
                for(let i in self.winningarr){
			          	var winningarr = self.winningarr[i];
			          	winningarr.MemberTelePhone = winningarr.MemberTelePhone.substr(0,3) + '****' + winningarr.MemberTelePhone.substr(7);
				        }
              },
              error : function(error) {
                console.log(error)
              }
            });
            this.starttit = true
            this.shade = true
            window.clearInterval(this.times)
            self.showstartnum=0
            self.drawPrizeId=null
          }

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
    min-width: 1300px;
    background: url("../../../../static/image/lottery/lotteryshow_back.png") #e0292e;
    background-size: 100% 100%;
    overflow: hidden;
  }
  .lotteryhead{
    text-align: center;
    line-height: 60px;
    height: 57px;
    font-size: 50px;
    color: white;
    letter-spacing: 2px;
    margin-top: 20px;
    font-weight: bold;
  }
  .lotteryhead img{
    vertical-align: middle;
    display: inline-block;
    width: 67px;
    height: 51px;
  }
  .lotterymain{
    width: 1250px;
    margin: auto;
    overflow: hidden;
    margin-top: 50px;
  }
  .lotteryright{
    position: relative;
    float: left;
    width: 210px;
    height: 550px;
    background-color: #fff8ce;
    border-radius: 10px;
  }
  .title img{
    vertical-align:middle;
    margin-top: -3px;
    margin-right: 10px;
  }
  .title{
  	border-radius: 5px 5px 0 0 ;
    width: 100%;
    height: 50px;
    background: #ffdd7b;
    color: #ff7e20;
    text-align: center;
    font-size: 20px;
    line-height: 50px;
  }
  .leftitem p{
    font-size: 16px;
    margin: 5px 0;
  }
  .leftitem p span{
    font-weight: bold;
  }
  .leftitem{
    text-align: center;
    padding: 10px;
  }
  .btn{
    cursor: pointer;
    border-top: 1px solid #ff7e20;
    font-size: 18px;
    color: #ff7e20;
    text-align: center;
    line-height: 40px;
    font-weight: bold;
    margin-top: 20px;
  }
  .btnshade li p{
    cursor: pointer;
  }
  .btnshade li{
    text-align: center;
    line-height: 30px;
    color: #ff7e22;
  }
  .btnshade{
    z-index: 999;
    background-color: rgba(255,248,206,0.7);
    border-radius: 10px;
    position: absolute;
    left: -160px;
    bottom: 0;
    width: 160px;
  }
  .lotteryleft{
    position: relative;
    width: 1000px;
    float: left;
    height: 550px;
    background: url('../../../../static/image/lottery/lottery_mainbg.png') no-repeat center;
    background-size: 100% 100%;
    border-radius: 10px;
    padding: 20px 20px;
    margin-right: 3%;
  }
  .lotteryleft li{
    width: 85px;
    height: 105px;
    float: left;
    margin: 10px 25px;
  }
  .lotteryleft li p{
    width: 100%;
    /*overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;*/
    font-size: 14px;
    text-align: center;
    line-height: 28px;
    letter-spacing: 1px;
    color: #ffffff;
  }
  .lotteryleft li img{
    display: block;
    width: 85px;
    height: 85px;
    border-radius: 50%;
    background: url("../../../../static/image/HeadPortrait.png") no-repeat;
    background-size: 100% 100%;
  }
  .btnstart{
    position: absolute;
    top: 240px;
    left: 50%;
    width: 180px;
    height: 70px;
    cursor: pointer;
    line-height: 70px;
    margin-left: -90px;
    font-size: 20px;
    text-align: center;
    color: #d20031;
    font-weight: bold;
    letter-spacing: 4px;
    border-radius: 5px;
    background: linear-gradient(to bottom,rgba(255,255,255,1) 0%,rgba(251,186,43,1) 50%,rgba(253,170,14,1)100%);
  }
  .btnbottom{
    width: 250px;
    position: absolute;
    text-align: center;
    font-size: 18px;
    color: white;
    font-weight: bold;
    top: 270px;
    left: 50%;
    margin-left: -125px;
  }
  .shade{
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.3);
    position: fixed;
    top: 0;
    left: 0;
  }
  .pop{
    width: 500px;
    height: auto;
    min-height: 280px;
    background: white;
    border-radius: 5px;
    position: absolute;
    top: 100px;
    left: 50%;
    margin-left: -250px;
  }
  .poptit{
    width: 100%;
    font-weight: bold;
    font-size: 24px;
    color: #e0292e;
    text-align: center;
    position: absolute;
    bottom: 20px;
  }
  .luckitem{
    padding: 10px 20px;
  }
  .luckitem{
    text-align: center;
    padding-top: 40px;
    padding-bottom: 60px;
  }
  .luckitem li{
    position: relative;
    display: inline-block;
    margin: 10px 16px;
  }
  .luckitem li p{
    color: #020202;
    font-size: 16px;
    width: 100%;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    text-align: center;
  }
  .luckitem li .itemimg{
    width: 80px;
    height: 80px;
    border-radius: 50%;
  }
  .king{
    width: 40px;
    height: 40px;
    position: absolute;
    top: -20px;
    right: -11px;
  }
  .close{
    width: 35px;
    height: 35px;
    cursor: pointer;
    position: absolute;
    right: 0;
    top: -21px;
  }
  .default-image{
    background: url("../../../../static/image/HeadPortrait.png") no-repeat;
    background-size: 100% 100%;
  }
  .end{
    position: relative;
    top: 0;
    left: 0;
    margin: 0;
    float: right;
    margin-top: 10px;
  }
  .actitem{
    border: yellow solid 1px;
  }
</style>
<style>
  body,html,#app{
    width: 100%;
    height: 100%;
  }
</style>
