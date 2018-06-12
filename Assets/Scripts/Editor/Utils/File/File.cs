using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Multiverse.Editor
{
    public class File
    {
        public static string GetFileMD5(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetFileMD5 Failed:" + ex.Message);
            }
        }

        public static void CopyDir(string src, string dst)
        {
            try
            {
                if (!Directory.Exists(dst))
                    Directory.CreateDirectory(dst);

                DirectoryInfo dir = new DirectoryInfo(src);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();

                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)
                    {
                        if (!Directory.Exists(dst + "\\" + i.Name))
                        {
                            Directory.CreateDirectory(dst + "\\" + i.Name);
                        }
                        CopyDir(i.FullName, dst + "\\" + i.Name);
                    }
                    else
                    {
                        System.IO.File.Copy(i.FullName, dst + "\\" + i.Name, true);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CollectFiles(string path, List<string> filesList)
        {
            if (System.IO.File.Exists(path))
            {
                if (!path.EndsWith(".meta"))
                {
                    filesList.Add(path);
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
                        CollectFiles(files[i].FullName, filesList);
                    }
                    else if (!files[i].Name.EndsWith(".meta"))
                    {
                        filesList.Add(files[i].FullName);
                    }
                }
            }
        }
    }
}
