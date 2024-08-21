using Common;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class CreateRoleTagEntity : Entity
{
    [AttComInfo("txt_tag")]
    TextMeshProUGUI txt_tag;

    [AttComInfo("img_bg")]
    Image img_bg;

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

        txt_tag.text = initData.Desc;

        UniTask.Void(async () =>
        {
            Sprite sprite = await ResMgr.GetCommonSprite("quality_" + initData.Quality);
            img_bg.sprite = sprite;
        });






    }
}
