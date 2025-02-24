using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Minigun : WeaponSystem
    {
        [SerializeField]
        public GameObject   m_bulletPrefab;

        private float       m_fSpinVelocity = 0.0f;
        private bool        m_bIsFiring;
        private float       m_fLastSpawnTime = 0.0f;
        private Transform   m_muzzlePoint;

        const float         MAX_SPIN = 360.0f * 2.0f;
        const float         FIRE_RATE = 0.2f;

        #region Properties

        public override bool IsReady => true;

        #endregion

        private void OnEnable()
        {
            m_muzzlePoint = transform.parent.Find("MuzzlePoint");
        }

        private void Update()
        {

            // update spin velocity
            m_fSpinVelocity = Mathf.MoveTowards(m_fSpinVelocity, m_bIsFiring ? MAX_SPIN : 0.0f, Time.deltaTime * MAX_SPIN * 0.5f);
            m_bIsFiring = false;

            // update spin
            transform.localEulerAngles += new Vector3(0.0f, 0.0f, m_fSpinVelocity * Time.deltaTime);
        }

        public override void FireAt(Vector3 vTarget)
        {
            const float SPREAD = 2.0f;

            m_bIsFiring = true;

            // spawn a new bullet
            if (m_fSpinVelocity > MAX_SPIN * 0.9f &&
                Time.time - m_fLastSpawnTime > FIRE_RATE &&
                m_bulletPrefab != null)
            {
                m_fLastSpawnTime = Time.time;
                Quaternion qBulletRotation = Quaternion.LookRotation(vTarget - m_muzzlePoint.position);
                qBulletRotation *= Quaternion.Euler(Random.Range(-SPREAD, SPREAD), Random.Range(-SPREAD, SPREAD), Random.Range(-SPREAD, SPREAD));
                Instantiate(m_bulletPrefab, m_muzzlePoint.position, qBulletRotation);
            }
        }
    }
}