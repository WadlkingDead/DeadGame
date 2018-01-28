using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoGameFrame
{
    public class LoTweenPath : LoTweenSingleBase
    {
        [System.Serializable]
        public enum Axis {
            X = 1, Y = 2, Z = 4,
        }

        public bool lookToTarget;
        public Axis followAxis = Axis.X;
        public bool reverseLook;

        void Start()
        {
            SetCurveInfo();
        }

        void SetCurveInfo()
        {
            if (curves == null) curves = new List<BezierCurve>();

            int count = Count;
            lengths = new float[count];
            for (int i = 0; i < count; i++)
            {
                lengths[i] = curves[i].length;
            }
        }
        

        #region Curves


        [SerializeField]
        List<BezierCurve> curves;


        public BezierCurve this[int index] { get { return curves[index]; } }

        float[] lengths;

        /// <summary>
        /// 曲线数量.
        /// </summary>
        public int Count
        {
            get
            {
                if (curves == null)
                    curves = new List<BezierCurve>();
                return curves.Count;
            }
        }

        float _length = -1;

        /// <summary>
        /// 曲线总长度.
        /// </summary>
        public float Length
        {
            get
            {
                if (this._length < 0)
                {
                    _length = 0;
                    curves.ForEach((item) => {
                        _length += item.length;
                    });
                }

                return _length;
            }
        }

        public BezierCurve FirstCurve
        {
            get
            {
                if (curves == null)
                    curves = new List<BezierCurve>();

                if (curves.Count == 0)
                {
                    Debug.LogError("还没有添加曲线！！");
                    return null;
                }
                return this[0];
            }
        }

        public BezierCurve LastCurve
        {
            get
            {
                if (curves == null)
                    curves = new List<BezierCurve>();

                if (curves.Count == 0)
                {
                    Debug.LogError("还没有添加曲线！！");
                    return null;
                }
                return this[Count - 1];
            }
        }

        public void AddTail(BezierCurve curve)
        {
            if (curves == null) curves = new List<BezierCurve>();
            curves.Add(curve);
        }

        public void AddTail()
        {
            if (curves == null || curves.Count == 0)
            {
                Debug.LogError("还没有添加曲线！！");
            }

            curves.Add(BezierCurve.GetTail(LastCurve));
        }

        public void InsertCurve(int index)
        {
            curves.Insert(index, BezierCurve.InsertCurve(curves[index]));
        }

        public void RemoveCurve(int index)
        {
            if (index >= Count)
            {
                if (index == 1)
                {
                    DestroyImmediate(this);
                    return;
                }
                BezierCurve.RemoveCurve(curves[index - 1], false);
                curves.RemoveAt(index - 1);
            }
            else
            {
                BezierCurve.RemoveCurve(curves[index], true);
                curves.RemoveAt(index);
            }

        }

        #endregion

        #region ForMove

        Vector3 beginPos = undefinedVector3;
        Vector3 beginScale;

        public void ResetBeginPos()
        {
            beginPos = undefinedVector3;
        }

        public override void PlayFoward(float _waitTime)
        {
            base.PlayFoward(_waitTime);
            if (beginPos == undefinedVector3)
            {
                beginPos = transform.localPosition;
                beginScale = transform.localScale;
            }

#if UNITY_EDITOR
            if (editorPlayStart != null) editorPlayStart.Invoke();
#endif
        }

        public override void PlayReverse(float _waitTime)
        {
            base.PlayReverse(_waitTime);
            if (beginPos == undefinedVector3)
            {
                beginPos = transform.localPosition;
                beginScale = transform.localScale;
            }
#if UNITY_EDITOR
            if (editorPlayStart != null) editorPlayStart.Invoke();
#endif
        }

#if UNITY_EDITOR
        public Action editorPlayStart;
#endif

        // 目标点.
        Vector3 targetPos;
        // 目标向量.
        Vector3 targetVector;


        // 投影向量.
        Vector2 proVector;

        protected override void UpdateValue(float time)
        {
            bool dontFollow = time == 1;
            time = Mathf.Clamp01(time) * Length;
            int currentIndex = GetTimeIndex(ref time);
            targetPos = BezierCurve.GetBezierPos(curves[currentIndex].start, curves[currentIndex].startTangent, curves[currentIndex].endTangent, curves[currentIndex].end, time);// + beginPos;
            targetPos.x *= beginScale.x;
            targetPos.y *= beginScale.y;
            targetPos.z *= beginScale.z;
            targetPos += beginPos;
            if (lookToTarget && !dontFollow)
            {
                targetVector = reverseLook ? (transform.localPosition - targetPos).normalized : (targetPos - transform.localPosition).normalized;

                switch (followAxis)
                {
                    case Axis.X:
                        transform.right = targetVector;
                        break;
                    case Axis.Y:
                        transform.up = targetVector;
                        break;
                    case Axis.Z:
                        transform.forward = targetVector;
                        break;
                }
            }
            transform.localPosition = targetPos;
        }


        int GetTimeIndex(ref float time)
        {
            float total = 0;
            for (int i = 0; i < lengths.Length; i++)
            {
                total += lengths[i];
                if (time <= total)
                {
                    time = (time - total + lengths[i]) / lengths[i];
                    return i;
                }
            }

            return 0;
        }

        #endregion


    }

}


