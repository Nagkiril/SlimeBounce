using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.ExtraModules
{
    //Note that the interface is in ExtraModules namespace - not in "ExtraModules.Suspension"; this way we avoid knowledge about what are classes-implementations while working with the interface
    public interface ISuspensionModule
    {
        public bool IsSuspended { get; }

        public bool Suspend(Transform suspension);

        public bool Unsuspend(Transform suspension);
    }
}