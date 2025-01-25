using UnityEngine;
using static Comic.Comic;
using CustomArchitecture;

namespace Comic
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BoxBully : BaseBehaviour
    {
        [SerializeField, ReadOnly] private Rigidbody2D m_rigidbody;
        [SerializeField, ReadOnly] private Collider2D m_collider;
        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_collider = GetComponent<Collider2D>();

            m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer(playerLayerName))
            {
                return;
            }
            Player player = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPlayer();

            if (!player.CanPushBoxes())
            {
                return;
            }

            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
