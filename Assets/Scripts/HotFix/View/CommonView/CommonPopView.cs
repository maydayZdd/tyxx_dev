using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIView;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用界面
/// </summary>
public class CommonPopView : UIBaseView
{
    #region 绑定组件
    /// <summary>
    /// 自定义面板大小 TODO
    /// </summary>
    [AttComInfo("game_panl")]
    GameObject game_panl;

    [AttComInfo("txt_title")]
    TextMeshProUGUI txt_title;

    [AttComInfo("btn_ok")]
    Button btn_ok;

    [AttComInfo("btn_cal")]
    Button btn_cal;

    [AttComInfo("txt_conter")]
    TextMeshProUGUI txt_conter;

    #endregion

    private CommonPopViewData commonPopViewData;
    public struct CommonPopViewData
    {
        public string title;
        public string content;
        public Action ok_action;
        public Action cal_action;

        public CommonPopViewData(string _title, string _content, Action _ok_action, Action _cal_action)
        {
            this.title = _title;
            this.content = _content;
            this.ok_action = _ok_action;
            this.cal_action = _cal_action;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        commonPopViewData = (CommonPopViewData)ViewData;

        btn_ok.onClick.AddListener(() =>
        {
            if (commonPopViewData.ok_action != default)
            {
                commonPopViewData.ok_action();
            }


            Close();

        });

        btn_cal.onClick.AddListener(() =>
        {
            if (commonPopViewData.cal_action != default)
            {
                commonPopViewData.cal_action();
            }
            Close();
        });


    }

    public override void OnFlush(string key)
    {
        ;
        txt_title.text = commonPopViewData.title;
        txt_conter.text = commonPopViewData.content;
    }

}