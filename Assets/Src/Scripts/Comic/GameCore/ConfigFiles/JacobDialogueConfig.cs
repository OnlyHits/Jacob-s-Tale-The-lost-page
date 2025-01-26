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
    
    public enum DialogueBubbleType
    {
        BubbleType_Speech,
        BubbleType_Exclamation,
        BubbleType_Thinking
    }

    public enum DialogueAppearIntensity
    {
        Intensity_Normal,
        Intensity_Medium,
        Intensity_Hard
    }

    public enum DialogueName
    {
        Dialogue_Welcome,
        Dialogue_ChangePage,
        Dialogue_UnlockBF,
        Dialogue_UnlockBully,
        Dialogue_UnlockBL,
        Dialogue_UnlockBoss,
        Dialogue_LostPage
    }

    [System.Serializable]
    public class PartOfDialogueChoiceConfig : PartOfDialogueConfig
    {
        public DialogueType m_choiceOneDialogue;
        public DialogueType m_choiceTwoDialogue;

        public override bool IsMultipleChoice()
        {
            return true;
        }
    }

    [System.Serializable]
    public class PartOfDialogueConfig
    {
        public virtual bool IsMultipleChoice()
        {
            return false;
        }

        public VoiceType m_speaker;
        public NpcIconType m_iconType;
        public DialogueType m_associatedDialogue;
        public DialogueAppearIntensity m_intensity;
        public DialogueBubbleType m_bubbleType;
        public bool m_isMainDialogue;
        public float m_waitAfterDisappear = 0f;        
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