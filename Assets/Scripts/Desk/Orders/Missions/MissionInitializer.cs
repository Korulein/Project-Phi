using UnityEngine;

public class MissionInitializer : MonoBehaviour
{
    public OrderScreenUI trainingPanelOrderScreenUI;
    public OrderScreenUI marsPanelOrderScreenUI;
    public OrderScreenUI moonPanelOrderScreenUI;
    // Copy previous and change name for new mission

    public Missions trainingMission;
    public Missions marsMission;
    public Missions moonMission;
    // Copy previous and change name for new mission

    private void Start()
    {
        if (trainingMission == null) Debug.LogError("trainingMission is NULL");
        if (marsMission == null) Debug.LogError("marsMission is NULL");
        if (moonMission == null) Debug.LogError("marsMission is NULL");
        if (trainingPanelOrderScreenUI == null) Debug.LogError("trainingPanelOrderScreenUI is NULL");
        if (marsPanelOrderScreenUI == null) Debug.LogError("marsPanelOrderScreenUI is NULL");
        if (moonPanelOrderScreenUI == null) Debug.LogError("marsPanelOrderScreenUI is NULL");

        trainingPanelOrderScreenUI.SetMission(trainingMission);
        marsPanelOrderScreenUI.SetMission(marsMission);
        moonPanelOrderScreenUI.SetMission(moonMission);
    }
}