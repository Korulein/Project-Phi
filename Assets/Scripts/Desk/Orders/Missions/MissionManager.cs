using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance { get; private set; }

    [System.Serializable]
    public class Mission
    {
        public string missionName;
        public GameObject blueprint; // Blueprint GameObject voor deze missie
    }

    [Header("Missions")]
    [SerializeField] private List<Mission> missions = new List<Mission>();

    private Mission currentMission;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMission(int missionIndex)
    {
        if (missionIndex < 0 || missionIndex >= missions.Count)
        {
            Debug.LogError("Invalid mission index.");
            return;
        }

        foreach (var mission in missions)
        {
            if (mission.blueprint != null)
                mission.blueprint.SetActive(false);
        }

        currentMission = missions[missionIndex];
        if (currentMission.blueprint != null)
        {
            currentMission.blueprint.SetActive(true);
            Debug.Log($"Mission '{currentMission.missionName}' started. Blueprint is now visible.");
        }
    }
}