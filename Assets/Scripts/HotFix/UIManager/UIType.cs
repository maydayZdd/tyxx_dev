using System.Collections;
using System.Collections.Generic;
public struct UIType
{
    /// <summary>
    /// 界面唯一标识 (不能重复)
    /// </summary>
    public string GuidName { get; private set; }
    public string Path {  get; private set; }
    public ViewType ViewType { get; private set; }

    public UIType(string guid_name, string path, ViewType viewType = ViewType.Normal_View)
    {
        GuidName = guid_name;
        Path = path;
        ViewType = viewType;
    }
}
