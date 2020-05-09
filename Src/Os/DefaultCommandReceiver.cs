using System.Text;
using slim_jre.Os.Interfaces;

namespace slim_jre.Os
{
    public class DefaultCommandReceiver : ICommandReceiver
    {
        public bool cancel;

        private StringBuilder output;
        public bool IsCancelled()
        {
            return cancel;
        }

        public virtual void Command(string command)
        {
            output = new StringBuilder();
        }

        public virtual void Output(string log)
        {
            output.Append(log);
        }

        public string GetOutput()
        {
            return output.ToString();
        }
    }
}