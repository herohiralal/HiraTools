using System;
using System.Threading;
using HiraEngine.Components.Planner.Internal;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using AssertionException = UnityEngine.Assertions.AssertionException;
using Object = UnityEngine.Object;

namespace HiraTests.HiraEngine.Components.Planner
{
    [TestFixture]
    public class PlannerCore
    {
        [Test]
        public void simple_open_break_door_test()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor, 2);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new[]
                    {
                        keySet.GetOpenDoorAction(1),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorAction(5)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan, planLength) =>
                    {
                        Assert.AreEqual(PlannerResult.Success, result);

                        Assert.AreEqual(OpenBreakDoorKeySet.PICKUP_KEY_ACTION_NAME, plan[0].Name);
                        Assert.AreEqual(OpenBreakDoorKeySet.OPEN_DOOR_ACTION_NAME, plan[1].Name);
                    });

                planner.Run();
            }
        }

        [Test]
        public void inverted_open_break_door_test()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor, 5);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new[]
                    {
                        keySet.GetOpenDoorAction(10),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorAction(5)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan, planLength) =>
                    {
                        Assert.AreEqual(PlannerResult.Success, result);

                        Assert.AreEqual(OpenBreakDoorKeySet.PICKUP_CROWBAR_ACTION_NAME, plan[0].Name);
                        Assert.AreEqual(OpenBreakDoorKeySet.BREAK_DOOR_ACTION_NAME, plan[1].Name);
                    });

                planner.Run();
            }
        }

        [Test]
        public void inverted_open_break_door_test_requiring_stamina()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor, 10);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new[]
                    {
                        keySet.GetOpenDoorAction(10),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorActionRequiringStamina(5),
                        keySet.GetDrinkWaterAction(1)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan, planLength) =>
                    {
                        Assert.AreEqual(PlannerResult.Success, result);

                        try
                        {
                            Assert.AreEqual(OpenBreakDoorKeySet.PICKUP_CROWBAR_ACTION_NAME, plan[0].Name);
                            Assert.AreEqual(OpenBreakDoorKeySet.DRINK_WATER_ACTION_NAME, plan[1].Name);
                        }
                        catch (AssertionException)
                        {
                            Assert.AreEqual(OpenBreakDoorKeySet.DRINK_WATER_ACTION_NAME, plan[0].Name);
                            Assert.AreEqual(OpenBreakDoorKeySet.PICKUP_CROWBAR_ACTION_NAME, plan[1].Name);
                        }

                        Assert.AreEqual(OpenBreakDoorKeySet.BREAK_DOOR_ACTION_NAME, plan[2].Name);
                    });

                planner.Run();
            }
        }

        [Test]
        public void plan_failure_on_action_limit_cross()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor, 1);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new[]
                    {
                        keySet.GetOpenDoorAction(1),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorAction(5)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan, planLength) =>
                    {
                        Assert.AreEqual(PlannerResult.Failure, result);
                    });

                planner.Run();
            }
        }

        [Test]
        public void plan_failure_on_f_score_crossed()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor, 3);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new[]
                    {
                        keySet.GetOpenDoorAction(1),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorAction(5)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1)
                    .WithCallback((result, plan, planLength) =>
                    {
                        Assert.AreEqual(PlannerResult.Failure, result);
                    });

                planner.Run();
            }
        }
    }

    internal class OpenBreakDoorKeySet : IDisposable
    {
        public const string OPEN_DOOR_ACTION_NAME = "Open Door";
        public const string BREAK_DOOR_ACTION_NAME = "Break Door";
        public const string PICKUP_KEY_ACTION_NAME = "Pickup Key";
        public const string PICKUP_CROWBAR_ACTION_NAME = "Pickup Crowbar";
        public const string DRINK_WATER_ACTION_NAME = "Drink Water";

        public OpenBreakDoorKeySet()
        {
            DoorOpenKey = ScriptableObject.CreateInstance<SerializableBlackboardKey>();
            DoorOpenKey.Setup("Door Is Open", BlackboardKeyType.Bool, false);

            HasKeyKey = ScriptableObject.CreateInstance<SerializableBlackboardKey>();
            HasKeyKey.Setup("Has Key", BlackboardKeyType.Bool, false);

            HasCrowbarKey = ScriptableObject.CreateInstance<SerializableBlackboardKey>();
            HasCrowbarKey.Setup("Has Crowbar", BlackboardKeyType.Bool, false);

            HasStaminaKey = ScriptableObject.CreateInstance<SerializableBlackboardKey>();
            HasStaminaKey.Setup("Has Stamina", BlackboardKeyType.Bool, false);

            _keys = new[] {DoorOpenKey, HasKeyKey, HasCrowbarKey, HasStaminaKey};
            Value = ScriptableObject.CreateInstance<HiraBlackboardKeySet>();
            Value.Collection1 = _keys;
            Value.Initialize();
        }

        private readonly SerializableBlackboardKey[] _keys = null;
        private SerializableBlackboardKey DoorOpenKey { get; }
        private SerializableBlackboardKey HasKeyKey { get; }
        private SerializableBlackboardKey HasCrowbarKey { get; }
        private SerializableBlackboardKey HasStaminaKey { get; }
        public HiraBlackboardKeySet Value { get; }

        public void Dispose()
        {
            Object.DestroyImmediate(Value);
            foreach (var key in _keys) Object.DestroyImmediate(key);
        }

        public IBlackboardQuery[] GetOpenDoorGoal()
        {
            var goal = new SerializableBlackboardQuery();
            goal.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(true));
            return new[] {goal.Query};
        }

        public Action GetGetKeyAction(float cost)
        {
            var precondition = new SerializableBlackboardQuery();
            precondition.Setup<BoolEqualsValue>(Value, HasKeyKey, new BoolReference(false));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, HasKeyKey, new BoolReference(true));

            return new Action(PICKUP_KEY_ACTION_NAME, new[] {precondition.Query}, new[] {effect.Modification}, cost);
        }

        public Action GetGetCrowbarAction(float cost)
        {
            var precondition = new SerializableBlackboardQuery();
            precondition.Setup<BoolEqualsValue>(Value, HasCrowbarKey, new BoolReference(false));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, HasCrowbarKey, new BoolReference(true));

            return new Action(PICKUP_CROWBAR_ACTION_NAME, new[] {precondition.Query}, new[] {effect.Modification}, cost);
        }

        public Action GetOpenDoorAction(float cost)
        {
            var precondition1 = new SerializableBlackboardQuery();
            precondition1.Setup<BoolEqualsValue>(Value, HasKeyKey, new BoolReference(true));

            var precondition2 = new SerializableBlackboardQuery();
            precondition2.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(false));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(true));

            return new Action(OPEN_DOOR_ACTION_NAME, new[] {precondition1.Query, precondition2.Query},
                new[] {effect.Modification}, cost);
        }

        public Action GetDrinkWaterAction(float cost)
        {
            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, HasStaminaKey, new BoolReference(true));

            return new Action(DRINK_WATER_ACTION_NAME, new IBlackboardQuery[0], new[] {effect.Modification}, cost);
        }

        public Action GetBreakDoorActionRequiringStamina(float cost)
        {
            var precondition1 = new SerializableBlackboardQuery();
            precondition1.Setup<BoolEqualsValue>(Value, HasCrowbarKey, new BoolReference(true));

            var precondition2 = new SerializableBlackboardQuery();
            precondition2.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(false));

            var precondition3 = new SerializableBlackboardQuery();
            precondition3.Setup<BoolEqualsValue>(Value, HasStaminaKey, new BoolReference(true));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(true));

            return new Action(BREAK_DOOR_ACTION_NAME,
                new[] {precondition1.Query, precondition2.Query, precondition3.Query},
                new[] {effect.Modification}, cost);
        }

        public Action GetBreakDoorAction(float cost)
        {
            var precondition1 = new SerializableBlackboardQuery();
            precondition1.Setup<BoolEqualsValue>(Value, HasCrowbarKey, new BoolReference(true));

            var precondition2 = new SerializableBlackboardQuery();
            precondition2.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(false));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(true));

            return new Action(BREAK_DOOR_ACTION_NAME, new[] {precondition1.Query, precondition2.Query},
                new[] {effect.Modification}, cost);
        }
    }

    internal readonly struct Action : IAction
    {
        public Action(string name, IBlackboardQuery[] preconditions, IBlackboardModification[] effects, float cost)
        {
            Name = name;
            Preconditions = preconditions;
            Effects = effects;
            Cost = cost;
        }

        public string Name { get; }
        public IBlackboardQuery[] Preconditions { get; }
        public IBlackboardModification[] Effects { get; }

        public void BuildPrePlanCache()
        {
        }

        public float Cost { get; }
    }
}