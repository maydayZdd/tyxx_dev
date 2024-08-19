using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIView
{
    /// <summary>
    /// UI 控制器
    /// </summary>
    public class UICtrlView : Singleton<UICtrlView>
    {
        /// <summary>
        /// Ui 核心类
        /// </summary>
        private UICore _uiCore = default;

        // 使用protected构造函数来防止外部直接实例化
        protected UICtrlView()
        {
            Log.Info("UICtrlView Init ..");
            _uiCore = new UICore();
        }

        // 使用protected构造函数(带参数)来防止外部直接实例化
        protected UICtrlView(bool dummy) : base(dummy) { }
        public void Init() { }

        #region 打开关闭刷新 通用方法

        /// <summary> 
        /// 打开界面
        /// </summary>
        public void OpenView(string guid_name, object userData = default)
        {
            var config_view = ConfigData.Instance.Tables.TbUiView[guid_name];
            if (config_view == null)
            {
                Debug.LogError($"找不到当前界面配置 : {guid_name}");
                return;
            }
            UIType uIType = new UIType(
                guid_name: config_view.ViewName,
                path: config_view.Path,
                viewType: (ViewType)config_view.ViewType
            );
            OpenView(uIType, userData);
        }

        /// <summary> 
        /// 打开界面
        /// </summary>
        public void OpenView(UIType uIType, object userData = default)
        {
            _uiCore.Open(uIType, userData);
        }

        /// <summary> 
        /// 关闭界面
        /// </summary>
        public void CloseView(string guid_name)
        {
            var config_view = ConfigData.Instance.Tables.TbUiView[guid_name];
            CloseView(guid_name, (ViewType)config_view.ViewType);
        }

        /// <summary> 
        /// 关闭界面
        /// </summary>
        public void CloseView(string guid_name, ViewType viewType)
        {
            _uiCore.CloseView(guid_name, viewType);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="guid_name"></param>
        /// <param name="view_key"></param>
        public void FlushView(string guid_name, string view_key = "all")
        {
            var config_view = ConfigData.Instance.Tables.TbUiView[guid_name];
            FlushView(guid_name, (ViewType)config_view.ViewType, view_key);

        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="guid_name"></param>
        /// <param name="viewType"></param>
        /// <param name="view_key"></param>
        public void FlushView(string guid_name, ViewType viewType, string view_key = "all")
        {
            _uiCore.FlushView(guid_name, viewType, view_key);
        }

        #endregion

        #region 打开通用弹窗界面
        public void OpenCommonPopView(string guid_name, string title, string content, Action ok_action = default, Action cal_action = default)
        {
            CommonPopView.CommonPopViewData commonPopViewData = new CommonPopView.CommonPopViewData(
               _title: title,
               _content: content,
               _ok_action: ok_action,
               _cal_action: cal_action
            );
            OpenView(guid_name, commonPopViewData);
        }

        #endregion
    }
}
