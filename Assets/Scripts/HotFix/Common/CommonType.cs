using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 界面类型
/// </summary>
public enum ViewType
{
    /// <summary>
    /// 主界面类型  (主界面不会走定时销毁逻辑)
    /// </summary>
    Main_View = 1,
    /// <summary>
    /// 普通界面类型
    /// </summary>
    Normal_View = 2,

    /// <summary>
    /// 弹窗界面类型
    /// </summary>
    Pop_View = 3,

    /// <summary>
    /// 顶层界面类型 (这种不管层级关系)
    /// </summary>
    Top_View = 4,

}

/// <summary>
/// 通用颜色
/// </summary>
public class Color2d
{
    /// <summary>
    /// 白色 #ffffff
    /// </summary>
    public static readonly string White = "#ffffff";

    /// <summary>
    /// 绿色 #66f86f
    /// </summary>
    public static readonly string Green = "#66f86f";

    /// <summary>
    /// 蓝色 #4cbef3
    /// </summary>
    public static readonly string Blue = "#4cbef3";

    /// <summary>
    /// 紫色 #be92ff
    /// </summary>
    public static readonly string Purple = "#be92ff";

    /// <summary>
    /// 粉色 #ff6dc6
    /// </summary>
    public static readonly string Pink = "#ff6dc6";

    /// <summary>
    /// 橙色 #ffaa4f
    /// </summary>
    public static readonly string Orange = "#ffaa4f";

    /// <summary>
    /// 红色 #ff5e5e
    /// </summary>
    public static readonly string Red = "#ff5e5e";

    public static string GetQualityColor(int quality)
    {
        string color = White;
        switch (quality)
        {
            case 1:
                color = White;
                break;
            case 2:
                color = Green;
                break;
            case 3:
                color = Blue;
                break;
            case 4:
                color = Purple;
                break;
            case 5:
                color = Pink;
                break;
            case 6:
                color = Orange;
                break;
            case 7:
                color = Red;
                break;
            default:
                Debug.LogError($"找不到对应品质色 请检查{quality}");
                break;
        }

        return color;
    }

    /// <summary>
    /// 获取设置颜色文本的字符串
    /// </summary>
    /// <param name="quality">颜色品质</param>
    /// <param name="center">文本内容</param>
    /// <returns></returns>
    public static string GetColorQualityByStr(int quality, string center)
    {
        return $"<color=#{GetQualityColor(quality)}>{center}</color>";
    }

    /// <summary>
    /// 获取设置颜色文本的字符串
    /// </summary>
    /// <param name="color">颜色</param>
    /// <param name="center">文本内容</param>
    /// <returns></returns>
    public static string GetColorByStr(string color, string center)
    {
        return $"<color=#{color}>{center}</color>";
    }

}

/// <summary>
/// 界面索引
/// </summary>
public class ViewName
{
    /// <summary>
    ///  登录界面
    /// </summary>
    public static readonly string LoginView = "LoginView";

}

