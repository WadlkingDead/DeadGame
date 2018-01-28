using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoGameFrame
{

    public class LoTweenControl : MonoBehaviour
    {
        public static float Lerp(float a, float b, float time)
        {
            return a + time * (b - a);
        }

        public static float ReverseLerp(float a, float b, float target)
        {
            return (target - a) / (b - a);
        }

        static Vector2 tempVector2;
        public static Vector2 Lerp(Vector2 a, Vector2 b, float time)
        {
            tempVector2.x = Lerp(a.x, b.x, time);
            tempVector2.y = Lerp(a.y, b.y, time);
            return tempVector2;
        }

        static Vector3 tempVector3;
        public static Vector3 Lerp(Vector3 a, Vector3 b, float time)
        {
            tempVector3.x = Lerp(a.x, b.x, time);
            tempVector3.y = Lerp(a.y, b.y, time);
            tempVector3.z = Lerp(a.z, b.z, time);
            return tempVector3;
        }

        static Vector4 tempVector4;
        public static Vector4 Lerp(Vector4 a, Vector4 b, float time)
        {
            tempVector4.x = Lerp(a.x, b.x, time);
            tempVector4.y = Lerp(a.y, b.y, time);
            tempVector4.z = Lerp(a.z, b.z, time);
            tempVector4.w = Lerp(a.w, b.w, time);
            return tempVector4;
        }

        static Quaternion tempQuaternion;
        public static Quaternion Lerp(Quaternion a, Quaternion b, float time)
        {
            return Quaternion.Lerp(a, b, time);
        }


        public static Color Lerp(Color a, Color b, float time)
        {
            return Color.Lerp(a, b, time);
        }



    }

}
