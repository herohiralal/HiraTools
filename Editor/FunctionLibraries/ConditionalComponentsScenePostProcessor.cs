using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEditor.Internal
{
	internal static class ConditionalComponentsScenePostProcessor
	{
		[PostProcessScene(0)]
		public static void Strip()
		{
			if (!BuildPipeline.isBuildingPlayer) return;

			Debug.Log($"Stripping Editor-Only objects.");

			var deletedBehaviours = new List<MonoBehaviour>();
			var deletedGameObjects = new List<GameObject>();
			
			foreach (var behaviour in Object.FindObjectsOfType<MonoBehaviour>())
			{
				var type = behaviour.GetType();

				if (ConditionalComponentHelpers.IsComponentEditorOnly(type))
				{
					deletedBehaviours.Add(behaviour);
				}

				var behaviourGameObject = behaviour.gameObject;

				if (!deletedGameObjects.Contains(behaviourGameObject)
				    && ConditionalComponentHelpers.IsGameObjectEditorOnly(type))
				{
					deletedGameObjects.Add(behaviourGameObject);
				}
			}

			Debug.Log($"Stripping {deletedGameObjects.Count} game object(s) & {deletedBehaviours.Count} component(s).");

			foreach (var deletedBehaviour in deletedBehaviours)
			{
				try
				{
					Object.DestroyImmediate(deletedBehaviour);
				}
				catch (Exception e)
				{
					throw new BuildFailedException(e);
				}
			}

			foreach (var deletedGameObject in deletedGameObjects)
			{
				try
				{
					Object.DestroyImmediate(deletedGameObject);
				}
				catch (Exception e)
				{
					throw new BuildFailedException(e);
				}
			}
		}
	}
}