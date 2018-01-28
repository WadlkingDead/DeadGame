using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoGameFrame
{

    [CreateAssetMenu(menuName = "CreatNewTween", fileName = "NewTween")]
    public class LoTweenObject : ScriptableObject
    {

        [SerializeField]
        public List<string> names;

        [SerializeField]
        public List<AnimationCurve> curves;

        public void UpdateCurve()
        {
            LoCurveStor.allCurves = new Dictionary<string, AnimationCurve>();
            for (int i = 0; i < names.Count; i++)
            {
                LoCurveStor.allCurves.Add(names[i], curves[i]);
            }
        }

        public void AddCurve(string curveName, AnimationCurve curve)
        {
            if (names == null)
            {
                names = new List<string>();
            }

            if (curves == null)
            {
                curves = new List<AnimationCurve>();
            }

            if (curve == null)
            {
                return;
            }

            if (names.Contains(curveName))
            {
                curves[names.IndexOf(curveName)] = curve;
            }
            else
            {
                names.Add(curveName);
                curves.Add(curve);
            }

        }


        public void RemoveCurve(string name)
        {
            if (names == null || curves == null)
            {
                return;
            }
            if (names.Contains(name))
            {
                int index = names.IndexOf(name);
                names.Remove(name);
                curves.RemoveAt(index);

            }
        }


        public AnimationCurve editorCurve = new AnimationCurve();
        public string editorName = "请输入曲线名";
    }


    [CreateAssetMenu(menuName = "CreatCurveArray", fileName = "NewCurveArray")]
    public class LoBezierCurveArrayObject : ScriptableObject
    {
        public struct BezierCurveArray {
            public string name;
            public List<BezierCurve> curves;
        }
    }

}