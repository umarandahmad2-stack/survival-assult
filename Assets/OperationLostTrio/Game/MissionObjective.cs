using UnityEngine;
using UnityEngine.Events;

namespace OperationLostTrio
{
    public class MissionObjective : MonoBehaviour
    {
        [System.Serializable] public class ObjectiveEvent : UnityEvent<string> { }
        public string[] objectiveNames = { "Find the supply cache", "Rescue all three teammates", "Reach the extraction zone" };
        public ObjectiveEvent onObjectiveChanged;
        public int CurrentIndex { get; private set; }
        public bool IsComplete { get; private set; }

        void Start() { Announce(); }

        public void AdvanceObjective()
        {
            if (IsComplete) return;
            CurrentIndex++;
            if (CurrentIndex >= objectiveNames.Length)
            {
                IsComplete = true;
                onObjectiveChanged?.Invoke("MISSION COMPLETE — EXTRACTION SUCCESSFUL");
                Debug.Log("Mission complete!");
                return;
            }
            Announce();
        }

        void Announce()
        {
            string text = objectiveNames != null && objectiveNames.Length > 0 && CurrentIndex < objectiveNames.Length
                ? objectiveNames[CurrentIndex] : "Complete all objectives";
            onObjectiveChanged?.Invoke(text);
            Debug.Log("Objective: " + text);
        }

        public string CurrentText
        {
            get
            {
                if (IsComplete) return "MISSION COMPLETE";
                if (objectiveNames == null || objectiveNames.Length == 0 || CurrentIndex >= objectiveNames.Length) return "Complete all objectives";
                return objectiveNames[CurrentIndex];
            }
        }
    }
}