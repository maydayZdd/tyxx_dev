using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 加载界面
/// </summary>
public class LogingView : BaseView
{
    #region 绑定组件

    [AttComInfo("txt_decs")]
    TextMeshProUGUI txt_decs;

    #endregion

    public override void OnInit()
    {
        base.OnInit();

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }


}