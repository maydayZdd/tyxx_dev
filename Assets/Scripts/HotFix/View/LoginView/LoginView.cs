using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : BaseView
{
    #region 绑定组件

    [AttComInfo("txt_version")]
    TextMeshProUGUI txt_version;

    [AttComInfo("btn_login")]
    Button btn_login;

    [AttComInfo("btn_gonggao")]
    Button btn_gonggao;

    [AttComInfo("btn_exit")]
    Button btn_exit;

    [AttComInfo("btn_seting")]
    Button btn_seting;

    #endregion

    public override void OnInit()
    {
        base.OnInit();

        btn_login.onClick.AddListener(() =>
        {


        });

        btn_gonggao.onClick.AddListener(() => {

            Log.Info("开发中..");
        });

        btn_exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        btn_seting.onClick.AddListener(() => {

            Log.Info("开发中..");
        });


    }

    public override void OnFlush(string key)
    {

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
