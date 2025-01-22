using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace CustomArchitecture
{
    public class TMP_AnimatedTextController : BaseBehaviour
    {
        [SerializeField] private float _waveSpeed;
        [SerializeField] private float _waveAmplitude;
        [SerializeField] private Vector2 _bounceSpeed;
        [SerializeField] private Vector2 _bounceAmplitude;
        [SerializeField] private Vector2 _woodleSpeed;
        [SerializeField] private Vector2 _woodleAmplitude;
        [SerializeField] private float _popDuration;

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
        }

        #endregion


    }
}