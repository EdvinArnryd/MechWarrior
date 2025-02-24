using Math;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class Leg : MonoBehaviour
    {
        [SerializeField]
        public Transform[]      m_bones = new Transform[3];

        [SerializeField]
        public Vector3[]        m_boneAngleOffsets = new Vector3[3];

        [SerializeField, Header("Stepping"), Range(0.2f, 3.0f)]
        public float            m_fStepTrigger = 1.2f;

        [SerializeField, Range(0.2f, 5.0f)]
        public float            m_fMaxStepLength = 2.5f;

        [SerializeField, Range(0.0f, 2.0f)]
        public float            m_fOverstepLength = 0.5f;

        [SerializeField, Range(0.5f, 5.0f)]
        public float            m_fStepSpeed = 2.0f;

        private List<Leg>       m_otherLegs;
        private Mech            m_mech;

        Vector3 m_vLocalGroundPos;

        #region Properties

        public Pose TargetPose { get; set; }

        public Vector3 Pole { get; set; }

        public Pose GroundPose
        {
            get
            {
                // snap to terrain
                Pose gp = new Pose(m_mech.transform.TransformPoint(m_vLocalGroundPos), m_mech.transform.rotation);

                if (m_mech != null && m_mech.m_terrain != null)
                {
                    // snap position
                    RaycastHit hit;
                    if (m_mech.m_terrain.Raycast(new Ray(gp.position + Vector3.up * 5.0f, Vector3.down), out hit, 10.0f))
                    {
                        gp.position = hit.point + Vector3.up * 0.5f;

                        // calculate rotation
                        RaycastHit hitForward, hitRight;
                        if (m_mech.m_terrain.Raycast(new Ray(gp.position + gp.forward * 0.25f + Vector3.up * 5.0f, Vector3.down), out hitForward, 10.0f) &&
                            m_mech.m_terrain.Raycast(new Ray(gp.position + gp.right * 0.25f + Vector3.up * 5.0f, Vector3.down), out hitRight, 10.0f))
                        {
                            Vector3 vToForward = hitForward.point - hit.point;
                            Vector3 vToRight = hitRight.point - hit.point;
                            Vector3 vUp = Vector3.Cross(vToForward, vToRight);
                            gp.rotation = Quaternion.LookRotation(vToForward, vUp);
                        }
                    }
                }

                return gp;
            }
        }

        public bool ShouldStep
        {
            get
            {
                return Vector3.Distance(TargetPose.position, GroundPose.position) > m_fStepTrigger ||
                       Quaternion.Angle(TargetPose.rotation, GroundPose.rotation) > 60.0f;
            }
        }

        public bool CanStep => m_otherLegs.FindIndex(l => l.IsStepping) < 0;

        public bool IsStepping { get; private set; }

        public Transform Foot => m_bones[2];

        public float Lift { get; private set; }

        public bool IsRight => name.EndsWith("_R");     // UGLY

        #endregion

        private void OnEnable()
        {
            m_mech = GetComponentInParent<Mech>();

            // get other legs
            m_otherLegs = new List<Leg>(transform.parent.GetComponentsInChildren<Leg>());
            m_otherLegs.Remove(this);

            // get ground position (in pelvis space)
            Vector3 vGroundPos = transform.position;
            vGroundPos.y = 0.5f;
            m_vLocalGroundPos = m_mech.transform.InverseTransformPoint(vGroundPos);
            m_vLocalGroundPos.x *= 1.5f;

            // initialize target pose
            TargetPose = GroundPose;

            // start stepping
            StartCoroutine(SteppingLogic());
        }

        private void LateUpdate()
        {
            UpdateLeg();
        }

        public void UpdateLeg()
        {
            if (m_bones == null || m_bones.Length != 3 || 
                m_boneAngleOffsets == null || m_boneAngleOffsets.Length != 3)
            {
                return;
            }

            // solve leg IK
            InverseKinematics.TwoBoneIK(m_bones, m_boneAngleOffsets, TargetPose.position, Pole);

            // rotate foot
            m_bones[2].rotation = TargetPose.rotation;

            // set the pole
            Transform pelvis = transform.parent;
            Pole = (m_bones[2].position + pelvis.position) * 0.5f + pelvis.forward * 4.0f;
        }

        IEnumerator SteppingLogic()
        {
            while (true)
            {
                if (ShouldStep && CanStep)
                {
                    IsStepping = true;

                    Pose start = TargetPose;
                    Pose goal = GroundPose;

                    // overstep then cap the step length
                    Vector3 vStepTarget = goal.position + Vector3.Normalize(goal.position - start.position) * m_fOverstepLength;
                    if (Vector3.Distance(start.position, vStepTarget) > m_fMaxStepLength)
                    {
                        vStepTarget = Vector3.MoveTowards(start.position, goal.position, m_fMaxStepLength);
                    }

                    for (float f = 0.0f; f < 1.0f; f += Time.deltaTime * m_fStepSpeed)
                    {
                        Lift = 1.0f - Mathf.Abs(f - 0.5f) / 0.5f;
                        Vector3 vLift = Vector3.up * MathUtil.SmoothStep(Lift) * 1.2f;
                        TargetPose = new Pose(Vector3.Lerp(start.position, vStepTarget, f) + vLift,
                                              Quaternion.Slerp(start.rotation, goal.rotation, f));                    
                        yield return null;
                    }

                    IsStepping = false;
                }            

                yield return null;
            }
        }
    }
}