using UnityEngine;
using CustomArchitecture;

namespace Comic
{
    [ExecuteAlways]
    public class EditablePageMargin : BaseBehaviour
    {
        public SpriteRenderer parentSpriteRenderer;

        public float topOffset = 0f;
        public float bottomOffset = 0f;
        public float leftOffset = 0f;
        public float rightOffset = 0f;

        public Color gapColor = new Color(1f, 0f, 0f, 0.3f);

        private void Start()
        {
            if (parentSpriteRenderer == null)
            {
                parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
            }
        }

        protected override void OnUpdate(float elasped_time)
        {
            #if UNITY_EDITOR
            MatchParentSizeAndCenter();
            #endif
        }

        public void MatchParentSizeAndCenter()
        {
            if (parentSpriteRenderer == null) return;

            SpriteRenderer childSpriteRenderer = GetComponent<SpriteRenderer>();
            if (childSpriteRenderer == null) return;

            // Get parent size in world space
            Vector2 parentSize = GetWorldSize(parentSpriteRenderer);

            // Adjust parent size with offsets
            Vector2 adjustedSize = new Vector2(
                parentSize.x + leftOffset + rightOffset,
                parentSize.y + topOffset + bottomOffset
            );

            // Get child size in world space
            Vector2 childSize = GetWorldSize(childSpriteRenderer);

            // Avoid division by zero
            if (childSize.x == 0) childSize.x = 1;
            if (childSize.y == 0) childSize.y = 1;

            // Scale child to match adjusted parent size
            transform.localScale = new Vector3(
                transform.localScale.x * (adjustedSize.x / childSize.x),
                transform.localScale.y * (adjustedSize.y / childSize.y),
                1f
            );

            transform.position = GetCenteredPosition(true);
        }

        private Vector2 GetWorldSize(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer == null || spriteRenderer.sprite == null) return Vector2.one;
            return spriteRenderer.sprite.bounds.size * (Vector2)spriteRenderer.transform.lossyScale;
        }

        private Vector3 GetCenteredPosition(bool adjust_position)
        {
            Vector3 parentPos = parentSpriteRenderer.transform.position;

            if (parentSpriteRenderer.sprite == null)
                return parentPos;

            Vector2 spriteSize = parentSpriteRenderer.sprite.bounds.size;

            Vector2 pivot = parentSpriteRenderer.sprite.pivot / parentSpriteRenderer.sprite.rect.size;

            // Correct pivot adjustment
            Vector3 pivotOffset = new Vector3(
                (0.5f - pivot.x) * spriteSize.x * parentSpriteRenderer.transform.lossyScale.x,
                (0.5f - pivot.y) * spriteSize.y * parentSpriteRenderer.transform.lossyScale.y,
                0f
            );

            if (adjust_position)
            {
                pivotOffset += new Vector3(
                    (rightOffset - leftOffset) / 2f,
                    (topOffset - bottomOffset) / 2f,
                    0f
                );
            }

            return parentPos + pivotOffset;
        }

        private void OnDrawGizmos()
        {
            if (parentSpriteRenderer == null) return;

            SpriteRenderer childSpriteRenderer = GetComponent<SpriteRenderer>();
            if (childSpriteRenderer == null) return;

            Vector2 parentSize = GetWorldSize(parentSpriteRenderer);
            Vector2 childSize = GetWorldSize(childSpriteRenderer);

            Vector3 parentPosition = GetCenteredPosition(false);
            Vector3 childPosition = transform.position;

            float parentHalfWidth = parentSize.x / 2f;
            float parentHalfHeight = parentSize.y / 2f;

            float childHalfWidth = childSize.x / 2f;
            float childHalfHeight = childSize.y / 2f;

            // Top Gap
            float topGapHeight = (parentPosition.y + parentHalfHeight) - (childPosition.y + childHalfHeight);
            if (topGapHeight > 0)
                DrawFilledRectangle(new Vector3(childPosition.x, childPosition.y + childHalfHeight + (topGapHeight / 2f), 0), new Vector2(childSize.x, topGapHeight));

            // Bottom Gap
            float bottomGapHeight = (childPosition.y - childHalfHeight) - (parentPosition.y - parentHalfHeight);
            if (bottomGapHeight > 0)
                DrawFilledRectangle(new Vector3(childPosition.x, childPosition.y - childHalfHeight - (bottomGapHeight / 2f), 0), new Vector2(childSize.x, bottomGapHeight));

            // Right Gap
            float rightGapWidth = (parentPosition.x + parentHalfWidth) - (childPosition.x + childHalfWidth);
            if (rightGapWidth > 0)
                DrawFilledRectangle(new Vector3(childPosition.x + childHalfWidth + (rightGapWidth / 2f), childPosition.y, 0), new Vector2(rightGapWidth, childSize.y));

            // Left Gap
            float leftGapWidth = (childPosition.x - childHalfWidth) - (parentPosition.x - parentHalfWidth);
            if (leftGapWidth > 0)
                DrawFilledRectangle(new Vector3(childPosition.x - childHalfWidth - (leftGapWidth * .5f), childPosition.y, 0), new Vector2(leftGapWidth, childSize.y));

        }

        private void DrawFilledRectangle(Vector3 position, Vector2 size)
        {
            Gizmos.color = new Color(gapColor.r, gapColor.g, gapColor.b, 0.3f);
            Gizmos.DrawCube(position, new Vector3(size.x, size.y, 0.01f));
        }
    }
}
