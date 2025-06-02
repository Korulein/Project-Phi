using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RequirementTextUI : MonoBehaviour
{
    public TMP_Text requirementsText;

    public List<string> requiredComponents;
    public List<string> playerComponents;

    private string greenHex = "#7ADC5E";
    private string redHex = "#DC5E5E";

    void Start()
    {
        RefreshRequirements();
    }

    public static RequirementTextUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void RefreshRequirements()
    {
        string result = "";

        foreach (string comp in requiredComponents)
        {
            bool has = playerComponents.Contains(comp);
            string color = has ? greenHex : redHex;
            result += $"<color={color}>{comp}</color>\n";
        }

        requirementsText.text = result;
    }
}