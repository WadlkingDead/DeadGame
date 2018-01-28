using UnityEngine;
using System.Collections;

namespace LoGameFrame
{
    public class AnimationCurveArray { }

    public sealed class AnimationCurveArray2 : AnimationCurveArray
    {

        public AnimationCurve x = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve y = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);


        public AnimationCurveArray2() { }

        public AnimationCurveArray2(AnimationCurve x, AnimationCurve y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator AnimationCurveArray2(AnimationCurveArray3 cur)
        {
            return new AnimationCurveArray2(cur.x, cur.y);
        }

        public static implicit operator AnimationCurveArray3(AnimationCurveArray2 cur)
        {
            return new AnimationCurveArray3(cur.x, cur.y, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
        }

    }


    public sealed class AnimationCurveArray3 : AnimationCurveArray
    {
        public AnimationCurve x = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve y = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve z = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public AnimationCurveArray3() { }

        public AnimationCurveArray3(AnimationCurve x, AnimationCurve y, AnimationCurve z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public AnimationCurveArray3(AnimationCurve x, AnimationCurve y)
        {
            this.x = x;
            this.y = y;
        }

    }


    public class AnimationCurveArray4 : AnimationCurveArray
    {
        public AnimationCurve x = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve y = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve z = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve w = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public AnimationCurveArray4() { }

        public AnimationCurveArray4(AnimationCurve x, AnimationCurve y, AnimationCurve z, AnimationCurve w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

}
