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

        bool hasComputeChaptersKey = false;
        Chapters chapterComputed = Chapters.Chapter_None;

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            hasComputeChaptersKey = true;
            chapterComputed = Chapters.The_Prequel;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hasComputeChaptersKey = true;
            chapterComputed = Chapters.The_First_Chapter;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasComputeChaptersKey = true;
            chapterComputed = Chapters.The_Second_Chapter;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasComputeChaptersKey = true;
            chapterComputed = Chapters.The_Third_Chapter;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hasComputeChaptersKey = true;
            chapterComputed = Chapters.The_Fourth_Chapter;
        }

        if (hasComputeChaptersKey)
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(chapterComputed, false, false);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(chapterComputed);
        }
    }
}

#endif
