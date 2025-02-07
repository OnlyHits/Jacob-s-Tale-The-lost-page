// using UnityEngine;

// [ExecuteInEditMode]
// public class SpriteOutline : MonoBehaviour {
//     public Color color = Color.white;

//     private SpriteRenderer spriteRenderer;

//     void OnEnable() {
//         spriteRenderer = GetComponent<SpriteRenderer>();

//         UpdateOutline(true);
//     }

//     void OnDisable() {
//         UpdateOutline(false);
//     }

//     void Update() {
//         Debug.Log("ntm");
//         UpdateOutline(true);
//     }

//     void UpdateOutline(bool outline) {
//         MaterialPropertyBlock mpb = new MaterialPropertyBlock();
//         spriteRenderer.GetPropertyBlock(mpb);
//         mpb.SetFloat("_Outline", 1f);
//         mpb.SetColor("_OutlineColor", color);
//         spriteRenderer.SetPropertyBlock(mpb);
//     }
// }