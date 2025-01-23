#if UNITY_EDITOR

using Comic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Prequel, false, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_First_Chapter, true, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Second_Chapter, true, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Third_Chapter, true, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Fourth_Chapter, true, true);
        }
    }
}

#endif
