using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MathsUtils {

    public static Transform GetNearest(Vector2 fromPosition, List<Transform> list, float minDistance = 0, float maxDistance = 100, List<Transform> exemptions = null) {
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform current in list) {
            if (current == null) { continue; }

            float dist = Vector2.Distance(fromPosition, current.position);

            if (exemptions != null) {
                if (exemptions.Contains(current)) {
                    continue;
                }
            }

            if (minDistance < dist && dist < maxDistance) {
                if (dist < minDist) {
                    minDist = dist;
                    nearest = current;
                }
            }
        }
        return nearest;
    }

    public static List<Transform> GetRandomInList(List<Transform> originalList, int x) {
        List<Transform> randomTransforms = new List<Transform>();
        List<Transform> tempList = new List<Transform>(originalList);

        for (int i = 0; i < x; i++) {
            if (tempList.Count == 0)
                break;

            int randomIndex = Random.Range(0, tempList.Count);

            randomTransforms.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return randomTransforms;
    }

    public static List<GameObject> GetRandomInList(List<GameObject> originalList, int x) {
        List<GameObject> randomGameObjects = new List<GameObject>();
        List<GameObject> tempList = new List<GameObject>(originalList);

        for (int i = 0; i < x; i++) {
            if (tempList.Count == 0)
                break;

            int randomIndex = Random.Range(0, tempList.Count);

            randomGameObjects.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return randomGameObjects;
    }

    public static List<T> RetrieveListInList<T>(List<T> desired, List<T> toRetrive) {
        if (toRetrive == null) {
            return desired;
        }

        foreach (T item in desired.ToList()) {
            if (toRetrive.Contains(item)) {
                desired.Remove(item);
            }
        }

        return desired;
    }

    public static bool AlmostEqual(Vector3 pos1, Vector3 pos2, Vector3 step) {
        bool isInRangeX = pos1.x - step.x < pos2.x && pos2.x < pos1.x + step.x;
        bool isInRangeY = pos1.y - step.y < pos2.y && pos2.y < pos1.y + step.y;
        bool isInRangeZ = pos1.z - step.z < pos2.z && pos2.z < pos1.z + step.z;

        return isInRangeX && isInRangeY && isInRangeZ;
    }

    public static bool AlmostEqual(float pos1, float pos2, float step) {
        return pos1 - step < pos2 && pos2 < pos1 + step;
    }

    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) {
        float x = Mathf.Clamp(value.x, min.x, max.x);
        float y = Mathf.Clamp(value.y, min.y, max.y);
        float z = Mathf.Clamp(value.z, min.z, max.z);

        return new Vector3(x, y, z);
    }

    public static float BiggestAbs(float x, float y) {
        return Mathf.Abs(Mathf.Abs(x) > Mathf.Abs(y) ? x : y);
    }

    public static float SmallestAbs(float x, float y) {
        return Mathf.Abs(Mathf.Abs(x) < Mathf.Abs(y) ? x : y);
    }

    public static Vector3 GetMedianPos(List<Transform> transforms) {
        Vector3 medianPos = Vector3.zero;
        int count = transforms.Count;

        if (transforms == null || transforms.Count == 0) {
            return medianPos;
        }

        foreach (Transform trf in transforms) {
            if (trf == null || trf?.transform == null) {
                count = (int)Mathf.Clamp(count - 1, 1, Mathf.Infinity);
                continue;
            }
            medianPos += trf.transform.position;
        }

        medianPos /= transforms.Count;

        return medianPos;
    }

    public static Vector3 Abs(Vector3 v) {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static float ClampNegativeOnly(float value)
    {
        if (value < 0)
            return 0f;
        return value;
    }
}