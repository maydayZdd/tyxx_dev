using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIView
{

    public abstract class UIBaseView
    {
        public UIType UIType { get; set; }

        /// <summary>
        /// 传入的数据
        /// </summary>
        public object ViewData { get; set; }

        /// <summary>
        /// 场景里界面实际的GameObject
        /// </summary>
        public GameObject ActiveObj { get; set; } = default;

        /// <summary>
        /// 列表-按钮
        /// </summary>
        public List<Button> listButton = new List<Button>();

        /// <summary>
        /// 列表-滚动视图
        /// </summary>
        public List<ScrollRect> listScroll = new List<ScrollRect>();

        /// <summary>
        /// toggle 列表
        /// </summary>
        public List<Toggle> listToggle = new List<Toggle>();

        protected UIBaseView() { }

        public void SetData(GameObject activeObj, UIType uIType, object userData = default)
        {
            ActiveObj = activeObj;
            UIType = uIType;
            ViewData = userData;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        public void Close()
        {
            UICtrlView.Instance.CloseView(UIType.GuidName);
        }



        /// <summary>
        /// 界面初始化
        /// </summary>
        public virtual void OnInit()
        {

            //检查是否有遮罩相关
            CheckIsUIMask();
            //收集所有监听事件
            CollectAllListeners();
            //填充绑定组件
            FillComponent();
            //关闭所有txt 射线
            DisableTMPRaycastTarget();

        }

        /// <summary>
        /// 界面激活
        /// </summary>
        public virtual void OnEnable()
        {
            ActiveObj.SetActive(true);
        }

        /// <summary>
        /// 界面刷新
        /// </summary>
        public virtual void OnFlush(string key) { }

        /// <summary>
        /// 界面关闭
        /// </summary>
        public virtual void OnDisable()
        {
            ActiveObj.SetActive(false);
        }

        /// <summary>
        /// 界面销毁
        /// </summary>
        public virtual void OnDestroy()
        {
            RemoveAllListeners();
        }

        #region UIMask
        public void CheckIsUIMask()
        {
            if (ActiveObj == null) return;
            if (UIType.ViewType != ViewType.Pop_View) return;

            Transform _ui_mask = ActiveObj.transform.Find("UI_Mask");
            if (_ui_mask != null)
            {
                _ui_mask.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Close();
                });
            }
        }

        #endregion

        #region 绑定组件相关

        /// <summary>
        /// 通过标识，直接访问视图节点上绑定的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected GameObject Ele(string key)
        {
            return UIBindTool.Find(ActiveObj, key);
        }

        /// <summary>
        /// 通过标识，直接访问视图节点上绑定的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Component Ele(string key, Type t)
        {
            var gameObj = Ele(key);
            if (gameObj == default)
            {
                return default;
            };
            var com = gameObj.GetComponent(t);
            if (com == null)
            {
                Debug.LogError($"{ActiveObj.name} 没有在[{gameObj.name}]上找到[{t}] key[{key}]");
                return default;
            };
            return com;
        }

        /// <summary>
        /// 类型到属性对象列表的映射
        /// </summary>
        static Dictionary<Type, List<FieldInfo>> _fieldInfoCache = new Dictionary<Type, List<FieldInfo>>(0);

        /// <summary>
        /// 获取身上的属性对象列表
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static List<FieldInfo> GetFieldInfo(object o)
        {
            var selfType = o.GetType();
            if (!_fieldInfoCache.ContainsKey(selfType))
            {
                List<FieldInfo> propInfoList = new List<FieldInfo>();
                propInfoList.AddRange(selfType.GetFields(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public));
                _fieldInfoCache[selfType] = propInfoList;
            };
            return _fieldInfoCache[selfType];
        }

        /// <summary>
        /// 填充组件
        /// </summary>
        private void FillComponent()
        {
            List<FieldInfo> propInfoList = GetFieldInfo(this);
            for (int propInfoIdx = 0; propInfoIdx < propInfoList.Count; propInfoIdx++)
            {
                FieldInfo propInfo = propInfoList[propInfoIdx];
                // 尝试读取该字段上面的信息特性
                var cfgArray = propInfo.GetCustomAttributes(typeof(AttComInfo), false);
                AttComInfo cfg = null;
                if (cfgArray != null && cfgArray.Length != 0)
                {
                    cfg = cfgArray[0] as AttComInfo;
                };

                if (cfg != null)
                {
                    if (propInfo.FieldType == ActiveObj.GetType())
                    {
                        // 直接设置为物体
                        propInfo.SetValue(this, Ele(cfg.tag));
                    }
                    else
                    {
                        // 直接设置为对应的组件
                        propInfo.SetValue(this, Ele(cfg.tag, propInfo.FieldType));
                    }
                }
            }
        }


        #endregion

        #region 优化相关

        /// <summary>
        /// 关闭TextMeshProUGUI 射线
        /// </summary>
        public void DisableTMPRaycastTarget()
        {
            TextMeshProUGUI[] textMeshProUGUIs = ActiveObj.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < textMeshProUGUIs.Length; i++)
            {
                textMeshProUGUIs[i].raycastTarget = false;
            }
        }

        public void CollectAllListeners()
        {
            listButton.AddRange(ActiveObj.GetComponentsInChildren<Button>(true));
            listScroll.AddRange(ActiveObj.GetComponentsInChildren<ScrollRect>(true));
            listToggle.AddRange(ActiveObj.GetComponentsInChildren<Toggle>(true));
        }

        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        public void RemoveAllListeners()
        {
            for (int i = 0; i < listButton.Count; i++)
            {
                if (!listButton[i])
                {
                    continue;
                };
                listButton[i].onClick.RemoveAllListeners();
            };
            for (int i = 0; i < listScroll.Count; i++)
            {
                if (!listScroll[i])
                {
                    continue;
                };
                listScroll[i].onValueChanged.RemoveAllListeners();
            };

            for (int i = 0; i < listToggle.Count; i++)
            {
                if (!listToggle[i])
                {
                    continue;
                };
                listToggle[i].onValueChanged.RemoveAllListeners();
            };
        }



        #endregion
    }
}
