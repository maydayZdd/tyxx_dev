using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

/// <summary>
/// 本地数据管理器
/// </summary>
public class LocalDataManager :Singleton<LocalDataManager>
{
    // 修改1：增加usersData缓存数据
    public static Dictionary<string, PlayerDatas> playerDatas = new Dictionary<string, PlayerDatas>();
    // 加密1：选择一些用于亦或操作的字符（注意保密）
    public static char[] keyChars = { 'b', 'm', 'w', 'a', 'c', 'e' };

    // 使用protected构造函数来防止外部直接实例化
    protected LocalDataManager()
    {
        Log.Info("LocalDataManager Init...");
    }

    // 使用protected构造函数(带参数)来防止外部直接实例化
    protected LocalDataManager(bool dummy) : base(dummy) { }

    public void Init(){}

    // 保存用户数据文本
    public void SavePlayerDatas(PlayerDatas PlayerDatas)
    {
        // 在persistentDataPath下创建一个/users文件夹，方便管理
        if (!File.Exists(Application.persistentDataPath + "/users"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/users");
        }

        // 修改2：保存缓存数据
        playerDatas[PlayerDatas.Guid] = PlayerDatas;

        // 转换用户数据为JSON字符串
        string jsonData = JsonConvert.SerializeObject(PlayerDatas);
        // 加密4
        Log.Info("加密数据..");
        jsonData = Encrypt(jsonData);
        // 将JSON字符串写入文件中（文件名为PlayerDatas.name）
        File.WriteAllText(Application.persistentDataPath + string.Format("/users/{0}.json", PlayerDatas.Guid), jsonData);
    }

    // 读取用户数据到内存
    public PlayerDatas LoadPlayerDatas(string guid)
    {
        // 修改3： 率先从缓存中取数据，而不是从文本文件中读取
        /*        if (playerDatas.ContainsKey(guid))
                {
                    return playerDatas[guid];
                }*/
        string path = Application.persistentDataPath + string.Format("/users/{0}.json", guid);
        // 检查用户配置文件是否存在
        if (File.Exists(path))
        {
            // 从文本文件中加载JSON字符串
            string jsonData = File.ReadAllText(path);
            // 解密
            Log.Info("解密数据..");
            jsonData = Decrypt(jsonData);
            // 将JSON字符串转换为用户内存数据
            PlayerDatas PlayerDatas = JsonConvert.DeserializeObject<PlayerDatas>(jsonData);
            return PlayerDatas;
        }
        else
        {
            Debug.LogError("找不到数据..");
            return null;
        }
    }

    #region 数据加密

    public string strKey = "bwmbbami";//注意：这里的密钥sKey必须能转为8个byte，即输入密钥为8半角个字符或者4个全角字符或者4个汉字的字符串
    public string strIV = "ijklmnop";
    // 加密
    public string Encrypt(string _strQ)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(_strQ);
        MemoryStream ms = new MemoryStream();
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(Encoding.UTF8.GetBytes(strKey), Encoding.UTF8.GetBytes(strIV)), CryptoStreamMode.Write);
        encStream.Write(buffer, 0, buffer.Length);
        encStream.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }

    // 解密
    public string Decrypt(string _strQ)
    {
        byte[] buffer = Convert.FromBase64String(_strQ);
        MemoryStream ms = new MemoryStream();
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        CryptoStream encStream = new CryptoStream(ms, des.CreateDecryptor(Encoding.UTF8.GetBytes(strKey), Encoding.UTF8.GetBytes(strIV)), CryptoStreamMode.Write);
        encStream.Write(buffer, 0, buffer.Length);
        encStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    #endregion

}
