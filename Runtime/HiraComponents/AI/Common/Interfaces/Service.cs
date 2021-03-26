namespace HiraEngine.Components.AI
{
    public abstract class Service
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
    }
}