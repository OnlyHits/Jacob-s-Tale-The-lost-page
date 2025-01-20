using UnityEngine;

public static class SpriteUtils {

    public static Sprite TextureToSprite(Texture2D texture) {
        if (texture == null) {
            Debug.LogWarning("Could not create sprite from texture because texture was null in SpriteUtils.cs");
            return null;
        }
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
