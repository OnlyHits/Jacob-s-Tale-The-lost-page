using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ExtensionMethods;

public class SerializeDialogue : MonoBehaviour
{
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

    [Serializable]
    public struct ModifiableText
    {
        public string _text;
        public TextModifier _textModifier;
        public TextColor _colorModifier;
        public Color32 _solidColor;
        public Color32 _gradientColor;
        [Range(0.0f, 1.0f)] public float _colorSpeedRandomness;
        [HideInInspector] public int _firstIndex;
        [HideInInspector] public int _lastIndex;
        [HideInInspector] public List<float> _colorRandomSpeed;
    }

    [Serializable]
    public struct DialoguePartOfSide
    {
        public ModifiableText[] _modifiableText;
        [HideInInspector] public string _fullText;
    }

    [Serializable]
    public struct Dialogue
    {
        public string dialogueName;
        public bool _setupOnStart;
        [HideInInspector] public bool _isSetup;
        public DialoguePartOfSide[] _partOfSide;
    }
}