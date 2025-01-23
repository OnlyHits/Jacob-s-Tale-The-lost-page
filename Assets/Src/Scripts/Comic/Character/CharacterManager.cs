using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class CharacterManager : BaseBehaviour
    {
        [SerializeField, ReadOnly] private Player m_player;
        private Dictionary<VoiceType, Character> m_npcs;
        public Player GetPlayer() => m_player;

        private void Awake()
        {
            PageManager.onSwitchPage += OnSwitchPage;
        }

        public void Init()
        {
            LoadCharacters();

            m_player.Init();
            foreach (Character npc in m_npcs.Values)
            {
                npc.Init();
            }

            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockVoice(OnUnlockVoice);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToLockVoice(OnLockVoice);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockChapter(OnUnlockChapter);

            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData())
            {
                // Spawn NPCS
                TrySpawnNPCsByChapter(data.m_chapterType);

                // Disable NPC if already got in UI
                if (data.m_hasUnlockVoice)
                {
                    VoiceType voiceType = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType);
                    EnableNPC(voiceType, false);
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

        private void TrySpawnNPCsByChapter(Chapters chapter)
        {
            Dictionary<VoiceType, int> npcsSpawnPages = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetNpcsSpawnPageByChapter(chapter); ;

            if (npcsSpawnPages == null)
            {
                return;
            }

            foreach (VoiceType npcVoice in npcsSpawnPages.Keys)
            {
                if (!m_npcs.ContainsKey(npcVoice))
                {
                    return;
                }
                int idxPageToSpawn = npcsSpawnPages[npcVoice];

                Transform spawn = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPageManager().GetSpawnPointByPageIndex(idxPageToSpawn);

                if (spawn == null)
                {
                    Debug.LogWarning("No spawn pos for page " + idxPageToSpawn.ToString() + ", npc " + npcVoice.ToString() + " cannot spawn properly");
                    return;
                }

                m_npcs[npcVoice].transform.position = spawn.position;
                m_npcs[npcVoice].transform.parent = spawn;
                EnableNPC(npcVoice, true);
            }
        }

        private void OnUnlockVoice(VoiceType voiceType)
        {
            if (!m_npcs.ContainsKey(voiceType))
            {
                Debug.LogWarning(voiceType + " is not register");
                return;
            }

            EnableNPC(voiceType, false);
        }

        public void OnLockVoice(VoiceType voiceType)
        {
            if (!m_npcs.ContainsKey(voiceType))
            {
                Debug.LogWarning(voiceType + " is not register");
                return;
            }

            EnableNPC(voiceType, true);
        }

        private void OnUnlockChapter(Chapters chapter)
        {
            TrySpawnNPCsByChapter(chapter);
        }

        private void EnableNPC(VoiceType voiceType, bool enable)
        {
            if (!m_npcs.ContainsKey(voiceType))
            {
                Debug.LogWarning("Try to enable a npc of type " + voiceType.ToString() + " which is not in npc list from CharacterManager");
                return;
            }
            m_npcs[voiceType].gameObject.SetActive(enable);
        }

        private void OnSwitchPage(bool nextPage, Page p1, Page p2)
        {
            PauseAllCharacters(true);

            StartCoroutine(CoroutineUtils.InvokeOnDelay(1f, () =>
            {
                PauseAllCharacters(false);
            }));
        }

        public void PauseAllCharacters(bool pause = true)
        {
            m_player.Pause(pause);
            foreach (Npc npc in m_npcs.Values)
            {
                npc.Pause(pause);
            }
        }
    }
}
