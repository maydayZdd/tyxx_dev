using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//游戏启动入口
public class GameManager : MonoBehaviour
{
    public static bool isEnableLogInfo = default;

    /// <summary>
    /// 单例
    /// </summary>
    public GameManager Instance { get; private set; }

    public static void Run(bool isEnableLogInfo)
    {
        GameObject.Find("GameBoot").AddComponent<GameManager>();
        GameManager.isEnableLogInfo = isEnableLogInfo;

    }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 启动游戏
    /// </summary>
    void Start()
    {
        //加载日志框架
        Log.Initialization();
        //加载UI框架
        UICtrlView.Instance.Init();
        //加载载入界面
        UICtrlView.Instance.OpenView(new UIType(guid_name: "LogingView", path: "Assets/Prefabs/Views/LogingView/LogingView.prefab"));

        // 加载配置
        ConfigData.Instance.Init();

        //加载存档数据 TODO
        LocalDataManager.Instance.Init();

        //打开登录界面
        UICtrlView.Instance.OpenView(ViewName.LoginView);

        UICtrlView.Instance.CloseView("LogingView", ViewType.Normal_View);

    }

}

