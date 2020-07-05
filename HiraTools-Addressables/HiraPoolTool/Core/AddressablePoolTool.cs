﻿using System;
using System.Collections;
using HiraPoolTool.Abstractions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine
{
    [AddComponentMenu("HiraTools/Pooling/PoolTool (Addressable)")]
    public class AddressablePoolTool : PoolToolAbstract
    {
        [SerializeField] private AssetReference targetReference = null;

        #region Memory management

        public override void LoadResource(Func<GameObject, Component> getTarget = null, byte? initialPopulateOverride = null)
        {
            getTarget ??= go => go.transform;
            StartCoroutine(LoadResourceEnumerator(getTarget, initialPopulateOverride));
        }
        
        private IEnumerator LoadResourceEnumerator(Func<GameObject, Component> getTarget, byte? initialPopulateOverride)
        {
            if (IsResourceLoaded) yield break;
            var operation = targetReference.LoadAssetAsync<GameObject>();
            yield return operation;
            if (operation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogErrorFormat(this, $"Failed to load {targetReference}.");
                yield break;
            }

            Pool = GetPool(getTarget(operation.Result));

            if (initialPopulateOverride.HasValue) Pool.AddToPool(initialPopulateOverride.Value);
            else if (initialPopulate > 0) Pool.AddToPool(initialPopulate);
        }

        public override void UnloadResource()
        {
            if (!IsResourceLoaded) return;

            Pool.Dispose();
            Pool = null;
            targetReference.ReleaseAsset();
        }

        #endregion
    }
}