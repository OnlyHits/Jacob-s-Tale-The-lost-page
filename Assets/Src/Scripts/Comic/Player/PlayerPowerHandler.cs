using System;
using System.Collections.Generic;
using CustomArchitecture;
using DG.Tweening;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

namespace Comic
{
    public partial class Player : Character
    {
        [Header("Powers")]
        [SerializeField, ReadOnly] private List<Power> m_powers;
        [SerializeField, ReadOnly] private PowerType m_powerTypeSelected = PowerType.Power_None;
        [SerializeField, ReadOnly] private Power m_powerSelected;

        [Header("Dummy")]
        [SerializeField] private GameObject m_dummyPrefab;
        private List<GameObject> m_dummies = new List<GameObject>();


        private Action m_onNextPower;
        private Action m_onPrevPower;

        private void Start()
        {
            OnPowerSelected(PowerType.Power_Rotate_Room);
        }

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

        #region ON POWER SELECTED
        private void OnPowerSelected(PowerType powerType)
        {
            Power power = GetPowerByType(powerType);
            if (power == null) return;
            m_powerTypeSelected = powerType;
            m_powerSelected = power;
        }
        #endregion ON POWER SELECTED

        #region ON SWITCH PAGE
        private void OnBeforeSwitchPage(bool _1, Page p1, Page p2)
        {
            DestroyDummies();
        }

        private void OnAfterSwitchPage(bool _1, Page p1, Page p2)
        {

        }
        #endregion ON SWITCH PAGE

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
            Debug.Log(on ? "ON" : "OFF" + " | Power Action " + m_powerTypeSelected.ToString());

            m_powerSelected?.Activate(on);

            if (m_powerTypeSelected == PowerType.Power_LegUp)
            {

            }
            else if (m_powerTypeSelected == PowerType.Power_Dummy)
            {
                DummyPower();
            }
            else if (m_powerTypeSelected == PowerType.Power_Telekinesis)
            {

            }
            else if (m_powerTypeSelected == PowerType.Power_Rotate_Room)
            {
                RotatePower(on);
            }
        }

        #region ROTATE POWER
        private void RotatePower(bool on)
        {
            if (on == false)
            {
                return;
            }

            Case c = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetCurrentCase();

            if (c == null)
            {
                Debug.LogWarning("Cannot rotate current case because no current case was found");
                return;
            }

            if (c.IsRotating())
            {
                return;
            }

            Pause(true);
            c.Rotate180(0.5f, () => Pause(false));
        }
        #endregion ROTATE POWER

        #region DUMMY POWER
        private void DummyPower()
        {
            GameObject dummy = Instantiate(m_dummyPrefab);
            dummy.transform.position = transform.position;
            m_dummies.Add(dummy);
        }
        private void DestroyDummies()
        {
            foreach (GameObject dummy in m_dummies)
            {
                if (dummy != null)
                {
                    Destroy(dummy);
                }
            }
        }
        #endregion DUMMY POWER
    }
}