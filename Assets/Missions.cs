using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission System/Mission")]
public class Missions : ScriptableObject
{
    public string missionTitle;
    public string description;
    public string requirementsIntro;
    public List<Requirement> requirements;
    public Sprite resultSprite;
}