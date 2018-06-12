using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Multiverse.Editor
{
    public class ABMaker
    {
        static string assetsDir = Application.dataPath.Replace("/", "\\") + "\\Res\\";
        static string datasDir = Application.dataPath.Replace("/", "\\") + "\\Datas\\";
        static string luaDir = Application.dataPath.Replace("/", "\\") + "\\Lua\\";
        static string bundleDir = Application.dataPath.Replace("/", "\\") + "\\StreamingAssets\\";
        static string filesName = Application.dataPath.Replace("/", "\\") + "\\StreamingAssets\\files.txt";

        [MenuItem("Tools/ABMaker/IOS", false, 1)]
        public static void BuildABForIOS()
        {
            BuildAB(BuildTarget.iOS);
        }

        [MenuItem("Tools/ABMaker/Android", false, 2)]
        public static void BuildABForAndroid()
        {
            BuildAB(BuildTarget.Android);
        }

        [MenuItem("Tools/ABMaker/Windows", false, 3)]
        public static void BuildABForWindows()
        {
            BuildAB(BuildTarget.StandaloneWindows64);
        }

        public static void BuildAB(BuildTarget buildTarget)
        {
            ClearAB();
            ClearABNames();
            SetABNames(assetsDir);
            BuildPipeline.BuildAssetBundles(bundleDir, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, buildTarget);
            CopyData();
            CopyLua();
            GenerateFileList();
        }

        private static void ClearAB()
        {
            if (Directory.Exists(bundleDir))
            {
                Directory.Delete(bundleDir, true);
            }

            Directory.CreateDirectory(bundleDir);
        }

        private static void ClearABNames()
        {
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();

            for (int i = 0; i < abNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(abNames[i], true);
            }
        }

        private static void SetABNames(string path)
        {
            if (System.IO.File.Exists(path))
            {
                if (!path.EndsWith(".meta"))
                {
                    SetABName(path);
                }
            }
            else if (System.IO.Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileSystemInfo[] files = dir.GetFileSystemInfos();

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i] is DirectoryInfo)
                    {
                        SetABNames(files[i].FullName);
                    }
                    else if (!files[i].Name.EndsWith(".meta"))
                    {
                        SetABName(files[i].FullName);
                    }
                }
            }
        }

        private static void SetABName(string assetPath)
        {
            string importerPath = "Assets" + assetPath.Substring(Application.dataPath.Length);
            AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);

            string tempName = assetPath.Substring(assetPath.LastIndexOf(@"\") + 1);
            string assetName = tempName.Remove(tempName.LastIndexOf("."));
            assetImporter.assetBundleName = assetName;
        }

        private static void CopyData()
        {
            File.CopyDir(datasDir, bundleDir + "Datas\\");
        }

        private static void CopyLua()
        {
            File.CopyDir(luaDir, bundleDir + "Lua\\");
        }

        private static void GenerateFileList()
        {
            List<string> files = new List<string>();
            File.CollectFiles(bundleDir, files);

            FileStream fs = new FileStream(filesName, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.EndsWith(".manifest"))
                    continue;

                string md5 = File.GetFileMD5(file);
                string value = file.Replace(bundleDir, string.Empty);
                sw.WriteLine(value + "|" + md5);
            }
            sw.Close();
            fs.Close();
            AssetDatabase.Refresh();
        }
    }
}
