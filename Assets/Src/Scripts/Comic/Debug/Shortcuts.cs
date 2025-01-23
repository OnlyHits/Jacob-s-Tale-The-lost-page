#if UNITY_EDITOR

using System.Collections.Generic;
using Comic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    public bool unlock = true;
    public Dictionary<KeyCode, Chapters> keyByChapters = new Dictionary<KeyCode, Chapters>()
    {
        { KeyCode.Alpha0, Chapters.The_Prequel },
        { KeyCode.Alpha1, Chapters.The_First_Chapter },
        { KeyCode.Alpha2, Chapters.The_Second_Chapter },
        { KeyCode.Alpha3, Chapters.The_Third_Chapter },
        { KeyCode.Alpha4, Chapters.The_Fourth_Chapter },
    };

    private void Update()
    {
        bool hasComputeChaptersKey = false;
        Chapters chapterComputed = Chapters.Chapter_None;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            unlock = !unlock;
        }

        foreach (KeyCode key in keyByChapters.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                hasComputeChaptersKey = true;
                chapterComputed = keyByChapters[key];
                break;
            }
        }

        if (hasComputeChaptersKey)
        {
            if (unlock)
                ComicGameCore.Instance.GetGameMode<MainGameMode>().UnlockChapter(chapterComputed, true, true);
            else
                ComicGameCore.Instance.GetGameMode<MainGameMode>().LockChapter(chapterComputed);
        }
    }
}

#endif
