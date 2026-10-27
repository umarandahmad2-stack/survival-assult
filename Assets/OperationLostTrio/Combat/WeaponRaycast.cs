using System.Collections;
using UnityEngine;
namespace OperationLostTrio {
    public class WeaponRaycast : MonoBehaviour {
        public Camera aimCamera; public float damage = 25f, range = 100f, fireRate = 8f; public int magazineSize = 12, reserveAmmo = 60; public float reloadSeconds = 1.4f; public LayerMask hitMask = ~0;
        public int AmmoInMagazine { get; private set; } float nextShotTime; bool reloading;
        void Awake() { AmmoInMagazine = magazineSize; }
        void Update() {
            if (Input.GetKeyDown(KeyCode.R)) ReloadNow();
            if (Input.GetMouseButton(0) && Time.time >= nextShotTime && !reloading) { nextShotTime = Time.time + 1f / Mathf.Max(0.1f, fireRate); Shoot(); }
        }
        void Shoot() {
            if (AmmoInMagazine <= 0 || aimCamera == null) return; AmmoInMagazine--;
            if (Physics.Raycast(aimCamera.transform.position, aimCamera.transform.forward, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore)) {
                Health target = hit.collider.GetComponentInParent<Health>(); if (target != null && target.gameObject != gameObject) target.Damage(damage);
            }
        }
        public void ReloadNow() { if (!reloading) StartCoroutine(Reload()); }
        IEnumerator Reload() {
            if (AmmoInMagazine == magazineSize || reserveAmmo <= 0) yield break; reloading = true; yield return new WaitForSeconds(reloadSeconds);
            int loaded = Mathf.Min(magazineSize - AmmoInMagazine, reserveAmmo); AmmoInMagazine += loaded; reserveAmmo -= loaded; reloading = false;
        }
        public void AddAmmo(int amount) { reserveAmmo += Mathf.Max(0, amount); }
    }
}