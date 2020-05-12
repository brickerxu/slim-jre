using System.IO;
using System.Text;
using slim_jre.Base;
using slim_jre.Entity;
using slim_jre.Os;
using slim_jre.Os.Interfaces;

namespace slim_jre.Service.Tools
{
    public class JdepsTool
    {
        public static void VerboseClass(Jar jar, ICommandReceiver receiver)
        {
            StringBuilder command = new StringBuilder(Dir.jdepsPath);
            command.Append(" -verbose:class ").Append(jar.path);
            foreach (string lib in jar.thirdLibs)
            {
                command.Append(" ").Append(lib);
            }

            foreach (string lib in jar.jreLibs)
            {
                command.Append(" ").Append(lib);
            }
            CommandExecutor.Execute(command.ToString(), receiver);
        }
    }
}