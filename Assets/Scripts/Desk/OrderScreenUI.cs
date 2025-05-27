using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

public class OrderScreenUI : MonoBehaviour
{
    public static OrderScreenUI instance;

    public TMP_Text missionTitleText;
    public TMP_Text descriptionText;
    public TMP_Text requirementsIntroText;
    public Transform requirementsContainer;
    public GameObject requirementUIPrefab;

    public Missions currentMission;

    public List<RequirementUI> requirementUIs = new List<RequirementUI>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        if (currentMission == null) return;

        missionTitleText.text = currentMission.missionTitle;
        descriptionText.text = currentMission.description;
        requirementsIntroText.text = currentMission.requirementsIntro;

        ClearRequirementUI();
        CreateRequirementUI();

        CheckRequirements();
    }

    private void ClearRequirementUI()
    {
        foreach (var reqUI in requirementUIs)
        {
            reqUI.ResetData();
            reqUI.gameObject.SetActive(false);
        }
        requirementUIs.Clear();
    }

    private void CreateRequirementUI()
    {
        if (currentMission.requirements.Count == 0) return;

        foreach (var req in currentMission.requirements)
        {
            GameObject go = GetOrCreateRequirementItem();
            RequirementUI reqUI = go.GetComponent<RequirementUI>();
            reqUI.Setup(req);
            requirementUIs.Add(reqUI);
        }
    }

    private GameObject GetOrCreateRequirementItem()
    {
        foreach (Transform child in requirementsContainer)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                return child.gameObject;
            }
        }

        return Instantiate(requirementUIPrefab, requirementsContainer);
    }


    private void CheckRequirements()
    {
        foreach (var reqUI in requirementUIs)
        {
            bool isMet = reqUI.Evaluate();
            reqUI.SetColor(isMet ? Color.green : Color.red);
        }
    }

    public void NotifyComponentPlaced(string categoryName)
    {
        foreach (Transform child in requirementsContainer)
        {
            RequirementUI ui = child.GetComponent<RequirementUI>();
            if (ui != null && ui.HasData())
            {
                ui.OnComponentPlaced(categoryName);
            }
        }
    }
}