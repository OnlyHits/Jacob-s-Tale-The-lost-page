using UnityEngine;

namespace Comic
{
    [ExecuteAlways]
    public class EditablePanelMargin : MonoBehaviour
    {
        [SerializeField] private float topOffset = 0.5f;
        [SerializeField] private float bottomOffset = 0.5f;
        [SerializeField] private float leftOffset = 0.5f;
        [SerializeField] private float rightOffset = 0.5f;        
        [SerializeField] private Color gapColor = new Color(1f, 0f, 0f, 0.3f);

        private void OnDrawGizmos()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
            
            Vector2 spriteSize = GetWorldSize(spriteRenderer);
            Vector3 spritePosition = transform.position;
            
            float halfWidth = spriteSize.x / 2f;
            float halfHeight = spriteSize.y / 2f;

            DrawFilledRectangle(new Vector3(spritePosition.x, spritePosition.y + halfHeight + (topOffset / 2f), 0), new Vector2(spriteSize.x, topOffset));
            DrawFilledRectangle(new Vector3(spritePosition.x, spritePosition.y - halfHeight - (bottomOffset / 2f), 0), new Vector2(spriteSize.x, bottomOffset));
            DrawFilledRectangle(new Vector3(spritePosition.x + halfWidth + (rightOffset / 2f), spritePosition.y, 0), new Vector2(rightOffset, spriteSize.y));
            DrawFilledRectangle(new Vector3(spritePosition.x - halfWidth - (leftOffset / 2f), spritePosition.y, 0), new Vector2(leftOffset, spriteSize.y));
        }

        private void DrawFilledRectangle(Vector3 position, Vector2 size)
        {
            Gizmos.color = new Color(gapColor.r, gapColor.g, gapColor.b, 0.3f);
            Gizmos.DrawCube(position, new Vector3(size.x, size.y, 0.01f));
        }

        private Vector2 GetWorldSize(SpriteRenderer spriteRenderer)
        {
            return spriteRenderer.bounds.size;
        }
    }
}
