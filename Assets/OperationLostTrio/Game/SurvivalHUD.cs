using UnityEngine;
namespace OperationLostTrio {
    public class SurvivalHUD : MonoBehaviour {
        public Health playerHealth; public WeaponRaycast weapon; public OperationGameManager gameManager;
        void OnGUI() {
            GUILayout.BeginArea(new Rect(16, 16, 340, 150), GUI.skin.box); GUILayout.Label("OPERATION: LOST TRIO");
            if (playerHealth != null) GUILayout.Label("Health: " + Mathf.CeilToInt(playerHealth.Current));
            if (weapon != null) GUILayout.Label("Ammo: " + weapon.AmmoInMagazine + " / " + weapon.reserveAmmo);
            if (gameManager != null) GUILayout.Label("Teammates rescued: " + gameManager.RescuesCompleted + " / " + gameManager.requiredRescues);
            GUILayout.Label("WASD move | Shift sprint | Mouse aim | LMB fire | R reload | E interact"); GUILayout.EndArea();
        }
    }
}