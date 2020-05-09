using System.Threading;
using slim_jre.Os;

namespace slim_jre.Service.Receivers
{
    public class JdepsCommandReceiver : DefaultCommandReceiver
    {
        public override void Output(string log)
        {
            base.Output(log);
            Adapter.dAppendText(log);
            Thread.Sleep(1);
        }
    }
}