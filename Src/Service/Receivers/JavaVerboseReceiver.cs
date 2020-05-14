using System.Collections.Generic;
using System.Text.RegularExpressions;
using slim_jre.Os;
using static slim_jre.Service.Receivers.RegexConstant;

namespace slim_jre.Service.Receivers
{
    public class JavaVerboseReceiver : DefaultCommandReceiver
    {
        public Dictionary<string, List<string>> _jreClassesDic;

        public JavaVerboseReceiver()
        {
            _jreClassesDic = new Dictionary<string, List<string>>();
        }


        public override void Output(string log)
        {
            Match match = DEPENDENCY_THIRD_CLASS_REGEX.Match(log);
            if (match.Success)
            {
                string className = match.Groups[1].Value;
                string jarPath = match.Groups[2].Value;
                return;
            }

            match = DEPENDENCY_JRE_CLASS_REGEX.Match(log);
            if (match.Success)
            {
                string className = match.Groups[1].Value;
                string jarPath = match.Groups[2].Value;
                if (!_jreClassesDic.ContainsKey(jarPath))
                {
                    _jreClassesDic.Add(jarPath, new List<string>());
                }
                _jreClassesDic[jarPath].Add(className);
            }
        }
        
        
    }
}