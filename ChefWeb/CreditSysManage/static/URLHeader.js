// var URLHeader ={//正式版
// 	"marketing":"http://kongapi.shinho.net.cn/ecs/activity",//营销	8086
// 	"Integral":"http://kongapi.shinho.net.cn/ecs/integral",//积分管理	8085
// 	"ECActivities":"http://kongapi.shinho.net.cn/ecs/ec",//EC活动 	8083
// 	"ShoppingMall":"http://kongapi.shinho.net.cn/ecs/mall",//积分商城 8084
// 	"Tools":"http://kongapi.shinho.net.cn/ecs/common",//公共类  (例:上传图片)8081
// 	"Loading":'https://s3-010-shinho-ecshool-prd-bjs.s3.cn-north-1.amazonaws.com.cn',//下载图片
// 	"user":"http://kongapi.shinho.net.cn/ecs/user",//用户8082
//   "foodLibrary":'',//菜品库
// }

   var URLHeader ={//测试版
   	"marketing":"http://shkapi4uat.shinho.net.cn/ecs/activity",//营销活动活动8006
   	"Integral":"http://shkapi4uat.shinho.net.cn/ecs/integral",//积分管理8005
   	"ECActivities":"http://10.211.1.249:8003",//EC活动8003   http://192.168.8.35:8003
   	"ShoppingMall":"http://shkapi4uat.shinho.net.cn/ecs/mall",//积分商城8004
   	"Tools":"http://shkapi4uat.shinho.net.cn/ecs/common",//公共类  (例:模板下载,上传图片) :8001
   	"Loading":'https://s3-010-shinho-ecshool-uat-bjs.s3.cn-north-1.amazonaws.com.cn',//下载图片
   	"user":"http://shkapi4uat.shinho.net.cn/ecs/user",//用户8002
     "foodLibrary":'http://10.211.1.249:8087',//菜品库
   }

//UPImage:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Product',//上传插件地址
//UPImageMember:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Member',
//UPImageCook:URLHeader.Tools+'/api/Image/ImgUpload?img_type=Cook',
