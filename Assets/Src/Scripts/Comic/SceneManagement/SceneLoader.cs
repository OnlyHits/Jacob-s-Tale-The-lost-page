using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System;

namespace LittleKnightOdyssey
{
    public class SceneLoader : MonoBehaviour
    {
        public string m_transitionScene = "LoadingScene";
        private List<string> m_currentScenes = new()
        {
            "BaseScene"
        };

        public Action m_onScenesLoaded;


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

            // Unload transition scene
            yield return SceneManager.UnloadSceneAsync(m_transitionScene);
            m_currentScenes.Remove(m_transitionScene);

            // All game & ui scene are loaded
            m_onScenesLoaded?.Invoke();
        }


        #region ACTIVE_SCENE
        public void SetGameScenePrincipal()
        {
            if (m_currentScenes.Count < 2)
            {
                Debug.LogError("Trying set principal scene but none scene has been Loaded with this SceneLoader");
                return;
            }

            SetScenePrincipalByIdex(1);
        }

        public void SetUiScenePrincipal()
        {
            if (m_currentScenes.Count < 1)
            {
                Debug.LogError("Trying set principal scene but none scene has been Loaded with this SceneLoader");
                return;
            }

            SetScenePrincipalByIdex(0);
        }

        private void SetScenePrincipalByIdex(int index)
        {
            Scene scene = SceneManager.GetSceneByName(m_currentScenes[index]);

            if (!scene.IsValid())
            {
                Debug.LogError("Trying set principal scene but the scene is not valid");
                return;
            }

            if (!scene.isLoaded)
            {
                Debug.LogError("Trying set principal scene but the scene is not loaded yet");
                return;
            }

            SceneManager.SetActiveScene(scene);
        }

        #endregion ACTIVE_SCENE

        #region GET_SCENE_OBJ
        public T GetGameSceneObj<T>()
        {
            if (m_currentScenes.Count < 2)
            {
                Debug.LogError("Trying to access to scene root but none scene has been Loaded with this SceneLoader");
                return default;
            }

            return GetSceneObjByIndex<T>(1);
        }
        public T GetUiSceneObj<T>()
        {
            if (m_currentScenes.Count < 1)
            {
                return default;
            }

            return GetSceneObjByIndex<T>(0);
        }

        private T GetSceneObjByIndex<T>(int index)
        {
            Scene scene = SceneManager.GetSceneByName(m_currentScenes[index]);

            if (!scene.IsValid())
            {
                Debug.LogError("Trying to access to scene root but the scene is not valid");
                return default;
            }

            if (!scene.isLoaded)
            {
                Debug.LogError("Trying to access to scene root but the scene is not loaded yet");
                return default;
            }

            foreach (var child in scene.GetRootGameObjects())
            {
                if (child.GetComponent<T>() is T obj)
                {
                    return obj;
                }
            }

            return default;
        }
        #endregion GET_SCENE_OBJ

    }
}