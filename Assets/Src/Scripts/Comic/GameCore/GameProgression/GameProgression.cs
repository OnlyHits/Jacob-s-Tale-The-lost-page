using System;
using System.Collections.Generic;
using UnityEngine;
using CustomArchitecture;

namespace Comic
{
    [System.Serializable]
    public class ChapterSavedData
    {
        public Chapters         m_chapterType = Chapters.Chapter_None;
        public bool             m_hasUnlockPower = false;
        public bool             m_hasUnlockVoice = false;
        public bool             m_hasEncounterVoice = false;
    }

    public class GameProgression
    {
        private List<ChapterSavedData>                              m_unlockChapters = null;
        private readonly SaveUtilitary<List<ChapterSavedData>>      m_saveUtilitary;

        public GameProgression()
        {
            m_saveUtilitary = new SaveUtilitary<List<ChapterSavedData>>("ChapterSavedData", FileType.SaveFile);
            m_unlockChapters = m_saveUtilitary.Load();
            if (m_unlockChapters == null)
                m_unlockChapters = new List<ChapterSavedData>();
        }

        public List<ChapterSavedData> GetUnlockedChaptersDatas() => m_unlockChapters;

        public void ClearSaveDebug()
        {
            m_unlockChapters.Clear();
            m_saveUtilitary.Save(m_unlockChapters);
        }

        public void EncounterVoice(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type && chapter.m_hasEncounterVoice)
                {
                    Debug.LogWarning("You already unlock this voice");
                    return;
                }
                else if (chapter.m_chapterType == type && !chapter.m_hasEncounterVoice)
                {
                    chapter.m_hasEncounterVoice = true;
                    m_saveUtilitary.Save(m_unlockChapters);
                    return;
                }
            }

            Debug.Log("You didn't unlock this chapter");
        }

        public void UnlockVoice(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type && chapter.m_hasUnlockVoice)
                {
                    Debug.LogWarning("You already unlock this voice");
                    return;
                }
                else if (chapter.m_chapterType == type && !chapter.m_hasUnlockVoice)
                {
                    chapter.m_hasUnlockVoice = true;
                    m_saveUtilitary.Save(m_unlockChapters);
                    return;
                }
            }

            Debug.Log("You didn't unlock this chapter");
        }

        public void UnlockPower(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type && chapter.m_hasUnlockPower)
                {
                    Debug.LogWarning("You already unlock this voice");
                    return;
                }
                else if (chapter.m_chapterType == type && !chapter.m_hasUnlockPower)
                {
                    chapter.m_hasUnlockPower = true;
                    m_saveUtilitary.Save(m_unlockChapters);
                    return;
                }
            }
        }

        public void UnlockChapter(Chapters type)
        {
            foreach (var unlock_chapter in m_unlockChapters)
            {
                if (unlock_chapter.m_chapterType == type)
                {
                    Debug.LogWarning("Chapter is already unlocked");
                    return;
                }
            }

            ChapterSavedData chapter = new ChapterSavedData();
            m_unlockChapters.Add(chapter);
            
            chapter.m_chapterType = type;

            m_saveUtilitary.Save(m_unlockChapters);
        }

        public void LockChapter(Chapters type)
        {
            foreach (var unlock_chapter in m_unlockChapters)
            {
                if (unlock_chapter.m_chapterType == type)
                {
                    m_unlockChapters.Remove(unlock_chapter);
                    break;
                }
            }

            m_saveUtilitary.Save(m_unlockChapters);
        }

        public bool HasUnlockChapter(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type)
                    return true;
            }
            return false;
        }

        public bool HasUnlockVoice(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type && chapter.m_hasUnlockVoice)
                    return true;
            }
            return false;
        }

        public bool HasUnlockPower(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type && chapter.m_hasUnlockPower)
                    return true;
            }
            return false;
        }

        public bool HasEncounterVoice(Chapters type)
        {
            foreach (var chapter in m_unlockChapters)
            {
                if (chapter.m_chapterType == type && chapter.m_hasEncounterVoice)
                    return true;
            }
            return false;
        }

        public void Save()
        {
            m_saveUtilitary.Save(m_unlockChapters);
        }
    }
}