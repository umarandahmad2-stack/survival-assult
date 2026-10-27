using UnityEngine;
using UnityEngine.AI;
namespace OperationLostTrio {
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPatrol : MonoBehaviour {
        public Transform target; public float detectionRange = 18f, attackRange = 1.8f, attackDamage = 10f, attackCooldown = 1.2f;
        NavMeshAgent agent; float nextAttack;
        void Awake() { agent = GetComponent<NavMeshAgent>(); }
        void Update() {
            if (target == null) return; float distance = Vector3.Distance(transform.position, target.position);
            if (distance > detectionRange) { agent.isStopped = true; return; }
            agent.isStopped = false; agent.SetDestination(target.position);
            if (distance <= attackRange && Time.time >= nextAttack) { nextAttack = Time.time + attackCooldown; Health health = target.GetComponent<Health>(); if (health != null) health.Damage(attackDamage); }
        }
    }
}