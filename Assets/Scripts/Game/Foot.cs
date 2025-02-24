using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    [ExecuteInEditMode]
    public class Foot : MonoBehaviour
    {
        private void LateUpdate()
        {
            float fHeightAboveGround = transform.position.y - 0.4f;
            float fToeRotation = Mathf.InverseLerp(0.3f, 1.2f, fHeightAboveGround);


            // don't try this at home.
            foreach (Transform child in GetComponentInChildren<Transform>())
            {
                if (!child.name.Contains("Toe"))
                {
                    continue;
                }

                // rotate toe
                Vector3 vLocalRotation = child.localEulerAngles;
                vLocalRotation.x = fToeRotation * 0.0f;             // TODO based of Leg GroundPose
                child.localEulerAngles = vLocalRotation;
            }
        }

    }
}