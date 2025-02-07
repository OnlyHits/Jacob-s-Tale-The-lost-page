using UnityEngine;
using UnityEditor;

namespace Comic
{
    [CustomEditor(typeof(Page))]
    public class PageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Page page = (Page)target;

            if (GUILayout.Button("Spawn panel"))
            {
                page.InstantiatePanel();
            }

            if (GUILayout.Button("Refresh list"))
            {
                page.RefreshList();
            }
        }
    }
}