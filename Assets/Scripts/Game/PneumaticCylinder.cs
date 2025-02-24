using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class PneumaticCylinder : MonoBehaviour
    {
        [SerializeField]
        public Transform    m_target;

        private Transform   m_arm;
        private Transform   m_attachment;

        void OnEnable()
        {
            m_arm = transform.Find("PneumaticArm");
            m_attachment = m_arm.Find("PneumaticAttachment");
        }

        void LateUpdate()
        {
            if (m_target == null)
            {
                return;
            }

            // look at target
            Vector3 vToTarget = m_target.position - transform.position;
            Vector3 vUp = Vector3.Cross(vToTarget, Vector3.forward);
            transform.rotation = Quaternion.LookRotation(vToTarget.normalized, vUp) * Quaternion.Euler(90.0f, 0.0f, 0.0f);

            // extend arm
            float fDistance = vToTarget.magnitude;
            m_arm.localPosition = new Vector3(0.0f, fDistance - 0.5f, 0.0f);

            // place attachment
            m_attachment.position = m_target.position;
            m_attachment.rotation = m_target.rotation;
        }
    }
}