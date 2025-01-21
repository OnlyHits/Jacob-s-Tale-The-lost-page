using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public partial class Player : BaseBehaviour
    {
        public class PlayerInputsController : AInputManager
        {
            #region ACTIONS
            private InputAction m_moveAction;
            private InputAction m_lookAction;
            private InputAction m_jumpAction;
            private InputAction m_sprintAction;
            private InputAction m_interactAction;
            private InputAction m_nextPageAction;
            private InputAction m_prevPageAction;

            #endregion ACTIONS

            #region CALLBACKS
            public Action<InputType, Vector2> onMoveAction;
            public Action<InputType, Vector2> onLookAction;
            public Action<InputType, bool> onJumpAction;
            public Action<InputType, bool> onSprintAction;
            public Action<InputType, bool> onInteractAction;
            public Action<InputType, bool> onNextPageAction;
            public Action<InputType, bool> onPrevPageAction;

            #endregion CALLBACKS

            public override void Init()
            {
                FindAction();
                InitInputActions();
            }

            private void FindAction()
            {
                m_moveAction = InputSystem.actions.FindAction("Move");
                m_lookAction = InputSystem.actions.FindAction("Look");
                m_jumpAction = InputSystem.actions.FindAction("Jump");
                m_sprintAction = InputSystem.actions.FindAction("Sprint");
                m_interactAction = InputSystem.actions.FindAction("Interact");
                // m_nextPageAction = InputSystem.actions.FindAction("NextPage");
                // m_prevPageAction = InputSystem.actions.FindAction("PrevPage");
            }

            private void InitInputActions()
            {
                InputActionStruct<Vector2> iMove = new InputActionStruct<Vector2>(m_moveAction, onMoveAction, Vector2.zero, true);
                InputActionStruct<Vector2> iLook = new InputActionStruct<Vector2>(m_lookAction, onLookAction, Vector2.zero, true);
                InputActionStruct<bool> iJump = new InputActionStruct<bool>(m_jumpAction, onJumpAction, false);
                InputActionStruct<bool> iSprint = new InputActionStruct<bool>(m_sprintAction, onSprintAction, false);
                InputActionStruct<bool> iInteract = new InputActionStruct<bool>(m_interactAction, onInteractAction, false);
                // InputActionStruct<bool> iNextPage = new InputActionStruct<bool>(m_nextPageAction, onNextPageAction, false);
                // InputActionStruct<bool> iPrevPage = new InputActionStruct<bool>(m_prevPageAction, onPrevPageAction, false);

                m_inputActionStructsV2.Add(iMove);
                m_inputActionStructsV2.Add(iLook);
                m_inputActionStructsBool.Add(iJump);
                m_inputActionStructsBool.Add(iSprint);
                m_inputActionStructsBool.Add(iInteract);
                // m_inputActionStructsBool.Add(iNextPage);
                // m_inputActionStructsBool.Add(iPrevPage);
            }

            protected override void OnUpdate(float elapsed_time)
            {
                base.OnUpdate(elapsed_time);
            }
        }
    }
}
