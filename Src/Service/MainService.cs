using System.Collections.Generic;
using System.IO;
using System.Linq;
using slim_jre.Entity;

namespace slim_jre.Service
{
    class MainService
    {
        private JarService jarService;

        private JdepsService jdepsService;

        private List<string> analysisedClasses = new List<string>();

        private List<string> jreJarNames = new List<string>();

        public MainService()
        {
            jarService = new JarService();
            jdepsService = new JdepsService();
        }

        public void StartWork(string jarPath)
        {
            Jar jar = jarService.ReadJar(jarPath);
            List<Jar> jars = jdepsService.Verbose(jar);
            jreJarNames = GetJreJarNames(jar, jars);
            Dictionary<string, List<string>> jreClasseDic = GetDependencyJreClass(jar, jars);
        }

        private Dictionary<string, List<string>> GetDependencyJreClass(Jar jar, List<Jar> jars)
        {
            List<JarClass> jarClasses = jar.classes;
            Dictionary<string, List<string>> dependencyClassDic = new Dictionary<string, List<string>>();
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
            return GetDependencyJreClass(dependencyClassDic, jars);
        }

        private Dictionary<string, List<string>> GetDependencyJreClass(Dictionary<string, List<string>> dependencyClassDic, List<Jar> jars)
        {
            Dictionary<string, List<string>> jreClasseDic = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> newDependencyClassDic = new Dictionary<string, List<string>>();
            foreach (string jarName in dependencyClassDic.Keys)
            {
                List<string> classes = dependencyClassDic[jarName].Distinct().ToList();
                if (jreJarNames.Contains(jarName))
                {
                    jreClasseDic.Add(jarName, classes);
                }
                foreach (string className in classes)
                {
                    if (analysisedClasses.Contains(jarName+className))
                    {
                        continue;
                    }

                    JarClass jarClass = GetJarClass(jarName, className, jars);
                    if (null == jarClass)
                    {
                        continue;
                    }

                    analysisedClasses.Add(jarName+className);
                    if (jarClass.dependencySelfClass.Count > 0)
                    {
                        if (newDependencyClassDic.ContainsKey(jarName))
                        {
                            newDependencyClassDic[jarName].AddRange(jarClass.dependencySelfClass);
                        }
                        else
                        {
                            newDependencyClassDic.Add(jarName, jarClass.dependencySelfClass);
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

        private List<string> GetJreJarNames(Jar jar, List<Jar> jars)
        {
            List<string> jreJarLibs = new List<string>();
            jreJarLibs.AddRange(jar.jreLibs);
            foreach (Jar j in jars)
            {
                jreJarLibs.AddRange(j.jreLibs);
            }

            List<string> jreJarNames = new List<string>();
            foreach (string lib in jreJarLibs.Distinct().ToList())
            {
                jreJarNames.Add(Path.GetFileName(lib));
            }
            return jreJarNames;
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
        
        private JarClass GetJarClass(string jarName, string className, List<Jar> jars)
        {
            return GetJarClass(className, GetJar(jarName, jars));
        }
    }
}
