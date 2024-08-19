using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 引用池管理器
    /// </summary>
    public static class ReferencePools
    {
        private static readonly Dictionary<Type, ReferenceCollection> m_ReferenceCollections = new Dictionary<Type, ReferenceCollection>();
        public static int Count => m_ReferenceCollections.Count;//获取引用池的数量
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

        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        public static void Recovery(IReference reference)
        {
            GetReferenceCollection(reference.GetType()).Recovery(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }

        public static void Remove<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }

        public static void RemoveAll<T>() where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }

        private static ReferenceCollection GetReferenceCollection(Type type)
        {
            if (type == null)
            {
                throw new Exception("Type 类型 为空!!!");
            }
            ReferenceCollection referenceCollection = null;
            lock (m_ReferenceCollections)
            {
                if (!m_ReferenceCollections.TryGetValue(type, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(type);
                    m_ReferenceCollections.Add(type, referenceCollection);
                }
            }
            return referenceCollection;
        }

        /// <summary>
        /// 获取所有引用池的数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, ReferenceCollection> GetAllReferenceData()
        {
            return m_ReferenceCollections;
        }
    }
}
