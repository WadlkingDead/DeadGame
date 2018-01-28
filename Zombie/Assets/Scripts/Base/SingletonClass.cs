using UnityEngine;
using System.Collections;

public class SingletonClass<T> where T : new() {

    private static T instance = default(T);
    public static T Instance {
        get {
            if (instance == null) instance = new T();
            return instance;
        }
    }



    #region Coroutines

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return LoGameFrame.LoCurveStor.Instance.mStartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName)
    {
        return LoGameFrame.LoCurveStor.Instance.mStartCoroutine(methodName);
    }

    public Coroutine StartCoroutine(string methodName, object value)
    {
        return LoGameFrame.LoCurveStor.Instance.mStartCoroutine(methodName, value);
    }

    public void StopCoroutine(Coroutine coroutine)
    {
        LoGameFrame.LoCurveStor.Instance.mStopCoroutine(coroutine);
    }

    public void StopCoroutine(string methodName)
    {
        LoGameFrame.LoCurveStor.Instance.mStopCoroutine(methodName);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        LoGameFrame.LoCurveStor.Instance.mStopCoroutine(routine);
    }

    public void StopAllCoroutines()
    {
        LoGameFrame.LoCurveStor.Instance.mStopAllCoroutines();
    }

    public Coroutine StartCoroutine_Auto(IEnumerator routine)
    {
        return LoGameFrame.LoCurveStor.Instance.mStartCoroutine_Auto(routine);
    }

    public void Invoke(string methodName, float time)
    {
        LoGameFrame.LoCurveStor.Instance.mInvoke(methodName, time);
    }

    public void CancleInvoke(string methodName)
    {
        LoGameFrame.LoCurveStor.Instance.mCancleInvoke(methodName);
    }

    #endregion


}
