using slim_jre.Utils;
using System;
using System.IO;

namespace slim_jre.Base
{
    class Dir
    {
        /**
         * 程序根目录
         */
        public static string applicationPath;

        /**
         * 临时文件存储目录
         */
        public static string tempDirPath;

        /**
         * jdk目录
         */
        public static string jdkDirPath;

        /**
         * java.exe工具路径
         */
        public static string javaPath;

        /**
         * jar.exe工具路径
         */
        public static string jarPath;

        /**
         * jdeps.exe工具路径
         */
        public static string jdepsPath;

        /**
         * jre目录
         */
        public static string jreDirPath;

        static Dir()
        {
            applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            tempDirPath = Path.Combine(applicationPath, "temp");
            jdkDirPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (StringUtils.isNotEmpty(jdkDirPath))
            {
                string jdkBinPath = Path.Combine(jdkDirPath, "bin");
                javaPath = Path.Combine(jdkBinPath, "java.exe");
                jarPath = Path.Combine(jdkBinPath, "jar.exe");
                jdepsPath = Path.Combine(jdkBinPath, "jdeps.exe");
                jreDirPath = Path.Combine(jdkDirPath, "jre");
            }
            init();
        }

        private static void init()
        {
            FileUtils.Remove(tempDirPath);
            Directory.CreateDirectory(tempDirPath);
        }
    }
}