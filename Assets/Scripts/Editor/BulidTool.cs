using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.U2D;

/// <summary>
/// 打包工具
/// </summary>
public static class BulidTool
{
    [MenuItem("ZTool/打包工具/拷贝HotUpdate.dll到资源文件")]
    public static void HotUpdateDLLCopyByBytes()
    {
        string sourceDirectory = @"D:\tyxx_proj\tyxx_dev\HybridCLRData\HotUpdateDlls\StandaloneWindows64"; // 指定的目录
        string destinationDirectory = Application.dataPath +"/AssetRaw/Codes/"; // 目的目录

        // 获取指定目录下的所有文件
        foreach (string filePath in Directory.GetFiles(sourceDirectory))
        {
            // 获取文件名
            string fileName = Path.GetFileName(filePath);
            if (fileName == "HotUpdate.dll")
            {
                // 设置新的文件名
                string newFileName = fileName + ".bytes"; // 根据需要修改文件名的逻辑
                // 复制文件并修改文件名
                string destinationFilePath = Path.Combine(destinationDirectory, newFileName);
                File.Copy(filePath, destinationFilePath, true); // true 表示如果目标文件存在则覆盖它

                Debug.Log($"文件 {fileName} 已复制并重命名为 {newFileName}");
                AssetDatabase.Refresh();
                return;
            }
            

        }

        
    }

}
