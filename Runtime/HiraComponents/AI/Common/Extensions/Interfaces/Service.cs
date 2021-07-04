namespace HiraEngine.Components.AI
{
    public abstract class Service : System.IDisposable
    {
        public virtual void OnServiceStart()
        {
        }

        public virtual void Run()
        {
        }

        public virtual void OnServiceStop()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}