using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Common {
    public class ObjectPoolCollection
    {
        private readonly Queue<GameObject> m_Pool = new Queue<GameObject>();

        private int m_CurrUsingRefCount;//当前引用的数量

        public int CurrUsingRefCount => m_CurrUsingRefCount;
        public int TotalRefCount => m_Pool.Count;

        public ObjectPoolCollection()
        {
            m_CurrUsingRefCount = 0;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public GameObject Acquire(AssetHandle handle, out bool is_Init)
        {
            if (handle == null)
            {
                throw new Exception("[对象池]对象资源获取失败");
            }

            m_CurrUsingRefCount++;
            lock (m_Pool)
            {
                if (m_Pool.Count > 0)
                {
                    is_Init = false;
                    GameObject go = m_Pool.Dequeue();
                    go.SetActive(true);
                    return go;
                }
            }
            is_Init = true;
            return handle.InstantiateSync();
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pools"></param>
        /// <exception cref="Exception"></exception>
        public void Recovery(GameObject obj, GameObject pools)
        {
            lock (m_Pool)
            {
                if (m_Pool.Contains(obj))
                {
                    throw new Exception("[对象池] 对象已经被释放，请勿重新释放!!!");
                }

                obj.transform.SetParent(pools.transform);
                obj.SetActive(false);
                m_Pool.Enqueue(obj);

            }

            m_CurrUsingRefCount--;
        }

        /// <summary>
        /// 预加载对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <exception cref="Exception"></exception>
        public void Add(AssetHandle handle, int count)
        {
            if (handle == null)
            {
                throw new Exception("[对象池] 对象资源获取失败");
            }

            lock (m_Pool)
            {
                while (count-- > 0)
                {
                    m_Pool.Enqueue(handle.InstantiateSync());
                }

            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="count"></param>
        public void Remove(int count)
        {
            lock (m_Pool)
            {
                if (count > m_Pool.Count)
                {
                    count = m_Pool.Count;
                }
                while (count-- > 0)
                {
                    GameObject go = m_Pool.Dequeue();
                    if (go != null) 
                    {
                        GameObject.Destroy(go);
                    }
                    
                }
            }
        }

        public void RemoveAll()
        {
            lock (m_Pool)
            {
                int count = m_Pool.Count;
                while (count-- > 0)
                {
                    GameObject go = m_Pool.Dequeue();
                    if (go != null)
                    {
                        GameObject.Destroy(go);
                    }

                }
                m_Pool.Clear();
            }
        }

    }

}