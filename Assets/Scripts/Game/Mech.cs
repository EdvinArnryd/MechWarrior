using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class Mech : MonoBehaviour
    {
        [SerializeField]
        public TerrainCollider      m_terrain;

        [SerializeField, Range(0.0f, 3.0f)]
        public float                m_fPelvisShiftAmount = 1.5f;

        private Transform           m_pelvis;
        private Leg[]               m_legs;

        public static List<Mech>    AllMechs = new List<Mech>();

        #region Properties

        public Pose MoveTarget { get; set; }

        public abstract bool PlayerTeam { get; }

        #endregion

        protected virtual void OnEnable()
        {
            MoveTarget = new Pose(transform.position, transform.rotation);
            m_pelvis = transform.Find("Pelvis");
            m_legs = GetComponentsInChildren<Leg>();
            AllMechs.Add(this);
        }

        protected virtual void OnDisable()
        {
            AllMechs.Remove(this);    
        }

        protected virtual void Update()
        {
            const float SPEED = 4.0f;

            // move mech towards target
            transform.position = Vector3.MoveTowards(transform.position, MoveTarget.position, Time.deltaTime * SPEED);
            transform.rotation = Quaternion.Slerp(transform.rotation, MoveTarget.rotation, Time.deltaTime * 1.0f);

            // center pelvis above feet
            Vector3 vFeetCenter = Vector3.zero;
            float fShiftWeight = 0.0f;
            float fPelvisPitch = 0.0f;            

            foreach (Leg leg in m_legs)
            {
                vFeetCenter += leg.Foot.position;
                fPelvisPitch = Mathf.Max(fPelvisPitch, leg.Lift);

                // calculate pelvis shift
                if (leg.Lift > Mathf.Abs(fShiftWeight))
                {
                    fShiftWeight = leg.Lift * (leg.IsRight ? 1 : -1);
                }
            }
            vFeetCenter /= m_legs.Length;

            // raycast against terrain for pelvis height
            float fPelvisHeight = 4.0f;
            if (m_terrain != null)
            {
                // snap move target to terrain
                RaycastHit hit;
                if (m_terrain.Raycast(new Ray(vFeetCenter + Vector3.up * 100.0f, Vector3.down), out hit, 200.0f))
                {
                    fPelvisHeight = hit.point.y + 3.6f;
                }
            }

            m_pelvis.position = new Vector3(vFeetCenter.x, fPelvisHeight, vFeetCenter.z) + 
                                transform.right * fShiftWeight * m_fPelvisShiftAmount;

            // smooth pelvis rotation
            Vector3 vPelvisRotation = m_pelvis.localEulerAngles;
            float fPelvisTargetRotation = fPelvisPitch * 10.0f;
            vPelvisRotation.x += (fPelvisTargetRotation - vPelvisRotation.x) * Time.deltaTime * 4.0f;
            m_pelvis.localEulerAngles = vPelvisRotation;
        }

        public virtual void OnPartDestroyed(MechPart part)
        {
        }
    }
}