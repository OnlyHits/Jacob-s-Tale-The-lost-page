using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using CustomArchitecture;
using UnityEngine.Rendering;

namespace Comic
{
    public class GameCameraRegister : ACameraRegister
    {
        public SpriteRenderer m_screenshotSpr;

        public override Camera GetCameraForScreenshot()
        {
            if (m_cameras.Count > 0 && m_cameras[0] != null)
                return m_cameras[0];
            
            return null;
        }

        public override Vector3? GetScreenshotMin(Camera base_camera)
        {
            if (m_screenshotSpr == null)
            {
                Debug.LogError("Missing required components for cropping!");
                return null;
            }

            Bounds sprite_bounds = m_screenshotSpr.bounds;

            return base_camera.WorldToScreenPoint(new Vector3(sprite_bounds.min.x, sprite_bounds.min.y, sprite_bounds.center.z));
        }

        public override Vector3? GetScreenshotMax(Camera base_camera)
        {
            if (m_screenshotSpr == null)
            {
                Debug.LogError("Missing required components for cropping!");
                return null;
            }

            Bounds sprite_bounds = m_screenshotSpr.bounds;

            return base_camera.WorldToScreenPoint(new Vector3(sprite_bounds.max.x, sprite_bounds.max.y, sprite_bounds.center.z));
        }
    }
}
