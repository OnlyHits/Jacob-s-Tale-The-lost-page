using UnityEngine;
using UnityEditor;

namespace Comic
{
    [CustomEditor(typeof(MainGameMode))]
    public class MainGameModeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MainGameMode game_mode = (MainGameMode)target;

            // Create a button in the Inspector
            if (GUILayout.Button("Init game"))
            {
                game_mode.InitGame();
            }

            if (GUILayout.Button("Init hud"))
            {
                game_mode.InitHud();
            }
        }
    }
}