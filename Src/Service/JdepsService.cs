using System.Collections.Generic;
using System.Linq;
using slim_jre.Entity;
using slim_jre.Service.Receivers;
using slim_jre.Service.Tools;

namespace slim_jre.Service
{
    class JdepsService
    {

        public List<Jar> Verbose(Jar jar)
        {
            JdepsCommandReceiver receiver = new JdepsCommandReceiver(jar);
            JdepsTool.VerboseClass(jar, receiver);
            return receiver.jars;
        }

    }
}
