namespace HiraEngine.Components.Console.Internal
{
    internal readonly struct CommandMetadata
    {
        public CommandMetadata(string commandName, string argumentData)
        {
            CommandName = commandName;
            DisplayName = commandName + argumentData;
        }
        
        public readonly string CommandName;
        public readonly string DisplayName;
    }
}