using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

/// <summary>
/// UI Core 管理器
/// </summary>
public class UICore
{
    // 记录面板各层级显示状态
    public Dictionary<ViewType, Dictionary<string, UIBaseView>> dic_all_panel = new Dictionary<ViewType, Dictionary<string, UIBaseView>>();

    //界面销毁倒计时状态
    public Dictionary<string, CancellationTokenSource> dic_cancell_token = new Dictionary<string, CancellationTokenSource>();

    /// <summary>
    /// UIRoot 根节点
    /// </summary>
    public GameObject UIRoot = default;

    /// <summary>
    /// 初始化
    /// </summary>
    public UICore()
    {
        Log.Info("UICore Init...");
        UIRoot = GameObject.Find("UIRoot");

        dic_all_panel.Add(ViewType.Main_View, new Dictionary<string, UIBaseView>());
        dic_all_panel.Add(ViewType.Normal_View,new Dictionary<string, UIBaseView>());
        dic_all_panel.Add(ViewType.Pop_View, new Dictionary<string, UIBaseView>());
        dic_all_panel.Add(ViewType.Top_View, new Dictionary<string, UIBaseView>());
    }


    private GameObject GetSingleObject(UIType uIType)
    {
        AssetHandle handle = YooAssets.LoadAssetSync<GameObject>(uIType.Path);
        GameObject gameObject = handle.AssetObject as GameObject;
        return GameObject.Instantiate(gameObject, GetViewTypeParentTrans(uIType.ViewType));
    }

    private RectTransform GetViewTypeParentTrans(ViewType viewType)
    {
        RectTransform parent_trans = default;

        if (viewType == ViewType.Main_View)
        {
            parent_trans = UIRoot.transform.Find("UIMain").GetComponent<RectTransform>(); ;
        }
       
        if (viewType == ViewType.Normal_View)
        {
            parent_trans = UIRoot.transform.Find("UINormal").GetComponent<RectTransform>(); ;
        }

        if (viewType == ViewType.Pop_View)
        {
            parent_trans = UIRoot.transform.Find("UIPop").GetComponent<RectTransform>();
        }

        if (viewType == ViewType.Top_View)
        {
            parent_trans = UIRoot.transform.Find("UITop").GetComponent<RectTransform>();
        }
        return parent_trans;
    }
    
    public void Open(UIType uIType,object userData)
    {
        Log.Info($"打开界面 {uIType.GuidName}");
        //先判断是否还存在
        UIBaseView basePanel = default;
        if (dic_all_panel[uIType.ViewType].ContainsKey(uIType.GuidName))
        {
            basePanel = dic_all_panel[uIType.ViewType][uIType.GuidName];
            CancelCountDownDestroy(basePanel);
        }
        else
        {
            //用反射来加载脚本
            Type type = Type.GetType(uIType.GuidName);
            basePanel = Activator.CreateInstance(type) as UIBaseView;
            basePanel.SetData(GetSingleObject(uIType), uIType, userData);
            dic_all_panel[uIType.ViewType].Add(basePanel.UIType.GuidName, basePanel);
            basePanel.OnInit();

        }

        //层级排序
        RectTransform parent_transform = GetViewTypeParentTrans(uIType.ViewType);
        RectTransform[] children = parent_transform.GetComponentsInChildren<RectTransform>();
        int max_siblingIndex = children.Max(c => c.GetSiblingIndex());
        basePanel.ActiveObj.GetComponent<RectTransform>().SetSiblingIndex(max_siblingIndex);
        basePanel.OnEnable();
        basePanel.OnFlush("all");

        /*        // 通知各层状态
                foreach (var dic in dic_all_panel)
                {
                    Dictionary<string, BasePanel> keyValuePairs = dic.Value as Dictionary<string, BasePanel>;
                    foreach (var keyValue in keyValuePairs)
                    {
                        if (keyValue.Key != uIType.Name)
                        {
                            keyValue.Value.OnDisable();
                            if (dic.Key == ViewType.Normal_View)
                            {
                                CountDownDestroy(keyValue.Value).Forget();
                            }
                        }
                    }

                }*/
    }

    /// <summary>
    /// 倒计时销毁实体
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid CountDownDestroy(UIBaseView basePanel)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        dic_cancell_token.Add(basePanel.UIType.GuidName, cancellationTokenSource);
        try
        {
            int time_count = GetDownDestroyTimeer(basePanel.UIType.ViewType);

            await UniTask.Delay(TimeSpan.FromSeconds(time_count), cancellationToken: cancellationTokenSource.Token);

            dic_cancell_token.Remove(basePanel.UIType.GuidName);
            string name_key = basePanel.UIType.GuidName;
            basePanel.OnDestroy();
            GameObject ui_basePanel = dic_all_panel[basePanel.UIType.ViewType][name_key].ActiveObj;
            if (ui_basePanel != default)
            {
                GameObject.Destroy(ui_basePanel);
            }
            dic_all_panel[basePanel.UIType.ViewType].Remove(name_key);
            Log.Info($"销毁界面实例：{name_key}");
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("界面销毁中断..");
        }

    }

    public int GetDownDestroyTimeer(ViewType viewType)
    {
        int time_count = 10;
        if (viewType == ViewType.Pop_View || viewType == ViewType.Top_View)
        {
            time_count = 5;
        }
        return time_count;
    }

    /// <summary>
    /// 取消销毁实例
    /// </summary>
    /// <param name="basePanel"></param>
    private void CancelCountDownDestroy(UIBaseView basePanel)
    {
        if (dic_cancell_token.ContainsKey(basePanel.UIType.GuidName))
        {
            CancellationTokenSource cancellationTokenSource = dic_cancell_token[basePanel.UIType.GuidName];
            cancellationTokenSource.Cancel();
            dic_cancell_token.Remove(basePanel.UIType.GuidName);
        }
    }

    public void CloseView(string guid_name,ViewType viewType) 
    {
        Dictionary<string, UIBaseView> guid_base_view = dic_all_panel[viewType];
        if (guid_base_view.ContainsKey(guid_name))
        {
            UIBaseView baseView = guid_base_view[guid_name];
            baseView.OnDisable();
            CountDownDestroy(baseView).Forget();
        }
    }

    public void FlushView(string guid_name, ViewType viewType,string view_key)
    {
        Dictionary<string, UIBaseView> guid_base_view = dic_all_panel[viewType];
        if (guid_base_view.ContainsKey(guid_name))
        {
            UIBaseView baseView = guid_base_view[guid_name];
            baseView.OnFlush(view_key);
        }
    }


}
