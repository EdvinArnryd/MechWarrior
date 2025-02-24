using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShoulderPack : WeaponSystem
    {
        [SerializeField, Range(0.1f, 5.0f)]
        public float            m_fRotationSpeed = 1.0f;

        [SerializeField, Range(10.0f, 60.0f)]
        public float            m_fAngleLimit = 30.0f;

        private MechPart        m_targetPart;

        #region Properties

        public Vector3 Target { get; set; }

        public override bool IsReady => m_targetPart != null && DistanceToLock < 1.0f;

        protected Vector3 DesiredTarget
        {
            get
            {
                if (m_targetPart != null)
                {
                    return m_targetPart.transform.position;
                }

                return Mech.transform.position + Mech.transform.forward * 20.0f + Vector3.up * 4.0f;
            }
        }

        public float DistanceToLock => Vector3.Distance(Target, DesiredTarget);

        #endregion

        private void OnEnable()
        {
            Target = transform.position + transform.forward * 5.0f;
        }

        private void Update()
        {
            const float MAX_DISTANCE = 100.0f;

            // find best target
            if (m_targetPart == null)
            {
                float fBestDistance = MAX_DISTANCE;
                Mech closestEnemy = null;
                foreach (Mech mech in Mech.AllMechs)
                {
                    if (mech.PlayerTeam != Mech.PlayerTeam)
                    {
                        float fDistance = Vector3.Distance(mech.transform.position, Mech.transform.position);
                        if (fDistance < fBestDistance)
                        {
                            fBestDistance = fDistance;
                            closestEnemy = mech;
                        }
                    }
                }

                // get random target
                if (closestEnemy != null)
                {
                    MechPart[] parts = closestEnemy.GetComponentsInChildren<MechPart>();
                    m_targetPart = parts[Random.Range(0, parts.Length)];
                }
            }
            else if (Vector3.Distance(m_targetPart.transform.position, Mech.transform.position) > MAX_DISTANCE)
            {
                m_targetPart = null;
            }

            // update the shoulder pack's rotation look at target
            Target = Vector3.MoveTowards(Target, DesiredTarget, Time.deltaTime * 10.0f);
        }

        void LateUpdate()
        {
            // store original rotation
            Quaternion qOriginalRotation = transform.localRotation;
            transform.localRotation = Quaternion.identity;

            // calculate the local target direction
            Vector3 vToTarget = Target - transform.position;
            Vector3 vLocalToTarget = transform.InverseTransformDirection(vToTarget);

            // limit the look at
            vLocalToTarget = Vector3.RotateTowards(Vector3.forward, vLocalToTarget, m_fAngleLimit * Mathf.Deg2Rad, 0);

            // do look at in local space
            Quaternion qLocalRotation = Quaternion.LookRotation(vLocalToTarget.normalized);
            transform.localRotation = Quaternion.Slerp(qOriginalRotation, qLocalRotation, Time.deltaTime * m_fRotationSpeed);
        }

        public override void FireAt(Vector3 vTarget)
        {
            Missile missile = GetComponentInChildren<Missile>();
            if (missile != null)
            {
                missile.Target = m_targetPart;
                //missile.OffYouGo();
            }
        }

        public override void OnDeath()
        {
            // destroy pneumatic cylinders
            foreach (PneumaticCylinder pc in transform.parent.GetComponentsInChildren<PneumaticCylinder>())
            {
                Destroy(pc);
            }

            base.OnDeath();
        }
    }
}