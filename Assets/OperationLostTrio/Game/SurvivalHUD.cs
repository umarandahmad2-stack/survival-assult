using UnityEngine;

namespace OperationLostTrio
{
    /// <summary>Clean, high-contrast tactical HUD. Uses IMGUI so no Canvas prefab is required.</summary>
    public class SurvivalHUD : MonoBehaviour
    {
        public Health playerHealth;
        public WeaponRaycast weapon;
        public OperationGameManager gameManager;
        public EnemyWaveSpawner waveSpawner;
        public MissionObjective missionObjective;

        GUIStyle panel, title, label, large, small;
        Texture2D whitePixel;

        void Awake()
        {
            whitePixel = new Texture2D(1, 1);
            whitePixel.SetPixel(0, 0, Color.white);
            whitePixel.Apply();
            panel = new GUIStyle(GUI.skin.box);
            panel.normal.background = whitePixel;
            panel.padding = new RectOffset(14, 14, 10, 10);
            title = new GUIStyle(GUI.skin.label) { fontSize = 18, fontStyle = FontStyle.Bold, normal = { textColor = new Color(1f, 0.48f, 0.12f) } };
            label = new GUIStyle(GUI.skin.label) { fontSize = 14, normal = { textColor = Color.white } };
            large = new GUIStyle(GUI.skin.label) { fontSize = 25, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleRight, normal = { textColor = Color.white } };
            small = new GUIStyle(GUI.skin.label) { fontSize = 11, normal = { textColor = new Color(0.78f, 0.83f, 0.84f) } };
        }

        void OnGUI()
        {
            Color old = GUI.color;
            GUI.color = new Color(0.035f, 0.055f, 0.065f, 0.88f);
            GUI.Box(new Rect(18, 18, 300, 144), GUIContent.none, panel);
            GUI.color = Color.white;
            GUI.Label(new Rect(32, 27, 270, 26), "OPERATION // LOST TRIO", title);
            GUI.Label(new Rect(32, 58, 120, 22), "VITALS", small);
            float hp = playerHealth != null ? playerHealth.Current : 0f;
            float maxHp = playerHealth != null ? Mathf.Max(1f, playerHealth.maxHealth) : 100f;
            DrawBar(new Rect(32, 82, 264, 13), hp / maxHp, hp <= 25f ? new Color(0.95f,0.18f,0.1f) : new Color(0.2f,0.82f,0.42f));
            GUI.Label(new Rect(32, 100, 130, 28), "HP " + Mathf.CeilToInt(hp), large);
            if (gameManager != null)
                GUI.Label(new Rect(174, 104, 122, 22), "RESCUE " + gameManager.RescuesCompleted + "/" + gameManager.requiredRescues, label);

            GUI.color = new Color(0.035f, 0.055f, 0.065f, 0.88f);
            GUI.Box(new Rect(Screen.width - 250, 18, 232, 112), GUIContent.none, panel);
            GUI.color = Color.white;
            GUI.Label(new Rect(Screen.width - 232, 27, 200, 24), "WEAPON STATUS", title);
            if (weapon != null)
            {
                GUI.Label(new Rect(Screen.width - 232, 55, 125, 40), weapon.AmmoInMagazine.ToString("00"), large);
                GUI.Label(new Rect(Screen.width - 125, 67, 95, 22), "/ " + weapon.reserveAmmo + " RES", label);
            }
            if (waveSpawner != null)
                GUI.Label(new Rect(Screen.width - 232, 92, 200, 22), "HOSTILE WAVE " + waveSpawner.CurrentWave, label);

            if (missionObjective != null)
            {
                GUI.color = new Color(0.035f, 0.055f, 0.065f, 0.82f);
                GUI.Box(new Rect(18, Screen.height - 84, 420, 64), GUIContent.none, panel);
                GUI.color = Color.white;
                GUI.Label(new Rect(32, Screen.height - 75, 390, 20), "CURRENT OBJECTIVE", small);
                GUI.Label(new Rect(32, Screen.height - 54, 390, 25), missionObjective.CurrentText, label);
            }

            GUI.color = new Color(0.035f, 0.055f, 0.065f, 0.78f);
            GUI.Box(new Rect(Screen.width / 2f - 95, Screen.height - 36, 190, 22), GUIContent.none, panel);
            GUI.color = Color.white;
            GUI.Label(new Rect(Screen.width / 2f - 88, Screen.height - 34, 180, 18), "WASD MOVE   •   E INTERACT   •   R RELOAD", small);
            GUI.color = old;
        }

        void DrawBar(Rect rect, float value, Color fill)
        {
            GUI.color = new Color(0.15f, 0.18f, 0.19f, 1f);
            GUI.DrawTexture(rect, whitePixel);
            GUI.color = fill;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * Mathf.Clamp01(value), rect.height), whitePixel);
            GUI.color = Color.white;
        }

        void OnDestroy() { if (whitePixel != null) Destroy(whitePixel); }
    }
}