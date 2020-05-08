using System.IO;
using slim_jre.Base;
using slim_jre.Entity;
using slim_jre.Os;

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
    }
}
