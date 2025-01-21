using UnityEngine;
using CustomArchitecture;
using System.Collections.Generic;

namespace Comic
{
    public class DialogueView : AView
    {
        [SerializeField] protected Transform                            m_iconContainer;
        private Dictionary<VoiceType, GameObject>                       m_icons;
        private List<GameObject>                                        m_currentIcons;

        public override void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockVoice(UnlockVoice);

            m_currentIcons = new();
            m_icons = new()
            {
                { VoiceType.Voice_Gaetan,       Resources.Load<GameObject>("GUI/Icon_Gaetan") },
                { VoiceType.Voice_Bethany,      Resources.Load<GameObject>("GUI/Icon_Bethany") },
                { VoiceType.Voice_Dylan,        Resources.Load<GameObject>("GUI/Icon_Dylan") },
                { VoiceType.Voice_Ivyc,         Resources.Load<GameObject>("GUI/Icon_Ivyc") },
            };

            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetSavedValues())
            {
                if (data.m_hasUnlockVoice)
                {
                    UnlockVoice(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType));
                }
            }
        }

        public void UnlockVoice(VoiceType type)
        {
            if (!m_icons.ContainsKey(type))
            {
                Debug.LogWarning(type + " is not register");
                return;
            }

            m_currentIcons.Add(Instantiate(m_icons[type], m_iconContainer));
        }
    }
}
