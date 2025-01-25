using System;
using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using CustomArchitecture;

namespace Comic
{
    public enum VoiceType
    {
        Voice_BestFriend, // Geatan
        Voice_Beloved, // Bethany
        Voice_Bully, // Dylan
        Voice_Boss, // Yvick
        Voice_Jacob,
        Voice_None
    }

    public enum PowerType
    {
        Power_LegUp,
        Power_Telekinesis,
        Power_Dummy,
        Power_Rotate_Room,
        Power_None,
    }

    public enum Chapters
    {
        The_Prequel,
        The_First_Chapter,
        The_Second_Chapter,
        The_Third_Chapter,
        The_Fourth_Chapter,
        Chapter_None
    }

    [System.Serializable]
    public class ChapterConfig
    {
        public VoiceType m_voiceType;
        public PowerType m_powerType;
        public List<int> m_pages;
        [OdinSerialize, ShowInInspector] public Dictionary<VoiceType, int> m_npcSpawnByPage;
    }

    [CreateAssetMenu(fileName = "GameConfig", menuName = "Comic/GameConfig")]
    [System.Serializable]
    public class GameConfig : SerializedScriptableObject
    {
        [NonSerialized] private readonly SaveUtilitary<Dictionary<Chapters, ChapterConfig>> m_saveUtilitary;

        [OdinSerialize, ShowInInspector] public Dictionary<Chapters, ChapterConfig> m_config;

        public GameConfig()
        {
            m_saveUtilitary = new SaveUtilitary<Dictionary<Chapters, ChapterConfig>>("GameConfig", FileType.ConfigFile);

            Load();
        }

        public Chapters GetChapterByPower(PowerType type)
        {
            foreach (var chapter in m_config)
            {
                if (chapter.Value.m_powerType == type)
                    return chapter.Key;
            }

            return Chapters.Chapter_None;
        }

        public Chapters GetChapterByVoice(VoiceType type)
        {
            foreach (var chapter in m_config)
            {
                if (chapter.Value.m_voiceType == type)
                    return chapter.Key;
            }

            return Chapters.Chapter_None;
        }

        public PowerType GetPowerByChapter(Chapters type)
        {
            if (!m_config.ContainsKey(type))
            {
                Debug.LogWarning("Doesn't find chapter");
                return PowerType.Power_None;
            }

            return m_config[type].m_powerType;
        }

        public VoiceType GetVoiceByChapter(Chapters type)
        {
            if (!m_config.ContainsKey(type))
            {
                Debug.LogWarning("Doesn't find chapter");
                return VoiceType.Voice_None;
            }

            return m_config[type].m_voiceType;
        }

        public List<int> GetPagesByChapter(Chapters type)
        {
            if (!m_config.ContainsKey(type))
            {
                Debug.LogWarning("Doesn't find chapter");
                return null;
            }

            return m_config[type].m_pages;
        }

        public Dictionary<VoiceType, int> GetNpcsSpawnPageByChapter(Chapters type)
        {
            if (!m_config.ContainsKey(type))
            {
                Debug.LogWarning("Doesn't find chapter");
                return null;
            }

            return m_config[type].m_npcSpawnByPage;
        }



        public ChapterConfig GetChapterDatas(Chapters type)
        {
            if (!m_config.ContainsKey(type))
            {
                Debug.LogWarning("Couldn't find chapter");
                return null;
            }

            return m_config[type];
        }

        [Button("Save")]
        private void SaveData()
        {
            Save();
            Debug.Log("Data saved successfully!");
        }

        [Button("Load")]
        private void LoadData()
        {
            Load();
            Debug.Log("Data loaded successfully!");
        }

        public Dictionary<Chapters, ChapterConfig> GetConfig()
        {
            return m_config;
        }

        public void Save()
        {
            m_saveUtilitary.Save(m_config);
        }

        public void Load()
        {
            m_config = m_saveUtilitary.Load();
        }
    }
}