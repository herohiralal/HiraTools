using System;
using System.Text;

namespace UnityEngine.Internal
{
    [CreateAssetMenu]
    public class StaticLoggerCreator : ScriptableObject, IHiraScriptCreator
    {
        [Serializable]
        private struct TypeData
        {
            public string name;
            public string type;
        }

        [SerializeField] private TypeData[] typeData = { };
        
        public ScriptableObject[] Dependencies { get; } = { };
        [SerializeField] [HideInInspector] private string cachedFilePath = "";
        public string CachedFilePath
        {
            get => cachedFilePath;
            set => cachedFilePath = value;
        }

        public string FileName => "StaticLogger";

        private const string dll_import_string = "[SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]";

        public string FileData
        {
            get
            {
                var sb = new StringBuilder(1000);
                sb
                    .AppendLine(@"// ReSharper disable All")
                    .AppendLine(@"using System;")
                    .AppendLine(@"using System.Runtime.InteropServices;")
                    .AppendLine(@"using System.Security;")
                    .AppendLine(@"")
                    .AppendLine(@"namespace UnityEngine.Internal")
                    .AppendLine(@"{")
                    .AppendLine(@"    public static class StaticLogger")
                    .AppendLine(@"    {")
                    .AppendLine(@"        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]")
                    .AppendLine(@"        public static void OnLoad()")
                    .AppendLine(@"        {")
                    .AppendLine(@"            HiraNativeHook.PreNativeHookCreated -= Initialize;")
                    .AppendLine(@"            HiraNativeHook.PreNativeHookCreated += Initialize;")
                    .AppendLine(@"        }")
                    .AppendLine(@"")
                    .AppendLine(@"        private static void Initialize()")
                    .AppendLine(@"        {")
                    .AppendLine(@"            InitLoggerLogStart(LogStart);")
                    .Append(Initializers)
                    .AppendLine(@"            InitLoggerLogEnd(LogEnd);")
                    .AppendLine(@"        }")
                    .AppendLine(@"")
                    .AppendLine(@"        private const string prefix = ""<color=red><b>Native log: </b></color>"";")
                    .AppendLine(@"        private static LogType _trackedLogType = LogType.Log;")
                    .AppendLine(@"        private static readonly System.Text.StringBuilder string_builder = new System.Text.StringBuilder(1000);")
                    .AppendLine(@"")
                    .AppendLine(@"        // Start Logger")
                    .AppendLine($"        {dll_import_string}")
                    .AppendLine(@"        private static extern void InitLoggerLogStart(Action<LogType> logger);")
                    .AppendLine(@"")
                    .AppendLine(@"        [AOT.MonoPInvokeCallback(typeof(Action<LogType>))]")
                    .AppendLine(@"        private static void LogStart(LogType type)")
                    .AppendLine(@"        {")
                    .AppendLine(@"            _trackedLogType = type;")
                    .AppendLine(@"            string_builder.Clear();")
                    .AppendLine(@"            string_builder.Append(prefix);")
                    .AppendLine(@"        }")
                    .AppendLine(@"")
                    .AppendLine(@"        // End Logger")
                    .AppendLine($"        {dll_import_string}")
                    .AppendLine(@"        private static extern void InitLoggerLogEnd(Action logger);")
                    .AppendLine(@"")
                    .AppendLine(@"        [AOT.MonoPInvokeCallback(typeof(Action))]")
                    .AppendLine(@"        private static void LogEnd()")
                    .AppendLine(@"        {")
                    .AppendLine(@"            Debug.LogFormat(_trackedLogType, LogOption.NoStacktrace, null, string_builder.ToString());")
                    .AppendLine(@"        }")
                    .Append(Loggers)
                    .AppendLine(@"    }")
                    .AppendLine(@"}");
                
                
                return sb.ToString();
            }
        }

        private string Initializers
        {
            get
            {
                var sb = new StringBuilder(500);
                
                foreach (var data in typeData)
                {
                    sb.AppendLine($"            Init{data.name}Logger(Log{data.name});");
                }

                return sb.ToString();
            }
        }

        private string Loggers
        {
            get
            {
                var sb = new StringBuilder(500);
                
                foreach (var data in typeData)
                {
                    sb
                        .AppendLine(@"")
                        .AppendLine($"        // {data.name}")
                        .AppendLine($"        {dll_import_string}")
                        .AppendLine($"        private static extern void Init{data.name}Logger(Action<{data.type}> logger);")
                        .AppendLine(@"")
                        .AppendLine($"        private static void Log{data.name}({data.type} toLog) => string_builder.Append(toLog.ToString());");
                }

                return sb.ToString();
            }
        }
    }
}