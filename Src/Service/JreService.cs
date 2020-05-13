using System;
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
        private List<string> libPaths;

        public JreService(List<string> libPaths)
        {
            this.libPaths = libPaths;
        }

        public void ExtractJre(Dictionary<string, List<string>> jreClasseDic)
        {
            string currentTime = TimeUtils.GetTimeStamp();
            foreach (string jarName in jreClasseDic.Keys)
            {
                string jarPath = GetJarPath(jarName);
                string unJarPath = Path.Combine(Dir.tempDirPath, currentTime, "original_" + jarName);
                string newJarPath = Path.Combine(Dir.tempDirPath, currentTime, "new_" + jarName);
                if (JarTool.UnJar(unJarPath, jarPath))
                {
                    List<string> classes = jreClasseDic[jarName].Distinct().ToList();
                    // java/lang/VirtualMachineError
                    // java/lang/IllegalMonitorStateException
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
        

        private string GetJarPath(string jarName)
        {
            foreach (string lp in libPaths)
            {
                if (jarName.Equals(Path.GetFileName(lp)))
                {
                    return lp;
                }
            }

            return null;
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