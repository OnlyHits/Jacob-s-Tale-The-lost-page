using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public class PlayerInputsController : AInputManager
    {
        #region ACTIONS
        private InputAction m_nextPageAction;
        private InputAction m_prevPageAction;
        private InputAction m_settingPageAction;
        private InputAction m_creditPageAction;
        private InputAction m_gamePageAction;
        private InputAction m_menuPageAction;

        #endregion ACTIONS

        #region CALLBACKS
        public Action<InputType, bool> onNextPageAction;
        public Action<InputType, bool> onPrevPageAction;
        public Action<InputType, bool> onSettingPageAction;
        public Action<InputType, bool> onCreditPageAction;
        public Action<InputType, bool> onGamePageAction;
        public Action<InputType, bool> onMenuPageAction;

        #endregion CALLBACKS

        public override void Init()
        {
            FindAction();
            InitInputActions();
        }

        private void FindAction()
        {
            m_nextPageAction = InputSystem.actions.FindAction("NextPage");
            m_prevPageAction = InputSystem.actions.FindAction("PrevPage");
        }

        private void InitInputActions()
        {
            // InputActionStruct<Vector2> iSetting = new InputActionStruct<Vector2>(m_settingPageAction, on, Vector2.zero, true);
            // InputActionStruct<Vector2> iCredit = new InputActionStruct<Vector2>(m_creditPageAction, onLookAction, Vector2.zero, true);
            // InputActionStruct<bool> iGame = new InputActionStruct<bool>(m_gamePageAction, onJumpAction, false);
            // InputActionStruct<bool> iMenu = new InputActionStruct<bool>(m_menuPageAction, onSprintAction, false);
            // InputActionStruct<bool> iNextPage = new InputActionStruct<bool>(m_nextPageAction, onNextPageAction, false);
            // InputActionStruct<bool> iPrevPage = new InputActionStruct<bool>(m_prevPageAction, onPrevPageAction, false);

            // m_inputActionStructsV2.Add(iSetting);
            // m_inputActionStructsV2.Add(iCredit);
            // m_inputActionStructsBool.Add(iGame);
            // m_inputActionStructsBool.Add(iMenu);
            // m_inputActionStructsBool.Add(iNextPage);
            // m_inputActionStructsBool.Add(iPrevPage);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);
        }


        private void OnNextPage(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnPrevPage(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }
    }
}