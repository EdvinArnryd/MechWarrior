using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(Mech), true)]
    public class MechEditor : Editor
    {
        private void OnSceneGUI()
        {
            Tools.current = Tool.None;
            Mech mech = target as Mech;

            // edit target
            Pose p = mech.MoveTarget;
            Vector3 vNewPos = Handles.PositionHandle(p.position, p.rotation.normalized);
            if (Vector3.Distance(vNewPos, p.position) > 0.0001f)
            {
                p.position = vNewPos;
                mech.MoveTarget = p;
            }

            Quaternion qNewRot = Handles.RotationHandle(p.rotation.normalized, p.position);
            if (Quaternion.Angle(qNewRot, p.rotation) > 0.001f)
            {
                p.rotation = qNewRot;
                mech.MoveTarget = p;
            }
            Handles.color = Color.red;
            Handles.SphereHandleCap(0, p.position, Quaternion.identity, 0.3f, EventType.Repaint);
        }
    }
}