using System;
using System.Reflection;
using System.Text;

namespace HiraEngine.Components.Console.Internal
{
	internal partial class ConsoleCommand
	{
        public ConsoleCommand(MethodInfo method, ParameterInfo[] parameters)
		{
			_method = method;
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

			_argumentCount = i;
		}

		public const byte MAX_SUPPORTED_ARGUMENTS = 5;

		private readonly MethodInfo _method;
		private readonly byte _argumentCount;
		private readonly HiraConsoleCommandArgumentType _arg1;
		private readonly HiraConsoleCommandArgumentType _arg2;
		private readonly HiraConsoleCommandArgumentType _arg3;
		private readonly HiraConsoleCommandArgumentType _arg4;
		private readonly HiraConsoleCommandArgumentType _arg5;
		private readonly HiraConsoleCommandArgumentType _arg6;

		public static bool IsTypeSupported(Type type) => supported_types.ContainsKey(type);

		public bool TryInvoke(string args)
		{
			var parsedArgs = args.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
			var parameters = new object[_argumentCount];

			if (parsedArgs.Length < _argumentCount)
				return false;

			for (byte i = 0; i < _argumentCount; i++)
			{
				if (!ParsingUtility.TryParse(parsedArgs[i], GetArgumentType(i), out var parsedParameter))
					return false;

				parameters[i] = parsedParameter;
			}

			_method.Invoke(null, parameters);
			return true;
		}
        
        private HiraConsoleCommandArgumentType GetArgumentType(byte index) => index switch
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
                var parameters = _method.GetParameters();
                foreach (var parameterInfo in parameters)
                {
                    sb.Append($" [{parameterInfo.ParameterType.Name} {parameterInfo.Name}]");
                }

                return sb.ToString();
            }
        }
	}
}