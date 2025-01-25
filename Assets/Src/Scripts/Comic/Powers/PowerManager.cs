using System;
using System.Collections.Generic;
using CustomArchitecture;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Comic
{
    public class PowerManager : BaseBehaviour
    {
        [SerializeField] private List<Power> m_allPowers;

        public void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockPower(OnUnlockPower);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToLockPower(OnLockPower);
        }


        private void OnUnlockPower(PowerType powerType)
        {
            foreach (Power pow in m_allPowers)
            {
                if (pow.GetPowerType() == powerType)
                {
                    ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPlayer().AddPower(pow);
                }
            }
        }

        private void OnLockPower(PowerType powerType)
        {
            foreach (Power pow in m_allPowers)
            {
                if (pow.GetPowerType() == powerType)
                {
                    ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPlayer().RemovePower(pow);
                }
            }
        }
    }
}
