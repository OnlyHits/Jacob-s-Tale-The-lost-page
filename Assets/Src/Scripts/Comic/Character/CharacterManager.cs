using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.Utilities;
using UnityEditor.SearchService;
using UnityEngine;

namespace Comic
{
    public class CharacterManager : BaseBehaviour
    {
        [SerializeField, ReadOnly] private Player m_player;
        [SerializeField, ReadOnly] private Dictionary<VoiceType, Character> m_npcs;
        public Player GetPlayer() => m_player;

        public void Init()
        {
            LoadCharacters();

            m_player.Init();
            foreach (Character npc in m_npcs.Values)
            {
                npc.Init();
            }

            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockVoice(OnUnlockVoice);

            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetSavedValues())
            {
                if (data.m_hasUnlockVoice)
                {
                    VoiceType voiceType = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType);
                    DisableNPC(voiceType);
                }
            }
        }

        private void LoadCharacters()
        {
            Player playerPrefab = Resources.Load<Player>("Player/Player");
            Character bestFriend = Resources.Load<Character>("NPC/BestFriend");
            Character beloved = Resources.Load<Character>("NPC/Beloved");
            Character bully = Resources.Load<Character>("NPC/Bully");
            Character boss = Resources.Load<Character>("NPC/Boss");

            m_player = Instantiate(playerPrefab);
            m_npcs = new()
            {
                { VoiceType.Voice_BestFriend,    Instantiate(bestFriend)},
                { VoiceType.Voice_Beloved,       Instantiate(beloved)},
                { VoiceType.Voice_Bully,         Instantiate(bully)},
                { VoiceType.Voice_Boss,          Instantiate(boss)},
            };
        }

        private void OnUnlockVoice(VoiceType voiceType)
        {
            if (!m_npcs.ContainsKey(voiceType))
            {
                Debug.LogWarning(voiceType + " is not register");
                return;
            }

            DisableNPC(voiceType);
        }

        private void DisableNPC(VoiceType voiceType)
        {
            m_npcs[voiceType].gameObject.SetActive(false);
        }
    }
}
