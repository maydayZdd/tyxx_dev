using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using YooAsset;
using Cysharp.Threading.Tasks;

/// <summary>
/// 资源加载 
/// </summary>
public static class ResMgr
{
    private static SpriteAtlas common_spriteAtlas = default;
    public static async UniTask<Sprite> GetCommonSprite(string name)
    {
        string path = "Assets/UIs/Common/common_atlas.spriteatlas";
        if (common_spriteAtlas == null)
        {
            var common_assetHandle = YooAssets.LoadAssetAsync<SpriteAtlas>(path);
            await common_assetHandle.Task;
            common_spriteAtlas = common_assetHandle.AssetObject as SpriteAtlas;
        }
        Sprite sprite = common_spriteAtlas.GetSprite(name);
        if (sprite == null)
        {
            Log.Error($"找不到图片 name :{name} path :{path}");
            return null;
        }
        return sprite;
    }

}
