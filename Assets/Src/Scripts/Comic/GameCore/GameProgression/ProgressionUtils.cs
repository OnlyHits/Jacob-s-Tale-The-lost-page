using UnityEngine;

namespace Comic
{
    public static class ProgressionUtils
    {
        public static bool HasUnlockVoice(VoiceType type)
        {
            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData())
            {
                if (ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType)
                    == type && data.m_hasUnlockVoice)
                {
                    return true;
                }
            }

            return false;
        }
   }
}