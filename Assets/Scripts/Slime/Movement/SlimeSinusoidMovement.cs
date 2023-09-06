using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlimeBounce.Slime.Movement
{
    public class SlimeSinusoidMovement : SlimeMovementCore
    {
        private const float DELTA_SIN_STEP = 0.008f;
        private float _sinState;

        override public Vector3 GetMovementDelta()
        {
            if (_isMovementAllowed)
            {
                _sinState += DELTA_SIN_STEP;
                return (transform.forward + 1.8f * transform.right * Mathf.Sin(_sinState * Mathf.PI)).normalized * MovementSpeed;
            }
            return Vector3.zero;
        }
    }
}