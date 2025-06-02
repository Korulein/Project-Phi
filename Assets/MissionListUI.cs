using System.Collections.Generic;
using UnityEngine;

public class MissionListUI : MonoBehaviour
{
    public Missions[] missions;

    void Start()
    {
        MissionSelectButton[] buttons = GetComponentsInChildren<MissionSelectButton>(true);

        for (int i = 0; i < buttons.Length && i < missions.Length; i++)
        {
            buttons[i].Setup(missions[i]);
            Debug.Log("Setup done for: " + missions[i].missionTitle);
        }
    }
}