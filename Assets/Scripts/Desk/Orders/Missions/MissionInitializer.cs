using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MissionUIEntry
{
    public Missions mission;
    public OrderScreenUI orderScreenUI;
}

public class MissionInitializer : MonoBehaviour
{
    public List<MissionUIEntry> missionUIEntries = new List<MissionUIEntry>();

    private void Start()
    {
        foreach (var entry in missionUIEntries)
        {
            if (entry.mission == null)
                Debug.LogError("Mission is NULL in one of the entries");

            if (entry.orderScreenUI == null)
                Debug.LogError("OrderScreenUI is NULL in one of the entries");

            if (entry.mission != null && entry.orderScreenUI != null)
                entry.orderScreenUI.SetMission(entry.mission);
        }
    }
}