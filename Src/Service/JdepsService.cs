using System.Windows.Controls;
using slim_jre.Entity;
using slim_jre.Service.Receivers;
using slim_jre.Service.Tools;

namespace slim_jre.Service
{
    class JdepsService
    {

        public void Verbose(Jar jar, RichTextBox console)
        { 
            JdepsCommandReceiver receiver = new JdepsCommandReceiver(console);
            
            JdepsTool.VerboseClass(jar, receiver);
            console.AppendText(receiver.GetOutput());
        }
    }
}
