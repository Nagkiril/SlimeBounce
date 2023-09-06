using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    public interface IFloorBoundsProvider
    {
        public Vector3 Up { get; }

        public Vector3 GetFloorPosition(Vector3 position);

        public bool ValidateOnWidth(float worldX);
    }
}