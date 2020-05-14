using System.Text.RegularExpressions;

namespace slim_jre.Service.Receivers
{
    public class RegexConstant
    {
        public static readonly Regex DEPENDENCY_JAR_REGEX = new Regex("^(.*?\\.jar) -> (.*?\\.jar)\r\n$");

        public static readonly Regex CLASS_NAME_REGEX = new Regex("^(\\S+?) \\((\\S+?)\\)\r\n$");

        public static readonly Regex DEPENDENCY_CLASS_REGEX = new Regex("^-> (\\S+?)\r\n$|^-> (\\S+?)\\s*?(\\S*?)\r\n$");
        
        public static readonly Regex DEPENDENCY_JRE_CLASS_REGEX = new Regex("^\\[Loaded (.*?) from (.*?\\.jar)\\]\r\n$");
        
        public static readonly Regex DEPENDENCY_THIRD_CLASS_REGEX = new Regex("^\\[Loaded (.*?) from file:/(.*?\\.jar)\\]\r\n$");
    }
}