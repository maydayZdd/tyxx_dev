using System;

/// <summary>
/// 用于表示界面组件绑定关系的特性
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class AttComInfo : Attribute
{
    /// <summary>
    /// 对应的标签名称
    /// </summary>
    public readonly string tag;
    public AttComInfo(string tag)
    {
        this.tag = tag;
    }
}