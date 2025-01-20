#if Cinemachine
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y to a max co-ordinate
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("")] // Hide in menu
public class ClampCameraY : CinemachineExtension {
    [Tooltip("Clamp the camera's Y position to this value")]
    [NoSaveDuringPlay] public float PositionMinY = 0;
    [NoSaveDuringPlay] public float PositionMaxY = 0;

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

            state.RawPosition = new Vector3(
                pos.x,
                Mathf.Clamp(pos.y, PositionMinY, PositionMaxY),
                pos.z
            );
        }
    }
}
#endif
