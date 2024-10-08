
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.ui
{
public sealed partial class UIView : Luban.BeanBase
{
    public UIView(JSONNode _buf) 
    {
        { if(!_buf["view_name"].IsString) { throw new SerializationException(); }  ViewName = _buf["view_name"]; }
        { if(!_buf["path"].IsString) { throw new SerializationException(); }  Path = _buf["path"]; }
        { if(!_buf["view_type"].IsNumber) { throw new SerializationException(); }  ViewType = _buf["view_type"]; }
        { if(!_buf["desc"].IsString) { throw new SerializationException(); }  Desc = _buf["desc"]; }
    }

    public static UIView DeserializeUIView(JSONNode _buf)
    {
        return new ui.UIView(_buf);
    }

    /// <summary>
    /// 界面唯一标识
    /// </summary>
    public readonly string ViewName;
    /// <summary>
    /// 路径
    /// </summary>
    public readonly string Path;
    /// <summary>
    /// 界面类型
    /// </summary>
    public readonly int ViewType;
    /// <summary>
    /// 描述
    /// </summary>
    public readonly string Desc;
   
    public const int __ID__ = 1966741491;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "viewName:" + ViewName + ","
        + "path:" + Path + ","
        + "viewType:" + ViewType + ","
        + "desc:" + Desc + ","
        + "}";
    }
}

}

