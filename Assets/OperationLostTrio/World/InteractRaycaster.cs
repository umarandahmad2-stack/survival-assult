using UnityEngine;
namespace OperationLostTrio {
    public class InteractRaycaster : MonoBehaviour {
        public Camera viewCamera; public float interactDistance = 3f; public LayerMask interactMask = ~0;
        void Update() {
            if (!Input.GetKeyDown(KeyCode.E) || viewCamera == null) return;
            if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, out RaycastHit hit, interactDistance, interactMask, QueryTriggerInteraction.Collide))
                hit.collider.GetComponentInParent<IInteractable>()?.Interact(gameObject);
        }
    }
    public interface IInteractable { void Interact(GameObject interactor); }
}