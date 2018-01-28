using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace LoGameFrame
{
    public class LoCurveStor : MonoBehaviour
    {

        public static Dictionary<string, AnimationCurve> allCurves;


        [RuntimeInitializeOnLoadMethod]
        static void OnInit()
        {
            GameObject go = new GameObject("LoTweenContainer");
            go.AddComponent<LoCurveStor>();
            go.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
        }

        public static LoCurveStor Instance = null;
        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            if (allCurves == null)
                LoadCurves();
        }

        public static void LoadCurves()
        {
            LoTweenObject obj = Resources.Load<LoTweenObject>(LoTweenOption.curveFileName);

            allCurves = new Dictionary<string, AnimationCurve>();
            int cureveCount = obj.names.Count;
            for (int i = 0; i < cureveCount; i++)
            {
                allCurves.Add(obj.names[i], obj.curves[i]);
            }
        }


        public static AnimationCurve GetDefaultCurve()
        {
            Keyframe[] keys = new Keyframe[2];
            keys[0] = new Keyframe(0, 0, 1, 1);
            keys[1] = new Keyframe(1, 1, 1, 1);

            return new AnimationCurve(keys);
        }

        public static AnimationCurve GetCurve(string curveName)
        {
            AnimationCurve c;
            if (allCurves == null)
                LoadCurves();
            if (!allCurves.TryGetValue(curveName, out c))
            {
                Debug.LogWarning("CurveName: '" + curveName + "' Can't Find, Load Default Curve(line)");
                c = GetDefaultCurve();
            }
            return c;
        }

        public static AnimationCurve GetCurve(string curveName, float offset)
        {
            return GetTranslationCurve(GetCurve(curveName), offset, false, false);
        }

        public static AnimationCurve GetCurve(string curveName, float offset, bool inCludeStart, bool inCludeEnd)
        {
            return GetTranslationCurve(GetCurve(curveName), offset, inCludeStart, inCludeEnd);
        }

        static AnimationCurve GetTranslationCurve(AnimationCurve curve, float offset, bool inCludeStart, bool inCludeEnd)
        {

            Keyframe[] keys = curve.keys;

            int start = 0;
            if (!inCludeStart)
            {
                start = 1;
            }

            int end = keys.Length;
            if (!inCludeEnd)
            {
                end = keys.Length - 1;
            }

            for (int i = start; i < end; i++)
            {
                keys[i].value += offset;
            }


            return new AnimationCurve(keys); ;
        }

        public static AnimationCurve GetReverseCurve(AnimationCurve targetCurve)
        {
            int count = targetCurve.keys.Length;
            Keyframe[] keys = new Keyframe[count];
            for (int i = 0; i < keys.Length; i++)
            {
                Keyframe tempKey = targetCurve.keys[count - 1 - i];
                keys[i].value = tempKey.value;
                keys[i].time = 1 - tempKey.time;
                keys[i].inTangent = -tempKey.outTangent;
                keys[i].outTangent = -tempKey.inTangent;

            }
            return new AnimationCurve(keys);
        }

        public static AnimationCurve GetReverseCurve(string curveName)
        {
            return GetReverseCurve(GetCurve(curveName));
        }




        #region Coroutines

        public Coroutine mStartCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public Coroutine mStartCoroutine(string methodName)
        {
            return StartCoroutine(methodName);
        }

        public Coroutine mStartCoroutine(string methodName, object value)
        {
            return StartCoroutine(methodName, value);
        }

        public void mStopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }

        public void mStopCoroutine(string methodName)
        {
            StopCoroutine(methodName);
        }

        public void mStopCoroutine(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        public void mStopAllCoroutines()
        {
            StopAllCoroutines();
        }

        public Coroutine mStartCoroutine_Auto(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void mInvoke(string methodName, float time)
        {
            Invoke(methodName, time);
        }

        public void mCancleInvoke(string methodName)
        {
            CancelInvoke(methodName);
        }

        #endregion



    }

}
