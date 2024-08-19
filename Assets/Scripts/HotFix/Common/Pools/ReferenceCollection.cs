using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 引用池
    /// </summary>
    public class ReferenceCollection
    {
        private readonly Queue<IReference> m_References = new Queue<IReference>();
        private Type m_ReferenceType;
        private int m_CurrUsingRefCount;//当前引用的数量

        public int CurrUsingRefCount => m_CurrUsingRefCount;
        public int TotalRefCount => m_References.Count;


        public ReferenceCollection(Type refType)
        {
            m_ReferenceType = refType;
            m_CurrUsingRefCount = 0;
        }

        public T Acquire<T>() where T : class, IReference, new()
        {
            if (typeof(T) != m_ReferenceType)
            {
                throw new Exception("[引用池]类型不相同无法请求!!!");
            }
            m_CurrUsingRefCount++;
            lock (m_References)
            {
                if (m_References.Count > 0)
                {
                    return (T)m_References.Dequeue();
                }
            }
            return new T();
        }




        public void Recovery(IReference reference)
        {
            reference.Clear();
            lock (m_References)
            {
                if (m_References.Contains(reference))
                {
                    throw new Exception("引用已经被释放，请勿重新释放!!!");
                }

                m_References.Enqueue(reference);
            }

            m_CurrUsingRefCount--;
        }

        public void Add<T>(int count) where T : class, IReference, new()
        {
            if (typeof(T) != m_ReferenceType)
            {
                throw new Exception("类型不相同无法添加!!!");
            }
            lock (m_References)
            {
                while (count-- > 0)
                {
                    m_References.Enqueue(new T());
                }
            }
        }

        public void Remove(int count)
        {
            lock (m_References)
            {
                if (count > m_References.Count)
                {
                    count = m_References.Count;
                }
                while (count-- > 0)
                {
                    m_References.Dequeue();
                }
            }
        }

        public void RemoveAll()
        {
            lock (m_References)
            {
                m_References.Clear();
            }
        }
    }
}