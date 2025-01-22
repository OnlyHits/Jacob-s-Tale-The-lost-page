
using System.Collections.Generic;
using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class Page : BaseBehaviour
    {
        [SerializeField] private GameObject visual;
        public void Enable(bool enable)
        {
            visual.SetActive(enable);
        }
    }
}
