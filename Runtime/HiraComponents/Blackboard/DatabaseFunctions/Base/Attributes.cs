using System;

namespace HiraEngine.Components.Blackboard.Internal
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class HiraBlackboardDecoratorAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class HiraBlackboardScoreCalculatorAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class HiraBlackboardEffectorAttribute : Attribute
	{
	}
}