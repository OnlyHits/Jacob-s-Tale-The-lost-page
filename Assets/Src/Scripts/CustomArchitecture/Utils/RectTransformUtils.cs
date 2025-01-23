using UnityEngine;

namespace CustomArchitecture
{
    public static class RectTransformUtils
    {
        public static Vector3 GetPivotInWorldSpace(this RectTransform source)
        {
            // Rewrite Rect.NormalizedToPoint without any clamping.
            Vector2 pivot = new Vector2(
                source.rect.xMin + source.pivot.x * source.rect.width,
                source.rect.yMin + source.pivot.y * source.rect.height);
            // Apply scaling and rotations.
            return source.TransformPoint(new Vector3(pivot.x, pivot.y, 0f));
        }

        // Set the RectTransform's pivot point in world coordinates, without moving the position.
        // This is like dragging the pivot handle in the editor.
        //
        public static void SetPivotInWorldSpace(this RectTransform source, Vector3 pivot)
        {
            // Strip scaling and rotations.
            pivot = source.InverseTransformPoint(pivot);
            Vector2 pivot2 = new Vector2(
                (pivot.x - source.rect.xMin) / source.rect.width,
                (pivot.y - source.rect.yMin) / source.rect.height);

            // Now move the pivot, keeping and restoring the position which is based on it.
            Vector2 offset = pivot2 - source.pivot;
            offset.Scale(source.rect.size);
            Vector3 worldPos = source.position + source.TransformVector(offset);
            source.pivot = pivot2;
            source.position = worldPos;
        }
    }
}