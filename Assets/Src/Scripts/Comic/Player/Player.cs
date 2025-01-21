using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public partial class Player : BaseBehaviour
    {
        [SerializeField, ReadOnly] private PlayerInputsController m_inputsController;
        [SerializeField] private Rigidbody2D m_rb;

        [Header("Grounded")]
        [SerializeField] private bool m_isGrounded = false;

        [Header("Move")]
        [SerializeField] private bool m_isMoving = false;
        [SerializeField] private float m_speed = 10f;

        [Header("Jump")]
        [SerializeField] private bool m_isJumping = false;
        [SerializeField] private float m_jumpForce = 10f;

        [Header("Fall")]
        [SerializeField] private bool m_isFalling = false;

        [Header("Others")]
        [SerializeField] private PageManager m_pageManager;

        protected void Awake()
        {
            m_inputsController = gameObject.AddComponent<PlayerInputsController>();
            m_inputsController.onMoveAction += OnMove;
            m_inputsController.onLookAction += OnLook;
            m_inputsController.onJumpAction += OnJump;
            m_inputsController.onSprintAction += OnSprint;
            m_inputsController.onInteractAction += OnInteract;
            m_inputsController.onNextPageAction += OnNextPage;
            m_inputsController.onPrevPageAction += OnPrevPage;

            m_inputsController.Init();
        }

        protected override void OnUpdate(float elapsed_time) { }
        protected override void OnFixedUpdate(float elapsed_time)
        {
            m_isGrounded = IsGrounded();

            if (!m_isGrounded && !m_isFalling)
            {
                TryFall();
            }
            else if (m_isFalling && m_isGrounded)
            {
                StopFall();
            }
        }
        protected override void OnLateUpdate(float elapsed_time) { }

        private bool IsGrounded()
        {
            return true;
        }

        #region MOVE
        private void StartMove(Vector2 v)
        {
            PlayRun(true);
            m_isMoving = true;
        }
        private void Move(Vector2 v)
        {
            if (!m_isGrounded)
            {
                return;
            }
            Vector2 newVel = v * m_speed;
            Vector2 expectedVel = (newVel - m_rb.linearVelocity) * Time.fixedDeltaTime;
            m_rb.AddForce(expectedVel, ForceMode2D.Force);
        }
        private void StopMove(Vector2 v)
        {
            PlayRun(false);
            m_isMoving = false;
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