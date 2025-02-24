using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerMech : Mech
    {
        private float           m_fVelocity = 0.0f;
        private Minigun         m_minigun;
        private ShoulderPack[]  m_shoulderPacks;
        private Vector2         m_vLastMouse;
        private Vector3         m_vLocalLook;
        private Transform       m_mainBody;
        private Text            m_missileLock;

        static PlayerMech       sm_instance = null;

        #region Properties

        public static PlayerMech Instance => sm_instance;

        public override bool PlayerTeam => true;

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            sm_instance = this;
            m_minigun = GetComponentInChildren<Minigun>();
            m_mainBody = transform.Find("Pelvis/MainBody");
            m_shoulderPacks = GetComponentsInChildren<ShoulderPack>();
            m_missileLock = GetComponentInChildren<Text>();     // UGLY
        }

        protected override void Update()
        {
            // move forward
            Pose moveTarget = MoveTarget;
            m_fVelocity = Mathf.MoveTowards(m_fVelocity, Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ? 8.0f : 0.0f, Time.deltaTime * 6.0f);
            moveTarget.position = transform.position + m_fVelocity * transform.forward;

            // turn
            float fTurn = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) ? -1.0f :
                          Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
            moveTarget.rotation *= Quaternion.Euler(0.0f, fTurn * Time.deltaTime * 60.0f, 0.0f);

            // raycast against terrain
            if (m_terrain != null)
            {
                // snap move target to terrain
                RaycastHit hit;
                if (m_terrain.Raycast(new Ray(moveTarget.position + Vector3.up * 100.0f, Vector3.down), out hit, 200.0f))
                {
                    moveTarget.position = hit.point;
                }
            }
            MoveTarget = moveTarget;

            // fire minigun?
            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && m_minigun != null)
            {
                m_minigun.FireAt(m_minigun.transform.position + m_minigun.transform.forward * 100.0f);
            }

            // mouse look
            Vector2 vMouseDelta = (Vector2)Input.mousePosition - m_vLastMouse;
            m_vLastMouse = Input.mousePosition;
            m_vLocalLook.y += (vMouseDelta.x / Screen.width) * 100.0f;
            m_vLocalLook.x += (vMouseDelta.y / Screen.height) * 100.0f;
            m_vLocalLook.x = Mathf.Clamp(m_vLocalLook.x, -30.0f, 30.0f);
            m_vLocalLook.y = Mathf.Clamp(m_vLocalLook.y, -20.0f, 20.0f);
            m_mainBody.localEulerAngles = m_vLocalLook;

            // missile lock
            string strMissileLock = "Missile Lock";
            bool bIsReady = false;
            foreach (ShoulderPack sp in m_shoulderPacks)
            {
                strMissileLock += "\n" + sp.name + ": " + sp.DistanceToLock.ToString("0.0"); 
                bIsReady = bIsReady || sp.IsReady;
            }
            m_missileLock.text = strMissileLock;
            m_missileLock.color = bIsReady ? Color.green : Color.red;

            // fire missle?
            if ((Input.GetKey(KeyCode.Tab) || Input.GetMouseButton(1)) && bIsReady)
            {
                // find a ready shoulder pack
                // call fire missle
            }

            base.Update();
        }

        public override void OnPartDestroyed(MechPart part)
        {
            base.OnPartDestroyed(part);

            if (part == m_minigun)
            {
                m_minigun = null;
            }
        }
    }
}