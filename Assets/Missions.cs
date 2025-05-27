using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Missions
{
    public string missionTitle;
    public string description;
    public string requirementsIntro;
    public List<Requirement> requirements;

    public Sprite resultSprite;
}