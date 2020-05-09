namespace slim_jre.Os.Interfaces
{
    public interface ICommandReceiver
    {
        bool IsCancelled();

        void Command(string command);
        
        void Output(string log);
    }
}