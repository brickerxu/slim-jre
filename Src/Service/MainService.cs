using System.Collections.Generic;
using System.Linq;
using slim_jre.Entity;

namespace slim_jre.Service
{
    class MainService
    {
        private JarService jarService;

        private JdepsService jdepsService;

        private List<string> analysisedClasses = new List<string>();

        public MainService()
        {
            jarService = new JarService();
            jdepsService = new JdepsService();
        }

        public void StartWork(string jarPath)
        {
            Jar jar = jarService.ReadJar(jarPath);
            List<Jar> jars = jdepsService.Verbose(jar);
            List<string> jreClasses = GetDependencyJreClass(jar, jars);
        }

        private List<string> GetDependencyJreClass(Jar jar, List<Jar> jars)
        {
            List<string> jreClasses = new List<string>();
            List<JarClass> jarClasses = jar.classes;
            Dictionary<string, List<string>> dependencyClassDic = new Dictionary<string, List<string>>();
            foreach (JarClass jc in jarClasses)
            {
                jreClasses.AddRange(jc.dependencyJreClass);
                foreach (string key in jc.dependencyThirdClass.Keys)
                {
                    if (dependencyClassDic.ContainsKey(key))
                    {
                        dependencyClassDic[key].AddRange(jc.dependencyThirdClass[key]);
                    }
                    else
                    {
                        dependencyClassDic.Add(key, jc.dependencyThirdClass[key]);
                    }
                }
            }
            jreClasses.AddRange(GetDependencyJreClass(dependencyClassDic, jars));
            return jreClasses.Distinct().ToList();
        }

        private List<string> GetDependencyJreClass(Dictionary<string, List<string>> dependencyClassDic, List<Jar> jars)
        {
            List<string> jreClasses = new List<string>();
            Dictionary<string, List<string>> newDependencyClassDic = new Dictionary<string, List<string>>();
            foreach (string jarName in dependencyClassDic.Keys)
            {
                List<string> classes = dependencyClassDic[jarName];
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

                    jreClasses.AddRange(jarClass.dependencyJreClass);
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
                    foreach (string key in jarClass.dependencyThirdClass.Keys)
                    {
                        if (newDependencyClassDic.ContainsKey(key))
                        {
                            newDependencyClassDic[key].AddRange(jarClass.dependencyThirdClass[key]);
                        }
                        else
                        {
                            newDependencyClassDic.Add(key, jarClass.dependencyThirdClass[key]);
                        }
                    }
                }
            }

            if (newDependencyClassDic.Keys.Count > 0)
            {
                jreClasses.AddRange(GetDependencyJreClass(newDependencyClassDic, jars));
            }
            return jreClasses.Distinct().ToList();;
        }

        private JarClass GetJarClass(string jarName, string className, List<Jar> jars)
        {
            foreach (Jar jar in jars)
            {
                if (jarName.Equals(jar.jarName))
                {
                    foreach (JarClass jc in jar.classes)
                    {
                        if (className.Equals(jc.className))
                        {
                            return jc;
                        }
                    }
                }
            }

            return null;
        }
    }
}
