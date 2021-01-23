﻿using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using static HiraTests.HiraEngine.Components.Blackboard.BlackboardTestsCommon;

namespace HiraTests.HiraEngine.Components.Blackboard
{
    [TestFixture]
    public class ValueAccessor
    {
        [Test]
        public void set_value_is_reflected_when_getting_it([Values(0, 1, 2, 99, 100)] int input)
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                mainAccessor.SetIntValue(INT_KEY, input);

                var value = mainAccessor.GetIntValue(INT_KEY);
                Assert.AreEqual(input, value);
            }
        }

        [Test]
        public void hash_and_key_name_should_refer_to_the_same_value(
            [Values("Haha", "JasonSchneider", "NotJasonSchneider")]
            string input)
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
                var hash = mainAccessor.GetHash(STRING_KEY);

                mainAccessor.SetStringValue(hash, input);

                Assert.AreEqual(input, mainAccessor.GetStringValue(STRING_KEY));
            }
        }

        [Test]
        public void blackboard_throws_exception_for_garbage_key_names(
            [Values("RandomKey", "Hah", "Stupid")] string input)
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                NUnit.Framework.Assert.Throws<KeyNotFoundException>(() => mainAccessor.GetStringValue(input));
            }
        }

        [Test]
        public void instance_synced_keys_have_instance_synced_values([Values(2.538f, 934.1f, 83410.92103f, 798213.123f)]
            float input)
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var firstAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
                var secondAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                firstAccessor.SetFloatValue(FLOAT_KEY_INSTANCE_SYNCED, input);

                Assert.AreEqual(input, firstAccessor.GetFloatValue(FLOAT_KEY_INSTANCE_SYNCED));
                Assert.AreEqual(input, secondAccessor.GetFloatValue(FLOAT_KEY_INSTANCE_SYNCED));
            }
        }

        [Test]
        public void instance_synced_keys_can_be_modified_from_key_set_directly(
            [Random(float.MinValue, float.MaxValue, 2)]
            float inputX,
            [Random(float.MinValue, float.MaxValue, 2)]
            float inputY,
            [Random(float.MinValue, float.MaxValue, 2)]
            float inputZ)
        {
            var vector = new Vector3(inputX, inputY, inputZ);

            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                keyData.ValueAccessor.SetVectorValue(VECTOR_KEY_INSTANCE_SYNCED, vector);

                Assert.AreEqual(vector, mainAccessor.GetVectorValue(VECTOR_KEY_INSTANCE_SYNCED));
            }
        }

        [Test]
        public void setting_values_invokes_callback()
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                var checkedValue = false;
                mainAccessor.OnValueUpdate += () => checkedValue = true;

                mainAccessor.SetBooleanValue(BOOLEAN_KEY, false);

                Assert.AreEqual(true, checkedValue);
            }
        }

        [Test]
        public void setting_instance_synced_values_invokes_callback([Random(float.MinValue, float.MaxValue, 4)]
            float input)
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
                var secondAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                var checkedValue = false;
                mainAccessor.OnValueUpdate += () => checkedValue = true;

                secondAccessor.SetFloatValue(FLOAT_KEY_INSTANCE_SYNCED, input);

                Assert.AreEqual(true, checkedValue);
            }
        }

        [Test]
        public void setting_a_value_from_the_key_set_changes_values_of_all_blackboards(
            [Random(float.MinValue, float.MaxValue, 3)]
            float input)
        {
            using (var resource = Resource)
            {
                var keyData = BlackboardTypes.GetKeyData(resource.Keys);
                var mainAccessor = BlackboardTypes.GetMainValueAccessor(keyData);
                var secondAccessor = BlackboardTypes.GetMainValueAccessor(keyData);

                keyData.ValueAccessor.SetFloatValue(FLOAT_KEY, input);

                Assert.AreEqual(input, mainAccessor.GetFloatValue(FLOAT_KEY));
                Assert.AreEqual(input, secondAccessor.GetFloatValue(FLOAT_KEY));
            }
        }
    }
}