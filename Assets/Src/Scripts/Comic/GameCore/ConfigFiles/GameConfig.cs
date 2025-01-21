using System;
using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Comic
{
    public enum VoiceType
    {
        Voice_Gaetan, // best friend
        Voice_Bethany, // voice belove
        Voice_Dylan, // voice bully
        Voice_Ivyc, // voice boss
    }

    public enum PowerType
    {
        Power_LegUp,
        Power_Telekinesis,
        Power_Dummy,
        Power_Rotate_Room,
    }

    public enum Chapters
    {
        The_Prequel,
        The_First_Chapter,
        The_Second_Chapter,
        The_Third_Chapter,
    }

    [System.Serializable]
    public class ChapterConfig
    {
        public VoiceType    m_pnj;
        public PowerType    m_powerType;
        public List<int>    m_pages; 
    }

    [CreateAssetMenu(fileName = "GameConfig", menuName = "Comic/GameConfig")]
    [System.Serializable]
    public class GameConfig : SerializedScriptableObject
    {
        [NonSerialized] private readonly SaveUtilitary<Dictionary<Chapters, ChapterConfig>>   m_saveUtilitary;

        [OdinSerialize, ShowInInspector] public Dictionary<Chapters, ChapterConfig>  m_config;

        public GameConfig()
        {
            m_saveUtilitary = new SaveUtilitary<Dictionary<Chapters, ChapterConfig>>("GameConfig", FileType.ConfigFile);

            Load();
        }

        [Button("Save")]
        private void SaveData()
        {
            Save();
            Debug.Log("Data saved successfully!");
        }

        // Method to load data
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