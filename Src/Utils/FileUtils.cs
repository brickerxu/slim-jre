using System.Collections.Generic;
using System.IO;

namespace slim_jre.Utils
{
    public class FileUtils
    {
        /**
         * 删除文件夹及其子项
         */
        public static void Remove(string filePath)
        {

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else if (Directory.Exists(filePath))
            {
                string[] dirs = Directory.GetDirectories(filePath);
                foreach (string dir in dirs)
                {
                    Remove(dir);
                }
                string[] files = Directory.GetFiles(filePath);
                foreach (string path in files)
                {
                    Remove(path);
                }
                Directory.Delete(filePath);
            }
        }

        public static List<string> GetFiles(string path, string fileType)
        {
            List<string> fileList = new List<string>();
            if (File.Exists(path))
            {
                if (fileType.ToUpper().Equals(GetFileType(path).ToUpper()))
                {
                    fileList.Add(path);
                }
            } else if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    fileList.AddRange(GetFiles(dir, fileType));
                }
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string ss = GetFileType(file);
                    if (fileType.ToUpper().Equals(GetFileType(file).ToUpper()))
                    {
                        fileList.Add(file);
                    }
                }
            }

            return fileList;
        }

        public static string GetFileType(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf('.') + 1);
        }

        public static void CopyDirectory(string sourcePath, string destPath, bool overwrite)
        {
            if (File.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                File.Copy(sourcePath, Path.Combine(destPath, Path.GetFileName(sourcePath)), overwrite);
            } else if (Directory.Exists(sourcePath))
            {
                string newDestPath = Path.Combine(destPath, Path.GetFileName(sourcePath));
                string[] dirs = Directory.GetDirectories(sourcePath);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, newDestPath, overwrite);
                }
                string[] files = Directory.GetFiles(sourcePath);
                foreach (string file in files)
                {
                    CopyDirectory(file, newDestPath, overwrite);
                }
            }
        }
    }
}