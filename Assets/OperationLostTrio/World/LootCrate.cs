using UnityEngine;
namespace OperationLostTrio {
    public class LootCrate : MonoBehaviour, IInteractable {
        public int ammoAmount = 18; public float healAmount = 25f; public bool containsMedkit = true; bool looted;
        public void Interact(GameObject interactor) {
            if (looted) return;
            WeaponRaycast weapon = interactor.GetComponentInChildren<WeaponRaycast>(); if (weapon != null) weapon.AddAmmo(ammoAmount);
            Health health = interactor.GetComponent<Health>(); if (containsMedkit && health != null) health.Heal(healAmount);
            looted = true; gameObject.SetActive(false);
        }
    }
}