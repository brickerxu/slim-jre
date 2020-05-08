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
    }
}