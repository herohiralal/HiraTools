﻿using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = Application.isPlaying;
            
            if (GUILayout.Button("Test")) 
                ((GameEvent) target).Raise();

            GUI.enabled = true;
        }
    }
}