using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIView
{
    public class UIBindTool : MonoBehaviour
    {
        [Serializable]
        public struct BindingVO
        {
            public string key;
            public GameObject gameObject;
        }

        [Header("数据引用")]
        public List<BindingVO> resList = new List<BindingVO>();

        //组件导入规则
        #region
        private void Reset()
        {
            //UI 绑定组件规则
            List<string> ui_bings = new List<string>();
            ui_bings.Add("btn_");
            ui_bings.Add("txt_");
            ui_bings.Add("img_");
            ui_bings.Add("slider_");
            ui_bings.Add("scroll_");
            ui_bings.Add("game_");
            ui_bings.Add("input_");
            //TODO

            RectTransform[] rectTransform_list = transform.GetComponentsInChildren<RectTransform>();
            foreach (var rect in rectTransform_list)
            {
                foreach (string str in ui_bings)
                {
                    if (rect.name.StartsWith(str))
                    {
                        BindingVO bindingVO = new BindingVO();
                        bindingVO.key = rect.name;
                        bindingVO.gameObject = rect.gameObject;
                        resList.Add(bindingVO);
                    }
                }
            }

        }

        #endregion




        /// <summary>
        /// 找到Key对应的资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public GameObject Find(string key)
        {
            foreach (var vo in resList)
            {
                if (vo.key == key)
                {
                    return vo.gameObject;
                }
            }
            return default;
        }

        public static GameObject Find(GameObject go, string key)
        {
            var data = go.GetComponent<UIBindTool>();
            if (null == data)
            {
                return null;
            }
            return data.Find(key);
        }
    }
}