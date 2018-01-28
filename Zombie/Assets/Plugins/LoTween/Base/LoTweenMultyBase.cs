using UnityEngine;
using System.Collections;

namespace LoGameFrame
{

    public class LoTweenMultyBase : LoTweenBase
    {

        [SerializeField, HideInInspector]
        protected AnimationCurve[] curves;


        protected virtual void UpdateValue(float[] delta, float timer) { }


        protected float[] deltaValues;


        public override void Clone(LoTweenBase target)
        {
            base.Clone(target);
            this.curves = ((LoTweenMultyBase)target).curves;
        }


        void Update()
        {
            if (pause) return;
            if (playIndex == 1)
            {
                if (!useTimeScale)
                {
                    if (timer < 0)
                    {
                        timer += Time.unscaledDeltaTime;
                    }
                    else
                    {
                        timer += Time.unscaledDeltaTime / duration;
                    }

                }
                else
                {
                    if (timer < 0)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        timer += Time.deltaTime / duration;
                    }

                }

                isTrig = false;

                if (timer >= 0)
                {
                    for (int i = 0; i < curves.Length; i++)
                    {
                        deltaValues[i] = curves[i].Evaluate(timer);
                    }
                    UpdateValue(deltaValues, timer);

                    isTrig = CheckEvent(timer);
                }

                if (timer >= 1)
                {
                    StopPlay();
                }
                if (isTrig)
                    InvokeSavedEvents();
            }
        }

    }

}
