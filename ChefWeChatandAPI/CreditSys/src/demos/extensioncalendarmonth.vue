<template>
  <div class="extensioncalendarmonth">
  	<div class="top">
  		<p class="red">今日注册人数：{{dayNum}}</p>
  		<p class="red">本月注册人数：{{monthNum}}</p>
  		<p class="content">此处新注册人数是指给到客户的注册码的数量，仅作为发展会员的参考数据，会存在会员转移情况，会员总数以【绑定厨师】板块为准。</p>
  	</div>
  	<div class="main">
		<inline-calendar
		  class="inline-calendar-demo"
		  v-model="value"
		  :start-date="startDate"
		  :end-date="endDate"
		  :show-last-month="showLastMonth"
		  :show-next-month="showNextMonth"
		  :highlight-weekend="highlightWeekend"
		  :return-six-rows="return6Rows"
		  :hide-header="hideHeader"
		  :hide-week-list="hideWeekList"
		  :replace-text-list="replaceTextList"
		  :weeks-list="weeksList"
		  :render-function="buildSlotFn"
		  :disable-past="disablePast"
		  :disable-future="disableFuture">
		</inline-calendar>
  	</div>
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import { InlineCalendar, cookie } from 'vux'
export default({
	components: {
		InlineCalendar,
		cookie
	},
	data(){
		return {
			userData:'',
			userId: 10003,
			dayNum: '12',
			monthNum: '47',
			value: '',
			startDate: '',
			endDate: '',
		    showLastMonth: true,
		    showNextMonth: true,
		    highlightWeekend: false,
		    return6Rows: true,
		    hideHeader: false,
		    hideWeekList: false,
		    replaceTextList: {},
		    replace: false,
		    weeksList: ['日', '一', '二', '三', '四', '五', '六 '],
		    buildSlotFn: () => '',
		    disablePast: false,
		    disableFuture: false
		}
	},
	methods: {
		getDate: function() {
			 var date = new Date;
			 if(sessionStorage.getItem('postYear') && sessionStorage.getItem('postMonth')) {
				 var year = sessionStorage.getItem('postYear'); 
				 var month = sessionStorage.getItem('postMonth');
			 }else{
			 	 var year = date.getFullYear(); 
			 	 var month = date.getMonth() + 1;
			 }

			 var day = date.getDate();		
			 month < 10 ? month = '0'+ month : '';
			 day < 10 ? day = '0' + day : '';
			 this.value = year + '-' + month + '-' + day;

			 // startDate
			 var startDate = "2010-01-01"; // 开始日期自定义
			 this.startDate = startDate;
			 // endDate
			 var curYear = date.getFullYear(); 
			 var curMonth = date.getMonth() + 1;
			 curMonth < 10 ? curMonth = '0'+curMonth : '';

			 // 截止日期是设置的当天, 暂时废弃
			 // this.endDate = curYear + '-' + curMonth + '-' + day;

			 // 获取月的天数，将截止日期设为传入月的最后一天
	  		 var days = this.getDays(year, month);
	  		 this.endDate = curYear + '-' + curMonth + '-' + days;
			 // console.log(year, month, curYear, curMonth, days);
			 
			 this.getData(parseInt(year), parseInt(month)); // 获取每天注册人数			
		},
		getDays: function(year, month) {
		    var d = new Date(year, month, 0);
		    return d.getDate();
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
	  			_this.dayNum = data.DayCount;
	  			_this.monthNum = data.MonthCount;//本月注册人数
	  			var dayNumArr = data.data;

	  			// 判断当前传入年月有多少天，构造数组
	  			var days = _this.getDays(year, month);
	  			var dayArr = new Array();
				for (var i=0; i<days; i++) { dayArr[i] = ''; }
	  			for(let i in dayNumArr) {
	  				dayArr[dayNumArr[i].Day - 1] = '+' + dayNumArr[i].Num;
	  			}			

				// 每天注册人数赋值
			    _this.buildSlotFn = (line, index, data) => {			    	
			    	if(data.isNextMonth || data.isLastMonth) {
			        	return '<div style="font-size:12px;text-align:center;"><span style="display:inline-block;color:#E83428;"></span></div>';
			    	}else{
						return '<div style="font-size:12px;text-align:center;"><span style="display:inline-block;color:#E83428;">' + 
dayArr[data.date-1] + '</span></div>';
			    	}

			    };
	  		})
		},
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
//	  	this.userId = 30628;//测试
			this.getDate();
			this.replace = true;
			$('.extensioncalendarmonth').find('.main').find('table').addClass('tr_btm_red');

		var _this = this;
		// 年份更改，调取数据		
		$('.year-prev').parent('span').click(function() {
//			var setYear = parseInt($('.calendar-year-txt').text()) - 1;
			var setYear = parseInt($('.calendar-year-txt').text());
			var setMonth = parseInt($('.calendar-month-txt').text());
			console.log(setYear, setMonth);
			_this.getData(setYear, setMonth); // 获取每天注册人数
		})
		$('.year-next').parent('span').click(function() {
//			var setYear = parseInt($('.calendar-year-txt').text()) + 1;
			var setYear = parseInt($('.calendar-year-txt').text());
			var setMonth = parseInt($('.calendar-month-txt').text());
			console.log(setYear, setMonth);
			_this.getData(setYear, setMonth); // 获取每天注册人数
		})
		// 月份更改，调取数据
		$('.month-prev').parent('span').click(function() {
			var setYear = parseInt($('.calendar-year-txt').text());
//			var setMonth = parseInt($('.calendar-month-txt').text()) - 1;
			var setMonth = parseInt($('.calendar-month-txt').text());
			if(setMonth == 0) { setMonth = 12; setYear --; }
			console.log(setYear, setMonth);
			_this.getData(setYear, setMonth); // 获取每天注册人数	
		})
		$('.month-next').parent('span').click(function() {
			var setYear = parseInt($('.calendar-year-txt').text());
//			var setMonth = parseInt($('.calendar-month-txt').text()) + 1;
			var setMonth = parseInt($('.calendar-month-txt').text());
			if(setMonth == 13) { setMonth = 1; setYear ++; }
			console.log(setYear, setMonth);
			_this.getData(setYear, setMonth); // 获取每天注册人数	
		})

	},
	updated(){
		if($(window).width() < 375) {
			$('.extensioncalendarmonth').find('.inline-calendar').find('td').css('padding', '2px 0 0 0');
		}
	},
	watch: { 
	    replace (val) {
	      this.replaceTextList = val ? { 'TODAY': '今' } : {}
	    }
	}
})
</script>

<style scoped>
	.extensioncalendarmonth { height: 100%; background: #f4f4f4; overflow: hidden; }
	.extensioncalendarmonth .top { padding-top: 15px; margin-bottom: 10px;  background: #fff; overflow: hidden; }
	.extensioncalendarmonth .top p { line-height: 20px; padding: 0 15px; font-size: 14px; }
	.extensioncalendarmonth .top p.red { line-height: 25px; color: #E83428; font-size: 16px; }
	.extensioncalendarmonth .top p.content { margin-bottom: 10px; margin-top: 5px; }
	.extensioncalendarmonth .main { background: #fff; padding-left: 15px; padding-right: 15px; }
	.extensioncalendarmonth .main .inline-calendar { padding-bottom: 5px; padding-top: 10px; }

	@media screen and (max-width: 374px) {
		.extensioncalendarmonth .top { padding-top: 10px; margin-bottom: 5px;  background: #fff; overflow: hidden; }
		.extensioncalendarmonth .top p { line-height: 18px; padding: 0 10px; font-size: 12px; }
		.extensioncalendarmonth .top p.red { line-height: 20px; color: #E83428; font-size: 14px; }
		.extensioncalendarmonth .top p.content { margin-bottom: 5px; margin-top: 0; }
	}

</style>
<style type="text/css">
	.extensioncalendarmonth .vux-prev-icon,
	.extensioncalendarmonth .vux-next-icon { border-bottom: 1px solid #E83428 !important; border-left: 1px solid #E83428 !important;}
	.extensioncalendarmonth .inline-calendar td.is-today { color: #E83428; }
	.extensioncalendarmonth .inline-calendar td.current > span.vux-calendar-each-date { background-color: #E83428 !important; }
	
	table.tr_btm_red thead tr { border-bottom: 1px solid #E83428; }
	table.tr_btm_red thead tr th { padding-bottom: 10px; }
	table.tr_btm_red tbody tr:first-child td { padding-top: 8px !important; }
</style>
