using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Extensions
    {
        public static Vector3 ToVector3XZ(this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }

        public static Vector2 XZToVector2(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
    }
}