using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;

namespace SlimeBounce.Environment
{
    public interface IDeploymentActor
    {
        public DeployableSlime DeploySlime(DeployableSlime prefab);
    }
}