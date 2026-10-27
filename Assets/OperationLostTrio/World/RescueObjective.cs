using UnityEngine;
namespace OperationLostTrio {
    public class RescueObjective : MonoBehaviour, IInteractable {
        public OperationGameManager gameManager; bool rescued;
        public void Interact(GameObject interactor) { if (rescued) return; rescued = true; if (gameManager != null) gameManager.RegisterRescue(); gameObject.SetActive(false); }
    }
}