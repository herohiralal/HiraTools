namespace UnityEngine
{
	public class BlackboardAction : HiraCollection<IIndividualQuery, IIndividualModification>, IBlackboardAction
	{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
#pragma warning disable 414
		private static readonly string collection1_name = "Preconditions";
		private static readonly string collection2_name = "Effects";
#pragma warning restore 414
#endif
		
		public string Name => name;
		public string PreconditionCheck =>
			Collection1
				.ConcatenateStringsWith(
					q => q.Condition,
					" && ");

		public string ApplyEffect =>
			Collection2
				.ConcatenateStringsWith(m => $" {m.Modification}",
					";");
	}
}