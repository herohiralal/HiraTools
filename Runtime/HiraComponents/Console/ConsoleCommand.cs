using System;
using System.Reflection;
using System.Text;

namespace HiraEngine.Components.Console.Internal
{
	internal partial class ConsoleCommand
	{
        public ConsoleCommand(MethodInfo method, ParameterInfo[] parameters)
		{
			Method = method;
			byte i;
			var count = parameters.Length;
			for (i = 0; i < count; i++)
			{
				switch (i)
				{
					case 0: _arg1 = supported_types[parameters[i].ParameterType];
						break;
					case 1: _arg2 = supported_types[parameters[i].ParameterType];
						break;
					case 2: _arg3 = supported_types[parameters[i].ParameterType];
						break;
					case 3: _arg4 = supported_types[parameters[i].ParameterType];
						break;
					case 4: _arg5 = supported_types[parameters[i].ParameterType];
						break;
					case 5: _arg6 = supported_types[parameters[i].ParameterType];
						break;
					default: throw new ArgumentOutOfRangeException();
				}
			}

			ArgumentCount = i;
		}

		public const byte MAX_SUPPORTED_ARGUMENTS = 5;

		public readonly MethodInfo Method;
		public readonly byte ArgumentCount;
		private readonly ConsoleCommandArgumentType _arg1;
		private readonly ConsoleCommandArgumentType _arg2;
		private readonly ConsoleCommandArgumentType _arg3;
		private readonly ConsoleCommandArgumentType _arg4;
		private readonly ConsoleCommandArgumentType _arg5;
		private readonly ConsoleCommandArgumentType _arg6;

		public static bool IsTypeSupported(Type type) => supported_types.ContainsKey(type);
        
        public ConsoleCommandArgumentType GetArgumentType(byte index) => index switch
			{
				0 => _arg1,
				1 => _arg2,
				2 => _arg3,
				3 => _arg4,
				4 => _arg5,
				5 => _arg6,
				_ => throw new ArgumentOutOfRangeException()
			};

        public string ArgumentMetadata
        {
            get
            {
                var sb = new StringBuilder(100);
                var parameters = Method.GetParameters();
                for (byte i = 0; i < parameters.Length; i++)
                {
	                sb.Append($" [{GetArgumentType(i)} {parameters[i].Name}]");
                }

                return sb.ToString();
            }
        }
	}
}