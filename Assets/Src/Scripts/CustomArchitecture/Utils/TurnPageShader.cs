using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CustomArchitecture
{
    [RequireComponent(typeof(Image))]
    public class TurnPageShader : BaseBehaviour
    {
        public Material pageFlipMaterial; // Assign this in the Inspector
        public float flipSpeed = 2.0f;
        private float flipAmount = 0.0f;

        protected override void OnUpdate(float elapsed_time)
        {
            if (Input.GetKey(KeyCode.Space)) // Hold Space to flip the page
            {
                flipAmount = Mathf.PingPong(Time.time * flipSpeed, 1);
                pageFlipMaterial.SetFloat("_FlipAmount", flipAmount);
            }
        }
    }
}