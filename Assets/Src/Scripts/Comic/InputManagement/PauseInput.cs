using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public class PauseInput : AInputManager
    {
        private bool m_extPause = false;

        #region ACTIONS
        private InputAction m_pauseAction;

        #endregion ACTIONS

        #region CALLBACKS
        public Action<InputType, bool> onPause;

        #endregion CALLBACKS

        public override void Init()
        {
            onPause += OnPause;
            FindAction();
            InitInputActions();
        }

        private void FindAction()
        {
            m_pauseAction = InputSystem.actions.FindAction("Pause");
        }

        private void InitInputActions()
        {
            InputActionStruct<bool> iPause = new InputActionStruct<bool>(m_pauseAction, onPause, false);

            m_inputActionStructsBool.Add(iPause);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);
        }

        private void OnPause(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
                m_extPause = !m_extPause;
                ComicGameCore.Instance.MainGameMode.Pause(m_extPause);
            }
        }
    }
}