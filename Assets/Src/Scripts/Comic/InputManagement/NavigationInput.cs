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

        private InputAction m_navigationAction;
        private InputAction m_validateAction;
        private InputAction m_cancelAction;

        #endregion ACTIONS

        #region CALLBACKS
        public Action<InputType, Vector2> onNavigationAction;
        public Action<InputType, bool> onValidateAction;
        public Action<InputType, bool> onCancelAction;

        #endregion CALLBACKS

        public void SubscribeToNavigate(Action<InputType, Vector2> function)
        {
            onNavigationAction -= function;
            onNavigationAction += function;
        }

        public void SubscribeToValidate(Action<InputType, bool> function)
        {
            onValidateAction -= function;
            onValidateAction += function;
        }

        public void SubscribeToCancel(Action<InputType, bool> function)
        {
            onCancelAction -= function;
            onCancelAction += function;
        }

        public override void Init()
        {
            FindAction();
            InitInputActions();
        }

        private void FindAction()
        {
            m_cancelAction = ComicGameCore.Instance.MainGameMode.GetInputAsset().FindAction("Cancel");
            m_validateAction = ComicGameCore.Instance.MainGameMode.GetInputAsset().FindAction("Validate");
            m_navigationAction = ComicGameCore.Instance.MainGameMode.GetInputAsset().FindAction("Navigation");
        }

        private void InitInputActions()
        {
            InputActionStruct<Vector2> iNavigate = new InputActionStruct<Vector2>(m_navigationAction, onNavigationAction, Vector2.zero, true);
            InputActionStruct<bool> iValidate = new InputActionStruct<bool>(m_validateAction, onValidateAction, false);
            InputActionStruct<bool> iCancel = new InputActionStruct<bool>(m_cancelAction, onCancelAction, false);

            m_inputActionStructsV2.Add(iNavigate);
            m_inputActionStructsBool.Add(iValidate);
            m_inputActionStructsBool.Add(iCancel);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);
        }
   }
}