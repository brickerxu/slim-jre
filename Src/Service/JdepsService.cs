using System.Collections.Generic;
using System.Linq;
using slim_jre.Entity;
using slim_jre.Service.Receivers;
using slim_jre.Service.Tools;

namespace slim_jre.Service
{
    class JdepsService
    {

        public DependencyInfo Verbose(Jar jar)
        {
            return Verbose(jar, new List<string>(), new List<string>());
        }

        public DependencyInfo Verbose(Jar jar, List<string> classScope, List<string> readClassName)
        {
            DependencyInfo di = new DependencyInfo(jar.path);
            di.readClassName = readClassName;
            JdepsCommandReceiver receiver = new JdepsCommandReceiver(di, classScope);
            
            JdepsTool.VerboseClass(jar, receiver);
            Dictionary<string, List<string>>.KeyCollection keys = di.dependencyThirdClass.Keys;
            foreach (string key in keys)
            {
                if (di.dependencyThirdClass[key].Count == 0)
                {
                    continue;
                }

                Jar thirdJar = new Jar();
                thirdJar.path = key;
                thirdJar.classPaths = jar.classPaths;
                DependencyInfo dii;
                if (key.Equals(jar.path))
                {
                    List<string> dependencyThirdClass = di.dependencyThirdClass[key];
                    for (int i=dependencyThirdClass.Count - 1;i>=0;i--)
                    {
                        string dependencyThirdClassName = dependencyThirdClass[i];
                        if (di.readClassName.Contains(dependencyThirdClassName))
                        {
                            dependencyThirdClass.Remove(dependencyThirdClassName);
                        }
                    }

                    // foreach (string dependencyThirdClass in di.dependencyThirdClass[key])
                    // {
                    //     if (di.readClassName.Contains(dependencyThirdClass))
                    //     {
                    //         di.dependencyThirdClass[key].Remove(dependencyThirdClass);
                    //     }
                    // }
                    if (di.dependencyThirdClass[key].Count == 0)
                    {
                        continue;
                    }

                    dii = Verbose(thirdJar, di.dependencyThirdClass[key], di.readClassName);
                }
                else
                {
                    dii = Verbose(thirdJar, di.dependencyThirdClass[key], new List<string>());
                }

                di.dependencyJreClass.AddRange(dii.dependencyJreClass.Distinct().ToList());
            }

            return di;
        }
    }
}
