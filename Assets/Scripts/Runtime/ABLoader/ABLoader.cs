using System;
using System.IO;
using UnityEngine;

namespace Multiverse.Runtime
{
    public class ABLoader : MonoSingleton<ABLoader>
    {
        public static string StreamingAssetsPath
        {
            get
            {
                string contentPath;
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        contentPath = "jar:file://" + Application.dataPath + "!/assets/";
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        contentPath = Application.dataPath + "/Raw/";
                        break;
                    default:
                        contentPath = Application.dataPath + "/StreamingAssets/";
                        break;
                }
                return contentPath;
            }
        }

        public void LoadFromLocal(string fileName, Action<object> fileLoaded)
        {
            //首先从persistentDataPath中查找
            //如果不存在，则从streamingAsset中查找
        }

        public void LoadFromWWW(string fileName, Action<string> fileLoaded)
        {

        }

        public static void ReleaseToPersistentDataPath(string streamingAssetName, string persistentDataAssetName, Action releaseFinish)
        {
            string streamingAssetPath = Path.Combine(StreamingAssetsPath, streamingAssetName);
            string persistentDataPath = Path.Combine(Application.persistentDataPath, persistentDataAssetName);

#if !UNITY_EDITOR && UNITY_ANDROID
        using (WWW www = new WWW(streamingAssetPath))
        {
            while (!www.isDone) { }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                File.WriteAllBytes(persistentDataPath, www.bytes);
                if (null != releaseFinish)
                {
                    releaseFinish.Invoke();
                }
            }
            else
            {
                Debug.LogError("Release data error:" + www.error);
            }
        }
#else
            File.Copy(streamingAssetPath, persistentDataPath, true);
            if (null != releaseFinish)
            {
                releaseFinish.Invoke();
            }
#endif
        }
    }
}
