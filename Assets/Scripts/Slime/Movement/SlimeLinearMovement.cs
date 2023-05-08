using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlimeBounce.Slime.Movement
{
    public class SlimeLinearMovement : SlimeMovementCore
    {
        override public Vector3 GetMovementDelta()
        {
            if (_isMovementAllowed)
            {
                return transform.forward * MovementSpeed;
            }
            return Vector3.zero;
        }
    }
}