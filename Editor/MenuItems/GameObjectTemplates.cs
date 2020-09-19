using HiraEngine.HiraWorlds;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.MenuItems
{
    internal static class GameObjectTemplates
    {
        [MenuItem("GameObject/Hiralal/HiraWorld Volume", false, 10)]
        private static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var go = new GameObject("HiraWorld Volume");
            go.AddComponent<BoxCollider>().isTrigger = true;
            go.AddComponent<HiraWorldVolume>();
            
            if (menuCommand.context is GameObject gameObject) 
                go.transform.SetParent(gameObject.transform);
        }
    }
}