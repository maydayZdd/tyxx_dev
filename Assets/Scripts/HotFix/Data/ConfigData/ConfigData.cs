using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using cfg;
using YooAsset;
using JetBrains.Annotations;

namespace Data
{

    public class ConfigData : Singleton<ConfigData>
    {
        public Tables Tables
        {
            get;
            private set;
        }

        // 使用protected构造函数来防止外部直接实例化
        protected ConfigData()
        {
            Log.Info("ConfigData Init..");
        }

        // 使用protected构造函数(带参数)来防止外部直接实例化
        protected ConfigData(bool dummy) : base(dummy) { }

        public void Init()
        {
            Dictionary<string, TextAsset> dic_textAsset = new Dictionary<string, TextAsset>();
            var gamePackage = YooAssets.GetPackage("DefaultPackage");
            AllAssetsHandle handle = gamePackage.LoadAllAssetsSync<UnityEngine.TextAsset>("item_tbitem");
            foreach (var assetObj in handle.AllAssetObjects)
            {
                TextAsset textAsset = assetObj as TextAsset;
                dic_textAsset.Add(textAsset.name, textAsset);
                Log.Info($"加载配置表.. : {textAsset.name}");
            }

            Tables = new Tables((file) =>
            {
                if (dic_textAsset.ContainsKey(file))
                {
                    JSONNode json_node = JSON.Parse(dic_textAsset[file].text);
                    return json_node;
                }
                Debug.LogError($"读取配置失败{file}");
                return null;
            });
        }



    }
}