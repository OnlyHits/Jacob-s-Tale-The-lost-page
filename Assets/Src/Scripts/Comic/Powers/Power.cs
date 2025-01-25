using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class Power : BaseBehaviour
    {
        [SerializeField] private PowerType m_powerType = PowerType.Power_None;
        public PowerType GetPowerType() => m_powerType;
    }
}
