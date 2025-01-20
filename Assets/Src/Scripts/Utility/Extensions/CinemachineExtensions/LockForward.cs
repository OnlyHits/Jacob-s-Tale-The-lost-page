#if Cinemachine
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y to a max co-ordinate
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("")] // Hide in menu
public class LockForward : CinemachineExtension {
    [Tooltip("")]
    [NoSaveDuringPlay] public Transform target;

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if (stage == CinemachineCore.Stage.Finalize) {
            var pos = state.RawPosition;

            state.RawOrientation.Set(target.rotation.x, target.rotation.y, target.rotation.z, target.rotation.w);
        }
    }
}
#endif
