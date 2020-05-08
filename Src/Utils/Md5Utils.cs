using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace slim_jre.Utils
{
    public class Md5Utils
    {
        static public string GetStringMD5(string str)
        {
            string md5Str = "";
            byte[] data = Encoding.GetEncoding("utf-8").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                md5Str += bytes[i].ToString("x2");
            }

            return md5Str;
        }

        public static string GetFileMD5(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(file);
            string md5Str = System.BitConverter.ToString(hash_byte);
            return md5Str.Replace("-", "");
        }
    }
}