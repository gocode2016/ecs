using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxScanAPI.Models
{
    public class PayResultModel
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string mch_appid { get; set; }
        public string mchid { get; set; }
        public string device_info { get; set; }
        public string nonce_str { get; set; }
        public string result_code { get; set; }
        public string partner_trade_no { get; set; }
        public string payment_no { get; set; }
        public string payment_time { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
    }

    public class TextModel
    {
        //文本消息内容
        public string Content { get; set; }
    }

    public class ImageModel
    {
        //图片链接
        public string PicUrl { get; set; }
        //图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        public string MediaId { get; set; }
    }

    public class VoiceModel
    {
        //语音格式，如amr，speex等
        public string Format { get; set; }
        //语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        public string MediaId { get; set; }
    }

    public class VideoModel
    {
        //视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        public string ThumbMediaId { get; set; }
        //视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        public string MediaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class LocationModel
    {
        //地理位置维度
        public double Location_X { get; set; }
        //地理位置经度
        public double Location_Y { get; set; }
        //地图缩放大小
        public string Scale { get; set; }
        //地理位置信息
        public string Label { get; set; }
    }

    public class LinkModel
    {
        //消息标题
        public double Title { get; set; }
        //消息描述
        public double Description { get; set; }
        //消息链接
        public string Url { get; set; }
    }

    public class MusicModel
    {
        //音乐标题
        public string Title { get; set; }
        //音乐描述
        public string Description { get; set; }
        //音乐链接
        public string MusicURL { get; set; }
        //高质量音乐链接，WIFI环境优先使用该链接播放音乐
        public string HQMusicUrl { get; set; }
        //缩略图的媒体id，通过上传多媒体文件，得到的id
        public string ThumbMediaId { get; set; }
    }

    public class EventModel
    {
        //事件类型
        public string Event { get; set; }
        //已关注扫描事件
        public EventKeyModel EventKeyModel { get; set; }
        //上传地理位置
        public LocationEventModel LocationEventModel { get; set; }
        //菜单拉取消息时的事件推送
        public ClickEventModel ClickEventModel { get; set; }
    }

    public class EventKeyModel
    {
        //事件KEY值，qrscene_为前缀，后面为二维码的参数值或创建二维码时的二维码scene_id
        public string EventKey { get; set; }
        //二维码的ticket，可用来换取二维码图片
        public string Ticket { get; set; }
    }
    public class LocationEventModel
    {
        //地理位置纬度
        public double Latitude { get; set; }
        //地理位置经度
        public double Longitude { get; set; }
        //地理位置精度
        public double Precision { get; set; }
    }

    public class ClickEventModel
    {
        //事件KEY值，与自定义菜单接口中KEY值对应
        public string EventKey { get; set; }
        //扫一扫内容
        public string ScanResult { get; set; }
    }

    public class ArticelModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
    }
}