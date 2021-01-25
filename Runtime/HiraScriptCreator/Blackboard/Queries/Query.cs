using System.Text;

namespace UnityEngine
{
    [CreateAssetMenu]
    public class Query : HiraCollection<IIndividualQuery>
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        , IHiraScriptCreator
#endif
    {
        [SerializeField] private Blackboard target;
        [SerializeField] private ScriptableObject[] dependencies = { };
        public string @namespace = "UnityEngine";
        [SerializeField] [HideInInspector] private string cachedFilePath = "";

        public string CachedFilePath
        {
            get => cachedFilePath;
            set => cachedFilePath = value;
        }

        public ScriptableObject[] Dependencies => dependencies;

        public string FileName => name;

        public string FileData =>
            new StringBuilder(5000)
                .AppendLine(@"// ReSharper disable All") // it's a generated file, obviously not state of the art
                .AppendLine(@"")
                .AppendLine($"namespace {@namespace}")
                .AppendLine(@"{")
                .AppendLine(@"    [System.Serializable]")
                .AppendLine(@"    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]")
                .AppendLine($"    public struct {name}")
                .AppendLine(@"    {")
                .AppendLine($"        public bool IsSatisfiedBy(ref {target.name} blackboard) =>")
                .AppendLine(Conditions)
                .AppendLine(@"    }")
                .AppendLine(@"}")
                .ToString();

        private string Conditions
        {
            get
            {
                var sb = new StringBuilder(250);
                var conditions = Collection1;
                var conditionsLength = conditions.Length;
                
                for (var i = 0; i < conditionsLength; i++)
                {
                    var condition = conditions[i];
                    sb.Append($"            {condition.Condition}");
                    if (i < conditionsLength - 1) sb.AppendLine(" &&");
                }

                sb.AppendLine(@";");

                return sb.ToString();
            }
        }
    }
}