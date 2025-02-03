using CustomArchitecture;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Comic
{
    public class TestInputTurnPage : AInputManager
    {
        [SerializeField] private TurningPage m_turningPage;

        #region ACTIONS
        private InputAction m_nextPageAction;
        private InputAction m_prevPageAction;
        
        #endregion ACTIONS

        #region CALLBACKS

        public Action<InputType, bool> onNextPageAction;
        public Action<InputType, bool> onPrevPageAction;

        #endregion CALLBACKS

        public override void Init() {}

        public void Awake()
        {
            onNextPageAction += NextPage;
            onPrevPageAction += PrevPage;
            FindAction();
            InitInputActions();
        }

        private void FindAction()
        {
            m_nextPageAction = ComicGameCore.Instance.GetInputAsset().FindAction("NextPage");
            m_prevPageAction = ComicGameCore.Instance.GetInputAsset().FindAction("PrevPage");
        }

        private void InitInputActions()
        {
            InputActionStruct<bool> iNextPage = new InputActionStruct<bool>(m_nextPageAction, onNextPageAction, false);
            InputActionStruct<bool> iPrevPage = new InputActionStruct<bool>(m_prevPageAction, onPrevPageAction, false);

            m_inputActionStructsBool.Add(iNextPage);
            m_inputActionStructsBool.Add(iPrevPage);
        }

        protected override void OnLateUpdate(float elapsed_time)
        {
            base.OnLateUpdate(elapsed_time);
        }

        private void NextPage(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                m_turningPage.NextPage();
            }
        }

        private void PrevPage(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                m_turningPage.PreviousPage();
            }
        }
    }
}
