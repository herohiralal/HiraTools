using System;

namespace UnityEngine
{
    public class HiraBlackboard : HiraControllerComponent, IHiraControllerBlackboard
    {
        [SerializeField] private HiraBlackboardKeySet keySet = null;
        private IBlackboardValueAccessor _mainBlackboard = null;

        public override void Initialize<T>(in T spawnParameters)
        {
            _mainBlackboard = BlackboardTypes.GetMainValueAccessor(keySet);
        }

        public override void OnPossess(HiraCreature inCreature)
        {
        }

        public override void OnDispossess()
        {
            _mainBlackboard.ResetValues();
        }

        public override void Stop()
        {
            _mainBlackboard = null;
        }

        public IReadOnlyBlackboardDataSet DataSet => _mainBlackboard.DataSet;


        public event Action OnValueUpdate
        {
            add => _mainBlackboard.OnValueUpdate += value;
            remove => _mainBlackboard.OnValueUpdate -= value;
        }

        public void ResetValues() => _mainBlackboard.ResetValues();

        public override void DoGUI()
        {
            GUILayout.BeginVertical();
            var data = "Blackboard -\n";
            var keys = keySet.Keys;
            foreach (var key in keys)
            {
                var keyName = key.Name;
                switch (key.KeyType)
                {
                    case BlackboardKeyType.Undefined:
                        break;
                    case BlackboardKeyType.Bool:
                        data += $"{keyName}: {this.GetBooleanValue(keyName)}\n";
                        break;
                    case BlackboardKeyType.Float:
                        data += $"{keyName}: {this.GetFloatValue(keyName)}\n";
                        break;
                    case BlackboardKeyType.Int:
                        data += $"{keyName}: {this.GetIntValue(keyName)}\n";
                        break;
                    case BlackboardKeyType.String:
                        data += $"{keyName}: {this.GetStringValue(keyName)}\n";
                        break;
                    case BlackboardKeyType.Vector:
                        data += $"{keyName}: {this.GetVectorValue(keyName)}\n";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            GUILayout.Label(data);
            GUILayout.EndVertical();
        }

        #region Getters

        public uint GetHash(in string keyName) => _mainBlackboard.GetHash(in keyName);

        public bool GetBooleanValue(uint hash) => _mainBlackboard.GetBooleanValue(hash);

        public bool GetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex) =>
            _mainBlackboard.GetBooleanValueWithTypeSpecificIndex(typeSpecificIndex);

        public float GetFloatValue(uint hash) => _mainBlackboard.GetFloatValue(hash);

        public float GetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex) =>
            _mainBlackboard.GetFloatValueWithTypeSpecificIndex(typeSpecificIndex);

        public int GetIntValue(uint hash) => _mainBlackboard.GetIntValue(hash);

        public int GetIntValueWithTypeSpecificIndex(uint typeSpecificIndex) =>
            _mainBlackboard.GetIntValueWithTypeSpecificIndex(typeSpecificIndex);

        public string GetStringValue(uint hash) => _mainBlackboard.GetStringValue(hash);

        public string GetStringValueWithTypeSpecificIndex(uint typeSpecificIndex) =>
            _mainBlackboard.GetStringValueWithTypeSpecificIndex(typeSpecificIndex);

        public Vector3 GetVectorValue(uint hash) => _mainBlackboard.GetVectorValue(hash);

        public Vector3 GetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex) =>
            _mainBlackboard.GetVectorValueWithTypeSpecificIndex(typeSpecificIndex);

        #endregion

        #region Setters

        public void SetBooleanValue(uint hash, bool value) => _mainBlackboard.SetBooleanValue(hash, value);

        public void SetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex, bool value) =>
            _mainBlackboard.SetBooleanValueWithTypeSpecificIndex(typeSpecificIndex, value);

        public void SetFloatValue(uint hash, float value) => _mainBlackboard.SetFloatValue(hash, value);

        public void SetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex, float value) =>
            _mainBlackboard.SetFloatValueWithTypeSpecificIndex(typeSpecificIndex, value);

        public void SetIntValue(uint hash, int value) => _mainBlackboard.SetIntValue(hash, value);

        public void SetIntValueWithTypeSpecificIndex(uint typeSpecificIndex, int value) =>
            _mainBlackboard.SetIntValueWithTypeSpecificIndex(typeSpecificIndex, value);

        public void SetStringValue(uint hash, string value) => _mainBlackboard.SetStringValue(hash, value);

        public void SetStringValueWithTypeSpecificIndex(uint typeSpecificIndex, string value) =>
            _mainBlackboard.SetStringValueWithTypeSpecificIndex(typeSpecificIndex, value);

        public void SetVectorValue(uint hash, Vector3 value) => _mainBlackboard.SetVectorValue(hash, value);

        public void SetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex, Vector3 value) =>
            _mainBlackboard.SetVectorValueWithTypeSpecificIndex(typeSpecificIndex, value);

        #endregion
    }
}