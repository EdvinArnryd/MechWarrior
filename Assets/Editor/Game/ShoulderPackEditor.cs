using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(ShoulderPack))]
    public class ShoulderPackEditor : Editor
    {
        private void OnSceneGUI()
        {
            Tools.current = Tool.None;
            ShoulderPack sp = target as ShoulderPack;
            sp.Target = Handles.PositionHandle(sp.Target, Quaternion.identity);
            Handles.color = Color.red;
            Handles.SphereHandleCap(0, sp.Target, Quaternion.identity, 0.3f, EventType.Repaint);
        }
    }
}