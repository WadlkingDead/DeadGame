using UnityEngine;
using System.Collections;

namespace LoGameFrame
{
    public class LoTweenNumber
    {
        public float[] changeElement;
        public int elementCount;

        public LoTweenNumber() { }

        public LoTweenNumber(object type)
        {
            if (type is float)
            {
                changeElement = new float[1];
                changeElement[0] = (float)type;
                elementCount = 1;
            }
            else if (type is Vector2)
            {
                changeElement = new float[2];
                Vector2 temp = (Vector2)type;
                changeElement[0] = temp.x;
                changeElement[1] = temp.y;
                elementCount = 2;
            }
            else if (type is Vector3)
            {
                changeElement = new float[3];
                Vector3 temp = (Vector3)type;
                changeElement[0] = temp.x;
                changeElement[1] = temp.y;
                changeElement[2] = temp.z;
                elementCount = 3;
            }
            else if (type is Vector4)
            {
                changeElement = new float[4];
                Vector4 temp = (Vector4)type;
                changeElement[0] = temp.x;
                changeElement[1] = temp.y;
                changeElement[2] = temp.z;
                changeElement[3] = temp.w;
                elementCount = 4;
            }
            else if (type is Color)
            {
                changeElement = new float[4];
                Color temp = (Color)type;
                changeElement[0] = temp.r;
                changeElement[1] = temp.g;
                changeElement[2] = temp.b;
                changeElement[3] = temp.a;
                elementCount = 4;
            }
            else if (type is Quaternion)
            {
                changeElement = new float[4];
                Quaternion temp = (Quaternion)type;
                changeElement[0] = temp.x;
                changeElement[1] = temp.y;
                changeElement[2] = temp.z;
                changeElement[3] = temp.w;
                elementCount = 4;
            }
        }

        public static LoTweenNumber Change(LoTweenNumber element, object type)
        {
            if (type is float)
            {
                element.changeElement[0] = (float)type;
            }
            else if (type is Vector2)
            {
                Vector2 temp = (Vector2)type;
                element.changeElement[0] = temp.x;
                element.changeElement[1] = temp.y;
            }
            else if (type is Vector3)
            {
                Vector3 temp = (Vector3)type;
                element.changeElement[0] = temp.x;
                element.changeElement[1] = temp.y;
                element.changeElement[2] = temp.z;
            }
            else if (type is Vector4)
            {
                Vector4 temp = (Vector4)type;
                element.changeElement[0] = temp.x;
                element.changeElement[1] = temp.y;
                element.changeElement[2] = temp.z;
                element.changeElement[3] = temp.w;
            }
            else if (type is Color)
            {
                Color temp = (Color)type;
                element.changeElement[0] = temp.r;
                element.changeElement[1] = temp.g;
                element.changeElement[2] = temp.b;
                element.changeElement[3] = temp.a;
            }
            else if (type is Quaternion)
            {
                Quaternion temp = (Quaternion)type;
                element.changeElement[0] = temp.x;
                element.changeElement[1] = temp.y;
                element.changeElement[2] = temp.z;
                element.changeElement[3] = temp.w;
            }
            return element;
        }

        public static LoTweenNumber operator +(LoTweenNumber a, LoTweenNumber b)
        {
            if (a == null || b == null)
            {
                return null;
            }
            int count = Mathf.Min(a.elementCount, b.elementCount);
            LoTweenNumber result = new LoTweenNumber();
            result.elementCount = count;
            result.changeElement = new float[count];
            for (int i = 0; i < count; i++)
            {
                result.changeElement[i] = a.changeElement[i] + b.changeElement[i];
            }
            return result;
        }

        public static LoTweenNumber operator -(LoTweenNumber a, LoTweenNumber b)
        {
            if (a == null || b == null)
            {
                return null;
            }
            int count = Mathf.Min(a.elementCount, b.elementCount);
            LoTweenNumber result = new LoTweenNumber();
            result.elementCount = count;
            result.changeElement = new float[count];
            for (int i = 0; i < count; i++)
            {
                result.changeElement[i] = a.changeElement[i] - b.changeElement[i];
            }
            return result;
        }

    }
}





