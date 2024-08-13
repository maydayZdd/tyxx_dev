using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 创建角色界面
/// </summary>
public class CreateRoleView : UIBaseView
{
    #region 绑定组件

    [AttComInfo("txt_bili")]
    TextMeshProUGUI txt_bili;

    [AttComInfo("txt_shenshi")]
    TextMeshProUGUI txt_shenshi;

    [AttComInfo("txt_tili")]
    TextMeshProUGUI txt_tili;

    [AttComInfo("txt_wuxing")]
    TextMeshProUGUI txt_wuxing;

    [AttComInfo("txt_fuyuan")]
    TextMeshProUGUI txt_fuyuan;

    [AttComInfo("input_name")]
    TMP_InputField input_name;

    [AttComInfo("btn_go")]
    Button btn_go;

    [AttComInfo("btn_rang")]
    Button btn_rang;

    [AttComInfo("btn_tips")]
    Button btn_tips;
    #endregion

    //默认属性点
    private int[] _array = new int[5] {1,1,1,1,1};

    //随机分配的属性点
    private int add_attr = 45;

    public override void OnInit()
    {
        base.OnInit();

        btn_go.onClick.AddListener(() =>
        {


        });

        btn_rang.onClick.AddListener(() =>
        {
            OnFlush(default);

        });

        btn_tips.onClick.AddListener(() =>
        {
            UICtrlView.Instance.OpenView(ViewName.RuleTipsView,1);

        });

    }

    public override void OnFlush(string key)
    {
        RandomAllAttr();
        txt_bili.text = _array[0].ToString();
        txt_shenshi.text = _array[1].ToString();
        txt_tili.text = _array[2].ToString();
        txt_wuxing.text = _array[3].ToString();
        txt_fuyuan.text = _array[4].ToString();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// 随机所有属性
    /// </summary>
    private void RandomAllAttr()
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = 1;
        }

        for (int i = 0; i < add_attr; i++) {
            int randomInt = Random.Range(0, 5);
            _array[randomInt] = _array[randomInt] + 1;
        }
    }




}
