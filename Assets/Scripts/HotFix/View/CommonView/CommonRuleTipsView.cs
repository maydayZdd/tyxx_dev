using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIView;
using UnityEngine;

/// <summary>
/// 说明通用界面
/// </summary>
public class CommonRuleTipsView : UIBaseView
{
    [AttComInfo("txt_title")]
    TextMeshProUGUI txt_title;

    [AttComInfo("txt_conter")]
    TextMeshProUGUI txt_conter;

    public int Id;

    public override void OnInit()
    {
        base.OnInit();
        Id = (int)ViewData;
    }

    public override void OnFlush(string key)
    {
        var config = Data.ConfigData.Instance.Tables.TbRuleTips[Id];
        txt_title.text = config.Title;
        txt_conter.text = config.Desc;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}