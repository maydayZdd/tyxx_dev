
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.attr
{
public partial class TbAttribute
{
    private readonly System.Collections.Generic.Dictionary<int, attr.Attribute> _dataMap;
    private readonly System.Collections.Generic.List<attr.Attribute> _dataList;
    
    public TbAttribute(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, attr.Attribute>();
        _dataList = new System.Collections.Generic.List<attr.Attribute>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            attr.Attribute _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = attr.Attribute.DeserializeAttribute(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, attr.Attribute> DataMap => _dataMap;
    public System.Collections.Generic.List<attr.Attribute> DataList => _dataList;

    public attr.Attribute GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public attr.Attribute Get(int key) => _dataMap[key];
    public attr.Attribute this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
