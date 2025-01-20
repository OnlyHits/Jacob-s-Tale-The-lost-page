using System;
using System.Collections.Generic;

public sealed class UtilsEase {

    public enum EaseType {
        None,
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint
    }

    private static Dictionary<EaseType, Func<float, float>> easeByTypes = new Dictionary<EaseType, Func<float, float>>()
    {
        { EaseType.Linear, Linear},
        { EaseType.InQuad, InQuad},
        { EaseType.OutQuad, OutQuad},
        { EaseType.InOutQuad, InOutQuad},
        { EaseType.InCubic, InCubic},
        { EaseType.OutCubic, OutCubic},
        { EaseType.InOutCubic, InOutCubic},
        { EaseType.InQuart, InQuart},
        { EaseType.OutQuart, OutQuart},
        { EaseType.InOutQuart, InOutQuart},
        { EaseType.InQuint, InQuint},
        { EaseType.OutQuint, OutQuint},
        { EaseType.InOutQuint, InOutQuint},
    };

    public static Func<float, float> GetEase(EaseType easeType) {
        foreach (var e in easeByTypes) {
            if (e.Key == easeType) {
                return e.Value;
            }
        }
        //default value
        return Linear;
    }

    public static float Linear(float t) {
        return t;
    }

    public static float InQuad(float t) {
        return t * t;
    }

    public static float OutQuad(float t) {
        return t * (2 - t);
    }

    public static float InOutQuad(float t) {
        return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }

    public static float InCubic(float t) {
        return t * t * t;
    }

    public static float OutCubic(float t) {
        return (--t) * t * t + 1;
    }

    public static float InOutCubic(float t) {
        return t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
    }

    public static float InQuart(float t) {
        return t * t * t * t;
    }

    public static float OutQuart(float t) {
        return 1 - (--t) * t * t * t;
    }

    public static float InOutQuart(float t) {
        return t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
    }

    public static float InQuint(float t) {
        return t * t * t * t * t;
    }

    public static float OutQuint(float t) {
        return 1 + (--t) * t * t * t * t;
    }

    public static float InOutQuint(float t) {
        return t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
    }

}
