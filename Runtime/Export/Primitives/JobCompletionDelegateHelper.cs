using System;

namespace Unity.Jobs
{
	public struct JobCompletionDelegateHelper
	{
		public JobHandle TrackedJobHandle;
		public Action OnCompletion;

		public void Invoke()
		{
			TrackedJobHandle.Complete();
			OnCompletion.Invoke();
		}
	}
}