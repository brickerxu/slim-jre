using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using slim_jre.Base;
using slim_jre.Entity;
using slim_jre.Os;
using slim_jre.Utils;

namespace slim_jre.Service.Receivers
{
    public class JdepsCommandReceiver : DefaultCommandReceiver
    {

        Jar jar;

        string lastJarName;

        Jar currentJar;

        JarClass currentJarClass;
        
        public List<Jar> jars;
            
        public JdepsCommandReceiver(Jar jar)
        {
            jars = new List<Jar>();
            this.jar = jar;
            this.currentJar = jar;
        }

        public override void Output(string log)
        {
            Match match = RegexConstant.DEPENDENCY_CLASS_REGEX.Match(log);
            if (match.Success)
            {
                GroupCollection gcs = match.Groups;
                string dependencyClassName = gcs[1].Value;
                string dependencyJarName = "";
                if (StringUtils.isEmpty(dependencyClassName))
                {
                    dependencyClassName = gcs[2].Value;
                    dependencyJarName = gcs[3].Value;
                }
                if (dependencyJarName.Equals(currentJar.jarName))
                {
                    currentJarClass.dependencySelfClass.Add(dependencyClassName);
                }
                else
                {
                    string jarPath = GetJarPath(dependencyJarName);
                    if (StringUtils.isEmpty(jarPath))
                    {
                        return;
                    }

                    if (!currentJarClass.dependencyOtherClass.ContainsKey(jarPath))
                    {
                        currentJarClass.dependencyOtherClass.Add(jarPath, new List<string>());
                    }
                    currentJarClass.dependencyOtherClass[jarPath].Add(dependencyClassName);
                    
                }

                return;
            }

            match = RegexConstant.CLASS_NAME_REGEX.Match(log);
            if (match.Success)
            {
                string className = match.Groups[1].Value;
                JarClass jc = new JarClass();
                jc.className = className;
                jc.jar = currentJar;
                currentJar.classes.Add(jc);
                currentJarClass = jc;
                return;
            }
            
            match = RegexConstant.DEPENDENCY_JAR_REGEX.Match(log);
            if (match.Success)
            {
                GroupCollection gcs = match.Groups;
                string jarName = gcs[1].Value;
                string dependencyJarPath = gcs[2].Value;
                if (StringUtils.isEmpty(lastJarName))
                {
                    jar.jarName = jarName;
                    jar.thirdLibs.Clear();
                    jar.jreLibs.Clear();
                    jars.Add(jar);
                    lastJarName = jarName;
                    Adapter.dAppendText(jarName);
                }
                else if (!lastJarName.Equals(jarName))
                {
                    Jar newJar = new Jar();
                    newJar.jarName = jarName;
                    jars.Add(newJar);
                    currentJar = newJar;
                    lastJarName = jarName;
                    Adapter.dAppendText(jarName);
                }
                if (dependencyJarPath.StartsWith(Dir.jreDirPath))
                {
                    currentJar.jreLibs.Add(dependencyJarPath);
                }
                else
                {
                    currentJar.thirdLibs.Add(dependencyJarPath); 
                }
            }
        }

        private string GetJarPath(string jarName)
        {
            foreach (string lib in currentJar.jreLibs)
            {
                string libName = Path.GetFileName(lib);
                if (jarName.Equals(libName))
                {
                    return lib;
                }
            }
            foreach (string lib in currentJar.thirdLibs)
            {
                string libName = Path.GetFileName(lib);
                if (jarName.Equals(libName))
                {
                    return lib;
                }
            }

            return null;
        }
    }
}