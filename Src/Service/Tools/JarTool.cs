using System.IO;
using slim_jre.Base;
using slim_jre.Entity;
using slim_jre.Os;
using slim_jre.Utils;

namespace slim_jre.Service.Tools
{
    class JarTool
    {
        public static string UnJar(Jar jar) 
        {
            string command = Dir.jarPath + " xf " + jar.path + " META-INF\\MANIFEST.MF";
            string unPath = Path.Combine(Dir.tempDirPath, jar.md5);
            Directory.CreateDirectory(unPath);
            CommandExecutor.Execute(unPath, command);
            string manifestPath = Path.Combine(unPath, "META-INF", "MANIFEST.MF");
            if (!File.Exists(manifestPath))
            {
                // 提取命令没有执行成功
                return "";
            }

            return manifestPath;
        }
        
        public static bool UnJar(string unPath, string jarPath) 
        {
            if (!File.Exists(jarPath))
            {
                return false;
            }
            string command = Dir.jarPath + " xf " + jarPath;
            Directory.CreateDirectory(unPath);
            string result = CommandExecutor.Execute(unPath, command);
            return CommonConstant.EOF.Equals(result);
        }

        public static void CreateJar(string jarPath, string manifestPath, string sourceDir)
        {
            string command = Dir.jarPath + " cvfm " + jarPath + " " + manifestPath + " -C " + sourceDir + " .";
            CommandExecutor.Execute(null, command);
        }
    }
}
