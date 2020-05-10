using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using slim_jre.Entity;
using slim_jre.Os;
using slim_jre.Utils;

namespace slim_jre.Service.Receivers
{
    public class JdepsCommandReceiver : DefaultCommandReceiver
    {
        private DependencyInfo dependencyInfo;

        /**
         * 是否为主jar
         */
        private bool mainJar;
        
        /**
         * 是否跳过当前Class
         */
        private bool skipClass;

        /**
         * 需要查找的class集合
         * 主Jar全部查找
         */
        private List<string> classScope;

        public JdepsCommandReceiver(DependencyInfo di, List<string> classScope)
        {
            dependencyInfo = di;
            mainJar = classScope.Count == 0;
            this.classScope = classScope.Distinct().ToList();
            skipClass = false;
        }

        public override void Output(string log)
        {
            // Adapter.dAppendText(log);
            Match match = RegexConstant.DEPENDENCY_CLASS_REGEX.Match(log);
            if (match.Success)
            {
                if (skipClass)
                {
                    return;
                }

                GroupCollection gcs = match.Groups;
                string dependencyClassName = gcs[1].Value;
                string dependencyJarName = "";
                if (StringUtils.isEmpty(dependencyClassName))
                {
                    dependencyClassName = gcs[2].Value;
                    dependencyJarName = gcs[3].Value;
                }

                if (StringUtils.isEmpty(dependencyJarName))
                {
                    dependencyInfo.dependencyJreClass.Add(dependencyClassName);
                    return;
                }
                if (dependencyJarName.Equals(dependencyInfo.jarName))
                {
                    if (mainJar)
                    {
                        return;
                    }

                    if (dependencyInfo.readClassName.Contains(dependencyClassName))
                    {
                        return;
                    }

                    if (!dependencyInfo.dependencyThirdClass.Keys.Contains(dependencyInfo.path))
                    {
                        dependencyInfo.dependencyThirdClass.Add(dependencyInfo.path, new List<string>());
                    }
                }

                Dictionary<string, List<string>>.KeyCollection keys = dependencyInfo.dependencyThirdClass.Keys;
                foreach (string key in keys)
                {
                    if (key.Contains(dependencyJarName))
                    {
                        dependencyInfo.dependencyThirdClass[key].Add(dependencyClassName);
                        return;
                    }
                }
            }

            if (!mainJar)
            {
                match = RegexConstant.CLASS_NAME_REGEX.Match(log);
                if (match.Success)
                {
                    if (classScope.Count == 0)
                    {
                        SetCancel(true);
                        return;
                    }

                    string className = match.Groups[1].Value;
                    if (classScope.Contains(className))
                    {
                        if (dependencyInfo.readClassName.Contains(className))
                        {
                            skipClass = true;
                        }
                        else
                        {
                            skipClass = false;
                            dependencyInfo.readClassName.Add(className);
                        }
                        classScope.Remove(className);
                    }
                    else
                    {
                        skipClass = true;
                    }

                    return;
                }
            }
            
            match = RegexConstant.DEPENDENCY_JAR_REGEX.Match(log);
            if (match.Success)
            {
                GroupCollection gcs = match.Groups;
                string jarName = gcs[1].Value;
                if (StringUtils.isEmpty(dependencyInfo.jarName))
                {
                    dependencyInfo.jarName = jarName;
                }
                // 只解析第一个jar包
                // 后续通过依赖关系递归解析
                else if (!dependencyInfo.jarName.Equals(jarName))
                {
                    SetCancel(true);
                    return;
                }
                string dependencyJarPath = gcs[2].Value;
                dependencyInfo.dependencyThirdClass.Add(dependencyJarPath, new List<string>());
                
            }
        }
    }
}