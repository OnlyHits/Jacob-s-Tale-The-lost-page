using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System;

namespace CustomArchitecture
{
    public class SceneLoader : BaseBehaviour
    {
        public float m_waitAfterLoad = 1f;
        public string m_transitionScene = "LoadingScene";
        public Action m_onScenesLoaded;

        private List<string> m_currentScenes = new()
        {
            "StartingScene",
        };

        public void SubscribeToEndLoading(Action function)
        {
            m_onScenesLoaded -= function;
            m_onScenesLoaded += function;
        }

        public void LoadGameModeScenes(string ui_scene, string game_scene)
        {
            StartCoroutine(LoadScenesCoroutine(ui_scene, game_scene));
        }

        private IEnumerator LoadScenesCoroutine(string ui_scene, string game_scene)
        {
            // load transition scene
            if (!SceneManager.GetSceneByName(m_transitionScene).isLoaded)
            {
                yield return SceneManager.LoadSceneAsync(m_transitionScene, LoadSceneMode.Additive);
            }

            // Unload current scenes
            foreach (string scene in m_currentScenes)
            {
                if (scene != m_transitionScene && SceneManager.GetSceneByName(scene).isLoaded)
                {
                    yield return SceneManager.UnloadSceneAsync(scene);
                }
            }

            m_currentScenes.Clear();
            m_currentScenes.Add(m_transitionScene);

            AsyncOperation ui_load = null;
            AsyncOperation game_load = null;

            if (ui_scene != null)
            {
                ui_load = SceneManager.LoadSceneAsync(ui_scene, LoadSceneMode.Additive);
                m_currentScenes.Add(ui_scene);
            }

            while (ui_load != null && !ui_load.isDone)
            {
                yield return null;
            }

            if (game_scene != null)
            {
                game_load = SceneManager.LoadSceneAsync(game_scene, LoadSceneMode.Additive);
                m_currentScenes.Add(game_scene);
            }

            while (game_load != null && !game_load.isDone)
            {
                yield return null;
            }

            yield return new WaitForSeconds(m_waitAfterLoad);

            yield return SceneManager.UnloadSceneAsync(m_transitionScene);
            m_currentScenes.Remove(m_transitionScene);

            m_onScenesLoaded?.Invoke();
        }
    }
}