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

    public int blueprintID;

    public void SetMission(Missions mission)
    {
        currentMission = mission;

        BlueprintManager.instance.activeMission = mission;
        BlueprintManager.instance.activeOrderScreenUI = this;


        missionTitleText.text = currentMission.missionTitle;
        descriptionText.text = currentMission.description;
        requirementsIntroText.text = currentMission.requirementsIntro;

        ClearRequirementUI();
        CreateRequirementUI();
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


    public void CheckRequirements()
    {
        foreach (var reqUI in requirementUIs)
        {
            bool isMet = reqUI.Evaluate();
            if (isMet)
            {
                reqUI.SetColor(Color.green);
            }
            else if (reqUI.isPurchased)
            {
                reqUI.SetColor(Color.yellow);
            }
            else
            {
                reqUI.SetColor(Color.red);
            }
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

    public void NotifyComponentPurchased(string categoryName)
    {
        foreach (Transform child in requirementsContainer)
        {
            RequirementUI ui = child.GetComponent<RequirementUI>();
            if (ui != null && ui.HasData())
            {
                ui.OnComponentPurchased(categoryName);
            }
        }
    }

    public void NotifyComponentRemoved(string categoryName)
    {
        foreach (Transform child in requirementsContainer)
        {
            RequirementUI ui = child.GetComponent<RequirementUI>();
            if (ui != null && ui.HasData())
            {
                ui.OnComponentRemoved(categoryName);
            }
        }
    }

    public void EndMission()
    {
        BlueprintManager.instance.isMissionActive = false;
        BlueprintManager.instance.activeMission = null;
        BlueprintManager.instance.activeOrderScreenUI = null;

        //gameObject.SetActive(false);
    }
}