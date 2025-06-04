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

    [Header("Pop-ups")]
    [SerializeField] GameObject coffeeMachinePopUp;
    [SerializeField] GameObject blurLayer;
    [SerializeField] public TextMeshProUGUI RAMSRatings;

    [Header("OrderButton")]
    [SerializeField] private Button orderButton; // Sleep in Inspector
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private float blinkInterval = 0.5f;
    private Coroutine blinkCoroutine;

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
    public bool TryConsumeLeftClick()
    {
        if (LeftClickDown && !leftClickConsumed)
        {
            leftClickConsumed = true;
            return true;
        }
        return false;
    }
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
    public void DisplayPopUp()
    {
        blurLayer.SetActive(true);
    }
    public void ClosePopUp()
    {
        blurLayer.SetActive(false);
        RAMSRatings.text = "";
    }
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
    public void DisplayComponentPopUp()
    {
        // Make the entire canvas visible
        infoPopup.SetActive(true);
    }

    public void CloseComponentPopUp()
    {
        // Hide the entire canvas
        infoPopup.SetActive(false);
    }
}
