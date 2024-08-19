using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoleTagEntity : Entity
{
    public class InitData : IReference
    {
        public int Quality { get; set; }
        public string Desc { get; set; }

        public InitData(){}

        public void Clear()
        {

        }
    }

    public override void OnFlush<DATA>(DATA data)
    {
        InitData initData = data as InitData;
        Log.Info(initData.Desc);
    }
}
