using UnityEngine;
using Data;
using UIView;
using System.Collections.Generic;
using System;

//游戏启动入口
public class GameManager : MonoBehaviour
{
    public static bool isEnableLogInfo = false;
    public static bool IsDebugger = false;
    public static string aot_logs = default;

    /// <summary>
    /// 单例
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// 调试器框架
    /// </summary>
    private DebuggerTool debuggerTool = default;

    /// <summary>
    /// Update事件委托
    /// </summary>
    public event Action onUpdate = default;

    /// <summary>
    /// OnGUI事件委托
    /// </summary>
    public event Action onGUI = default;


    public static void Run(bool isEnableLogInfo,bool IsDebugger, string logDatas)
    {
        GameManager.isEnableLogInfo = isEnableLogInfo;
        GameManager.IsDebugger = IsDebugger;
        GameManager.aot_logs = logDatas;
        GameObject.Find("GameBoot").AddComponent<GameManager>();
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
        //加载调试器
        if (IsDebugger)
        {
            debuggerTool = new DebuggerTool(GameManager.aot_logs);
        }

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

    void Update()
    {
        if (onUpdate != default)
        {
            onUpdate.Invoke();
        }
    }

    void OnGUI()
    {
        if (onGUI != default)
        {
            onGUI.Invoke();
        }
    }

    void OnDestroy()
    {
        debuggerTool.OnDestory();
    }

}

