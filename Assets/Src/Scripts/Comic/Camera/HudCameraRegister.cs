using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using CustomArchitecture;
using UnityEngine.Rendering;
using System;

namespace Comic
{
    public class HudCameraRegister : ACameraRegister
    {
        [SerializeField] private RectTransform m_screenshotRect;
        [SerializeField] private TurningPage m_turningPage;

        public void SetFrontSprite(Sprite sprite) => m_turningPage.SetFrontSprite(sprite);
        public void SetBackSprite(Sprite sprite) => m_turningPage.SetBackSprite(sprite);
        public void RegisterToEndTurning(Action function) => m_turningPage.RegisterToEndTurning(function);

        public void Init(Camera world_camera, Bounds sprite_bounds)
        {
            Vector3 minWorld = sprite_bounds.min;
            Vector3 maxWorld = sprite_bounds.max;

            if (m_cameras.Count > 1)
                m_turningPage.MatchBounds(m_cameras[1],
                    world_camera.WorldToScreenPoint(minWorld),
                    world_camera.WorldToScreenPoint(maxWorld));
        }

        public void TurnPage(bool previous)
        {
            if (previous)
                m_turningPage.PreviousPage();
            else
                m_turningPage.NextPage();
        }

        public override Camera GetCameraForScreenshot()
        {
            if (m_cameras.Count > 0 && m_cameras[0] != null)
                return m_cameras[0];
            
            return null;
        }

        public override Vector3? GetScreenshotMin(Camera base_camera)
        {
            if (m_screenshotRect != null)
            {
                Vector3[] world_corners = new Vector3[4];
                m_screenshotRect.GetWorldCorners(world_corners);

                return base_camera.WorldToScreenPoint(world_corners[0]);
            }
            else
            {
                Debug.LogError("Rect transform for screenshot is null");
                return null;
            }
        }

        public override Vector3? GetScreenshotMax(Camera base_camera)
        {
            if (m_screenshotRect != null)
            {
                // Convert RectTransform to screen space
                Vector3[] world_corners = new Vector3[4];
                m_screenshotRect.GetWorldCorners(world_corners);


                return base_camera.WorldToScreenPoint(world_corners[2]); // Top-right
            }
            else
            {
                Debug.LogError("Rect transform for screenshot is null");
                return null;
            }
        } 
    }
}
