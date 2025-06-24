using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeskUIManager : MonoBehaviour
{
    public static DeskUIManager instance { get; private set; }

    [Header("Tablet Screens")]
    [SerializeField] public List<GameObject> tabletScreens = new List<GameObject>();
    [SerializeField] public List<GameObject> orderScreens = new List<GameObject>();
    [SerializeField] public Transform dragLayer;
    [SerializeField] public RectTransform blueprintGridContainer;
    [SerializeField] public RectTransform inventoryContainer;

    [Header("Product Popup")]
    [SerializeField] GameObject productPopup;
    [SerializeField] private GameObject controlData;
    [SerializeField] GameObject deskBlurLayer;
    [SerializeField] public TextMeshProUGUI RAMSRatings;

    [Header("Supplier Component Information Popup")]
    [SerializeField] public GameObject basicComponentInformationPopup;
    [SerializeField] public Image companyLogo;
    [SerializeField] public Image componentIcon;
    //[SerializeField] public Button closePopupButton;
    [SerializeField] public TextMeshProUGUI componentName;
    [SerializeField] public TextMeshProUGUI componentRating;
    [SerializeField] public TextMeshProUGUI componentSlotType;

    [Header("Order Button")]
    [SerializeField] private Button orderButton; // Sleep in Inspector
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private float blinkInterval = 0.5f;
    private Coroutine blinkCoroutine;

    [Header("Blueprint Visual Elements")]
    [SerializeField] public GameObject RAMSModifierPanel;
    [SerializeField] public TextMeshProUGUI reliabilityModifier;
    [SerializeField] public TextMeshProUGUI availabilityModifier;
    [SerializeField] public TextMeshProUGUI maintainabilityModifier;
    [SerializeField] public TextMeshProUGUI safetyModifier;

    [Header("Blueprint Data Elements")]
    [SerializeField] public GameObject blueprintDataPopup;
    [SerializeField] public GameObject blueprintBlurLayer;
    [SerializeField] public TextMeshProUGUI producedHeat;
    [SerializeField] public TextMeshProUGUI heatThreshold;
    [SerializeField] public TextMeshProUGUI effectiveCooling;
    [SerializeField] public TextMeshProUGUI powerNeeded;
    [SerializeField] public TextMeshProUGUI powerInBlueprint;
    [SerializeField] public TextMeshProUGUI electronicComponentsInBlueprint;
    [SerializeField] public TextMeshProUGUI CPUSlots;

    [Header("Player Input")]
    private bool leftClickConsumed;
    public bool LeftClickDown { get; private set; }
    public bool RightClickDown { get; private set; }
    public bool RKeyDown { get; private set; }
    public bool EscapeDown { get; private set; }
    public Vector3 MousePosition { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (tabletScreens.Count > 0)
        {
            OpenOrder(0);
        }
    }
    private void Update()
    {
        MousePosition = Input.mousePosition;
        LeftClickDown = Input.GetMouseButtonDown(0);
        RightClickDown = Input.GetMouseButtonDown(1);
        EscapeDown = Input.GetKeyDown(KeyCode.Escape);
        RKeyDown = Input.GetKeyDown(KeyCode.R);
    }
    void LateUpdate()
    {
        // Reset consumption state
        leftClickConsumed = false;
    }

    #region Enable / Disable UI
    public void OpenScreen(int screenIndex)
    {
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);

        for (int i = 0; i < tabletScreens.Count; i++)
        {
            if (i != screenIndex)
                tabletScreens[i].SetActive(false);
        }
        tabletScreens[screenIndex].SetActive(true);
    }
    public void OpenOrder(int orderIndex)
    {
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);

        for (int i = 0; i < orderScreens.Count; i++)
        {
            if (i != orderIndex)
                orderScreens[i].SetActive(false);
        }
        orderScreens[orderIndex].SetActive(true);
    }
    public void DisplayProductPopup()
    {
        deskBlurLayer.SetActive(true);
    }
    public void CloseProductPopup()
    {
        deskBlurLayer.SetActive(false);
        ProductManager.instance.PlayTimeline();
        RAMSRatings.text = "";
    }
    public void DisplaySupplierComponentInformationPopup()
    {
        basicComponentInformationPopup.SetActive(true);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
    public void CloseSupplierComponentInformationPopup()
    {
        basicComponentInformationPopup.SetActive(false);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
    public void DisplayBlueprintDataPopup()
    {
        blueprintDataPopup.SetActive(true);
        blueprintBlurLayer.SetActive(true);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
    public void CloseBlueprintDataPopup()
    {
        blueprintDataPopup.SetActive(false);
        blueprintBlurLayer.SetActive(false);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
    public void DisplayControlDataPopup()
    {
        controlData.SetActive(true);
        blueprintBlurLayer.SetActive(true);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
    public void CloseControlDataPopup()
    {
        controlData.SetActive(false);
        blueprintBlurLayer.SetActive(false);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
    public void DisplayRAMSModifiersPanel()
    {
        RAMSModifierPanel.SetActive(true);
    }
    #endregion
    #region UI Update
    public void InitializeSupplierComponentInformation(ComponentData component)
    {
        companyLogo.sprite = component.companyLogo;
        componentIcon.sprite = component.componentSprite;
        componentName.text = component.componentName;
        componentRating.text = component.componentRating.ToString();
        switch (component.slotSize)
        {
            case SlotSize.Small:
                componentSlotType.text = "Small (1x1)";
                break;
            case SlotSize.Medium:
                componentSlotType.text = "Medium (2x2)";
                break;
            case SlotSize.Large:
                componentSlotType.text = "Large (3x3)";
                break;
            case SlotSize.Custom:
                componentSlotType.text = $"Custom: ({component.width}x{component.height})";
                break;
        }
    }
    public void ChangeUIRAMSText(float reliability, float availability, float maintainability, float safety)
    {
        // Reliability
        ProductManager.instance.RoundFloatsToOneDecimal(ref reliability, ref availability, ref maintainability, ref safety);

        if (reliability > 1)
        {
            reliabilityModifier.text = $"+{(100 * reliability - 100):F0}%";
            reliabilityModifier.color = new Color(101f / 255f, 191f / 255f, 53f / 255f, 1f);
        }
        else if (reliability < 1)
        {
            reliabilityModifier.text = $"-{(100 - reliability * 100):F0}%";
            reliabilityModifier.color = new Color(127f / 255f, 2f / 255f, 5f / 255f, 1f);
        }
        else
        {
            reliabilityModifier.text = $"0%";
            reliabilityModifier.color = new Color(238f / 255f, 226f / 255f, 226f / 255f, 1f);
        }

        // Availability
        if (availability > 1)
        {
            availabilityModifier.text = $"+{(100 * availability - 100):F0}%";
            availabilityModifier.color = new Color(101f / 255f, 191f / 255f, 53f / 255f, 1f);
        }
        else if (availability < 1)
        {
            availabilityModifier.text = $"-{(100 - availability * 100):F0}%";
            availabilityModifier.color = new Color(127f / 255f, 2f / 255f, 5f / 255f, 1f);
        }
        else
        {
            availabilityModifier.text = $"0%";
            availabilityModifier.color = new Color(238f / 255f, 226f / 255f, 226f / 255f, 1f);
        }

        // Maintainability
        if (maintainability > 1)
        {
            maintainabilityModifier.text = $"+{(100 * maintainability - 100):F0}%";
            maintainabilityModifier.color = new Color(101f / 255f, 191f / 255f, 53f / 255f, 1f);
        }
        else if (maintainability < 1)
        {
            maintainabilityModifier.text = $"-{(100 - maintainability * 100):F0}%";
            maintainabilityModifier.color = new Color(127f / 255f, 2f / 255f, 5f / 255f, 1f);
        }
        else
        {
            maintainabilityModifier.text = $"0%";
            maintainabilityModifier.color = new Color(238f / 255f, 226f / 255f, 226f / 255f, 1f);
        }

        // Safety
        if (safety > 1)
        {
            safetyModifier.text = $"+{(100 * safety - 100):F0}%";
            safetyModifier.color = new Color(101f / 255f, 191f / 255f, 53f / 255f, 1f);
        }
        else if (safety < 1)
        {
            safetyModifier.text = $"-{(100 - safety * 100):F0}%";
            safetyModifier.color = new Color(127f / 255f, 2f / 255f, 5f / 255f, 1f);
        }
        else
        {
            safetyModifier.text = $"0%";
            safetyModifier.color = new Color(238f / 255f, 226f / 255f, 226f / 255f, 1f);
        }
    }
    public void UpdateBlueprintDataPopup(float heatOutput, float coolingOutput, int electronicComponents, int operationalCPUSlots, float requiredPower, float powerInProduct)
    {
        producedHeat.text = heatOutput.ToString();
        heatThreshold.text = BlueprintManager.instance.currentProduct.maxSustainedHeat.ToString();
        effectiveCooling.text = coolingOutput.ToString();
        powerNeeded.text = requiredPower.ToString();
        powerInBlueprint.text = powerInProduct.ToString();
        electronicComponentsInBlueprint.text = electronicComponents.ToString();
        CPUSlots.text = operationalCPUSlots.ToString();
    }
    public void ClearBlueprintData()
    {
        producedHeat.text = "0";
        heatThreshold.text = BlueprintManager.instance.currentProduct.maxSustainedHeat.ToString();
        effectiveCooling.text = "0";
        powerNeeded.text = "0";
        powerInBlueprint.text = "0";
        electronicComponentsInBlueprint.text = "0";
        CPUSlots.text = "0";
    }
    #endregion

    #region Email Button Visuals
    public void UpdateEmailButtonVisual()
    {
        if (BlueprintManager.instance.isMissionActive == false)
        {
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkEmailButton());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                ResetEmailButtonVisual();
            }
        }
    }
    private IEnumerator BlinkEmailButton()
    {
        bool highlight = false;
        while (true)
        {
            highlight = !highlight;

            ColorBlock colors = orderButton.colors;
            colors.normalColor = highlight ? highlightColor : defaultColor;
            orderButton.colors = colors;

            yield return new WaitForSeconds(blinkInterval);
        }
    }
    private void ResetEmailButtonVisual()
    {
        ColorBlock colors = orderButton.colors;
        colors.normalColor = defaultColor;
        orderButton.colors = colors;
    }
    #endregion

    #region Helper method
    public bool TryConsumeLeftClick()
    {
        if (LeftClickDown && !leftClickConsumed)
        {
            leftClickConsumed = true;
            return true;
        }
        return false;
    }
    #endregion
}
