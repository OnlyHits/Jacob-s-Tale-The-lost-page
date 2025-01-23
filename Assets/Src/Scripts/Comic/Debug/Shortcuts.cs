#if UNITY_EDITOR

using Comic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    public bool unlock = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            unlock = !unlock;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Prequel, false, false);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(Chapters.The_Prequel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_First_Chapter, true, true);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(Chapters.The_First_Chapter);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Second_Chapter, true, true);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(Chapters.The_Second_Chapter);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Third_Chapter, true, true);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(Chapters.The_Third_Chapter);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(Chapters.The_Fourth_Chapter, true, true);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(Chapters.The_Fourth_Chapter);
        }
    }
}

#endif
