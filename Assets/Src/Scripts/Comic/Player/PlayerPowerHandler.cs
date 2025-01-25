using System;
using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public partial class Player : Character
    {
        [Header("Powers")]
        [SerializeField, ReadOnly] private List<Power> m_powers;
        [SerializeField, ReadOnly] private PowerType m_powerTypeSelected;
        [SerializeField, ReadOnly] private Power m_powerSelected;

        private Action m_onNextPower;
        private Action m_onPrevPower;


        #region CALLBACKS
        public void SubscribeToNextPower(Action function)
        {
            m_onNextPower -= function;
            m_onNextPower += function;
        }

        public void SubscribeToPrevPower(Action function)
        {
            m_onPrevPower -= function;
            m_onPrevPower += function;
        }
        #endregion CALLBACKS

        #region ON POWER SELECT
        private void SelectNextPower()
        {
            m_onNextPower?.Invoke();
        }

        private void SelectPrevPower()
        {
            m_onPrevPower?.Invoke();
        }
        #endregion ON POWER SELECT

        #region ON POWER SELECTED
        private void OnPowerSelected(PowerType powerType)
        {
            Power power = GetPowerByType(powerType);
            if (power == null) return;
            m_powerTypeSelected = powerType;
            m_powerSelected = power;
        }
        #endregion ON POWER SELECTED

        #region POWERS ADD & REMOVE
        public void AddPower(Power power)
        {
            if (m_powers.Contains(power))
            {
                Debug.LogWarning("Add power " + power.GetPowerType().ToString() + " already assigned to player");
                return;
            }

            m_powers.Add(power);
        }

        public void RemovePower(Power power)
        {
            if (!m_powers.Contains(power))
            {
                Debug.LogWarning("Remove the power " + power.GetPowerType().ToString() + " which is not assigned to player");
                return;
            }

            m_powers.Remove(power);
        }
        #endregion POWERS ADD & REMOVE

        private Power GetPowerByType(PowerType powerType)
        {
            foreach (Power pow in m_powers)
            {
                if (pow.GetPowerType() == powerType)
                {
                    return pow;
                }
            }

            Debug.LogWarning("Trying to select power " + powerType.ToString() + " which is not in player power list");
            return null;
        }

        private void PowerAction(bool on)
        {

        }
    }
}