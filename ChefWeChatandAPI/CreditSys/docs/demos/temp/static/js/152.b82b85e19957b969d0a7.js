webpackJsonp([152],{1749:function(n,t,e){"use strict";var i=e(688),r=e(61),s=e(63);t.a={components:{Spinner:i.a,Cell:s.a,Group:r.a},data:function(){return{types:["android","ios","ios-small","bubbles","circles","crescent","dots","lines","ripple","spiral"]}}}},1750:function(n,t,e){"use strict";var i=function(){var n=this,t=n.$createElement,e=n._self._c||t;return e("div",[e("group",n._l(n.types,function(n,t){return e("cell",{key:n,attrs:{title:n,"inline-desc":3===t?"size=40px":""}},[e("spinner",{attrs:{type:n,size:3===t?"40px":""},slot:"value"})],1)}))],1)},r=[],s={render:i,staticRenderFns:r};t.a=s},378:function(n,t,e){"use strict";Object.defineProperty(t,"__esModule",{value:!0});var i=e(1749),r=e(1750),s=e(0),a=s(i.a,r.a,null,null,null);t.default=a.exports},688:function(n,t,e){"use strict";function i(n){e(689)}var r=e(691),s=e(694),a=e(0),o=i,c=a(r.a,s.a,o,null,null);t.a=c.exports},689:function(n,t,e){var i=e(690);"string"==typeof i&&(i=[[n.i,i,""]]),i.locals&&(n.exports=i.locals);e(227)("74856fa7",i,!0)},690:function(n,t,e){t=n.exports=e(226)(),t.push([n.i,"\n.vux-spinner {\r\n  stroke: #444;\r\n  fill: #444;\r\n  vertical-align: middle;\r\n  display: inline-block;\r\n  width: 28px;\r\n  height: 28px;\n}\n.vux-spinner svg {\r\n  width: 28px;\r\n  height: 28px;\n}\n.vux-spinner.vux-spinner-inverse {\r\n  stroke: #fff;\r\n  fill: #fff;\n}\n.vux-spinner-android {\r\n  stroke: #4b8bf4;\n}\n.vux-spinner-ios, .vux-spinner-ios-small {\r\n  stroke: #69717d;\n}\n.vux-spinner-spiral .stop1 {\r\n  stop-color: #fff;\r\n  stop-opacity: 0;\n}\n.vux-spinner-spiral.vux-spinner-inverse .stop1 {\r\n  stop-color: #000;\n}\n.vux-spinner-spiral.vux-spinner-inverse .stop2 {\r\n  stop-color: #fff;\n}\r\n",""])},691:function(n,t,e){"use strict";var i=e(692),r=["android","ios","ios-small","bubbles","circles","crescent","dots","lines","ripple","spiral"];t.a={name:"spinner",mounted:function(){var n=this;this.$nextTick(function(){Object(i.a)(n.$el,n.type,n.size)})},props:{type:{type:String,default:"ios"},size:String},computed:{styles:function(){if(void 0!==this.size&&"28px"!==this.size)return{width:this.size,height:this.size}},className:function(){for(var n={},t=0;t<r.length;t++)n["vux-spinner-"+r[t]]=this.type===r[t];return n}}}},692:function(n,t,e){"use strict";function i(n,t,e,s,a){var o,u,l,f=document.createElement(c[n]||n);for(o in t)if("[object Array]"===Object.prototype.toString.call(t[o]))for(u=0;u<t[o].length;u++)if(t[o][u].fn)for(l=0;l<t[o][u].t;l++)i(o,t[o][u].fn(l,s),f,s);else i(o,t[o][u],f,s);else r(f,o,t[o]);a&&"28px"!==a&&r(f,"style","width: "+a+"; height: "+a),e.appendChild(f)}function r(n,t,e){n.setAttribute(c[t]||t,e)}function s(n,t){var e=n.split(";"),i=e.slice(t),r=e.slice(0,e.length-i.length);return e=i.concat(r).reverse(),e.join(";")+";"+e[0]}function a(n,t){return(n/=t/2)<1?.5*n*n*n:.5*((n-=2)*n*n+2)}var o=e(693),c=(e.n(o),{a:"animate",an:"attributeName",at:"animateTransform",c:"circle",da:"stroke-dasharray",os:"stroke-dashoffset",f:"fill",lc:"stroke-linecap",rc:"repeatCount",sw:"stroke-width",t:"transform",v:"values"}),u={v:"0,32,32;360,32,32",an:"transform",type:"rotate",rc:"indefinite",dur:"750ms"},l={sw:4,lc:"round",line:[{fn:function(n,t){return{y1:"ios"===t?17:12,y2:"ios"===t?29:20,t:"translate(32,32) rotate("+(30*n+(n<6?180:-180))+")",a:[{fn:function(){return{an:"stroke-opacity",dur:"750ms",v:s("0;.1;.15;.25;.35;.45;.55;.65;.7;.85;1",n),rc:"indefinite"}},t:1}]}},t:12}]},f={android:{c:[{sw:6,da:128,os:82,r:26,cx:32,cy:32,f:"none"}]},ios:l,"ios-small":l,bubbles:{sw:0,c:[{fn:function(n){return{cx:24*Math.cos(2*Math.PI*n/8),cy:24*Math.sin(2*Math.PI*n/8),t:"translate(32,32)",a:[{fn:function(){return{an:"r",dur:"750ms",v:s("1;2;3;4;5;6;7;8",n),rc:"indefinite"}},t:1}]}},t:8}]},circles:{c:[{fn:function(n){return{r:5,cx:24*Math.cos(2*Math.PI*n/8),cy:24*Math.sin(2*Math.PI*n/8),t:"translate(32,32)",sw:0,a:[{fn:function(){return{an:"fill-opacity",dur:"750ms",v:s(".3;.3;.3;.4;.7;.85;.9;1",n),rc:"indefinite"}},t:1}]}},t:8}]},crescent:{c:[{sw:4,da:128,os:82,r:26,cx:32,cy:32,f:"none",at:[u]}]},dots:{c:[{fn:function(n){return{cx:16+16*n,cy:32,sw:0,a:[{fn:function(){return{an:"fill-opacity",dur:"750ms",v:s(".5;.6;.8;1;.8;.6;.5",n),rc:"indefinite"}},t:1},{fn:function(){return{an:"r",dur:"750ms",v:s("4;5;6;5;4;3;3",n),rc:"indefinite"}},t:1}]}},t:3}]},lines:{sw:7,lc:"round",line:[{fn:function(n){return{x1:10+14*n,x2:10+14*n,a:[{fn:function(){return{an:"y1",dur:"750ms",v:s("16;18;28;18;16",n),rc:"indefinite"}},t:1},{fn:function(){return{an:"y2",dur:"750ms",v:s("48;44;36;46;48",n),rc:"indefinite"}},t:1},{fn:function(){return{an:"stroke-opacity",dur:"750ms",v:s("1;.8;.5;.4;1",n),rc:"indefinite"}},t:1}]}},t:4}]},ripple:{f:"none","fill-rule":"evenodd",sw:3,circle:[{fn:function(n){return{cx:32,cy:32,a:[{fn:function(){return{an:"r",begin:-1*n+"s",dur:"2s",v:"0;24",keyTimes:"0;1",keySplines:"0.1,0.2,0.3,1",calcMode:"spline",rc:"indefinite"}},t:1},{fn:function(){return{an:"stroke-opacity",begin:-1*n+"s",dur:"2s",v:".2;1;.2;0",rc:"indefinite"}},t:1}]}},t:2}]},spiral:{defs:[{linearGradient:[{id:"sGD",gradientUnits:"userSpaceOnUse",x1:55,y1:46,x2:2,y2:46,stop:[{offset:.1,class:"stop1"},{offset:1,class:"stop2"}]}]}],g:[{sw:4,lc:"round",f:"none",path:[{stroke:"url(#sGD)",d:"M4,32 c0,15,12,28,28,28c8,0,16-4,21-9"},{d:"M60,32 C60,16,47.464,4,32,4S4,16,4,32"}],at:[u]}]}},d={android:function(n){function t(){if(!e.stop){var n=a(Date.now()-i,650),l=1,f=0,d=188-58*n,p=182-182*n;s%2&&(l=-1,f=-64,d=128- -58*n,p=182*n);var m=[0,-101,-90,-11,-180,79,-270,-191][s];r(u,"da",Math.max(Math.min(d,188),128)),r(u,"os",Math.max(Math.min(p,182),0)),r(u,"t","scale("+l+",1) translate("+f+",0) rotate("+m+",32,32)"),o+=4.1,o>359&&(o=0),r(c,"t","rotate("+o+",32,32)"),n>=1&&(s++,s>7&&(s=0),i=Date.now()),window.requestAnimationFrame(t)}}var e=this;this.stop=!1;var i,s=0,o=0,c=n.querySelector("g"),u=n.querySelector("circle");return function(){return i=Date.now(),t(),e}}};t.a=function(n,t,e){var r,s;r=t;var a=document.createElement("div");return i("svg",{viewBox:"0 0 64 64",g:[f[r]]},a,r,e),n.innerHTML=a.innerHTML,function(){d[r]&&(s=d[r](n)())}(),n}},693:function(n,t){for(var e=0,i=["webkit","moz"],r=0;r<i.length&&!window.requestAnimationFrame;++r)window.requestAnimationFrame=window[i[r]+"RequestAnimationFrame"],window.cancelAnimationFrame=window[i[r]+"CancelAnimationFrame"]||window[i[r]+"CancelRequestAnimationFrame"];window.requestAnimationFrame||(window.requestAnimationFrame=function(n,t){var i=(new Date).getTime(),r=Math.max(0,16-(i-e)),s=window.setTimeout(function(){n(i+r)},r);return e=i+r,s}),window.cancelAnimationFrame||(window.cancelAnimationFrame=function(n){clearTimeout(n)})},694:function(n,t,e){"use strict";var i=function(){var n=this,t=n.$createElement;return(n._self._c||t)("span",{staticClass:"vux-spinner",class:n.className,style:n.styles})},r=[],s={render:i,staticRenderFns:r};t.a=s}});