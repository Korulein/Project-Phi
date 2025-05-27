using System.Collections.Generic;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public OrderScreenUI orderScreenUI;

    public Missions missionToStart;

    public GameObject blueprintUIObject;

    public void OnStartMissionButtonClicked()
    {
        if (missionToStart == null) return;

        // Stel de mission data in op het UI-scherm
        //orderScreenUI.currentMission = missionToStart;

        // Verwijder eerst bestaande items in de UI (optioneel)
        foreach (Transform child in orderScreenUI.requirementsContainer)
        {
            Destroy(child.gameObject);
        }

        // Voeg de juiste requirement UI elementen toe
        foreach (var requirement in missionToStart.requirements)
        {
            GameObject reqObj = Instantiate(orderScreenUI.requirementUIPrefab, orderScreenUI.requirementsContainer);
            reqObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = requirement;
        }

        blueprintUIObject.SetActive(true);
    }

    public void SetMission(Missions mission)
    {
        missionToStart = mission;
    }

    [CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission")]
    public class Missions : ScriptableObject
    {
        public List<string> requirements;
    }

}