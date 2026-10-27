using UnityEngine;
using UnityEngine.Events;
namespace OperationLostTrio {
    public class Health : MonoBehaviour {
        public float maxHealth = 100f; public float Current { get; private set; } public bool IsDead => Current <= 0f; public UnityEvent onDeath;
        void Awake() { Current = maxHealth; }
        public void Damage(float amount) { if (IsDead || amount <= 0f) return; Current = Mathf.Max(0f, Current - amount); if (IsDead) onDeath?.Invoke(); }
        public void Heal(float amount) { if (!IsDead && amount > 0f) Current = Mathf.Min(maxHealth, Current + amount); }
    }
}