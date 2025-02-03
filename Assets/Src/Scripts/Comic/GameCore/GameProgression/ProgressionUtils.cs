using UnityEngine;

namespace Comic
{
    public static class ProgressionUtils
    {
        public static bool HasUnlockVoice(VoiceType type)
        {
            foreach (var data in ComicGameCore.Instance.MainGameMode.GetUnlockChaptersData())
            {
                if (ComicGameCore.Instance.MainGameMode.GetGameConfig().GetVoiceByChapter(data.m_chapterType)
                    == type && data.m_hasUnlockVoice)
                {
                    return true;
                }
            }

            return false;
        }

        public static PowerType GetPowerByVoice(VoiceType type)
        {
            foreach (var data in ComicGameCore.Instance.MainGameMode.GetUnlockChaptersData())
            {
                if (ComicGameCore.Instance.MainGameMode.GetGameConfig().GetVoiceByChapter(data.m_chapterType)
                    == type && data.m_hasUnlockPower)
                {
                    return ComicGameCore.Instance.MainGameMode.GetGameConfig().GetPowerByChapter(data.m_chapterType);
                }
            }

            return PowerType.Power_None;
        }


   }
}