using slim_jre.Entity;
using slim_jre.Service.Receivers;
using slim_jre.Service.Tools;

namespace slim_jre.Service
{
    class JdepsService
    {

        public void Verbose(Jar jar)
        { 
            JdepsCommandReceiver receiver = new JdepsCommandReceiver();
            
            JdepsTool.VerboseClass(jar, receiver);
        }
    }
}
