using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using slim_jre.Base;
using slim_jre.Entity;
using slim_jre.Os;
using slim_jre.Service.Receivers;
using slim_jre.Utils;

namespace slim_jre.Service.Tools
{
    public class JavaTool
    {
        public static Dictionary<string, List<string>> RunJar(Jar jar)
        {
            StringBuilder command = new StringBuilder(Dir.javaPath);
            command.Append(" -jar -verbose:class ").Append(jar.path);
            JavaVerboseReceiver receiver = new JavaVerboseReceiver();
            CommandExecutor.Execute(command.ToString(), receiver);
            return receiver._jreClassesDic;
        }

        public static Dictionary<string, List<string>> GetWorkClass()
        {
            Dictionary<string, List<string>> jreClassDic = new Dictionary<string, List<string>>();
            string command = Dir.javaPath + " -verbose:class";
            string result = CommandExecutor.Execute(null, command);
            result = result.Replace("[Loaded ", "").Replace("from ", "").Replace("]", "");
            string[] lines = result.Split(new []{CommonConstant.EOF}, StringSplitOptions.None);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (StringUtils.isEmpty(line))
                {
                    continue;
                }

                string[] parts = line.Split(' ');
                if (parts.Length != 2)
                {
                    continue;
                }

                string jarName = Path.GetFileName(parts[1]);
                if (!jreClassDic.ContainsKey(jarName))
                {
                    jreClassDic.Add(jarName, new List<string>());
                }
                jreClassDic[jarName].Add(parts[0]);
            }
            return jreClassDic;
        }
    }
}