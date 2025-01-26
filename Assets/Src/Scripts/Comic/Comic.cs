
using UnityEngine;

namespace Comic
{
    public static class Comic
    {
        public static readonly string playerLayerName = "Player";

        public static readonly string frontLayerName = "SwitchPage";
        public static readonly string backLayerName = "NotSwitchPage";
        public static readonly string defaultLayerName = "Default";

        public static int frontLayerId => SortingLayer.NameToID(frontLayerName);
        public static int backLayerId => SortingLayer.NameToID(backLayerName);
        public static int defaultLayerId => SortingLayer.NameToID(defaultLayerName);
    }
}
