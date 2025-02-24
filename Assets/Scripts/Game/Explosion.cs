using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Explosion : MonoBehaviour
    {
        static GameObject sm_prefab = null;

        public static void CreateAt(Vector3 vWorldPosition)
        {
            if (sm_prefab == null)
            {
                sm_prefab = Resources.Load<GameObject>("Prefabs/Explosion");
            }

            GameObject go = Instantiate(sm_prefab, vWorldPosition, Quaternion.identity);
        }
    }
}