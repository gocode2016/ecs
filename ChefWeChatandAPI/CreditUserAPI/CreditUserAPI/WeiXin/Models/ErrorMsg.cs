using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditUserAPI.WeiXin.Models
{
    public class ErrorMsg
    {
        public int? errcode { get; set; }
        public string errmsg { get; set; }
    }

    public class ErrorManage
    {
        private static Dictionary<int, string> dic;

        #region 赋值

        static ErrorManage()
        {
            dic = new Dictionary<int, string>();

            dic.Add(-1, "系统繁忙 ");
            dic.Add(0, "请求成功 ");
            dic.Add(40001, "获取access_token时AppSecret错误，或者access_token无效 ");
            dic.Add(40002, "不合法的凭证类型 ");
            dic.Add(40003, "不合法的OpenID ");
            dic.Add(40004, "不合法的媒体文件类型");
            dic.Add(40005, "不合法的文件类型 ");
            dic.Add(40006, "不合法的文件大小 ");
            dic.Add(40007, "不合法的媒体文件id");
            dic.Add(40008, "不合法的消息类型");
            dic.Add(40009, "不合法的图片文件大小 ");
            dic.Add(40010, "不合法的语音文件大小");
            dic.Add(40011, "不合法的视频文件大小 ");
            dic.Add(40012, "不合法的缩略图文件大小");
            dic.Add(40013, "不合法的APPID");
            dic.Add(40014, "不合法的access_token");
            dic.Add(40015, "不合法的菜单类型 ");
            dic.Add(40016, "不合法的按钮个数 ");
            dic.Add(40017, "不合法的按钮个数 ");
            dic.Add(40018, "不合法的按钮名字长度");
            dic.Add(40019, "不合法的按钮KEY长度");
            dic.Add(40020, "不合法的按钮URL长度 ");
            dic.Add(40021, "不合法的菜单版本号 ");
            dic.Add(40022, "不合法的子菜单级数 ");
            dic.Add(40023, "不合法的子菜单按钮个数 ");
            dic.Add(40024, "不合法的子菜单按钮类型 ");
            dic.Add(40025, "不合法的子菜单按钮名字长度 ");
            dic.Add(40026, "不合法的子菜单按钮KEY长度 ");
            dic.Add(40027, "不合法的子菜单按钮URL长度 ");
            dic.Add(40028, "不合法的自定义菜单使用用户 ");
            dic.Add(40029, "不合法的oauth_code ");
            dic.Add(40030, "不合法的refresh_token");
            dic.Add(40031, "不合法的openid列表 ");
            dic.Add(40032, "不合法的openid列表长度 ");
            dic.Add(40033, "不合法的请求字符，不能包含\\uxxxx格式的字符 ");
            dic.Add(40035, "不合法的参数");
            dic.Add(40038, "不合法的请求格式");
            dic.Add(40039, "不合法的URL长度 ");
            dic.Add(40050, "不合法的分组id ");
            dic.Add(40051, "分组名字不合法 ");
            dic.Add(41001, "缺少access_token参数");
            dic.Add(41002, "缺少appid参数 ");
            dic.Add(41003, "缺少refresh_token参数 ");
            dic.Add(41004, "缺少secret参数 ");
            dic.Add(41005, "缺少多媒体文件数据 ");
            dic.Add(41006, "缺少media_id参数 ");
            dic.Add(41007, "缺少子菜单数据 ");
            dic.Add(41008, "缺少oauth code ");
            dic.Add(41009, "缺少openid ");
            dic.Add(42001, "access_token超时");
            dic.Add(42002, "refresh_token超时");
            dic.Add(42003, "oauth_code超时 ");
            dic.Add(43001, "需要GET请求");
            dic.Add(43002, "需要POST请求");
            dic.Add(43003, "需要HTTPS请求");
            dic.Add(43004, "需要接收者关注 ");
            dic.Add(43005, "需要好友关系");
            dic.Add(44001, "多媒体文件为空 ");
            dic.Add(44002, "POST的数据包为空 ");
            dic.Add(44003, "图文消息内容为空");
            dic.Add(44004, "文本消息内容为空 ");
            dic.Add(45001, "多媒体文件大小超过限制");
            dic.Add(45002, "消息内容超过限制 ");
            dic.Add(45003, "标题字段超过限制 ");
            dic.Add(45004, "描述字段超过限制 ");
            dic.Add(45005, "链接字段超过限制 ");
            dic.Add(45006, "图片链接字段超过限制 ");
            dic.Add(45007, "语音播放时间超过限制 ");
            dic.Add(45008, "图文消息超过限制 ");
            dic.Add(45009, "接口调用超过限制 ");
            dic.Add(45010, "创建菜单个数超过限制 ");
            dic.Add(45015, "回复时间超过限制 ");
            dic.Add(45016, "系统分组，不允许修改 ");
            dic.Add(45017, "分组名字过长 ");
            dic.Add(45018, "分组数量超过上限");
            dic.Add(46001, "不存在媒体数据 ");
            dic.Add(46002, "不存在的菜单版本 ");
            dic.Add(46003, "不存在的菜单数据 ");
            dic.Add(46004, "不存在的用户");
            dic.Add(47001, "解析JSON/XML内容错误");
            dic.Add(48001, "api功能未授权");
            dic.Add(50001, "用户未授权该api");

            //加解密时的错误码
            dic.Add(-40001, "签名验证错误");
            dic.Add(-40002, "xml解析失败");
            dic.Add(-40003, "sha加密生成签名失败");
            dic.Add(-40004, "AESKey 非法");
            dic.Add(-40005, "appid 校验错误");
            dic.Add(-40006, "AES 加密失败");
            dic.Add(-40007, "AES 解密失败");
            dic.Add(-40008, "公众平台发送的xml不合法");
            dic.Add(-40009, "base64加密异常");
            dic.Add(-40010, "base64解密异常");
            dic.Add(-40011, "公众帐号生成回包xml失败");
        }

        #endregion

        /// <summary>
        /// 获取错误信息
        /// </summary>
        public static string GetErrorInfo(int errcode)
        {
            string errmsg = "";

            try
            {
                errmsg = dic[errcode];
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(errmsg))
            {
                errmsg = dic[0];
            }

            return errmsg;
        }
    }

}
