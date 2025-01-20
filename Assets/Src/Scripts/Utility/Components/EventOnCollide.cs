using System;
using UnityEngine;

public class EventOnCollide : MonoBehaviour {

    public Action<Collision> onCollisionEnter;
    public Action<Collision> onCollisionExit;

    public Action<Collider> onTriggerEnter;
    public Action<Collider> onTriggerExit;

    [SerializeField] private LayerMask _layerToCollide;

    private void Awake() {
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision?.gameObject == null) {
            return;
        }
        if (LayerUtils.IsLayerInLayerMask(collision.gameObject.layer, _layerToCollide)) {
            onCollisionEnter?.Invoke(collision);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision?.gameObject == null) {
            return;
        }
        if (LayerUtils.IsLayerInLayerMask(collision.gameObject.layer, _layerToCollide)) {
            onCollisionEnter?.Invoke(collision);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider?.gameObject == null) {
            return;
        }
        if (LayerUtils.IsLayerInLayerMask(collider.gameObject.layer, _layerToCollide)) {
            onTriggerEnter?.Invoke(collider);
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider?.gameObject == null) {
            return;
        }
        if (LayerUtils.IsLayerInLayerMask(collider.gameObject.layer, _layerToCollide)) {
            onTriggerExit?.Invoke(collider);
        }
    }

}

