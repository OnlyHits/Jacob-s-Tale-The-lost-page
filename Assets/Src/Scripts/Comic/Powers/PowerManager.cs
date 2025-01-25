using System.Collections.Generic;
using CustomArchitecture;
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

            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData())
            {
                if (data.m_hasUnlockPower)
                {
                    PowerType powerType = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetPowerByChapter(data.m_chapterType);
                    OnUnlockPower(powerType);
                }
            }
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
