using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public partial class Player : Character
    {
        [SerializeField, ReadOnly] private PlayerInputsController m_inputsController;

        [Header("Grounded")]
        [SerializeField, ReadOnly] private bool m_isGrounded = false;

        [Header("Move")]
        [SerializeField, ReadOnly] private bool m_isMoving = false;
        [SerializeField] private float m_speed = 10f;

        [Header("Jump")]
        [SerializeField, ReadOnly] private bool m_isJumping = false;
        [SerializeField] private float m_jumpForce = 10f;

        [Header("Fall")]
        [SerializeField, ReadOnly] private bool m_isFalling = false;

        [Header("Others")]
        [SerializeField] private PageManager m_pageManager;

        protected override void Awake()
        {
            base.Awake();
            m_inputsController = gameObject.AddComponent<PlayerInputsController>();
            m_inputsController.onMoveAction += OnMove;
            m_inputsController.onLookAction += OnLook;
            m_inputsController.onJumpAction += OnJump;
            m_inputsController.onSprintAction += OnSprint;
            m_inputsController.onInteractAction += OnInteract;
            m_inputsController.onNextPageAction += OnNextPage;
            m_inputsController.onPrevPageAction += OnPrevPage;
            m_inputsController.onPowerAction += OnPower;
            m_inputsController.onNextPowerAction += OnNextPower;
            m_inputsController.onPrevPowerAction += OnPrevPower;

            m_inputsController.Init();
        }

        public override void Init()
        {
            base.Init();

            m_pageManager = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPageManager();

            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToPowerSelected(OnPowerSelected);

            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToBeforeSwitchPage(OnBeforeSwitchPage);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToAfterSwitchPage(OnAfterSwitchPage);
        }

        public override void Pause(bool pause = true)
        {
            base.Pause(pause);
            m_inputsController.Pause(pause);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);
        }
        protected override void OnFixedUpdate(float elapsed_time)
        {
            base.OnFixedUpdate(elapsed_time);
            m_isGrounded = IsGrounded();

            /*if (!m_isGrounded && !m_isFalling)
            {
                TryFall();
            }
            else if (m_isFalling && m_isGrounded)
            {
                StopFall();
            }*/
        }
        protected override void OnLateUpdate(float elapsed_time)
        {
            base.OnLateUpdate(elapsed_time);
        }

        private bool IsGrounded()
        {
            return true;
        }

        #region MOVE
        private void StartMove(Vector2 v)
        {
            PlayRun(true);
            m_isMoving = true;

            Vector2 newVel = new Vector2(0, m_rb.linearVelocity.y);
            m_rb.linearVelocity = newVel;
        }
        private void Move(Vector2 v)
        {
            SetSprireFaceDirection(v);

            if (!m_isGrounded)
            {
                return;
            }
            Vector2 newVel = v * m_speed;
            Vector2 currentVel = new Vector2(m_rb.linearVelocity.x, m_isJumping ? 0 : m_rb.linearVelocity.y);
            Vector2 expectedVel = (newVel - currentVel) * Time.fixedDeltaTime;
            m_rb.linearVelocityX = expectedVel.x;
            //m_rb.AddForce(expectedVel, ForceMode2D.Force);
        }
        private void StopMove(Vector2 v)
        {
            PlayRun(false);
            m_isMoving = false;

            Vector2 newVel = new Vector2(0, m_rb.linearVelocity.y);
            m_rb.linearVelocity = newVel;
        }
        #endregion MOVE

        #region JUMP
        private void TryJump()
        {
            if (!m_isGrounded)
            {
                return;
            }
            PlayJump(true);
            m_isJumping = true;

            Vector2 direction = Vector2.up;
            //Vector2 newVel = m_jumpForce * direction;
            //Vector2 currentVel = new Vector2(0, m_rb.linearVelocity.y);
            //Vector2 expectedVel = (newVel - currentVel) * Time.fixedDeltaTime;
            //m_rb.linearVelocityY = expectedVel.y;
            m_rb.AddForce(m_jumpForce * direction, ForceMode2D.Impulse);
        }
        #endregion JUMP


        #region FALL
        private void TryFall()
        {
            if (m_isGrounded || m_isJumping || m_isFalling)
            {
                return;
            }
            m_isFalling = true;
            PlayFall(true);
        }

        private void StopFall()
        {
            if (!m_isGrounded && m_isFalling)
            {
                return;
            }
            m_isFalling = false;
            PlayFall(false);
        }
        #endregion FALL

    }
}