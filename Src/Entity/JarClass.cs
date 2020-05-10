using System.Collections.Generic;

namespace slim_jre.Entity
{
    public class JarClass
    {
        public string className;
        /**
         * 类所属的jar包
         */
        public Jar jar;

        /**
         * 依赖的jre中的类
         */
        public List<string> dependencyJreClass = new List<string>();
        
        /**
         * 依赖自身的类
         */
        public List<string> dependencySelfClass = new List<string>();

        /**
         * 依赖的第三方jar的类
         */
        public Dictionary<string, List<string>> dependencyThirdClass = new Dictionary<string, List<string>>();
    }
}