using UnityEngine;

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

        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);

        targetOrderScreenUI.SetMission(mission);
        targetOrderScreenUI.gameObject.SetActive(true);
        targetOrderScreenUI.CheckRequirements();

        BlueprintManager.instance.isMissionActive = true;
        BlueprintManager.instance.activeMission = mission;

        DeskUIManager.instance.UpdateEmailButtonVisual();
        DeskUIManager.instance.DisplayRAMSModifiersPanel();

        BlueprintManager.instance.ActivateBlueprint(targetOrderScreenUI.blueprintID);
    }
}