using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using Cysharp.Threading.Tasks;
using System;
using UIView;
using Common;
using System.Security.Principal;
using System.IO;

/// <summary>
/// 实体渲染组件
/// </summary>
public sealed class EntityRender<T> where T : Entity, new()
{
    /// <summary>
    /// 实例数据
    /// </summary>
    public Queue<Entity> queye_entitys = new Queue<Entity>();

    /// <summary>
    /// Asset 资源
    /// </summary>
    private AssetHandle handle = default;

    /// <summary>
    /// 要渲染的资源路径
    /// </summary>
    private string res_path = default;
    /// <summary>
    /// 父节点transform
    /// </summary>
    private Transform parent_trans = default;

    /// <summary>
    /// 所属的界面
    /// </summary>
    private UIBaseView to_view = default;

    public void Init(UIBaseView baseView, string path, Transform parent)
    {
        res_path = path;
        parent_trans = parent;
        to_view = baseView;
    }

    public void SetData<DATA>(List<DATA> listData = default) where DATA : IReference
    {
        //回收所有的对象
        if (listData == default)
        {
            int count = queye_entitys.Count;
            for (int i = 0; i < count; i++) {
                Entity entity = queye_entitys.Dequeue();
                entity.OnRecovery();
                ObjectPools.Recovery<T>(entity.ActiveObj);
            }
            return;
        }

        //如果当前数据少于 目前实例化的数据
        if(listData.Count < queye_entitys.Count)
        {
            //需要回收的个数
            int rec_num = queye_entitys.Count - listData.Count;
            for (int i = 0; i < rec_num; i++)
            {
                Entity entity = queye_entitys.Dequeue();
                entity.OnRecovery();
                ObjectPools.Recovery<T>(entity.ActiveObj);
            }


        }

        UniTask.Void(async () =>
        {
            if (handle == null)
            {
                handle = YooAssets.LoadAssetAsync<GameObject>(res_path);
            }
            await handle.Task;

            for (int i = 0; i < listData.Count; i++)
            {
                bool is_Init = false;
                GameObject go = ObjectPools.Acquire<T>(handle, out is_Init);
                go.transform.SetParent(parent_trans);
                if (go.GetComponent<T>() == null)
                {
                    go.AddComponent<T>();
                }
                Entity entity = go.GetComponent<T>();
                entity.ToView = to_view;
                if (is_Init) 
                {
                    entity.OnInit();
                }
                entity.SetData<DATA>(listData[i]);
                queye_entitys.Enqueue(entity);
            }
        });
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void RemoveAll()
    {
        handle.Release();
        queye_entitys.Clear();
        ObjectPools.RemoveAll<T>();
    }



}
