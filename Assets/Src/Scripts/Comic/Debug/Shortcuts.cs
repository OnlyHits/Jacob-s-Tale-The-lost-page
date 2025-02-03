using System.Collections.Generic;
using Comic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
#if UNITY_EDITOR
    public bool unlock = true;
    public Dictionary<KeyCode, Chapters> keyByChapters = new Dictionary<KeyCode, Chapters>()
    {
        //{ KeyCode.Alpha0, Chapters.The_Prequel },
        { KeyCode.Alpha1, Chapters.The_First_Chapter },
        { KeyCode.Alpha2, Chapters.The_Second_Chapter },
        { KeyCode.Alpha3, Chapters.The_Third_Chapter },
        { KeyCode.Alpha4, Chapters.The_Fourth_Chapter },
    };

    private void Update()
    {
        CheckInputSave();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ComicGameCore.Instance.MainGameMode.PlayEndGame();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Time.timeScale = 1f;
        }
    }

    private void CheckInputSave()
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
                ComicGameCore.Instance.MainGameMode.UnlockChapter(chapterComputed, true, true);
            else
                ComicGameCore.Instance.MainGameMode.LockChapter(chapterComputed);
        }
    }
#endif
}

