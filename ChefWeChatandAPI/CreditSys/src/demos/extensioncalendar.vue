<template>
  <div class="extensioncalendar">
  	<div class="top">
  		<p>当年新注册人数：{{currentYearNum}}</p>
  	</div>
  	<div class="main">
  		<div class="year">
  			<span class="l-jt" @click="preMonth"></span>
  			<span class="current-year">{{setYear}}年</span>
  			<span class="r-jt" @click="nextMonth"></span>
  		</div>
  		<div class="month">
  			<ul>
  				<li v-for="(item, index) in monthNumArr">   				
    				<span @click="jump(index+1)" v-if="index+1 == currentMonth" class="current-month">{{ index+1 }}月</span>
    				<span @click="jump(index+1)" v-else>{{ index+1 }}月</span><i>{{ item }}</i>
  				</li>
  			</ul>
  		</div>
  	</div>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { cookie } from 'vux'
export default({
	components: {
		cookie
	},
	data(){
		return {
			userData:'',
			userId: 10003,
			currentYearNum: 123,
			monthNumArr: [],
			currentYear: '',
			currentMonth: '',
			setYear: ''
		}
	},
	methods: {
		preMonth: function() {
			this.setYear --;
			this.getData(this.setYear, 0);
			$('.month').find('li').children('span').removeClass('current-month');
		},
		nextMonth: function() {
			this.currentYear > this.setYear ? this.setYear ++ : '';
			this.getData(this.setYear, 0);
			$('.month').find('li').children('span').removeClass('current-month');
			if(this.currentYear == this.setYear) {
				$('.month').find('li:nth('+ (this.currentMonth - 1) +')').children('span').addClass('current-month');
			}	
		},
		getDate: function() {
			 var date = new Date;
			 var year = date.getFullYear(); 
			 var month = date.getMonth();
			 this.setYear = year;
			 this.currentYear = year;
			 this.currentMonth = month + 1;
			 $('.month').find('li:nth('+ month +')').children('span').addClass('current-month');
			 // 针对年取数据，月传0
			 this.getData(this.currentYear, 0);
		},
		getData: function(year, month) {
			var _this=this;
	  		var params={
	  		  'Month': month,
	  		  'Year': year,
	  		  'SalemanId': this.userId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getSalemanContribute,
	  			data:params
	  		}).then(function(response){  	
	  			var data=JSON.parse(response.data);
	  			_this.currentYearNum = data.YearCount;
	  			var numArr = data.data;
	  			var monthArr = new Array();
	  			monthArr = ['','','','','','','','','','','',''];
	  			for(let i in numArr) {
	  				monthArr[numArr[i].Month - 1] = '+' + numArr[i].Num;
	  			}
	  			_this.monthNumArr = monthArr;
	  			//console.log(_this.monthNumArr);
	  		})
		},
		jump(month){
			sessionStorage.setItem('postYear', this.setYear);
			sessionStorage.setItem('postMonth', month);
			var link = '/component/extensioncalendarmonth'
  			this.$router.push(link);
  		}
	},
	mounted(){
	  	this.userData=cookie.get('WeiXinUser',function(val){
	  		  var a = val.split("&");
					var obj = {};
					for (var i = 0; i < a.length; i++) {
						var b = a[i].split("=");
						obj[b[0]] = b[1];
					}
					return obj;
	  	})  	
	  	this.userId = parseInt(this.userData.UserId);	
		this.getDate();
	}
})
</script>

<style scoped>
	.extensioncalendar { height: 100%; background: #f4f4f4; }
	.extensioncalendar .top { margin-bottom: 10px;  background: #fff; }
	.extensioncalendar .top p { line-height: 80px; color: #E83428; text-indent: 20px; font-size: 18px; }
	.extensioncalendar .main { background: #fff; }
	.extensioncalendar .main .year { margin: 0 20px; padding: 20px 0; border-bottom: 1px solid #E83428; overflow: hidden; }
	.extensioncalendar .main .year span { display: block; width: 30px; height: 30px; float: left; }
	.extensioncalendar .main .year span.l-jt { width: 20%; background: url('../../static/credit/l_jt.png') no-repeat center; background-size: 25px;  }
	.extensioncalendar .main .year span.r-jt { width: 20%; background: url('../../static/credit/r_jt.png') no-repeat center; background-size: 25px;  }
	.extensioncalendar .main .year span.current-year { width: 60%; text-align: center; color: #E83428; font-size: 18px; letter-spacing: .1em;  }
	.extensioncalendar .main .month ul { overflow: hidden; }
	.extensioncalendar .main .month ul li { width: 25%; padding: 15px 0; float: left; list-style: none; font-size: 18px; text-align: center; }
	.extensioncalendar .main .month ul li span { display: block; height: 43px; width: 43px; line-height: 43px; border-radius: 50%; margin: 0 auto; }
	.extensioncalendar .main .month ul li span.current-month { background: #E83428; color: #fff; }
	.extensioncalendar .main .month ul li i { height: 20px; display: block; font-style: normal; font-size: 14px; color: #E83428; }

</style>
