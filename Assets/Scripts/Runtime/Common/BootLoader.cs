using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class BootLoader : MonoBehaviour
    {
        public static string bundleNames = "files.txt";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator Start()
        {
            string natives;
            string remotes;

            if (System.IO.File.Exists(Application.persistentDataPath + bundleNames))
            {
                natives = System.IO.File.ReadAllText(Application.persistentDataPath + bundleNames);
            }
            else
            {
                string nativeBundleNames = ABLoader.StreamingAssetsPath + bundleNames;
                WWW nativeWWW = new WWW(nativeBundleNames);
                yield return nativeWWW;

                natives = nativeWWW.text;
            }

            string remoteBundleNames = ABLoader.StreamingAssetsPath + bundleNames;
            WWW remoteWWW = new WWW(remoteBundleNames);
            yield return remoteWWW;

            remotes = remoteWWW.text;

            if (!FileIsSame(natives, remotes))
            {
                Dictionary<string, string> nativeResources = CreateFileHashes(natives);
                Dictionary<string, string> remoteResources = CreateFileHashes(remotes);

                List<string> needUpdateResources = new List<string>();

                Dictionary<string, string>.Enumerator iter = remoteResources.GetEnumerator();
                while (iter.MoveNext())
                {
                    if ((nativeResources.ContainsKey(iter.Current.Key) && nativeResources[iter.Current.Key] != iter.Current.Value) || !nativeResources.ContainsKey(iter.Current.Key))
                    {
                        needUpdateResources.Add(iter.Current.Key);
                    }
                }

                if (needUpdateResources.Count > 0)
                {
                    //从WWW更新资源，并释放到Cache，同时显示更新界面
                    //将remotes释放到Cache中
                    for (int i = 0; i < needUpdateResources.Count; i++)
                    {
                        ABLoader.ReleaseToPersistentDataPath(needUpdateResources[i], needUpdateResources[i], null);//需要同步显示更新界面
                    }
                    System.IO.File.WriteAllBytes(Path.Combine(Application.persistentDataPath, bundleNames),Encoding.Default.GetBytes(remotes));
                }
            }

            
            ApplicationController.Instance.Init();//接管

            DestroyImmediate(gameObject);
        }

        bool FileIsSame(string src, string dst)
        {
            //return false;
            return File.GetFileContentMD5(src) == File.GetFileContentMD5(dst);
        }

        Dictionary<string, string> CreateFileHashes(string content)
        {
            Dictionary<string, string> resources = new Dictionary<string, string>();
            string[] files = content.Split(new string[] { "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < files.Length; i++)
            {
                string[] kvs = files[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                resources.Add(kvs[0], kvs[1]);
            }
            return resources;
        }
    }
}
