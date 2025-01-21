using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public class NavigationInput : AInputManager
    {
        #region ACTIONS
        private InputAction m_nextPageAction;
        private InputAction m_prevPageAction;
        private InputAction m_navigationAction;
        private InputAction m_validateAction;
        private InputAction m_cancelAction;

        #endregion ACTIONS

        #region CALLBACKS
        public Action<InputType, bool> onNextPageAction;
        public Action<InputType, bool> onPrevPageAction;
        public Action<InputType, Vector2> onNavigationAction;
        public Action<InputType, bool> onValidateAction;
        public Action<InputType, bool> onCancelAction;

        #endregion CALLBACKS

        public override void Init()
        {
            onNextPageAction += OnNextPage;
            onPrevPageAction += OnPrevPage;
            onNavigationAction += OnNavigate;
            onCancelAction += OnCancel;
            onValidateAction += OnValidation;

            FindAction();
            InitInputActions();
        }

        private void FindAction()
        {
            m_nextPageAction = InputSystem.actions.FindAction("NextPage");
            m_prevPageAction = InputSystem.actions.FindAction("PrevPage");
            m_cancelAction = InputSystem.actions.FindAction("Cancel");
            m_validateAction = InputSystem.actions.FindAction("Validate");
            m_navigationAction = InputSystem.actions.FindAction("Navigation");
        }

        private void InitInputActions()
        {
            InputActionStruct<Vector2> iNavigate = new InputActionStruct<Vector2>(m_navigationAction, onNavigationAction, Vector2.zero, true);
            InputActionStruct<bool> iValidate = new InputActionStruct<bool>(m_validateAction, onValidateAction, false);
            InputActionStruct<bool> iCancel = new InputActionStruct<bool>(m_cancelAction, onCancelAction, false);
            InputActionStruct<bool> iNextPage = new InputActionStruct<bool>(m_nextPageAction, onNextPageAction, false);
            InputActionStruct<bool> iPrevPage = new InputActionStruct<bool>(m_prevPageAction, onPrevPageAction, false);

            m_inputActionStructsV2.Add(iNavigate);
            m_inputActionStructsBool.Add(iValidate);
            m_inputActionStructsBool.Add(iCancel);
            m_inputActionStructsBool.Add(iNextPage);
            m_inputActionStructsBool.Add(iPrevPage);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);
        }

        private void OnNavigate(InputType input, Vector2 v)
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

        private void OnValidation(InputType input, bool b)
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

        private void OnCancel(InputType input, bool b)
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