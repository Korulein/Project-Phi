using UnityEngine;
using static MissionManager;

public class MissionSelectButton : MonoBehaviour
{
    private Missions mission;
    public OrderScreenUI targetOrderScreenUI;

    public void Setup(Missions newMission)
    {
        mission = newMission;
    }

    public void StartMission()
    {
        if (BlueprintManager.instance.isMissionActive)
        {
            return;
        }

        if (targetOrderScreenUI == null)
        {
            return;
        }

        targetOrderScreenUI.SetMission(mission);
        targetOrderScreenUI.gameObject.SetActive(true);
        targetOrderScreenUI.CheckRequirements();

        BlueprintManager.instance.isMissionActive = true;
        BlueprintManager.instance.activeMission = mission;
        BlueprintManager.instance.ActivateBlueprint(targetOrderScreenUI.blueprintID);
    }
}