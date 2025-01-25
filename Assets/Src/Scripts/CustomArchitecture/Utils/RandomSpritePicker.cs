using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSpritePicker : BaseBehaviour
{
    [SerializeField, ReadOnly] private SpriteRenderer m_spriteRd;
    [SerializeField] private List<Sprite> m_spriteList;

    private void Awake()
    {
        m_spriteRd = GetComponent<SpriteRenderer>();

        if (m_spriteList?.Count <= 0)
        {
            Debug.LogWarning("None sprite assigned in list to randomize & set one in RandomSpritePicker");
            return;
        }

        m_spriteRd.sprite = m_spriteList[Random.Range(0, m_spriteList.Count - 1)];
    }
}
