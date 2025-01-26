using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Comic
{
    //public partial class Player : Character
    //{
    public class PlayerInputsController : AInputManager
    {
        public InputActionAsset m_inputActionAsset;

        #region ACTIONS
        private InputAction m_moveAction;
        private InputAction m_lookAction;
        private InputAction m_jumpAction;
        private InputAction m_sprintAction;
        private InputAction m_interactAction;
        private InputAction m_nextPageAction;
        private InputAction m_prevPageAction;
        private InputAction m_powerAction;
        private InputAction m_nextPowerAction;
        private InputAction m_prevPowerAction;

        #endregion ACTIONS

        #region CALLBACKS
        public Action<InputType, Vector2> onMoveAction;
        public Action<InputType, Vector2> onLookAction;
        public Action<InputType, bool> onJumpAction;
        public Action<InputType, bool> onSprintAction;
        public Action<InputType, bool> onInteractAction;
        public Action<InputType, bool> onNextPageAction;
        public Action<InputType, bool> onPrevPageAction;
        public Action<InputType, bool> onPowerAction;
        public Action<InputType, bool> onNextPowerAction;
        public Action<InputType, bool> onPrevPowerAction;

        #endregion CALLBACKS

        public override void Init()
        {
            FindAction();
            InitInputActions();
        }

        private void FindAction()
        {
            m_moveAction = m_inputActionAsset.FindAction("Move");
            m_lookAction = m_inputActionAsset.FindAction("Look");
            m_jumpAction = m_inputActionAsset.FindAction("Jump");
            m_sprintAction = m_inputActionAsset.FindAction("Sprint");
            m_interactAction = m_inputActionAsset.FindAction("Interact");
            m_nextPageAction = m_inputActionAsset.FindAction("NextPage");
            m_prevPageAction = m_inputActionAsset.FindAction("PrevPage");
            m_powerAction = m_inputActionAsset.FindAction("Power");
            m_nextPowerAction = m_inputActionAsset.FindAction("NextPower");
            m_prevPowerAction = m_inputActionAsset.FindAction("PrevPower");
        }

        private void InitInputActions()
        {
            InputActionStruct<Vector2> iMove = new InputActionStruct<Vector2>(m_moveAction, onMoveAction, Vector2.zero, true);
            InputActionStruct<Vector2> iLook = new InputActionStruct<Vector2>(m_lookAction, onLookAction, Vector2.zero, true);
            InputActionStruct<bool> iJump = new InputActionStruct<bool>(m_jumpAction, onJumpAction, false);
            InputActionStruct<bool> iSprint = new InputActionStruct<bool>(m_sprintAction, onSprintAction, false);
            InputActionStruct<bool> iInteract = new InputActionStruct<bool>(m_interactAction, onInteractAction, false);
            InputActionStruct<bool> iNextPage = new InputActionStruct<bool>(m_nextPageAction, onNextPageAction, false);
            InputActionStruct<bool> iPrevPage = new InputActionStruct<bool>(m_prevPageAction, onPrevPageAction, false);
            InputActionStruct<bool> iPower = new InputActionStruct<bool>(m_powerAction, onPowerAction, false);
            InputActionStruct<bool> iNextPower = new InputActionStruct<bool>(m_nextPowerAction, onNextPowerAction, false);
            InputActionStruct<bool> iPrevPower = new InputActionStruct<bool>(m_prevPowerAction, onPrevPowerAction, false);

            m_inputActionStructsV2.Add(iMove);
            m_inputActionStructsV2.Add(iLook);
            m_inputActionStructsBool.Add(iJump);
            m_inputActionStructsBool.Add(iSprint);
            m_inputActionStructsBool.Add(iInteract);
            m_inputActionStructsBool.Add(iNextPage);
            m_inputActionStructsBool.Add(iPrevPage);
            m_inputActionStructsBool.Add(iPower);
            m_inputActionStructsBool.Add(iNextPower);
            m_inputActionStructsBool.Add(iPrevPower);
        }

        protected override void OnLateUpdate(float elapsed_time)
        {
            base.OnLateUpdate(elapsed_time);
        }
    }
    //}
}
