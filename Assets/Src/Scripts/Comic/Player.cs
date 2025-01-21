using CustomArchitecture;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace Comic
{
    public partial class Player : BaseBehaviour
    {
        [SerializeField, ReadOnly] private PlayerInputsController m_inputsController;
        [SerializeField] private Rigidbody2D m_rb;
        [SerializeField] private float m_speed = 10f;
        [SerializeField] private bool m_isGrounded = false;
        [SerializeField] private bool m_isMoving = false;
        [SerializeField] private float m_jumpForce = 10f;
        [SerializeField] private PageManager m_pageManager;
        //[SerializeField] private Rigidbody2D m_rb;

        protected void Awake()
        {
            m_inputsController = gameObject.AddComponent<PlayerInputsController>();
            m_inputsController.onMoveAction += OnMove;
            m_inputsController.onLookAction += OnLook;
            m_inputsController.onJumpAction += OnJump;
            m_inputsController.onSprintAction += OnSprint;
            m_inputsController.onInteractAction += OnInteract;
        }

        protected override void OnUpdate(float elapsed_time) { }
        protected override void OnFixedUpdate(float elapsed_time)
        {
            m_isGrounded = IsGrounded();
        }
        protected override void OnLateUpdate(float elapsed_time) { }

        private bool IsGrounded()
        {
            return true;
        }
        private void StartMove(Vector2 v)
        {
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
            m_isMoving = false;
        }

    }
}