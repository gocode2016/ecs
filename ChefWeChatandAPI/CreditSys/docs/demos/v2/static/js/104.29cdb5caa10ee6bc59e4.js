webpackJsonp([104],{1865:function(e,t,n){var i=n(1866);"string"==typeof i&&(i=[[e.i,i,""]]),i.locals&&(e.exports=i.locals);n(227)("a449041a",i,!0)},1866:function(e,t,n){t=e.exports=n(226)(),t.push([e.i,"\n.align-middle[data-v-b693c802] {\r\n  text-align: center;\n}\r\n",""])},1867:function(e,t,n){"use strict";var i=n(61),r=n(63),a=n(659),o=n(664),s=n(533);t.a={components:{Marquee:a.a,MarqueeItem:o.a,Group:i.a,Cell:r.a,Divider:s.a},mounted:function(){var e=this;setTimeout(function(){e.asyncCount=5},1e3)},methods:{onClick:function(e){console.log(e)}},data:function(){return{asyncCount:0}}}},1868:function(e,t,n){"use strict";var i=function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("div",[n("divider",[e._v(e._s(e.$t("Default usage")))]),e._v(" "),n("marquee",e._l(5,function(t){return n("marquee-item",{key:t,staticClass:"align-middle",nativeOn:{click:function(n){e.onClick(t)}}},[e._v("hello world "+e._s(t))])})),e._v(" "),n("br"),e._v(" "),n("divider",[e._v(e._s(e.$t("Used in a cell")))]),e._v(" "),n("group",[n("cell",{attrs:{title:e.$t("News")}},[n("marquee",e._l(5,function(t){return n("marquee-item",{key:t,nativeOn:{click:function(n){e.onClick(t)}}},[e._v(e._s(e.$t("JavaScript is the best language"))+" "+e._s(t))])}))],1)],1),e._v(" "),n("br"),e._v(" "),n("divider",[e._v(e._s(e.$t("Async data")))]),e._v(" "),n("marquee",e._l(e.asyncCount,function(t){return n("marquee-item",{key:t,staticClass:"align-middle",nativeOn:{click:function(n){e.onClick(t)}}},[e._v("hello world "+e._s(t))])}))],1)},r=[],a={render:i,staticRenderFns:r};t.a=a},410:function(e,t,n){"use strict";function i(e){n(1865)}Object.defineProperty(t,"__esModule",{value:!0});var r=n(1867),a=n(1868),o=n(0),s=i,u=o(r.a,a.a,s,"data-v-b693c802",null);t.default=u.exports},533:function(e,t,n){"use strict";function i(e){n(534)}var r=n(536),a=n(537),o=n(0),s=i,u=o(r.a,a.a,s,null,null);t.a=u.exports},534:function(e,t,n){var i=n(535);"string"==typeof i&&(i=[[e.i,i,""]]),i.locals&&(e.exports=i.locals);n(227)("30b9f0ea",i,!0)},535:function(e,t,n){t=e.exports=n(226)(),t.push([e.i,"\n.vux-divider {\r\n  display: table;\r\n  white-space: nowrap;\r\n  height: auto;\r\n  overflow: hidden;\r\n  line-height: 1;\r\n  text-align: center;\r\n  padding: 10px 0;\r\n  color: #666;\n}\n.vux-divider:after,.vux-divider:before {\r\n  content: '';\r\n  display: table-cell;\r\n  position: relative;\r\n  top: 50%;\r\n  width: 50%;\r\n  background-repeat: no-repeat;\r\n  background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABaAAAAACCAYAAACuTHuKAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyFpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNS1jMDE0IDc5LjE1MTQ4MSwgMjAxMy8wMy8xMy0xMjowOToxNSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo1OThBRDY4OUNDMTYxMUU0OUE3NUVGOEJDMzMzMjE2NyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDo1OThBRDY4QUNDMTYxMUU0OUE3NUVGOEJDMzMzMjE2NyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjU5OEFENjg3Q0MxNjExRTQ5QTc1RUY4QkMzMzMyMTY3IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjU5OEFENjg4Q0MxNjExRTQ5QTc1RUY4QkMzMzMyMTY3Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+VU513gAAADVJREFUeNrs0DENACAQBDBIWLGBJQby/mUcJn5sJXQmOQMAAAAAAJqt+2prAAAAAACg2xdgANk6BEVuJgyMAAAAAElFTkSuQmCC)\n}\n.vux-divider:before {\r\n  background-position: right 1em top 50%\n}\n.vux-divider:after {\r\n  background-position: left 1em top 50%\n}\r\n",""])},536:function(e,t,n){"use strict";t.a={name:"divider"}},537:function(e,t,n){"use strict";var i=function(){var e=this,t=e.$createElement;return(e._self._c||t)("p",{staticClass:"vux-divider"},[e._t("default")],2)},r=[],a={render:i,staticRenderFns:r};t.a=a},659:function(e,t,n){"use strict";function i(e){n(660)}var r=n(662),a=n(663),o=n(0),s=i,u=o(r.a,a.a,s,null,null);t.a=u.exports},660:function(e,t,n){var i=n(661);"string"==typeof i&&(i=[[e.i,i,""]]),i.locals&&(e.exports=i.locals);n(227)("5b1a623f",i,!0)},661:function(e,t,n){t=e.exports=n(226)(),t.push([e.i,"\n.vux-marquee {\n  width: 100%;\n  overflow: hidden;\n}\n.vux-marquee-box {\n  padding: 0;\n  margin: 0;\n  width: 100%;\n  height: auto;\n}\n.vux-marquee-box li {\n  margin: 0;\n  width: 100%;\n}\n",""])},662:function(e,t,n){"use strict";t.a={name:"marquee",props:{interval:{type:Number,default:2e3},duration:{type:Number,default:300},direction:{type:String,default:"up"}},beforeDestroy:function(){this.destroy()},data:function(){return{currenTranslateY:0,height:"",length:0,currentIndex:0,noAnimate:!1}},methods:{destroy:function(){this.timer&&clearInterval(this.timer)},init:function(){this.destroy(),this.cloneNode&&this.$refs.box.removeChild(this.cloneNode),this.cloneNode=null;var e=this.$refs.box.firstElementChild;return!!e&&(this.length=this.$refs.box.children.length,this.height=e.offsetHeight,"up"===this.direction?(this.cloneNode=e.cloneNode(!0),this.$refs.box.appendChild(this.cloneNode)):(this.cloneNode=this.$refs.box.lastElementChild.cloneNode(!0),this.$refs.box.insertBefore(this.cloneNode,e)),!0)},start:function(){var e=this;"down"===this.direction&&this.go(!1),this.timer=setInterval(function(){"up"===e.direction?(e.currentIndex+=1,e.currenTranslateY=-e.currentIndex*e.height):(e.currentIndex-=1,e.currenTranslateY=-(e.currentIndex+1)*e.height),e.currentIndex===e.length?setTimeout(function(){e.go(!0)},e.duration):-1===e.currentIndex?setTimeout(function(){e.go(!1)},e.duration):e.noAnimate=!1},this.interval+this.duration)},go:function(e){this.noAnimate=!0,e?(this.currentIndex=0,this.currenTranslateY=0):(this.currentIndex=this.length-1,this.currenTranslateY=-(this.currentIndex+1)*this.height)}}}},663:function(e,t,n){"use strict";var i=function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("div",{staticClass:"vux-marquee",style:{height:e.height+"px"}},[n("ul",{ref:"box",staticClass:"vux-marquee-box",style:{transform:"translate3d(0,"+e.currenTranslateY+"px,0)",transition:"transform "+(e.noAnimate?0:e.duration)+"ms"}},[e._t("default")],2)])},r=[],a={render:i,staticRenderFns:r};t.a=a},664:function(e,t,n){"use strict";var i=n(665),r=n(666),a=n(0),o=a(i.a,r.a,null,null,null);t.a=o.exports},665:function(e,t,n){"use strict";t.a={name:"marquee-item",mounted:function(){var e=this;this.$nextTick(function(){e.$parent.destroy(),e.$parent.init(),e.$parent.start()})}}},666:function(e,t,n){"use strict";var i=function(){var e=this,t=e.$createElement;return(e._self._c||t)("li",[e._t("default")],2)},r=[],a={render:i,staticRenderFns:r};t.a=a}});