using System;
using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using CustomArchitecture;

namespace Comic
{
    public enum NpcIconType
    {
        Icon_Beloved,
        Icon_BestFriend,
        Icon_Boss_1,
        Icon_Boss_2,
        Icon_Bully,
        Icon_Jacob_0,
        Icon_Jacob_1,
        Icon_Jacob_2,
        Icon_Jacob_3,
        Icon_Jacob_4,
    }
    
    public enum DialogueAppearIntensity
    {
        Intensity_Normal,
        Intensity_Medium,
        Intensity_Hard
    }

    public enum DialogueName
    {
        DialogueWelcome,
    }

    [System.Serializable]
    public class PartOfDialogueConfig
    {
        public VoiceType m_speaker;
        public NpcIconType m_iconType;
        public DialogueType m_associatedDialogue;
        public DialogueAppearIntensity m_intensity;
        public bool m_isMainDialogue;
    }

    [CreateAssetMenu(fileName = "JacobDialogueConfig", menuName = "Comic/JacobDialogueConfig")]
    [System.Serializable]
    public class JacobDialogueConfig : SerializedScriptableObject
    {
        [NonSerialized] private readonly SaveUtilitary<Dictionary<DialogueName, List<PartOfDialogueConfig>>> m_saveUtilitary;

        [OdinSerialize, ShowInInspector] public Dictionary<DialogueName, List<PartOfDialogueConfig>> m_config;

        public JacobDialogueConfig()
        {
            m_saveUtilitary = new SaveUtilitary<Dictionary<DialogueName, List<PartOfDialogueConfig>>>("JacobDialogueConfig", FileType.ConfigFile);

            Load();
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

        public Dictionary<DialogueName, List<PartOfDialogueConfig>> GetConfig()
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