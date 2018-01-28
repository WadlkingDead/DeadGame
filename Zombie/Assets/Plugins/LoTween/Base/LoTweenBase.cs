using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoGameFrame
{
    public class LoTweenBase : MonoBehaviour
    {

        public enum PlayType
        {
            Clamp,
            Circle,
            PingPong,
        }

        [HideInInspector]
        public string curveName;

        public bool isPlaying { get { return playIndex != 0; } }

        /// <summary>
        /// 动画类型.  Clamp- 只播一次, Circle- 循环播, PingPong- 播放完后反向播.
        /// </summary>
        [HideInInspector]
        public PlayType playType = PlayType.Clamp;

        /// <summary>
        /// 重复播放次数, 负数时永不停止.
        /// </summary>
        [HideInInspector]
        public int repeatTimes = -1;

        /// <summary>
        /// 当前剩余重复次数.
        /// </summary>
        protected int currentRemainTimes;


        /// <summary>
        /// 播放控制器.   1- 正向播, 2- 反向播, 3- 停止
        /// </summary>
        protected byte playIndex = 0;

        public bool pause = false;

        /// <summary>
        /// 播正向动画.
        /// </summary>
        /// <param name="_waitTime">等待时长.</param>
        public virtual void PlayFoward(float _waitTime)
        {
            if (playIndex == 0)
            {
                timer = 0 - _waitTime;
            }

            currentRemainTimes = repeatTimes;

            pause = false;
            playIndex = 1;
        }

        public void PlayFoward()
        {
            PlayFoward(0);
        }

        public void Play()
        {
            PlayFoward();
        }

        public void Play(float waitTime)
        {
            PlayFoward(waitTime);
        }


        /// <summary>
        /// 播反向动画.
        /// </summary>
        /// <param name="_waitTime">等待时长.</param>
        public virtual void PlayReverse(float _waitTime)
        {
            if (playIndex == 0)
            {
                timer = 1 + _waitTime;
            }
            else
            {
                timer = timer + _waitTime;
            }

            currentRemainTimes = repeatTimes;

            pause = false;
            playIndex = 2;
        }

        public void PlayReverse()
        {
            PlayReverse(0);
        }

        /// <summary>
        /// 停止播放.
        /// </summary>
        public void StopPlay()
        {
            playIndex = 0;
            timer = 0;

            ResetEvent();
        }

        public void Stop()
        {
            StopPlay();
        }


        [SerializeField, HideInInspector]
        float _duration = 0.3f;

        /// <summary>
        /// 动画持续时间.
        /// </summary>
        public float duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = Mathf.Max(value, 0.001f);
            }
        }


        /// <summary>
        /// 启动组件时播放.
        /// </summary>
        [HideInInspector]
        public bool playOnEnable = false;


        /// <summary>
        /// 是否被Time.timeScale影响.
        /// </summary>
        [HideInInspector]
        public bool useTimeScale = true;


        /// <summary>
        /// 添加事件.
        /// </summary>
        /// <param name="time">响应时间.</param>
        /// <param name="func">回调方法.</param>
        /// <param name="tag">标记.</param>
        public void AddEvent(float time, LoTweenEventWithTagDelegate func, params object[] tag)
        {
            if (events == null)
            {
                events = new List<LoTweenEventBase>();
            }

            RemoveEvent(time);
            events.Add(new LoTweenEventWithTag(time, func, tag));
        }

        /// <summary>
        /// 添加事件.
        /// </summary>
        /// <param name="time">响应时间.</param>
        /// <param name="func">回调方法.</param>
        public void AddEvent(float time, LoTweenEventDelegate func)
        {
            if (events == null)
            {
                events = new List<LoTweenEventBase>();
            }

            RemoveEvent(time);
            events.Add(new LoTweenEventBase(time, func));
        }

        public void RemoveEvent(float time)
        {
            if (events == null)
            {
                return;
            }

            if (time < 0)
            {
                events.Clear();
                return;
            }

            events.Remove(events.Find((LoTweenEventBase ev) =>
            {
                return ev.time == time;
            }));
        }

        public void RemoveEvent(int index)
        {
            if (events == null)
            {
                return;
            }

            if (index < 0)
            {
                events.Clear();
                return;
            }

            if (index > events.Count)
            {
                return;
            }
            events.RemoveAt(index);
        }

        public void ClearEvent()
        {
            if (events != null)
            {
                events.Clear();
            }
        }


        protected List<LoTweenEventBase> events;


        /// <summary>
        /// 计时器.
        /// </summary>
        protected float timer;

        protected bool isTrig = false;

        protected List<LoTweenEventBase> needToCallEvents = new List<LoTweenEventBase>();

        /// <summary>
        /// 检测事件
        /// </summary>
        /// <param name="time"></param>
        protected bool CheckEvent(float time)
        {
            needToCallEvents.Clear();
            if (events != null && Time.deltaTime > 0)
            {
                events.ForEach((LoTweenEventBase tempEvent) =>
                {
                    isTrig = false;
                    if (!tempEvent.isInvoke)
                    {
                        if (playIndex == 1 && time >= tempEvent.time && tempEvent.time > 0)
                        {
                            isTrig = true;
                        }
                        else if (playIndex == 2 && time <= tempEvent.time && tempEvent.time < 1)
                        {
                            isTrig = true;
                        }

                        if (isTrig)
                        {
                            tempEvent.isInvoke = true;
                            needToCallEvents.Add(tempEvent);
                        }
                    }
                });
            }
            return needToCallEvents.Count > 0;
        }

        protected void InvokeSavedEvents()
        {
            foreach (LoTweenEventBase tempEvent in needToCallEvents)
            {
                if (tempEvent is LoTweenEventWithTag)
                {
                    LoTweenEventWithTag e = (LoTweenEventWithTag)tempEvent;
                    e.trigger.Invoke(e.tag);
                }
                else
                {
                    tempEvent.trigger.Invoke();
                }
            }
        }


        /// <summary>
        /// 重置Event.
        /// </summary>
        void ResetEvent()
        {
            if (events != null)
            {
                events.ForEach((LoTweenEventBase tempEvent) =>
                {
                    tempEvent.isInvoke = false;
                });
            }
        }

        public System.Action<LoTweenBase> onEnableCallBack;
        public System.Action<LoTweenBase> onDisableCallBack;

        /// <summary>
        /// 组件可用时自动播放动画.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (playOnEnable)
            {
                PlayFoward();
            }
            if (onEnableCallBack != null) onEnableCallBack.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            if (onDisableCallBack != null) onDisableCallBack.Invoke(this);
        }



        /// <summary>
        /// 通过名称获得曲线.
        /// </summary>
        /// <param name="name">曲线名称.</param>
        /// <returns></returns>
        public AnimationCurve GetCurve(string name)
        {
            curveName = name;
            return LoCurveStor.GetCurve(name);
        }

        /// <summary>
        /// 通过名称获得曲线.
        /// </summary>
        /// <param name="name">曲线名称.</param>
        /// <param name="offset">偏移量(不包含起点和重点).</param>
        /// <returns></returns>
        public AnimationCurve GetCurve(string name, float offset)
        {
            curveName = name;
            return LoCurveStor.GetCurve(name, offset);
        }

        /// <summary>
        /// 通过名称获得曲线.
        /// </summary>
        /// <param name="name">曲线名称.</param>
        /// <param name="offset">偏移量.</param>
        /// <param name="inCludeStart">是否包含起点.</param>
        /// <param name="inCludeEnd">是否包含终点.</param>
        /// <returns></returns>
        public AnimationCurve GetCurve(string name, float offset, bool inCludeStart, bool inCludeEnd)
        {
            curveName = name;
            return LoCurveStor.GetCurve(name, offset, inCludeStart, inCludeEnd);
        }

        /// <summary>
        /// 获取反向的曲线.
        /// </summary>
        /// <param name="name">曲线名称.</param>
        /// <returns></returns>
        public AnimationCurve GetReverseCurve(string name)
        {
            return LoCurveStor.GetReverseCurve(name);
        }

        /// <summary>
        /// 获取反向的曲线.
        /// </summary>
        /// <param name="curve">已有的曲线.</param>
        /// <returns></returns>
        public AnimationCurve GetReverseCurve(AnimationCurve curve)
        {
            return LoCurveStor.GetReverseCurve(curve);
        }

        public virtual void Clone(LoTweenBase target)
        {
            this.useTimeScale = target.useTimeScale;
            this.playType = target.playType;
            this.events = target.events;
            this.duration = target.duration;
            this.playOnEnable = target.playOnEnable;
        }

        public virtual void ResetToBegin() { }

        public virtual void ResetToEnd() { }


        public static Vector3 undefinedVector3 = new Vector3(12345.6f, 23456.7f, 34567.8f);
    }
}