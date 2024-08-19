using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    /// <summary>
    /// 玩家数据结构
    /// </summary>
    public class PlayerDatas
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
    }
}
