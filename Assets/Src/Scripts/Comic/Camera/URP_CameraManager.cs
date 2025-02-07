using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using CustomArchitecture;

namespace Comic
{
    public enum URP_OverlayCameraType
    {
        Camera_Game,
        Camera_Hud,
        Camera_Hud_TurnPage,
    }

    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(UniversalAdditionalCameraData))]
    public class URP_CameraManager : MonoBehaviour
    {
        private Dictionary<URP_OverlayCameraType, ACameraRegister> m_overlayCameras;
        private Camera m_baseCamera;

        private Action<bool, Sprite> m_onScreenshotSprite;

        [SerializeField] private URP_OverlayCameraType m_screenType;
        [SerializeField] private SpriteRenderer m_leftScreenshot;
        [SerializeField] private SpriteRenderer m_rightScreenshot;

        public Bounds GetScreenshotBounds() => m_leftScreenshot.bounds;
        public bool IsCameraRegister(URP_OverlayCameraType type) => m_overlayCameras != null && m_overlayCameras.ContainsKey(type);

        public void ClearCameraStack()
        {
            UniversalAdditionalCameraData baseCameraData = m_baseCamera.GetComponent<UniversalAdditionalCameraData>();
            if (baseCameraData == null)
            {
                Debug.LogError("Base Camera is missing UniversalAdditionalCameraData component!");
                return;
            }

            baseCameraData.cameraStack.Clear();
        }

        public void Init()
        {
            m_overlayCameras = new();
            m_onScreenshotSprite += OnScreenshotSprite;
            m_baseCamera = GetComponent<Camera>();

            // important for hud instantiation 
            // ((HudCameraRegister)m_overlayCameras[URP_OverlayCameraType.Camera_Hud]).Init(m_baseCamera, GetScreenshotBounds());
        }

        public void RegisterCameras(GameCameraRegister game_camera)
        {
            if (game_camera != null)
            {
                if (m_overlayCameras.ContainsKey(URP_OverlayCameraType.Camera_Game))
                {
                    Debug.Log("Game cameras are already registered");
                    return;
                }
                else
                {
                    m_overlayCameras.Add(URP_OverlayCameraType.Camera_Game, game_camera);
                }
            }
            else
            {
                Debug.LogWarning("Game camera data is null");
            }
            
            UniversalAdditionalCameraData base_camera_data = m_baseCamera.GetComponent<UniversalAdditionalCameraData>();
            
            if (base_camera_data == null)
            {
                Debug.LogError("Base Camera is missing UniversalAdditionalCameraData component!");
                return;
            }

            foreach (var camera in m_overlayCameras[URP_OverlayCameraType.Camera_Game].GetCameras())
            {
                UniversalAdditionalCameraData camera_data = camera.GetComponent<UniversalAdditionalCameraData>();
                if (camera_data != null)
                {
                    camera_data.renderType = CameraRenderType.Overlay;
                    base_camera_data.cameraStack.Add(camera);
                }
            }
        }

        public void RegisterCameras(HudCameraRegister hud_camera)
        {
            if (hud_camera != null)
            {
                if (m_overlayCameras.ContainsKey(URP_OverlayCameraType.Camera_Hud))
                {
                    Debug.Log("Hud cameras are already registered");
                    return;
                }
                else
                {
                    m_overlayCameras.Add(URP_OverlayCameraType.Camera_Hud, hud_camera);
                }
            }
            else
            {
                Debug.LogWarning("Hud camera data is null");
            }
            
            UniversalAdditionalCameraData base_camera_data = m_baseCamera.GetComponent<UniversalAdditionalCameraData>();
            
            if (base_camera_data == null)
            {
                Debug.LogError("Base Camera is missing UniversalAdditionalCameraData component!");
                return;
            }

            foreach (var camera in m_overlayCameras[URP_OverlayCameraType.Camera_Hud].GetCameras())
            {
                UniversalAdditionalCameraData camera_data = camera.GetComponent<UniversalAdditionalCameraData>();
                if (camera_data != null)
                {
                    camera_data.renderType = CameraRenderType.Overlay;
                    base_camera_data.cameraStack.Add(camera);
                }
            }
        }

        private List<bool> UnactiveAllCameras(Camera ignore_camera)
        {
            List<bool> register_actives = new();

            foreach (var registered_camera in m_overlayCameras)
            {
                foreach (var camera in registered_camera.Value.GetCameras())
                {
                    register_actives.Add(camera.gameObject.activeSelf);

                    camera.gameObject.SetActive(camera == ignore_camera);
                }
            }

            return register_actives;
        }

        private void RestoreActiveCameras(List<bool> register_actives)
        {
            int i = 0;

            foreach (var registered_camera in m_overlayCameras)
            {
                foreach (var camera in registered_camera.Value.GetCameras())
                {
                    camera.gameObject.SetActive(register_actives[i]);
                    ++i;
                }
            }
        }

        private void OnScreenshotSprite(bool front, Sprite sprite)
        {
            if (front)
            {
                ((HudCameraRegister)m_overlayCameras[URP_OverlayCameraType.Camera_Hud]).SetFrontSprite(sprite);
            }
            else
            {
                ((HudCameraRegister)m_overlayCameras[URP_OverlayCameraType.Camera_Hud]).SetBackSprite(sprite);
            }
        }

        // public IEnumerator ScreenGameAndSetupTurningPage(bool previous)
        // {
        //     if (!m_overlayCameras.ContainsKey(URP_OverlayCameraType.Camera_Hud)
        //         || !m_overlayCameras.ContainsKey(URP_OverlayCameraType.Camera_Game))
        //     {
        //         Debug.LogWarning("Camera is not setup");
        //         yield break;
        //     }

        //     yield return new WaitForEndOfFrame();

        //     yield return StartCoroutine(CaptureURPScreenshot(URP_OverlayCameraType.Camera_Hud));
        //     yield return StartCoroutine(CaptureURPScreenshot(URP_OverlayCameraType.Camera_Game));

        //     ((HudCameraRegister)m_overlayCameras[URP_OverlayCameraType.Camera_Hud]).TurnPage(previous);
        // }

        public IEnumerator ScreenAndApplyTexture()
        {
            if (!m_overlayCameras.ContainsKey(URP_OverlayCameraType.Camera_Hud))
                yield break;

            yield return new WaitForEndOfFrame();

            CaptureAllCameraURPScreenshot(m_leftScreenshot, false);
            CaptureAllCameraURPScreenshot(m_rightScreenshot, true);

            ((HudCameraRegister)m_overlayCameras[URP_OverlayCameraType.Camera_Hud]).TurnPage(false);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                StartCoroutine(ScreenAndApplyTexture());
            }
        }

        #region Screenshot

        private void SaveTextureAsPNG(Texture2D texture, string fileName)
        {
            if (texture == null)
            {
                Debug.LogError("Texture is null, cannot save.");
                return;
            }

            byte[] bytes = texture.EncodeToPNG();
            string path = Path.Combine(Application.streamingAssetsPath, fileName);
            File.WriteAllBytes(path, bytes);
        }

        private Texture2D CropTexture(Texture2D source_texture, Vector3 screen_min, Vector3 screen_max)
        {
            if (source_texture == null || m_baseCamera == null)
            {
                Debug.LogError("Missing required components for cropping!");
                return null;
            }

            // Convert screen space to texture coordinates with better accuracy
            int x = Mathf.RoundToInt(screen_min.x * source_texture.width / Screen.width);
            int y = Mathf.RoundToInt(screen_min.y * source_texture.height / Screen.height);
            int width = Mathf.RoundToInt((screen_max.x - screen_min.x) * source_texture.width / Screen.width);
            int height = Mathf.RoundToInt((screen_max.y - screen_min.y) * source_texture.height / Screen.height);

            // Ensure cropping dimensions are within valid range
            x = Mathf.Clamp(x, 0, source_texture.width);
            y = Mathf.Clamp(y, 0, source_texture.height);
            width = Mathf.Clamp(width, 1, source_texture.width - x);
            height = Mathf.Clamp(height, 1, source_texture.height - y);

            // Create cropped texture with the same format as the source
            Texture2D cropped_texture = new Texture2D(width, height, source_texture.format, false);
            cropped_texture.SetPixels(source_texture.GetPixels(x, y, width, height));
//            cropped_texture.Apply(); // Apply changes to the texture

            return cropped_texture;
        }

        private Texture2D PrepareTextureForSprite(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogError("Texture is NULL!");
                return null;
            }

            texture.Apply(false, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.anisoLevel = 1;

            Texture2D processedTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            processedTexture.SetPixels(texture.GetPixels());
            processedTexture.Apply(false, false); 

            return processedTexture;
        }

        private Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null)
                return null;

            texture = PrepareTextureForSprite(texture);

            Rect spriteRect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, spriteRect, new Vector2(0.5f, 0.5f));
        }

        public void CaptureAllCameraURPScreenshot(SpriteRenderer sprite, bool front)
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            rt.antiAliasing = 8;
            rt.useMipMap = true;

            RenderTexture original_rt = m_baseCamera.targetTexture;

            foreach (var registered_camera in m_overlayCameras)
            {
                foreach (var camera in registered_camera.Value.GetCameras())
                {
                    if (camera.gameObject.activeSelf)
                    {
                        camera.targetTexture = rt;
                        camera.Render();
                    }
                }
            }

            m_baseCamera.targetTexture = rt;
            m_baseCamera.Render();

            RenderTexture.active = rt;
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply(updateMipmaps: false, makeNoLongerReadable: false);

            Vector3? min = GetScreenshotMin(sprite);
            Vector3? max = GetScreenshotMax(sprite);

            Texture2D cropped_texture = null;

            if (min != null && max != null)
            {
                cropped_texture = CropTexture(screenshot, min.Value, max.Value);

                m_onScreenshotSprite?.Invoke(front, ConvertTextureToSprite(cropped_texture));

#if UNITY_EDITOR
                SaveTextureAsPNG(cropped_texture, "Tests/screenshot.png");
#endif
                Destroy(cropped_texture);
                Destroy(screenshot);
            }
            else
            {
                m_onScreenshotSprite?.Invoke(front, ConvertTextureToSprite(screenshot));

                Destroy(screenshot);
            }

            RenderTexture.active = null;
            m_baseCamera.targetTexture = null;
        }

        public IEnumerator CaptureURPScreenshot(URP_OverlayCameraType camera_type)
        {
            Camera selected_camera = m_overlayCameras[camera_type].GetCameraForScreenshot();
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            rt.antiAliasing = 8;
            rt.useMipMap = true;

            RenderTexture original_rt = selected_camera.targetTexture;

            List<bool> camera_active_states = UnactiveAllCameras(selected_camera);

            selected_camera.targetTexture = rt;
            selected_camera.Render();

            m_baseCamera.targetTexture = rt;
            m_baseCamera.Render();

            RenderTexture.active = rt;
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply(updateMipmaps: false, makeNoLongerReadable: false);

            Vector3? min = m_overlayCameras[camera_type].GetScreenshotMin(m_baseCamera);
            Vector3? max = m_overlayCameras[camera_type].GetScreenshotMax(m_baseCamera);

            Texture2D cropped_texture = null;

            if (min != null && max != null)
            {
                cropped_texture = CropTexture(screenshot, min.Value, max.Value);

//                m_onScreenshotSprite?.Invoke(camera_type, ConvertTextureToSprite(cropped_texture));

#if UNITY_EDITOR
                SaveTextureAsPNG(cropped_texture, "Tests/screenshot.png");
#endif
                Destroy(cropped_texture);
                Destroy(screenshot);
            }
            else
            {
//                m_onScreenshotSprite?.Invoke(camera_type, ConvertTextureToSprite(screenshot));

                Destroy(screenshot);
            }

            selected_camera.targetTexture = original_rt;
            RenderTexture.active = null;

            RestoreActiveCameras(camera_active_states);

            m_baseCamera.targetTexture = null;

            yield return null;
        }

        #endregion

        #region ScreenshotUtils

        public Vector3? GetScreenshotMin(SpriteRenderer sprite)
        {
            if (sprite == null)
            {
                Debug.LogError("Missing required components for cropping!");
                return null;
            }

            Bounds sprite_bounds = sprite.bounds;

            return m_baseCamera.WorldToScreenPoint(new Vector3(sprite_bounds.min.x, sprite_bounds.min.y, sprite_bounds.center.z));
        }

        public Vector3? GetScreenshotMax(SpriteRenderer sprite)
        {
            if (sprite == null)
            {
                Debug.LogError("Missing required components for cropping!");
                return null;
            }

            Bounds sprite_bounds = sprite.bounds;

            return m_baseCamera.WorldToScreenPoint(new Vector3(sprite_bounds.max.x, sprite_bounds.max.y, sprite_bounds.center.z));
        }

        #endregion
    }
}
