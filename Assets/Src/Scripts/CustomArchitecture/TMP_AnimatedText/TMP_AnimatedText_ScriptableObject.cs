using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using ExtensionMethods;

namespace CustomArchitecture
{
    public enum DialogueApparitionType
    {
        INCREMENTAL,
        SIMULTANEOUS
    }

    public enum TextColor
    {
        SOLID,
        HORIZONTAL_GRADIENT,
        VERTICAL_GRADIENT
    }

    public enum TextModifier
    {
        NONE,
        WODDLE,
        WAVE,
        RAND_BOUNCE,
        BOUNCE
    }

    public enum DialogueType
    {
        Prequel,
        Tutorial_ChangePage_Jacob_1,
        Tutorial_ChangePage_Mom_1,
        Tutorial_ChangePage_Jacob_2,
        Tutorial_ChangePage_Mom_2,
        Tutorial_ChangePage_Jacob_3,
        Tutorial_ChangePage_Mom_3,
        Tutorial_ChangePage_Jacob_4,
        MainStory_UnlockBF_BestFriend_1,
        MainStory_UnlockBF_Jacob_1,
        MainStory_UnlockBF_BestFriend_2,
        MainStory_UnlockBF_Jacob_2,
        MainStory_UnlockBF_BestFriend_3,
        MainStory_UnlockBF_Jacob_3,
        MainStory_UnlockBF_BestFriend_4,
        MainStory_UnlockBF_Choice_1,
        MainStory_UnlockBF_Choice_2,
        MainStory_UnlockBully_Bully_1,
        MainStory_UnlockBully_Jacob_1,
        MainStory_UnlockBully_Jacob_2,
        MainStory_UnlockBully_Bully_2,
        MainStory_UnlockBully_BestFriend_1,
        MainStory_UnlockBully_Jacob_3,
        MainStory_UnlockBully_Bully_3,
        MainStory_UnlockBully_Jacob_4,
        MainStory_UnlockBully_Bully_4,
        MainStory_UnlockBully_BestFriend_2,
        MainStory_UnlockBully_Bully_5,
        MainStory_UnlockBully_BestFriend_3,
        MainStory_UnlockBully_Bully_6,
        MainStory_UnlockBully_BestFriend_4,
        MainStory_UnlockBully_Jacob_5,
        MainStory_UnlockBully_Bully_7,
        MainStory_UnlockBully_Choice_1,
        MainStory_UnlockBully_Choice_2,
        MainStory_UnlockBL_Jacob_1,
        MainStory_UnlockBL_BL_1,
        MainStory_UnlockBL_Bully_1,
        MainStory_UnlockBL_BestFriend_1,
        MainStory_UnlockBL_Jacob_2,
        MainStory_UnlockBL_BL_2,
        MainStory_UnlockBL_Bully_2,
        MainStory_UnlockBL_Jacob_3,
        MainStory_UnlockBL_BL_3,
        MainStory_UnlockBL_BestFriend_2,
        MainStory_UnlockBL_Jacob_4,
        MainStory_UnlockBL_BestFriend_3,
        MainStory_UnlockBL_BL_4,
        MainStory_UnlockBL_Jacob_5,
        MainStory_UnlockBL_Bully_3,
        MainStory_UnlockBL_BestFriend_4,
        MainStory_UnlockBL_BL_5,
        MainStory_UnlockBL_Jacob_6,
        MainStory_UnlockBL_BL_6,
        MainStory_UnlockBL_Choice_1,
        MainStory_UnlockBL_Choice_2,
        MainStory_UnlockBoss_Boss_1,
        MainStory_UnlockBoss_Jacob_1,
        MainStory_UnlockBoss_BestFriend_1,
        MainStory_UnlockBoss_Boss_2,
        MainStory_UnlockBoss_Bully_1,
        MainStory_UnlockBoss_BL_1,
        MainStory_UnlockBoss_Boss_3,
        MainStory_UnlockBoss_TheLostPage_1,
    }

    public struct DynamicAnimatedTextData
    {
        public int m_firstIndex;
        public int m_lastIndex;
        public List<float> m_colorRandomSpeed;
    }

    public struct DynamicSentenceData
    {
        public DynamicAnimatedTextData[] m_animatedTextDatas;
        public string m_fullText;
    }

    public struct DynamicDialogueData
    {
        public DynamicSentenceData[] m_sentenceData;
    }

    [Serializable]
    public struct AnimatedTextConfig
    {
        public string m_text;
        public TextModifier m_textModifier;
        public TextColor m_colorModifier;
        public Color32 m_solidColor;
        public Color32 m_gradientColor;
        [Range(0.0f, 1.0f)] public float m_colorSpeedRandomness;
    }

    [Serializable]
    public struct DialogueSentenceConfig
    {
        public AnimatedTextConfig[] m_animatedText;
        public DialogueApparitionType m_apparitionType;
    }

    [Serializable]
    public struct DialogueConfig
    {
        public DialogueSentenceConfig[] m_dialogueSentences;
        public bool m_handleByInput;
        public float m_durationBetweenSentence;
    }


    public interface TMP_AnimatedText_ScriptableObject_Provider
    {
        public DynamicDialogueData GetDialogueDatas(DialogueType type);
        public DialogueConfig GetDialogueConfig(DialogueType type);
    }
    
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Comic/Dialogue")]
    [System.Serializable]
    public class TMP_AnimatedText_ScriptableObject : SerializedScriptableObject, TMP_AnimatedText_ScriptableObject_Provider
    {
        [ShowInInspector] public string m_saveName;
        [OdinSerialize, ShowInInspector] public Dictionary<DialogueType, DialogueConfig>  m_dialogues;
        private Dictionary<DialogueType, DynamicDialogueData> m_dynamicDatas;

        public void Init(string save_name)
        {
            m_dynamicDatas = new();
            m_saveUtilitary = new SaveUtilitary<Dictionary<DialogueType, DialogueConfig>>(save_name, FileType.ConfigFile);
            Load();
            SetupDialogue();
        }

        public DynamicDialogueData GetDialogueDatas(DialogueType type)
        {
            if (!m_dynamicDatas.ContainsKey(type))
            {
                Debug.LogWarning("Dialogue didn't exist");
            }

            return m_dynamicDatas[type];
        }
        
        public DialogueConfig GetDialogueConfig(DialogueType type)
        {
            if (!m_dialogues.ContainsKey(type))
            {
                Debug.LogWarning("Dialogue didn't exist");
            }

            return m_dialogues[type];
        }

        #region ScriptableObject

        [NonSerialized] private SaveUtilitary<Dictionary<DialogueType, DialogueConfig>>   m_saveUtilitary;

        [Button("Save")]
        private void SaveData()
        {
            SaveUtilitary<Dictionary<DialogueType, DialogueConfig>> s = new SaveUtilitary<Dictionary<DialogueType, DialogueConfig>>(m_saveName, FileType.ConfigFile);
            s.Save(m_dialogues);

            Debug.Log("Data saved successfully!");
        }

        [Button("Load")]
        private void LoadData()
        {
            SaveUtilitary<Dictionary<DialogueType, DialogueConfig>> s = new SaveUtilitary<Dictionary<DialogueType, DialogueConfig>>(m_saveName, FileType.ConfigFile);
            m_dialogues = s.Load();
            Debug.Log("Data loaded successfully!");
        }
        
        public void Save()
        {
            m_saveUtilitary.Save(m_dialogues);
        }

        public void Load()
        {
            m_dialogues = m_saveUtilitary.Load();
        }

        #endregion
    
        #region DynamicDatas

        public void SetupDialogue()
        {
            foreach (var dialogue in m_dialogues)
            {
                int index = 0;
                DialogueConfig config = dialogue.Value;

                DynamicDialogueData d = new DynamicDialogueData();
                d.m_sentenceData = new DynamicSentenceData[config.m_dialogueSentences.Length];

                m_dynamicDatas.Add(dialogue.Key, d);

                for (int j = 0; j < config.m_dialogueSentences.Length; ++j, index = 0)
                {
                    m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_animatedTextDatas = new DynamicAnimatedTextData[config.m_dialogueSentences[j].m_animatedText.Length];

                    for (int i = 0; i < config.m_dialogueSentences[j].m_animatedText.Length; ++i)
                    {
                        m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_animatedTextDatas[i].m_colorRandomSpeed = new List<float>();

                        if (config.m_dialogueSentences[j].m_animatedText[i].m_text.Contains("\\n"))
                        {
                            config.m_dialogueSentences[j].m_animatedText[i].m_text = config.m_dialogueSentences[j].m_animatedText[i].m_text.Replace("\\n", "\n");
                        }

                        foreach (char c in config.m_dialogueSentences[j].m_animatedText[i].m_text)
                        {
                            if (c != ' ' && c != '\n')
                            {
                                float random = UnityEngine.Random.Range(0.0f, 1.0f * config.m_dialogueSentences[j].m_animatedText[i].m_colorSpeedRandomness);
                                m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_animatedTextDatas[i].m_colorRandomSpeed.Add(1.0f - random);
                            }
                        }
                        
                        int substringToDeduce = config.m_dialogueSentences[j].m_animatedText[i].m_text.CountSubstring("<i>") * 3 + config.m_dialogueSentences[j].m_animatedText[i].m_text.CountSubstring("</i>") * 4;
                        substringToDeduce += config.m_dialogueSentences[j].m_animatedText[i].m_text.CountSubstring("<b>") * 3 + config.m_dialogueSentences[j].m_animatedText[i].m_text.CountSubstring("</b>") * 4;
                        m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_animatedTextDatas[i].m_firstIndex = index;
                        m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_animatedTextDatas[i].m_lastIndex = index + config.m_dialogueSentences[j].m_animatedText[i].m_text.Length - substringToDeduce;
                        index = m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_animatedTextDatas[i].m_lastIndex;
                        m_dynamicDatas[dialogue.Key].m_sentenceData[j].m_fullText += config.m_dialogueSentences[j].m_animatedText[i].m_text;
                    }
                }
            }
        }

        #endregion
    }
}