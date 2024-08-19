using UnityEngine;
using System.Collections.Generic;
using System;
using Common;

public class DebuggerTool
{
    /// <summary>
    /// 是否允许调试
    /// </summary>
    private bool AllowDebugging = true;

    private DebugType _debugType = DebugType.Console;
    private List<LogData> _logInformations = new List<LogData>();
    private int _currentLogIndex = -1;
    private int _infoLogCount = 0;
    private int _warningLogCount = 0;
    private int _errorLogCount = 0;
    private int _fatalLogCount = 0;
    private bool _showInfoLog = true;
    private bool _showWarningLog = true;
    private bool _showErrorLog = true;
    private bool _showFatalLog = true;
    private Vector2 _scrollLogView = Vector2.zero;
    private Vector2 _scrollCurrentLogView = Vector2.zero;
    private Vector2 _scrollSystemView = Vector2.zero;
    private bool _expansion = false;
    private Rect _windowRect = new Rect(0, 0, 100, 60);

    private int _fps = 0;
    private Color _fpsColor = Color.white;
    private int _frameNumber = 0;
    private float _lastShowFPSTime = 0f;

    private bool _showObject = false;
    private bool _showReference = true;

    public DebuggerTool(string aot_log)
    {
        if (AllowDebugging)
        {
            AotLog(aot_log);
            GameManager.Instance.onGUI += OnGUI;
            GameManager.Instance.onUpdate += Update;
            Application.logMessageReceived += LogHandler;
        }
    }

    private void Update()
    {
        if (AllowDebugging)
        {
            _frameNumber += 1;
            float time = Time.realtimeSinceStartup - _lastShowFPSTime;
            if (time >= 1)
            {
                _fps = (int)(_frameNumber / time);
                _frameNumber = 0;
                _lastShowFPSTime = Time.realtimeSinceStartup;
            }
        }
    }
    public void OnDestory()
    {
        if (AllowDebugging)
        {
            GameManager.Instance.onUpdate -= Update;
            Application.logMessageReceived -= LogHandler;
        }
    }

    private void AotLog(string aot_log)
    {
        string[] line_str = aot_log.Split('|');
        for (int i = 0; i < line_str.Length; i++)
        {
            string[] log_data = line_str[i].Split('#');
            if(log_data.Length == 4)
            {
                LogAotHandler(log_data[0], log_data[1], log_data[2], log_data[3]);
            }
           
        }
    }

    private void LogHandler(string condition, string stackTrace, LogType type)
    {
        LogData log = new LogData();
        log.time = DateTime.Now.ToString("HH:mm:ss");
        log.message = condition;
        log.stackTrace = stackTrace;

        if (type == LogType.Assert)
        {
            log.type = "Fatal";
            _fatalLogCount += 1;
        }
        else if (type == LogType.Exception || type == LogType.Error)
        {
            log.type = "Error";
            _errorLogCount += 1;
        }
        else if (type == LogType.Warning)
        {
            log.type = "Warning";
            _warningLogCount += 1;
        }
        else if (type == LogType.Log)
        {
            log.type = "Info";
            _infoLogCount += 1;
        }

        _logInformations.Add(log);

        if (_warningLogCount > 0)
        {
            _fpsColor = Color.yellow;
        }
        if (_errorLogCount > 0)
        {
            _fpsColor = Color.red;
        }
    }

    private void LogAotHandler(string time,string condition, string stackTrace,string type)
    {
        LogData log = new LogData();
        log.time = time;
        log.message = condition;
        log.stackTrace = stackTrace;
        log.type = type;

        if (type == "Fatal")
        {
            _fatalLogCount += 1;
        }
        else if (type == "Error")
        {
            _errorLogCount += 1;
        }
        else if (type == "Warning")
        {
            _warningLogCount += 1;
        }
        else if (type == "Info")
        {
            _infoLogCount += 1;
        }

        _logInformations.Add(log);

        if (_warningLogCount > 0)
        {
            _fpsColor = Color.yellow;
        }
        if (_errorLogCount > 0)
        {
            _fpsColor = Color.red;
        }
    }

    private void OnGUI()
    {
        if (AllowDebugging)
        {
            if (_expansion)
            {
                _windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "DEBUGGER");
            }
            else
            {
                _windowRect = GUI.Window(0, _windowRect, ShrinkGUIWindow, "DEBUGGER");
            }
        }
    }
    private void ExpansionGUIWindow(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        #region title
        GUILayout.BeginHorizontal();
        GUI.contentColor = _fpsColor;
        if (GUILayout.Button("FPS:" + _fps, GUILayout.Height(30)))
        {
            _expansion = false;
            _windowRect.width = 100;
            _windowRect.height = 60;
        }
        GUI.contentColor = (_debugType == DebugType.Console ? Color.white : Color.gray);
        if (GUILayout.Button("Console", GUILayout.Height(30)))
        {
            _debugType = DebugType.Console;
        }
        GUI.contentColor = (_debugType == DebugType.Memory ? Color.white : Color.gray);
        if (GUILayout.Button("Memory", GUILayout.Height(30)))
        {
            _debugType = DebugType.Memory;
        }
        GUI.contentColor = (_debugType == DebugType.System ? Color.white : Color.gray);
        if (GUILayout.Button("System", GUILayout.Height(30)))
        {
            _debugType = DebugType.System;
        }
        GUI.contentColor = (_debugType == DebugType.Screen ? Color.white : Color.gray);
        if (GUILayout.Button("Screen", GUILayout.Height(30)))
        {
            _debugType = DebugType.Screen;
        }
        GUI.contentColor = (_debugType == DebugType.Quality ? Color.white : Color.gray);
        if (GUILayout.Button("Quality", GUILayout.Height(30)))
        {
            _debugType = DebugType.Quality;
        }
        GUI.contentColor = (_debugType == DebugType.Pools ? Color.white : Color.gray);
        if (GUILayout.Button("Pools", GUILayout.Height(30)))
        {
            _debugType = DebugType.Pools;
        }

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
        #endregion

        #region console
        if (_debugType == DebugType.Console)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear"))
            {
                _logInformations.Clear();
                _fatalLogCount = 0;
                _warningLogCount = 0;
                _errorLogCount = 0;
                _infoLogCount = 0;
                _currentLogIndex = -1;
                _fpsColor = Color.white;
            }
            GUI.contentColor = (_showInfoLog ? Color.white : Color.gray);
            _showInfoLog = GUILayout.Toggle(_showInfoLog, "Info [" + _infoLogCount + "]");
            GUI.contentColor = (_showWarningLog ? Color.white : Color.gray);
            _showWarningLog = GUILayout.Toggle(_showWarningLog, "Warning [" + _warningLogCount + "]");
            GUI.contentColor = (_showErrorLog ? Color.white : Color.gray);
            _showErrorLog = GUILayout.Toggle(_showErrorLog, "Error [" + _errorLogCount + "]");
            GUI.contentColor = (_showFatalLog ? Color.white : Color.gray);
            _showFatalLog = GUILayout.Toggle(_showFatalLog, "Fatal [" + _fatalLogCount + "]");
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();

            _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(165));
            for (int i = 0; i < _logInformations.Count; i++)
            {
                bool show = false;
                Color color = Color.white;
                switch (_logInformations[i].type)
                {
                    case "Fatal":
                        show = _showFatalLog;
                        color = Color.red;
                        break;
                    case "Error":
                        show = _showErrorLog;
                        color = Color.red;
                        break;
                    case "Info":
                        show = _showInfoLog;
                        color = Color.white;
                        break;
                    case "Warning":
                        show = _showWarningLog;
                        color = Color.yellow;
                        break;
                    default:
                        break;
                }

                if (show)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(_currentLogIndex == i, ""))
                    {
                        _currentLogIndex = i;
                    }
                    GUI.contentColor = color;
                    GUILayout.Label("[" + _logInformations[i].type + "] ");
                    GUILayout.Label("[" + _logInformations[i].time + "] ");
                    GUILayout.Label(_logInformations[i].message);
                    GUILayout.FlexibleSpace();
                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();

            _scrollCurrentLogView = GUILayout.BeginScrollView(_scrollCurrentLogView, "Box", GUILayout.Height(100));
            if (_currentLogIndex != -1)
            {
                GUILayout.Label(_logInformations[_currentLogIndex].message + "\r\n\r\n" + _logInformations[_currentLogIndex].stackTrace);
            }
            GUILayout.EndScrollView();
        }
        #endregion

        #region memory
        else if (_debugType == DebugType.Memory)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Memory Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
#if UNITY_5
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemory() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemory() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSize() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSize() / 1000000 + "MB");
#endif
#if UNITY_7
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSizeLong() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSizeLong() / 1000000 + "MB");
#endif
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("卸载未使用的资源"))
            {
                Resources.UnloadUnusedAssets();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("使用GC垃圾回收"))
            {
                GC.Collect();
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region system
        else if (_debugType == DebugType.System)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("System Information");
            GUILayout.EndHorizontal();

            _scrollSystemView = GUILayout.BeginScrollView(_scrollSystemView, "Box");
            GUILayout.Label("操作系统：" + UnityEngine.SystemInfo.operatingSystem);
            GUILayout.Label("系统内存：" + UnityEngine.SystemInfo.systemMemorySize + "MB");
            GUILayout.Label("处理器：" + UnityEngine.SystemInfo.processorType);
            GUILayout.Label("处理器数量：" + UnityEngine.SystemInfo.processorCount);
            GUILayout.Label("显卡：" + UnityEngine.SystemInfo.graphicsDeviceName);
            GUILayout.Label("显卡类型：" + UnityEngine.SystemInfo.graphicsDeviceType);
            GUILayout.Label("显存：" + UnityEngine.SystemInfo.graphicsMemorySize + "MB");
            GUILayout.Label("显卡标识：" + UnityEngine.SystemInfo.graphicsDeviceID);
            GUILayout.Label("显卡供应商：" + UnityEngine.SystemInfo.graphicsDeviceVendor);
            GUILayout.Label("显卡供应商标识码：" + UnityEngine.SystemInfo.graphicsDeviceVendorID);
            GUILayout.Label("设备模式：" + UnityEngine.SystemInfo.deviceModel);
            GUILayout.Label("设备名称：" + UnityEngine.SystemInfo.deviceName);
            GUILayout.Label("设备类型：" + UnityEngine.SystemInfo.deviceType);
            GUILayout.Label("设备标识：" + UnityEngine.SystemInfo.deviceUniqueIdentifier);
            GUILayout.EndScrollView();
        }
        #endregion

        #region screen
        else if (_debugType == DebugType.Screen)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Screen Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("DPI：" + Screen.dpi);
            GUILayout.Label("分辨率：" + Screen.currentResolution.ToString());
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("全屏"))
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region Quality
        else if (_debugType == DebugType.Quality)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Quality Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            string value = "";
            if (QualitySettings.GetQualityLevel() == 0)
            {
                value = " [最低]";
            }
            else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1)
            {
                value = " [最高]";
            }

            GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("降低一级图形质量"))
            {
                QualitySettings.DecreaseLevel();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("提升一级图形质量"))
            {
                QualitySettings.IncreaseLevel();
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region Pools
        else if (_debugType == DebugType.Pools)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Pools Information");
            if(GUILayout.Toggle(_showReference, "引用池"))
            {
                _showObject = false;
            }
            else
            {
                _showObject = true;
            }
            if (GUILayout.Toggle(_showObject, "对象池"))
            {
                _showReference = false;
            }
            else
            {
                _showReference = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");

            if (_showReference)
            {
                GUILayout.Label("类型总数量：" + ReferencePools.Count);
                GUILayout.Label("--->");
                Dictionary<Type, ReferenceCollection> dic_ref = ReferencePools.GetAllReferenceData();
                foreach (var dic in dic_ref)
                {
                    Type type = dic.Key;
                    ReferenceCollection referenceCollection = dic.Value;
                    GUILayout.Label("引用类型：" + type.Name);
                    GUILayout.Label("当前引用数量：" + referenceCollection.CurrUsingRefCount);
                    GUILayout.Label("引用池剩余数量：" + referenceCollection.TotalRefCount);
                    GUILayout.Label("--------------------");
                }
            }

            if (_showObject)
            {
                GUILayout.Label("类型总数量：" + ObjectPools.Count);
                GUILayout.Label("--->");
                Dictionary<Type, ObjectPoolCollection> dic_ref = ObjectPools.GetAllReferenceData();
                foreach (var dic in dic_ref)
                {
                    Type type = dic.Key;
                    ObjectPoolCollection referenceCollection = dic.Value;
                    GUILayout.Label("对象引用类型：" + type.Name);
                    GUILayout.Label("当前对象引用数量：" + referenceCollection.CurrUsingRefCount);
                    GUILayout.Label("对象池剩余数量：" + referenceCollection.TotalRefCount);
                    GUILayout.Label("--------------------");
                }
            }
            GUILayout.EndVertical();


           
        }
        #endregion
    }
    private void ShrinkGUIWindow(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        GUI.contentColor = _fpsColor;
        if (GUILayout.Button("FPS:" + _fps, GUILayout.Width(80), GUILayout.Height(30)))
        {
            _expansion = true;
            _windowRect.width = 600;
            _windowRect.height = 360;
        }
        GUI.contentColor = Color.white;
    }
}
public struct LogData
{
    public string time;
    public string type;
    public string message;
    public string stackTrace;
}
public enum DebugType
{
    Console,
    Memory,
    System,
    Screen,
    Quality,
    Pools
}

