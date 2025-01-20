using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
public class RecordTransform : MonoBehaviour {
#if UNITY_EDITOR
    public AnimationClip clip;
    private GameObjectRecorder m_Recorder;
    private void Start() {
        // Create recorder and record the script GameObject.
        this.m_Recorder = new GameObjectRecorder(this.gameObject);
        // Bind all the Transforms on the GameObject and all its children.
        this.m_Recorder.BindComponentsOfType<Transform>(this.gameObject, true);
    }

    private void FixedUpdate() {
        if (this.clip == null) {
            return;
        }
        // Take a snapshot and record all the bindings values for this frame.
        this.m_Recorder.TakeSnapshot(Time.fixedDeltaTime);
    }
    
    private void OnDisable() {
        if (this.clip == null) {
            return;
        }
        if (this.m_Recorder.isRecording) {
            // Save the recorded session to the clip.
            this.m_Recorder.SaveToClip(this.clip);
        }
    }
#endif
}
