namespace UnityEngine
{
	public class BlackboardGoal : HiraCollection<IIndividualQuery, IIndividualQuery>, IBlackboardGoal
	{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
#pragma warning disable 414
		private static readonly string collection1_name = "Requirements";
		private static readonly string collection2_name = "Targets";
#pragma warning restore 414
#endif
		
		public string Name => name;
		public string TargetHeuristicString =>
			Collection2.Length > 0
				? Collection2
					.ConcatenateStringsWith(
						q => $"({q.Condition} ? 0 : {q.Weight.ToCode()})",
						" + ")
				: "0";
	}
}