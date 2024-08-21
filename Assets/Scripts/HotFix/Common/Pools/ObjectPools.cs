using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace Common
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public static class ObjectPools
    {
        private static readonly Dictionary<Type, ObjectPoolCollection> m_ReferenceCollections = new Dictionary<Type, ObjectPoolCollection>();
        public static int Count => m_ReferenceCollections.Count;//获取引用池的数量

        private static GameObject CachePools = default;

        public static void ClearAll()
        {
            lock (m_ReferenceCollections)
            {
                foreach (var reference in m_ReferenceCollections.Values)
                {
                    reference.RemoveAll();
                }
                m_ReferenceCollections.Clear();
            }
        }

        public static GameObject Acquire<T>(AssetHandle handle ,out bool is_Init) where T : EntityInterface
        {
            return GetObjectPoolCollection(typeof(T)).Acquire(handle, out is_Init);
        }

        public static void Recovery<T>(GameObject obj)
        {
            if (CachePools == default)
            {
                CachePools = new GameObject();
                CachePools.name = "ObjectPools";
                GameObject.DontDestroyOnLoad(CachePools);
            }
            GetObjectPoolCollection(typeof(T)).Recovery(obj, CachePools);
        }

        public static void Add<T>(AssetHandle handle,int count) where T : EntityInterface
        {
            GetObjectPoolCollection(typeof(T)).Add(handle,count);
        }

        public static void Remove<T>(int count) where T : EntityInterface
        {
            GetObjectPoolCollection(typeof(T)).Remove(count);
        }

        public static void RemoveAll<T>() where T : EntityInterface
        {
            GetObjectPoolCollection(typeof(T)).RemoveAll();
        }

        private static ObjectPoolCollection GetObjectPoolCollection(Type type)
        {
            ObjectPoolCollection referenceCollection = null;
            lock (m_ReferenceCollections)
            {
                if (!m_ReferenceCollections.TryGetValue(type, out referenceCollection))
                {
                    referenceCollection = new ObjectPoolCollection();
                    m_ReferenceCollections.Add(type, referenceCollection);
                }
            }
            return referenceCollection;
        }

        /// <summary>
        /// 获取所有对象池的数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, ObjectPoolCollection> GetAllReferenceData()
        {
            return m_ReferenceCollections;
        }
    }
}