using System;
using System.Collections;
using UnityEngine;

public static class CoroutineUtils
{
    public static IEnumerator InvokeRepeating(float delay, float repeatRate, Action lambda, bool unscaleTIme = false)
    {
        if (unscaleTIme == true)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        while (true)
        {
            lambda();

            if (unscaleTIme == true)
            {
                yield return new WaitForSecondsRealtime(repeatRate);
            }
            else
            {
                yield return new WaitForSeconds(repeatRate);
            }
        }
    }

    public static IEnumerator InvokeOnDelay(float delay, Action callback, bool unscaleTIme = false)
    {
        if (unscaleTIme == true)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        callback?.Invoke();
    }


    public static IEnumerator InvokeNextFrame(Action function)
    {
        yield return new WaitForEndOfFrame();

        function?.Invoke();
    }

    public static IEnumerator InvokeOnCondition(Func<bool> condition, Action function)
    {
        while (condition?.Invoke() == false)
        {
            yield return null;
        }

        function?.Invoke();
    }

}
