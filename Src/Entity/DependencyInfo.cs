using System.Collections.Generic;

namespace slim_jre.Entity
{
    public class DependencyInfo
    {
        public string path;
        public string jarName;
        /**
         * 已经读取过的class
         * 主要用于依赖包
         */
        public List<string> readClassName = new List<string>();
        public List<string> dependencyJreClass = new List<string>();
        public Dictionary<string, List<string>> dependencyThirdClass = new Dictionary<string, List<string>>();

        public DependencyInfo(string path)
        {
            this.path = path;
        }
    }
}