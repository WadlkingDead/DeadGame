using UnityEngine;
using System.Collections;

namespace LoGameFrame
{
    public delegate void LoTweenEventWithTagDelegate(params object[] obj);
    public delegate void LoTweenEventDelegate();

    public class LoTweenEventWithTag: LoTweenEventBase
    {
        public new LoTweenEventWithTagDelegate trigger { get; set; }
        public object[] tag { get; set; }

        public LoTweenEventWithTag(float _time, LoTweenEventWithTagDelegate _trigger, params object[] _tag)
        {
            time = _time;
            trigger = _trigger;
            tag = _tag;

            isInvoke = false;

        }

    }

    public class LoTweenEventBase
    {
        public float time { get; set; }
        public LoTweenEventDelegate trigger { get; set; }

        public bool isInvoke { get; set; }

        public LoTweenEventBase() { }

        public LoTweenEventBase(float _time, LoTweenEventDelegate _trigger)
        {
            time = _time;
            trigger = _trigger;
            isInvoke = false;
        }
    }
}


