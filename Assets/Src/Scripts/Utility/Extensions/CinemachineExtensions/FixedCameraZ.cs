#if Cinemachine
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's X co-ordinate
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("")] // Hide in menu
public class FixedCameraZ : CinemachineExtension {
    [Tooltip("Fix the camera's Z position to this value")]
    [NoSaveDuringPlay] public float PositionZ = 12;

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
            state.RawPosition = new Vector3(pos.x, pos.y, PositionZ);
        }
    }
}
#endif
