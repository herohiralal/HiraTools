namespace HiraEngine.Components.AI
{
    public interface IService
    {
        void OnServiceStart();
        void Run();
        void OnServiceStop();
    }
}