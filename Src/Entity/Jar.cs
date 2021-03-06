﻿using System.Collections.Generic;

namespace slim_jre.Entity
{
    public class Jar
    {
        public string md5;

        public string jarName;

        public string path;

        /**
         * jar包包含的所有类
         */
        public List<JarClass> classes = new List<JarClass>();

        /**
         * 依赖的jre jar包路径
         */
        public List<string> jreLibs = new List<string>();
        
        /**
         * 依赖的第三方jar包路径
         */
        public List<string> thirdLibs = new List<string>();
    }
}