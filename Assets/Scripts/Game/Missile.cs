using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Missile : MonoBehaviour
    {
        #region Properties

        public MechPart Target { get; set; }

        #endregion

        void Update()
        {
            /*
            if (m_bAlive)
            {
                float fStep = m_fSpeed * Time.deltaTime;
                Vector3 vNextPosition = transform.position + transform.forward * fStep;
                RaycastHit hit;
                if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, fStep))
                {
                    // hit something?
                    m_bAlive = false;
                    StartCoroutine(OnDeath());

                    // deal damage?
                    MechPart part = hit.collider.GetComponentInParent<MechPart>();
                    if (part != null)
                    {
                        part.TakeDamage(1.0f);
                    }
                }
                else
                {
                    transform.position = vNextPosition;
                }
            }*/
        }
    }
}