using System.Windows.Controls;
using slim_jre.Os;

namespace slim_jre.Service.Receivers
{
    public class JdepsCommandReceiver : DefaultCommandReceiver
    {
        private RichTextBox console;

        public JdepsCommandReceiver(RichTextBox console)
        {
            this.console = console;
        }

        public override void Command(string command)
        {
            base.Command(command);
            console.AppendText(command);
        }

        public override void Output(string log)
        {
            base.Output(log);
            console.AppendText(log);
            console.ScrollToEnd();
        }
    }
}