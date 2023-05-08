using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {

        public static Vector3 RandomVector3(Vector3 min, Vector3 max)
        {
            Vector3 result = Vector3.zero;

            result.x = UnityEngine.Random.Range(min.x, max.x);
            result.y = UnityEngine.Random.Range(min.y, max.y);
            result.z = UnityEngine.Random.Range(min.z, max.z);

            return result;
        }
    }
}