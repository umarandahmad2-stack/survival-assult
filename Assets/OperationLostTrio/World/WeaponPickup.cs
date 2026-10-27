using UnityEngine;

namespace OperationLostTrio
{
    /// <summary>Place over a pickup prop with a collider; interaction uses E.</summary>
    public class WeaponPickup : MonoBehaviour, IInteractable
    {
        public int ammoAmount = 24;
        public string pickupLabel = "Ammo cache";
        public bool refillMagazine = false;
        bool collected;

        public void Interact(GameObject interactor)
        {
            if (collected) return;
            WeaponRaycast weapon = interactor.GetComponentInChildren<WeaponRaycast>();
            if (weapon == null) return;
            weapon.AddAmmo(ammoAmount);
            if (refillMagazine) weapon.ReloadNow();
            collected = true;
            Debug.Log("Picked up " + pickupLabel + " (+" + ammoAmount + " ammo)");
            Destroy(gameObject);
        }
    }
}