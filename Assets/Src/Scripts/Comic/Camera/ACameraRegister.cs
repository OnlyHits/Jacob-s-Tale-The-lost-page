using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using CustomArchitecture;
using UnityEngine.Rendering;

namespace Comic
{
    public abstract class ACameraRegister : BaseBehaviour
    {
        public List<Camera> m_cameras;

        public List<Camera> GetCameras() => m_cameras;

        public abstract Camera GetCameraForScreenshot();
        public abstract Vector3? GetScreenshotMin(Camera base_camera);
        public abstract Vector3? GetScreenshotMax(Camera base_camera);
    }
}
