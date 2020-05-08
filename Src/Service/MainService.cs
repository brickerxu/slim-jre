using slim_jre.Entity;

namespace slim_jre.Service
{
    class MainService
    {
        private JarService jarService;

        public MainService()
        {
            jarService = new JarService();
        }

        public void StartWork(string jarPath)
        {
            Jar jar = jarService.ReadJar(jarPath);
        }
    }
}
