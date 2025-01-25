using UnityEngine;
using CustomArchitecture;
using Sirenix.OdinInspector;

namespace Comic
{
    public abstract class AView : BaseBehaviour
    {
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Show() => gameObject.SetActive(true);
        public abstract void Init();
        public abstract void ActiveGraphic(bool active);
    }
}