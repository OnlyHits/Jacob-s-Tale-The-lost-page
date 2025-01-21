using System;
using System.Collections.Generic;

namespace Comic
{
    [System.Serializable]
    public class ChapterSavedData
    {
        public Chapters         m_chapterType = Chapters.The_First_Chapter;
        public VoiceType        m_voiceType = VoiceType.Voice_Gaetan;
        public bool             m_hasUnlockVoice = false;
        public bool             m_hasEncounterVoice = false;
        public List<int>        m_chapterPages = null;
    }

    public class GameProgression
    {
        public List<ChapterSavedData>                           m_unlockChapters = null;
        public int                                              m_currentPage = 0;
        private readonly SaveUtilitary<List<ChapterSavedData>>  m_saveUtilitary;

        public GameProgression()
        {
            m_saveUtilitary = new SaveUtilitary<List<ChapterSavedData>>("ChapterSavedData", FileType.SaveFile);

            m_unlockChapters = m_saveUtilitary.Load();
        }
    }
}