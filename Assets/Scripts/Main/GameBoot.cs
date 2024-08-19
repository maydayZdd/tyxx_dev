using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Event;
using YooAsset;
using System.IO;
using System.Linq;
using HybridCLR;
using System.Reflection;
using Unity.VisualScripting;
using System.Text;

/// <summary>
/// 游戏启动入口
/// </summary>
public class GameBoot : MonoBehaviour
{
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    [Tooltip("资源系统运行模式")]
    public EPlayMode PlayMode = EPlayMode.HostPlayMode;

    /// <summary>
    /// 是否开启日志
    /// </summary>
    [Header("是否开启Info日志级别")]
    public bool IsEnableLogInfo = true;

    /// <summary>
    /// 是否开启日志
    /// </summary>
    [Header("是否开启Debugger")]
    public bool IsDebugger = true;

    /// <summary>
    /// 唯一自己
    /// </summary>
    public static GameBoot Behaviour = default; 

    // 热更界面Obj
    private GameObject obj;


    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        Behaviour = this;
    }

    void Start()
    {
        if (IsDebugger)
        {
            Application.logMessageReceived += LogHandler;
        }
        StartCoroutine(InitYooAssets(StartGame));
    }

    IEnumerator InitYooAssets(Action onComplete)
    {
        Debug.Log($"[GameBoot] 资源系统运行模式：{PlayMode}");
        // 初始化事件系统
        UniEvent.Initalize();

        // 初始化资源系统
        YooAssets.Initialize();

        // 加载更新页面
        obj = GameObject.Instantiate(Resources.Load<GameObject>("PatchWindow"), transform.Find("UIRoot").transform);

        // 开始补丁更新流程
        PatchOperation operation = new PatchOperation("DefaultPackage", EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        // 设置默认的资源包
        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);
        

        // 程序也要热更 先拿到热更的DLL
        var assets = new List<string> { "HotUpdate.dll" }.Concat(AOTMetaAssemblyFiles);
        foreach (var asset in assets)
        {
            var handle = gamePackage.LoadAssetAsync<TextAsset>(asset);
            yield return handle;
            var assetObj = handle.AssetObject as TextAsset;
            asset_dlls[asset] = assetObj;

            Debug.Log($"[GameBoot] 读取DLL文件 :{asset}   {assetObj != null}");
            asset_dlls[asset] = assetObj;
          
        }
        onComplete();
    }

    void StartGame()
    {
        // 先补充元数据
        LoadMetadataForAOTAssemblies();
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll"));
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif

        //加载日志到调试器
        _logInformations = sb.ToString();

        //加载热更程序集
        Type game_mgr_type = hotUpdateAss.GetType("GameManager");
        game_mgr_type.GetMethod("Run").Invoke(
            game_mgr_type, 
            new object[] {                  //传入参数
                IsEnableLogInfo ,
                IsDebugger,
                _logInformations,
        });


        //销毁加载界面
        GameObject.Destroy(obj);

    }

    #region 热更DLL

    private static Dictionary<string, TextAsset> asset_dlls = new Dictionary<string, TextAsset>();
    private static List<string> AOTMetaAssemblyFiles { get; } = new() 
    {
        "Luban.Runtime.dll",
        "Newtonsoft.Json.dll",
        "System.Core.dll",
        "UniTask.dll",
        "UnityEngine.CoreModule.dll",
        "YooAsset.dll",
        "mscorlib.dll",
    };
   
    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            byte[] dllBytes = ReadBytesFromStreamingAssets(aotDllName);
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"补充元数据 LoadMetadataForAOTAssembly:{aotDllName}  ret:{err}");
        }
    }

    /// <summary>
    /// 加载程序集二进制
    /// </summary>
    /// <param name="dllName"></param>
    /// <returns></returns>
    public static byte[] ReadBytesFromStreamingAssets(string dllName)
    {
        if (asset_dlls.ContainsKey(dllName))
        {
            return asset_dlls[dllName].bytes;
        }

        return Array.Empty<byte>();
    }

    #endregion

    #region 日志相关

    //存放启动日志
    private static string _logInformations = default;
    private static StringBuilder sb = new StringBuilder();

    private static void LogHandler(string condition, string stackTrace, LogType type)
    {
        if (_logInformations != default)
        {
            sb.Clear();
            Application.logMessageReceived -= LogHandler;
            return;
        }

        string log_type = default;
        if (type == LogType.Assert)
        {
            log_type = "Fatal";
        }
        else if (type == LogType.Exception || type == LogType.Error)
        {
            log_type = "Error";
        }
        else if (type == LogType.Warning)
        {
            log_type = "Warning";
        }
        else if (type == LogType.Log)
        {
            log_type = "Info";
        }
        sb.Append(DateTime.Now.ToString("HH:mm:ss"));
        sb.Append("#");
        sb.Append(condition);
        sb.Append("#");
        sb.Append(stackTrace);
        sb.Append("#");
        sb.Append(log_type);
        sb.Append("|");
    }

    #endregion  

}