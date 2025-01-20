using UnityEngine;

public static class LayerUtils {

    public static void SetLayer(GameObject obj, LayerMask newLayer, bool applyToChilds = true) {
        int layerValue = GetLayerFromLayerMask(newLayer);
        SetLayer(obj, layerValue, applyToChilds);
    }

    public static void SetLayer(GameObject obj, int newLayer, bool applyToChilds = true) {
        if (obj == null) {
            return;
        }

        obj.layer = newLayer;

        if (applyToChilds) {
            foreach (Transform child in obj.transform) {
                SetLayer(child.gameObject, newLayer, true);
            }
        }
    }

    //public static bool IsLayerInLayerMask(int layer, LayerMask layerMask) {
    //    return (layerMask.value & (1 << layer)) != 0;
    //}

    public static bool IsLayerInLayerMask(int layerMaskValue, int layer)
    {
        return (layerMaskValue & (1 << layer)) != 0;
    }

    public static int GetLayerFromLayerMask(LayerMask layerMask) {
        int mask = layerMask.value;
        int layer = 0;

        while (mask > 1) {
            mask >>= 1;
            layer++;
        }

        return layer;
    }
}