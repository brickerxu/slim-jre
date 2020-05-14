using System.Collections.Generic;
using System.IO;
using System.Linq;
using slim_jre.Base;
using slim_jre.Service.Tools;
using slim_jre.Utils;

namespace slim_jre.Service
{
    public class JreService
    {
        public void ExtractJre(Dictionary<string, List<string>> jreClassDic)
        {
            string currentTime = TimeUtils.GetTimeStamp();
            foreach (string jarPath in jreClassDic.Keys)
            {
                string jarName = Path.GetFileName(jarPath);
                string unJarPath = Path.Combine(Dir.tempDirPath, currentTime, "original_" + jarName);
                string newJarPath = Path.Combine(Dir.tempDirPath, currentTime, "new_" + jarName);
                if (JarTool.UnJar(unJarPath, jarPath))
                {
                    List<string> classes = jreClassDic[jarPath].Distinct().ToList();
                    foreach (string className in classes)
                    {
                        string classPath = GetClassPath(unJarPath, className);
                        string newClassPath = GetClassPath(newJarPath, className);
                        if (!Directory.Exists(Path.GetDirectoryName(newClassPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(newClassPath));
                        }
                        File.Copy(classPath, newClassPath, false);
                    }

                    string metaInfPath = Path.Combine(unJarPath, "META-INF", "MANIFEST.MF");
                    JarTool.CreateJar(Path.Combine(Dir.tempDirPath, currentTime, jarName), metaInfPath, newJarPath);
                }
                else
                {
                    return;
                }
            }
        }
        
        private string GetClassPath(string parentPath, string className)
        {
            string path = parentPath;
            string[] ss = className.Split('.');
            foreach (string s in ss)
            {
                path = Path.Combine(path, s);
            }

            return path + ".class";
        }
    }
}