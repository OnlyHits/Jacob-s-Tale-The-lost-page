using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Sirenix.OdinInspector;

namespace CustomArchitecture
{
    public interface TMP_AnimatedTextController_Provider
    {
        public DynamicDialogueData GetDialogueDatas<T>(DialogueType type) where T : AGameCore<T>;
        public DialogueConfig GetDialogueConfig<T>(DialogueType type) where T : AGameCore<T>;
    }

    public class TMP_AnimatedTextController : BaseBehaviour, TMP_AnimatedTextController_Provider
    {
        public float m_waveSpeed;
        public float m_waveAmplitude;
        public Vector2 m_bounceSpeed;
        public Vector2 m_bounceAmplitude;
        public Vector2 m_woodleSpeed;
        public Vector2 m_woodleAmplitude;
        public float m_popDuration;
        public float m_simultaneousApparitionDuration;

        // Prevent direct instantiation
        protected TMP_AnimatedTextController() { }

        #region Singleton

        private static TMP_AnimatedTextController m_instance;

        public static TMP_AnimatedTextController Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindFirstObjectByType<TMP_AnimatedTextController>();

                    if (m_instance == null)
                    {
                        GameObject singletonObject = new GameObject("TMP_AnimatedTextController");
                        m_instance = singletonObject.AddComponent<TMP_AnimatedTextController>();
                    }
                }
                return m_instance;
            }
            private set
            {
                m_instance = value;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log("Already instantiate");
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            OnAwake();
        }

        #endregion

        private Dictionary<Language, TMP_AnimatedText_ScriptableObject> m_dialogues;

        private void OnAwake()
        {
            m_dialogues = new()
            {
                { Language.French, SerializedScriptableObject.CreateInstance<TMP_AnimatedText_ScriptableObject>() },
                { Language.English, SerializedScriptableObject.CreateInstance<TMP_AnimatedText_ScriptableObject>() },
            };

            m_dialogues[Language.French].Init("French_Dialogue");
            m_dialogues[Language.English].Init("English_Dialogue");
        }

        public DynamicDialogueData GetDialogueDatas<T>(DialogueType type) where T : AGameCore<T>
        {
            return m_dialogues[AGameCore<T>.Instance.GetSettings().m_settingDatas.m_language].GetDialogueDatas(type);
        }
        
        public DialogueConfig GetDialogueConfig<T>(DialogueType type) where T : AGameCore<T>
        {
            return m_dialogues[AGameCore<T>.Instance.GetSettings().m_settingDatas.m_language].GetDialogueConfig(type);
        }
    }
}