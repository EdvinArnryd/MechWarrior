using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MainBody : MechPart
    {
        #region Properties

        #endregion

        public override void OnDeath()
        {
            Mech mech = GetComponentInParent<Mech>();

            foreach (MeshRenderer mr in mech.GetComponentsInChildren<MeshRenderer>())
            {
                Collider collider = mr.GetComponent<Collider>();
                if(collider != null) 
                {
                    collider.transform.parent = null;
                    Rigidbody rb = collider.gameObject.AddComponent<Rigidbody>();
                    rb.AddForce(Vector3.Normalize(collider.transform.position - transform.position) * 30.0f, ForceMode.Impulse);
                    Explosion.CreateAt(collider.transform.position);
                }

                Leg leg = mr.GetComponent<Leg>();
                if (leg != null)
                {
                    Destroy(leg);
                }
            }

            GetComponentInParent<Mech>()?.OnPartDestroyed(this);
            Destroy(mech.gameObject);
            Destroy(this);
        }
    }
}