using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoGameFrame
{

    public class LoTweenSingleBase : LoTweenBase
    {

        [SerializeField, HideInInspector]
        AnimationCurve curve;

        /// <summary>
        /// 当前播放动画曲线.
        /// </summary>
        public AnimationCurve Curve
        {
            get
            {
                if (curve == null)
                {
                    curve = LoCurveStor.GetDefaultCurve();
                }
                return curve;
            }
            set
            {
                curve = value;
            }
        }


        /// <summary>
        /// 设置曲线.
        /// </summary>
        /// <param name="name">曲线名称.</param>
        public void SetCurve(string name)
        {
            Curve = GetCurve(name);
        }

        /// <summary>
        /// 固定变化值.
        /// </summary>
        public bool fixedValue = false;


        public System.Action onFinished;
        void CheckFinishedFunc()
        {
            if (gameObject && playType == PlayType.Clamp)
            {
                if (playIndex == 1)
                {
                    ResetToEnd();
                }
                else
                {
                    ResetToBegin();
                }
            }
            if (onFinished != null) onFinished.Invoke();
        }


        protected virtual void UpdateValue(float percent) { }


        void Update()
        {
            if (pause) return;
            if (playIndex == 1)
            {
                if (timer < 0)
                {
                    if (!useTimeScale) timer += Time.unscaledDeltaTime;
                    else timer += Time.deltaTime;
                }
                else
                {
                    if (!useTimeScale) timer += Time.unscaledDeltaTime / duration;
                    else timer += Time.deltaTime / duration;
                }
                isTrig = false;

                if (timer >= 0)
                {
                    UpdateValue(Curve.Evaluate(timer));

                    isTrig = CheckEvent(timer);
                }

                if (timer >= 1)
                {
                    switch (playType)
                    {
                        case PlayType.Clamp:
                            UpdateValue(Curve.Evaluate(1));
                            CheckFinishedFunc();
                            StopPlay();
                            break;
                        case PlayType.Circle:
                            StopPlay();
                            currentRemainTimes--;
                            if (currentRemainTimes != 0) playIndex = 1;
                            else CheckFinishedFunc();
                            break;
                        case PlayType.PingPong:
                            StopPlay();
                            currentRemainTimes--;
                            if (currentRemainTimes != 0)
                            {
                                timer = 1;
                                playIndex = 2;
                            }
                            else
                            {
                                CheckFinishedFunc();
                            }
                            break;
                    }
                }

                if (isTrig) InvokeSavedEvents();
            }
            else if (playIndex == 2)
            {

                if (timer > 1)
                {
                    if (useTimeScale) timer -= Time.unscaledDeltaTime;
                    else timer -= Time.deltaTime;
                }
                else
                {
                    if (useTimeScale) timer -= Time.unscaledDeltaTime / duration;
                    else timer -= Time.deltaTime / duration;
                }


                isTrig = false;

                if (timer <= 1)
                {
                    UpdateValue(Curve.Evaluate(timer));

                    isTrig = CheckEvent(timer);
                }

                if (timer <= 0)
                {
                    switch (playType)
                    {
                        case PlayType.Clamp:
                            CheckFinishedFunc();
                            StopPlay();
                            break;
                        case PlayType.Circle:
                            StopPlay();
                            currentRemainTimes--;
                            if (currentRemainTimes != 0)
                            {
                                timer = 1;
                                playIndex = 2;
                            }
                            else
                                CheckFinishedFunc();
                            break;
                        case PlayType.PingPong:
                            StopPlay();
                            currentRemainTimes--;
                            if (currentRemainTimes != 0) playIndex = 1;
                            else CheckFinishedFunc();
                            break;
                    }
                }

                if (isTrig) InvokeSavedEvents();
            }
        }


        public override void Clone(LoTweenBase target)
        {
            base.Clone(target);
            this.Curve = ((LoTweenSingleBase)target).Curve;
        }


    }

}
