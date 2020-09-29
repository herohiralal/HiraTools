using System;
using System.Collections.Generic;
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
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new []
                    {
                        keySet.GetOpenDoorAction(1),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorAction(5)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan) =>
                    {
                        Assert.AreEqual(PlannerResult.Success, result);
                        
                        Assert.AreEqual(OpenBreakDoorKeySet.PickupKeyActionName, plan[0].Name);
                        Assert.AreEqual(OpenBreakDoorKeySet.OpenDoorActionName, plan[1].Name);
                    });
                
                planner.GeneratePlan();
            }
        }
        [Test]
        public void inverted_open_break_door_test()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new []
                    {
                        keySet.GetOpenDoorAction(10),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorAction(5)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan) =>
                    {
                        Assert.AreEqual(PlannerResult.Success, result);
                        
                        Assert.AreEqual(OpenBreakDoorKeySet.PickupCrowbarActionName, plan[0].Name);
                        Assert.AreEqual(OpenBreakDoorKeySet.BreakDoorActionName, plan[1].Name);
                    });
                
                planner.GeneratePlan();
            }
        }

        [Test]
        public void inverted_open_break_door_test_requiring_stamina()
        {
            using (var keySet = new OpenBreakDoorKeySet())
            {
                var planner = PlannerTypes.GetPlanner<Action>(keySet.Value.ValueAccessor);
                planner.Initialize()
                    .ForGoal(keySet.GetOpenDoorGoal())
                    .WithAvailableTransitions(new []
                    {
                        keySet.GetOpenDoorAction(10),
                        keySet.GetGetKeyAction(1),
                        keySet.GetGetCrowbarAction(1),
                        keySet.GetBreakDoorActionRequiringStamina(5),
                        keySet.GetDrinkWaterAction(1)
                    })
                    .WithCancellationToken(CancellationToken.None)
                    .WithMaxFScore(1000)
                    .WithCallback((result, plan) =>
                    {
                        Assert.AreEqual(PlannerResult.Success, result);
                        
                        try
                        {
                            Assert.AreEqual(OpenBreakDoorKeySet.PickupCrowbarActionName, plan[0].Name);
                            Assert.AreEqual(OpenBreakDoorKeySet.DrinkWaterActionName, plan[1].Name);
                        }
                        catch (AssertionException)
                        {
                            
                            Assert.AreEqual(OpenBreakDoorKeySet.DrinkWaterActionName, plan[0].Name);
                            Assert.AreEqual(OpenBreakDoorKeySet.PickupCrowbarActionName, plan[1].Name);
                        }

                        Assert.AreEqual(OpenBreakDoorKeySet.BreakDoorActionName, plan[2].Name);
                    });
                
                planner.GeneratePlan();
            }
        }
    }

    internal class OpenBreakDoorKeySet : IDisposable
    {
        public const string OpenDoorActionName = "Open Door";
        public const string BreakDoorActionName = "Break Door";
        public const string PickupKeyActionName = "Pickup Key";
        public const string PickupCrowbarActionName = "Pickup Crowbar";
        public const string DrinkWaterActionName = "Drink Water";
        
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
            Value.Setup(_keys);
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

        public IEnumerable<IBlackboardQuery> GetOpenDoorGoal()
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

            return new Action(PickupKeyActionName, new[] {precondition.Query}, new[] {effect.Modification}, cost);
        }

        public Action GetGetCrowbarAction(float cost)
        {
            var precondition = new SerializableBlackboardQuery();
            precondition.Setup<BoolEqualsValue>(Value, HasCrowbarKey, new BoolReference(false));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, HasCrowbarKey, new BoolReference(true));

            return new Action(PickupCrowbarActionName, new[] {precondition.Query}, new[] {effect.Modification}, cost);
        }

        public Action GetOpenDoorAction(float cost)
        {
            var precondition1 = new SerializableBlackboardQuery();
            precondition1.Setup<BoolEqualsValue>(Value, HasKeyKey, new BoolReference(true));

            var precondition2 = new SerializableBlackboardQuery();
            precondition2.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(false));

            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, DoorOpenKey, new BoolReference(true));

            return new Action(OpenDoorActionName, new[] {precondition1.Query, precondition2.Query},
                new[] {effect.Modification}, cost);
        }

        public Action GetDrinkWaterAction(float cost)
        {
            var effect = new SerializableBlackboardModification();
            effect.Setup<BoolEqualsValue>(Value, HasStaminaKey, new BoolReference(true));
            
            return new Action(DrinkWaterActionName, new IBlackboardQuery[0], new[] {effect.Modification}, cost);
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

            return new Action(BreakDoorActionName, new[] {precondition1.Query, precondition2.Query, precondition3.Query},
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

            return new Action(BreakDoorActionName, new[] {precondition1.Query, precondition2.Query},
                new[] {effect.Modification}, cost);
        }
    }

    internal readonly struct Action : IAction
    {
        public Action(string name, IReadOnlyList<IBlackboardQuery> preconditions,
            IReadOnlyList<IBlackboardModification> effects, float cost)
        {
            Name = name;
            Preconditions = preconditions;
            Effects = effects;
            Cost = cost;
        }

        public string Name { get; }
        public IReadOnlyList<IBlackboardQuery> Preconditions { get; }
        public IReadOnlyList<IBlackboardModification> Effects { get; }
        public float Cost { get; }
    }
}