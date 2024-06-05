using System;
using System.IO;
using LGF.Res;
using Unity;
using UnityEngine;

namespace LGF.Path
{
    public static class PathConfig
    {
        public static string AssetsPath = Application.dataPath;
        public static string ExDataPath = $"{AssetsPath}/GameMain/ExData";

        private static string StreamingAssetsPath = Application.streamingAssetsPath;

        private static string _hotfixPath = $"{Game.ResManager.DefaultPackage.GetPackageSandboxRootDirectory()}";
        private static string _buildinRootPath = $"{Game.ResManager.DefaultPackage.GetPackageBuildinRootDirectory()}";

        public static string ArtPath = $"{AssetsPath}/GameMain/Art";

        //获取资源路径
        public static string GetMaterialPath(string assetName)
        {
            string assetPath = GlobalToLocalPath($"{ArtPath}/Materials/{GetFirstWord(assetName)}/{assetName}");
            return assetName;
        }

        public static string GetJsonDataPath(string assetName)
        {
            return assetName;
        }

        public static string GetUIPrefabPath(string assetName)
        {
            string assetPath = "";
            assetPath = GlobalToLocalPath($"{AssetsPath}/GameMain/UIPrefabs/{GetFirstWord(assetName)}/{assetName}");

            Debug.Log(AssetsPath);
            Debug.Log(assetPath);
            return assetPath;
        }

        // 获取大驼峰第一个单词长度
        public static int GetFirstWordLength(string str)
        {
            int length = 0;
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    length = i;
                    break;
                }
            }

            return length;
        }

        // 将大驼峰第一个单词提取并返回
        public static string GetFirstWord(string str)
        {
            int length = GetFirstWordLength(str);
            return str.Substring(0, length);
        }

        // 绝对路径转本地Assets路径
        public static string GlobalToLocalPath(string absolutePath)
        {
            return absolutePath.Replace(Application.dataPath, "Assets");
        }
    }
}