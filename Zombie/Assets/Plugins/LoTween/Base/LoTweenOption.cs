using UnityEngine;

namespace LoGameFrame
{

    public class LoTweenOption
    {

        public static bool isMobilePlayform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    return true;
                default:
                    return false;
            }
        }

        public static string curveFilePath = "Assets/Plugins/LoTween/Resources/LoTweenCurves.asset";
        public static string curveFileName = "LoTweenCurves";

        public static AnimationCurve CopyCurve(AnimationCurve source)
        {
            return new AnimationCurve(source.keys);
        }

    }
}
