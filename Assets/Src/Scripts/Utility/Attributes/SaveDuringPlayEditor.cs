#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

[InitializeOnLoad]
public static class SaveDuringPlayEditor {

    static SaveDuringPlayEditor() {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static Dictionary<int, Dictionary<FieldInfo, object>> savedValues = new Dictionary<int, Dictionary<FieldInfo, object>>();

    private static void OnPlayModeStateChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingPlayMode) {
            SaveAllValues();
        } else if (state == PlayModeStateChange.EnteredEditMode) {
            LoadAllValues();
        } else if (state == PlayModeStateChange.EnteredPlayMode) {
            LoadAllValues();
        }
    }

    private static void SaveAllValues() {
        savedValues.Clear();
        foreach (var obj in Object.FindObjectsByType(typeof(MonoBehaviour), FindObjectsSortMode.None)) {
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var objValues = new Dictionary<FieldInfo, object>();

            foreach (var field in fields) {
                if (field.IsDefined(typeof(SaveDuringPlayAttribute), true)) {
                    objValues[field] = field.GetValue(obj);
                }
            }

            if (objValues.Count > 0) {
                savedValues[obj.GetInstanceID()] = objValues;
            }
        }
    }

    private static void LoadAllValues() {
        foreach (var obj in Object.FindObjectsByType(typeof(MonoBehaviour), FindObjectsSortMode.None)) {
            var instanceID = obj.GetInstanceID();
            if (savedValues.ContainsKey(instanceID)) {
                var values = savedValues[instanceID];
                foreach (var fieldEntry in values) {
                    var field = fieldEntry.Key;
                    var value = fieldEntry.Value;
                    field.SetValue(obj, value);
                }
            }
        }
    }
}
#endif
