using System;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class HudManager : BaseBehaviour
    {
        private ViewManager m_viewManager;
        private HudCameraRegister m_cameras;

        public ViewManager GetViewManager() => m_viewManager;
        public HudCameraRegister GetRegisteredCameras() => m_cameras;

        public void Init()
        {
            m_viewManager = gameObject.GetComponent<ViewManager>();
            m_cameras = gameObject.GetComponent<HudCameraRegister>();

            m_viewManager.Init();
        }
    }
}