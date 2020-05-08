using System;
using System.Collections.Generic;
using System.IO;
using slim_jre.Base;
using slim_jre.Entity;
using slim_jre.Service.Tools;
using slim_jre.Utils;

namespace slim_jre.Service
{
    class JarService
    {
        public Jar ReadJar(string jarPath)
        {
            Jar jar = new Jar();
            jar.md5 = Md5Utils.GetFileMD5(jarPath);
            jar.path = jarPath;
            ReadManifest(jar);
            return jar;
        }

        public void ReadManifest(Jar jar)
        {
            string manifestPath = JarTool.UnJar(jar);
            if (StringUtils.isEmpty(manifestPath))
            {
                return;
            }

            string text = File.ReadAllText(manifestPath);
            text = text.Replace(CommonConstant.EOF + " ", "");
            string[] lines = text.Split(new string[] {CommonConstant.EOF}, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line.Contains("Main-Class"))
                {
                    jar.mainClass = line.Split(':')[1];
                }
                else if (line.Contains("Class-Path"))
                {
                    List<string> classPaths = new List<string>();
                    string[] libs = line.Split(new string[] {": "}, StringSplitOptions.None)[1].Split(' ');
                    string rootPath = Directory.GetParent(jar.path).FullName;
                    foreach (string lib in libs)
                    {
                        classPaths.Add(Path.Combine(rootPath, lib));
                    }

                    jar.classPaths = classPaths;
                }
            }
        }
    }
}
