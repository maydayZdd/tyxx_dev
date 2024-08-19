using Common;
using Cysharp.Threading.Tasks;
using Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using TMPro;
using UIView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using YooAsset;

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

    [AttComInfo("game_tags")]
    Transform game_tags;

    #endregion

    //默认属性点
    private int[] _array = new int[5] { 1, 1, 1, 1, 1 };  //臂力 神识 体力 悟性 福源

    //随机分配的属性点
    private int add_attr = 45;

    private string render_path = "Assets/Prefabs/Views/CreateRoleView/CreateTagRender.prefab";

    /// <summary>
    /// 标签数据
    /// </summary>
    private List<CreateRoleTagEntity.InitData> roleTag_initData = new List<CreateRoleTagEntity.InitData>();

    /// <summary>
    /// 实体渲染器
    /// </summary>
    private EntityRender<CreateRoleTagEntity> entityRender = new EntityRender<CreateRoleTagEntity>();

    public override void OnInit()
    {
        base.OnInit();
        entityRender.Init(this, render_path,game_tags);
        btn_go.onClick.AddListener(() =>
        {

        });

        btn_rang.onClick.AddListener(() =>
        {
            OnFlush(default);

        });

        btn_tips.onClick.AddListener(() =>
        {
            UICtrlView.Instance.OpenView(ViewName.RuleTipsView, 1);

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

        //回收对象
        for (int i = 0; i < roleTag_initData.Count; i++)
        {
            ReferencePools.Recovery(roleTag_initData[i]);
        }
        roleTag_initData.Clear();


        //计算标签

        int ave = (add_attr + _array.Length) / _array.Length;
        //计算特殊标签
        if (_array[0] == ave && _array[1] == ave && _array[2] == ave && _array[3] == ave && _array[4] == ave)
        {
            CreateRoleTagEntity.InitData initData = ReferencePools.Acquire<CreateRoleTagEntity.InitData>();
            initData.Quality = 7;
            initData.Desc = Language.PingPingWuQi;
            roleTag_initData.Add(initData);
        }

        //计算特殊标签
        for (int i = 0; i < _array.Length; i++)
        {
            if (_array[i] > 35)
            {
                CreateRoleTagEntity.InitData initData = ReferencePools.Acquire<CreateRoleTagEntity.InitData>();
                initData.Quality = 7;
                initData.Desc = Language.WanZhongWuYi;
                roleTag_initData.Add(initData);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            var config_list = ConfigData.Instance.Tables.TbRoleTag.DataList;
            foreach (var config in config_list)
            {
                if (config.AttrType != i)
                {
                    continue;
                }

                if (config.ActionNum == 0)
                {
                    if (_array[i] <= config.DropNum)
                    {

                        CreateRoleTagEntity.InitData initData = ReferencePools.Acquire<CreateRoleTagEntity.InitData>();
                        initData.Quality = config.Quality;
                        initData.Desc = config.Desc;
                        roleTag_initData.Add(initData);
                    }

                }

                if (config.ActionNum == 1)
                {
                    if (_array[i] >= config.DropNum)
                    {

                        CreateRoleTagEntity.InitData initData = ReferencePools.Acquire<CreateRoleTagEntity.InitData>();
                        initData.Quality = config.Quality;
                        initData.Desc = config.Desc;
                        roleTag_initData.Add(initData);
                    }
                }
            }
        }

        entityRender.SetData<CreateRoleTagEntity.InitData>(roleTag_initData);

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ReferencePools.RemoveAll<CreateRoleTagEntity.InitData>();
        entityRender.RemoveAll();
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

        for (int i = 0; i < add_attr; i++)
        {
            int randomInt = Random.Range(0, 5);
            _array[randomInt] = _array[randomInt] + 1;
        }
    }


}