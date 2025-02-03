using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace CustomArchitecture
{
    public static class SceneUtils
    {
        public static T FindObjectAcrossScenes<T>() where T : BaseBehaviour
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                foreach (GameObject rootObject in scene.GetRootGameObjects())
                {
                    T component = rootObject.GetComponentInChildren<T>(true);
                    if (component != null)
                        return component;
                }
            }
            return null;
        }
    }
}