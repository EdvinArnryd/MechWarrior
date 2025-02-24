using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class EnemyMech : Mech
    {
        private Material        m_enemyMetal = null;
        private List<Vector3>   m_path = null;

        #region Properties

        public override bool PlayerTeam => false;

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();

            // change enemy mech metal
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                Material[] materials = mr.sharedMaterials;
                Material metal = System.Array.Find(materials, m => m.name == "Metal");
                if (metal != null)
                {
                    if (m_enemyMetal == null)
                    {
                        m_enemyMetal = new Material(metal);
                        m_enemyMetal.color = new Color(1.0f, 0.5f, 0.5f);
                    }

                    mr.sharedMaterials = System.Array.ConvertAll(materials, m => m == metal ? m_enemyMetal : m);
                }
            }

            StartCoroutine(UpdatePath());
        }

        protected override void Update()
        {
            // AI stuffs
            if (m_path != null && PlayerMech.Instance != null)
            {
                // limit nav points
                m_path.RemoveAll(v => Vector3.Distance(v, transform.position) < 2.1f);

                // move along path
                if (m_path.Count > 0)
                {
                    Vector3 vNext = m_path[0];
                    Vector3 vToNext = vNext - transform.position;
                    vToNext.y = 0.0f;

                    bool bTooCloseToPlayer = Vector3.Distance(transform.position, PlayerMech.Instance.transform.position) < 12.0f;

                    if (bTooCloseToPlayer)
                    {
                        MoveTarget = new Pose(transform.position,
                                              Quaternion.LookRotation(PlayerMech.Instance.transform.position - transform.position));
                    }
                    else
                    {
                        MoveTarget = new Pose(Vector3.MoveTowards(transform.position, vNext, 2.0f),
                                              Quaternion.LookRotation(vToNext));
                    }
                }
            }

            base.Update();
        }

        IEnumerator UpdatePath()
        {
            while (true)
            {
                // get path to player
                NavMeshPath path = new NavMeshPath();
                if (PlayerMech.Instance != null && 
                    NavMesh.CalculatePath(transform.position, PlayerMech.Instance.transform.position, NavMesh.AllAreas, path))
                {
                    m_path = new List<Vector3>(path.corners);
                }

                yield return new WaitForSeconds(2.0f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // debug draw path
            if (m_path != null && m_path.Count > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + Vector3.up, m_path[0]);
                for (int i = 1; i < m_path.Count; ++i)
                {
                    Gizmos.DrawLine(m_path[i - 1] + Vector3.up, m_path[i] + Vector3.up);
                }
            }
        }
    }
}