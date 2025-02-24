using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class MechPart : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 20.0f)]
        public float        m_fHealth = 5.0f;

        #region Properties

        public Mech Mech => GetComponentInParent<Mech>();

        #endregion

        public virtual void TakeDamage(float fDamage)
        {
            float fOldHealth = m_fHealth;
            m_fHealth -= fDamage;
            if (fOldHealth >= 0.0f && m_fHealth < 0.0f)
            {
                OnDeath();
            }
        }

        public virtual void OnDeath()
        {
            GetComponentInParent<Mech>()?.OnPartDestroyed(this);
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>();
            Explosion.CreateAt(transform.position);
            Destroy(this);
        }
    }
}