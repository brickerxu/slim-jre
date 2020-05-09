using slim_jre.Entity;

namespace slim_jre.Service
{
    class MainService
    {
        private JarService jarService;

        private JdepsService jdepsService;

        public MainService()
        {
            jarService = new JarService();
            jdepsService = new JdepsService();
        }

        public void StartWork(string jarPath)
        {
            Jar jar = jarService.ReadJar(jarPath);
            jdepsService.Verbose(jar);
        }
    }
}
