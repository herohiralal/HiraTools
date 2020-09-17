using System.Collections.Generic;
using HiraCreatures.Components.Blackboard.Helpers;
using HiraCreatures.Components.Blackboard.Internal;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hiralal.Components.Blackboard.Tests
{
    [TestFixture]
    public class ValueSetConfirmations
    {
        private const string boolean_key = "Boolean";
        private const string float_key = "Float";
        private const string int_key = "Integer";
        private const string string_key = "String";
        private const string vector_key = "Vector";
        private const string float_key_instance_synced = "InstanceSyncedFloat";
        private const string vector_key_instance_synced = "InstanceSyncedVector";

        private static SerializableKey[] Keys =>
            new[]
            {
                new SerializableKey(boolean_key, BlackboardKeyType.Bool, false),
                new SerializableKey(float_key, BlackboardKeyType.Float, false),
                new SerializableKey(int_key, BlackboardKeyType.Int, false),
                new SerializableKey(string_key, BlackboardKeyType.String, false),
                new SerializableKey(vector_key, BlackboardKeyType.Vector, false),
                new SerializableKey(float_key_instance_synced, BlackboardKeyType.Float, true),
                new SerializableKey(vector_key_instance_synced, BlackboardKeyType.Vector, true)
            };

        [Test]
        public void integer_value_should_correctly_set([Values(0, 1, 2, 99, 100)] int input)
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

            mainAccessor.SetIntValue(int_key, input);

            var value = mainAccessor.GetIntValue(int_key);
            Assert.AreEqual(input, value);

            keyData.Deactivate();
        }

        [Test]
        public void hash_and_key_name_should_refer_to_the_same_value([Values("Haha", "Noooo", "Yeahhhhh")] string input)
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
            var hash = mainAccessor.GetHash(string_key);

            mainAccessor.SetStringValue(hash, input);

            Assert.AreEqual(input, mainAccessor.GetStringValue(string_key));

            keyData.Deactivate();
        }

        [Test]
        public void blackboard_throws_exception_for_garbage_key_names(
            [Values("RandomKey", "Hah", "Stupid")] string input)
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

            NUnit.Framework.Assert.Throws<KeyNotFoundException>(() => mainAccessor.GetStringValue(input));

            keyData.Deactivate();
        }

        [Test]
        public void instance_synced_keys_have_instance_synced_values([Values(2.538f, 934.1f, 83410.92103f, 798213.123f)]
            float input)
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var firstAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
            var secondAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

            firstAccessor.SetFloatValue(float_key_instance_synced, input);

            Assert.AreEqual(input, firstAccessor.GetFloatValue(float_key_instance_synced));
            Assert.AreEqual(input, secondAccessor.GetFloatValue(float_key_instance_synced));

            keyData.Deactivate();
        }

        [Test]
        public void instance_synced_keys_can_be_modified_from_key_set_directly(
            [Random(float.MinValue, float.MaxValue, 2)] float inputX,
            [Random(float.MinValue, float.MaxValue, 2)] float inputY,
            [Random(float.MinValue, float.MaxValue, 2)] float inputZ)
        {
            var vector = new Vector3(inputX, inputY, inputZ);
            
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

            keyData.ValueAccessor.SetVectorValue(vector_key_instance_synced, vector);
            
            Assert.AreEqual(vector, mainAccessor.GetVectorValue(vector_key_instance_synced));

            keyData.Deactivate();
        }

        [Test]
        public void setting_values_invokes_callback()
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

            var checkedValue = false;
            mainAccessor.OnValueUpdate += () => checkedValue = true;

            mainAccessor.SetBooleanValue(boolean_key, false);

            Assert.AreEqual(true, checkedValue);

            keyData.Deactivate();
        }

        [Test]
        public void setting_instance_synced_values_invokes_callback([Random(float.MinValue, float.MaxValue, 4)] float input)
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
            var secondAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

            var checkedValue = false;
            mainAccessor.OnValueUpdate += () => checkedValue = true;

            secondAccessor.SetFloatValue(float_key_instance_synced, input);

            Assert.AreEqual(true, checkedValue);

            keyData.Deactivate();
        }

        [Test]
        public void duplicating_a_data_set_should_give_a_new_object(
            [Random(0u, 20u, 2)] uint boolCount,
            [Random(0u, 20u, 2)] uint floatCount,
            [Random(0u, 20u, 2)] uint intCount,
            [Random(0u, 20u, 2)] uint stringCount,
            [Random(0u, 20u, 2)] uint vectorCount
            )
        {
            var dataSet = BlackboardTypes.GetWriteableDataSet(boolCount, floatCount, intCount, stringCount, vectorCount);
            var duplicate = dataSet.GetDuplicate();

            Assert.AreNotEqual(duplicate, dataSet);
            Assert.AreNotEqual(duplicate.Booleans, dataSet.Booleans);
            Assert.AreNotEqual(duplicate.Floats, dataSet.Floats);
            Assert.AreNotEqual(duplicate.Integers, dataSet.Integers);
            Assert.AreNotEqual(duplicate.Strings, dataSet.Strings);
            Assert.AreNotEqual(duplicate.Vectors, dataSet.Vectors);
            Assert.AreEqual(boolCount, duplicate.Booleans.Length);
            Assert.AreEqual(floatCount, duplicate.Floats.Length);
            Assert.AreEqual(intCount, duplicate.Integers.Length);
            Assert.AreEqual(stringCount, duplicate.Strings.Length);
            Assert.AreEqual(vectorCount, duplicate.Vectors.Length);
        }

        [Test]
        public void duplicating_a_data_set_should_duplicate_values(
            [Random(float.MinValue, float.MaxValue, 2)] float first,
            [Random(float.MinValue, float.MaxValue, 2)] float second,
            [Random(float.MinValue, float.MaxValue, 2)] float third)
        {
            var dataSet = BlackboardTypes.GetWriteableDataSet(0, 3, 0, 0, 0);
            (dataSet.Floats[0], dataSet.Floats[1], dataSet.Floats[2]) = (first, second, third);

            var duplicate = dataSet.GetDuplicate();
            
            Assert.AreEqual(first, duplicate.Floats[0]);
            Assert.AreEqual(second, duplicate.Floats[1]);
            Assert.AreEqual(third, duplicate.Floats[2]);

            (dataSet.Floats[0], dataSet.Floats[1], dataSet.Floats[2]) = (third, first, second);
            
            Assert.AreEqual(first, duplicate.Floats[0]);
            Assert.AreEqual(second, duplicate.Floats[1]);
            Assert.AreEqual(third, duplicate.Floats[2]);
        }

        [Test]
        public void setting_a_value_from_the_key_set_changes_values_of_all_blackboards([Random(float.MinValue, float.MaxValue, 3)] float input)
        {
            var keyData = BlackboardTypes.GetKeyData(Keys);
            keyData.Activate();
            var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
            var secondAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
            
            keyData.ValueAccessor.SetFloatValue(float_key, input);
            
            Assert.AreEqual(input, mainAccessor.GetFloatValue(float_key));
            Assert.AreEqual(input, secondAccessor.GetFloatValue(float_key));

            keyData.Deactivate();
        }
    }
}