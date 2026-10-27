using UnityEngine;
using UnityEngine.Events;
namespace OperationLostTrio {
    public class OperationGameManager : MonoBehaviour {
        public int requiredRescues = 3; public int RescuesCompleted { get; private set; } public UnityEvent onMissionComplete; bool complete;
        public void RegisterRescue() { if (complete) return; RescuesCompleted++; if (RescuesCompleted >= requiredRescues) { complete = true; onMissionComplete?.Invoke(); Debug.Log("Operation complete: all teammates rescued."); } }
    }
}