using UnityEngine;

namespace Multiverse.Runtime
{
    public class BootLoader : MonoBehaviour
    {
        public static string bundleListFileName = "files.txt";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            ABLoader.Instance.LoadFromLocal(bundleListFileName, FileListLoaded);
            //获取本地files，计算md5
            //获取远端md5
            //如果不一致，则开首先获取远端的files，比对不一致的文件，开启更新，需要显示更新界面，将远端文件写入到cache中，完成后将files.txt写入到cache中
        }

        public void FileListLoaded(object file)
        {
            //计算md5
            //获取远端文件的md5
        }

        void Start()
        {
            //检查更新（比对本地的fileList的md5和远端的fileList的md5），如果有的话，则打开更新对话框，进行更新，更新完毕进入登录
            //如果没有更新，则直接进入登录
            //登录最好保存用户名和密码
            //Jar包如何更新

            //
        }

        void Update()
        {

        }

        //第一步，计算本地的fileList的md5（如果缓存区存在，则优先使用缓存区的fileList，否则，从bundle中获取）
        //第二步，获取远端的fileList的md5
        //如果本地和远端不一致，则计算出两者的差异，并且开启更新，将差异文件更新到缓存区，更新完成后将最新fileList放入缓存区
        //以后每次加载优先从缓存区加载，如果不存在，则从bundle中加载
    }
}
