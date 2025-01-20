using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Comic
{
    public partial class Player : BaseBehaviour
    {
        public enum InputType
        {
            NONE = 0,
            PRESSED = 1,
            COMPUTED = 2,
            RELEASED = 3,
        }

        public class PlayerInputsController : BaseBehaviour
        {
            [Serializable]
            public class InputActionStruct<T>
            {
                public InputActionStruct(InputAction a, Action<InputType, T> cb, T v, bool cmpEchFrm = false)
                {
                    action = a;
                    callback = cb;
                    value = v;
                    computeEachFrame = cmpEchFrm;
                }

                public InputAction action;
                public Action<InputType, T> callback;
                public T value;
                // Invoke the callback each frame the key is pressed
                public bool computeEachFrame;

                public void InvokeCallback(InputType i, T v)
                {
                    callback?.Invoke(i, v);
                }
                public bool IsSameValue(T v)
                {
                    bool isSame = false;
                    if (value.GetType() == typeof(Vector2))
                    {
                        var vDest = (Vector2)Convert.ChangeType(v, typeof(Vector2));
                        var vSrc = (Vector2)Convert.ChangeType(value, typeof(Vector2));
                        isSame = vDest == vSrc;
                    }
                    if (value.GetType() == typeof(bool))
                    {
                        var vDest = (bool)Convert.ChangeType(v, typeof(bool));
                        var vSrc = (bool)Convert.ChangeType(value, typeof(bool));
                        isSame = vDest == vSrc;
                    }
                    return isSame;
                }
                public void SetValue(T v)
                {
                    value = v;
                }
                public T GetValue()
                {
                    if (value.GetType() == typeof(Vector2))
                    {
                        Vector2 res = action.ReadValue<Vector2>();
                        return (T)Convert.ChangeType(res, typeof(T));
                    }
                    if (value.GetType() == typeof(bool))
                    {
                        bool res = action.IsPressed();
                        return (T)Convert.ChangeType(res, typeof(T));
                    }

                    return value;
                }
            }

            [HideInInspector] public List<InputActionStruct<Vector2>> m_inputActionStructsV2 = new List<InputActionStruct<Vector2>>();
            [HideInInspector] public List<InputActionStruct<bool>> m_inputActionStructsBool = new List<InputActionStruct<bool>>();

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


            private void Awake()
            {
                m_moveAction = InputSystem.actions.FindAction("Move");
                m_lookAction = InputSystem.actions.FindAction("Look");
                m_jumpAction = InputSystem.actions.FindAction("Jump");
                m_sprintAction = InputSystem.actions.FindAction("Sprint");
                m_interactAction = InputSystem.actions.FindAction("Interact");
                m_nextPageAction = InputSystem.actions.FindAction("NextPage");
                m_prevPageAction = InputSystem.actions.FindAction("PrevPage");

            }

            private void Start()
            {
                InputActionStruct<Vector2> iMove = new InputActionStruct<Vector2>(m_moveAction, onMoveAction, Vector2.zero, true);
                InputActionStruct<Vector2> iLook = new InputActionStruct<Vector2>(m_lookAction, onLookAction, Vector2.zero, true);
                InputActionStruct<bool> iJump = new InputActionStruct<bool>(m_jumpAction, onJumpAction, false);
                InputActionStruct<bool> iSprint = new InputActionStruct<bool>(m_sprintAction, onSprintAction, false);
                InputActionStruct<bool> iInteract = new InputActionStruct<bool>(m_interactAction, onInteractAction, false);
                InputActionStruct<bool> iNextPage = new InputActionStruct<bool>(m_nextPageAction, onNextPageAction, false);
                InputActionStruct<bool> iPrevPage = new InputActionStruct<bool>(m_prevPageAction, onPrevPageAction, false);

                m_inputActionStructsV2.Add(iMove);
                m_inputActionStructsV2.Add(iLook);
                m_inputActionStructsBool.Add(iJump);
                m_inputActionStructsBool.Add(iSprint);
                m_inputActionStructsBool.Add(iInteract);
                m_inputActionStructsBool.Add(iNextPage);
                m_inputActionStructsBool.Add(iPrevPage);
            }

            protected override void OnUpdate(float elapsed_time)
            {
                base.OnUpdate(elapsed_time);

                foreach (var ias in m_inputActionStructsV2)
                {
                    TryGetAction<Vector2>(ias);
                }
                foreach (var ias in m_inputActionStructsBool)
                {
                    TryGetAction<bool>(ias);
                }
            }

            private void TryGetAction<T>(InputActionStruct<T> inputActStruct)
            {
                InputType input = GetInputType(inputActStruct.action);
                T value = inputActStruct.GetValue();

                if (input == InputType.NONE) return;

                if (input == InputType.PRESSED || input == InputType.RELEASED)
                {
                    inputActStruct.InvokeCallback(input, value);
                }

                else if (inputActStruct.computeEachFrame)
                {
                    // Invoke callback each frame
                    if (input == InputType.COMPUTED)
                    {
                        inputActStruct.InvokeCallback(input, value);
                    }
                }
                else
                {
                    // Invoke callback once
                    if (inputActStruct.IsSameValue(value) == false)
                    {
                        inputActStruct.InvokeCallback(input, value);
                    }
                }
                inputActStruct.SetValue(value);
            }

            private InputType GetInputType(InputAction action)
            {
                InputType input = InputType.NONE;
                bool pressed = action.WasPressedThisFrame();
                bool released = action.WasReleasedThisFrame();
                bool isComputed = action.IsPressed();

                input = pressed ? InputType.PRESSED : input;
                input = released ? InputType.RELEASED : input;
                input = !pressed && !released && isComputed ? InputType.COMPUTED : input;

                return input;
            }

        }
    }
}
