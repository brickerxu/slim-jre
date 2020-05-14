using System.Collections.Generic;
using System.IO;
using System.Linq;
using slim_jre.Entity;
using slim_jre.Service.Tools;

namespace slim_jre.Service
{
    class MainService
    {
        private JarService jarService;

        private JdepsService jdepsService;

        private List<string> analysisedClasses = new List<string>();

        private List<string> jreJarPaths = new List<string>();

        public MainService()
        {
            jarService = new JarService();
            jdepsService = new JdepsService();
        }

        public void StartWork(string jarPath)
        {
            Adapter.dAppendText("精简开始");
            Jar jar = jarService.ReadJar(jarPath);
            Dictionary<string, List<string>> runTimeJreClassDic = JavaTool.RunJar(jar);
            List<Jar> jars = jdepsService.Verbose(jar);
            jreJarPaths = GetJreJarPaths(jar, jars);
            Dictionary<string, List<string>> jreClassDic = GetDependencyJreClass(jar, jars, runTimeJreClassDic);
            foreach (string jp in runTimeJreClassDic.Keys)
            {
                if (jreClassDic.ContainsKey(jp))
                {
                    jreClassDic[jp].AddRange(runTimeJreClassDic[jp]);
                }
            }
            JreService jreService = new JreService();
            jreService.ExtractJre(jreClassDic);
            Adapter.dAppendText("精简完成");
        }

        private Dictionary<string, List<string>> GetDependencyJreClass(Jar jar, List<Jar> jars, Dictionary<string, List<string>> runTimeJreClassDic)
        {
            Dictionary<string, List<string>> dependencyClassDic = new Dictionary<string, List<string>>();
            List<JarClass> jarClasses = jar.classes;
            foreach (JarClass jc in jarClasses)
            {
                foreach (string key in jc.dependencyOtherClass.Keys)
                {
                    if (dependencyClassDic.ContainsKey(key))
                    {
                        dependencyClassDic[key].AddRange(jc.dependencyOtherClass[key]);
                    }
                    else
                    {
                        dependencyClassDic.Add(key, jc.dependencyOtherClass[key]);
                    }
                }
            }

            foreach (string jarPath in runTimeJreClassDic.Keys)
            {
                if (dependencyClassDic.ContainsKey(jarPath))
                {
                    dependencyClassDic[jarPath].AddRange(runTimeJreClassDic[jarPath]);
                }
                else
                {
                    dependencyClassDic.Add(jarPath, runTimeJreClassDic[jarPath]);
                }
            }
            return GetDependencyJreClass(dependencyClassDic, jars);
        }

        private Dictionary<string, List<string>> GetDependencyJreClass(Dictionary<string, List<string>> dependencyClassDic, List<Jar> jars)
        {
            Dictionary<string, List<string>> jreClasseDic = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> newDependencyClassDic = new Dictionary<string, List<string>>();
            foreach (string jarPath in dependencyClassDic.Keys)
            {
                List<string> classes = dependencyClassDic[jarPath].Distinct().ToList();
                if (jreJarPaths.Contains(jarPath))
                {
                    jreClasseDic.Add(jarPath, classes);
                }
                foreach (string className in classes)
                {
                    if (analysisedClasses.Contains(jarPath+className))
                    {
                        continue;
                    }

                    JarClass jarClass = GetJarClass(jarPath, className, jars);
                    if (null == jarClass)
                    {
                        continue;
                    }

                    analysisedClasses.Add(jarPath+className);
                    if (jarClass.dependencySelfClass.Count > 0)
                    {
                        if (newDependencyClassDic.ContainsKey(jarPath))
                        {
                            newDependencyClassDic[jarPath].AddRange(jarClass.dependencySelfClass);
                        }
                        else
                        {
                            newDependencyClassDic.Add(jarPath, jarClass.dependencySelfClass);
                        }
                    }
                    foreach (string key in jarClass.dependencyOtherClass.Keys)
                    {
                        if (newDependencyClassDic.ContainsKey(key))
                        {
                            newDependencyClassDic[key].AddRange(jarClass.dependencyOtherClass[key]);
                        }
                        else
                        {
                            newDependencyClassDic.Add(key, jarClass.dependencyOtherClass[key]);
                        }
                    }
                }
            }

            if (newDependencyClassDic.Keys.Count > 0)
            {
                Dictionary<string, List<string>> newJreClasseDic = GetDependencyJreClass(newDependencyClassDic, jars);
                foreach (string key in newJreClasseDic.Keys)
                {
                    if (jreClasseDic.ContainsKey(key))
                    {
                        jreClasseDic[key].AddRange(newJreClasseDic[key]);
                    }
                    else
                    {
                        jreClasseDic.Add(key, newJreClasseDic[key]);
                    }
                }
            }

            return jreClasseDic;
        }

        private List<string> GetJreJarPaths(Jar jar, List<Jar> jars)
        {
            List<string> jreJarLibs = new List<string>();
            jreJarLibs.AddRange(jar.jreLibs);
            foreach (Jar j in jars)
            {
                jreJarLibs.AddRange(j.jreLibs);
            }

            return jreJarLibs.Distinct().ToList();
        }

        private Jar GetJar(string jarName, List<Jar> jars)
        {
            foreach (Jar jar in jars)
            {
                if (jarName.Equals(jar.jarName))
                {
                    return jar;
                }
            }

            return null;
        }

        private JarClass GetJarClass(string className, Jar jar)
        {
            if (null != jar)
            {
                foreach (JarClass jc in jar.classes)
                {
                    if (className.Equals(jc.className))
                    {
                        return jc;
                    }
                }
            }

            return null;
        }
        
        private JarClass GetJarClass(string jarPath, string className, List<Jar> jars)
        {
            return GetJarClass(className, GetJar(Path.GetFileName(jarPath), jars));
        }

        private List<string> GetLibPaths(List<Jar> jars)
        {
            List<string> libPaths = new List<string>();
            foreach (Jar jar in jars)
            {
                libPaths.AddRange(jar.jreLibs);
                libPaths.AddRange(jar.thirdLibs);
            }

            return libPaths.Distinct().ToList();
        }
    }
}
