using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(Leg))]
    public class LegEditor : Editor
    {
        private void OnSceneGUI()
        {
            Tools.current = Tool.None;
            Leg leg = target as Leg;

            // edit target
            Pose p = leg.TargetPose;
            Vector3 vNewPos = Handles.PositionHandle(p.position, p.rotation.normalized);
            if (Vector3.Distance(vNewPos, p.position) > 0.0001f)
            {
                p.position = vNewPos;
                leg.TargetPose = p;
                leg.UpdateLeg();
            }

            Quaternion qNewRot = Handles.RotationHandle(p.rotation.normalized, p.position);
            if (Quaternion.Angle(qNewRot, p.rotation) > 0.001f)
            {
                p.rotation = qNewRot;
                leg.TargetPose = p;
                leg.UpdateLeg();
            }
            Handles.color = Color.red;
            Handles.SphereHandleCap(0, p.position, Quaternion.identity, 0.3f, EventType.Repaint);

            // edit pole
            Vector3 vNewPole = Handles.PositionHandle(leg.Pole, Quaternion.identity);
            if (Vector3.Distance(vNewPole, leg.Pole) > 0.0001f)
            {
                leg.Pole = vNewPole;
                leg.UpdateLeg();
            }

            Handles.color = Color.yellow;
            Handles.SphereHandleCap(0, leg.Pole, Quaternion.identity, 0.3f, EventType.Repaint);

            // draw ground pose
            Handles.color = Color.green;
            Handles.CubeHandleCap(0, leg.GroundPose.position, leg.GroundPose.rotation, 0.3f, EventType.Repaint);
        }
    }
}