using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Common
{
    /// <summary>
    /// 引用池接口
    /// </summary>
    public interface IReference
    {
        /// <summary>
        /// 回收对象
        /// </summary>
        void Clear();
    }

}

