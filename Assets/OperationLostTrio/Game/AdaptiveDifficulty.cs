using UnityEngine;

namespace OperationLostTrio
{
    /// <summary>Raises enemy toughness and damage over time for a harder survival loop.</summary>
    public class AdaptiveDifficulty : MonoBehaviour
    {
        public Health player;
        public EnemyPatrol[] enemies;
        public float rampEverySeconds = 90f;
        [Range(0f, 1f)] public float maxExtraDamage = 1f;
        public float maxExtraHealth = 100f;
        float elapsed;
        int stage;

        void Start() { ApplyStage(0); }

        void Update()
        {
            elapsed += Time.deltaTime;
            int nextStage = Mathf.FloorToInt(elapsed / Mathf.Max(10f, rampEverySeconds));
            if (nextStage > stage) { stage = nextStage; ApplyStage(stage); }
        }

        void ApplyStage(int level)
        {
            float t = Mathf.Clamp01(level / 6f);
            foreach (EnemyPatrol enemy in enemies)
            {
                if (enemy == null) continue;
                enemy.attackDamage = 10f * (1f + maxExtraDamage * t);
                Health health = enemy.GetComponent<Health>();
                if (health != null) health.maxHealth = 100f + maxExtraHealth * t;
                enemy.detectionRange = 18f + 8f * t;
            }
            Debug.Log("Survival difficulty stage: " + level);
        }
    }
}