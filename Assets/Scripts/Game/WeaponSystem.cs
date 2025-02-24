using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class WeaponSystem : MechPart
    {
        #region Properties

        public abstract bool IsReady { get; }

        #endregion

        public abstract void FireAt(Vector3 vTarget);
    }
}